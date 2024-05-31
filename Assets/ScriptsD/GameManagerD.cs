using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using Random = UnityEngine.Random;

public class GameManagerD : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;

    private float masterVolume;
    private float sfxValue;
    private float voicesValue;
    private float musicValue;

    [SerializeField] private AudioSource sfx;
    [SerializeField] private AudioSource voices;
    [SerializeField] private AudioSource music;
    [SerializeField] private AudioClip win;
    [SerializeField] private AudioClip lose;

    private bool mirror;
    private bool gridShow;
    private bool party;
    private bool autoend;

    //variables con el array de todos los colliders que se cruzará el rayo y el rayo en si
    private RaycastHit[] raycastHits;
    private Ray ray;


    //variable que tendrá la posición del ratón 
    private Vector3 position;

    //variables que tendrán el punto en el que impacta el rayo de raycast en el mapa y la escala de las casillas del mapa
    private Vector3 point;
    private int scale = 2;

    //booleanos para indicar en que fase está el player
    private bool spawnPhase = true;
    private bool movementPhase = false;

    //el mapa, las posiciones de spawn que tiene el mapa y personajes que usará el player.
    private MyGrid map;
    private Vector2[] spawnPos;
    private Character[] characters;


    //variables para seleccionar una casilla o personaje, y variables con el objeto de las casillas
    private MyCell selected;
    private MyCell selectedAux;
    private Character charSelected;
    [SerializeField] private GameObject alliesTile;
    [SerializeField] private GameObject enemiesTile;

    //variable que tendrá todas las posiciones a las que se puede mover un pj
    private Vector2[] movementArray;

    //variable para indicar si una casilla es spawn
    private bool isASpawn = false;

    //booleano para indicar si el personaje se está moviendo.
    private bool moving = false;

    //materiales que ponerle a los personajes antes y después de moverse
    [SerializeField] private Material normalMat;
    [SerializeField] private Material movedMat;


    //variable para determinar si es el turno del enemigo o aliado (int porque puede haber más de un bando enemigo)
    private int turn = 0;

    //variable que guarda el script de la ia del enemigo y el numero de bandos 
    [SerializeField] private int numberOfSides = 2;
    private EnemyAI enemyBrain;

    //booleanos para saber si ya ha activado el turno enemigo y si el juego está pausado o no.
    private bool enemyTurnStarted = false;
    private bool paused = false;

    private UIBattle uiBattle;
    private Battle battle;
    private VisualBattleV2 visualBattle;

    [SerializeField] GameObject[] AlliesPrefabs;

    [SerializeField] private Camera cameraGrid;
    [SerializeField] private Camera cameraBattle;

    [SerializeField] private GameObject battleCanvas;
    [SerializeField] private GameObject gridCanvas;

    private bool fightHasEnded = true;
    private bool ReadyToFight = false;

    [SerializeField] private Volume volume;

    private AudioClip mapMusic;

    [SerializeField] private AudioClip partyMusic;

    private bool winned = false;

    void Start()
    {
        cameraBattle.enabled = false;
        battleCanvas.SetActive(false);
        //encuentra el mapa y recoje las posiciones de spawn
        map = FindObjectOfType<MyGrid>();
        enemyBrain = gameObject.GetComponent<EnemyAI>();
        spawnPos = map.GetSpawn();
        uiManager.EndTurnOff();
        uiManager.HideOptions();
        uiBattle = FindObjectOfType<UIBattle>();
        battle = FindObjectOfType<Battle>();
        characters = new Character[AlliesPrefabs.Length];
        visualBattle = FindObjectOfType<VisualBattleV2>();
        
        volume.weight = 0;
        //posiciona los personajes en posiciones de spawn
        
        for (int i = 0; i < AlliesPrefabs.Length; i++)
        {
            GameObject aux;
            String auxName;

            Instantiate(AlliesPrefabs[i]);

            auxName = AlliesPrefabs[i].name + "(Clone)";
           
            aux = GameObject.Find(auxName);
            
            characters[i] = aux.GetComponent<Character>();

            characters[i].SetMap(map);

            characters[i].SetPosition((int)spawnPos[i].x, (int)spawnPos[i].y);
            map.GetCell((int)spawnPos[i].x, (int)spawnPos[i].y).SetCharacter(characters[i]);
        }

        
        // Pone la musica
        ChangeMusic(uiManager.MusicVolume());
        ChangeSFX(uiManager.SfxVolume());
        ChangeVoices(uiManager.VoicesVolume());
        ChangeMaster(uiManager.MasterVolume());
        mapMusic = map.GetMusic();
        music.clip = mapMusic;
        music.Play();

        //spawnea las casillas azules para indicar donde pueden spawnear los personajes
        SpawnTiles(spawnPos);



    }

    void Update()
    {
        if (YouLost())
        {
            Debug.Log("you lose!");
        }
        else if (YouWin())
        {
            
            if (!winned)
            {
                Debug.Log("you win!");
                music.clip = win;
                music.Play();
                winned = true;
                uiManager.YouWin();
            }
            
        }

        else if (autoend)
        {
            if (AllAlliesHaveActed())
            {
                EndAllyTurn();
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused)
            {
                uiManager.ShowOptions();
                paused = true;
                Time.timeScale = 0;
                uiManager.HideTurn();
                uiManager.HideEndOfSpawn();
                battleCanvas.SetActive(false);
            }
            else
            {
                uiManager.HideOptions();
                paused = false;
                Time.timeScale = 1.0f;
                uiManager.ShowTurn();
                if (spawnPhase) uiManager.ShowEndOfSpawn();
                if(!fightHasEnded) battleCanvas.SetActive(true);
            }

        }
        if(turn == 0)
        {
            //FASE DE SPAWN
            if (spawnPhase && !paused)
            {
                //si hace click izquierdo
                if (Input.GetMouseButtonUp(0))
                {
                    //se dispara un rayo en esa posición y dirección
                    ShootRay();

                    //si el rayo ha dado a algo
                    if (raycastHits.Length != 0)
                    {
                        //se pone a point la casilla de donde ha dado el rayo
                        GetPosition(raycastHits[0]);

                        //revisa si esa casilla es un spawn
                        CheckSpawn(new Vector2(point.x, point.z));

                        //si no se ha seleccionado una casilla ya, y point es un spawn se selecciona la casilla
                        if (selected == null && isASpawn)
                        {
                            selected = map.GetCell((int)point.x, (int)point.z);
                        }
                        //Si ya se tenía algo seleccionado y es un spawn se intercambia lo que estaba en esas dos casillas si había algo, y se desseleccionan ambas casillas
                        else if (isASpawn)
                        {
                            selectedAux = map.GetCell((int)point.x, (int)point.z);
                            if (selected.GetCharacter() != null || selectedAux.GetCharacter() != null && selected != selectedAux)
                            {
                                ExchangePos(selected, selectedAux);
                            }
                            selected = null;
                        }
                        //se vuelve a poner isASpawn a false
                        isASpawn = false;
                    }
                }
                //Si haces click derecho se desseleccionas lo que ya tenías seleccionado
                if (Input.GetMouseButtonDown(1))
                {
                    selected = null;
                }

            }

            //FASE DE MOVIMIENTO
            else if (movementPhase && !paused)
            {
                if (Input.GetKeyDown(KeyCode.Z) && selected != null && selected.GetCharacter() != null)
                {
                    ReturnPos();


                }

                //Si no hay un pj moviendose y haces click izq.
                if (Input.GetMouseButtonUp(0) && !moving)
                {
                    //se dispara el rayo y se mira donde ha caído
                    ShootRay();
                    if (raycastHits.Length != 0)
                    {
                        //si ya habia un pj seleccionado se borran sus casillas de movimiento
                        if (selected != null) DeleteTiles();
                        GetPosition(raycastHits[0]);

                        //se selecciona la cell correspondiente
                        selected = map.GetCell((int)point.x, (int)point.z);
                        if (!selected.GetWalkable())
                        {
                            uiManager.ChangeTileInfo(69);
                        }
                        else
                        {
                            uiManager.ChangeTileInfo(selected.GetDifficulty());
                        }
                        

                        //si hay un pj en la casilla seleccionada y no se ha movido en ese turno se spawnean las casillas de movimiento 
                        if (selected.GetCharacter() != null)
                        {
                            if (!selected.GetCharacter().GetHasMoved())
                            {
                                movementArray = map.GetMovement((int)point.x, (int)point.z, selected.GetCharacter().GetMovement()).ToArray();
                                SpawnTiles(movementArray);
                            }
                            else selected = null;
                        }

                        //si no hay un pj enseña información del terreno
                        else
                        {
                            // 
                            // Debug.Log(selected.GetWalkable() + ", " + selected.GetDifficulty());
                            //TODO: HAY QUE CAMBIAR ESTO POR UN PANEL QUE DE INFO DEL TERRENO 
                        }
                    }
                }

                //si haces click derecho y nadie se está moviendo
                else if (Input.GetMouseButtonUp(1) && !moving)
                {
                    //se dispara el rayo y se mira donde ha caído
                    ShootRay();
                    if (raycastHits.Length != 0)
                    {
                        GetPosition(raycastHits[0]);

                        //se selecciona la casilla donde cayó
                        selectedAux = map.GetCell((int)point.x, (int)point.z);

                        //si ya tenía una casilla con personaje seleccionada 
                        if (selected != null && selected.GetCharacter() != null)
                        {
                            //si en la nueva casilla seleccionada hay un enemigo se mueve cerca suyo y ataca
                            if (selectedAux.GetCharacter() != null && selectedAux.GetCharacter().GetSide() > 0)
                            {
                                Character auxCharA = selected.GetCharacter();
                                
                                MoveChar(selected, map.GetCell(NearbyTile(selected.GetPosition(), selectedAux.GetPosition())));
                                // Debug.Log("attack");
                                //AQUI VA LA LÓGICA DEL ATAQUE
                                Fight(auxCharA, selectedAux.GetCharacter());
                                fightHasEnded = false;
                                
                                DeleteTiles();
                            }

                            //si no hay un personaje y se puede mover ahí se mueve
                            else if (selectedAux.GetCharacter() == null && CheckMove())
                            {
                                MoveChar(selected, selectedAux);
                                selected = selectedAux = null;
                                DeleteTiles();
                            }
                        }
                    }
                }
            }
        }
        
        else if(turn != 0)
        {
            if (!enemyTurnStarted)
            {
                enemyBrain.EnemyTurn();
                enemyTurnStarted = true;
            }
            else if (!enemyBrain.IsEnemyTurn())
            {
                EndEnemyTurn();
            }
            
        }


    }

    public void Fight(Character battler1, Character battler2)
    {
        if (battler1 == null || battler2 == null)
        {
            if (battler1 == null && battler2 != null)
            {
                Debug.LogError("Atacker");
                return;
            }
            else if (battler2 == null && battler1 != null)
            {
                Debug.LogError("Defender");
                return;
            }
            else
            {
                Debug.LogError("Both");
                return;
            }
   
        }
        

        // Debug.Log(battler1.name + battler2.name);
        battle.updateUIBattle(battler1, battler2);
        
        //! EFECTO DROGA
        
        gridCanvas.SetActive(false);
        cameraBattle.enabled = true;
        cameraGrid.enabled = false;
        battleCanvas.SetActive(true);

        visualBattle.SpawnCharacters(battler1, battler2);
        


    }
    //función de volver a la posición anterior ALFA BETA GAMMA 
    private void ReturnPos()
    {
        MyCell aux = map.GetCell(selected.GetCharacter().GetPosition());
        Character auxChar = selected.GetCharacter();
        if (map.GetCell(auxChar.GetPreviousPosition()).GetCharacter() == null)
        {
            auxChar.SetPosition(auxChar.GetPreviousPosition());

            aux.SetCharacter(null);
            map.GetCell(auxChar.GetPosition()).SetCharacter(auxChar);
        }


        DeleteTiles();
    }

    //Función para ver si el punto seleccionado está en el array de movimiento
    private bool CheckMove()
    {
        bool aux = false;
        Vector2 secondpoint = new Vector2(point.x, point.z);
        for (int i = 0; i < movementArray.Length && !aux; i++)
        {
            if (secondpoint == movementArray[i]) { aux = true; }
        }
        return aux;
    }

    //Función para ver si el target seleccionado está en el array de movimiento
    private bool CheckMove(Vector2 target, bool enemy = false)
    {
        bool aux = false;
        for (int i = 0; i < movementArray.Length && !aux; i++)
        {
            if (target == movementArray[i]) { aux = true; }
        }
        return aux;
    }


    //Funcion para encontrar la casilla más cercana adyacente a un enemigo
    private Vector2 NearbyTile(Vector2 attackerPos, Vector2 defenderPos)
    {

        //se hace un array con las cuatro posiciones adyacentes al que recibe el ataque
        Vector2[] aux = new Vector2[4];
        int x = 0;
        int score = -1;
        int scoreaux;
        aux[0] = new Vector2(defenderPos.x + 1, defenderPos.y);
        aux[1] = new Vector2(defenderPos.x - 1, defenderPos.y);
        aux[2] = new Vector2(defenderPos.x, defenderPos.y + 1);
        aux[3] = new Vector2(defenderPos.x, defenderPos.y - 1);
        //revisa que cells de las que se puede mover le queda más cerca al atacante 
        for (int i = 0; i < aux.Length; i++)
        {
            if (turn != 0 || CheckMove(aux[i]))
            {

                scoreaux = ((Mathf.Abs((int)attackerPos.x - (int)aux[i].x)) + (Mathf.Abs((int)attackerPos.y - (int)aux[i].y)));
                if (map.GetCell(aux[i]).GetCharacter() != null) scoreaux += 100000;
                if (scoreaux < score || score == -1)
                {
                    score = scoreaux;
                    x = i;
                }
            }
        }


        return aux[x];
    }

    private Vector2 NearbyTileEnemy(Vector2 attackerPos, Vector2 defenderPos, ref List<(int, int)> moveList)
    {

        //se hace un array con las cuatro posiciones adyacentes al que recibe el ataque
        Vector2[] aux = new Vector2[4];
        int x = 0;
        int score = -1;
        int scoreaux;
        aux[0] = new Vector2(defenderPos.x + 1, defenderPos.y);
        aux[1] = new Vector2(defenderPos.x - 1, defenderPos.y);
        aux[2] = new Vector2(defenderPos.x, defenderPos.y + 1);
        aux[3] = new Vector2(defenderPos.x, defenderPos.y - 1);
        //revisa que cells de las que se puede mover le queda más cerca al atacante 
        for (int i = 0; i < aux.Length; i++)
        {
            if (map.CheckBounds(aux[i]))
            {

                scoreaux = ((Mathf.Abs((int)attackerPos.x - (int)aux[i].x)) + (Mathf.Abs((int)attackerPos.y - (int)aux[i].y)));
                if (map.GetCell(aux[i]).GetCharacter() != null) scoreaux += 100000;
                if (!moveList.Contains(((int)aux[i].x, (int)aux[i].y))) scoreaux += 100000;
                if (scoreaux < score || score == -1)
                {
                    score = scoreaux;
                    x = i;
                }
            }
        }


        return aux[x];
    }

    public Vector2 AttackTile(Vector2 attackerPos, Vector2 defenderPos, ref List<(int, int)>  moveList)
    {
        return NearbyTileEnemy(attackerPos, defenderPos, ref moveList);
    }

    //función para enseñar las casillas de movimiento o de spawn, dependiendo de la fase en la que estés
    private void SpawnTiles(Vector2[] pos)
    {
        for (int i = 0; i < pos.Length; i++)
        {
            if (spawnPhase) Instantiate(alliesTile, new Vector3(pos[i].x * scale + 1, 0.1f, -pos[i].y * scale - 1), Quaternion.identity);
            else
            {
                if (map.GetCell((int)pos[i].x, (int)pos[i].y).GetCharacter() == null)
                {
                    Instantiate(alliesTile, new Vector3(pos[i].x * scale + 1, 0.1f, -pos[i].y * scale - 1), Quaternion.identity);
                }
                else if (map.GetCell((int)pos[i].x, (int)pos[i].y).GetCharacter().GetSide() > 0)
                {
                    Instantiate(enemiesTile, new Vector3(pos[i].x * scale + 1, 0.1f, -pos[i].y * scale - 1), Quaternion.identity);
                }
            }

        }
    }


    //Función que elimina todas las casillas de movimiento o spawn
    private void DeleteTiles()
    {
        GameObject[] destroying = GameObject.FindGameObjectsWithTag("Tile_mark");


        for (int i = 0; i < destroying.Length; i++)
        {
            Destroy(destroying[i]);
        }
    }

    //Funcion que termina la fase de spawn y empieza la de movimiento
    public void EndSpawnPhase()
    {
        spawnPhase = false;

        DeleteTiles();

        movementPhase = true;
    }

    //función que dispara un rayo desde la camara en direccioón de a donde apunta el ratón y guarda donde choca
    private void ShootRay()
    {
        position = Input.mousePosition;
        position.z = Camera.main.nearClipPlane;
        ray = Camera.main.ScreenPointToRay(position);
        raycastHits = Physics.RaycastAll(ray);
    }

    //revisa si la posición que le pasa está en spawnPos
    private void CheckSpawn(Vector2 aux)
    {
        for (int i = 0; i < spawnPos.Length && !isASpawn; i++)
        {

            if (aux == spawnPos[i]) isASpawn = true;
        }
    }

    //Pone en point la casilla en la que ha acertado el rayo
    private void GetPosition(RaycastHit hit)
    {
        point = hit.point;
        point.x = ((int)point.x);
        point.z = ((int)point.z);
        point /= 2;
        point.x = ((int)point.x);
        point.z = ((int)point.z);
        point.y = 0;
        point.z = Mathf.Abs(point.z);
    }

    //intercambia posiciones entre ambos personajes que hay en las casillas, si en una casilla.
    private void ExchangePos(MyCell cell1, MyCell cell2)
    {
        charSelected = cell1.GetCharacter();
        Character charSelectedAux = cell2.GetCharacter();
        cell2.SetCharacter(charSelected);
        cell1.SetCharacter(charSelectedAux);

        if (charSelectedAux != null) charSelectedAux.SetPosition(cell1);
        if (charSelected != null)
        {
            charSelected.SetPosition(cell2);
        }

    }


    //Función para mover el personaje 
    private void MoveChar(MyCell cell1, MyCell cell2, bool side = true)
    {
        (int, int)[] aux;
        if (side)
        {
            aux = map.FindPath(cell1.GetPosition(), cell2.GetPosition());
        }
        else
        {
            aux = map.FindPathEnemy(cell1.GetPosition(), cell2.GetPosition());
        }
            StartCoroutine(MovingChar(aux));
        return;

    }

    public void MoveEnemy(MyCell cell1, MyCell cell2)
    {
        MoveChar(cell1, cell2, false);
    }

    //Corutina para mover el personaje casilla por casilla
    private IEnumerator MovingChar((int, int)[] path)
    {
        Character selectedChar = map.GetCell(path[0].Item1, path[0].Item2).GetCharacter();
        selectedChar.SetPreviousPosition(path[0].Item1, path[0].Item2);
        map.GetCell(path[0].Item1, path[0].Item2).SetCharacter(null);
        moving = true;
        for (int i = 1; i < path.Length; i++)
        {
            selectedChar.SetPosition(path[i].Item1, path[i].Item2);
            yield return new WaitForSeconds(0.2f);
        }
        map.GetCell(path[path.Length - 1].Item1, path[path.Length - 1].Item2).SetCharacter(selectedChar);

        yield return new WaitForSeconds(0.2f);
        
        //CAMBIAR DE COLOR EL PJ PARA DENOTAR QUE SE HA MOVIDO
        if (selectedChar.GetSide() == 0) selectedChar.GameObject().GetComponentsInChildren<Renderer>()[0].material = movedMat;
        if(turn != 0)
        {
            enemyBrain.DoneMoving();
        }
        moving = false;
        selectedChar.SetHasMoved(true);
    }

    private IEnumerator WaitToFight(Character attacker, Character defender)
    {
        while (!ReadyToFight)
        {
            yield return new WaitForSeconds(0.5f);
        }
        ReadyToFight=false;
        Fight(attacker, defender);
        fightHasEnded = false;
        while (!fightHasEnded)
        {
            
            yield return new WaitForSeconds(1f);
        }
        
        enemyBrain.DoneFighting();
        fightHasEnded = true;
    }

    public void WaitingForAFight(Character att, Character def)
    {
        StartCoroutine(WaitToFight(att, def));
    }

    public void IsGoingToFight()
    {
        ReadyToFight = true;
    }

    //Función para terminar el turno aliado
    private void EndAllyTurn()
    {
        if (!moving)
        {
            turn++;
            DeleteTiles();
            uiManager.EndTurnOff();
        }
        
    }


    //Función para terminar el turno enemigo, es igual que el aliado pero activa el botón de terminar turno y pone el enemyturnstarted a false, también si el proximo turno es el del jugador reinicia el movimiento de los personajes
    private void EndEnemyTurn()
    {
        turn++;
        if (turn >= numberOfSides)
        {
            turn = 0;
            for (int i = 0; i < characters.Length; i++)
            {
                characters[i].SetHasMoved(false);
                characters[i].GetComponentInChildren<Renderer>().material = normalMat;
            }
            uiManager.EndTurnOn();
        }
        enemyTurnStarted = false;
    }

    //Funciones para cambiar el valor de un audiosource con cada uno de los sliders de las opciones
    public void ChangeMaster(float x)
    {
        masterVolume = x;
        // Debug.Log(musicValue + ", " + masterVolume);
        ChangeSFX(sfxValue);
        ChangeMusic(musicValue);
        ChangeVoices(voicesValue);
    }

    public void ChangeSFX(float x)
    {
        sfxValue = x;
        sfx.volume = (sfxValue * masterVolume);
    }

    public void ChangeMusic(float x)
    {
        musicValue = x;
        music.volume = (musicValue*masterVolume);
    }

    public void ChangeVoices(float x)
    {
        voicesValue = x;
        voices.volume = (voicesValue * masterVolume);
    }

    //funciones para activar varias opciones
    public void ShowGrid(bool x)
    {
        gridShow = x;
    }

    public void PartyMode(bool x)
    {
        party = x;
        if (x)
        {
            volume.weight = 1;
            music.clip = partyMusic;
        }
        else
        {
            volume.weight = 0;
            music.clip = mapMusic;
        }
    }

    public void MirrorStats(bool x)
    {
        mirror = x;
    }

    public void AutoEndOnOff(bool x)
    {
        autoend = x;
    }

    //funciones para terminar la fase de spawn y el turno aliado respectivamente
    public void EndSpawn()
    {
        EndSpawnPhase();
        uiManager.HideEndOfSpawn();
        uiManager.EndTurnOn();
    }

    public void EndTurn()
    {
        EndAllyTurn();
        
    }


    //función que devuelve que número ocupa el personaje seleccionado en el array de aliados
    public int GetNumb(Character person) 
    { 
        for(int i = 0; i<characters.Length; i++)
        {
            if (characters[i] == person) return i;
        }
        return -1;
    }

    //Funcion que devuelve todos los personajes aliados
    public Character[] GetCharacters()
    {
        return characters;
    }

    //Funcion que oculta el panel de opciones y quita la pausa.
    public void Hideoptions()
    {
        uiManager.HideOptions();
        paused = false;
        Time.timeScale = 1.0f;
        uiManager.ShowTurn();
        if (spawnPhase) uiManager.ShowEndOfSpawn();
        if (!fightHasEnded) battleCanvas.SetActive(true);
    }

    public void FightHasEnded(Character dead)
    {
        cameraGrid.enabled = true;
        cameraBattle.enabled = false;
        fightHasEnded = true;
        battleCanvas.SetActive(false);
        gridCanvas.SetActive(true);

        if (dead != null)
        {
            MyCell aux = map.GetCell(dead.GetPosition());
            aux.SetCharacter(null);
            if (dead.side == 0)
            {
                for (int i = 0; i < characters.Length; i++)
                {
                    if (characters[i] == dead)
                    {
                        Destroy(characters[i].GameObject());
                        characters[i] = null;
                    }
                }
            }
            else
            {

                map.DeleteCharacter(dead);
            }
        }
        
    }

    
    private bool AllAlliesHaveActed()
    {
        for(int i=0; i<characters.Length; i++)
        {
            if (characters[i] != null && !characters[i].GetHasMoved())
            {
                return false;
            }
        }
        return true;
    }

    private bool YouLost()
    {
        for (int i= 0; i<characters.Length; i++)
        {
            if (characters[i] != null)
            {
                return false;
            }
        }
        return true;
    }

    private bool YouWin()
    {
        return map.YouWin();
    }

    public void playSFX(AudioClip soundEffect)
    {
        
        sfx.clip = soundEffect;
        sfx.Play();
        
        
    }

}

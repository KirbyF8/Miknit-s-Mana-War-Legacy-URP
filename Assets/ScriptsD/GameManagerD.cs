using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManagerD : MonoBehaviour
{
    private UIManager uiManager;

    private float masterVolume;
    private float sfxValue;
    private float voicesValue;
    private float musicValue;

    [SerializeField] private AudioSource sfx;
    [SerializeField] private AudioSource voices;
    [SerializeField] private AudioSource music;

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
    [SerializeField] private CharacterD[] characters;


    //variables para seleccionar una casilla o personaje, y variables con el objeto de las casillas
    private MyCell selected;
    private MyCell selectedAux;
    private CharacterD charSelected;
    [SerializeField] private GameObject alliesTile;
    [SerializeField] private GameObject enemiesTile;

    //variable que tendrá todas las posiciones a las que se puede mover un pj
    private Vector2[] movementArray;

    //variable para indicar si una casilla es spawn
    private bool isASpawn = false;

    //booleano para indicar si el personaje se está moviendo.
    private bool moving = false;


    [SerializeField] private Material normalMat;
    [SerializeField] private Material movedMat;

    private int turn = 0;


    void Start()
    {

        //encuentra el mapa y recoje las posiciones de spawn
        map = FindObjectOfType<MyGrid>();
        uiManager = FindObjectOfType<UIManager>();
        spawnPos = map.GetSpawn();
        uiManager.EndTurnOff();
        //posiciona los personajes en posiciones de spawn
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].SetPosition((int)spawnPos[i].x, (int)spawnPos[i].y);
            map.GetCell((int)spawnPos[i].x, (int)spawnPos[i].y).SetCharacter(characters[i]);
        }

        //spawnea las casillas azules para indicar donde pueden spawnear los personajes

        SpawnTiles(spawnPos);

    }

    void Update()
    {
        if(turn == 0)
        {
            //FASE DE SPAWN
            if (spawnPhase)
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
            else if (movementPhase)
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

                        //si hay un pj en la casilla seleccionada se spawnean las casillas de movimiento 
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
                            Debug.Log(selected.GetWalkable() + ", " + selected.GetDifficulty());
                            //HAY QUE CAMBIAR ESTO POR UN PANEL QUE DE INFO DEL TERRENO 
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
                            if (selectedAux.GetCharacter() != null && selectedAux.GetCharacter().GetSide() != 0)
                            {
                                MoveChar(selected, map.GetCell(NearbyTile(selected.GetPosition(), selectedAux.GetPosition())));
                                Debug.Log("attack");
                                //AQUI VA LA LÓGICA DEL ATAQUE
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
        
        else if(turn == 1)
        {
            //TURNO ENEMIGO 1, REPETIR POR BANNDOS POSIBLES
            EndEnemyTurn();
        }


    }


    //función de volver a la posición anterior ALFA BETA GAMMA 
    private void ReturnPos()
    {
        MyCell aux = map.GetCell(selected.GetCharacter().GetPosition());
        CharacterD auxChar = selected.GetCharacter();
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
    private bool CheckMove(Vector2 target)
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
            if (CheckMove(aux[i]))
            {

                scoreaux = ((Mathf.Abs((int)attackerPos.x - (int)aux[i].x)) + (Mathf.Abs((int)attackerPos.y - (int)aux[i].y)));

                if (scoreaux < score || score == -1)
                {
                    score = scoreaux;
                    x = i;
                }
            }
        }


        return aux[x];
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
        CharacterD charSelectedAux = cell2.GetCharacter();
        cell2.SetCharacter(charSelected);
        cell1.SetCharacter(charSelectedAux);

        if (charSelectedAux != null) charSelectedAux.SetPosition(cell1);
        if (charSelected != null)
        {
            charSelected.SetPosition(cell2);
        }

    }


    //Función para mover el personaje 
    private void MoveChar(MyCell cell1, MyCell cell2)
    {
        (int, int)[] aux = map.FindPath(cell1.GetPosition(), cell2.GetPosition());
        StartCoroutine(MovingChar(aux));
        return;

    }

    //Corutina para mover el personaje casilla por casilla
    private IEnumerator MovingChar((int, int)[] path)
    {
        CharacterD selectedChar = map.GetCell(path[0].Item1, path[0].Item2).GetCharacter();
        selectedChar.SetPreviousPosition(path[0].Item1, path[0].Item2);
        map.GetCell(path[0].Item1, path[0].Item2).SetCharacter(null);
        moving = true;
        for (int i = 1; i < path.Length; i++)
        {
            selectedChar.SetPosition(path[i].Item1, path[i].Item2);
            yield return new WaitForSeconds(0.2f);
        }
        map.GetCell(path[path.Length - 1].Item1, path[path.Length - 1].Item2).SetCharacter(selectedChar);
        moving = false;
        selectedChar.SetHasMoved(true);
        //CAMBIAR DE COLOR EL PJ PARA DENOTAR QUE SE HA MOVIDO
        selectedChar.GameObject().GetComponentsInChildren<Renderer>()[0].material = movedMat;
    }

    //Función para terminar el turno aliado
    private void EndAllyTurn()
    {
        turn++;
        DeleteTiles();
    }

    private void EndEnemyTurn()
    {
        turn++;
        if (turn >= 2)
        {
            turn = 0;
            for (int i = 0; i < characters.Length; i++)
            {
                characters[i].SetHasMoved(false);
                characters[i].GetComponentInChildren<Renderer>().material = normalMat;
            }
            uiManager.EndTurnOn();
        }
    }

    public void ChangeMaster(float x)
    {
        masterVolume = x;

        ChangeSFX(sfxValue);
        ChangeMusic(musicValue);
        ChangeVoices(voicesValue);
    }

    public void ChangeSFX(float x)
    {
        sfxValue = x;
        sfx.volume = (sfxValue + masterVolume) / 2;
    }

    public void ChangeMusic(float x)
    {
        musicValue = x;
        music.volume = (musicValue+masterVolume)/2;
    }

    public void ChangeVoices(float x)
    {
        voicesValue = x;
        voices.volume = (voicesValue + masterVolume) / 2;
    }

    public void ShowGrid(bool x)
    {
        gridShow = x;
    }

    public void PartyMode(bool x)
    {
        party = x;
    }

    public void MirrorStats(bool x)
    {
        mirror = x;
    }

    public void AutoEndOnOff(bool x)
    {
        autoend = x;
    }

    public void EndSpawn()
    {
        EndSpawnPhase();
        uiManager.HideEndOfSpawn();
        uiManager.EndTurnOn();
    }

    public void EndTurn()
    {
        EndAllyTurn();
        uiManager.EndTurnOff();
    }

}

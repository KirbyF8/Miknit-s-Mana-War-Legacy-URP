using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    //variable para seleccionar el enemigo que va a actuar.
    private int numberOfEnemy;

    //cariables para saber si es el turno del enemigo y si el enemigo anterior ya ha terminado el movimiento.
    private bool enemyTurn = false;
    private bool canDoTurn = true;

    //variables para los scripts de MyGrid y Gamemanager
    private MyGrid map;
    private GameManagerD gameManager;

    //array con todos los enemigos (en el momento de iniciar el turno)
    private Character[] enemies;

    //variable para saber que acción va a hacer el enemigo (la acción se decide en think), y variable auxiliar para guardar una posición a las que moverse.
    private Vector2Int actionToPerform;
    private Vector2Int movement;

    //listas de los enemigos a los que atacar y de la lista a la que se puede mover un personaje 
    private List<Character> alliesToAttack;
    private List<(int, int)> moveList;

    //variable para guardar puntuaciones de las acciones
    private int score;

    //variable para guardar a todos los aliados
    private Character[] Allies;

    //variable para guardar la posición del aliado que va a atacar
    private Vector2Int targetPos;

    //variable para saber si ya se ha actualizado la lista de enemigos.
    private bool updated = false;

    private Battle battle;

    // Start is called before the first frame update
    void Start()
    {
        //Declaración de algunas variables
        map = FindObjectOfType<MyGrid>();
        alliesToAttack = new List<Character>();
        gameManager = FindObjectOfType<GameManagerD>();
        Allies = gameManager.GetCharacters();
        numberOfEnemy = -1;
        battle = FindObjectOfType<Battle>();
    }

    private void Update()
    {
        if(enemyTurn)
        {
            if (!updated)
            {
                enemies = map.GetEnemies();
                updated = true;
            }
            if (canDoTurn)
            {
                // Debug.Log("Halo");
                canDoTurn = false;
                numberOfEnemy++;
                if (numberOfEnemy < enemies.Length)
                {
                    TrueEnemyTurn();
                }
                else
                {
                    enemyTurn = false;
                    updated = false;
                    canDoTurn=true;
                    numberOfEnemy = -1;
                }

                
            }
            
        }
    }

    public void EnemyTurn()
    {
        enemyTurn = true;
    }

    private void TrueEnemyTurn()
    {

        Think(enemies[numberOfEnemy]);
        Act(enemies[numberOfEnemy]);

    }

    private void Act(Character person)
    {
        if (actionToPerform.x == 1)
        {
            movement = new Vector2Int(-1000,-1000);
            // Debug.Log(actionToPerform.y);
            Character aux = Allies[actionToPerform.y];
            targetPos = new Vector2Int((int)aux.GetPosition().x, (int)aux.GetPosition().y);
            gameManager.MoveEnemy(map.GetCell((int)person.GetPosition().x, (int)person.GetPosition().y), map.GetCell(gameManager.AttackTile(person.GetPosition(), targetPos)));
            Debug.Log("ENEMY ATTACK");
            //Lógica de ataque
        }
        else if (actionToPerform.x == 2)
        {
            score = 10000;
            targetPos = new Vector2Int(0,0);
            List<Vector2Int> alreadyChecked = new List<Vector2Int>();
            FindClosestPos((int)person.GetPosition().x, (int)person.GetPosition().y, person.GetMovement(), ref alreadyChecked);
            gameManager.MoveEnemy(map.GetCell((int)person.GetPosition().x, (int)person.GetPosition().y), map.GetCell(targetPos.x, targetPos.y));

           
        }
        else return;
    }

    private void Think(Character person)
    {
        moveList = new List<(int, int)>();
        actionToPerform = new Vector2Int(0, 0);
        if (!person.GetActive())
        {
            return;
        }
        else if (AlliesInRange(person))
        {
            
            int max = -1000;
            Character Selected = null;
            foreach(Character target in alliesToAttack)
            {
                score = Calculate(person, target);
                //Debug.Log(person.GetPosition());
                // Debug.Log(CanGetToEnemy(target));
                if (score > max && CanGetToEnemy(target))
                {
                    // Debug.Log(CanGetToEnemy(target));
                    max = score;
                    Selected = target;
                }
            }
            if (Selected == null)
            {
                closestEnemy(person);
                actionToPerform.x = 2;
            }
            else
            {
                actionToPerform.x = 1;
                actionToPerform.y = gameManager.GetNumb(Selected);
                return;
            }
            
        }
        else
        {
            closestEnemy(person);
            actionToPerform.x = 2;
        }
    }

    private bool CanGetToEnemy( Character target)
    {
        (int,int) aux = ((int)target.GetPosition().x,(int)target.GetPosition().y);
        /*
         Debug.Log(moveList.Count);

        for(int i=0; i < moveList.Count; i++)
        {
            Debug.Log(moveList[i]);
        }*/
        
        aux.Item1++;
        if(aux.Item1 < map.Width() && moveList.Contains(aux)){

            if (map.GetCell(aux.Item1, aux.Item2).GetCharacter() == null || map.GetCell(aux.Item1, aux.Item2).GetCharacter() == enemies[numberOfEnemy]) return true;
        }
        
        aux.Item1 -= 2;
        if (aux.Item1 >=0 && moveList.Contains(aux))
        {
            if (map.GetCell(aux.Item1, aux.Item2).GetCharacter() == null || map.GetCell(aux.Item1, aux.Item2).GetCharacter() == enemies[numberOfEnemy]) return true;
        }

        aux.Item1++;
        aux.Item2++;
        if (aux.Item2 < map.Height() && moveList.Contains(aux))
        {
            if (map.GetCell(aux.Item1, aux.Item2).GetCharacter() == null || map.GetCell(aux.Item1, aux.Item2).GetCharacter() == enemies[numberOfEnemy]) return true;
        }

        aux.Item2 -= 2;
        if (aux.Item2 >= 0 && moveList.Contains(aux))
        {
            if (map.GetCell(aux.Item1, aux.Item2).GetCharacter() == null || map.GetCell(aux.Item1, aux.Item2).GetCharacter() == enemies[numberOfEnemy]) return true;
        }
        return false;
    }

    private void closestEnemy(Character person)
    {
        float posx = person.GetPosition().x;
        float posy = person.GetPosition().y;
        
        int min = 1000;
        Vector2 dir = new Vector2 (0,0);
        foreach(Character target in Allies)
        {
            score =(int) MathF.Abs(posx - target.GetPosition().x) + (int) MathF.Abs(posy-person.GetPosition().y);
            if (score < min)
            {
                min = score;
                dir = target.GetPosition();
            }
        }
        movement = new Vector2Int((int)dir.x, (int)dir.y);
        return;
    }

    private int Calculate(Character person, Character target)
    {
        //AQUI HAY QUE PONER EL CALCULO DE DAÑO QUE HACE Y RECIBE
        //BÁSICAMENTE EN LA VARIABLE AUXILIAR SUMAR 100 SI LO MATA, SI NO LO MATA EL PORCENTAJE DE VIDA QUE LE QUITARÍA, DIVIDIR ENTRE DOS Y RESTAR LA PROB. DE ESQUIVAR (SI ES UN 100% SE PUEDE HACER ALGO)
        //RESTAR LA MITAD DEL DAÑO QUE RECIBIRÍA EL PERSONAJE Y FINALMENTE UN RANDOM DEL 1 AL 20

        
        int aux = 0;

        aux = battle.CalculateDMG(person, target);

        aux += Random.Range(1, 21);

        return aux;
    }

    private bool AlliesInRange(Character person)
    {
        moveList = new List<(int, int)> ();
        alliesToAttack.Clear();
        Vector2 pos = person.GetPosition();
        List<Vector2Int> alreadyChecked = new List<Vector2Int>();
        GetEnemiesRange((int)pos.x, (int)pos.y, person.GetMovement() + person.GetRange(),ref alreadyChecked);
        //Debug.Log(alliesToAttack.Count);
        if (alliesToAttack.Count > 0) return true; else return false;
    }

    private void GetEnemiesRange(int x, int y, int range, ref List<Vector2Int> alreadyChecked)
    {
        //Si no está dentro de la grid, si ya está dentro de la lista, o si no se puede caminar por la casilla seleccionada vuelve
        if (!IsInBounds(x, y)) return;
        bool thereIs = alreadyChecked.Contains(new Vector2Int(x, y));
        int diff = map.GetCell(x, y).GetDifficulty();
        if (!map.GetCell(x, y).GetWalkable()) return;

        //si hay un enemigo se añade al array pero se pone el rango a 0 para que no pueda atravesarlo, pero salga una casilla en rojo debajo del enemigo
        if (map.GetCell(x, y).GetCharacter() != null && map.GetCell(x, y).GetCharacter().GetSide() == 0)
        {
            range = 0;
            if (!alliesToAttack.Contains(map.GetCell(x,y).GetCharacter())) alliesToAttack.Add(map.GetCell(x,y).GetCharacter());
        }
        //si el rango es menor o igual a zero se añade la posición y ya,
        //si es mayor a zero se añade y además vuelve a ejecutar la función con cada casilla adyacente y con rango menor dependiendo de la dificultad de movimiento
        if (range <= 0)
        {
            if (!thereIs)
            {
                alreadyChecked.Add(new Vector2Int(x, y));
                moveList.Add((x, y));

            }
        }
        else
        {
            if (!thereIs) alreadyChecked.Add(new Vector2Int(x, y));
            moveList.Add((x, y));
            int ran = Random.Range(1, 3);
            if (ran == 1)
            {
                GetEnemiesRange(x + 1, y, range - diff, ref alreadyChecked);
                GetEnemiesRange(x - 1, y, range - diff, ref alreadyChecked);
                GetEnemiesRange(x, y + 1, range - diff, ref alreadyChecked);
                GetEnemiesRange(x, y - 1, range - diff, ref alreadyChecked);
            }
            else
            {

                GetEnemiesRange(x, y + 1, range - diff, ref alreadyChecked);
                GetEnemiesRange(x, y - 1, range - diff, ref alreadyChecked);
                GetEnemiesRange(x + 1, y, range - diff, ref alreadyChecked);
                GetEnemiesRange(x - 1, y, range - diff, ref alreadyChecked);
            }
        }
        return;
    }

    private void FindClosestPos(int x, int y, int range, ref List<Vector2Int> alreadyChecked)
    {
        //Si no está dentro de la grid, o si no se puede caminar por la casilla seleccionada vuelve
        if (!IsInBounds(x, y)) return;
        bool thereIs = alreadyChecked.Contains(new Vector2Int(x, y));
        int diff = map.GetCell(x, y).GetDifficulty();
        if (!map.GetCell(x, y).GetWalkable()) return;

        int aux = Mathf.Abs(movement.x - x) + Mathf.Abs(movement.y-y);
        if (map.GetCell(x, y).GetCharacter() != null) aux += 10000;
        if (aux < score)
        {
            score = aux;
            targetPos.x = x;
            targetPos.y = y;

        }

        //si el rango es menor o igual a zero se añade la posición y ya,
        //si es mayor a zero se añade y además vuelve a ejecutar la función con cada casilla adyacente y con rango menor dependiendo de la dificultad de movimiento
        if (range <= 0)
        {
            if (!thereIs) alreadyChecked.Add(new Vector2Int(x, y));
        }
        else
        {

            if (!thereIs) alreadyChecked.Add(new Vector2Int(x, y));
            int ran = Random.Range(1, 3);
            if(ran == 1)
            {
                FindClosestPos(x + 1, y, range - diff, ref alreadyChecked);
                FindClosestPos(x - 1, y, range - diff, ref alreadyChecked);
                FindClosestPos(x, y + 1, range - diff, ref alreadyChecked);
                FindClosestPos(x, y - 1, range - diff, ref alreadyChecked);
            }
            else
            {

                FindClosestPos(x, y + 1, range - diff, ref alreadyChecked);
                FindClosestPos(x, y - 1, range - diff, ref alreadyChecked);
                FindClosestPos(x + 1, y, range - diff, ref alreadyChecked);
                FindClosestPos(x - 1, y, range - diff, ref alreadyChecked);
            }
            
        }
        return;
    }

    private bool IsInBounds(int x, int y)
    {
        if (x < 0 || y < 0) return false;
        if (x > map.Width() || y > map.Height()) return false;
        return true;
    }

    public void DoneMoving()
    {
        canDoTurn = true;
    }

    public bool IsEnemyTurn()
    {
        return enemyTurn;
    }

}

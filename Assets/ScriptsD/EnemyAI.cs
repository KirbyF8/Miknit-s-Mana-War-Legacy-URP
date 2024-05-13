using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{

    private MyGrid map;
    private GameManagerD gameManager;
    private CharacterD[] enemies;
    private Vector2Int actionToPerform;
    private Vector2Int movement;
    private List<CharacterD> alliesToAttack;
    private int score;
    private CharacterD[] Allies;
    private Vector2Int targetPos;
    private bool posFound = false;
    // Start is called before the first frame update
    void Start()
    {
        map = FindObjectOfType<MyGrid>();
        alliesToAttack = new List<CharacterD>();
        gameManager = FindObjectOfType<GameManagerD>();
        Allies = gameManager.GetCharacters();
    }

    public void EnemyTurn()
    {
        TrueEnemyTurn();
    }

    private void TrueEnemyTurn()
    {
        enemies = map.GetEnemies();
        for (int i = 0; i < enemies.Length; i++)
        {
            Think(enemies[i]);
            Act(enemies[i]);
        }
    }

    private void Act(CharacterD person)
    {
        if (actionToPerform.x == 1)
        {
            movement = new Vector2Int(-1000,-1000);
            //Debug.Log(actionToPerform.y);
            CharacterD aux = Allies[actionToPerform.y];
            targetPos = new Vector2Int((int)aux.GetPosition().x, (int)aux.GetPosition().y);

            List<Vector2Int> alreadyChecked = new List<Vector2Int>();
            FindPositionToGoTo((int)person.GetPosition().x, (int)person.GetPosition().y, person.GetRange(), ref alreadyChecked);
            gameManager.MoveCharacter(map.GetCell((int)person.GetPosition().x, (int)person.GetPosition().y), map.GetCell(movement.x, movement.y));
            posFound = false;
        }
        else if (actionToPerform.x == 2)
        {
            score = 10000;
            targetPos = new Vector2Int(0,0);
            List<Vector2Int> alreadyChecked = new List<Vector2Int>();
            FindClosestPos((int)person.GetPosition().x, (int)person.GetPosition().y, person.GetMovement(), ref alreadyChecked);
            gameManager.MoveCharacter(map.GetCell((int)person.GetPosition().x, (int)person.GetPosition().y), map.GetCell(targetPos.x, targetPos.y));
        }
        else return;
    }

    private void Think(CharacterD person)
    {
        actionToPerform = new Vector2Int(0, 0);
        if (!person.GetActive())
        {
            return;
        }
        else if (AlliesInRange(person))
        {
            int max = -1000;
            CharacterD Selected = null;
            foreach(CharacterD target in alliesToAttack)
            {
                score = Calculate(person, target);
                if(score > max)
                {
                    max = score;
                    Selected = target;
                }
            }
            actionToPerform.x = 1;
            actionToPerform.y=gameManager.GetNumb(Selected);
            return;
        }
        else
        {
            closestEnemy(person);
            actionToPerform.x = 2;
        }
    }


    private void closestEnemy(CharacterD person)
    {
        float posx = person.GetPosition().x;
        float posy = person.GetPosition().y;
        
        int min = 1000;
        Vector2 dir = new Vector2 (0,0);
        foreach(CharacterD target in Allies)
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

    private int Calculate(CharacterD person, CharacterD target)
    {
        //AQUI HAY QUE PONER EL CALCULO DE DAÑO QUE HACE Y RECIBE
        //BÁSICAMENTE EN LA VARIABLE AUXILIAR SUMAR 100 SI LO MATA, SI NO LO MATA EL PORCENTAJE DE VIDA QUE LE QUITARÍA, DIVIDIR ENTRE DOS Y RESTAR LA PROB. DE ESQUIVAR (SI ES UN 100% SE PUEDE HACER ALGO)
        //RESTAR LA MITAD DEL DAÑO QUE RECIBIRÍA EL PERSONAJE Y FINALMENTE UN RANDOM DEL 1 AL 20
        int aux = 0;
        aux += Random.Range(1, 21);
        return aux;
    }

    private bool AlliesInRange(CharacterD person)
    {
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
            alliesToAttack.Add(map.GetCell(x,y).GetCharacter());
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
            GetEnemiesRange(x + 1, y, range - diff, ref alreadyChecked);
            GetEnemiesRange(x - 1, y, range - diff, ref alreadyChecked);
            GetEnemiesRange(x, y + 1, range - diff, ref alreadyChecked);
            GetEnemiesRange(x, y - 1, range - diff, ref alreadyChecked);
        }
        return;
    }

    private void FindPositionToGoTo(int x, int y, int range, ref List<Vector2Int> alreadyChecked)
    {
        //Si no está dentro de la grid, o si no se puede caminar por la casilla seleccionada vuelve
        if (!IsInBounds(x, y)) return;
        bool thereIs = alreadyChecked.Contains(new Vector2Int(x, y));
        int diff = map.GetCell(x, y).GetDifficulty();
        if (!map.GetCell(x, y).GetWalkable()) return;

        int aux = Mathf.Abs(x-targetPos.x) + Mathf.Abs(y-targetPos.y);
        if (aux == range)
        {
            movement = new Vector2Int(x, y);
            posFound = true;
            return;
            
        }
        
            
        //si hay un enemigo se añade al array pero se pone el rango a 0 para que no pueda atravesarlo, pero salga una casilla en rojo debajo del enemigo
        if (map.GetCell(x, y).GetCharacter() != null && map.GetCell(x, y).GetCharacter().GetSide() == 0)
        {
            range = 0;
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
            FindPositionToGoTo(x + 1, y, range - diff, ref alreadyChecked);
            if (posFound) return;
            FindPositionToGoTo(x - 1, y, range - diff, ref alreadyChecked);
            if (posFound) return;
            FindPositionToGoTo(x, y + 1, range - diff, ref alreadyChecked);
            if (posFound) return;
            FindPositionToGoTo(x, y - 1, range - diff, ref alreadyChecked);
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
            FindClosestPos(x + 1, y, range - diff, ref alreadyChecked);
            FindClosestPos(x - 1, y, range - diff, ref alreadyChecked);
            FindClosestPos(x, y + 1, range - diff, ref alreadyChecked);
            FindClosestPos(x, y - 1, range - diff, ref alreadyChecked);
        }
        return;
    }

    private bool IsInBounds(int x, int y)
    {
        if (x < 0 || y < 0) return false;
        if (x > map.Width() || y > map.Height()) return false;
        return true;
    }

}

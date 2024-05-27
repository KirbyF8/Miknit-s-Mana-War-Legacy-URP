using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class Character : MonoBehaviour
{

    private MyCell position;
    private MyCell previousPos;
    private bool hasMovedThisTurn = false;
    [SerializeField] private MyGrid map;

    //CAMBIAR POR ARMA
    private int range = 1;
    [SerializeField] private bool active = false;

    private string Name = "Yuu";

    public int lvl = 1;
    public int exp;
    public int exp_max;


    public int hp = 8;

    public int mp = 12;

    public int movement = 6;

    // HP Mana Strenght Dexterity Magic Defense Resistance Speed Luck  

    public int[] stats;


    public int[] scale;


    public string arma;

    public bool dmgType;
    
    //? Hacha Lanza Espada Magia Arco

    private LvlUp lvlUp;

    public int side;
    
    void Start()
    {
        map = FindObjectOfType<MyGrid>();
        lvlUp = GetComponent<LvlUp>();
        //lvlUp = FindAnyObjectByType<LvlUp>();
        // ExpReset();
    }

    public void ExpReset()
    {
        if (exp >= exp_max)
        {
            lvl++;
            exp = exp - exp_max;
            exp_max += exp_max;

            lvlUp.LevelUpStat(ref stats, scale);
            
        }
    }



    //Funciones para leer y escribir las variables del CharacterD 

    public int GetMovement()
    {
        return movement;
    }

    public void SetMovement(int x)
    {
        movement = x;
    }

    public int GetSide()
    {
        return side;
    }

    public void SetSide(int x)
    {
        side = x;
    }
    public Vector2 GetPosition()
    {
        return position.GetPosition();
    }

    public void SetPosition(int x, int y)
    {
        position = map.GetCell(x, y);
        transform.position = new Vector3(x * 2 + 1, 0, -y * 2 - 1);
    }

    public Vector2 GetPreviousPosition()
    {
        return previousPos.GetPosition();
    }

    public void SetPreviousPosition(int x, int y)
    {
        previousPos = map.GetCell(x, y);
    }

    public void SetPosition(MyCell cell)
    {
        position = cell;
        transform.position = new Vector3(cell.GetPosition().x * 2 + 1, 0, cell.GetPosition().y * -2 - 1);
    }

    public void SetPosition(Vector2 pos)
    {
        Debug.Log(map == null);
        position = map.GetCell(pos);
        transform.position = new Vector3(position.GetPosition().x * 2 + 1, 0, position.GetPosition().y * -2 - 1);
    }
    public void SetMap(MyGrid map)
    {
        this.map = map;
    }

    public MyGrid GetMap()
    {
        return this.map;
    }

    public void SetHasMoved(bool x)
    {
        hasMovedThisTurn = x;
    }

    public bool GetHasMoved()
    {
        return hasMovedThisTurn;
    }

    public int GetRange()
    {
        return range;
    }

    public void SetRange(int x)
    {
        range = x;
    }

    public bool GetActive()
    {
        return active;
    }

    public void SetActive(bool x)
    {
        active = x;
    }



}

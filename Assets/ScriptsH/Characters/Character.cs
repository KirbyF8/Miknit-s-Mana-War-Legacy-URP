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

    private PersistenciaDeDatos persistenciaDeDatos;
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

    //? Hacha Lanza Espada Magia Arco

    public string arma;

    public bool dmgType;
    
    

    private LvlUp lvlUp;

    public int side;
    
    void Start()
    {
        map = FindObjectOfType<MyGrid>();
        lvlUp = GetComponent<LvlUp>();
        //lvlUp = FindAnyObjectByType<LvlUp>();

        persistenciaDeDatos = FindAnyObjectByType<PersistenciaDeDatos>();
        LoadData();

        
        
    }

    public void ExpReset()
    {

            lvl++;
            exp -= exp_max;
            exp_max += exp_max;

            lvlUp.LevelUpStat(ref stats, scale);
        SaveStats();
        

    }

    public void GiveExp(int x)
    {
        exp += x;
        if(exp>= exp_max)
        {
            ExpReset();
            
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
        //Debug.Log(map == null);
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

    

    //guardar y cargar las estadisticas de personajes jugables

    public void SaveStats()
    {
        if (side == 0)
        {
            persistenciaDeDatos.SetExp(exp);
            persistenciaDeDatos.SetLevel(lvl);

            persistenciaDeDatos.SetStats(stats[0], stats[1], stats[2], stats[3], stats[4], stats[5], stats[6], stats[7], stats[8]);
            persistenciaDeDatos.SaveStats(gameObject.name);
        }
        

    }

    public void LoadData()
    {

        if (side == 0)
        {
            persistenciaDeDatos.LoadStats(gameObject.name);


            for (int i = 0; i < stats.Length; i++)
            {
                if (persistenciaDeDatos.IfExist())
                {
                    stats[i] = persistenciaDeDatos.SendStats(i);
                }

                
            }

            lvl = persistenciaDeDatos.SendLevel();
            exp = persistenciaDeDatos.SendExp();
        }
         

    }

}

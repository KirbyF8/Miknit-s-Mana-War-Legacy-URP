using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

public class PersistenciaDeDatos : MonoBehaviour
{

    private string path = Application.dataPath + "/../saves/"+"save.json";
    private string statsPath = Application.dataPath + "/../stats saves/";




    private int id;
   
    private string description;
    private bool tutorialDone;


    private int level;
    private int exp;

    private int maxHp;
    private int maxMana;
    private int strengh;
    private int dexterity;
    private int magic;
    private int defense;
    private int resistance;
    private int speed;
    private int luck;

    private bool exists;


    private int[] stats = new int[9];

    private int aux;


    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(path);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("i"))
        {
            save();
        };
    }

    private void cosas()
    {
        
    }


    public void save()
    {

        Save save = new Save
        {
            Id = 0,
            //Name = "Jaime",
            //Description = "jaime",
            tutorialDone = tutorialDone,

        };


        string jsonContent = JsonUtility.ToJson(save);
        Debug.Log(jsonContent);
        File.WriteAllText(path, jsonContent);

    }

    public void SaveStats(string fileName)
    {



        Stats save = new Stats
        {
            level = level,
            exp = exp,

            maxHp = maxHp,
            maxMana = maxMana,
            strengh = strengh,
            dexterity = dexterity,
            magic = magic,
            defense = defense,
            resistance = resistance,
            speed = speed,
            luck = luck,


        };

        string jsonContent = JsonUtility.ToJson(save);
        Debug.Log(jsonContent);
        File.WriteAllText(statsPath + fileName + ".json", jsonContent);

    }

    public void LoadStats(string fileName)
    {
        if (File.Exists(statsPath + fileName + ".json"))
        {
            string jsonContent = File.ReadAllText(statsPath + fileName +".json");   

            Stats statsave = JsonUtility.FromJson<Stats>(jsonContent);

            Debug.Log(statsave.maxHp);

            level = statsave.level;
            exp = statsave.exp;
            
            stats[0] = statsave.maxHp;
            stats[1] = statsave.maxMana;
            stats[2] = statsave.strengh;
            stats[3] = statsave.dexterity;
            stats[4] = statsave.magic;
            stats[5] = statsave.defense;
            stats[6] = statsave.resistance;
            stats[7] = statsave.speed;
            stats[8] = statsave.luck;

            exists = true;
            
            /*
            maxHp = statsave.maxHp;
            maxMana = statsave.maxMana;
            strengh = statsave.strengh;
            dexterity = statsave.dexterity;
            magic = statsave.magic;
            defense = statsave.defense;
            resistance = statsave.resistance;
            speed = statsave.speed;
            luck = statsave.luck;
            */
            
            //aux = 0;

        }



        else
        {
            Debug.LogError("No existe el archivo de personaje");
            //exists = false;

        }

        



    }

    public int SendStats(int stat)
    {
        
            return stats[stat];
        

    }

    public bool IfExist()
    {
        return exists;
    }

    public void LoadData()
    {
        if (File.Exists(path))
        {
            string jsonContent = File.ReadAllText(path);

            Save save = JsonUtility.FromJson<Save>(jsonContent);

            //if(!inTitleScreen){}

            id = save.Id;
            name = save.Name;
            description = save.Description;
            tutorialDone = save.tutorialDone;
            //GameManager.Instance.Load(save.name, save.mapPosition, save.points, save.color, save.level, save.exp, save.initialPower, save.levelsCompleted, save.timePlayed);
            //PlayerControlMap.Instance.SetPlayerPos(save.mapPosition);
        }
        else
        {
            Debug.LogError("¡¡¡ EL ARCHIVO DE GUARDADO NO EXISTE !!!");

        }
    }




    public bool GetTutorialDone()
    {
        return tutorialDone;
    }

    public void SetExp(int newExp)
    {
        exp = newExp;
    }

    public void SetLevel(int newLevel)
    {
        level = newLevel;
    }

    public int SendExp()
    {
        return exp;
    }

    public int SendLevel()
    {
        return level;
    }

    public void SetStats(int getMaxHP, int getMaxMana, int getStrengh, int getDexterity, int getMagic, int getDefense, int getResistance, int getSpeed, int getLuck)
    {
        maxHp = getMaxHP;
        maxMana = getMaxMana;
        strengh = getStrengh;
        dexterity = getDexterity;
        magic = getMagic;
        defense = getDefense;
        resistance = getResistance;
        speed = getSpeed;
        luck = getLuck;

    }

    public void Skip()
    {
        tutorialDone = true;
    }
    public void NoSkip()
    {
        tutorialDone=false;
    }



}

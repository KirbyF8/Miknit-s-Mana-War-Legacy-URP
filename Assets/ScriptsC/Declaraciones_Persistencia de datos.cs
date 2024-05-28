using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

public class PersistenciaDeDatos : MonoBehaviour
{

    private string path = Application.dataPath + "/../saves/"+"save.json";




    private int id;
    private string name;
    private string description;
    private bool tutorialDone;
    


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(path);
        
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
            Name = "Jaime",
            Description = "jaime",
            HP= 50,
            Strengh= 10,
            Defense= 100,
            Magic= 5,
            Speed= 10,
        };


        string jsonContent = JsonUtility.ToJson(save);
        Debug.Log(jsonContent);
        File.WriteAllText(path, jsonContent);

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



}

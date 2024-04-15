using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Declaraciones_Persistenciadedatos : MonoBehaviour
{

    private string path = Application.dataPath + "/../saves/"+"save.json";

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




}

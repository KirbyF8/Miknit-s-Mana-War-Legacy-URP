using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Declaraciones_Persistenciadedatos : MonoBehaviour
{

    private string path =  "/save.json";

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Application.persistentDataPath);
        
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
            Description = "jaime"
        };


        string jsonContent = JsonUtility.ToJson(save);
        Debug.Log(jsonContent);
        File.WriteAllText(Application.persistentDataPath + path, jsonContent);

    }




}

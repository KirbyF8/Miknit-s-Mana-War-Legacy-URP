using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Finish", 3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Finish()
    {
        SceneManager.UnloadScene("LoadingScene");
    }


}

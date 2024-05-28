using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TittleManager : MonoBehaviour
{


    [SerializeField] private GameObject MainPanel;
    [SerializeField] private GameObject CreditsPanel;
    [SerializeField] private GameObject OptionsPanel;
    [SerializeField] private Button mainButtonSelect;

    private PersistenciaDeDatos persistenciaDeDatos;


    // Start is called before the first frame update
    void Start()
    {
        MainPanel.SetActive(true);
        CreditsPanel.SetActive(false);
        OptionsPanel.SetActive(false);
        persistenciaDeDatos = GetComponent<PersistenciaDeDatos>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void Exit()
    {
        Application.Quit();
    }

    public void ChangeToCredits()
    {

    }
    public void ShowCreditsPanel()
    {
        CreditsPanel.SetActive(true);
    }
    public void HideCreditsPanel()
    {
        CreditsPanel.SetActive(false);
        mainButtonSelect.Select();

    }

    public void ShowOptionsPanel()
    {
        OptionsPanel.SetActive(true);
    }
    public void HideOptionsPanel()
    {
        OptionsPanel.SetActive(false);
        mainButtonSelect.Select();
    }

    public void GoToTutoriralCoroutine()
    {
        StartCoroutine(GoToTutorial());

    }
    public IEnumerator GoToTutorial()
    {
        yield return new WaitForSeconds(0.50f);

        if (!persistenciaDeDatos.GetTutorialDone())
        {
            SceneManager.LoadScene(2);
        }
        else
        {
            SceneManager.LoadScene(3);
        }
        
        
        
    }


}

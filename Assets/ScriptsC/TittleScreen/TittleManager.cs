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

    [SerializeField] private GameObject skipPanel;

    private PersistenciaDeDatos persistenciaDeDatos;


    // Start is called before the first frame update
    void Start()
    {
        persistenciaDeDatos = FindAnyObjectByType<PersistenciaDeDatos>();
        persistenciaDeDatos.LoadData();
        MainPanel.SetActive(true);
        CreditsPanel.SetActive(false);
        OptionsPanel.SetActive(false);
        Time.timeScale = 1.0f;
        
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

        skipPanel.SetActive(true);

        

    }

    public void GoToPlay()
    {
        if (!persistenciaDeDatos.GetTutorialDone())
        {
            SceneManager.LoadScene(2);
        }
        else
        {
            SceneManager.LoadScene(3);
        }
    }

    public void SkippingTutorial(bool skip)
    {

        if (skip)
        {
            persistenciaDeDatos.Skip();
            persistenciaDeDatos.save();
            GoToPlay();
        }
        else if (!skip)
        {
            persistenciaDeDatos.NoSkip();
            persistenciaDeDatos.save();
            GoToPlay();
        }

    }

}

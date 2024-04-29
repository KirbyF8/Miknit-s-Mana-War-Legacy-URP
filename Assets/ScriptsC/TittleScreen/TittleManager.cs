using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TittleManager : MonoBehaviour
{


    [SerializeField] private GameObject MainPanel;
    [SerializeField] private GameObject CreditsPanel;
    [SerializeField] private GameObject OptionsPanel;

    // Start is called before the first frame update
    void Start()
    {
        MainPanel.SetActive(true);
        CreditsPanel.SetActive(false);
        OptionsPanel.SetActive(false);
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
    }

    public void ShowOptionsPanel()
    {
        OptionsPanel.SetActive(true);
    }
    public void HideOptionsPanel()
    {
        OptionsPanel.SetActive(false);
    }


}

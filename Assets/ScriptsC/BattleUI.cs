using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    [SerializeField] private GameObject normalUIPanel;
    [SerializeField] private Button buttonSelectUIPanel;
    [SerializeField] private GameObject talkUIPanel;
    [SerializeField] private Button buttonSelectTalkUIPanel;

    [SerializeField] private bool posibleTalk;
    [SerializeField] private bool opened;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown("z"))
        {
            OpenUI();
        }
        if (Input.GetKeyDown("x"))
        {
            CloseUI();
        }

    }

    private void OpenUI()
    {
        
        if (!opened)
        {
            if (!posibleTalk)
            {
                normalUIPanel.active = true;
                buttonSelectUIPanel.Select();
                opened = true;
            }
            else if (posibleTalk)
            {
                talkUIPanel.SetActive(true);
                buttonSelectTalkUIPanel.Select();
                opened = true;
            }
        }
        
        

    }
    private void CloseUI()
    {
        if (opened)
        {
            if (!posibleTalk)
            {
                normalUIPanel.active = false;
                opened = false;
            }
            else if (posibleTalk)
            {
                talkUIPanel.SetActive(false);
                opened = false;
            }
        }
    }



}

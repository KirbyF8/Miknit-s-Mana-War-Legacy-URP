using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField] private GameObject options;

    [SerializeField] private Button goBack;

    [SerializeField] private Slider master;
    [SerializeField] private Slider sfx;
    [SerializeField] private Slider voices;
    [SerializeField] private Slider music;

    [SerializeField] private Toggle autoEnd;
    [SerializeField] private Toggle showGrid;
    [SerializeField] private Toggle partyMode;
    [SerializeField] private Toggle mirror;


    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        goBack.onClick.AddListener(HideOptions);
        master.onValueChanged.AddListener(gameManager.ChangeMaster);
        sfx.onValueChanged.AddListener(gameManager.ChangeSFX);
        voices.onValueChanged.AddListener(gameManager.ChangeVoices);
        music.onValueChanged.AddListener(gameManager.ChangeMusic);
        autoEnd.onValueChanged.AddListener(gameManager.AutoEndOnOff);
        showGrid.onValueChanged.AddListener(gameManager.ShowGrid);
        partyMode.onValueChanged.AddListener(gameManager.PartyMode);
        mirror.onValueChanged.AddListener(gameManager.MirrorStats);
    }

    private void ShowOptions()
    {
        options.SetActive(true);
    }

    private void HideOptions()
    {
        options.SetActive(false);
    }

}

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

        //Asignamos a todos los elementos de la UI su función de GameManager correspondiente

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


    //Función para enseñar el panel de opciones
    public void ShowOptions()
    {
        options.SetActive(true);
    }

    //Función para ocultar el panel de opciones
    public void HideOptions()
    {
        options.SetActive(false);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameManagerD gameManager;

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

    [SerializeField] private Button endSpawnPhase;
    [SerializeField] private Button endTurn;
    [SerializeField] private Button mainMenu;

    void Start()
    {
        gameManager = FindObjectOfType<GameManagerD>();

        //Asignamos a todos los elementos de la UI su función de GameManager correspondiente

        goBack.onClick.AddListener(gameManager.Hideoptions);
        master.onValueChanged.AddListener(gameManager.ChangeMaster);
        sfx.onValueChanged.AddListener(gameManager.ChangeSFX);
        voices.onValueChanged.AddListener(gameManager.ChangeVoices);
        music.onValueChanged.AddListener(gameManager.ChangeMusic);
        autoEnd.onValueChanged.AddListener(gameManager.AutoEndOnOff);
        showGrid.onValueChanged.AddListener(gameManager.ShowGrid);
        partyMode.onValueChanged.AddListener(gameManager.PartyMode);
        mirror.onValueChanged.AddListener(gameManager.MirrorStats);

        endSpawnPhase.onClick.AddListener(gameManager.EndSpawn);
        endTurn.onClick.AddListener(gameManager.EndTurn);
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

    //Función para desactivar el botón de terminar la fase de spawn
    public void HideEndOfSpawn()
    {
        endSpawnPhase.gameObject.SetActive(false);
    }

    //Función para activar el botón de terminar la fase de spawn
    public void ShowEndOfSpawn()
    {
        endSpawnPhase.gameObject.SetActive(true);
    }

    public void EndTurnOn()
    {
        endTurn.interactable = true;
    }

    public void EndTurnOff()
    {
        endTurn.interactable = false;
    }

    public void HideTurn()
    {
        endTurn.gameObject.SetActive(false);
    }

    public void ShowTurn()
    {
        endTurn.gameObject.SetActive(true);
    }
}

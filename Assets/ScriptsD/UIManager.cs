using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private Toggle partyMode;
    [SerializeField] private Toggle mirror;

    [SerializeField] private Button endSpawnPhase;
    [SerializeField] private Button endTurn;
    [SerializeField] private Button mainMenu;

    [SerializeField] private TextMeshProUGUI tileDifficulty;

    [SerializeField] private TextMeshProUGUI[] stats;

    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;

    [SerializeField] private GameObject terreinPanel;

    private UIBattle uiBattle;
    void Start()
    {
        gameManager = FindObjectOfType<GameManagerD>();

        uiBattle = FindObjectOfType<UIBattle>();
        //Asignamos a todos los elementos de la UI su funci�n de GameManager correspondiente

        goBack.onClick.AddListener(gameManager.Hideoptions);
        master.onValueChanged.AddListener(gameManager.ChangeMaster);
        sfx.onValueChanged.AddListener(gameManager.ChangeSFX);
        voices.onValueChanged.AddListener(gameManager.ChangeVoices);
        music.onValueChanged.AddListener(gameManager.ChangeMusic);
        autoEnd.onValueChanged.AddListener(gameManager.AutoEndOnOff);
        partyMode.onValueChanged.AddListener(gameManager.PartyMode);
        mirror.onValueChanged.AddListener(gameManager.MirrorStats);

        endSpawnPhase.onClick.AddListener(gameManager.EndSpawn);
        endTurn.onClick.AddListener(gameManager.EndTurn);
    }


    //Funci�n para ense�ar el panel de opciones
    public void ShowOptions()
    {
        options.SetActive(true);
    }

    //Funci�n para ocultar el panel de opciones
    public void HideOptions()
    {
        options.SetActive(false);
        
    }

    //Funci�n para desactivar el bot�n de terminar la fase de spawn
    public void HideEndOfSpawn()
    {
        endSpawnPhase.gameObject.SetActive(false);
    }

    //Funci�n para activar el bot�n de terminar la fase de spawn
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

    public float MusicVolume()
    {
        return music.value;
    }

    public float SfxVolume()
    {
        return sfx.value;
    }

    public float VoicesVolume()
    {
        return voices.value;
    }
    public float MasterVolume()
    {
        return master.value;
    }

    public void ChangeTileInfo(int difficulty)
    {
        if (difficulty == 69) 
        {
            tileDifficulty.text = "Wall";
        }
        else
        {
            tileDifficulty.text = difficulty.ToString();
        }
        
    }

    public void YouWin()
    {
        winPanel.SetActive(true);
        endSpawnPhase.gameObject.SetActive(false);
        endTurn.gameObject.SetActive(false);
        terreinPanel.SetActive(false);
    }

    public void HideTerrain()
    {
        terreinPanel.SetActive(false);
        
    }

    public void ShowTerrain()
    {
        terreinPanel.SetActive(true);
    }
}

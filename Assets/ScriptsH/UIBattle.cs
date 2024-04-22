using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBattle : MonoBehaviour
{
    [SerializeField] TMP_Text[] attackerInfoTextsColorChange;
    [SerializeField] GameObject attackerBackGrounds1ColorChange;
    [SerializeField] GameObject attackerBackGrounds2ColorChange;
    [SerializeField] GameObject[] attackerBackGrounds3ColorChange;
    [SerializeField] Slider attackerHealthBar;
    [SerializeField] TextMeshProUGUI attackerName;
    [SerializeField] Image attackerWeapon;
    [SerializeField] Image attackerTriangle;
    [SerializeField] TextMeshProUGUI attackerHealth;


    [SerializeField] TextMeshProUGUI[] defenderInfoTextsColorChange;
    [SerializeField] GameObject defenderBackGrounds1ColorChange;
    [SerializeField] GameObject defenderBackGrounds2ColorChange;
    [SerializeField] GameObject[] defenderBackGrounds3ColorChange;
    [SerializeField] Slider defenderHealthBar;
    [SerializeField] TextMeshProUGUI defenderName;
    [SerializeField] Image defenderWeapon;
    [SerializeField] Image defenderTriangle;
    [SerializeField] TextMeshProUGUI defenderHealth;

    private void Start()
    {
        Debug.Log(attackerInfoTextsColorChange[0].name);
        // ColorChange();
    }

    private void ColorChange()
    {
       
        
        for (int i = 0; i < attackerInfoTextsColorChange.Length; i++)
        {
            
            attackerInfoTextsColorChange[i].color = Color.red;
        }

    }
}

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
    [SerializeField] Slider attackerManaBar;
    [SerializeField] TextMeshProUGUI attackerName;
    [SerializeField] Image attackerWeapon;
    [SerializeField] Image attackerTriangle;
    [SerializeField] TextMeshProUGUI attackerHealth;
    [SerializeField] TextMeshProUGUI attackerMana;
    [SerializeField] TextMeshProUGUI attackerDMG;
    [SerializeField] TextMeshProUGUI attackerATKS;
    [SerializeField] TextMeshProUGUI attackerHitC;
    [SerializeField] TextMeshProUGUI attackerCritC;


    [SerializeField] TextMeshProUGUI[] defenderInfoTextsColorChange;
    [SerializeField] GameObject defenderBackGrounds1ColorChange;
    [SerializeField] GameObject defenderBackGrounds2ColorChange;
    [SerializeField] GameObject[] defenderBackGrounds3ColorChange;
    [SerializeField] Slider defenderHealthBar;
    [SerializeField] Slider defenderManaBar;
    [SerializeField] TextMeshProUGUI defenderName;
    [SerializeField] Image defenderWeapon;
    [SerializeField] Image defenderTriangle;
    [SerializeField] TextMeshProUGUI defenderHealth;
    [SerializeField] TextMeshProUGUI defenderMana;
    [SerializeField] TextMeshProUGUI defenderDMG;
    [SerializeField] TextMeshProUGUI defenderATKS;
    [SerializeField] TextMeshProUGUI defenderHitC;
    [SerializeField] TextMeshProUGUI defenderCritC;

    private void Start()
    {
        // Debug.Log(attackerInfoTextsColorChange[0].name);
        // ColorChange();
    }

    private void ColorChange()
    {
       
        
        for (int i = 0; i < attackerInfoTextsColorChange.Length; i++)
        {
            
            attackerInfoTextsColorChange[i].color = Color.red;
        }

    }



    public void ValueChanges(Character atacker, Character defender, int aDMG, int aATKs, int aHit, int aCrit, int dDMG, int dATKs, int dHit, int dCrit)
    {

        attackerName.text = atacker.name;

        attackerHealthBar.maxValue = atacker.stats[0];
        attackerHealthBar.value = atacker.hp;
        attackerHealth.text = atacker.hp + "/" + atacker.stats[0];
       
        

        attackerManaBar.maxValue = atacker.stats[1];
        attackerManaBar.value = atacker.mp;
        attackerMana.text = atacker.mp + "/" + atacker.stats[1];

        attackerDMG.text = aDMG+"";
        attackerATKS.text = aATKs+"" ;
        attackerHitC.text = aHit+"";
        attackerCritC.text = aCrit+"";

        // attackerWeapon;
        // attackerTriangle;

        defenderName.text = defender.name;

        defenderHealthBar.maxValue = defender.stats[0];
        defenderHealthBar.value = defender.hp;
        defenderHealth.text = defender.hp + "/" + defender.stats[0];



        defenderManaBar.maxValue = defender.stats[1];
        defenderManaBar.value = defender.mp;
        defenderMana.text = defender.mp + "/" + defender.stats[1];

        defenderDMG.text = dDMG + "";
        defenderATKS.text = dATKs + "";
        defenderHitC.text = dHit + "";
        defenderCritC.text = dCrit + "";

        // defenderWeapon;
        // attackerTriangle;
    }
}

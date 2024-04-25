using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class UIBattle : MonoBehaviour
{
    [SerializeField] Sprite[] weaponTriangleSprites;
    [SerializeField] Sprite[] weaponSprites;

    
    [SerializeField] Image attackerBackGrounds1ColorChange;
    [SerializeField] Image attackerBackGrounds2ColorChange;
    [SerializeField] Image[] attackerBackGrounds3ColorChange;
    [SerializeField] Slider attackerHealthBar;
    [SerializeField] Slider attackerManaBar;
    [SerializeField] TextMeshProUGUI attackerName;
    [SerializeField] Image attackerWeapon;
    [SerializeField] Image attackerTriangle;
    [SerializeField] TextMeshProUGUI attackerHealth;
    [SerializeField] TextMeshProUGUI attackerMana;
    [SerializeField] TextMeshProUGUI attackerDMG;
    [SerializeField] TextMeshProUGUI attackerDMGinfo;
    [SerializeField] TextMeshProUGUI attackerATKS;
    [SerializeField] TextMeshProUGUI attackerATKSinfo;
    [SerializeField] TextMeshProUGUI attackerHitC;
    [SerializeField] TextMeshProUGUI attackerHitCinfo;
    [SerializeField] TextMeshProUGUI attackerCritC;
    [SerializeField] TextMeshProUGUI attackerCritCinfo;


    
    [SerializeField] Image defenderBackGrounds1ColorChange;
    [SerializeField] Image defenderBackGrounds2ColorChange;
    [SerializeField] Image[] defenderBackGrounds3ColorChange;
    [SerializeField] Slider defenderHealthBar;
    [SerializeField] Slider defenderManaBar;
    [SerializeField] TextMeshProUGUI defenderName;
    [SerializeField] Image defenderWeapon;
    [SerializeField] Image defenderTriangle;
    [SerializeField] TextMeshProUGUI defenderHealth;
    [SerializeField] TextMeshProUGUI defenderMana;
    [SerializeField] TextMeshProUGUI defenderDMG;
    [SerializeField] TextMeshProUGUI defenderDMGinfo;
    [SerializeField] TextMeshProUGUI defenderATKS;
    [SerializeField] TextMeshProUGUI defenderATKSinfo;
    [SerializeField] TextMeshProUGUI defenderHitC;
    [SerializeField] TextMeshProUGUI defenderHitCinfo;
    [SerializeField] TextMeshProUGUI defenderCritC;
    [SerializeField] TextMeshProUGUI defenderCritCinfo;

    private void Start()
    {
        // Debug.Log(attackerInfoTextsColorChange[0].name);
        // ColorChange();
    }

   



    public void ValueChanges(Character atacker, Character defender, int aDMG, int aATKs, int aHit, int aCrit, int dDMG, int dATKs, int dHit, int dCrit, int weaponTriangle)
    {

        Vector3 atackerColor = new Vector3();
        Vector3 atackerColor2 = new Vector3();
        Vector3 atackerColor3 = new Vector3();

        if (atacker.side == 0)
        {
            atackerColor = new Vector3(0f/255f,0f/255f,0f/255f);
            atackerColor2 = new Vector3(67f / 255f, 173f / 255f, 82f / 255f);
            atackerColor3 = new Vector3(119f / 255f, 206f / 255f, 131f / 255f);
        }
        else if (atacker.side == 1)
        {
            atackerColor = new Vector3(0f / 255f, 0f / 255f, 0f / 255f);
            atackerColor2 = new Vector3(99f / 255f, 155f / 255f, 1);
            atackerColor3 = new Vector3(143f / 255f, 183f / 255f, 1);
        }
        else if(atacker.side == 2)
        {
            atackerColor = new Vector3(0f / 255f, 0f / 255f, 0f / 255f);
            atackerColor2 = new Vector3(217f / 255f, 87f / 255f, 99f / 255f);
            atackerColor3 = new Vector3(230f / 255f, 117f / 255, 128f / 255f);
        }
        else
        {
            atackerColor = new Vector3(0f / 255f, 0f / 255f, 0f / 255f);
            atackerColor2 = new Vector3(251f/255f, 242f/255f, 54f/255f);
            atackerColor3 = new Vector3(1, 248f/255f, 104f/255f);
        }
        

       
        Color colorAtacker = new Color(atackerColor.x, atackerColor.y, atackerColor.z, 255);
        Color colorAtacker2 = new Color(atackerColor2.x, atackerColor2.y, atackerColor2.z, 255);
        Color colorAtacker3 = new Color(atackerColor3.x, atackerColor3.y, atackerColor3.z, 255);

        attackerBackGrounds2ColorChange.color = colorAtacker2;
        attackerBackGrounds1ColorChange.color = colorAtacker3;

        for (int i = 0; i > attackerBackGrounds3ColorChange.Length; i++)
        {
            attackerBackGrounds3ColorChange[i].color = colorAtacker3;
        }
        attackerName.color = colorAtacker;
        attackerName.text = atacker.name;

        attackerHealthBar.maxValue = atacker.stats[0];
        attackerHealthBar.value = atacker.hp;
        attackerHealth.text = atacker.hp + "/" + atacker.stats[0];
       attackerHealth.color = colorAtacker;
        

        attackerManaBar.maxValue = atacker.stats[1];
        attackerManaBar.value = atacker.mp;
        attackerMana.text = atacker.mp + "/" + atacker.stats[1];
        attackerMana.color = colorAtacker;

        attackerDMG.text = aDMG+"";
        attackerDMG.color = colorAtacker;
        attackerATKS.text = aATKs+"" ;
        attackerATKS.color = colorAtacker;
        attackerHitC.text = aHit+"";
        attackerHitC.color = colorAtacker;
        attackerCritC.text = aCrit+"";
        attackerCritC.color = colorAtacker;

        attackerATKSinfo.color = colorAtacker;
        attackerHitCinfo.color = colorAtacker;
        attackerDMGinfo.color = colorAtacker;
        attackerCritCinfo.color = colorAtacker;

        if (atacker.arma == "Espada")
        {
            attackerWeapon.sprite = weaponSprites[0]; 
        }
        else if (atacker.arma == "Hacha")
        {
            attackerWeapon.sprite = weaponSprites[1];
        }
        else if (atacker.arma == "Lanza")
        {
            attackerWeapon.sprite = weaponSprites[2];
        }
        else
        {
            attackerWeapon.sprite = weaponSprites[3];
        }

        attackerTriangle.sprite = weaponTriangleSprites[weaponTriangle];


        //////////////////////////////////////////////////////////////////////////////
        Vector3 defenderColor = new Vector3();
        Vector3 defenderColor2 = new Vector3();
        Vector3 defenderColor3 = new Vector3();

        if (defender.side == 0)
        {
            defenderColor = new Vector3(0f / 255f, 0f / 255f, 0f / 255f);
            defenderColor2 = new Vector3(67f/255f, 173f/255f, 82f/255f);
            defenderColor3 = new Vector3(119f/255f, 206f/255f, 131f/255f);
        }
        else if (defender.side == 1)
        {
            defenderColor = new Vector3(0f / 255f, 0f / 255f, 0f / 255f);
            defenderColor2 = new Vector3(99f/255f, 155f/255f, 1);
            defenderColor3 = new Vector3(143f / 255f, 183f / 255f, 1);
        }
        else if (defender.side == 2)
        {
            defenderColor = new Vector3(0f / 255f, 0f / 255f, 0f / 255f);
            defenderColor2 = new Vector3(217f/255f, 87f/255f, 99f/255f);
            defenderColor3 = new Vector3(230f/255f, 117f/255, 128f/255f);
        }
        else
        {
            defenderColor = new Vector3(0f / 255f, 0f / 255f, 0f / 255f);
            defenderColor2 = new Vector3(251f/ 255f, 242f/ 255f, 54f/255f);
            defenderColor3 = new Vector3(1, 248f/255f, 104f/255f);
        }

       

        Color colorDefender = new Color(defenderColor.x, defenderColor.y, defenderColor.z, 255);
        Color colorDefender2 = new Color(defenderColor2.x, defenderColor2.y, defenderColor2.z, 255);
        Color colorDefender3 = new Color(defenderColor3.x, defenderColor3.y, defenderColor3.z, 255);

        defenderBackGrounds2ColorChange.color = colorDefender2;
        defenderBackGrounds1ColorChange.color = colorDefender3;
        
        for(int i = 0; i > defenderBackGrounds3ColorChange.Length; i++)
        {
            defenderBackGrounds3ColorChange[i].color = colorDefender3;
        }

        defenderName.text = defender.name;
        defenderName.color = colorDefender;

        defenderHealthBar.maxValue = defender.stats[0];
        defenderHealthBar.value = defender.hp;

        defenderHealth.text = defender.hp + "/" + defender.stats[0];
        defenderHealth.color = colorDefender;



        defenderManaBar.maxValue = defender.stats[1];
        defenderManaBar.value = defender.mp;

        defenderMana.text = defender.mp + "/" + defender.stats[1];
        defenderMana.color = colorDefender;

        defenderDMG.text = dDMG + "";
        defenderDMG.color = colorDefender;
        defenderATKS.text = dATKs + "";
        defenderATKS.color = colorDefender;
        defenderHitC.text = dHit + "";
        defenderHitC.color = colorDefender;
        defenderCritC.text = dCrit + "";
        defenderCritC.color = colorDefender;

        defenderATKSinfo.color = colorDefender;
        defenderHitCinfo.color = colorDefender;
        defenderDMGinfo.color = colorDefender;
        defenderCritCinfo.color = colorDefender;

        if (defender.arma == "Espada")
        {
            defenderWeapon.sprite = weaponSprites[0];
        }
        else if (defender.arma == "Hacha")
        {
            defenderWeapon.sprite = weaponSprites[1];
        }
        else if (defender.arma == "Lanza")
        {
            defenderWeapon.sprite = weaponSprites[2];
        }
        else
        {
            defenderWeapon.sprite = weaponSprites[3];
        }

        if (weaponTriangle == 0)
        {
            defenderTriangle.sprite = weaponTriangleSprites[weaponTriangle];
            
        }
        else if (weaponTriangle == 1)
        {
            defenderTriangle.sprite = weaponTriangleSprites[2];
        }
        else
        {
            defenderTriangle.sprite = weaponTriangleSprites[1];
        }

        


    }
}

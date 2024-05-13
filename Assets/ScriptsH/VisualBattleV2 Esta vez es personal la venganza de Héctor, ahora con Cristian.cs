using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Sprites;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;

public class VisualBattleV2 : MonoBehaviour
{
    private Vector3 attackerPos = new Vector3(600, 180, 42);

    private Vector3 defenderPos = new Vector3(600, 180, 52);

    [SerializeField] private UIBattle uiBattle;

    private string defenderName;
    private string attackerName;

    private int maxAttacks1;
    private int attacks1;
    private int maxAttacks2;
    private int attacks2;

    private string atackerNameConst;

    private bool starded = false;

    private IEnumerator coroutine;



    public void SpawnCharacters(Character attacker, Character defender)
    {
        atackerNameConst = attacker.name + "(Clone)";

        //? Quitar mas tarde
        attacker.hp = attacker.stats[0];
        defender.hp = defender.stats[0];
        //? Quitar mas tarde

        Instantiate(attacker, attackerPos, Quaternion.Euler(0, 0, 0));
        Instantiate(defender, defenderPos, Quaternion.Euler(0, 180, 0));
    }

    public void callAttack(Character atacker, Character defender, int aCrit, int aDMG, int aHit)
    {
        //! Settear la corrutina y llamarla

        coroutine = Attack(atacker,defender, aCrit, aDMG,aHit);
        StartCoroutine(coroutine);
    }

    private IEnumerator Attack(Character atacker, Character defender, int aCrit, int aDMG, int aHit)
    {
        attackerName = atacker.name + "(Clone)";
        defenderName = defender.name + "(Clone)";

        int aux = Random.Range(0, 101);

        coroutine = ADamage(aDMG);

        if (atacker.hp <= 0 || defender.hp <= 0)
        {
            //? Dejenlo ya esta muerto
           yield return null;
        }
        else
        {
            if (aux <= aHit)
            {
                if (aDMG <= 0)
                {
                    //! No Damage


                    yield return null;
                }
                else
                {


                    aux = Random.Range(0, 101);
                    if (aux < aCrit)
                    {

                        StartCoroutine(coroutine);




                    }
                    else
                    {
                        StartCoroutine(coroutine);




                    }

                }
            }
            else
            {
                //! miss
                StartCoroutine(coroutine);


                yield return null;
            }
        }

        yield return null;
    }


    private IEnumerator ADamage(int aDMG)
    {

        starded = true;
        Character defender;
        //int aDMG = uiBattle.ReturnDamage();

        GameObject DefenderBody = GameObject.Find(defenderName);


        defender = DefenderBody.GetComponent<Character>();

        Character atacker;
        GameObject AtackerBody = GameObject.Find(attackerName);
        atacker = AtackerBody.GetComponent<Character>();
        Animator atackerAnimator = AtackerBody.GetComponent<Animator>();

        atackerAnimator.SetBool("AttackV2", true);
        Debug.Log(aDMG);
        yield return new WaitForSeconds(1.5f);
        atackerAnimator.SetBool("AttackV2", false);

        defender.hp -= aDMG;
        
        if (atacker.name == atackerNameConst)
        {
            uiBattle.HealthUpdate(atacker, defender);
        }
        else
        {
            uiBattle.HealthUpdate(defender, atacker);
        }
        starded = false;
    }
    public void GetAllAttackCosas(Character attacker,Character defender,int aCrit,int dCrit,int aDMG,int dDMG,int aATKs,int dATKs,int aHit,int dHit)
    {
        callAttack(attacker, defender, aCrit, aDMG, aHit);
    }


}

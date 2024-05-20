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
    private Battle battle;

    private string defenderName;
    private string attackerName;

    private int maxAttacksA;
    private int attacksA;
    private int maxAttacksD;
    private int attacksD;

    private string atackerNameConst;
    private string defenderNameConst;

    private bool starded = false;

    private IEnumerator coroutine;

    private int auxDos = 1;
    private int auxTres = 1;

    private bool isBattleOver;

    private bool defenderTurn = false;

    private ParticleSystem atackerParticles;
    private ParticleSystem defenderParticles;
 
    private void Start()
    {
        battle = GetComponent<Battle>();
    }
    public void SpawnCharacters(Character attacker, Character defender)
    {
        atackerNameConst = attacker.name + "(Clone)";
        defenderNameConst = defender.name + "(Clone)";


        //? Quitar mas tarde
        attacker.hp = attacker.stats[0];
        defender.hp = defender.stats[0];
        //? Quitar mas tarde


        Instantiate(attacker, attackerPos, Quaternion.Euler(0, 0, 0));

        GameObject a = GameObject.Find($"{atackerNameConst}"); 
        atackerParticles = a.GetComponent<ParticleSystem>();
       
       

        Instantiate(defender, defenderPos, Quaternion.Euler(0, 180, 0));
        GameObject d = GameObject.Find($"{defenderNameConst}");
        defenderParticles = d.GetComponent<ParticleSystem>();

    }

    public void callAttack(Character atacker, Character defender, int aCrit, int aDMG, int aHit, int aATKs )
    {
        //! Settear la corrutina y llamarla

        coroutine = Attack(atacker,defender, aCrit, aDMG,aHit,aATKs);


        
        
            StartCoroutine(coroutine);
        
       

    }

    private IEnumerator Attack(Character atacker, Character defender, int aCrit, int aDMG, int aHit, int aATKs)
    {
        attackerName = atacker.name + "(Clone)";
        defenderName = defender.name + "(Clone)";

        int aux = Random.Range(0, 101);

        coroutine = ADamage(aDMG, aATKs);

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


    private IEnumerator ADamage(int aDMG, int aATKs)
    {
        
        //Debug.Log(defenderParticles.name);
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
        yield return new WaitForSeconds(0.7f);
        atackerAnimator.SetBool("AttackV2", false);
        defender.hp -= aDMG;
        if (defenderTurn)
        {
            atackerParticles.Play();
        }
        else 
        { 
            defenderParticles.Play(); 
        }
       
      

        yield return new WaitForSeconds(0.5f);
        



        if (atacker.name == atackerNameConst)
        {
            uiBattle.HealthUpdate(atacker, defender);
        }
        else
        {
            uiBattle.HealthUpdate(defender, atacker);
        }
        starded = false;
      
       
        if (auxDos <= aATKs - 1 && auxTres < 2 && attacksA < maxAttacksA)
        {
            
            auxDos++;
            if (atacker.name == atackerNameConst)
            {
                auxTres++;
            }
            attacksA++;
                coroutine = ADamage(aDMG, aATKs);
            StartCoroutine(coroutine);
        }
        else if (!defenderTurn && attacksD < maxAttacksD)
        {
            auxDos = 1;
            auxTres = 1;
            defenderTurn = true;
            battle.GetAllAttack();
        }
        if(defenderTurn && auxDos <= aATKs - 1 && attacksD < maxAttacksD)
        {
            yield return new WaitForSeconds(1.4f);
            attacksD++;
            auxDos = 1;
            auxTres = 1;
            defenderTurn = false;
            battle.GetAllAttack();
        }
        
    }
    public void GetAllAttackCosas(Character attacker,Character defender,int aCrit,int dCrit,int aDMG,int dDMG,int aATKs,int dATKs,int aHit,int dHit)
    {
        maxAttacksA = aATKs;
        maxAttacksD = dATKs;

        if (!defenderTurn)
        {

            callAttack(attacker, defender, aCrit, aDMG, aHit, aATKs);

        }
        else
        {
            callAttack(defender, attacker, dCrit, dDMG, dHit, dATKs);
        }
       


    }


}

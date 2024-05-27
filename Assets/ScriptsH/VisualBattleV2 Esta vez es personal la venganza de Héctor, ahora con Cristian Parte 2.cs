using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Sprites;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;

public class VisualBattleV2 : MonoBehaviour
{
    private Vector3 attackerPos = new Vector3(-5, 0.2f, 304.5f);

    private Vector3 defenderPos = new Vector3(5, 0.2f, 304.5f);

    [SerializeField] private UIBattle uiBattle;
    private Battle battle;

    private string defenderName;
    private string attackerName;

    private string defenderBodyName;
    private string attackerBodyName;

    private int maxAttacksA;
    private int attacksA;
    private int maxAttacksD;
    private int attacksD;

    private string atackerNameConst;
    private string defenderNameConst;

    private bool started = false;

    private IEnumerator coroutine;

    private int actualAttacks = 0;
    

    private bool isBattleOver;

    private bool defenderTurn = false;

    private ParticleSystem atackerParticles;
    private ParticleSystem defenderParticles;

    private bool Crited = false;

    

    private int aDMGlocal;
    private int dDMGlocal;
    private int aHITLocal;
    private int dHITLocal;
    private int aCRITLocal;
    private int dCRITLocal;
    private int aATKsLocal;
    private int dATKsLocal;
   
 
    private void Start()
    {
        battle = GetComponent<Battle>();
    }
    public void SpawnCharacters(Character attacker, Character defender)
    {
        atackerNameConst = attacker.name;
        attackerBodyName = attacker.name + "(Clone)";
        defenderNameConst = defender.name;
        defenderBodyName = defender.name + "(Clone)";


        //? Quitar mas tarde
        attacker.hp = attacker.stats[0];
        defender.hp = defender.stats[0];
        //? Quitar mas tarde



        Instantiate(attacker.gameObject, attackerPos, Quaternion.Euler(0, 75, 0));

        GameObject a = GameObject.Find($"{attackerBodyName}"); 
        atackerParticles = a.GetComponent<ParticleSystem>();
       
       

        Instantiate(defender.gameObject, defenderPos, Quaternion.Euler(0, -75, 0));
        GameObject d = GameObject.Find($"{defenderBodyName}");
        defenderParticles = d.GetComponent<ParticleSystem>();

        battle.GetAllAttack();
       //callAttack(attacker, defender, aCrit, aDMG, aHit, aATKs);
    }

    public void callAttack(Character atacker, Character defender)
    {
        //! Settear la corrutina y llamarla

        coroutine = Attack(atacker,defender);

            StartCoroutine(coroutine);
        
       

    }

    private IEnumerator Attack(Character atacker, Character defender)
    {



        int aux = Random.Range(0, 101);

        coroutine = ADamage(atacker, defender);
        if (!defenderTurn)
        {
            if (atacker.hp <= 0 || defender.hp <= 0)
            {
                //? Dejenlo ya esta muerto
                yield return null;
            }
            else
            {
                if (aux <= aHITLocal)
                {
                    if (aDMGlocal <= 0)
                    {
                        //! No Damage
                        yield return null;
                    }
                    else
                    {
                        aux = Random.Range(0, 101);
                        if (aux < aCRITLocal)
                        {
                            //! Cr�tico
                            Crited = true;
                            StartCoroutine(coroutine);
                        }
                        else
                        {
                            Crited = false;
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
        else
        {
            if (atacker.hp <= 0 || defender.hp <= 0)
            {
                //? Dejenlo ya esta muerto
                yield return null;
            }
            else
            {
                if (aux <= dHITLocal)
                {
                    if (dDMGlocal <= 0)
                    {
                        //! No Damage
                        yield return null;
                    }
                    else
                    {
                        aux = Random.Range(0, 101);
                        if (aux < dCRITLocal)
                        {
                            //! Cr�tico
                            Crited = true;
                            StartCoroutine(coroutine);
                        }
                        else
                        {
                            Crited = false;
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
    }


    private IEnumerator ADamage(Character atacker,Character defender)
    {
        yield return new WaitForSeconds(1.5f);
        //Debug.Log(defenderParticles.name);

        started = true;
       
      
        Animator atackerAnimator = atacker.gameObject.GetComponent<Animator>();

        atackerAnimator.SetBool("AttackV2", true);
        // Debug.Log(aDMG);
        yield return new WaitForSeconds(0.7f);
        atackerAnimator.SetBool("AttackV2", false);

        DealDamage(atacker, defender);
        
        if (!defenderTurn)
        {
            attacksA++;
            
        }
        else
        {
            attacksD++;
        }
        Debug.Log( attacksA + "--" + attacksD );
        
        actualAttacks++;

        ParticleManager();
       
      

        yield return new WaitForSeconds(0.5f);


        HealthBarUpdate(atacker, defender);
        
        started = false;

        

        


        if (!defenderTurn)
        {
            if (attacksA < aATKsLocal && aATKsLocal > 2 && actualAttacks < 2)
            {
                
                    // 2 Ataques
                    coroutine = Attack(atacker, defender);
                    StartCoroutine(coroutine);
                
            }
            else
            {
                actualAttacks = 0;
               
                
                if ( attacksD >= maxAttacksD && attacksA < aATKsLocal)
                {
                    // Ataca el mismo
                    callAttack(atacker, defender);
                }
                else if ( attacksD >= maxAttacksD && attacksA >= aATKsLocal)
                {
                    yield return null;
                }
                else 
                {
                    // cambio de turno
                    defenderTurn = !defenderTurn;
                    coroutine = Attack(defender, atacker);
                    StartCoroutine(coroutine);
                }
               
               
            }
        }
        else if (defenderTurn)
        {
            Debug.Log(actualAttacks);
            if (attacksD < dATKsLocal && dATKsLocal > 2 && actualAttacks < 2)
            {
              
                    // 2 Ataques
                    coroutine = Attack(atacker, defender);
                    StartCoroutine(coroutine);

                

            }
            else
            {
                actualAttacks = 0;
               
               
                if (attacksA >= maxAttacksA && attacksD < dATKsLocal)
                {
                    // Ataca el mismo
                    callAttack(atacker, defender);
                }
                else if (attacksD >= maxAttacksD && attacksA >= aATKsLocal)
                {
                    yield return null;
                }
                else 
                {
                    // Cambio de Turno
                    defenderTurn = !defenderTurn;
                    coroutine = Attack(defender, atacker);
                    StartCoroutine(coroutine);
                }
                
            }
        }
        
        /*
         if (auxDos <= aATKs - 1 && auxTres < 2 && attacksA < maxAttacksA)
         {

             auxDos++;
             if (atacker.name == atackerNameConst)
             {
                 auxTres++;
             }
             attacksA++;
                 coroutine = ADamage(atacker, defender, aCrit, aDMG, aHit, aATKs);
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


         if (aATKs > 2 && attacksA < aATKs)
         {
             coroutine = Attack(atacker, defender, aCrit, aDMG, aHit, aATKs);
         }
        */

    }
    public void GetAllAttackCosas(Character attacker,Character defender,int aCrit,int dCrit,int aDMG,int dDMG,int aATKs,int dATKs,int aHit,int dHit)
    {
        maxAttacksA = aATKs;
        maxAttacksD = dATKs;

        SaveBattle(attacker, defender, aCrit, dCrit, aDMG, dDMG, aATKs, dATKs, aHit, dHit);
        Debug.Log(defenderTurn);
       
       
        if (!defenderTurn)
        {

            callAttack(attacker, defender);

        }
        else
        {
            callAttack(defender, attacker);
        }
       


    }

    private void HealthBarUpdate(Character atacker, Character defender)
    {

        if (atacker.name == atackerNameConst)
        {
            uiBattle.HealthUpdate(atacker, defender);
        }
        else
        {
            uiBattle.HealthUpdate(defender, atacker);
        }
    }

    private void ParticleManager()
    {

    if (Crited)
        {

            // Cambiar a Particulas De Cr�tico
            if (defenderTurn)
            {
               
                atackerParticles.Play();
            }
            else
            {
                defenderParticles.Play();
            }
        }
        else
        {
            if (defenderTurn)
            {
                atackerParticles.Play();
            }
            else
            {
                defenderParticles.Play();
            }
        }
        
    }

    private void SaveBattle(Character attacker, Character defender, int aCrit, int dCrit, int aDMG, int dDMG, int aATKs, int dATKs, int aHit, int dHit)
    {


      aDMGlocal = aDMG;
      dDMGlocal = dDMG;
      aHITLocal = aHit;
      dHITLocal = dHit;
      aCRITLocal = aCrit;
      dCRITLocal = dCrit;
      aATKsLocal = aATKs;
      dATKsLocal = dATKs;

}

    private void DealDamage(Character atacker, Character defender)
    {
    if (!defenderTurn)
        {
            if (Crited)
            {
                defender.hp -= aDMGlocal * 2;
            }
            else
            {
                defender.hp -= aDMGlocal;
            }
        }
        else
        {
            if (Crited)
            {
                defender.hp -= dDMGlocal * 2;
            }
            else
            {
                defender.hp -= dDMGlocal;
            }
        }
       
    }

}

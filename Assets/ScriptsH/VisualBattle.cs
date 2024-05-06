using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;

public class VisualBattle : MonoBehaviour
{
    private Vector3 attackerPos = new Vector3(600, 180, 40);
    private Vector3 attackertargetPos = new Vector3(600, 180, 50);

    private Vector3 defenderPos = new Vector3(600, 180, 53);
    private Vector3 defendertargetPos = new Vector3(600, 180, 42);

    [SerializeField] private UIBattle uiBattle;

    

    
    public void SpawnCharacters(Character attacker, Character defender)
    {
        //? Quitar mas tarde
        attacker.hp = attacker.stats[0];
        defender.hp = defender.stats[0];
        //? Quitar mas tarde

        Instantiate(attacker, attackerPos, Quaternion.Euler(0,0,0));
        Instantiate(defender, defenderPos, Quaternion.Euler(0, 180, 0));
    }

    private void A_Atack(Character atacker, Character defender, int aCrit, int aDMG, int aHit)
    {
        int aux = Random.Range(0, 101);
        if (atacker.hp <= 0 || defender.hp <= 0)
        {
            //? Dejenlo ya esta muerto
            return;
        }
        else
        {
            if (aux <= aHit)
            {
                if (aDMG <= 0)
                {
                    //! No Damage
                    Wait(atacker, defender, false, false, true, 0);
                    
                    return;
                }
                else
                {

                    {
                        aux = Random.Range(0, 101);
                        if (aux < aCrit)
                        {
                            Wait(atacker, defender, false, true, false, aDMG * 3);
                           
                            new WaitForSeconds(3f);
                            

                        }
                        else
                        {
                            Wait(atacker, defender, false, false, false, aDMG);
                            

                            new WaitForSeconds(3f);
                            
                        }
                    }
                }
            }
            else
            {
                //! miss
                Wait(atacker, defender, true, false, false, 0);
                
                new WaitForSeconds(3f);
                return;
            }
        }
    }
        

    


    public void AnimationBattle(Character atacker, Character defender, int aCrit, int dCrit, int aDMG, int dDMG, int aATKs, int dATKs, int aHit, int dHit)
    {

      

       

        int aux = Random.Range(0, 101);
        int auxDefenderHits = 0;
        int auxAttackerHits = 0;

         while (auxAttackerHits < aATKs || auxDefenderHits < dATKs)
        {
            if ((aATKs-auxAttackerHits) >= 3 )
            {
                A_Atack(atacker, defender, aCrit, aDMG,  aHit);
                auxAttackerHits++;
                uiBattle.HealthUpdate(atacker, defender);
                A_Atack(atacker, defender, aCrit, aDMG, aHit);
                auxAttackerHits++;
                uiBattle.HealthUpdate(atacker, defender);
            }
            else
            {
                A_Atack(atacker, defender, aCrit, aDMG, aHit);
                auxAttackerHits++;
                uiBattle.HealthUpdate(atacker, defender);
            }

            if ((dATKs - auxDefenderHits) >= 3)
            {
                A_Atack(defender, atacker, dCrit, dDMG, dHit);
                auxDefenderHits++;
                uiBattle.HealthUpdate(atacker, defender);
                A_Atack(defender, atacker, dCrit, dDMG, dHit);
                auxDefenderHits++;
                uiBattle.HealthUpdate(atacker, defender);
            }
            else
            {
                A_Atack(defender, atacker, dCrit, dDMG, dHit);
                auxDefenderHits++;
                uiBattle.HealthUpdate(atacker, defender);
            }

        }
       
          
      
        
    }

   

    private void Wait(Character atacker, Character defender, bool missed, bool crit, bool noDMG, int aDMG)
    {
        GameObject AtackerBody = GameObject.Find(atacker.name + "(Clone)");
        GameObject DefenderBody = GameObject.Find(defender.name + "(Clone)");

        Animator atackerAnimator = AtackerBody.GetComponent<Animator>();
        Animator defenderAnimator = DefenderBody.GetComponent<Animator>();


        atackerAnimator.SetBool("AttackV2", true);

        Invoke("Damage", 2f);
        
    }

    private void Damage()
    {
        Debug.Log("Hola");     
        /*
        Character defender; int aDMG;

        GameObject DefenderBody = GameObject.Find(defender.name + "(Clone)");
        defender = DefenderBody.GetComponent<Character>();

        GameObject AtackerBody = GameObject.Find(atacker.name + "(Clone)");
        Animator atackerAnimator = AtackerBody.GetComponent<Animator>();

        defender.hp -= aDMG;

        atackerAnimator.SetBool("AttackV2", false);
    */
        }


}

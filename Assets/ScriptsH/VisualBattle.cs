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

        Instantiate(attacker, attackerPos, Quaternion.Euler(0,10,0));
        Instantiate(defender, defenderPos, Quaternion.Euler(0, 170, 0));
    }

    private void A_Atack(Character atacker, Character defender, int aCrit, int aDMG, int aHit)
    {
        int aux = Random.Range(0, 101);
        if (atacker.hp <= 0 || defender.hp <= 0)
        {
            return;
        }
        else
        {
            if (aux <= aHit)
            {
                if (aDMG <= 0)
                {
                    //! No Damage
                    return;
                }
                else
                {

                    {
                        aux = Random.Range(0, 101);
                        if (aux < aCrit)
                        {
                            defender.hp -= aDMG * 3;


                        }
                        else
                        {
                            defender.hp -= aDMG;

                        }
                    }
                }
            }
            else
            {
                //! miss
                Debug.Log("miss");
                return;
            }
        }
    }
        

    


    public void AnimationBattle(Character atacker, Character defender, int aCrit, int dCrit, int aDMG, int dDMG, int aATKs, int dATKs, int aHit, int dHit)
    {

      

        GameObject AtackerBody = GameObject.Find(atacker.name+"(Clone)");
        GameObject DefenderBody = GameObject.Find(defender.name+"(Clone)");

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

        
        
         
        
       
}

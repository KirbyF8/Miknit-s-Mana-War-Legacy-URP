using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualBattle : MonoBehaviour
{
    private Vector3 attackerPos = new Vector3(600, 180, 40);
    

    private Vector3 defenderPos = new Vector3(600, 180, 53);
    

    public void SpawnCharacters(Character attacker, Character defender)
    {
        Instantiate(attacker, attackerPos, Quaternion.Euler(0,10,0));
        Instantiate(defender, defenderPos, Quaternion.Euler(0, 170, 0));
    }

    private void Battle()
    {
        
    }
}

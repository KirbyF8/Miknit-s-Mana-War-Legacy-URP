using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    private bool walkable;
    private float difficulty;
    private Character characterOnTop;
    private float evasionBuff;
    private float defenceBuff;

    public bool GetWalkable()
    {
        return walkable;
    }

    public void SetWalkable(bool x)
    {
        walkable = x;
    }

    public float GetDifficulty()
    {
        return difficulty;
    }

    public void SetDifficulty(float diff)
    {
        difficulty = diff;
    }

    public Character GetCharacter()
    {
        return characterOnTop;
    }

    public void SetCharacter(Character someone)
    {
        characterOnTop = someone;
    }

    public float GetEvasion()
    {
        return evasionBuff;
    }

    public void SetEvasion(float x)
    {
        evasionBuff = x;
    }

    public float GetDefence()
    {
        return defenceBuff;
    }

    public void SetDefence(float x)
    {
        defenceBuff = x;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCell : MonoBehaviour
{
    private bool walkable = true;
    private int difficulty = 1;
    private Character characterOnTop;
    private float evasionBuff;
    private float defenceBuff;
    private int positionx;
    private int positiony;

    public bool GetWalkable()
    {
        return walkable;
    }

    public void SetWalkable(bool x)
    {
        walkable = x;
    }

    public int GetDifficulty()
    {
        return difficulty;
    }

    public void SetDifficulty(int diff)
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

    public int[] GetPosition()
    {
        int[] position = new int[2];
        position[0] = positionx;
        position[1] = positiony;
        return position;
    }

    public void SetPosition(int x, int y)
    {
        positionx = x;
        positiony = y;
    }

    public void Copy(MyCell cell)
    {
        positiony = cell.positiony;
        positionx = cell.positionx;
        walkable = cell.walkable;
        difficulty = cell.difficulty;
        evasionBuff = cell.evasionBuff;
        defenceBuff = cell.defenceBuff;
    }

}

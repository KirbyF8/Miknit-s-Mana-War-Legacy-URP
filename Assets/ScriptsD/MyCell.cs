using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCell
{
    [SerializeField] private bool walkable = true;
    [SerializeField] private int difficulty = 1;
    
    [SerializeField] private float evasionBuff;
    [SerializeField] private float defenceBuff;
    private CharacterD characterOnTop;
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

    public CharacterD GetCharacter()
    {
        return characterOnTop;
    }

    public void SetCharacter(CharacterD someone)
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

    public Vector2 GetPosition()
    {
        Vector2 position = new Vector2 (positionx, positiony);
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

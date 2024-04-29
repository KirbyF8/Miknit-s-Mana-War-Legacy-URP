using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Cell")]

public class CellCreatorSO : ScriptableObject
{
    public bool walkable = true;
    public int difficulty = 1;

    public float evasionBuff;
    public float defenceBuff;
}

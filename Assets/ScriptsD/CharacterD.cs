using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterD : MonoBehaviour
{
    private MyCell position;

    [SerializeField] private MyGrid map;

    private void Start()
    {
       // map = FindObjectOfType<MyGrid>();
    }

    public Vector2 GetPosition()
    {
        return position.GetPosition();
    }

    public void SetPosition(int x, int y)
    {
        position = map.GetCell(x,y);
        transform.position = new Vector3(x*2+1, 0, y*2-1);
    }

    public void SetPosition(MyCell cell)
    {
        position = cell;
        transform.position = new Vector3(cell.GetPosition().x * 2 + 1, 0, cell.GetPosition().y * -2 - 1);
    }


    public void SetMap(MyGrid map)
    {
        this.map = map;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterD : MonoBehaviour
{
    private MyCell position;
    [SerializeField] private int movement = 2;
    [SerializeField] private int side = 0;

    [SerializeField] private MyGrid map;

  /*  private void Start()
    {
       // map = FindObjectOfType<MyGrid>();
    }*/

    public int GetMovement()
    {
        return movement;
    }

    public void SetMovement(int x)
    {
        movement = x;
    }
    public int GetSide()
    {
        return side;
    }

    public void SetSide(int x)
    {
        side = x;
    }
    public Vector2 GetPosition()
    {
        return position.GetPosition();
    }

    public void SetPosition(int x, int y)
    {
        position = map.GetCell(x,y);
        transform.position = new Vector3(x*2+1, 0, -y*2-1);
    }

    public void SetPosition(MyCell cell)
    {
        position = cell;
        transform.position = new Vector3(cell.GetPosition().x * 2 + 1, 0, cell.GetPosition().y * -2 - 1);
    }

    public void SetPosition(Vector2 pos)
    {
        position = map.GetCell((int)pos.x, (int)pos.y);
        transform.position = new Vector3(position.GetPosition().x * 2 + 1, 0, position.GetPosition().y * -2 - 1);
    }

    public void SetMap(MyGrid map)
    {
        this.map = map;
    }

}

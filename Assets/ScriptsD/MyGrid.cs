using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MyGrid : MonoBehaviour
{

    [SerializeField] private int height;
    [SerializeField] private int width;
    private MyCell[,] gridArray;
    [SerializeField] private Vector2[] spawnPositions;

    private void Start()
    {

    }

    public MyGrid(int height, int width, MyCell defaultcell)
    {
        CreateGrid(height, width, defaultcell);
    }

    public void SetSpawn(Vector2[] spawns)
    {
        spawnPositions = spawns;
    }

    public Vector2[] GetSpawn()
    {
        return spawnPositions;
    }

    public int Height()
    {
        return height;
    }

    public int Width()
    {
        return width;
    }

    public MyCell GetCell(int x, int y)
    {
        return gridArray[x, y];
    }

    public void CreateGrid(int height, int width, MyCell defaultcell)
    {
        this.height = height;
        this.width = width;
        gridArray = new MyCell[height, width];

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                gridArray[i,j] = new MyCell();
                gridArray[i, j].Copy(defaultcell);
                gridArray[i, j].SetPosition(i, j);
            }
        }
    }

    public bool IsInBounds(int x, int y)
    {
        if (x < 0 || y < 0) return false;
        if (x > width || y > height) return false;
        return true;
    }

    public void SetCell(MyCell cell, int x, int y)
    {
        gridArray[x, y] = cell;
    }

    public MyCell[] GetNeighbors(int x, int y, int range)
    {
        return GetNeighbors(x, y, range, range);
    }

    private MyCell[] GetNeighbors(int xCordinate, int yCordinate, int xEndOffset = 1, int yEndOffset = 1)
    {
        List<MyCell> neighbourCells = new List<MyCell>();

        int yEnd = (int)MathF.Min(height - 1, yCordinate + yEndOffset);
        int xEnd = (int)MathF.Min(width - 1, xCordinate + xEndOffset);

        for (int y = 0; y <= yEnd; y++)
        {
            for (int x = 0; x <= xEnd; x++)
            {
                if (x == xCordinate && y == yCordinate)
                {
                    continue;
                }
                if (MathF.Abs(x - xCordinate) + MathF.Abs(y - yCordinate) > xEndOffset) continue;
                neighbourCells.Add(gridArray[x, y]);
            }
        }


        MyCell[] tempArray = neighbourCells.ToArray();
        return tempArray;
    }

    public List<Vector2> GetMovement(int x, int y, int move)
    {
        List<Vector2> end = new List<Vector2>();
        GetMovementRec(x, y, move, ref end);
        return end;
    }

    private void GetMovementRec(int x, int y, int range, ref List<Vector2> end) 
    {
        if (x < 0 || y < 0 || x > width-1 || y > height-1) return;
        bool thereIs = end.Contains(new Vector2(x, y));
        int diff = gridArray[x,y].GetDifficulty();
        if (!gridArray[x, y].GetWalkable()) return;
        if (range <= 0)
        { 
            if(!thereIs) end.Add(new Vector2(x, y));
        }
        else
        {
            if (!thereIs) end.Add(new Vector2(x, y));
            GetMovementRec(x + 1, y, range - diff, ref end);
            GetMovementRec(x - 1, y, range - diff, ref end);
            GetMovementRec(x, y + 1, range - diff, ref end);
            GetMovementRec(x, y - 1, range - diff, ref end);
        }
        return;
    }
    
}

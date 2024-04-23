using AStar;
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
    [SerializeField] private CharacterD[] enemies;
    [SerializeField] private Vector2[] enemiesPositions;
    private bool[,] walkablemap;

    private void Start()
    {
        MyCell cell = new MyCell();
        cell.SetWalkable(true);
        CreateGrid(20, 20, cell);
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].SetPosition(enemiesPositions[i]);
            gridArray[(int)enemiesPositions[i].x, (int)enemiesPositions[i].y].SetCharacter(enemies[i]);
        }
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
    public MyCell GetCell(Vector2 x)
    {
        return gridArray[(int)x.x, (int)x.y];
    }


    public void CreateGrid(int height, int width, MyCell defaultcell)
    {
        this.height = height;
        this.width = width;
        gridArray = new MyCell[height, width];
        walkablemap = new bool[height,width];

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                gridArray[i,j] = new MyCell();
                gridArray[i, j].Copy(defaultcell);
                gridArray[i, j].SetPosition(i, j);
                walkablemap[i,j] = defaultcell.GetWalkable();
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

    public void SetWalkable(Vector2 cell, bool walk)
    {
        gridArray[(int)cell.x, (int)cell.y].SetWalkable(walk);
        walkablemap[(int)cell.x, (int)cell.y] = walk;
    }

    public void SetWalkable(int x, int y, bool walk)
    {
        gridArray[x, y].SetWalkable(walk);
        walkablemap[x, y] = walk;
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
        if (gridArray[x, y].GetCharacter() != null && (gridArray[x, y].GetCharacter().GetSide() > 0 || gridArray[x, y].GetCharacter().GetSide() < 0)) range = 0; ;
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

    public (int, int)[] FindPath(Vector2 start, Vector2 goal)
    {
        for(int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                walkablemap[(int)enemies[i].GetPosition().x, (int)enemies[i].GetPosition().y] = false;
            }
        }

        (int, int)[] aux = AStarPathfinding.GeneratePathSync((int)start.x, (int) start.y, (int)goal.x, (int)goal.y, walkablemap, true,true);

        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                walkablemap[(int)enemies[i].GetPosition().x, (int)enemies[i].GetPosition().y] = true;
            }
        }

        return aux;

    }
    
}

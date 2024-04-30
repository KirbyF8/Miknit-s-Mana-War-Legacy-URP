using AStar;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class MyGrid : MonoBehaviour
{

    [SerializeField] private int height;
    [SerializeField] private int width;
    private MyCell[,] gridArray;
    [SerializeField] private Vector2[] spawnPositions;
    [SerializeField] private CharacterD[] enemies;
    [SerializeField] private Vector2[] enemiesPositions;
    private bool[,] walkableMap;
    [SerializeField] private CellCreatorSO[] cellsAux;
    private MyCell[] cells;
    [SerializeField] private Vector2[] cellsToChange;
    [SerializeField] private int[] numberOfCell;
    private void Start()
    {
        cells = new MyCell[cellsAux.Length];
        for (int i = 0; i < cellsAux.Length; i++)
        {
            cells[i] = new MyCell();
            cells[i].SetWalkable(cellsAux[i].walkable);
            cells[i].SetEvasion(cellsAux[i].evasionBuff);
            cells[i].SetDefence(cellsAux[i].defenceBuff);
            cells[i].SetDifficulty(cellsAux[i].difficulty);
        }
        CreateGrid(20, 20);
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].SetPosition(enemiesPositions[i]);
            gridArray[(int)enemiesPositions[i].x, (int)enemiesPositions[i].y].SetCharacter(enemies[i]);
        }
        
    }

    public MyGrid(int height, int width)
    {
        CreateGrid(height, width);
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


    public void CreateGrid(int height, int width)
    {
        this.height = height;
        this.width = width;
        gridArray = new MyCell[height, width];
        walkableMap = new bool[height,width];

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                gridArray[i,j] = new MyCell();
                gridArray[i, j].Copy(cells[0]);
                gridArray[i, j].SetPosition(i, j);
                walkableMap[i,j] = cells[0].GetWalkable();
            }
        }
        for (int i = 0;i<cellsToChange.Length; i++)
        {
            gridArray[(int)cellsToChange[i].x, (int)cellsToChange[i].y].Copy(cells[numberOfCell[i]]);
            if (!gridArray[(int)cellsToChange[i].x, (int)cellsToChange[i].y].GetWalkable()) walkableMap[(int)cellsToChange[i].x, (int)cellsToChange[i].y] = false;

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
        walkableMap[(int)cell.x, (int)cell.y] = walk;
    }

    public void SetWalkable(int x, int y, bool walk)
    {
        gridArray[x, y].SetWalkable(walk);
        walkableMap[x, y] = walk;
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
                walkableMap[(int)enemies[i].GetPosition().x, (int)enemies[i].GetPosition().y] = false;
            }
        }
        Debug.Log(walkableMap[(int)enemies[0].GetPosition().x, (int)enemies[0].GetPosition().y]);
        //(int, int)[] aux = AStarPathfinding.GeneratePathSync((int)start.x, (int) start.y, (int)goal.x, (int)goal.y, walkableMap, false,false);
        List<(int, int)> aux = PathFinding(start, goal, gridArray[(int)start.x, (int) start.y].GetCharacter().GetMovement());

        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                walkableMap[(int)enemies[i].GetPosition().x, (int)enemies[i].GetPosition().y] = true;
            }
        }

        return aux.ToArray();

    }

    private List<(int, int)> PathFinding(Vector2 start, Vector2 goal, int range)
    {
        if (start == goal) return new List<(int, int)> { ((int)start.x, (int)start.y) };
        else if ((int)start.x < 0 || (int)start.x > width || (int)start.y < 0 || (int)start.y > height) return null;
        else if (!walkableMap[(int)start.x, (int)start.y]) return null;
        else if (range == 0) return null;
        else
        {
            if (start.y < goal.y)
            {
                List<(int, int)> possible = PathFinding(new Vector2(start.x, start.y + 1), goal, range - 1);
                if (possible != null)
                {
                    possible.Insert(0,((int)start.x, (int)start.y));
                    return possible;
                }
                else
                {
                    if (start.x < goal.x)
                    {
                        possible = PathFinding(new Vector2(start.x + 1, start.y), goal, range - 1);
                        if (possible != null)
                        {
                            possible.Insert(0, ((int)start.x, (int)start.y));
                            return possible;
                        }
                        possible = PathFinding(new Vector2(start.x - 1, start.y), goal, range - 1);
                        if (possible != null)
                        {
                            possible.Insert(0, ((int)start.x, (int)start.y));
                            return possible;
                        }
                    }
                    else
                    {
                        possible = PathFinding(new Vector2(start.x - 1, start.y), goal, range - 1);
                        if (possible != null)
                        {
                            possible.Insert(0, ((int)start.x, (int)start.y));
                            return possible;
                        }
                        possible = PathFinding(new Vector2(start.x + 1, start.y), goal, range - 1);
                        if (possible != null)
                        {
                            possible.Insert(0, ((int)start.x, (int)start.y));
                            return possible;
                        }
                    }
                    possible = PathFinding(new Vector2(start.x, start.y - 1), goal, range - 1);
                    if (possible != null)
                    {
                        possible.Insert(0, ((int)start.x, (int)start.y));
                        return possible;
                    }
                }
            }

            else if(start.y > goal.y) 
            {
                List<(int, int)> possible = PathFinding(new Vector2(start.x, start.y - 1), goal, range - 1);
                if (possible != null)
                {
                    possible.Insert(0, ((int)start.x, (int)start.y));
                    return possible;
                }
                else
                {
                    if (start.x < goal.x)
                    {
                        possible = PathFinding(new Vector2(start.x + 1, start.y), goal, range - 1);
                        if (possible != null)
                        {
                            possible.Insert(0, ((int)start.x, (int)start.y));
                            return possible;
                        }
                        possible = PathFinding(new Vector2(start.x - 1, start.y), goal, range - 1);
                        if (possible != null)
                        {
                            possible.Insert(0, ((int)start.x, (int)start.y));
                            return possible;
                        }
                    }
                    else
                    {
                        possible = PathFinding(new Vector2(start.x - 1, start.y), goal, range - 1);
                        if (possible != null)
                        {
                            possible.Insert(0, ((int)start.x, (int)start.y));
                            return possible;
                        }
                        possible = PathFinding(new Vector2(start.x + 1, start.y), goal, range - 1);
                        if (possible != null)
                        {
                            possible.Insert(0, ((int)start.x, (int)start.y));
                            return possible;
                        }
                    }
                    possible = PathFinding(new Vector2(start.x, start.y + 1), goal, range - 1);
                    if (possible != null)
                    {
                        possible.Insert(0, ((int)start.x, (int)start.y));
                        return possible;
                    }
                }
            }

            else
            {
                List<(int, int)> possible = null;
                if (start.x < goal.x)
                {
                    possible = PathFinding(new Vector2(start.x + 1, start.y), goal, range - 1);
                    if (possible != null)
                    {
                        possible.Insert(0, ((int)start.x, (int)start.y));
                        return possible;
                    }
                    possible = PathFinding(new Vector2(start.x, start.y + 1), goal, range - 1);
                    if (possible != null)
                    {
                        possible.Insert(0, ((int)start.x, (int)start.y));
                        return possible;
                    }
                    possible = PathFinding(new Vector2(start.x, start.y - 1), goal, range - 1);
                    if (possible != null)
                    {
                        possible.Insert(0, ((int)start.x, (int)start.y));
                        return possible;
                    }
                    possible = PathFinding(new Vector2(start.x - 1, start.y), goal, range - 1);
                    if (possible != null)
                    {
                        possible.Insert(0, ((int)start.x, (int)start.y));
                        return possible;
                    }
                }
                else
                {
                    possible = PathFinding(new Vector2(start.x - 1, start.y), goal, range - 1);
                    if (possible != null)
                    {
                        possible.Insert(0, ((int)start.x, (int)start.y));
                        return possible;
                    }
                    possible = PathFinding(new Vector2(start.x, start.y + 1), goal, range - 1);
                    if (possible != null)
                    {
                        possible.Insert(0, ((int)start.x, (int)start.y));
                        return possible;
                    }
                    possible = PathFinding(new Vector2(start.x, start.y - 1), goal, range - 1);
                    if (possible != null)
                    {
                        possible.Insert(0, ((int)start.x, (int)start.y));
                        return possible;
                    }
                    possible = PathFinding(new Vector2(start.x + 1, start.y), goal, range - 1);
                    if (possible != null)
                    {
                        possible.Insert(0, ((int)start.x, (int)start.y));
                        return possible;
                    }
                }
                
            }

            return null;
        }
        
    }

}

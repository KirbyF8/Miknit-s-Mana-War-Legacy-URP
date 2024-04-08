using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MyGrid<T> : MonoBehaviour
{

    [SerializeField] private int height;
    [SerializeField] private int width;
    private T[,] gridArray;

    public MyGrid(int height, int width, T defaultcell)
    {
        CreateGrid(height, width, defaultcell);
    }


    public int Height()
    {
        return height;
    }

    public int Width()
    {
        return width;
    }

    public T GetCell(int x, int y)
    {
        return gridArray[x, y];
    }

    public void CreateGrid(int height, int width, T defaultcell)
    {
        this.height = height;
        this.width = width;
        gridArray = new T[height, width];

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                gridArray[i, j] = defaultcell;
            }
        }

    }

    public bool IsInBounds(int x, int y)
    {
        if (x < 0 || y < 0) return false;
        if (x > width || y > height) return false;
        return true;
    }

    public void SetCell(T cell, int x, int y)
    {
        gridArray[x, y] = cell;
    }

    public T[] GetNeighbors(int x, int y, int range)
    {
        return GetNeighbors(x, y, range, range);
    }

    private T[] GetNeighbors(int xCordinate, int yCordinate, int xEndOffset = 1, int yEndOffset = 1)
    {
        List<T> neighbourCells = new List<T>();

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
                Debug.Log(x + " , " + y);
            }
        }


        T[] hola = neighbourCells.ToArray();
        Debug.Log(hola.Length);
        return hola;
    }

    public T[] GetMovement(int x, int y, int move)
    {
        return GetMovementRec(x, y, move);
    }

    private List<T> GetMovementRec(int x, int y, int range) 
    {
        T[] end = new T[0];

        if (range == 0)
        {
            end.Append(gridArray[x, y]);
            return end;
        }

        else
        {
            end GetMovementRec(x + 1, y, range - 1);
        }

    }

}

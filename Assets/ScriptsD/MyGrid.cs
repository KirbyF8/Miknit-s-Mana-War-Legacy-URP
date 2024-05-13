using AStar;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class MyGrid : MonoBehaviour
{

    //altura y anchura del mapa
    [SerializeField] private int height;
    [SerializeField] private int width;

    //Matriz de cells que es el mapa en si
    private MyCell[,] gridArray;

    //posiciones en los que los aliados pueden posicionarse al principio de la batalla
    [SerializeField] private Vector2[] spawnPositions;

    //los enemigos del mapa y sus correspondientes posiciones
    [SerializeField] private CharacterD[] enemies;
    [SerializeField] private Vector2[] enemiesPositions;

    //matriz de booleanos para registrar por donde se puede caminar y por donde no
    private bool[,] walkableMap;

    //array con los SO de las cells que va a haber en ese mapa y array que después llenaremos con las mismas cells pero pasadas de CellCreatorSO a MyCell
    [SerializeField] private CellCreatorSO[] cellsAux;
    private MyCell[] cells;

    //arrays con el mismo tamaño, cellsToChange tiene las cells que tienen que ser diferentes a la cell por predeterminado (cellsAux[0])
    //y numberOfCell tiene la casilla por la que hay que cambiar cada casilla de cellsToChange
    [SerializeField] private Vector2[] cellsToChange;
    [SerializeField] private int[] numberOfCell;
    private void Start()
    {

        //Al crearse un mapa se pasan las cellsSO del array cellsAux a cells normales que guardamos en otro array del mismo tamaño.

        cells = new MyCell[cellsAux.Length];
        for (int i = 0; i < cellsAux.Length; i++)
        {
            cells[i] = new MyCell();
            cells[i].SetWalkable(cellsAux[i].walkable);
            cells[i].SetEvasion(cellsAux[i].evasionBuff);
            cells[i].SetDefence(cellsAux[i].defenceBuff);
            cells[i].SetDifficulty(cellsAux[i].difficulty);
        }

        //Se crea la grid con la height y width introducidas por inspector

        CreateGrid(height, width);

        //Se colocan todos los enemigos en la posición que les corresponde de enemiesPositions.

        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].SetPosition(enemiesPositions[i]);
            gridArray[(int)enemiesPositions[i].x, (int)enemiesPositions[i].y].SetCharacter(enemies[i]);
        }
        
    }

    //Constructor de grid (ELIMINAR?)

    public MyGrid(int height, int width)
    {
        CreateGrid(height, width);
    }


    //Funciones para leer y escribir las variables de la grid

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

    public CharacterD[] GetEnemies()
    {
        return enemies;
    }

    public void SetEnemies(CharacterD[] x)
    {
        enemies = x;
    }

    //Función para crear la grid

    public void CreateGrid(int height, int width)
    {

        //primero se ponen la height y la width en las variables del objeto y se crea la matriz de cells y de bools con esas dos variables

        this.height = height;
        this.width = width;
        gridArray = new MyCell[height, width];
        walkableMap = new bool[height,width];

        //Se crean todas las cells y se ponen como la cell predeterminada

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

        //Ahora se revisa todo el array de CellsToChange y se cambia cada cell por la cell que indica numberOfCell

        for (int i = 0;i<cellsToChange.Length; i++)
        {
            gridArray[(int)cellsToChange[i].x, (int)cellsToChange[i].y].Copy(cells[numberOfCell[i]]);
            if (!gridArray[(int)cellsToChange[i].x, (int)cellsToChange[i].y].GetWalkable()) walkableMap[(int)cellsToChange[i].x, (int)cellsToChange[i].y] = false;

        }
    }

    //Función para revisar si la posición indicada corresponde a una casilla del mapa
    public bool IsInBounds(int x, int y)
    {
        if (x < 0 || y < 0) return false;
        if (x > width || y > height) return false;
        return true;
    }

    
    
    
    //Función que devuelve todas las casillas adyacentes a la casilla (x,y) hasta el rango indicado
    public MyCell[] GetNeighbors(int x, int y, int range = 1, int range2 = 0)
    {
        if(range2 == 0) return GetNeighborsa(x, y, range, range);
        else return GetNeighborsa(x, y, range, range2);
    }


    //no me gusta nada esta función peeero la pillé de un asset diferente y me ahorré el pensar cómo hacerla, tampoco creo que vayamos a usarla mucho
    private MyCell[] GetNeighborsa(int xCordinate, int yCordinate, int xEndOffset, int yEndOffset)
    {

        List<MyCell> neighbourCells = new List<MyCell>();

        //yEnd y xEnd son el máximo de rango al que va a llegar la lista de su respectivo eje, sin pasarse de el límite del mapa.
        int yEnd = (int)MathF.Min(height - 1, yCordinate + yEndOffset);
        int xEnd = (int)MathF.Min(width - 1, xCordinate + xEndOffset);

        //bucle que va a revisar toda la grid, y si la cell está en el rango que se busca la pone en la lista
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

        //transforma la lista en array y la devuelve
        MyCell[] tempArray = neighbourCells.ToArray();
        return tempArray;
    }

    //Función que devuelve una lista con todas las cells a las que se puede mover un personaje que está en (x,y) y tiene movimiento move
    public List<Vector2> GetMovement(int x, int y, int move)
    {
        List<Vector2> end = new List<Vector2>();
        GetMovementRec(x, y, move, ref end);
        return end;
    }

    //Función recursiva para hacer la anterior.
    private void GetMovementRec(int x, int y, int range, ref List<Vector2> end) 
    {
        //Si no está dentro de la grid, si ya está dentro de la lista, o si no se puede caminar por la casilla seleccionada vuelve
        if (!IsInBounds(x,y)) return;
        bool thereIs = end.Contains(new Vector2(x, y));
        int diff = gridArray[x,y].GetDifficulty();
        if (!gridArray[x, y].GetWalkable()) return;

        //si hay un enemigo se añade al array pero se pone el rango a 0 para que no pueda atravesarlo, pero salga una casilla en rojo debajo del enemigo
        if (gridArray[x, y].GetCharacter() != null && (gridArray[x, y].GetCharacter().GetSide() > 0 || gridArray[x, y].GetCharacter().GetSide() < 0)) range = 0; 

        //si el rango es menor o igual a zero se añade la posición y ya,
        //si es mayor a zero se añade y además vuelve a ejecutar la función con cada casilla adyacente y con rango menor dependiendo de la dificultad de movimiento
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

    //Función que devuelve el array con las posiciones por las que tiene que ir el personaje para llegar hasta el objetivo
    public (int, int)[] FindPath(Vector2 start, Vector2 goal)
    {
        //se pone a false en el array de booleanos todas las posiciones donde haya enemigos
        for(int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                walkableMap[(int)enemies[i].GetPosition().x, (int)enemies[i].GetPosition().y] = false;
            }
        }
        //pasa a la función que encuentra el camino
        List <(int, int)> aux = PathFinding(start, goal, gridArray[(int)start.x, (int) start.y].GetCharacter().GetMovement());

        //vuelve a poner las casillas con enemigos a true (para que el get movement no detecte los enemigos como paredes)
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                walkableMap[(int)enemies[i].GetPosition().x, (int)enemies[i].GetPosition().y] = true;
            }
        }
        
        return aux.ToArray();

    }

    //función que devuelve el camino (TOCHO INCOMING)
    private List<(int, int)> PathFinding(Vector2 start, Vector2 goal, int range)
    {
        // si start es goal devuelve una lista con la goal 
        if (start == goal) return new List<(int, int)> { ((int)start.x, (int)start.y) };
        
        //si start no está en bounds o si es una pared devuelve nulo
        else if (!IsInBounds((int)start.x, (int)start.y)) return null;
        else if (!walkableMap[(int)start.x, (int)start.y]) return null;

        //si se agota el movimiento devuelve nulo
        else if (range <= 0) return null;

        

        else
        {
            int diff = gridArray[(int)start.x, (int)start.y].GetDifficulty();
            //comprueba si la meta está arriba, abajo, o en la misma linea que start.
            //Si está más arriba se ejecutará la misma función pero con la casilla superior,
            //si esa no devuelve una respuesta (nulo) comprobará si la meta está a la izquierda o a la derecha del start,
            //hace la función primero hacia donde esté la meta, después hacia el otro lado,
            //y finmalmente hacia abajo, si alguno devuelve una respuesta sale de la función con esa respuesta,
            //si nadie tiene respuesta devuelve nulo
            //Si está más arriba el orden es el contrario, primero revisa abajo, después los lados, y después arriba.
            //Finalmente si está en la misma línea primero comprobará los lados, y si no hay respuesta válida comprobará arriba y abajo.
            if (start.y < goal.y)
            {
                List<(int, int)> possible = PathFinding(new Vector2(start.x, start.y + 1), goal, range - diff);
                if (possible != null)
                {
                    possible.Insert(0,((int)start.x, (int)start.y));
                    return possible;
                }
                else
                {
                    if (start.x < goal.x)
                    {
                        possible = PathFinding(new Vector2(start.x + 1, start.y), goal, range - diff);
                        if (possible != null)
                        {
                            possible.Insert(0, ((int)start.x, (int)start.y));
                            return possible;
                        }
                        possible = PathFinding(new Vector2(start.x - 1, start.y), goal, range - diff);
                        if (possible != null)
                        {
                            possible.Insert(0, ((int)start.x, (int)start.y));
                            return possible;
                        }
                    }
                    else
                    {
                        possible = PathFinding(new Vector2(start.x - 1, start.y), goal, range - diff);
                        if (possible != null)
                        {
                            possible.Insert(0, ((int)start.x, (int)start.y));
                            return possible;
                        }
                        possible = PathFinding(new Vector2(start.x + 1, start.y), goal, range - diff);
                        if (possible != null)
                        {
                            possible.Insert(0, ((int)start.x, (int)start.y));
                            return possible;
                        }
                    }
                    possible = PathFinding(new Vector2(start.x, start.y - 1), goal, range - diff);
                    if (possible != null)
                    {
                        possible.Insert(0, ((int)start.x, (int)start.y));
                        return possible;
                    }
                }
            }

            else if(start.y > goal.y) 
            {
                List<(int, int)> possible = PathFinding(new Vector2(start.x, start.y - 1), goal, range - diff);
                if (possible != null)
                {
                    possible.Insert(0, ((int)start.x, (int)start.y));
                    return possible;
                }
                else
                {
                    if (start.x < goal.x)
                    {
                        possible = PathFinding(new Vector2(start.x + 1, start.y), goal, range - diff);
                        if (possible != null)
                        {
                            possible.Insert(0, ((int)start.x, (int)start.y));
                            return possible;
                        }
                        possible = PathFinding(new Vector2(start.x - 1, start.y), goal, range - diff);
                        if (possible != null)
                        {
                            possible.Insert(0, ((int)start.x, (int)start.y));
                            return possible;
                        }
                    }
                    else
                    {
                        possible = PathFinding(new Vector2(start.x - 1, start.y), goal, range - diff);
                        if (possible != null)
                        {
                            possible.Insert(0, ((int)start.x, (int)start.y));
                            return possible;
                        }
                        possible = PathFinding(new Vector2(start.x + 1, start.y), goal, range - diff);
                        if (possible != null)
                        {
                            possible.Insert(0, ((int)start.x, (int)start.y));
                            return possible;
                        }
                    }
                    possible = PathFinding(new Vector2(start.x, start.y + 1), goal, range - diff);
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
                    possible = PathFinding(new Vector2(start.x + 1, start.y), goal, range - diff);
                    if (possible != null)
                    {
                        possible.Insert(0, ((int)start.x, (int)start.y));
                        return possible;
                    }
                    possible = PathFinding(new Vector2(start.x, start.y + 1), goal, range - diff);
                    if (possible != null)
                    {
                        possible.Insert(0, ((int)start.x, (int)start.y));
                        return possible;
                    }
                    possible = PathFinding(new Vector2(start.x, start.y - 1), goal, range - diff);
                    if (possible != null)
                    {
                        possible.Insert(0, ((int)start.x, (int)start.y));
                        return possible;
                    }
                    possible = PathFinding(new Vector2(start.x - 1, start.y), goal, range - diff);
                    if (possible != null)
                    {
                        possible.Insert(0, ((int)start.x, (int)start.y));
                        return possible;
                    }
                }
                else
                {
                    possible = PathFinding(new Vector2(start.x - 1, start.y), goal, range - diff);
                    if (possible != null)
                    {
                        possible.Insert(0, ((int)start.x, (int)start.y));
                        return possible;
                    }
                    possible = PathFinding(new Vector2(start.x, start.y + 1), goal, range - diff);
                    if (possible != null)
                    {
                        possible.Insert(0, ((int)start.x, (int)start.y));
                        return possible;
                    }
                    possible = PathFinding(new Vector2(start.x, start.y - 1), goal, range - diff);
                    if (possible != null)
                    {
                        possible.Insert(0, ((int)start.x, (int)start.y));
                        return possible;
                    }
                    possible = PathFinding(new Vector2(start.x + 1, start.y), goal, range - diff);
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

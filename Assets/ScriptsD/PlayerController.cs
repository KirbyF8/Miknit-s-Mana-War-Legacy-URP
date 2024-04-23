using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;

public class PlayerController : MonoBehaviour
{
    private RaycastHit[] raycastHits;
    private Ray ray;

    private Vector3 position;


    private Vector3 point;
    private int scale = 2;

    private bool spawnPhase = true;
    private bool movementPhase = false;

    private MyGrid map;
    private Vector2[] spawnPos;
    [SerializeField] private CharacterD[] characters;

    private MyCell selected;
    private MyCell selectedAux;
    private CharacterD charSelected;
    [SerializeField] private GameObject alliesTile;
    [SerializeField] private GameObject enemiesTile;

    private Vector2[] movementArray;

    private bool isASpawn = false;

    private bool moving = false;

    void Start()
    {
        map = FindObjectOfType<MyGrid>();
        spawnPos = map.GetSpawn();
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].SetPosition((int)spawnPos[i].x, (int)spawnPos[i].y);
            map.GetCell((int)spawnPos[i].x, (int)spawnPos[i].y).SetCharacter(characters[i]);
        }

        SpawnTiles(spawnPos);

    }

    void Update()
    {
        if (spawnPhase)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ShootRay();
                if (raycastHits.Length != 0)
                {
                    GetPosition(raycastHits[0]);

                    CheckSpawn(new Vector2(point.x, point.z));

                    if (selected == null && isASpawn)
                    {
                        selected = map.GetCell((int)point.x, (int)point.z);
                    }
                    else if (isASpawn)
                    {
                        selectedAux = map.GetCell((int)point.x, (int)point.z);
                        if (selected.GetCharacter() != null || selectedAux.GetCharacter() != null && selected != selectedAux)
                        {
                            ExchangePos(selected, selectedAux);
                        }
                        selected = null;
                    }
                    else
                    {
                        selected = null;
                    }
                    isASpawn = false;
                }
            }
        }
        else if (movementPhase)
        {
            if (Input.GetMouseButtonDown(0) && !moving)
            {
                ShootRay();
                if (raycastHits.Length != 0)
                {
                    if (selected != null) DeleteTiles();
                    GetPosition(raycastHits[0]);
                    selected = map.GetCell((int)point.x, (int)point.z);
                    if(selected.GetCharacter() != null)
                    {
                        movementArray = map.GetMovement((int)point.x, (int)point.z, selected.GetCharacter().GetMovement()).ToArray();
                        SpawnTiles(movementArray);
                    }
                    else
                    {
                        Debug.Log(selected.GetWalkable() + ", " + selected.GetDifficulty());
                        //HAY QUE CAMBIAR ESTO POR UN PANEL QUE DE INFO DEL TERRENO 
                    }
                }
            }
            else if (Input.GetMouseButtonDown(1) && !moving)
            {
                ShootRay();
                if (raycastHits.Length != 0)
                {
                    GetPosition(raycastHits[0]);
                    selectedAux = map.GetCell((int)point.x, (int)point.z);
                    if (selected != null && selected.GetCharacter() != null)
                    {
                        if(selectedAux.GetCharacter()!= null && selectedAux.GetCharacter().GetSide() !=0)
                        {
                            ExchangePos(selected, map.GetCell(NearbyTile(selected.GetPosition(), selectedAux.GetPosition())));
                            Debug.Log("attack");
                            DeleteTiles();
                        }
                        else if (selectedAux.GetCharacter() == null && CheckMove())
                        {
                            MoveChar(selected, selectedAux);
                            selected = selectedAux = null;
                            DeleteTiles();
                        }
                    }
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            EndSpawnPhase();
        }

        /*
        if (Input.GetMouseButtonDown(1))
        {
            position = Input.mousePosition;
            position.z = Camera.main.nearClipPlane;
            ray = Camera.main.ScreenPointToRay(position);
            raycastHits = Physics.RaycastAll(ray);
            if (raycastHits.Length != 0)
            {
                point = raycastHits[0].point;
                point.x = ((int)point.x);
                point.z = ((int)point.z);
                point /= 2;
                point.x = ((int)point.x);
                point.z = ((int)point.z);
                point.y = 0;
                point.z = Mathf.Abs(point.z);
            }
            Debug.DrawRay(transform.position, ray.direction * 100000, Color.red);
            show = true;
        }

        if (show) Debug.DrawRay(transform.position, ray.direction * 100000, Color.red);*/

    }

    private bool CheckMove()
    {
        bool aux = false;
        Vector2 secondpoint = new Vector2 (point.x, point.z);
        for (int i = 0; i<movementArray.Length && !aux; i++)
        {
            if(secondpoint == movementArray[i]) { aux = true;}
        }
        return aux;
    }

    private bool CheckMove(Vector2 target)
    {
        bool aux = false;
        for (int i = 0; i < movementArray.Length && !aux; i++)
        {
            if (target == movementArray[i]) { aux = true; }
        }
        return aux;
    }

    private Vector2 NearbyTile(Vector2 attackerPos, Vector2 defenderPos)
    {
        Vector2[] aux = new Vector2[4];
        int x = 0;
        int score = -1;
        int scoreaux;
        aux[0] = new Vector2(defenderPos.x + 1, defenderPos.y);
        aux[1] = new Vector2(defenderPos.x - 1, defenderPos.y);
        aux[2] = new Vector2(defenderPos.x, defenderPos.y + 1);
        aux[3] = new Vector2(defenderPos.x, defenderPos.y - 1);
        for (int i = 0; i < aux.Length; i++)
        {
            if (CheckMove(aux[i]))
            {

                scoreaux = ((Mathf.Abs((int)attackerPos.x - (int)aux[i].x)) + (Mathf.Abs((int)attackerPos.y - (int)aux[i].y)));

                if (scoreaux < score || score == -1)
                {
                    score = scoreaux;
                    x = i;
                }
            }
        }


        return aux[x];
    }

    private void SpawnTiles(Vector2[] pos)
    {
        for (int i = 0; i < pos.Length; i++)
        {
            if(spawnPhase) Instantiate(alliesTile, new Vector3(pos[i].x * scale + 1, 0.1f, -pos[i].y * scale - 1), Quaternion.identity);
            else
            {
                if (map.GetCell((int)pos[i].x, (int)pos[i].y).GetCharacter() == null)
                {
                    Instantiate(alliesTile, new Vector3(pos[i].x * scale + 1, 0.1f, -pos[i].y * scale - 1), Quaternion.identity);
                }
                else if (map.GetCell((int)pos[i].x, (int)pos[i].y).GetCharacter().GetSide() > 0)
                {
                    Instantiate(enemiesTile, new Vector3(pos[i].x * scale + 1, 0.1f, -pos[i].y * scale - 1), Quaternion.identity);
                }
            }
            
        }
    }
    
    private void DeleteTiles()
    {
        GameObject[] destroying = GameObject.FindGameObjectsWithTag("Tile_mark");


        for (int i = 0; i < destroying.Length; i++)
        {
            Destroy(destroying[i]);
        }
    }

    private void EndSpawnPhase()
    {
        spawnPhase = false;

        DeleteTiles();

        movementPhase = true;
    }

    private void ShootRay()
    {
        position = Input.mousePosition;
        position.z = Camera.main.nearClipPlane;
        ray = Camera.main.ScreenPointToRay(position);
        raycastHits = Physics.RaycastAll(ray);
    }

    private void CheckSpawn( Vector2 aux)
    {
        for (int i = 0; i < spawnPos.Length && !isASpawn; i++)
        {

            if (aux == spawnPos[i]) isASpawn = true;
        }
    }

    private void GetPosition(RaycastHit hit)
    {
        point = hit.point;
        point.x = ((int)point.x);
        point.z = ((int)point.z);
        point /= 2;
        point.x = ((int)point.x);
        point.z = ((int)point.z);
        point.y = 0;
        point.z = Mathf.Abs(point.z);
    }

    private void ExchangePos(MyCell cell1, MyCell cell2)
    {
        charSelected = cell1.GetCharacter();
        CharacterD charSelectedAux = cell2.GetCharacter();
        cell2.SetCharacter(charSelected);
        cell1.SetCharacter(charSelectedAux);
        
        if (charSelectedAux != null) charSelectedAux.SetPosition(cell1);
        if (charSelected != null)
        {
            charSelected.SetPosition(cell2);
        }
        
    }

    private void MoveChar(MyCell cell1, MyCell cell2)
    {
        (int,int)[] aux = map.FindPath(cell1.GetPosition(), cell2.GetPosition());
        StartCoroutine(MovingChar(aux));
        return;

    }


    private IEnumerator MovingChar((int, int)[] path)
    {
        CharacterD selectedChar = map.GetCell(path[0].Item1, path[0].Item2).GetCharacter();
        map.GetCell(path[0].Item1, path[0].Item2).SetCharacter(null);
        moving = true;
        for(int i = 1; i < path.Length; i++)
        {
            selectedChar.SetPosition(path[i].Item1, path[i].Item2);
            yield return new WaitForSeconds(0.5f);
        }
        map.GetCell(path[path.Length-1].Item1, path[path.Length - 1].Item2).SetCharacter(selectedChar);
        moving = false;
    }

}

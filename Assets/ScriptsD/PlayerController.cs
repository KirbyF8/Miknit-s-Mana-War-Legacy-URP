using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private RaycastHit[] raycastHits;
    private Ray ray;

    private Vector3 position;

    private bool show = false;

    private int x;
    private int y;

    private Vector3 point;

    private bool spawnPhase = true;

    private MyGrid map;
    private Vector2[] spawnPos;
    [SerializeField] private CharacterD[] characters;
    private Collider mapCollider;

    private MyCell selected;
    private MyCell selectedAux;
    private CharacterD charSelected;

    private bool isASpawn = false;

    void Start()
    {
        map = FindObjectOfType<MyGrid>();
        spawnPos = map.GetSpawn();
        mapCollider = map.GetComponent<Collider>();
        MyCell cell = new MyCell();
        cell.SetWalkable(true);
        map.CreateGrid(20, 20, cell);
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].SetPosition((int)spawnPos[i].x, (int)spawnPos[i].x);
            map.GetCell((int)spawnPos[i].x, (int)spawnPos[i].x).SetCharacter(characters[i]);
        }
        
    }

    void Update()
    {

        if (spawnPhase)
        {
            if (Input.GetMouseButtonDown(0))
            {
                position = Input.mousePosition;
                position.z = Camera.main.nearClipPlane;
                ray = Camera.main.ScreenPointToRay(position);
                raycastHits = Physics.RaycastAll(ray);
                if (raycastHits.Length != 0)
                {
                    GetPosition(raycastHits[0]);
                    Vector2 aux = new Vector2(point.x, point.z);
                    Debug.Log(isASpawn);
                    for (int i = 0; i < spawnPos.Length && !isASpawn; i++)
                    {

                        if (aux == spawnPos[i]) isASpawn = true;
                    }

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

}

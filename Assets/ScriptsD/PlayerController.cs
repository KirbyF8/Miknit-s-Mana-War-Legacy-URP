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

    void Start()
    {
        
    }

    void Update()
    {

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

        if (show) Debug.DrawRay(transform.position, ray.direction * 100000, Color.red);

    }

}

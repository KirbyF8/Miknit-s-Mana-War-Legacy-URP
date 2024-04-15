using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    private float verticalInput;
    private float horizontalInput;
    private Vector3 movementVector;

    // Update is called once per frame
    void Update()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
        movementVector = verticalInput * Vector3.forward + horizontalInput * Vector3.right;
        movementVector.Normalize();
        transform.Translate(movementVector * Time.deltaTime * moveSpeed);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            transform.Rotate(Vector3.up, rotationSpeed);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            transform.Rotate(Vector3.up, -rotationSpeed);
        }
    }

    private void Start()
    {
        MyCell cell = new MyCell();
        cell.SetWalkable(true);
        MyGrid grid = new MyGrid(10,10,cell);
        MyCell cell2 = grid.GetCell(6, 5);
        cell2.SetWalkable(false);
        List<Vector2> ey= grid.GetMovement(5, 5, 5);

        for (int i = 0; i < ey.Count; i++)
        {
            Debug.Log(ey[i].x + ", " + ey[i].y);
        }
        Debug.Log(ey.Count);

    }
}

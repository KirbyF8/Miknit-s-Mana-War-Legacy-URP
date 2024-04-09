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
        Cell cell = new Cell();
        MyGrid<Cell> grid = new MyGrid<Cell>(10,10,cell);
        grid.GetNeighbors(0, 0, 2);
    }
}

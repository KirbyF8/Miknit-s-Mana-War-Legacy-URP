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


    void Update()
    {

        //Leemos los inputs vertical y horizontal y creamos el vector de movimiento

        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
        movementVector = verticalInput * Vector3.forward + horizontalInput * Vector3.right;
        movementVector.Normalize();

        //Movemos la camara en la dirección del vector antes creado

        transform.Translate(movementVector * Time.deltaTime * moveSpeed);

        //Leemos los inputs de la Q y la E y rotamos en una dirección u otra si pulsan alguno.

        if (Input.GetKeyDown(KeyCode.Q))
        {
            transform.Rotate(Vector3.up, rotationSpeed);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            transform.Rotate(Vector3.up, -rotationSpeed);
        }
    }

}

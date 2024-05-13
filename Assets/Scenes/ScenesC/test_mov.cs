using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class test_mov : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(1,0,0) * hInput * 3 * Time.deltaTime);
        transform.Translate(new Vector3(0,0, 1) * vInput * 3 * Time.deltaTime);

    }
}

using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MainCharacterConfig : MonoBehaviour
{
    [SerializeField] private Material material;
    [SerializeField] private Material weaponMaterial;

    [SerializeField] private Slider sliderRed;
    [SerializeField] private Slider sliderGreen;
    [SerializeField] private Slider sliderBlue;

    public float red;
    public float blue;
    public float green;

    private string RED = "cuantityRed";

    [SerializeField] private TMP_Dropdown dropdown;
    private float x = 902.7f;
    private float y = 591.2f;
    private float z = 1923.45f;

    [SerializeField] private GameObject[] Characters;
    

    private int actualClass = 0;
    void Start()
    {
        red = 0; blue = 0; green = 0;

        
    }

    public void ChangeColorRed()
    {
        red = sliderRed.value;
        ColorChange();
    }

    public void ChangeColorBlue()
    {
        blue = sliderBlue.value;
        ColorChange();
    }

    public void ChangeColorGreen()
    {
        green = sliderGreen.value;
        ColorChange();
    }
    
    private void ColorChange()
    {
      

        // material.SetColor("_Color", new Color(red, green, blue));
        
        material.color = new Color(red, green, blue);
        weaponMaterial.color = new Color(red, green, blue);

        // weaponMaterial.SetColor("_Color", new Color(red, green, blue));
      

    }
   


    public void ClassChange()
    {
        // Characters[actualClass]

    }
}

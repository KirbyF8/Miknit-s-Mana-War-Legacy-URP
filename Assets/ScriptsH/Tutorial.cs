using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{

    [SerializeField] GameObject textBox;
    [SerializeField] TextMeshProUGUI text;

    private string tutorialText = "Pulsa clic izquierdo para seleccionar una unidad, después de eso pulsa clic derecho para moverla a una casilla en rango, si en esa casilla hay un enemigo una batalla comenzará";

    void Start()
    {
        textBox.SetActive(true);
        StartCoroutine("UpdateText");
    }

    private void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            StopCoroutine("UpdateText");
            FullText();
        }

        if (text.text == tutorialText)
        {
            StartCoroutine("HideTextBubble");
        }
    }


    private IEnumerator UpdateText()
    {
        
     yield return new WaitForSeconds(0.5f);
        
        text.text = "";
        foreach (char c in tutorialText)
        {
            text.text += c;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void FullText()
    {
        
        text.text = tutorialText;
        Debug.Log(text.text);
        Debug.Log(tutorialText);

    }

    private IEnumerator HideTextBubble()
    {
        yield return new WaitForSeconds(2f);
        textBox.SetActive(false);
        yield return new WaitForSeconds(0.1f);
    }
}

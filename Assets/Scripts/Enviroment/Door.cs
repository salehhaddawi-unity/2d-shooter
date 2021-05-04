using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int price = 750;

    public TextMesh text;

    void Start()
    {
        if (text == null)
        {
            text = GetComponentInChildren<TextMesh>();
        }

        text.text = price.ToString();
    }

    private void OnGUI()
    {
        
    }

    private void OnMouseOver()
    {
        Debug.Log(999);
    }

    private void OnMouseDown()
    {
        Debug.Log(44444);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

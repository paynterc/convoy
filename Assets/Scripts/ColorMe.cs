using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorMe : MonoBehaviour
{
    public Color colorStart = Color.red;
    Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material.color = colorStart;
    }

    void Update()
    {


    }
}

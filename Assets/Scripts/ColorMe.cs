using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorMe : MonoBehaviour
{
    public Color colorStart = Color.red;
    Renderer rend;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        rend.material.color = colorStart;
    }

}

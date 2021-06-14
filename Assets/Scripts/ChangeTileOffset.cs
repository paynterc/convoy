﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTileOffset : MonoBehaviour
{
    public float xx=0;
    public float yy=0;

    // Start is called before the first frame update
    void Awake()
    {
        GetComponent<Renderer>().material.mainTextureOffset = new Vector2(xx, yy);
    }


}

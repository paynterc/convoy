using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateVertsStart : MonoBehaviour
{
    public Color newColor = Color.gray;
    // Start is called before the first frame update
    void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Color[] colors = new Color[mesh.vertices.Length];
        int i = 0;
        while (i < mesh.vertices.Length)
        {
            colors[i] = newColor;
            i++;
        }
        mesh.colors = colors;
    }

}

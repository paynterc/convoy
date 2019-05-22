using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class UpdateVerts : MonoBehaviour
{
    public Color newColor = Color.gray;
    // Start is called before the first frame update
    void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;

        // create new colors array where the colors will be created.
        Color[] colors = new Color[vertices.Length];

        for (int i = 0; i < vertices.Length; i++)
            colors[i] = Color.Lerp(Color.red, Color.green, vertices[i].y);

        // assign the array of colors to the Mesh.
        mesh.colors = colors;
    }
    // Use a vertex color shader and a vertexMaterial
    // Update is called once per frame
    void Update()
    {
        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        Color[] colors = new Color[mesh.vertices.Length];
        int i = 0;
        while (i<mesh.vertices.Length)
        {
            colors[i] = newColor;
            i++;
        }
        mesh.colors = colors;
    }
}

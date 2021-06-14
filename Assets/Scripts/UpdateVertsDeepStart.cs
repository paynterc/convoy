using UnityEngine;
public class UpdateVertsDeepStart : MonoBehaviour
{
    public Color newColor = Color.gray;
    // Start is called before the first frame update
    void Awake()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        for (int j = 0; j < meshFilters.Length; j++)
        {
            Mesh mesh = meshFilters[j].mesh;
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

}
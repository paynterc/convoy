using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class CombineMeshes : MonoBehaviour
{
    // https://www.habrador.com/tutorials/unity-mesh-combining-tutorial/3-combine-meshes-colors/

    // Start is called before the first frame update
    void Start()
    {

        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        for (int j=0;j<meshFilters.Length;j++)
        {
            if (meshFilters[j].mesh!=null)
            {
                combine[i].mesh = meshFilters[j].sharedMesh;
                combine[i].transform = meshFilters[j].transform.localToWorldMatrix;
                meshFilters[j].gameObject.SetActive(false);

                i++;
            }
        }

        transform.GetComponent<MeshFilter>().mesh = new Mesh();
        transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        //MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        //meshRenderer.material = material;
        transform.position = new Vector3(0, 0, 0);

        transform.gameObject.SetActive(true);
    }

}

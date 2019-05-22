using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Combine trees with different materials into one mesh
public class CombineMeshesClr : MonoBehaviour
{
    public Material color1;
    public Material color2;
    public Material color3;// Default

    void Start()
    {
        CombineMeshes();
    }

    // Similar to Unity's reference, but with different materials
    // https://www.habrador.com/tutorials/unity-mesh-combining-tutorial/3-combine-meshes-colors/
    // http://docs.unity3d.com/ScriptReference/Mesh.CombineMeshes.html
    void CombineMeshes()
    {
        //Lists that holds mesh data that belongs to each submesh
        List<CombineInstance> clrList1 = new List<CombineInstance>();
        List<CombineInstance> clrList2 = new List<CombineInstance>();
        List<CombineInstance> clrList3 = new List<CombineInstance>();

        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();

        //Loop through all children
        for (int j = 0; j < meshFilters.Length; j++)
        {
            MeshFilter meshFilter = meshFilters[j];
            CombineInstance combine = new CombineInstance();
            MeshRenderer meshRender = meshFilter.GetComponent<MeshRenderer>();

            combine.mesh = meshFilter.mesh;
            combine.transform = meshFilter.transform.localToWorldMatrix;

            if (meshRender.material == color1)
            {
                clrList1.Add(combine);
            }
            else if (meshRender.material == color2)
            {
                clrList2.Add(combine);
            }
            else
            {
                meshRender.material = color3;
                clrList3.Add(combine);
            }
            meshFilters[j].gameObject.SetActive(false);
        }
        


        //First we need to combine the wood into one mesh and then the leaf into one mesh
        Mesh combinedColor1 = new Mesh();
        combinedColor1.CombineMeshes(clrList1.ToArray());

        Mesh combinedColor2 = new Mesh();
        combinedColor2.CombineMeshes(clrList2.ToArray());

        Mesh combinedColor3 = new Mesh();
        combinedColor3.CombineMeshes(clrList3.ToArray());

        //Create the array that will form the combined mesh
        CombineInstance[] totalMesh = new CombineInstance[3];

        //Add the submeshes in the same order as the material is set in the combined mesh
        totalMesh[0].mesh = combinedColor1;
        totalMesh[0].transform = transform.localToWorldMatrix;
        totalMesh[1].mesh = combinedColor2;
        totalMesh[1].transform = transform.localToWorldMatrix;
        totalMesh[2].mesh = combinedColor3;
        totalMesh[2].transform = transform.localToWorldMatrix;


        //Create the final combined mesh
        Mesh combinedAllMesh = new Mesh();
       
        //Make sure it's set to false to get 3 separate meshes
        combinedAllMesh.CombineMeshes(totalMesh, false);
        transform.GetComponent<MeshFilter>().mesh = combinedColor1;
        transform.position = new Vector3(0, 0, 0);
        transform.gameObject.SetActive(true);
    }
}
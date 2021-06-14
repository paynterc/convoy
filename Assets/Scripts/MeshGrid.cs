using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartCombine;

public class MeshTile : MonoBehaviour {
    public Mesh mesh;
    public Material[] materials;
}

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MeshGrid : MonoBehaviour {
    public MeshTile[] meshTiles;
    SmartMeshData[] meshData;

    void Awake() {
        meshData = new SmartMeshData[meshTiles.Length];
        for (int i = 0; i < meshTiles.Length; i++) {
            meshData[i] = new SmartMeshData(meshTiles[i].mesh, meshTiles[i].materials, meshTiles[i].transform.localPosition, meshTiles[i].transform.localRotation);
        }

        Mesh combinedMesh = new Mesh();
        combinedMesh.name = "Combined Mesh";
        Material[] combinedMaterials;

        combinedMesh.CombineMeshesSmart(meshData, out combinedMaterials);
        transform.GetComponent<MeshFilter>().mesh = combinedMesh;
        GetComponent<MeshRenderer>().sharedMaterials = combinedMaterials;
        
    }
}
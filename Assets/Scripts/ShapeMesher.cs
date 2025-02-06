using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ShapeMesher : MonoBehaviour
{
    [SerializeField]
    private Color fillColor = new Color(0.3632075f, 0.8306069f, 1.0f, 1.0f);
    private Material fillMaterial;

    void Start()
    {
        fillMaterial = Resources.Load("pool_material", typeof(Material)) as Material;
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = fillMaterial;

        fillMaterial.color = fillColor;

        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(vertices);

        int[] triangles = new int[(vertices.Length - 2) * 3];
        for (int i = 0; i < vertices.Length - 2; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        meshFilter.mesh = mesh;
    }
}
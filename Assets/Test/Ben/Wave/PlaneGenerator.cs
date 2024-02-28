using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PlaneGenerator : MonoBehaviour
{
	[SerializeField] private int xSize;
	[SerializeField] private int zSize;

	[Space]
	[SerializeField] private bool generatePlane = false;

	private Mesh mesh;

#if UNITY_EDITOR
	private void OnValidate()
    {
		EditorApplication.delayCall += () => Generate();
		generatePlane = false;
    }
#endif

	private void Generate()
	{
		mesh = new Mesh();
		mesh.name = "Procedural Grid";

		MeshFilter meshFilter;
		bool valid = TryGetComponent(out meshFilter);

		if (!valid) return;
		else meshFilter.mesh = mesh;

		Vector3[] vertices = new Vector3[(xSize + 1) * (zSize + 1)];
		Vector2[] uv = new Vector2[vertices.Length];
		Vector4[] tangents = new Vector4[vertices.Length];
		Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);

		int i = 0;
		for (float z = -zSize / 2f; z <= zSize / 2f; z++)
		{
			for (float x = -xSize / 2f; x <= xSize / 2f; x++, i++)
			{
				vertices[i] = new Vector3(x, 0, z);
				uv[i] = new Vector2((float)x / xSize, (float)z / zSize);
				tangents[i] = tangent;
			}
		}
		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.tangents = tangents;


		int[] triangles = new int[xSize * zSize * 6];
		int ti = 0, vi = 0;
		for (int z = 0; z < zSize; z++, vi++)
		{
			for (int x = 0; x < xSize ; x++, ti += 6, vi++)
			{
				triangles[ti] = vi;
				triangles[ti + 3] = triangles[ti + 2] = vi + 1;
				triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
				triangles[ti + 5] = vi + xSize + 2;
			}
		}
		mesh.triangles = triangles;

		mesh.RecalculateNormals();
		mesh.RecalculateTangents();

		Debug.Log("Generated Plane");
	}
}
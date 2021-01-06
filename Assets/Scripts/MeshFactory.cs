using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshFactory
{
	public static bool GetQuadMesh(int width, int height, out Mesh quadMesh)
	{
		// Unity 默认的单个Mesh的顶点数不能超过 2^16
		// 可以通过以下代码使得单个Mesh的顶点限制扩大到 2^32，单实测的时候会导致Unity闪退
		// mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
		if (width < 0 || height < 0 || width * height > 65536)
		{
			quadMesh = null;
			return false;
		}

		Vector3[] vertices = new Vector3[(width + 1) * (height + 1)];
		for (int i = 0; i < width + 1; i++)
		{
			for (int j = 0; j < height + 1; j++)
			{
				vertices[j * (width + 1) + i] = new Vector3(i - (float) width / 2, j - (float) height / 2);
			}
		}

		int[] triangles = new int[6 * width * height];
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				int nowIndex = 6 * (j * width + i);
				triangles[nowIndex] = j * (width + 1) + i;
				triangles[nowIndex + 1] = (j + 1) * (width + 1) + i;
				triangles[nowIndex + 2] = (j + 1) * (width + 1) + i + 1;
				triangles[nowIndex + 3] = (j + 1) * (width + 1) + i + 1;
				triangles[nowIndex + 4] = j * (width + 1) + i + 1;
				triangles[nowIndex + 5] = j * (width + 1) + i;
			}
		}

		Mesh mesh = new Mesh {name = "Quad", vertices = vertices, triangles = triangles};
		mesh.RecalculateNormals();
		mesh.RecalculateTangents();
		quadMesh = mesh;
		return true;
	}

	public static bool GetQuad(int width, int height, out GameObject quad)
	{
		if (GetQuadMesh(width, height, out Mesh quadMesh))
		{
			GameObject go = new GameObject("Quad");
			go.AddComponent<MeshFilter>().mesh = quadMesh;
			go.AddComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/DefaultMaterial");
			quad = go;
			return true;
		}
		
		quad = null;
		return false;
	}

	public static bool GetSectorMesh(float radius, double radian, int edges, out Mesh sectorMesh)
	{
		if (radian > 360)
		{
			Debug.LogWarning("Radian is larger than 2 * PI, and will be limited to 2 * PI");
			radian = 360;
		}

		double d = 2 * Mathf.PI * radian / (360 * edges);
		double offset = -d * edges / 2;
		Vector3[] vertices = new Vector3[edges + 2];
		vertices[0] = new Vector3(0, 0, 0);
		for (int i = 1; i < edges + 2; i++)
		{
			float r1 = (float) (d * (i - 1) + offset);
			vertices[i] = new Vector3(radius * Mathf.Sin(r1), radius * Mathf.Cos(r1), 0);
		}

		int[] triangles = new int[edges * 3];
		for (int i = 0; i < edges; i++)
		{
			int nowIndex = i * 3;
			triangles[nowIndex] = 0;
			triangles[nowIndex + 1] = i + 1;
			triangles[nowIndex + 2] = i + 2;
		}

		Mesh mesh = new Mesh() {name = "Sector", vertices = vertices, triangles = triangles};
		mesh.RecalculateNormals();
		mesh.RecalculateTangents();
		sectorMesh = mesh;
		return true;
	}
	
	public static bool GetSector(float radius, double radian, int edges, out GameObject sector)
	{
		if (GetSectorMesh(radius, radian, edges, out Mesh sectorMesh))
		{
			GameObject go = new GameObject("Sector");
			go.AddComponent<MeshFilter>().mesh = sectorMesh;
			go.AddComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/DefaultMaterial");
			sector = go;
			return true;
		}

		sector = null;
		return false;
	}
}
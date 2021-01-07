using System;
using System.Collections.Generic;

namespace UnityEngine.UI
{
	public class QOutline : QShadow
	{
		protected QOutline()
		{
		}

		private Mesh mesh;

		public override void ModifyMesh(VertexHelper vh)
		{
			if (!IsActive())
				return;

			if (mesh == null)
			{
				mesh = new Mesh(){name = "QOutline_Cached_Mesh"};
			}

			vh.FillMesh(mesh);
			Vector3[] vertices = mesh.vertices;
			Color[] colors = mesh.colors;
			Vector2[] uv0s = mesh.uv;
			Vector2[] uv1s = mesh.uv2;
			Vector2[] uv2s = mesh.uv3;
			Vector2[] uv3s = mesh.uv4;
			Vector3[] normals = mesh.normals;
			Vector4[] tangents = mesh.tangents;
			Int32[] triangls = mesh.triangles;

			if (!IsNumsEqual(vertices.Length, colors.Length, uv0s.Length, uv1s.Length, uv2s.Length, uv3s.Length, normals.Length, tangents.Length))
			{
				// Debug.LogWarning("It's a little odd that these length are not equal");
				return;
			}

			vh.Clear();

			Vector3[] newVertices = GetArrayCopies(vertices, 5);
			Color[] newColors = GetArrayCopies(colors, 5);
			Vector2[] newUv0s = GetArrayCopies(uv0s, 5);
			Vector2[] newUv1s = GetArrayCopies(uv1s, 5);
			Vector2[] newUv2s = GetArrayCopies(uv2s, 5);
			Vector2[] newUv3s = GetArrayCopies(uv3s, 5);
			Vector3[] newNormals = GetArrayCopies(normals, 5);
			Vector4[] newTangents = GetArrayCopies(tangents, 5);

			int verticeCount = vertices.Length;
			for (int i = 0; i < vertices.Length; i++)
			{
				newVertices[i] = vertices[i] + new Vector3(effectDistance.x, effectDistance.y);
				newVertices[i + verticeCount] = vertices[i] + new Vector3(-effectDistance.x, effectDistance.y);
				newVertices[i + verticeCount * 2] = vertices[i] + new Vector3(-effectDistance.x, -effectDistance.y);
				newVertices[i + verticeCount * 3] = vertices[i] + new Vector3(effectDistance.x, -effectDistance.y);
				newColors[i] = effectColor;
				newColors[i + verticeCount] = effectColor;
				newColors[i + verticeCount * 2] = effectColor;
				newColors[i + verticeCount * 3] = effectColor;
				if (useGraphicAlpha)
				{
					newColors[i].a = effectColor.a * colors[i].a;
					newColors[i + verticeCount].a = effectColor.a * colors[i].a;
					newColors[i + verticeCount * 2].a = effectColor.a * colors[i].a;
					newColors[i + verticeCount * 3].a = effectColor.a * colors[i].a;
				}
			}

			int[] newTriangles = new int[triangls.Length * 5];
			for (var i = 0; i < triangls.Length; i++)
			{
				newTriangles[i] = triangls[i];
				newTriangles[i + triangls.Length] = triangls[i] + verticeCount;
				newTriangles[i + triangls.Length * 2] = triangls[i] + verticeCount * 2;
				newTriangles[i + triangls.Length * 3] = triangls[i] + verticeCount * 3;
				newTriangles[i + triangls.Length * 4] = triangls[i] + verticeCount * 4;
			}

			for (var i = 0; i < newVertices.Length; i++)
			{
				vh.AddVert(newVertices[i], newColors[i], newUv0s[i], newUv1s[i], newUv2s[i], newUv3s[i], newNormals[i], newTangents[i]);
			}

			for (var i = 0; i < newTriangles.Length / 3; i++)
			{
				vh.AddTriangle(newTriangles[i * 3], newTriangles[i * 3 + 1], newTriangles[i * 3 + 2]);
			}
		}
	}
}
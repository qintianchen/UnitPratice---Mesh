using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityEngine.UI
{
	public class QShadow : BaseMeshEffect
	{
		[SerializeField] private Color m_EffectColor = new Color(0f, 0f, 0f, 0.5f);
		[SerializeField] private Vector2 m_EffectDistance = new Vector2(1f, -1f);
		[SerializeField] private bool m_UseGraphicAlpha = true;
		private const float kMaxEffectDistance = 600f;

		protected QShadow()
		{
		}

#if UNITY_EDITOR
		protected override void OnValidate()
		{
			effectDistance = m_EffectDistance;
			base.OnValidate();
		}
#endif
		public Color effectColor
		{
			get { return m_EffectColor; }
			set
			{
				m_EffectColor = value;
				if (graphic != null)
					graphic.SetVerticesDirty();
			}
		}

		public Vector2 effectDistance
		{
			get { return m_EffectDistance; }
			set
			{
				if (value.x > kMaxEffectDistance)
					value.x = kMaxEffectDistance;
				if (value.x < -kMaxEffectDistance)
					value.x = -kMaxEffectDistance;

				if (value.y > kMaxEffectDistance)
					value.y = kMaxEffectDistance;
				if (value.y < -kMaxEffectDistance)
					value.y = -kMaxEffectDistance;

				if (m_EffectDistance == value)
					return;

				m_EffectDistance = value;

				if (graphic != null)
					graphic.SetVerticesDirty();
			}
		}
		
		public bool useGraphicAlpha
		{
			get { return m_UseGraphicAlpha; }
			set
			{
				m_UseGraphicAlpha = value;
				if (graphic != null)
					graphic.SetVerticesDirty();
			}
		}

		private Mesh mesh;

		protected bool IsNumsEqual(params int[] nums)
		{
			for (var i = 0; i < nums.Length - 1; i++)
			{
				if (nums[i] != nums[i + 1])
					return false;
			}

			return true;
		}

		protected T[] GetArrayCopies<T>(T[] arr, int n)
		{
			T[] newArr = new T[n * arr.Length];
			for (var i = 0; i < n; i++)
			{
				arr.CopyTo(newArr, i * arr.Length);
			}

			return newArr;
		}

		public override void ModifyMesh(VertexHelper vh)
		{
			if (!IsActive())
				return;

			if (mesh == null)
			{
				mesh = new Mesh(){name = "QShadow_Cached_Mesh"};
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

			Vector3[] newVertices = GetArrayCopies(vertices, 2);
			Color[] newColors = GetArrayCopies(colors, 2);
			Vector2[] newUv0s = GetArrayCopies(uv0s, 2);
			Vector2[] newUv1s = GetArrayCopies(uv1s, 2);
			Vector2[] newUv2s = GetArrayCopies(uv2s, 2);
			Vector2[] newUv3s = GetArrayCopies(uv3s, 2);
			Vector3[] newNormals = GetArrayCopies(normals, 2);
			Vector4[] newTangents = GetArrayCopies(tangents, 2);

			for (int i = 0; i < vertices.Length; i++)
			{
				newVertices[i] = vertices[i] + new Vector3(effectDistance.x, effectDistance.y);
				newColors[i] = effectColor;
				if (useGraphicAlpha)
				{
					newColors[i].a = effectColor.a * colors[i].a;
				}
			}

			int[] newTriangles = new int[triangls.Length * 2];
			for (var i = 0; i < triangls.Length; i++)
			{
				newTriangles[i] = triangls[i];
				newTriangles[i + triangls.Length] = triangls[i] + vertices.Length;
			}

			for (var i = 0; i < newVertices.Length; i++)
			{
				vh.AddVert(newVertices[i], newColors[i], newUv0s[i], newUv1s[i], newUv2s[i], newUv3s[i], newNormals[i], newTangents[i]);
			}

			for (var i = 0; i < newTriangles.Length / 3; i++)
			{
				vh.AddTriangle(newTriangles[i*3], newTriangles[i*3 + 1], newTriangles[i*3 + 2]);
			}
		}
	}
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 多边形图片
/// </summary>
public class PolygonImage : MaskableGraphic
{
    [Tooltip("多边形半径")]
    [Range(0.1f, 1000)] public float radius = 100;
    [Tooltip("多边形的顶点数（非实际绘制的顶点数量）")]
    [Range(3, 20)] public int verts = 8;
    [Tooltip("旋转偏移")]
    public float offset = 0;
    [Tooltip("动画时长")]
    public float tweenTime = 1;
    [Tooltip("每条径的长度")]
    [SerializeField] private float[] sizes;
    
    public bool SetSize(int index, float value)
    {
        Debug.Log($"set size {index}, {value}");
        if (index >= sizes.Length)
        {
            return false;
        }

        float[] newSizes = new float[sizes.Length];
        sizes.CopyTo(newSizes, 0);
        newSizes[index] = value;
        StartCoroutine(ChangeSizesInCoroutine(newSizes));
        return true;
    }

    public bool SetSizes(float[] newSizes)
    {
        if (newSizes.Length > sizes.Length)
        {
            return false;
        }

        float[] _newSizes = new float[sizes.Length];
        sizes.CopyTo(_newSizes, 0);
        newSizes.CopyTo(_newSizes, 0);
        StartCoroutine(ChangeSizesInCoroutine(newSizes));
        return true;
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        if (sizes == null || sizes.Length != verts)
        {
            sizes = new float[verts];
            for (int i = 0; i < verts; i++)
            {
                sizes[i] = 1;
            }
        }

        Rect rect = GetPixelAdjustedRect();
        radius = Math.Min(radius, Math.Min(rect.height / 2, rect.width / 2));
        Color color32 = color;
        double degree = 2 * Math.PI / verts;

        vh.AddVert(new Vector3(0, 0), color32, new Vector2(0, 0));
        for (int i = 0; i < verts; i++)
        {
            float a1 = -sizes[i] * radius * (float) Math.Sin(degree * i + 2 * Math.PI * offset / 360);
            float a2 = sizes[i] * radius * (float) Math.Cos(degree * i + 2 * Math.PI * offset / 360);
            vh.AddVert(new Vector3(a1, a2), color32, new Vector2(0, 0));
        }

        for (int i = 1; i < verts + 1; i++)
        {
            if (i == verts)
                vh.AddTriangle(0, 1, i);
            else
                vh.AddTriangle(0, i + 1, i);
        }
    }

    private IEnumerator ChangeSizesInCoroutine(float[] newSizes)
    {
        float[] oldSizes = new float[sizes.Length];
        sizes.CopyTo(oldSizes, 0);
        float t = 0;
        while (tweenTime - t > 0.0001f)
        {
            t += Time.deltaTime;
            for (int i = 0; i < sizes.Length; i++)
            {
                sizes[i] = Mathf.Lerp(oldSizes[i], newSizes[i], t / tweenTime);
            }

            SetVerticesDirty();
            yield return null;
        }
    }
}
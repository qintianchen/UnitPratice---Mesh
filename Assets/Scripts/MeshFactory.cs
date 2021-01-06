using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshFactory
{
    public static bool GetQuad(int width, int height, out GameObject quad)
    {
        if (width < 0 || height < 0 || width * height > 65536)
        {
            quad = null;
            return false;
        }

        Mesh mesh = new Mesh {name = "Quad", indexFormat = UnityEngine.Rendering.IndexFormat.UInt32};
        GameObject go = new GameObject("Quad");
        go.AddComponent<MeshFilter>().mesh = mesh;
        go.AddComponent<MeshRenderer>();
        quad = go;
        return true;
    }
}

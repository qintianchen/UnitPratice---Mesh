using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolygonImageTest : MonoBehaviour
{
    public PolygonImage image;

    private float t = 0;
    private void Update()
    {
        t += Time.deltaTime;
        if (t > 2f)
        {
            t = 0;
 
            float[] newSizes = new float[image.verts];
            for (int i = 0; i < newSizes.Length; i++)
            {
                newSizes[i] = Random.Range(0, 1f);
            }
            if (!image.SetSizes(newSizes))
            {
                Debug.LogWarning("SetSize false");
            }
        }

    }
}
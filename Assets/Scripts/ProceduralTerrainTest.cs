using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralTerrainTest : MonoBehaviour
{
    [Header("Sector Settings")] public float radius = 1;
    public double radian = 180;
    public int edges = 80;

    private GameObject sector;
    void Start()
    {
        // MeshFactory.GetQuad(20, 20, out GameObject quad);
        MeshFactory.GetSector(radius, radian, edges, out sector);
    }

    private float t = 0;
    void Update()
    {
        t += Time.deltaTime;
        if (t > 0.2f)
        {
            t = 0;
            if (MeshFactory.GetSectorMesh(radius, radian, edges, out Mesh sectorMesh))
            {
                sector.GetComponent<MeshFilter>().mesh = sectorMesh;
            }
        }
            
    }
}

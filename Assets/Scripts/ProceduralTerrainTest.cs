using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralTerrainTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MeshFactory.GetQuad(10, 20, out GameObject quad);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiate512Cubes : MonoBehaviour
{
    public GameObject sampleCubePrefab;
    private GameObject[] sampleCube = new GameObject[512];

    public float maxScale;

    void Start()
    {
        for (int i = 0; i < 512; i++ )
        {
            GameObject instantiateSampleCube = (GameObject)Instantiate(sampleCubePrefab);
            instantiateSampleCube.transform.position = this.transform.position;
            instantiateSampleCube.transform.parent = this.transform;
            instantiateSampleCube.name = "SampleCube" + i;
            this.transform.eulerAngles = new Vector3(0, -0.703125f * i, 0);
            instantiateSampleCube.transform.position = Vector3.forward * 100;
            sampleCube[i] = instantiateSampleCube;
        }
    }

    
    void Update()
    {
        for (int i =0; i < 512; i++)
        {
            if (sampleCube != null)
            {
                sampleCube[i].transform.localScale = new Vector3(10, (AudioPeer.samples[i] * maxScale) + 2, 10);
            }
        }
    }
}

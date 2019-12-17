using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Light))]
public class LightOnAudio : MonoBehaviour
{
    public int band;
    public float minIntensity, maxIntensity;
    private Light light;
    
    void Start()
    {
        light = GetComponent<Light>();
    }

    
    void Update()
    {
        light.intensity = (AudioPeer.audioBandBuffer[band] * (maxIntensity - minIntensity)) + minIntensity;
    }
}

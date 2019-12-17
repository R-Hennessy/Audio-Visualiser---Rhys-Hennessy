using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (AudioSource))]
public class AudioPeer : MonoBehaviour
{
    AudioSource audioSource;

    public static float[] samples = new float[512];
    private float[] freqBand = new float[8];
    private float[] bandBuffer = new float[8];
    private float[] bufferDecrease = new float[8];

    private float[] freqBandHighest = new float[8];
    public static float[] audioBand = new float[8];
    public static float[] audioBandBuffer = new float[8];

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    
    void Update()
    {
        GetSpectrumAudioSource();
        MakefreqBands();
        BandBuffer();
        CreateAudioBands();
    }

    void CreateAudioBands()
    {
        for (int g = 0; g < 8; g++)
        {
            if (freqBand[g] > freqBandHighest[g])
            {
                freqBandHighest[g] = freqBand[g];
            }

            audioBand[g] = (freqBand[g] / freqBandHighest[g]);
            audioBandBuffer[g] = (bandBuffer[g] / freqBandHighest[g]);
        }
    }

    void GetSpectrumAudioSource()
    {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);
    }

    void MakefreqBands()
    {
        /*
         *22050 / 512 = 43 Hertz per sample
         * 
         * 20 - 60 Hertz
         * 250 - 500 Hertz
         * 500 - 2000 Hertz
         * 2000 - 4000 Hertz
         * 4000 - 6000 Hertz
         * 6000 - 20000 Hertz
         *  
         * 0 - 2 = 86 Hertz
         * 1 - 4 = 172 Hertz   - 87-258 
         * 2 - 8 = 344 Hertz   - 259-602
         * 3 - 16 = 688 Hertz  - 603-1290
         * 4 - 32 = 1376 Hertz - 1291-2666
         * 5 - 64 = 2752 Hertz - 2667-5418
         * 6 - 128= 5504 Hertz - 5419-10922
         * 7 - 256= 11008Hertz - 10923 - 21930
         * 
         *     510
         */

        int count = 0;

        for (int i =0; i < 8; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;

            if (i == 7)
            {
                sampleCount += 2;
            }
            
            for (int j = 0; j < sampleCount; j++)
            {
                average += samples[count] * (count + 1);
                count++;
            }

            average /= count;

            freqBand[i] = average * 10;
        }       
    }

    void BandBuffer()
    {
        for (int k = 0; k < 8; k++)
        {
            if (freqBand[k] > bandBuffer[k])
            {
                bandBuffer[k] = freqBand[k];
                bufferDecrease[k] = 0.005f;
            }

            if (freqBand[k] < bandBuffer[k])
            {
                bandBuffer[k] -= bufferDecrease[k];
                bufferDecrease[k] *= 1.2f;
            }
        }
    }
}

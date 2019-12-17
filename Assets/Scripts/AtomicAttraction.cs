using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomicAttraction : MonoBehaviour
{
    public GameObject atom, attractor;
    public Gradient gradient;
    public int[] attractPoints;
    [Range (0, 20)]
    public float spacingBetweenAttractPoints;
    [Range(0, 20)]
    public float scaleAttractPoints;
    public Vector3 spacingDirection;

    GameObject[] attractorArray, atomArray;
    [Range(1, 64)]
    public int amountOfAtomsPerPoint;
    public Vector2 atomScaleMinMax;
    float[] atomScaleSet;

    public float strengthOfAttraction, maxMagnitude, randomDistance;
    public bool useGravity;

    public Material material;
    Material[] sharedMaterial;
    Color[] sharedColor;

    public float audioScaleMultiplier, audioEmmissionMultiplier;

    [Range(0.0f, 1.0f)]
    public float tresholdEmission;

    float[] audioBandEmissionTreshold;
    float[] audioBandEmissionColor;
    float[] audioBandScale;

    public enum emissionTreshold { Buffered, NoBuffer };
    public emissionTreshold _emissionTreshold = new emissionTreshold();
    public enum emissionColor{ Buffered, NoBuffer };
    public emissionColor _emissionColor = new emissionColor();
    public enum atomScale { Buffered, NoBuffer };
    public atomScale _atomScale = new atomScale();


    private void OnDrawGizmos()

    {
        for (int i = 0; i < attractPoints.Length; i++)
        {
            float evaluateStep = 0.125f;

            Color color = gradient.Evaluate(Mathf.Clamp(evaluateStep * attractPoints[i], 0, 7));
            Gizmos.color = color;

            Vector3 pos = new Vector3(transform.position.x + (spacingBetweenAttractPoints * i * spacingDirection.x),
                                      transform.position.y + (spacingBetweenAttractPoints * i * spacingDirection.y),
                                      transform.position.z + (spacingBetweenAttractPoints * i * spacingDirection.z));
            Gizmos.DrawSphere(pos, scaleAttractPoints); 
        }
    }

    void Start()
    {
        attractorArray = new GameObject[attractPoints.Length];
        atomArray = new GameObject[attractPoints.Length * amountOfAtomsPerPoint];
        atomScaleSet = new float[attractPoints.Length * amountOfAtomsPerPoint];

        audioBandEmissionTreshold = new float[8];
        audioBandEmissionColor = new float[8];
        audioBandScale = new float[8];
        sharedMaterial = new Material[8];
        sharedColor = new Color[8];

        int countAtom = 0;

        //Instantiate Attract Points
        for (int i = 0; i < attractPoints.Length; i++)
        {
            GameObject attractorInstance = (GameObject)Instantiate(attractor);
            attractorArray[i] = attractorInstance;

            attractorInstance.transform.position = new Vector3(transform.position.x + (spacingBetweenAttractPoints * i * spacingDirection.x),
                                                               transform.position.y + (spacingBetweenAttractPoints * i * spacingDirection.y),
                                                               transform.position.z + (spacingBetweenAttractPoints * i * spacingDirection.z));

            attractorInstance.transform.parent = this.transform;
            attractorInstance.transform.localScale = new Vector3(scaleAttractPoints, scaleAttractPoints, scaleAttractPoints);

            //Set colors to material
            Material matInstance = new Material(material);
            sharedMaterial[i] = matInstance;
            sharedColor[i] = gradient.Evaluate(0.125f * i);

            //Instantiate atoms
            for (int j = 0; j < amountOfAtomsPerPoint; j++)
            {
                GameObject atomInstance = (GameObject)Instantiate(atom);
                atomArray[countAtom] = atomInstance;
                atomInstance.GetComponent<AttractTo>().attractedTo = attractorArray[i].transform;
                atomInstance.GetComponent<AttractTo>().strengthOfAttraction = strengthOfAttraction;
                atomInstance.GetComponent<AttractTo>().maxMagnitude = maxMagnitude;
                if(useGravity == true)
                {
                    atomInstance.GetComponent<Rigidbody>().useGravity = true;
                }
                else
                {
                    atomInstance.GetComponent<Rigidbody>().useGravity = false;
                }

                atomInstance.transform.position = new Vector3(attractorArray[i].transform.position.x + Random.Range(-randomDistance, randomDistance),
                                                              attractorArray[i].transform.position.y + Random.Range(-randomDistance, randomDistance),
                                                              attractorArray[i].transform.position.z + Random.Range(-randomDistance, randomDistance));

                float randomScale = Random.Range(atomScaleMinMax.x, atomScaleMinMax.y);
                atomScaleSet[countAtom] = randomScale;
                atomInstance.transform.localScale = new Vector3(atomScaleSet[countAtom], atomScaleSet[countAtom], atomScaleSet[countAtom]);

                atomInstance.transform.parent = attractorInstance.transform;
                atomInstance.GetComponent<MeshRenderer>().material = sharedMaterial[i];
                   countAtom++;
            }
        }
    }
    
    void Update()
    {
        SelectAudioValues();
        AtomBehaviour();
    }

    void AtomBehaviour()
    {
        int countAtom = 0;
        for (int i = 0; i < attractPoints.Length; i++)
        {
            if (audioBandEmissionTreshold[attractPoints[i]] >= tresholdEmission)
            {
                Color audioColor = new Color(sharedColor[i].r * audioBandEmissionColor[attractPoints[i]] * audioEmmissionMultiplier,
                                             sharedColor[i].g * audioBandEmissionColor[attractPoints[i]] * audioEmmissionMultiplier,
                                             sharedColor[i].b * audioBandEmissionColor[attractPoints[i]] * audioEmmissionMultiplier, 1);
                sharedMaterial[i].SetColor("_EmissionColor", audioColor);                
            } 
            else
            {
                Color audioColor = new Color(0, 0, 0, 1);
                sharedMaterial[i].SetColor("_EmissionColor", audioColor);
                
            }

            for (int j = 0; j < amountOfAtomsPerPoint; j++)
            {
                atomArray[countAtom].transform.localScale = new Vector3(atomScaleSet[countAtom] + audioBandScale[attractPoints[i]] * audioScaleMultiplier,
                                                                        atomScaleSet[countAtom] + audioBandScale[attractPoints[i]] * audioScaleMultiplier,
                                                                        atomScaleSet[countAtom] + audioBandScale[attractPoints[i]] * audioScaleMultiplier);
                countAtom++; 
            }           
        }
    }

    void SelectAudioValues()
    {
        //Treshold
        if (_emissionTreshold == emissionTreshold.Buffered)
        {
            for (int i =0; i < 8; i++)
            {
                audioBandEmissionTreshold[i] = AudioPeer.audioBandBuffer[i];
            }
        }
        if (_emissionTreshold == emissionTreshold.NoBuffer)
        {
            for (int i = 0; i < 8; i++)
            {
                audioBandEmissionTreshold[i] = AudioPeer.audioBand[i];
            }
        }

        //Emission Color
        if (_emissionColor == emissionColor.Buffered)
        {
            for (int i = 0; i < 8; i++)
            {
                audioBandEmissionColor[i] = AudioPeer.audioBandBuffer[i];
            }
        }       
        if (_emissionColor == emissionColor.NoBuffer)
        {
            for (int i = 0; i < 8; i++)
            {
                audioBandEmissionColor[i] = AudioPeer.audioBand[i];
            }
        }

        //Atom Scale
        if (_atomScale == atomScale.Buffered)
        {
            for (int i = 0; i < 8; i++)
            {
                audioBandScale[i] = AudioPeer.audioBandBuffer[i];
            }
        }
        if (_atomScale == atomScale.NoBuffer)
        {
            for (int i = 0; i < 8; i++)
            {
                audioBandScale[i] = AudioPeer.audioBand[i];
            }
        }
    }
}



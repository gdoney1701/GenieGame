﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StructDissolveHandler : MonoBehaviour
{
    public int reqPuzzles = 1;
    public List<Transform> structDamage;
    public List<Transform> structRepair;
    public List<Transform> shadowsDamage;
    public List<Transform> shadowsRepair;
    public List<Transform> toUse;
    public List<bool> reqMet;
    public bool beginDissolve = false;
    public int chunkSize = 10;
    public int chunkLoad = 0;
    public int simulchunks = 1;
    public float simulDelay = 0;
    private float t = 0.0f;
    public bool doneRepair;
    public bool doneDamage;
    public bool shadowsDone;
    // Start is called before the first frame update
    void Start()
    {
        foreach( Transform child in gameObject.transform)
        {
            toUse.Add(child);
        }
        for(int i = 0; i< reqPuzzles; i++)
        {
            reqMet.Add(false);
        }
        for (int i = 0; i < toUse.Count; i++)
        {
            if (toUse[i].gameObject.tag == "Destruction Group")
            {
                foreach (Transform child in toUse[i])
                {
                    structDamage.Add(child);
                    Collider collCheck = child.GetComponent<Collider>();
                    if (collCheck != null)
                    {
                        collCheck.enabled = true;
                    }

                }
            }
            else if (toUse[i].gameObject.tag == "Repair Group")
            {
                foreach (Transform child in toUse[i])
                {
                    structRepair.Add(child);
                    Collider collCheck = child.GetComponent<Collider>();
                    if (collCheck != null)
                    {
                        collCheck.enabled = false;
                    }
                }
            }
            if (toUse[i].gameObject.tag == "Shadow Damage")
            {
                foreach (Transform child in toUse[i])
                {
                    shadowsDamage.Add(child);
                    Collider collCheck = child.GetComponent<Collider>();
                    if (collCheck != null)
                    {
                        collCheck.enabled = true;
                    }

                }
            }
            if (toUse[i].gameObject.tag == "Shadow Repair")
            {
                foreach (Transform child in toUse[i])
                {
                    shadowsRepair.Add(child);
                    Collider collCheck = child.GetComponent<Collider>();
                    if (collCheck != null)
                    {
                        collCheck.enabled = true;
                    }

                }
            }
        }
        colorCorrection(structDamage, -1, new Vector4(1.630717f, 22.97174f, 0.0f, 1.0f), new Vector4(0f, 0.8089904f, 5.235602f, 1.0f));
        colorCorrection(shadowsDamage, -1, new Vector4(1.630717f, 22.97174f, 0.0f, 1.0f), new Vector4(0f, 0.8089904f, 5.235602f, 1.0f));
        colorCorrection(structRepair, 1, new Vector4(22.97174f, 0.0f, 22.76659f, 1.0f), new Vector4(5.235602f, 0.2092855f, 0f, 1.0f));
        colorCorrection(shadowsRepair, 1, new Vector4(22.97174f, 0.0f, 22.76659f, 1.0f), new Vector4(5.235602f, 0.2092855f, 0f, 1.0f));
        listRandomizer(structRepair);
        listRandomizer(structDamage);
    }

    // Update is called once per frame
    void Update()
    {
        if (beginDissolve)
        {
            if (!shadowsDone)
            {
                DissolveShadows(shadowsRepair, 1, -1);
                DissolveShadows(shadowsDamage, -1, 1);
            }
           if (chunkLoad+chunkSize < structDamage.Count-1)
            {
                    DissolveStep(chunkLoad, chunkSize, structDamage, -1, 1,false);
            }else if(chunkLoad + chunkSize > structDamage.Count && chunkLoad+chunkSize < structDamage.Count+chunkSize)
            {
                    int tempchunkSize = structDamage.Count - chunkLoad;
                    DissolveStep(chunkLoad, tempchunkSize, structDamage, -1,1,false);
            }else
            {
                print("Done with Damage");
                doneDamage = true;
                for (int i = 0; i< structDamage.Count; i++)
                {
                    Destroy(structDamage[i].gameObject);
                }
            }
           if(chunkLoad+chunkSize < structRepair.Count - 1)
            {
                DissolveStep(chunkLoad, chunkSize, structRepair, 1, -1, true);
            }else if (chunkLoad + chunkSize > structRepair.Count && chunkLoad + chunkSize < structRepair.Count + chunkSize)
            {
                int tempchunkSize = structRepair.Count - chunkLoad;
                DissolveStep(chunkLoad, tempchunkSize, structRepair, 1, -1, true);
            }
            else
            {
                print("Done with Repair");
                doneRepair = true;
            }
            
        }
        if (doneRepair && doneDamage)
        {
            GameObject manager = GameObject.FindGameObjectWithTag("PlayMan");
            manager.GetComponent<GameplayManager>().PuzzleComplete(0, 1);
            doneDamage = false;
            doneRepair = false;
            beginDissolve = false;
        }
    }
    public void DissolveStep(int workingChunk, int chunkStep, List<Transform> toDissolve, float start, float end, bool repair)
    {
        Vector3 chunkDissolving = new Vector3(0, 0, 0);

        for (int i = workingChunk; i < workingChunk + chunkStep; i++)
        {
            chunkDissolving = DissolveLerp(t, toDissolve[i].gameObject, start, end, "Vector1_A2CB8D29");
        }
        t += 0.005f;
        if (chunkDissolving.x >= 1)
        {
            for (int i = workingChunk; i < workingChunk +chunkStep; i++)
            {
                Collider collCheck = toDissolve[i].gameObject.GetComponent<Collider>();
                if(collCheck != null)
                {
                    if (repair)
                    {
                        collCheck.enabled = true;
                    }
                    else
                    {
                        collCheck.enabled = false;
                    }
                }
            }
            reInitDissolve();
        }
    }
    public void DissolveShadows(List<Transform> toDissove, float start, float end)
    {
        Vector3 dissolveJourney = new Vector3(0, 0, 0);
        for(int i = 0; i < toDissove.Count; i++)
        {
            dissolveJourney = DissolveLerp(t, toDissove[i].gameObject, start, end, "Vector1_A2CB8D29");
        }
        if(dissolveJourney.x >= 1)
        {
            shadowsDone = true;
        }
    }
    public Vector3 DissolveLerp(float time, GameObject toDissolve, float min, float max, string vectorName)
    {
        Vector3 journey = new Vector3(Mathf.Lerp(min, max, time), 0, 0);
        toDissolve.GetComponent<Renderer>().material.SetFloat(vectorName, journey[0]);
        return journey;
    }
    public void reInitDissolve()
    {
        t = .1f;
        chunkLoad += chunkSize;
    }
    public void PuzzleFound(int ID)
    {
        reqPuzzles -= 1;

        if (reqPuzzles <= 0)
        {
            beginDissolve = true;
        }
    }

    public void listRandomizer(List<Transform> toModify)
    {
        for (int i = 0; i < toModify.Count; i++)
        {
            Transform temp = toModify[i];
            int randomIndex = Random.Range(i, toModify.Count);
            toModify[i] = toModify[randomIndex];
            toModify[randomIndex] = temp;
        }
    }
    public void colorCorrection(List<Transform> toCorrect, float begin, Vector4 topColor, Vector4 bottomColor)
    {
        print(toCorrect.Count);
        for (int i = 0; i < toCorrect.Count; i++)
        {
            Renderer toChange = structDamage[i].gameObject.GetComponent<Renderer>();
            toChange.material.SetFloat("Vector1_A2CB8D29", begin);
            toChange.material.SetColor("Color_4DCAD544", topColor);
            toChange.material.SetColor("Color_AED29001", bottomColor);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StructDissolveHandler : MonoBehaviour
{
    public int reqPuzzles = 1;
    public List<Transform> structDamage;
    public List<Transform> subGroups;
    public List<bool> reqMet;
    public bool beginDissolve = false;
    public int chunkSize = 10;
    public int chunkLoad = 0;
    public int simulchunks = 1;
    public float simulDelay = 0;
    private float t = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i< reqPuzzles; i++)
        {
            reqMet.Add(false);
        }
        foreach (Transform child in gameObject.transform)
        {
            structDamage.Add(child);

        }
        for (int i = 0; i < structDamage.Count; i++)
        {
            Transform temp = structDamage[i];
            int randomIndex = Random.Range(i, structDamage.Count);
            structDamage[i] = structDamage[randomIndex];
            structDamage[randomIndex] = temp;
        }
        for(int i = 0; i< structDamage.Count / chunkSize;i++)
        {

        }

    }

    // Update is called once per frame
    void Update()
    {
        if (beginDissolve)
        {

            for (int j = 0; j < simulchunks; j++)
            {
                int tempchunk = chunkLoad + j * chunkSize;
                if (tempchunk+chunkSize < structDamage.Count)
                {
                    DissolveStep(tempchunk);

                }
                else if(chunkLoad+chunkSize <= structDamage.Count)
                {
                    DissolveStep(chunkSize);
                }else if(structDamage.Count - chunkLoad < chunkSize)
                {
                    chunkSize = structDamage.Count - chunkLoad;
                    DissolveStep(chunkSize);
                }
                {
                    print("Done");
                    //Destroy(gameObject);
                }
            }
            
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
        t = 0;
        chunkLoad += chunkSize;
    }
    public void PuzzleFound(int ID)
    {
        reqMet[ID] = true;
        bool ollKorrect = false;
        foreach(bool answer in reqMet)
        {
            if (answer)
            {
                ollKorrect = true;
            }
            else
            {
                ollKorrect = false;
            }
        }
        if (ollKorrect)
        {
            beginDissolve = true;
        }
    }
    public void DissolveStep(int workingChunk)
    {
        Vector3 chunkDissolving = new Vector3(0, 0, 0);

        for (int i = workingChunk; i < workingChunk + chunkSize; i++)
        {
            print("Dissolving" + structDamage[i].gameObject.name);
            chunkDissolving = DissolveLerp(t, structDamage[i].gameObject, -3f, 1f, "Vector1_A2CB8D29");
            t += 0.0005f;
        }
        print(chunkDissolving);
        if (chunkDissolving.x >= 1)
        {
            reInitDissolve();
        }
    }
}

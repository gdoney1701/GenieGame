using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestalScript : MonoBehaviour
{
    public List<Transform> childPositions;
    public List<int> filledlocations;
    public List<GameObject> suspendedPieces;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            childPositions.Add(child);
            filledlocations.Add(1);
        }
    }
    private void Update()
    {
        
    }
    // Update is called once per frame
    public bool MovingtoPedestal(GameObject objectToMove)
    {
        //in filled location, every index marked 1 is empty, every index marked 0 is full
        int location = 0;
        bool foundLocation = false;

        for(int i =0; i< childPositions.Count; ++i)
        {
            if(filledlocations[i] == 1 && foundLocation == false)
            {
                location = i;
                filledlocations[i] = 0;
                foundLocation = true;
            }else if (filledlocations[i] == 0)
            {
                foundLocation = false;
            }
        }
        if (foundLocation == true)
        {
            objectToMove.GetComponent<CloneTravel>().beginMovement(gameObject, objectToMove.transform, childPositions[location]);
            GameObject MC = GameObject.FindGameObjectWithTag("Player");
            MC.GetComponent<PlayerScript>().carrying = false;

        }
        else if (foundLocation == false)
        {
            print("No Empty Spots");
        }
        return (foundLocation);
    }
    public void CombineCheckPedestal()
    {
        bool correct = true;
        if (suspendedPieces.Count == childPositions.Count)
        {
            int acceptedID = 0;
            for (int i = 0; i < suspendedPieces.Count; i++)
            {
                int personalID = suspendedPieces[i].GetComponent<CloneTravel>().whoamI[0];
                int numNeeded = suspendedPieces[i].GetComponent<CloneTravel>().whoamI[1];
                if (i == 0)
                {
                    acceptedID = personalID;
                }
                else
                {
                    if (personalID != acceptedID)
                    {
                        correct = false;
                    }
                    else
                    {
                        correct = true;
                    }
                }
            }
            if (correct == true)
            {
                print("Combination");
                FinalCombination();

            }
            else if (correct == false)
            {
                print("Failed Combination");
                for (int i = 0; i < suspendedPieces.Count; i++)
                {
                    suspendedPieces[i].GetComponent<CloneTravel>().dropObject();
                    suspendedPieces.RemoveAt(i);
                }
            }

        }
       
    }
    public void FinalCombination()
    {
        GameObject newEmpty = new GameObject();
        GameObject driftObj = Instantiate(newEmpty);
        driftObj.transform.position = gameObject.transform.position + new Vector3(0, 2, 0);
        for (int i =0; i<suspendedPieces.Count; i++)
        {
            GameObject obj = suspendedPieces[i];
            obj.GetComponent<CloneTravel>().beginMovement(driftObj, obj.transform, driftObj.transform);
        }

    }
}

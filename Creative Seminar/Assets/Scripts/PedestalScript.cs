using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestalScript : MonoBehaviour
{
    public List<Transform> childPositions;
    public List<int> filledlocations;
    public List<GameObject> suspendedPieces;
    public bool puzzleComplete = false;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            childPositions.Add(child);
            filledlocations.Add(1);
            suspendedPieces.Add(Instantiate(new GameObject(), child));
        }
    }
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
                ListManagement(objectToMove, i, true);
            }else if (filledlocations[i] == 0)
            {
                foundLocation = false;
            }
        }
        if (foundLocation == true)
        {
            objectToMove.GetComponent<CloneTravel>().beginMovement(gameObject, objectToMove.transform, childPositions[location]);
            objectToMove.GetComponent<CloneTravel>().onPedestal.c = location;
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
        if (puzzleComplete == false)
        {
            //checks how many pieces are inside the pedestal
            int collectedNum = 0;
            bool correct = true;
            for(int i=0; i<suspendedPieces.Count;i++)
            {
                if(suspendedPieces[i].tag == "PickUp")
                {
                    collectedNum += 1;
                }
            }
            if (collectedNum == childPositions.Count)
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
                    puzzleComplete = true;
                    FinalCombination();
                }
                else if (correct == false)
                {
                    print("Failed Combination");
                    for (int i = 0; i < suspendedPieces.Count; i++)
                    {
                        suspendedPieces[i].GetComponent<CloneTravel>().dropObject();
                        ListManagement(suspendedPieces[i], i, false);
                    }
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
            obj.GetComponent<CloneTravel>().onPedestal.d = true;
            obj.GetComponent<CloneTravel>().beginMovement(driftObj, obj.transform, driftObj.transform);

        }
    }
    //adds and removes different objects from the suspended pieces
    public void ListManagement(GameObject obj,int location, bool add)
    {
        if (add == true)
        {
            suspendedPieces.RemoveAt(location);
            suspendedPieces.Insert(location, obj);

            //Destroy(suspendedPieces[location]);
        }
        else if (add == false)
        {
            suspendedPieces.Add(Instantiate(new GameObject(), childPositions[location]));
            filledlocations[location] = 1;
            suspendedPieces.RemoveAt(location);
        }


    }
}

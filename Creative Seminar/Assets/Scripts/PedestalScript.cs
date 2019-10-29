using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestalScript : MonoBehaviour
{
    public List<Transform> childPositions;
    public List<bool> filledlocations;
    public bool puzzleComplete = false;
    public GameObject[] suspendedPieces2;
    // Start is called before the first frame update
    void Start()
    {
        int num = 0;
        foreach (Transform child in transform)
        {
            childPositions.Add(child);
            filledlocations.Add(false);
            num++;
        }
        suspendedPieces2 = new GameObject[num];
    }
    public bool MovingtoPedestal(GameObject objectToMove)
    {
        //in filled location, every index marked 1 is empty, every index marked 0 is full
        int location = 0;
        bool foundLocation = false;
        for(int i =0; i< childPositions.Count; ++i)
        {
            if(filledlocations[i] == false && foundLocation == false)
            {
                location = i;
                filledlocations[i] = true;
                foundLocation = true;
                ListManagement(objectToMove, i, true);
                break;

            }else if (filledlocations[i] == true)
            {
                foundLocation = false;
            }
        }
        if (foundLocation == true)
        {
            objectToMove.GetComponent<CloneTravel>().beginMovement(gameObject, objectToMove.transform, childPositions[location]);
            //objectToMove.GetComponent<CloneTravel>().onPedestal.c = location;
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
            for(int i=0; i<suspendedPieces2.Length;i++)
            {
                if(suspendedPieces2[i] != null)
                {
                    collectedNum += 1;
                }
                else
                {
                    break;
                }
            }
            if (collectedNum == childPositions.Count)
            {

                int acceptedID = 0;
                for (int i = 0; i < suspendedPieces2.Length; i++)
                {
                    int personalID = suspendedPieces2[i].GetComponent<CloneTravel>().whoamI[0];
                    int numNeeded = suspendedPieces2[i].GetComponent<CloneTravel>().whoamI[1];
                    if (i == 0)
                    {
                        acceptedID = personalID;
                    }
                    else
                    {
                        if (personalID != acceptedID)
                        {
                            correct = false;
                            break;
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
                    for (int i = 0; i < suspendedPieces2.Length; i++)
                    {
                        if(suspendedPieces2[i] != null)
                        {
                            suspendedPieces2[i].GetComponent<CloneTravel>().dropObject();
                            suspendedPieces2[i].GetComponent<CloneTravel>().onPedestal.a = false;
                            ListManagement(suspendedPieces2[i], i, false);

                        }
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
        for (int i =0; i<suspendedPieces2.Length; i++)
        {
            GameObject obj = suspendedPieces2[i];
            obj.GetComponent<CloneTravel>().onPedestal.d = true;
            obj.GetComponent<CloneTravel>().beginMovement(driftObj, obj.transform, driftObj.transform);

        }
    }
    //adds and removes different objects from the suspended pieces
    public void ListManagement(GameObject obj,int location, bool add)
    {
        if (add == true)
        {
            suspendedPieces2[location] = obj;
            obj.GetComponent<CloneTravel>().onPedestal.c = location;
        }
        else if (add == false)
        {
            
            filledlocations[location] = false;
            suspendedPieces2[location] = null;
        }


    }
}

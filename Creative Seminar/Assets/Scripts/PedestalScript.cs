using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestalScript : MonoBehaviour
{
    public List<Transform> childPositions;
    public List<int> filledlocations;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            childPositions.Add(child);
            filledlocations.Add(1);
        }
    }

    // Update is called once per frame
    public bool MovingtoPedestal(GameObject objectToMove)
    {
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
            objectToMove.GetComponent<CloneTravel>().beginMovement( gameObject, objectToMove.transform, childPositions[location]);
            GameObject MC = GameObject.FindGameObjectWithTag("Player");
            MC.GetComponent<PlayerScript>().carrying = false;

        }
        else if (foundLocation == false)
        {
            print("No Empty Spots");
        }
        return (foundLocation);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Come to Me is a script that brings items from the past into the present by moving towards the B-Cam hand and spawning a clone that follows the transform and offset
//On trigger exit with the B-cam will disable the mesh renderer on this object and enable the mesh on the clone
//Once the object reaches its final destination, it is destroyed and the clone exists with all the necessary ID info

public class ComeToMe : MonoBehaviour
{
    public Transform startMarker;
    public Transform endMarker;
    public Transform handMarker;
    public float speed = 5.0f;
    private float startTime;
    private float journeyLength;
    public bool discovered = false;
    public int TimeLine;
    public bool clone;
    public GameObject currentClone;
    public GameObject clonePrefab;
    public float timeFrame;
    public int puzzleID;
    public int completeNum;
    Transform cloneStart;
    public int photoID;
    private bool crossedPlane = false;
    public bool tooBig;
    public bool structPuzzle;
    public GameObject targetStruct;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        clone = false;
        journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
        Rigidbody Rb = gameObject.GetComponent<Rigidbody>();
        Rb.useGravity = false;
        if (structPuzzle)
        {
            targetStruct.GetComponent<StructDissolveHandler>().reqPuzzles = completeNum;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject MC = GameObject.FindGameObjectWithTag("Player");

        Vector3 currentPos = transform.position;
        float distToEnd = Vector3.Distance(currentPos, endMarker.position);
        if (discovered == true)
        {
            Rigidbody Rb = gameObject.GetComponent<Rigidbody>();
            Rb.useGravity = false;
            if (MC.GetComponent<PlayerScript>().dist == timeFrame && MC.GetComponent<PlayerScript>().photoaround)
            {
                Vector3 offset = new Vector3(timeFrame, 0, 0);
                float distCovered = (Time.time - startTime) * speed;
                float fractionOfJourney = distCovered / journeyLength;
                transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fractionOfJourney);
                currentClone.transform.position = transform.position - offset;
                currentClone.transform.rotation = transform.rotation;
            }
            else
            {
                discovered = false;
            }
            if(distToEnd <= .1f && crossedPlane)
            {
                transform.position = endMarker.position;
                if (gameObject.tag == "PickUp" | gameObject.tag == "StructPickup")
                {
                    currentClone.GetComponent<CloneTravel>().beginMovement(MC, cloneStart, handMarker, 5);
                }else if( gameObject.tag == "PhotoPickup")
                {
                    currentClone.GetComponent<PhotoPickUp>().endMove();
                }

            }

        }
        if (Input.GetKeyDown(KeyCode.E) && MC.GetComponent<PlayerScript>().photoaround == true)
        {
            discovered = false;
        }
        if (!discovered && !structPuzzle)
        {
            Rigidbody Rb = gameObject.GetComponent<Rigidbody>();
            Rb.useGravity = true;
            Destroy(currentClone);
        }
        if (structPuzzle)
        {
            if(currentClone != null && !discovered)
            {
                Rigidbody Rb = gameObject.GetComponent<Rigidbody>();
                Rb.useGravity = true;
                Destroy(currentClone);
            }
        }

    }
    public void SpawnChild(float windowNum)
    {
        startTime = Time.time;
        timeFrame = windowNum;
        startMarker = gameObject.transform;
        discovered = true;
        Vector3 offset = new Vector3(timeFrame, 0, 0);
        currentClone = Instantiate(clonePrefab, offset + transform.position, transform.rotation);
        currentClone.GetComponent<MeshRenderer>().enabled = false;
        cloneStart = currentClone.transform;
        //currentClone.GetComponent<CloneTravel>().endMarker = handMarker;

        if (gameObject.tag == "PickUp")
        {
            currentClone.GetComponent<CloneTravel>().Dad = gameObject;
            currentClone.GetComponent<CloneTravel>().whoamI.Add(puzzleID);
            currentClone.GetComponent<CloneTravel>().whoamI.Add(completeNum);
        }else if(gameObject.tag == "PhotoPickup")
        {
            currentClone.GetComponent<PhotoPickUp>().clone = true;
            currentClone.GetComponent<PhotoPickUp>().Dad = gameObject;
            currentClone.GetComponent<PhotoPickUp>().photoTimeFrame = photoID;
        }else if(gameObject.tag == "StructPickup")
        {
            currentClone.GetComponent<CloneTravel>().Dad = gameObject;
            currentClone.GetComponent<CloneTravel>().presentTarget = targetStruct;
            currentClone.GetComponent<CloneTravel>().whoamI.Add(puzzleID);
            currentClone.GetComponent<CloneTravel>().whoamI.Add(completeNum);
        }
        currentClone.name = gameObject.name + " Clone";

    }
    public void PlaneCross()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        currentClone.GetComponent<MeshRenderer>().enabled = true;
        if (tooBig | structPuzzle)
        {
            currentClone.GetComponent<CloneTravel>().dissolve = true;
        }
        crossedPlane = true;

    }

    public void GoodbyeFather()
    {
        Destroy(gameObject);
    }
}

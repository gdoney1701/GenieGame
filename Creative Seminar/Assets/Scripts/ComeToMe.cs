using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    GameObject currentClone;
    public GameObject clonePrefab;
    public float timeFrame;
    public int puzzleID;
    public int completeNum;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        clone = false;

        journeyLength = Vector3.Distance(startMarker.position, endMarker.position);

        
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
            if (MC.GetComponent<PlayerScript>().dist == timeFrame)
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

        }
        if (Input.GetKeyDown(KeyCode.E) && MC.GetComponent<PlayerScript>().photoaround == true)
        {
            discovered = false;
        }
        if (discovered == false)
        {
            Rigidbody Rb = gameObject.GetComponent<Rigidbody>();
            Rb.useGravity = true;
            Destroy(currentClone);
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
        currentClone.GetComponent<CloneTravel>().startMarker = currentClone.transform;
        currentClone.GetComponent<CloneTravel>().endMarker = handMarker;
        currentClone.GetComponent<CloneTravel>().Dad = gameObject;
        currentClone.GetComponent<CloneTravel>().whoamI.Add(puzzleID);
        currentClone.GetComponent<CloneTravel>().whoamI.Add(completeNum);
        currentClone.name = gameObject.name + " Child";

    }
    public void PlaneCross()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        currentClone.GetComponent<MeshRenderer>().enabled = true;

    }

    public void GoodbyeFather()
    {
        Destroy(gameObject);
    }
}

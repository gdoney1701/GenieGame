using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComeToMe : MonoBehaviour
{
    public Transform startMarker;
    public Transform endMarker;
    public Transform handMarker;
    public float speed;
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
    public bool readytoSpawn = false;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        clone = false;

        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GameObject MC = GameObject.FindGameObjectWithTag("Player");

        Vector3 currentPos = transform.position;
        float distToEnd = Vector3.Distance(currentPos, endMarker.position);
        if (discovered)
        {
            if (MC.GetComponent<PlayerScript>().dist == timeFrame)
            {
                Vector3 offset = new Vector3(timeFrame, 0, 0);
                float distCovered = (Time.time - startTime) * speed;
                float fractionOfJourney = distCovered / journeyLength;
                transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fractionOfJourney);
                if (readytoSpawn)
                {
                    currentClone.transform.position = transform.position - offset;
                    currentClone.transform.rotation = transform.rotation;
                }

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
    public void SpawnChild()
    {
        Vector3 offset = new Vector3(-timeFrame, 0, 0);
        currentClone = Instantiate(clonePrefab, offset + transform.position, transform.rotation);
        currentClone.GetComponent<MeshRenderer>().enabled = false;
        currentClone.GetComponent<CloneTravel>().startMarker = currentClone.transform;
        currentClone.GetComponent<CloneTravel>().endMarker = handMarker;
        currentClone.GetComponent<CloneTravel>().Dad = gameObject;
        currentClone.GetComponent<CloneTravel>().whoamI.Add(puzzleID);
        currentClone.GetComponent<CloneTravel>().whoamI.Add(completeNum);
        currentClone.name = gameObject.name + " Child";

    }
    public void movetoHand(float dist)
    {
        startTime = Time.time;
        timeFrame = dist;
        startMarker = gameObject.transform;
        Rigidbody Rb = gameObject.GetComponent<Rigidbody>();
        Rb.useGravity = false;
        discovered = true;
        journeyLength = Vector3.Distance(startMarker.position, endMarker.position);

    }
    public void PlaneCross(bool onContact)
    {
        if (onContact)
        {
            SpawnChild();
            currentClone.GetComponent<MeshRenderer>().enabled = true;
        }
        if (!onContact)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    public void GoodbyeFather()
    {
        Destroy(gameObject);
    }
}

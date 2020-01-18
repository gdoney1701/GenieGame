using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructuralPuzzlePickup : MonoBehaviour
{
    public Transform startMarker;
    public Transform endMarker;
    public float speed;
    float startTime;
    float journeyLength;
    public bool wanted = false;
    public int photoTimeFrame;
    public bool clone;
    public GameObject Dad;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        startMarker = transform;
        journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (wanted)
        {
            float distCovered = (Time.time - startTime) * speed;
            float fractionOfJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fractionOfJourney);
            float disttoEnd = Vector3.Distance(startMarker.position, endMarker.position);
            if (disttoEnd <= .1f)
            {
                endMove();
            }
        }
    }
    public void initMove()
    {
        wanted = true;
        startTime = Time.time;
        Rigidbody Rb = gameObject.GetComponent<Rigidbody>();
        Rb.useGravity = false;

    }

    public void endMove()
    {
        if (clone)
        {
            Destroy(Dad);
        }
        GameObject MC = GameObject.FindGameObjectWithTag("Player");
        if (!MC.GetComponent<PlayerScript>().havePhotos)
        {
            MC.GetComponent<PlayerScript>().dist = (photoTimeFrame * 200);
            Camera portalCam = MC.GetComponent<PlayerScript>().Bcam;
            portalCam.GetComponent<CopyPositionOffset>().offset = new Vector3(photoTimeFrame * 200, 0, 0);
            MC.GetComponent<PlayerScript>().havePhotos = true;
            MC.GetComponent<PlayerScript>().timeIndex = photoTimeFrame - 1;
        }
        MC.GetComponent<PlayerScript>().carriedPhotos[photoTimeFrame - 1] = true;
        print("New Photo Added");
        Destroy(gameObject);
    }
}

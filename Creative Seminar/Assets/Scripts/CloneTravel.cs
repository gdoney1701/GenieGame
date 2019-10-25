using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneTravel : MonoBehaviour
{
    public Transform startMarker;
    public Transform endMarker;
    public float speed = 5.0f;
    private float startTime;
    private float journeyLength;
    public bool traveling;
    public bool wanted;
    public GameObject Dad;


    // Start is called before the first frame update
    void Start()
    {

        foreach (Transform child in Camera.main.transform)
        {
            endMarker = child;
        }
        beginMovement(startMarker, endMarker);

    }

    // Update is called once per frame
    void Update()
    {
        //traveling functionality
        if (traveling == true)
        {
            float distCovered = (Time.time - startTime) * speed;
            float fractionOfJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fractionOfJourney);
            Rigidbody Rb = gameObject.GetComponent<Rigidbody>();
            Rb.useGravity = false;
        }
        //snaps object to player character
        float gettingCloser = Vector3.Distance(endMarker.position, transform.position);
        if (gettingCloser <= 0.1f && wanted == true && traveling == true)
        {
            print("Catch");
            GameObject MC = GameObject.FindGameObjectWithTag("Player");
            MC.GetComponent<PlayerScript>().carrying = true;
            MC.GetComponent<PlayerScript>().objectHeld.Add(gameObject);
            traveling = false;
            transform.position = endMarker.position;
            Destroy(Dad);

        }
        if (traveling == false && wanted == true)
        {
            transform.position = endMarker.position;
            transform.rotation = startMarker.rotation;
        }

    }

    public void dropObject()
    {
        print("Drop");
        wanted = false;
        Rigidbody Rb = gameObject.GetComponent<Rigidbody>();
        Rb.useGravity = true;
        GameObject MC = GameObject.FindGameObjectWithTag("Player");
        Rb.AddForce(MC.transform.forward * 2f, ForceMode.Impulse);
    }

    public void beginMovement(Transform starter, Transform ender)
    {
        wanted = true;
        startTime = Time.time;
        traveling = true;
        journeyLength = Vector3.Distance(starter.position, ender.position);
        startMarker = gameObject.transform;
        Rigidbody Rb = gameObject.GetComponent<Rigidbody>();
        Rb.useGravity = false;
        startMarker = starter;
        endMarker = ender;
    }
}

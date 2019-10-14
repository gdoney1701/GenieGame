using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneTravel : MonoBehaviour
{
    public Transform startMarker;
    public Transform endMarker;
    public float speed = 1.0f;
    private float startTime;
    private float journeyLength;
    bool traveling;
    bool wanted;
    public GameObject Dad;

    // Start is called before the first frame update
    void Start()
    {
        wanted = true;
        startTime = Time.time;
        traveling = true;
        journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
        startMarker = gameObject.transform;
        foreach (Transform child in Camera.main.transform)
        {
            endMarker = child;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (traveling == true)
        {
            float distCovered = (Time.time - startTime) * speed;
            float fractionOfJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fractionOfJourney);
        }

        float gettingCloser = Vector3.Distance(endMarker.position, transform.position);
        if (gettingCloser <= 0.1f && wanted == true)
        {
            print("Catch");
            GameObject MC = GameObject.FindGameObjectWithTag("Player");
            MC.GetComponent<PlayerScript>().carrying = true;
            traveling = false;
            transform.position = endMarker.position;
            Destroy(Dad);
            Rigidbody Rb = gameObject.GetComponent<Rigidbody>();
            Rb.useGravity = false;
        }
        if (traveling == false && wanted == true)
        {
            transform.position = endMarker.position;
            transform.rotation = startMarker.rotation;
        }

        if (Input.GetKeyDown(KeyCode.F) && traveling == false && wanted == true)
        {
            print("Drop");
            wanted = false;
            Rigidbody Rb = gameObject.GetComponent<Rigidbody>();
            Rb.useGravity = true;
            GameObject MC = GameObject.FindGameObjectWithTag("Player");
            MC.GetComponent<PlayerScript>().carrying = false;
        }
    }
}

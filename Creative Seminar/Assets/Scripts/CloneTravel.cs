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
    public List<GameObject> targetObject;
    public List<int> whoamI; // First index is the puzzle ID, the second index is the total number of same IDs to complete puzzle
    //public bool onPedestal;
    public PedestalGroup onPedestal;


    // Start is called before the first frame update
    void Start()
    {
        onPedestal.a = false;
        onPedestal.d = false;
        GameObject MC = GameObject.FindGameObjectWithTag("Player");
        foreach (Transform child in Camera.main.transform)
        {
            endMarker = child;
        }
        beginMovement(MC,startMarker, endMarker);

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
            if (targetObject[0].tag == "Player")
            {
                targetObject[0].GetComponent<PlayerScript>().carrying = true;
                targetObject[0].GetComponent<PlayerScript>().objectHeld.Add(gameObject);
                targetObject.RemoveAt(0);
            }else if (targetObject[0].tag == "Pedestal")
            {
                onPedestal.b = targetObject[0];
                //targetObject[0].GetComponent<PedestalScript>().suspendedPieces.Add(gameObject);
                targetObject[0].GetComponent<PedestalScript>().CombineCheckPedestal();
                targetObject.RemoveAt(0);
                onPedestal.a = true;
            }
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

    public void beginMovement(GameObject target, Transform starter, Transform ender)
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
        targetObject.Add(target);
    }
    //the bool determines if its on the pedestal, the gameobject is the pedestal, the int is the location in the list, and the final bool is whether the puzzle is complete
    public struct PedestalGroup
    {
        public bool a;
        public GameObject b;
        public int c;
        public bool d;
        public PedestalGroup(bool ifOn, GameObject onWhat, int whereAm, bool isComplete)
        {
            this.a = ifOn;
            this.b = onWhat;
            this.c = whereAm;
            this.d = isComplete;
        }
    }
}

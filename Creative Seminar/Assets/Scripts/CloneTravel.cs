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
    public List<int> whoamI; // First index is the puzzle ID, the second index is the total number of same IDs to complete puzzle
    //public bool onPedestal;
    public PedestalGroup onPedestal;
    public List<GameObject> targetObject;
    public bool dissolve;
    private float t = 0.0f;
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
        //beginMovement(MC,startMarker, endMarker, 5.0f);

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
            traveling = false;
            Destroy(Dad);
            transform.position = endMarker.position;
            if (targetObject[0].tag == "Player")
            {
                targetObject[0].GetComponent<PlayerScript>().carrying = true;
                targetObject[0].GetComponent<PlayerScript>().objectHeld.Add(gameObject);
                targetObject.RemoveAt(0);
            }else if (targetObject[0].tag == "Pedestal")
            {
                if (targetObject[0].GetComponent<PedestalScript>().puzzleComplete == false)
                {
                    onPedestal.b = targetObject[0];
                    targetObject[0].GetComponent<PedestalScript>().CombineCheckPedestal();
                    targetObject.RemoveAt(0);
                    onPedestal.a = true;
                }
                else
                {
                    targetObject[0].GetComponent<PedestalScript>().ListManagement(gameObject, onPedestal.c, false);
                }
            }


        }
        if (traveling == false && wanted == true)
        {
            transform.position = endMarker.position;
            transform.rotation = startMarker.rotation;
        }
        if (dissolve)
        {
            DissolveLerp(t);
            t += .005f;
        }
        if(t > 1f)
        {
            Destroy(gameObject);
        }
        

    }
    public void DissolveLerp(float time)
    {
        Vector3 journey = new Vector3(Mathf.Lerp(-.5f, 1f, time), 0, 0);
        print(journey);
        gameObject.GetComponent<Renderer>().material.SetFloat("Vector1_A2CB8D29", journey[0]);
    }

    public void dropObject()
    {
        print("Drop");
        wanted = false;
        Rigidbody Rb = gameObject.GetComponent<Rigidbody>();
        Rb.useGravity = true;
        GameObject MC = GameObject.FindGameObjectWithTag("Player");
        if (onPedestal.a)
        {
            Rb.AddForce(Random.onUnitSphere * 2f, ForceMode.Impulse);
        }
        else
        {
            Rb.AddForce(MC.transform.forward, ForceMode.Impulse);
        }

    }

    public void beginMovement(GameObject target, Transform starter, Transform ender, float newSpeed)
    {
        wanted = true;
        speed = newSpeed;
        startTime = Time.time;
        traveling = true;
        startMarker = starter;
        journeyLength = Vector3.Distance(starter.position, ender.position);
        Rigidbody Rb = gameObject.GetComponent<Rigidbody>();
        Rb.useGravity = false;
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

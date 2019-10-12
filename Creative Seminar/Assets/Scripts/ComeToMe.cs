using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComeToMe : MonoBehaviour
{
    public Transform startMarker;
    public Transform endMarker;
    public float speed = 1.0f;
    private float startTime;
    private float journeyLength;
    public bool discovered = false;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;

        journeyLength = Vector3.Distance(startMarker.position, endMarker.position);

        
    }

    // Update is called once per frame
    void Update()
    {
        if (discovered == true)
        {
            Vector3.Distance()
            float distCovered = (Time.time - startTime) * speed;
            float fractionOfJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fractionOfJourney);
        }
        

        
    }
}

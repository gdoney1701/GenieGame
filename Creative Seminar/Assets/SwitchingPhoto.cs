using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchingPhoto : MonoBehaviour
{
    public float dist;
    public Camera Bcam;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dist += 200;
            Bcam.GetComponent<CopyPositionOffset>().offset = new Vector3(dist, 0, 0);
        }

        if (Input.GetMouseButtonDown(1) && dist > 200)
        {
            dist -= 200;
            Bcam.GetComponent<CopyPositionOffset>().offset = new Vector3(dist, 0, 0);
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickingUp : MonoBehaviour
{
    public int target;
    public float maxPullDistance;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // creates a layer mask for the targetted layer
        int layerMask = 1 << target;

        //var mask2 = LayerMask.GetMask("Photo");

        print(layerMask);



        if (Input.GetKeyDown(KeyCode.F))
        {
            RaycastHit hit;
            if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, maxPullDistance, layerMask))
            {
                print("yup");
            //    if (hit.distance <= maxPullDistance)
            //    {
                    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.green, 10.0f);
            //    } else
            //    {
            //        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                }
                
            //} else
            //{
            //    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.red);
            //}
            
        }

        
    }

    //public bool RayMan()

}

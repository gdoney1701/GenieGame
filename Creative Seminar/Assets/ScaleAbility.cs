using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleAbility : MonoBehaviour
{
    Vector3 minScale = new Vector3(0.1f, 0.1f, 0.1f);
    Vector3 maxScale = new Vector3(3, 3, 3);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float zoomValue = Input.GetAxis("Mouse ScrollWheel");

        if (Input.GetKey(KeyCode.E))
        {
            if (zoomValue != 0)
            {
                transform.localScale += Vector3.one * zoomValue;
                transform.localScale = Vector3.Max(transform.localScale, minScale);
                transform.localScale = Vector3.Min(transform.localScale, maxScale);
            }

        }
        
    }
}

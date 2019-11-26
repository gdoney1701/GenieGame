using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleAbility : MonoBehaviour
{
    Vector3 minScale = new Vector3(0.01f, 0.01f, 0.01f);
    Vector3 maxScale = new Vector3(5, 5, 5);
    public GameObject PortalVisual;
    private bool doneScaling;
    private bool doneGrowing;
    Renderer portalRenderer;
    public float minimum = -0.5f;
    public float maximum = -0.8f;
    public float speed;
    static float t = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        portalRenderer = PortalVisual.GetComponent<Renderer>();
        portalRenderer.material.SetFloat("Boolean_448AB95D", 0);
        doneScaling = false;
        doneGrowing = false;
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
        else if (!doneScaling && !doneGrowing)
        {
            portalRenderer.material.SetFloat("Boolean_448AB95D", 1);
            doneScaling = true;

        }
        if (doneScaling && !doneGrowing)
        {

            Vector3 journey = new Vector3(Mathf.Lerp(minimum, maximum, t), 0, 0);
            t += 0.01f;
            float goal = journey[0];
            portalRenderer.material.SetFloat("Vector1_7431778D", goal);

            if (t > 1.0f)
            {
                float temp = maximum;
                maximum = minimum;
                minimum = maximum;
                t = 0.0f;
                doneGrowing = true;
            }
        }
        
    }
}

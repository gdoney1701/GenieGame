using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleAbility : MonoBehaviour
{
    Vector3 minScale = new Vector3(0.01f, 0.01f, 0.01f);
    Vector3 maxScale = new Vector3(5, 5, 5);
    public bool doneScaling;
    public bool doneGrowing;
    public bool greyShrink;
    public Renderer portalRenderer;
    public float minimum = -0.8f;
    public float maximum = -0.5f;
    private float minGrain = 5;
    private float maxGrain = 10;
    static float t = 0.0f;
    static float l = 0.0f;
    public bool isCurrent;
    // Start is called before the first frame update
    void Start()
    {
        portalRenderer.material.SetFloat("Boolean_448AB95D", 0);
        portalRenderer.material.SetFloat("Boolean_4B03E015", 1);
        doneScaling = false;
        doneGrowing = false;
        greyShrink = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isCurrent)
        {
            if (doneScaling && !greyShrink && !doneGrowing)
            {

                Vector3 journey = new Vector3(Mathf.Lerp(minimum, maximum, t), 0, 0);
                t += 0.02f;
                float goal = journey[0];
                portalRenderer.material.SetFloat("Vector1_7431778D", goal);
                if (t > 0.20f)
                {
                    portalRenderer.material.SetFloat("Boolean_4B03E015", 0);
                }
                if (t > 1.0f)
                {
                    float temp = maximum;
                    maximum = minimum;
                    minimum = temp;
                    greyShrink = true;
                    t = 0.0f;
                    portalRenderer.material.SetFloat("Boolean_448AB95D", 1);
                }
            }
            if (doneScaling && !doneGrowing && greyShrink)
            {
                Vector3 journey2 = new Vector3(Mathf.Lerp(minimum, maximum, t), 0, 0);
                t += 0.01f;
                float goal2 = journey2[0];
                portalRenderer.material.SetFloat("Vector1_7431778D", goal2);
                if (t > 1.0f)
                {
                    float temp = maximum;
                    maximum = minimum;
                    minimum = temp;
                    t = 0.0f;
                    doneGrowing = true;
                    greyShrink = true;
                    portalRenderer.material.SetFloat("Boolean_4B03E015", 1);
                }
            }

        }
    }
    private void Update()
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
            doneScaling = true;
        }
    }
}

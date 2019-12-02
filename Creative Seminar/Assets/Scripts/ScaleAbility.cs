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
    private float maxGrain = 20f;
    static float t = 0.0f;
    static float l = 0.0f;
    public bool isCurrent;
    // Start is called before the first frame update
    void Start()
    {
        portalRenderer.material.SetFloat("Boolean_448AB95D", 0);
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
                Vector3 changeGrain = new Vector3(Mathf.Lerp(minGrain, maxGrain, l), 0, 0);
                float currentGrain = changeGrain[0];
                t += 0.02f;
                float goal = journey[0];
                portalRenderer.material.SetFloat("Vector1_7431778D", goal);
                portalRenderer.material.SetFloat("Vector1_DA1B0552", changeGrain[0]);
                if (t > 1.0f)
                {
                    float temp = maximum;
                    maximum = minimum;
                    minimum = temp;

                    float tempGrain = currentGrain;
                    maxGrain = minGrain;
                    minGrain = tempGrain;
                    t = 0.0f;
                    greyShrink = true;
                    portalRenderer.material.SetFloat("Boolean_448AB95D", 1);
                }
            }
            if (doneScaling && !doneGrowing && greyShrink)
            {

                Vector3 journey2 = new Vector3(Mathf.Lerp(minimum, maximum, t), 0, 0);
                Vector3 changeGrain2 = new Vector3(Mathf.Lerp(minGrain, maxGrain, l), 0, 0);
                t += 0.01f;
                float currentGrain2 = changeGrain2[0];
                float goal2 = journey2[0];
                portalRenderer.material.SetFloat("Vector1_7431778D", goal2);
                portalRenderer.material.SetFloat("Vector1_DA1B0552", changeGrain2[0]);
                if (t > 1.0f)
                {
                    float temp = maximum;
                    maximum = minimum;
                    minimum = temp;
                    t = 0.0f;
                    doneGrowing = true;

                    float tempGrain = currentGrain2;
                    maxGrain = minGrain;
                    minGrain = tempGrain;
                    t = 0.0f;
                    greyShrink = true;
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

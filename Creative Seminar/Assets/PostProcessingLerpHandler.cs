using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingLerpHandler : MonoBehaviour
{
    public PostProcessVolume start;
    public PostProcessVolume end;
    bool primary;
    public int transitionNum;
    float segmentNum;
    bool shifting = false;
    float time =0;
    // Start is called before the first frame update
    void Start()
    {
        segmentNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (shifting)
        {
            float jStart =  ShiftLerp(segmentNum, 1/transitionNum+segmentNum,time,start);
            float jEnd = ShiftLerp(1-segmentNum, 1-(1/transitionNum+segmentNum), time, end);
            time += 0.001f;
            print(jStart);
            print(jEnd);
            if (jEnd <= .66f && jStart >= .33f)
            {
                segmentNum += (1 / transitionNum);
                time = 0;
                shifting = false;
            }
            
        }
    }
    public void ActivateChange()
    {
        print("changing profile");
        start.weight = 1;
        end.weight = 0;
        shifting = true;
        time = 0;
    }
    public float ShiftLerp(float start, float end, float time, PostProcessVolume who)
    {
        float journey = Mathf.Lerp(start, end, time);
        who.weight = journey;
        return journey;
    }
}

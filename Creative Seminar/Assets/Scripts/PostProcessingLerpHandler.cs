using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingLerpHandler : MonoBehaviour
{
    public PostProcessVolume colorProfile;
    public PostProcessVolume greyProfile;
    public float transitionNum;
    public float segmentNum = 0;
    public bool shifting = false;
    public float time =0;
    public float startPoint = 1;
    public float endPoint = 0;
    // Start is called before the first frame update
    void Start()
    {
        //segmentNum = 0;
        //startPoint = 1;
        //endPoint = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (shifting)
        {
            bool jGrey =  ShiftLerp(startPoint, 1-(1/transitionNum+segmentNum),time, greyProfile);
            bool jColor = ShiftLerp(endPoint, 1/transitionNum+segmentNum, time, colorProfile);
            time += Time.deltaTime*0.1f;
            if (jGrey && jColor)
            {
                print(1 / transitionNum);
                segmentNum += 1 / transitionNum;
                startPoint -= (1 /transitionNum);
                endPoint += (1 / transitionNum);
                time = 0;
                shifting = false;
            }
            
        }
    }
    public void ActivateChange()
    {
        shifting = true;
        time = 0;
    }
    public bool ShiftLerp(float start, float end, float time, PostProcessVolume who)
    {
        float journey = Mathf.Lerp(start, end, time);
        who.weight = journey;
        if (journey == end)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

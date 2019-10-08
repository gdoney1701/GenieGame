using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.LWRP;
using RenderPipeline = UnityEngine.Experimental.Rendering.RenderPipeline;


public class CopyPositionOffset : MonoBehaviour
{
    public Transform transformToCopy;
    public Vector3 offset;


    // Update is called once per frame
    void UpdateTest()
    {
        transform.position = transformToCopy.position + offset;
        transform.rotation = transformToCopy.rotation;
    }

    private void OnEnable()
    {
        RenderPipeline.beginCameraRendering += UpdateCamera;
    }

    private void OnDisable()
    {
        RenderPipeline.beginCameraRendering -= UpdateCamera;
    }

    void UpdateCamera(Camera camera)
    {
        UpdateTest();
    }
}

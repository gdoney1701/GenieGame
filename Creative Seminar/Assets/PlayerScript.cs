using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float dist;
    public Camera Bcam;
    public bool photoaround;
    public GameObject Photoprefab;
    public Transform BPhoto;
    public Transform GM_Cam;
    public float maxPullDistance;

    // Start is called before the first frame update
    void Start()
    {
        photoaround = false;
    }

    // Update is called once per frame
    void Update()
    {
        float zoomValue = Input.GetAxis("Mouse ScrollWheel");

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
        if (Input.GetKeyDown(KeyCode.E) && photoaround == false)
        {
            GameObject newPhoto = Instantiate(Photoprefab, GM_Cam.transform.position + (GM_Cam.transform.forward * 2), GM_Cam.transform.rotation);
            newPhoto.transform.Rotate(0, 180, 0, Space.World);
            newPhoto.GetComponent<Portal>().pairPortal = BPhoto;
            newPhoto.GetComponentInChildren<CopyPositionOffset>().transformToCopy = GM_Cam;
            photoaround = true;

        } else if (Input.GetKeyDown(KeyCode.E) && photoaround == true)
        {
            Destroy(GameObject.FindGameObjectWithTag("Photo"));
            photoaround = false;
                
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            bool cam1Hit = HitDat(10, Camera.main);
            bool cam2Hit = HitDat(9, Bcam);
            if (cam1Hit == true && cam2Hit == true)
            {
                print("GOTCHA");
            }
        }

    }
    public bool HitDat(int target, Camera whoamI)
    {
        bool hitme;
        int layerMask = 1 << target;
        RaycastHit hit;
        if(Physics.Raycast(whoamI.transform.position, whoamI.transform.TransformDirection(Vector3.forward), out hit, maxPullDistance, layerMask))
        {
            hitme = true;
        }
        else
        {
            hitme = false;
        }
        return hitme;
    }
}

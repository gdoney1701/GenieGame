using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float dist;
    public Camera Bcam;
    public bool photoaround;
    public GameObject Photoprefab;
    public GameObject photoColliders;
    public Transform BPhoto;
    public Transform GM_Cam;
    public float maxPullDistance;
    public bool carrying;

    // Start is called before the first frame update
    void Start()
    {
        carrying = false;
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
            GameObject portalColliders = GameObject.FindGameObjectWithTag("PhotoColliders");
            portalColliders.transform.position += new Vector3(200,0,0);
        }

        if (Input.GetMouseButtonDown(1) && dist > 200)
        {
            dist -= 200;
            Bcam.GetComponent<CopyPositionOffset>().offset = new Vector3(dist, 0, 0);
            GameObject portalColliders = GameObject.FindGameObjectWithTag("PhotoColliders");
            portalColliders.transform.position += new Vector3(-200, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.E) && photoaround == false)
        {
            MakeABaby(true,0.0f, Photoprefab);
            MakeABaby(false,dist, photoColliders);
            photoaround = true;

        } else if (Input.GetKeyDown(KeyCode.E) && photoaround == true)
        {
            Destroy(GameObject.FindGameObjectWithTag("Photo"));
            Destroy(GameObject.FindGameObjectWithTag("PhotoColliders"));
            photoaround = false;
                
        }
        if (Input.GetKeyDown(KeyCode.F) && carrying == false)
        {

            HitGroup cam1Hit = HitDat(10, Camera.main);
            HitGroup cam2Hit = HitDat(9, Bcam);
            if (cam1Hit.b == true && cam2Hit.b == true)
            {
                GameObject hitManLee = cam2Hit.a;
                hitManLee.GetComponent<ComeToMe>().SpawnChild(dist);

            }
        }
        print(dist);

    }
    public HitGroup HitDat(int target, Camera whoamI)
    {
        bool hitme;
        GameObject whathit;
        int layerMask = 1 << target;
        RaycastHit hit;
        //Debug.DrawRay(transform.position, transform.forward, Color.red, 10f);
        if (Physics.Raycast(whoamI.transform.position, whoamI.transform.TransformDirection(Vector3.forward), out hit, maxPullDistance, layerMask))
        {
            //Debug.DrawRay(transform.position, transform.forward, Color.green, 10f);
            hitme = true;
            whathit = hit.transform.gameObject;
        }
        else
        {
            hitme = false;
            whathit = null;
        }
        HitGroup toReturn = new HitGroup(hitme, whathit);
        return toReturn;
    }

    public struct HitGroup
    {
        public bool b;
        public GameObject a;

        public HitGroup(bool ifso, GameObject thenWhat)
        {
            this.b = ifso;
            this.a = thenWhat;
        }
    }
    public void MakeABaby(bool actualPhoto, float whereBaby, GameObject fabBaby)
    {
        Vector3 babyPoint = new Vector3(whereBaby, 0, 0);
        GameObject newPhoto = Instantiate(fabBaby, babyPoint + GM_Cam.transform.position + (GM_Cam.transform.forward * 2), GM_Cam.transform.rotation);
        newPhoto.transform.Rotate(0, 0, 0, Space.World);
        if (actualPhoto == true)
        {
            newPhoto.GetComponent<Portal>().pairPortal = BPhoto;
            newPhoto.GetComponentInChildren<CopyPositionOffset>().transformToCopy = GM_Cam;
        }

    }
}

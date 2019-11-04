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
    public List<GameObject> objectHeld;
    public bool carrying;
    public bool[] carriedPhotos;
    public bool devcheats;
    public bool havePhotos;
    public int timeIndex;

    // Start is called before the first frame update
    void Start()
    {
        havePhotos = false;
        carriedPhotos = new bool[5];
        carrying = false;
        photoaround = false;
        for (int i = 0; i<carriedPhotos.Length; i++)
        {
            carriedPhotos[i] = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K) && devcheats)
        {
            for (int i = 0; i < carriedPhotos.Length; i++)
            {
                carriedPhotos[i] = true;
            }
            havePhotos = true;
            timeIndex = 0;
        }
        float zoomValue = Input.GetAxis("Mouse ScrollWheel");

        //pressing the left mouse button will bring the timeframe forward by 1 (200 units)
        if (Input.GetMouseButtonDown(0))
        {
                offsetController(true);
        }

        //pressing the right button with bring the timeframe back by 1 (-200 units)
        if (Input.GetMouseButtonDown(1) && dist > 200)
        {
            offsetController(false);
        }

        if (Input.GetKeyDown(KeyCode.E) && !photoaround && havePhotos)
        {
                MakeABaby(true, 0.0f, Photoprefab);
                MakeABaby(false, dist, photoColliders);
                photoaround = true;

        } else if (Input.GetKeyDown(KeyCode.E) && photoaround && havePhotos)
        {
            Destroy(GameObject.FindGameObjectWithTag("Photo"));
            Destroy(GameObject.FindGameObjectWithTag("PhotoColliders"));
            photoaround = false;
                
        }

        //pickup functionality
        if (Input.GetKeyDown(KeyCode.F) && !carrying)
        {
            //Runs through the raycast checks for 4 edge cases
            HitGroup cam1Hit = HitDat(10, Camera.main); //checking if the photo is front of the player
            HitGroup cam2Hit = HitDat(9, Bcam); //checking if the bcam can see the object
            HitGroup cam1HitAgain = HitDat(9, Camera.main); //checks if the player can see an object in front of them
            HitGroup photoCheck = HitDat(16, Camera.main); //checks if the player can see the photo pickup
            if (cam1Hit.b == true && cam2Hit.b == true)
            {
                GameObject hitManLee = cam2Hit.a;
                hitManLee.GetComponent<ComeToMe>().SpawnChild(dist);

            } else if (cam1HitAgain.b) //searching to pick something up in the present
            {
                if (cam1HitAgain.a.GetComponent<CloneTravel>().onPedestal.d == false)
                {
                    if (cam1HitAgain.a.GetComponent<CloneTravel>().onPedestal.a == true)
                    {
                        GameObject pedestal = cam1HitAgain.a.GetComponent<CloneTravel>().onPedestal.b;
                        int loc = cam1HitAgain.a.GetComponent<CloneTravel>().onPedestal.c;
                        pedestal.GetComponent<PedestalScript>().ListManagement(cam1HitAgain.a,loc, false);
                    }
                    Transform targetHand = Camera.main.transform.GetChild(0);
                    GameObject pickUpPresent = cam1HitAgain.a;
                    pickUpPresent.GetComponent<CloneTravel>().beginMovement(gameObject, pickUpPresent.transform, targetHand, 5.0f);
                }
            }else if (photoCheck.b)
            {
                photoCheck.a.GetComponent<PhotoPickUp>().initMove();
            }
        }
        if (Input.GetKeyDown(KeyCode.F) && carrying == true)
        {
            HitGroup pedestalHit = HitDat(15, Camera.main);
            if (pedestalHit.b == true)
            {
                bool arewemoving = pedestalHit.a.GetComponent<PedestalScript>().MovingtoPedestal(objectHeld[0]);
                if (arewemoving == true)
                {
                    carrying = false;
                    objectHeld.RemoveAt(0);
                }
            }
            else if (pedestalHit.b == false)
            {
                GameObject thingHeld = objectHeld[0];
                thingHeld.GetComponent<CloneTravel>().dropObject();
                carrying = false;
                objectHeld.RemoveAt(0);
            }
        }
    }
    //HitGoup.a = the gameobject hit HitGroup.b is the booleon for whether anything was hit
    public HitGroup HitDat(int target, Camera whoamI)
    {
        bool hitme;
        GameObject whathit;
        int layerMask = 1 << target;
        RaycastHit hit;
        if (Physics.Raycast(whoamI.transform.position, whoamI.transform.TransformDirection(Vector3.forward), out hit, maxPullDistance, layerMask))
        {
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
            //ugly way of fixing a rather game breaking bug by turning b cam off and on again
            Bcam.gameObject.SetActive(false);
            Bcam.gameObject.SetActive(true);
        }

    }
    void offsetController(bool positive)
    {
        int negMod = 0;
        bool foundaloc = false;
        int newLoc = 0; 
        for (int i = 0; i < carriedPhotos.Length; i++)
        {
            if (carriedPhotos[i])
            {
                if(positive && i > timeIndex)
                {
                    foundaloc = true;
                    newLoc = i;
                    negMod = 1;
                    break;
                }else if(!positive && i < timeIndex)
                {
                    foundaloc = true;
                    newLoc = i;
                    negMod = 1;
                    break;
                }
                else
                {
                    foundaloc = false;
                }
                break;
            }
            else
            {
                foundaloc = false;
            }
        }
        if (foundaloc)
        {
            int movement = (Mathf.Abs((int)dist - ((newLoc + 1) * 200))) * negMod;
            dist = (newLoc + 1) * 200;
            Bcam.GetComponent<CopyPositionOffset>().offset = new Vector3(dist, 0, 0);
            if (photoaround)
            {
                GameObject portalColliders = GameObject.FindGameObjectWithTag("PhotoColliders");
                portalColliders.transform.position += new Vector3(movement, 0, 0);
            }
        }
    }
}

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
    public bool verticalOffset;
    public GameObject activeTutorial = null;

    // Start is called before the first frame update
    void Start()
    {
        //ugly way of fixing a rather game breaking bug by turning b cam off and on again
        Bcam.gameObject.SetActive(false);
        Bcam.gameObject.SetActive(true);
        Camera[] allCams2 = Camera.allCameras;
        for (int i = 0; i <allCams2.Length; i++)
        {
            if(allCams2[i].tag != "MainCamera")
            {
                allCams2[i].gameObject.SetActive(false);
                allCams2[i].gameObject.SetActive(true);
            }
        }


    }

    // Update is called once per frame
    void Update()
    {
        if(activeTutorial != null)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                TutorialCheck(KeyCode.W, false);
            }
        if (Input.GetKeyDown(KeyCode.A))
            {
                TutorialCheck(KeyCode.A, false);
            }
        if (Input.GetKeyDown(KeyCode.S))
            {
                TutorialCheck(KeyCode.S, false);
            }
        if (Input.GetKeyDown(KeyCode.D))
            {
                TutorialCheck(KeyCode.D, false);
            }
        if(Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                TutorialCheck(KeyCode.None, true);
            }
        }
        
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

        if(Input.GetKeyDown(KeyCode.X) && devcheats)
        {
            if(GameObject.FindGameObjectWithTag("PlayMan").GetComponent<GameplayManager>().currentScene == "Entrance")
            {
                GameObject.FindGameObjectWithTag("PlayMan").GetComponent<GameplayManager>().entPuzzles.done = true;
            }else if(GameObject.FindGameObjectWithTag("PlayMan").GetComponent<GameplayManager>().currentScene == "GreatHall")
            {
                print("GameOver");
                GameObject.FindGameObjectWithTag("PlayMan").GetComponent<GameplayManager>().ghPuzzles.done = true;
            }
            
        }
        //pressing the left mouse button will bring the timeframe forward by 1 (200 units)
        if (Input.GetMouseButtonDown(0))
        {
            offsetController(true);
            TutorialCheck(KeyCode.Mouse0, true);
        }

        if(Input.GetKeyDown(KeyCode.N) && devcheats)
        {
            GameObject.FindGameObjectWithTag("PlayMan").GetComponent<GameplayManager>().GreatHallLoad();
        }

        //pressing the right button with bring the timeframe back by 1 (-200 units)
        if (Input.GetMouseButtonDown(1))
        {
            offsetController(false);
        }

        if (Input.GetKeyDown(KeyCode.E) && !photoaround && havePhotos)
        {
            GameObject photo = MakeABaby(true, 0.0f, Photoprefab);
            TutorialCheck(KeyCode.E, true);
            //GameObject colliders = MakeABaby(false, dist, photoColliders);
            //GameObject colliders = MakeABaby(false, 0.0f, photoColliders);
            //colliders.GetComponent<ScaleAbility>().portalRenderer = photo.GetComponent<ScaleAbility>().portalRenderer;
            //photoaround = true;

        }
        else if (Input.GetKeyDown(KeyCode.E) && photoaround && havePhotos)
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
            //cam1HitAgain is for identifying pickups in the present 
            //photoCheck determines if the pickup is a photoPickup
            if (cam1Hit.b && cam2Hit.b)
            {
                GameObject hitManLee = cam2Hit.a;
                Vector3 distVect = new Vector3(0, 0, 0);
                if (verticalOffset)
                {
                    distVect = new Vector3(0, -dist, 0);
                }
                else
                {
                    distVect = new Vector3(dist, 0, 0);
                }
                hitManLee.GetComponent<ComeToMe>().SpawnChild(distVect, verticalOffset);


            }
            else if (cam1HitAgain.b) //searching to pick something up in the present
            {
                if (cam1HitAgain.a.GetComponent<CloneTravel>().onPedestal.d == false)
                {
                    if (cam1HitAgain.a.GetComponent<CloneTravel>().onPedestal.a == true)
                    {
                        GameObject pedestal = cam1HitAgain.a.GetComponent<CloneTravel>().onPedestal.b;
                        int loc = cam1HitAgain.a.GetComponent<CloneTravel>().onPedestal.c;
                        pedestal.GetComponent<PedestalScript>().ListManagement(cam1HitAgain.a, loc, false);
                    }
                    Transform targetHand = Camera.main.transform.GetChild(0);
                    GameObject pickUpPresent = cam1HitAgain.a;
                    pickUpPresent.GetComponent<CloneTravel>().beginMovement(gameObject, pickUpPresent.transform, targetHand, 5.0f);
                }
            }
            else if (photoCheck.b)
            {
                photoCheck.a.GetComponent<PhotoPickUp>().initMove();
                TutorialCheck(KeyCode.F, true);
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
    public void TutorialCheck(KeyCode toKey, bool conditionMet)
    {
        if(activeTutorial != null)
        {
            if (toKey == activeTutorial.GetComponent<TutorialManager>().toLearn)
            {
                activeTutorial.GetComponent<TutorialManager>().buttonPress(conditionMet, gameObject.GetComponent<Collider>());
            }else if(Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                activeTutorial.GetComponent<TutorialManager>().buttonPress(conditionMet, gameObject.GetComponent<Collider>());
            }
        }
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
    //MakeABaby creates a physical photo (fabBaby) with actualPhoto determining whether to create the photocolliders or the rendertexture portal
    public GameObject MakeABaby(bool actualPhoto, float whereBaby, GameObject fabBaby)
    {
        Vector3 babyPoint = new Vector3(0, 0, 0);
        if (verticalOffset)
        {
            babyPoint.y = -whereBaby;
        }
        else
        {
            babyPoint.x = whereBaby;
        }
        //creates the photo using the gameobject defined by the function at the spawnpoint with 
        GameObject newPhoto = Instantiate(fabBaby, babyPoint + GM_Cam.transform.position + (GM_Cam.transform.forward * 2), GM_Cam.transform.rotation);
        newPhoto.transform.Rotate(0, 0, 0, Space.World);
        if (actualPhoto)
        {
            //sets the rendertexture camera position
            newPhoto.GetComponent<Portal>().pairPortal = BPhoto;
            newPhoto.GetComponentInChildren<CopyPositionOffset>().transformToCopy = GM_Cam;
            newPhoto.GetComponent<ScaleAbility>().isCurrent = true;
            newPhoto.transform.parent = Camera.main.transform;
        }
        return newPhoto;
    }
    void offsetController(bool positive)
    {
        
        int negMod = 0;
        bool foundaloc = false;
        int newLoc = 0;
        if (positive)
        {
            for (int i = 0; i < carriedPhotos.Length; i++)
            {
                if (carriedPhotos[i])
                {
                    if (i > timeIndex)
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
                }
                else
                {
                    foundaloc = false;
                }
            }
        }else if (!positive)
        {
            for (int i = carriedPhotos.Length-1; i >= 0; i--)
            {
                if (carriedPhotos[i])
                {
                    if (i < timeIndex)
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
                }
                else
                {
                    foundaloc = false;
                }
            }
        }
       
        if (foundaloc)
        {
            timeIndex = newLoc;
            if (positive)
            {
                gameObject.GetComponent<UIManager>().MovingForward(timeIndex);
            }else if (!positive)
            {
                gameObject.GetComponent<UIManager>().MovingBackward(timeIndex);
            }

            int movement = (Mathf.Abs((int)dist - ((newLoc + 1) * 200))) * negMod;
            dist = (newLoc + 1) * 200;
            if (!verticalOffset)
            {
                Bcam.GetComponent<CopyPositionOffset>().offset = new Vector3(dist, 0, 0);
            }
            else
            {
                Bcam.GetComponent<CopyPositionOffset>().offset = new Vector3(0, -dist, 0);
            }
            if (photoaround)
            {
                GameObject pCols = GameObject.FindGameObjectWithTag("PhotoColliders");
                Transform pPos = GameObject.FindGameObjectWithTag("Photo").transform;
                if (!verticalOffset)
                {
                    pCols.transform.position = pPos.position + new Vector3(dist, 0, 0);
                }
                else
                {
                    pCols.transform.position = pPos.position + new Vector3(0, -dist, 0);
                }

            }
        }else if (!foundaloc)
        {
            if (positive)
            {
                gameObject.GetComponent<UIManager>().FailedMoveUp();
                print("Failedmove");
            }
            else if (!positive)
            {
                gameObject.GetComponent<UIManager>().FailedMoveDown();
                print("Failedmove");
            }

        }
    }
    public void createColliders(GameObject photo)
    {
        GameObject colliders = MakeABaby(false, dist, photoColliders);
        colliders.transform.position = photo.transform.position;
        colliders.transform.rotation = photo.transform.rotation;
        colliders.transform.localScale = photo.transform.localScale;
        if (!verticalOffset)
        {
            colliders.transform.position += new Vector3(dist,0,0);
        }
        else
        {
            colliders.transform.position += new Vector3(0, -dist, 0);
        }
        photoaround = true;
    }
}

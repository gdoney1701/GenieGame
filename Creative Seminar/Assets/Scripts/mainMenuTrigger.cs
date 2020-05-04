using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainMenuTrigger : MonoBehaviour
{
    public GameObject masterManager;
    public GameObject portal;
    public bool clicked = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && !clicked)
        {
            RaycastHit hitTarget;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitTarget))
            {
                if (hitTarget.transform.name == "Circle")
                {
                    masterManager.GetComponent<GameplayManager>().PlayGame();
                    clicked = true;
                }
            }
        }
    }
    void BringItDown()
    {

    }
}

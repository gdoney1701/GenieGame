using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitScript : MonoBehaviour
{
    public GameObject manager;
    bool collected = false;
    private void Start()
    {
        manager = GameObject.FindGameObjectWithTag("PlayMan");
    }

    private void OnTriggerEnter(Collider other)
    {

        if(other.tag == "Player" && manager.GetComponent<GameplayManager>().entPuzzles.done)
        {
            print("Loading Scene");
            if(manager.GetComponent<GameplayManager>().enableDemo == true)
            {
                manager.GetComponent<GameplayManager>().DemoIntLoad();
            }
            else
            {
                manager.GetComponent<GameplayManager>().GreatHallLoad();
            }

        }
    }

}

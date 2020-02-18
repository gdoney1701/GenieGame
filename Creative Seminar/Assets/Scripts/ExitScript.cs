using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitScript : MonoBehaviour
{
    public GameObject manager;
    bool collected = false;
    private void OnTriggerEnter(Collider other)
    {

        if(other.tag == "Player" && manager.GetComponent<GameplayManager>().entPuzzles.done)
        {
            print("Loading Scene");
            //manager.GetComponent<GameplayManager>().GreatHallLoad();
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndofGameUI : MonoBehaviour
{

    public void returntoMenu()
    {
        GameObject.FindGameObjectWithTag("PlayMan").GetComponent<GameplayManager>().MainMenuLoad();
    }

    public void returntoDesktop()
    {
        GameObject.FindGameObjectWithTag("PlayMan").GetComponent<GameplayManager>().ExitGame();
    }
}

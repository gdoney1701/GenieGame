using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndofGameUI : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void returntoMenu()
    {
        GameObject.FindGameObjectWithTag("PlayMan").GetComponent<GameplayManager>().MainMenuLoad();
    }

    public void returntoDesktop()
    {
        GameObject.FindGameObjectWithTag("PlayMan").GetComponent<GameplayManager>().ExitGame();
    }
    public void continueGame()
    {
        GameObject.FindGameObjectWithTag("PlayMan").GetComponent<GameplayManager>().GreatHallLoad();
    }
}

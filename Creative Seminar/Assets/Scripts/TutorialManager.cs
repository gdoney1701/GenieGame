using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public KeyCode toLearn;
    public Animator keyAnim;
    public bool insideZone = false;
    public bool complete = false;
    public bool reqMet;
    public GameObject reqObj;

    private void OnTriggerEnter(Collider other)
    {
        beginTutorial(other);

    }
    public void beginTutorial(Collider other)
    {
        insideZone = true;
        print(other.name);
        if (reqMet)
        {
            other.GetComponent<PlayerScript>().activeTutorial = gameObject;
            StartCoroutine(FadeIn());
        }
    }
    IEnumerator FadeIn()
    {
        keyAnim.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
    }
    IEnumerator FadeOut()
    {
        keyAnim.SetTrigger("EndOfTime");
        yield return new WaitForSeconds(1);
    }
    private void OnTriggerExit(Collider other)
    {
        if (reqMet)
        {
            other.GetComponent<PlayerScript>().activeTutorial = null;
            StartCoroutine(FadeOut());
        }


    }
    IEnumerator buttonBoom(bool learned, Collider player)
    {
        keyAnim.SetTrigger("ButtonPress");
        yield return new WaitForSeconds(1.5f);

        if (learned)
        {
            StartCoroutine(FadeOut());
            if (reqObj != null)
            {
                reqObj.GetComponent<TutorialManager>().reqMet = true;
                reqObj.GetComponent<TutorialManager>().beginTutorial(player);

            }
        }
        
    }
    public void buttonPress(bool learnedKey, Collider player)
    {
        StartCoroutine(buttonBoom(learnedKey, player));
    }

}

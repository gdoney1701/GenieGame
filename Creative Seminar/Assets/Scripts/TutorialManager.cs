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

    private void OnTriggerEnter(Collider other)
    {
        insideZone = true;
        print(other.name);
        other.GetComponent<PlayerScript>().activeTutorial = gameObject;
        StartCoroutine(FadeIn());
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
        other.GetComponent<PlayerScript>().activeTutorial = null;
        StartCoroutine(FadeOut());

    }
    IEnumerator buttonBoom(bool learned)
    {
        keyAnim.SetTrigger("ButtonPress");
        yield return new WaitForSeconds(1.5f);
        if (learned)
        {
            StartCoroutine(FadeOut());
        }
    }
    public void buttonPress(bool learnedKey)
    {
        StartCoroutine(buttonBoom(learnedKey));
    }

}

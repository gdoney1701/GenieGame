using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour
{
    public Animator[] dialogue;
    public Animator[] nextScene;
    public int lineDelay;

    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(Director());

    }

    IEnumerator Director()
    {
        StartCoroutine(textFadeIn(dialogue));
        yield return new WaitForSeconds(lineDelay*(dialogue.Length));
        StartCoroutine(textFadeIn(nextScene));
        yield return new WaitForSeconds(lineDelay);
        GameObject.FindGameObjectWithTag("PlayMan").GetComponent<GameplayManager>().EntranceLoad();
    }
    IEnumerator textFadeIn(Animator[] textToParse)
    {
        for(int i =0; i <textToParse.Length; i++)
        {
            textToParse[i].SetTrigger("FadeFromBlack");
            yield return new WaitForSeconds(lineDelay);
        }
        StartCoroutine(textFadeOut(dialogue));


    }
    IEnumerator textFadeOut(Animator[] textToParse)
    {
        yield return new WaitForSeconds(lineDelay);
        for (int i = 0; i < textToParse.Length; i ++)
        {
            textToParse[i].SetTrigger("FadeAway");
        }
        yield return new WaitForSeconds(lineDelay);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject.FindGameObjectWithTag("PlayMan").GetComponent<GameplayManager>().EntranceLoad();
        }
    }
}

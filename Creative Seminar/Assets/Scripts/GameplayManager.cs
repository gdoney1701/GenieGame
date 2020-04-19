using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{
    public puzzleHandler entPuzzles;
    public puzzleHandler ghPuzzles;
    public Scene currentScene;
    public Animator transition;
    public float transitionTime = 2;
    public GameObject toBlack;
    public GameObject player;
    public Camera loadCam;
    // Start is called before the first frame update
    void Start()
    {
        toBlack.GetComponent<Image>().SetAllDirty();
        SceneManager.LoadScene("Entrance_1030", LoadSceneMode.Additive);
        StartCoroutine(DissolveHandler(false));
        DontDestroyOnLoad(gameObject);
        entPuzzles = new puzzleHandler(2, 1);
        ghPuzzles = new puzzleHandler(11, 3);
    }
    private void Awake()
    {
        
    }
    public void PlayGame()
    {
        List<string> entranceLevels = new List<string>();
        entranceLevels.Add("Tutorial 11_3");
        entranceLevels.Add("Entrance_0900");
        entranceLevels.Add("Entrance_1030");
        StartCoroutine(LoadLevel(entranceLevels, 0));
    }
    public void GreatHallLoad()
    {
        player = null;
        List<string> hallLevels = new List<string>();
        hallLevels.Add("GreatHall 1_23");
        hallLevels.Add("GH_0900");
        hallLevels.Add("GH_1030");
        hallLevels.Add("GH_1130");
        hallLevels.Add("GH_1200");
        StartCoroutine(LoadLevel(hallLevels, 1));
    }
    IEnumerator LoadLevel(List<string> toLoad, int matIndex)
    {
        StartCoroutine(DissolveHandler(true));
        yield return new WaitForSeconds(transitionTime+1);
        Physics.autoSimulation = false;
        for(int i =0; i < toLoad.Count; i++)
        {
            if (i == 0)
            {
                AsyncOperation opLoad = SceneManager.LoadSceneAsync(toLoad[i]);
                currentScene = SceneManager.GetSceneByName(toLoad[i]);

                while (!opLoad.isDone)
                {
                    Debug.Log(opLoad.progress);

                    yield return null;
                }
                player = GameObject.FindGameObjectWithTag("Player");
                player.SetActive(false);
                loadCam.enabled = true;
                print("Finished Scene Main");
            }
            else
            {

                AsyncOperation opLoad = SceneManager.LoadSceneAsync(toLoad[i], LoadSceneMode.Additive);

                while (!opLoad.isDone)
                {
                    Debug.Log(opLoad.progress);

                    yield return null;
                }
            }
            print("Finished " + toLoad[i]);
        }
        player.SetActive(true);
        loadCam.enabled = false;
        Physics.autoSimulation = true;
        StartCoroutine(DissolveHandler(false));

    }
    IEnumerator DissolveHandler(bool start)
    {
        float elapsedTime = 0f;
        if (start)
        {
            while(elapsedTime < transitionTime)
            {
                float j = Mathf.Lerp(0.6f, 0, elapsedTime);
                toBlack.GetComponent<Image>().material.SetFloat("_DissolveControl", j);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            while (elapsedTime < transitionTime)
            {
                float j = Mathf.Lerp(0, 0.6f, elapsedTime);
                toBlack.GetComponent<Image>().material.SetFloat("_DissolveControl", j);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }
    public void PuzzleComplete(int deltaObj, int deltaStruct)
    {
        GameObject handlerTarget = GameObject.FindGameObjectWithTag("Process Handler");
        handlerTarget.GetComponent<PostProcessingLerpHandler>().ActivateChange();
        if(currentScene.buildIndex == 1)
        {
            entPuzzles.objCount -= deltaObj;
            entPuzzles.structCount -= deltaStruct;
            if(entPuzzles.objCount == 0 && entPuzzles.structCount == 0)
            {
                print("Entrance Complete, I'm a very proud dev");
                entPuzzles.done = true;
            }
        }else if(currentScene.buildIndex == 4)
        {
            ghPuzzles.objCount -= deltaObj;
            ghPuzzles.structCount -= deltaStruct;
            if (ghPuzzles.objCount == 0 && ghPuzzles.structCount == 0)
            {
                print("Great Hall Complete, I'm a very proud dev");
                print("Congratulations, you have won");
            }
        }
    }
    public struct puzzleHandler
    {
        public int objCount;
        public int structCount;
        public bool done;

        public puzzleHandler(int inputObj, int inputStruct)
        {
            this.objCount = inputObj;
            this.structCount = inputStruct;
            this.done = false;
        }
    }
}

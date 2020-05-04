using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{
    public puzzleHandler entPuzzles;
    public puzzleHandler ghPuzzles;
    public string currentScene;
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
    private void Update()
    {
        if (ghPuzzles.done == true && currentScene == "GreatHall")
        {
            WinScreenLoad();
        }
    }
    public void PlayGame()
    {
        List<string> introLevels = new List<string>();
        introLevels.Add("Intro");
        StartCoroutine(LoadLevel(introLevels, "Intro"));
    }
    public void EntranceLoad()
    {
        List<string> entranceLevels = new List<string>();
        entranceLevels.Add("Tutorial 11_3");
        entranceLevels.Add("Entrance_0900");
        entranceLevels.Add("Entrance_1030");
        StartCoroutine(LoadLevel(entranceLevels, "Entrance"));
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
        StartCoroutine(LoadLevel(hallLevels, "GreatHall"));
    }
    public void MainMenuLoad()
    {
        List<string> menuLevels = new List<string>();
        menuLevels.Add("MainMenu");
        StartCoroutine(LoadLevel(menuLevels, "MainMenu"));
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    public void WinScreenLoad()
    {
        List<string> winScreen = new List<string>();
        player = null;
        winScreen.Add("WinScene");
        StartCoroutine(LoadLevel(winScreen, "Intro"));
    }
    IEnumerator LoadLevel(List<string> toLoad, string mainScene)
    {
        StartCoroutine(DissolveHandler(true));
        yield return new WaitForSeconds(transitionTime+1);
        Physics.autoSimulation = false;
        for(int i =0; i < toLoad.Count; i++)
        {
            if(mainScene == "Intro" || mainScene == "MainMenu")
            {
                SceneManager.LoadScene(toLoad[i]);
            }
            else
            {
                if (i == 0)
                {
                    AsyncOperation opLoad = SceneManager.LoadSceneAsync(toLoad[i]);
                    currentScene = mainScene;

                    while (!opLoad.isDone)
                    {
                        Debug.Log(opLoad.progress);

                        yield return null;
                    }
                    player = GameObject.FindGameObjectWithTag("Player");
                    player.SetActive(false);
                    loadCam.enabled = true;
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
            }
            
        }
        if (mainScene != "Intro" && mainScene != "MainMenu")
        {
            player.SetActive(true);
            Physics.autoSimulation = true;
        }
        loadCam.enabled = false;
        if(mainScene == "GreatHall")
        {
            GameObject mainPlay = GameObject.FindGameObjectWithTag("Player");
            mainPlay.GetComponent<UIManager>().ghInit();
        }
        
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
        if(currentScene == "Tutorial")
        {
            entPuzzles.objCount -= deltaObj;
            entPuzzles.structCount -= deltaStruct;
            if(entPuzzles.objCount <= 0 && entPuzzles.structCount <= 0)
            {
                entPuzzles.done = true;
            }
        }else if(currentScene == "MainScene")
        {
            ghPuzzles.objCount -= deltaObj;
            ghPuzzles.structCount -= deltaStruct;
            if (ghPuzzles.objCount <= 0 && ghPuzzles.structCount <= 0)
            {
                ghPuzzles.done = true;
                print("Great Hall Complete, I'm a very proud dev");
                print("Congratulations, you have won");
                WinScreenLoad();
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

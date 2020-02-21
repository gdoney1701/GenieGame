using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    public puzzleHandler entPuzzles;
    public puzzleHandler ghPuzzles;
    public Scene currentScene;
    // Start is called before the first frame update
    void Start()
    {

        DontDestroyOnLoad(gameObject);
        entPuzzles = new puzzleHandler(2, 1);
        ghPuzzles = new puzzleHandler(11, 4);
    }
    private void Awake()
    {
        
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(1); // Loads Entrance Present (Gameplay Level)
        currentScene = SceneManager.GetSceneAt(1);
        SceneManager.LoadScene(2, LoadSceneMode.Additive); // Loads Entrance 0900 (Past 1)
        SceneManager.LoadScene(3, LoadSceneMode.Additive); //Loads Entrance 1030 (Past 2) 
    }
    public void GreatHallLoad()
    {
        SceneManager.LoadScene("GreatHall 1_23"); //Loads Great Hall Present (Gameplay Level)
        currentScene = SceneManager.GetSceneByName("GreatHall 1_23");
        SceneManager.LoadScene("GH_0900", LoadSceneMode.Additive); //Loads Great Hall 0900 (Past 1)
        SceneManager.LoadScene("GH_1030", LoadSceneMode.Additive); //Loads Great Hall 1030 (Past 2)
        SceneManager.LoadScene("GH_1130", LoadSceneMode.Additive); //Loads Great Hall 1130 (Past 3)
        SceneManager.LoadScene("GH_1200", LoadSceneMode.Additive); //Loads Great Hall 1200 (Past 4)
    }

    public void PuzzleComplete(int deltaObj, int deltaStruct)
    {
        print("Puzzle Complete Logged");
        print(currentScene.buildIndex);
        if(currentScene.buildIndex == 1)
        {
            entPuzzles.objCount -= deltaObj;
            entPuzzles.structCount -= deltaStruct;
            print(entPuzzles.objCount);
            print(entPuzzles.structCount);
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

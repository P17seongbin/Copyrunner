using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Game_Start()
    {
        //SceneManager.UnloadSceneAsync("Game_Start_Scene");
        SceneManager.LoadSceneAsync("Main", LoadSceneMode.Single);
    }

    public void Game_ReStart()
    {
        //SceneManager.UnloadSceneAsync("Main");
        Debug.Log("Hello");
        SceneManager.LoadSceneAsync("Main", LoadSceneMode.Single);
    }

    public void GoTo_Menu()
    {
        //SceneManager.UnloadSceneAsync("Game_Start_Scene");
        //SceneManager.UnloadSceneAsync("Main");
        
        SceneManager.LoadSceneAsync("Game_Start_Scene", LoadSceneMode.Single);
    }
}

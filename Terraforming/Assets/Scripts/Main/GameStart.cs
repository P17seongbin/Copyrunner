﻿using System.Collections;
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
        SceneManager.LoadScene("Main");
    }

    public void GoTo_Menu()
    {
        SceneManager.LoadScene("Game_Start_Scene");
    }
}

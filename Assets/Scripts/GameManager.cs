﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver;


    private void Update()
	{
		//if the r key was pressed
		//restart the current scene
		if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
		{
            //Current Game Scene
			SceneManager.LoadScene(1); //more optimized way via index
			//SceneManager.LoadScene("Game");
		}
	}


    public void GameOver()
	{
		Debug.Log("GameManager::GameOver() Called");
        _isGameOver = true;
	}

}

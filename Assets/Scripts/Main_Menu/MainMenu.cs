using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
   //has to public so it display in onclick events for new game button i.e unity could see it
   public void LoadGame()
	{
        //load the game scene
        SceneManager.LoadScene(1); //game scene
	}
}

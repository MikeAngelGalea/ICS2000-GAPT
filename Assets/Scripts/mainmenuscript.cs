using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public void PlayGame(string sceneName){
            SceneManager.LoadScene(sceneName);
    }
    
   public void QuitGame(){
    Application.Quit();
   } 
}

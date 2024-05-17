using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    //load scene by name
    public void PlayGame(string sceneName){
        //specified scenename parameter
            SceneManager.LoadScene(sceneName);
    }
    
    //quit game method
   public void QuitGame(){
    #if UNITY_EDITOR
    //stop game if running unity editor
        UnityEditor.EditorApplication.isPlaying = false;
    #else
    //quit application if running in built application
        Application.Quit();
    #endif
}
 
}

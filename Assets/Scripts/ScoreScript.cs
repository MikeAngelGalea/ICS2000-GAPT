using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreScript : MonoBehaviour
{
    public TMP_Text MyscoreText; 
    private int ScoreNum;

    // calling start before the first frame update
    void Start()
    {
        //initializing score to 0
        ScoreNum = 00;
        //updating score text on ui
        MyscoreText.text = "Score : " + ScoreNum;
    }

    private void OnTriggerEnter2D(Collider2D collectable)
    {
        //checking if collided object has tag collectible (astronaut and spanner)
        if (collectable.CompareTag("Collectible"))
        {
            ScoreNum += 1;
            //destroying collectible object
            Destroy(collectable.gameObject);
            //updating score text on ui
            MyscoreText.text = "Score : " + ScoreNum;
        }
    }
}

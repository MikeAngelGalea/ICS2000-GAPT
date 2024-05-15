using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreScript : MonoBehaviour
{
    public TMP_Text MyscoreText; 
    private int ScoreNum;

    // Start is called before the first frame update
    void Start()
    {
        ScoreNum = 00;
        MyscoreText.text = "Score : " + ScoreNum;
    }

    private void OnTriggerEnter2D(Collider2D collectable)
    {
        if (collectable.CompareTag("Collectible"))
        {
            ScoreNum += 1;
            Destroy(collectable.gameObject);
            MyscoreText.text = "Score : " + ScoreNum;
        }
    }
}

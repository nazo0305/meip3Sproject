using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCount : MonoBehaviour
{
    int score = 0;
    public Text scoreText;

   
    public void AddScore()
    {
        score++;
    }

    void Update()
    {
        scoreText.text = "SCORE: " + score.ToString();
    }
}
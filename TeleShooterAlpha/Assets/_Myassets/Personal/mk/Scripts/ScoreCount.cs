using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCount : MonoBehaviour
{
    int score = 0;
    public Text scoreText;
    [SerializeField] int mulscore=100;

   
    public void AddScore()
    {
        score+=mulscore;
    }

    void Update()
    {
        scoreText.text = "SCORE: " + score.ToString();
    }
}
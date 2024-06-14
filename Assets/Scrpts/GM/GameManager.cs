using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using TMPro;

public class GameManager : NetworkBehaviour
{
    [SerializeField] TextMeshProUGUI redScoreText;
    [SerializeField] TextMeshProUGUI blueScoreText;
    [SerializeField] TextMeshProUGUI gameResultText;
    [SerializeField] Timer timer;

    private int redScore = 0;
    private int blueScore = 0;

    void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        redScore = 0;
        blueScore = 0;
        UpdateScoreText();
        StartRound();
        gameResultText.text = "";
    }

    public void StartRound()
    {
        redScore = 0;
        blueScore = 0;
        UpdateScoreText();
        timer.StartTimer();
    }

    public void EndRound()
    {
        string result;
        if (redScore > blueScore)
        {
            result = "Red Team Wins!";
        }
        else if (blueScore > redScore)
        {
            result = "Blue Team Wins!";
        }
        else
        {
            result = "Draw!";
        }

        gameResultText.text = result;
        Invoke("ReturnToMainMenu", 5f);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void AddScore(string team, int score)
    {
        if (team == "Red")
        {
            redScore += score;
            redScoreText.text = redScore.ToString();
        }
        else if (team == "Blue")
        {
            blueScore += score;
            blueScoreText.text = blueScore.ToString();
        }
    }

    public void RemoveScore(string team, int score)
    {
        if (team == "Red")
        {
            redScore -= score;
            redScore = Mathf.Max(0, redScore);
            redScoreText.text = redScore.ToString();
        }
        else if (team == "Blue")
        {
            blueScore -= score;
            blueScore = Mathf.Max(0, blueScore);
            blueScoreText.text = blueScore.ToString();
        }
    }

    void UpdateScoreText()
    {
        redScoreText.text = redScore.ToString();
        blueScoreText.text = blueScore.ToString();
    }
}

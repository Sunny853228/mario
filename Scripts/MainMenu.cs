using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI Score, MaxScore;
    public void Start()
    {
        Score.text = "Score: " + PlayerPrefs.GetInt("Score").ToString();
        MaxScore.text = "MaxScore: " + PlayerPrefs.GetInt("MaxScore").ToString();
    }
    public void Play (int index)
    {
        PlayerPrefs.SetInt("Score", 0);
        SceneManager.LoadScene(index);
    }

    public void Exit()
    {
        Application.Quit();
    }
}

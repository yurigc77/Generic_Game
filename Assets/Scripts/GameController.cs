using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public int score;
    public Text scoreText;

    public GameObject gameOverPanel;

    private void Awake()
    {
        Time.timeScale = 1;

        DontDestroyOnLoad(this);

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if(PlayerPrefs.GetInt("score")>0)
        {
            score = PlayerPrefs.GetInt("score");
            scoreText.text = "x " + score.ToString();
        }
    }

    public void GetCoin()
    {
        score++;

        scoreText.text = "x "+score.ToString();

        PlayerPrefs.SetInt("score",score);
    }



    public void ShowGameOver()
    {
        Time.timeScale = 0;
        gameOverPanel.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

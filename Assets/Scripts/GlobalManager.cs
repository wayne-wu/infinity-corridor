using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class GlobalManager : MonoBehaviour
{
    public float paceMultiplier = 0.0001f;
    public AudioClip gameOverClip;

    float scoreTimer;
    int score;
    int highScore;

    bool gameOver = false;

    float paceTimer;
    // float pace;

    WorldGenerator generator;
    PlayerControl player;
    TMP_Text scoreText;
    TMP_Text highScoreText;

    List<GameObject> gameOverObjs;

    // Start is called before the first frame update
    void Start()
    {
        // Need to make sure we reset the gravity each time since
        // it's a global setting.
        Physics.gravity = new Vector3(0, -9.81f, 0);

        score = 0;
        highScore = PlayerPrefs.GetInt("highscore");

        generator = GameObject.FindObjectOfType<WorldGenerator>();
        player = GameObject.FindObjectOfType<PlayerControl>();

        gameOverObjs = new List<GameObject>();

        Canvas canvas = FindObjectOfType<Canvas>();
        int childCount = canvas.transform.childCount;
        for(int i = 0; i < childCount-2; i++)
        {
            gameOverObjs.Add(canvas.gameObject.transform.GetChild(i).gameObject);
        }

        scoreText = canvas.gameObject.transform.GetChild(
            childCount - 2).gameObject.GetComponent<TMP_Text>();
        highScoreText = canvas.gameObject.transform.GetChild(
            childCount - 1).gameObject.GetComponent<TMP_Text>();

        highScoreText.text = String.Format("HIGHSCORE: {0}", highScore);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver) return;

        scoreTimer += Time.deltaTime;
        if (scoreTimer > 1.0f)
        {
            score++;
            scoreText.text = String.Format("SCORE: {0}", score);
            scoreTimer = 0;
        }

        paceTimer += Time.deltaTime;
        if (paceTimer > 25f)
        {
            generator.speed += 0.01f;
            paceTimer = 0;
        }

        if (Mathf.Abs(player.transform.position.x) > 6.0f ||
            Mathf.Abs(player.transform.position.y) > 6.0f)
            EndGame();
    }

    public void EndGame()
    {
        if (gameOver) return;

        if (player.hasInvisible)
        {
            player.MakeInvisible();
        }
        else
        {
            gameOver = true;

            generator.stop = true;
            player.Die();

            foreach (GameObject obj in gameOverObjs)
                obj.SetActive(true);

            if(score > highScore)
            {
                highScore = score;
                PlayerPrefs.SetInt("highscore", highScore);
                PlayerPrefs.Save();
            }

            AudioSource.PlayClipAtPoint(gameOverClip, player.transform.position);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}

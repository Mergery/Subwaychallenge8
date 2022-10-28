using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private const int COIN_SCORE_AMOUNT = 5;
    public static GameManager Instance { set; get; }

    public bool IsDead { set; get; }
    private bool isGameStarted = false;
    private PlayerController controller;

    //Death Menu
    public Animator deathMenuAnim;
    public TextMeshProUGUI deadScoreText, deadCoinText;

    //UI and the UI fields
    public Animator gameCanvas;
    public Animator menuCanvas;
    public GameObject scorePanel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI modifierText, highScoreText;
    private float score;
    private float coinScore;
    private float modifierScore;
    private int lastScore;

    private void Awake()
    {
        Instance = this;
        modifierScore = 1;
        controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();


        scoreText.text = score.ToString("0");
        coinText.text = coinScore.ToString("0");
        modifierText.text = "x" + modifierScore.ToString("0.0");
        highScoreText.text = PlayerPrefs.GetInt("High Score").ToString();
    }

    private void Update()
    {
        if (MobileInput.Instance.Tap && !isGameStarted)
        {
            isGameStarted = true;
            controller.StartRunning();
            FindObjectOfType<GlacierSpawner>().IsScrolling = true;
            FindObjectOfType<CameraController>().IsMoving = true;
            gameCanvas.SetTrigger("Show");
            menuCanvas.SetTrigger("Hide");
        }

        if (isGameStarted && !IsDead)
        {
            // Bump the score up
            score += (Time.deltaTime * modifierScore);
            if (lastScore != (int)score)
            {
                lastScore = (int)score;
                Debug.Log(lastScore);

                scoreText.text = score.ToString("0");
            }
        }
    }

    public void GetCoin()
    {
        coinScore ++;
        coinText.text = coinScore.ToString("0");
        score += COIN_SCORE_AMOUNT;
        scoreText.text = score.ToString("0");
    }


    public void UpdateModifier(float modifierAmount)
    {
        modifierScore = 1.0f + modifierAmount;
        modifierText.text = "x" + modifierScore.ToString("0.0");
    }

    public void OnPlayButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game Scene");
    }

    public void OnDeath()
    {
        IsDead = true;
        FindObjectOfType<GlacierSpawner>().IsScrolling = false;
        deadScoreText.text = score.ToString("0");
        deadCoinText.text = coinScore.ToString("0");
        deathMenuAnim.SetTrigger("Dead");
        gameCanvas.SetTrigger("Hide");

        // Check if this a highscore
        if (score > PlayerPrefs.GetInt("High Score"))
        {
            float s = score;
            if (s % 1 == 0)
            {
                s += 1;
            }
            PlayerPrefs.SetInt("High Score", (int)score);
        }
    }
}

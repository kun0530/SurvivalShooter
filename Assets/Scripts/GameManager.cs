using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }

    public UiManager uiManager;

    private int score;
    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            uiManager.SetScoreText(score);
        }
    }
    public bool IsPaused { get; private set; }
    public bool IsGameOver { get; private set; }

    private void Start()
    {
        if (Instance != this)
            Destroy(gameObject);

        Score = 0;
        IsPaused = false;
        IsGameOver = false;
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (IsGameOver)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                    Restart();
                return;
            }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseOrResumeGame();
        }
    }

    public void PauseOrResumeGame()
    {
        IsPaused = !IsPaused;
        Time.timeScale = IsPaused ? 0f : 1f;
        uiManager.pauseUi.SetActive(IsPaused);
    }

    public void PauseOrResumeGame(bool isPaused)
    {
        IsPaused = !isPaused;
        PauseOrResumeGame();
    }

    public void QuitGame()
    {
        Debug.Log("종료!");
        Application.Quit();
    }

    public void GameOver()
    {
        if (IsGameOver)
            return;
        
        IsGameOver = true;
        uiManager.gameOverText.enabled = true;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
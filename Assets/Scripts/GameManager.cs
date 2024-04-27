using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    private void Update()
    {
        if (IsGameOver)
            return;

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
}
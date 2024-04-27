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
    public bool isGameOver { get; private set; }

    private void Start()
    {
        if (Instance != this)
            Destroy(gameObject);

        Score = 0;
    }

    private void Update()
    {
        // 점수 UI 테스트
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Score += 100;
        }
    }
}
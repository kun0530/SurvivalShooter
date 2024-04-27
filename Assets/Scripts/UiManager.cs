using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    private static readonly string scoreFormat = "SCORE: ";

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;

    void Start()
    {
        scoreText.enabled = true;
        gameOverText.enabled = false;
    }

    public void SetScoreText(int score)
    {
        scoreText.text = $"{scoreFormat}{score}";
    }
}

using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    public GameObject container;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;

    public float maxTimeForBonus = 900f;
    public int pointsPerSavedSecond = 10;

    void Start()
    {
        container.SetActive(false);
    }

    public void ShowWinScreen()
    {
        container.SetActive(true);
        Time.timeScale = 0f;

        CalculateAndDisplayScore();
    }

    private void CalculateAndDisplayScore()
    {
        int finalScore = 1000;

        PlayerHealth player = FindAnyObjectByType<PlayerHealth>();
        if(player  != null )
        {
            float healthPercentage = player.CurrentHealth/player.MaxHealth;
            finalScore += Mathf.RoundToInt(healthPercentage * 500);
        }

        LevelManager levelManager = FindAnyObjectByType<LevelManager>();
        if (levelManager != null)
        {
            if(levelManager.KilledPersistentEnemyIds != null)
            {
                finalScore += levelManager.KilledPersistentEnemyIds.Count * 50;
            }
            if(levelManager.PickedUpPersistentItemIds != null)
            {
                finalScore += levelManager.PickedUpPersistentItemIds.Count * 100;
            }

            float timePlayed = levelManager.TotalPlayTime;
            int minutes = Mathf.FloorToInt(timePlayed / 60F);
            int seconds = Mathf.FloorToInt(timePlayed - minutes * 60);
            string timeInfo = string.Format("{0:00}:{1:00}", minutes, seconds);

            if(timeText != null)
            {
                timeText.text = "TIME: " + timeInfo;
            }

            if(timePlayed < maxTimeForBonus)
            {
                float secondsSaved = maxTimeForBonus - timePlayed;
                finalScore += Mathf.RoundToInt(secondsSaved * pointsPerSavedSecond);
            }
        }

        if(scoreText != null)
        {
            scoreText.text = "SCORE: " + finalScore.ToString();
        }
    }

    public void MainMenuButton()
    {
        SaveSystem.DeleteSave();
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitButton()
    {
        #if UNITY_EDITOR
        SaveSystem.DeleteSave();
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        SaveSystem.DeleteSave();
        Application.Quit();
        #endif
    }
}

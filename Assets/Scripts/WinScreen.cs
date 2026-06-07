using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    public GameObject container;

    void Start()
    {
        container.SetActive(false);
    }

    public void ShowWinScreen()
    {
        container.SetActive(true);
        Time.timeScale = 0f;
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

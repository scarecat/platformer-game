using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PauseMenu : MonoBehaviour
{
    public GameObject container;
    public GameObject settingsMenu;
    public Slider volumeSlider;
    public Toggle fullscreenToggle;

    [Header("D¿wiêki")]
    public AudioClip pauseSound;
    public AudioClip clickSound;

    private InputAction pauseAction;

    void Start()
    {
        Time.timeScale = 1.0f;
        pauseAction = InputSystem.actions.FindAction("Pause");
        pauseAction.performed += OnPausePressed;
    }

    void OnPausePressed(InputAction.CallbackContext context)
    {
        AudioManager.Instance.PlaySFX(pauseSound);
        container.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeButton()
    {
        AudioManager.Instance.PlaySFX(clickSound);
        container.SetActive(false);
        Time.timeScale = 1;
    }

    public void SettingsButton()
    {
        container.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void MainMenuButton()
    {
        AudioManager.Instance.PlaySFX(clickSound);
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitButton()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    public void ReturnButton()
    {
        container.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    public void SetFullscreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
}

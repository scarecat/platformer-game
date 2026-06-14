using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public Button continueButton;
    public Slider volumeSlider;
    public Toggle fullscreenToggle;

    private void Start()
    {
        if(continueButton != null)
        {
            continueButton.interactable = SaveSystem.SaveExists();
        }
    }

    public void ContinueButton()
    {
        SceneManager.LoadScene("Game");
    }

    public void NewGameButton()
    {
        SaveSystem.DeleteSave();
        SceneManager.LoadScene("Game");
    }

    public void SettingsButton()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
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
        mainMenu.SetActive(true);
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

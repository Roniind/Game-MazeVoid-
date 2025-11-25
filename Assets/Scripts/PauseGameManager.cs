using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseGameManager : MonoBehaviour
{
    public TextMeshProUGUI LevelText;
    public GameObject PauseCanvas;
    public KeyCode PauseKey = KeyCode.Tab;
    bool isPaused;

    private void Update()
    {
        LevelText.text = SceneManager.GetActiveScene().name;

        // Corregido: 'input' -> 'Input'
        if (Input.GetKeyUp(PauseKey)) 
        {
            isPaused = !isPaused;

            if (!isPaused) ResumeGame();
        }

        if (isPaused)
        {
            PauseCanvas.SetActive(true);
            Time.timeScale = 0;
            AudioListener.pause = true;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // Evita error si no hay componente FirstPersonMovement
            FirstPersonMovement movement = FindObjectOfType<FirstPersonMovement>();
            if (movement != null)
                movement.enabled = false;
        }
        else
        {
            PauseCanvas.SetActive(false);
        }
    }

    public void ResumeGame()
    {
        isPaused = false;

        Time.timeScale = 1;
        AudioListener.pause = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        FirstPersonMovement movement = FindObjectOfType<FirstPersonMovement>();
        if (movement != null)
            movement.enabled = true;
    }

    public void QuitGame()
    {
        isPaused = false;

        Time.timeScale = 1;
        AudioListener.pause = false;

        SceneManager.LoadScene("Main Menu");
    }

    public void RestartLevel()
    {
        ResumeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

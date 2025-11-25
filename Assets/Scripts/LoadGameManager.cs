using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Falta para usar Button

public class LoadGameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI LoadLevelText;
    [SerializeField] Button LoadButton; // Estaba mal escrito como "Buttom"

    void Awake()
    {
        // Corrige el uso incorrecto de PlayerPrefs.GetString
        if (string.IsNullOrWhiteSpace(PlayerPrefs.GetString("loaded level", " ")))
        {
            LoadButton.interactable = false;
        }
        else
        {
            LoadButton.interactable = true;
            LoadLevelText.text = PlayerPrefs.GetString("loaded level", " ");
        }
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(PlayerPrefs.GetString("loaded level"));
    }
}

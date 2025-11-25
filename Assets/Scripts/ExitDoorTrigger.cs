using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ExitDoorTrigger : MonoBehaviour
{
    public GameObject WinUI;
    public TextMeshProUGUI LevelText;

    private void Start()
    {
        // Asegura que la UI de victoria esté desactivada al inicio
        WinUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Desactivar el collider de la puerta
        GetComponent<BoxCollider>().enabled = false;

        // Desactivar el movimiento del jugador
        FindObjectOfType<FirstPersonMovement>().enabled = false;


        // Mostrar el cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Mostrar UI de victoria
        WinUI.SetActive(true);
    }

    private void Update()
    {
        // Mostrar el nombre del nivel actual en pantalla
        LevelText.text = SceneManager.GetActiveScene().name;
    }
}

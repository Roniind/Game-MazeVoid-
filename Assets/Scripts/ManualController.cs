using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualController : MonoBehaviour
{
    [Header("UI")]
    public GameObject manualPanel;
    public KeyCode openKey = KeyCode.Tab;

    [Header("Sonidos")]
    public AudioSource audioSource;
    public AudioClip openSound;
    public AudioClip closeSound;

    private bool isOpen = false;

    void Start()
    {
        // Abrir manual automáticamente al iniciar el nivel
        isOpen = false;     // Asegura que empiece cerrado
        ToggleManual();     // Lo abre al iniciar
    }

    void Update()
    {
        if (Input.GetKeyDown(openKey))
        {
            ToggleManual();
        }
    }

    void ToggleManual()
    {
        isOpen = !isOpen;
        manualPanel.SetActive(isOpen);

        if (isOpen)
            PlaySound(openSound);
        else
            PlaySound(closeSound);

        LockPlayer(isOpen);
    }

    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }

    void LockPlayer(bool state)
    {
        FirstPersonMovement move = FindObjectOfType<FirstPersonMovement>();
        if (move != null)
            move.canMove = !state;

        Jump jump = FindObjectOfType<Jump>();
        if (jump != null)
            jump.enabled = !state;

        Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = state;
    }
}


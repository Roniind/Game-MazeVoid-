using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MachinePuzzle : MonoBehaviour
{
    [Header("Configuración")]
    public int puzzleIndex; // 0 = máquina 1, 1 = máquina 2, 2 = máquina 3
    public PuzzleManager puzzleManager;
    public KeyCode interactKey = KeyCode.E;

    [Header("UI de interacción")]
    public GameObject interactUI;        
    public TextMeshProUGUI interactText;

    private bool playerInRange = false;

    void Start()
    {
        if (puzzleManager == null)
            Debug.LogError("FALTA asignar PuzzleManager en: " + gameObject.name);

        if (interactUI != null)
            interactUI.SetActive(false);
    }

    void Update()

    {
        if (!playerInRange) return;

        // Si ya está completada
        if (puzzleManager.IsPuzzleCompleted(puzzleIndex))
        {
            if (interactText != null)
                interactText.text = "Acertijo completado ✓";
            return;
        }
        else
        {
            if (interactText != null)
                interactText.text = "Presiona E para interactuar";
        }

        // PRESIONAR E
        if (Input.GetKeyDown(interactKey))
        {
            puzzleManager.ShowPuzzle(puzzleIndex);
        }
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (interactUI != null)
                interactUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (interactUI != null)
                interactUI.SetActive(false);
        }
    }
}

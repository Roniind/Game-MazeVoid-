using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PuzzleManager : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject PuzzlePanel;
    public TextMeshProUGUI QuestionText;
    public TMP_InputField AnswerInput;
    public TextMeshProUGUI FeedbackText;
    public Button SubmitButton;

    public TextMeshProUGUI[] taskTexts;
    public Slider StressBar;

    [Header("Configuración de acertijos")]
    public PuzzleData[] puzzles;

    private int currentPuzzleIndex = -1;
    private Dictionary<int, bool> completedPuzzles = new Dictionary<int, bool>();

    // -------------------------
    // BLOQUEAR / DESBLOQUEAR JUGADOR
    // -------------------------
    private void LockPlayer(bool state)
    {
        // Usar la variable canMove del FirstPersonMovement (no .enabled)
        FirstPersonMovement move = FindObjectOfType<FirstPersonMovement>();
        if (move != null)
        {
            move.canMove = !state;
            Debug.Log($"[PuzzleManager] FirstPersonMovement.canMove = {!state}");
        }
        else
        {
            Debug.LogWarning("[PuzzleManager] No se encontró FirstPersonMovement en la escena.");
        }

        // Salto
        Jump jump = FindObjectOfType<Jump>();
        if (jump != null)
        {
            jump.enabled = !state;
            Debug.Log($"[PuzzleManager] Jump.enabled = {!state}");
        }

        // Cursor
        if (state)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // Limpia selección del EventSystem cuando desbloqueas (para evitar inputs fantasma)
        if (!state && EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(null);
    }

    // -------------------------
    // INICIALIZACIÓN
    // -------------------------
    void Awake()
    {
        puzzles = new PuzzleData[3];

        puzzles[0] = new PuzzleData()
        {
            question = "Piensa un número. Si lo multiplicas por 4 y le restas 6 obtienes 30. ¿Cuál es el número?",
            correctAnswer = "9",
            completedTaskText = "Máquina 1 - Acertijo completado ✓"
        };

        puzzles[1] = new PuzzleData()
        {
            question = "¿Cuál es el siguiente número en la secuencia? 2, 3, 5, 9, 17, ?",
            correctAnswer = "33",
            completedTaskText = "Máquina 2 - Acertijo completado ✓"
        };

        puzzles[2] = new PuzzleData()
        {
            question = "Si 5 máquinas fabrican 5 piezas en 5 minutos, ¿cuánto tardarán 100 máquinas en fabricar 100 piezas?",
            correctAnswer = "5",
            completedTaskText = "Máquina 3 - Acertijo completado ✓"
        };
    }

    void Start()
    {
        // Asegúrate de que las referencias estén asignadas
        if (PuzzlePanel == null) Debug.LogError("[PuzzleManager] PuzzlePanel no asignado.");
        if (QuestionText == null) Debug.LogError("[PuzzleManager] QuestionText no asignado.");
        if (AnswerInput == null) Debug.LogError("[PuzzleManager] AnswerInput no asignado.");
        if (FeedbackText == null) Debug.LogError("[PuzzleManager] FeedbackText no asignado.");
        if (SubmitButton == null) Debug.LogError("[PuzzleManager] SubmitButton no asignado.");
        if (StressBar == null) Debug.LogError("[PuzzleManager] StressBar no asignado.");
        if (taskTexts == null || taskTexts.Length < 3) Debug.LogWarning("[PuzzleManager] taskTexts no totalmente asignado (esperado 3).");

        // Asegurar listener del boton (evita no tenerlo conectado en inspector)
        SubmitButton.onClick.RemoveAllListeners();
        SubmitButton.onClick.AddListener(SubmitAnswer);

        HidePuzzle();
        FeedbackText.text = "";

        StressBar.maxValue = 100;
        StressBar.value = 0;

        for (int i = 0; i < puzzles.Length; i++)
            completedPuzzles[i] = false;
    }

    // -------------------------
    // MOSTRAR EL ACERTIJO
    // -------------------------
    public void ShowPuzzle(int puzzleIndex)
    {
        if (puzzleIndex < 0 || puzzleIndex >= puzzles.Length)
        {
            Debug.LogError("[PuzzleManager] showPuzzle: índice fuera de rango: " + puzzleIndex);
            return;
        }

        currentPuzzleIndex = puzzleIndex;
        PuzzlePanel.SetActive(true);

        QuestionText.text = puzzles[puzzleIndex].question;
        AnswerInput.text = "";
        FeedbackText.text = "";

        // Forzar foco en el InputField
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(AnswerInput.gameObject);
            AnswerInput.ActivateInputField();
        }

        LockPlayer(true);
        Debug.Log("[PuzzleManager] ShowPuzzle " + puzzleIndex);
    }

    // -------------------------
    // OCULTAR EL ACERTIJO
    // -------------------------
    public void HidePuzzle()
    {
        PuzzlePanel.SetActive(false);
        currentPuzzleIndex = -1;

        LockPlayer(false);
        Debug.Log("[PuzzleManager] HidePuzzle()");
    }

    // -------------------------
    // VALIDAR RESPUESTA
    // -------------------------
    public void SubmitAnswer()
    {
        if (currentPuzzleIndex == -1)
        {
            Debug.Log("[PuzzleManager] SubmitAnswer llamado pero currentPuzzleIndex == -1");
            return;
        }

        string playerAnswer = AnswerInput.text.Trim().ToLower();
        string correctAnswer = puzzles[currentPuzzleIndex].correctAnswer.ToLower();

        Debug.Log("Respuesta ingresada: " + playerAnswer);

        if (playerAnswer == correctAnswer)
        {
            FeedbackText.text = "¡CORRECTO!";
            FeedbackText.color = Color.green;

            if (taskTexts != null && currentPuzzleIndex < taskTexts.Length)
            {
                taskTexts[currentPuzzleIndex].text = puzzles[currentPuzzleIndex].completedTaskText;
                taskTexts[currentPuzzleIndex].color = Color.gray;
                taskTexts[currentPuzzleIndex].fontStyle = FontStyles.Strikethrough;
            }

            StressBar.value = Mathf.Max(0, StressBar.value - 20);
            completedPuzzles[currentPuzzleIndex] = true;

            StartCoroutine(ClosePuzzleAfterDelay(0.8f));
        }
        else
        {
            FeedbackText.text = "Incorrecto, ¡intenta de nuevo!";
            FeedbackText.color = Color.red;
            StressBar.value = Mathf.Min(100, StressBar.value + 15);
        }
    }

    // -------------------------
    // CORRUTINA PARA CERRAR PANEL
    // -------------------------
    IEnumerator ClosePuzzleAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        HidePuzzle();
    }

    public bool IsPuzzleCompleted(int index)
    {
        return completedPuzzles.ContainsKey(index) && completedPuzzles[index];
    }
}

[System.Serializable]
public class PuzzleData
{
    public string question;
    [TextArea] public string correctAnswer;
    public string completedTaskText;
}

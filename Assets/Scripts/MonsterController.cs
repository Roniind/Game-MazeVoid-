using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public GameObject monsterObject;  // El modelo del monstruo
    public float showTime = 2f;       // Tiempo visible
    public AudioSource audioSource;  
    public AudioClip screamSound;

    private bool isActive = false;

    public void TriggerMonster()
    {
        if (isActive) return;  // Evita spam de sustos

        StartCoroutine(MonsterRoutine());
    }

    IEnumerator MonsterRoutine()
    {
        isActive = true;

        // Activar monstruo
        monsterObject.SetActive(true);

        // Sonido
        if (audioSource != null && screamSound != null)
            audioSource.PlayOneShot(screamSound);

        // Bloquear jugador
        LockPlayer(true);

        yield return new WaitForSeconds(showTime);

        // Ocultar monstruo
        monsterObject.SetActive(false);

        // Desbloquear movimiento
        LockPlayer(false);

        // Cooldown antes de permitir otro susto
        yield return new WaitForSeconds(2f);
        isActive = false;
    }

    void LockPlayer(bool state)
    {
        FirstPersonMovement move = FindObjectOfType<FirstPersonMovement>();
        if (move != null) move.canMove = !state;

        Jump jump = FindObjectOfType<Jump>();
        if (jump != null) jump.enabled = !state;

        Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = state;
    }
}

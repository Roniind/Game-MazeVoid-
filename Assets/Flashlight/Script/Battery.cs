using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{
    [Header("Options")]
    [Tooltip("The amount of battery the player gains when collecting this item.")]
    [SerializeField] int batteryWeight;

    [Tooltip("Key that needs to be pressed to collect this battery.")]
    [SerializeField] KeyCode CollectKey = KeyCode.E;

    [Header("References")]
    [Tooltip("Objects that are shown when the player hovers over the battery (like UI prompts).")]
    [SerializeField] GameObject[] HoverObject;

    private FlashlightManager flashlightManager;

    private void Start()
    {
        flashlightManager = FindObjectOfType<FlashlightManager>();

        // Asegura que los objetos de hover estén ocultos al inicio
        if (HoverObject != null)
        {
            foreach (GameObject obj in HoverObject)
                obj.SetActive(false);
        }
    }

    private void OnMouseOver()
    {
        // Mostrar objetos de hover (por ejemplo, el mensaje “Presiona E para recoger”)
        if (HoverObject != null)
        {
            foreach (GameObject obj in HoverObject)
                obj.SetActive(true);
        }

        // Si el jugador presiona la tecla para recoger
        if (Input.GetKeyDown(CollectKey))
        {
            if (flashlightManager != null)
            {
                flashlightManager.GainBattery(batteryWeight);
            }

            // Destruye la batería después de recogerla
            Destroy(gameObject);
        }
    }

    private void OnMouseExit()
    {
        // Oculta los objetos de hover al dejar de apuntar
        if (HoverObject != null)
        {
            foreach (GameObject obj in HoverObject)
                obj.SetActive(false);
        }
    }
}

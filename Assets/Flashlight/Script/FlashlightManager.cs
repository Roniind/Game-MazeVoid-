using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlashlightState
{
    Off,
    On,
    Dead
}

[RequireComponent(typeof(AudioSource))]
public class FlashlightManager : MonoBehaviour
{
    [Header("Options")]
    [Tooltip("The speed that the battery is lost at.")]
    [Range(0.0f, 2f)]
    [SerializeField]
    float BatteryLossTick = 0.5f;

    [Tooltip("This is the amount of battery the player starts with.")]
    [SerializeField]
    int startBattery = 100;

    [Tooltip("This is the amount of the battery that the player currently has.")]
    public int currentBattery;

    [Tooltip("The current state of the flashlight.")]
    public FlashlightState state;

    [Tooltip("Is the flashlight on?")]
    private bool flashlightIsOn;

    [Tooltip("The key that is required to be pressed to turn on/off the flashlight.")]
    [SerializeField]
    KeyCode ToggleKey = KeyCode.F;

    [Header("References")]
    [Tooltip("The light that will be shown if the flashlight is on.")]
    [SerializeField]
    GameObject FlashlightLight; // corregido el nombre

    [Tooltip("Sound that will be played when the flashlight is toggled.")]
    [SerializeField]
    AudioClip FlashlightOn_FX, FlashlightOff_FX;

    private AudioSource audioSource;

    private void Start()
    {
        currentBattery = startBattery;
        state = FlashlightState.Off;
        audioSource = GetComponent<AudioSource>();
        InvokeRepeating(nameof(LoseBattery), 0, BatteryLossTick);

        // Asegurar que la linterna empiece apagada
        if (FlashlightLight != null)
            FlashlightLight.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(ToggleKey))
            ToggleFlashlight();

        // Control de visibilidad según el estado
        if (FlashlightLight != null)
        {
            FlashlightLight.SetActive(state == FlashlightState.On);
        }

        // Si la batería se acaba
        if (currentBattery <= 0)
        {
            currentBattery = 0;
            state = FlashlightState.Dead;
            flashlightIsOn = false;
        }
    }

    public void GainBattery(int amount)
    {
        // Si estaba muerta, revive al ganar batería
        if (state == FlashlightState.Dead && amount > 0)
        {
            state = FlashlightState.Off;
        }

        currentBattery = Mathf.Min(currentBattery + amount, startBattery);
    }

    private void LoseBattery()
    {
        if (state == FlashlightState.On)
        {
            currentBattery--;
            if (currentBattery <= 0)
            {
                currentBattery = 0;
                state = FlashlightState.Dead;
            }
        }
    }

    private void ToggleFlashlight()
    {
        if (state == FlashlightState.Dead)
            return;

        flashlightIsOn = !flashlightIsOn;
        state = flashlightIsOn ? FlashlightState.On : FlashlightState.Off;

        // Reproducir sonidos al encender o apagar
        if (audioSource != null)
        {
            if (flashlightIsOn && FlashlightOn_FX != null)
                audioSource.PlayOneShot(FlashlightOn_FX);
            else if (!flashlightIsOn && FlashlightOff_FX != null)
                audioSource.PlayOneShot(FlashlightOff_FX);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpscareTrigger : MonoBehaviour
{
    public Animation  JumpscareAnimation;
    public AudioSource JumpscareAudio;

    public void OnMouseOver()
    {
        JumpscareAnimation.Play();

        if (JumpscareAudio != null )
        {
            JumpscareAudio.Play();
        }
        this.gameObject.SetActive(false);


    }
}

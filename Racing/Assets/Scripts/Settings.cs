using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Settings : MonoBehaviour {

    public AudioMixer audioMixer;
    public Movement movement;

	public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetLaps(float laps)
    {
        if(laps > 1)
        {
            movement.numberOfLaps = laps;
        }

        else
        {
            movement.numberOfLaps = 1;
        }
    }
}

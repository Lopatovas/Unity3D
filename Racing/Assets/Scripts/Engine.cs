using UnityEngine;
using System.Collections;

public class Engine : MonoBehaviour
{
    //audiosource reference
    private AudioSource carSound;

    //the range for audio source pitch
    private const float lowPtich = 0.3f;
    private const float highPitch = 0.8f;
    private const float reductionFactor = .1f;

    //Rigidbody2D carRigidbody;
    private float userInput;

    Movement movement;

    void Awake()
    {
        //get the Audio Source component attached to the car
        carSound = GetComponent<AudioSource>();
        movement = GetComponent<Movement>();
    }

    void FixedUpdate()
    {
        //get the userInput
        userInput = Input.GetAxis("Accelerate");
        //calculate the pitch factor which will be added to the audio source
        float forwardSpeed = Mathf.Abs(movement.acceleration);
        float pitchFactor = Mathf.Abs(forwardSpeed * reductionFactor * userInput);
        //clamp the calculated pitch factor between lowPitch and highPitch
        carSound.pitch = Mathf.Clamp(pitchFactor, lowPtich, highPitch);
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Movement : MonoBehaviour
{
Rigidbody rb;
AudioSource audioSource;

[SerializeField] float thrustValue = 1000f;
[SerializeField] float rotationThrust = 100f;
[SerializeField] AudioClip scream;

[SerializeField] ParticleSystem leftLegParticles;
[SerializeField] ParticleSystem rightLegParticles;
[SerializeField] ParticleSystem mainFlyParticles;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }
    void ProcessThrust()
    {
       if (Input.GetKey(KeyCode.Space))
        {
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }

    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
            RotateLeft();
        else if (Input.GetKey(KeyCode.D))
        {
            RotateRight();
        }
        else
        {
            StopRotating();
        }

    }

    private void StopThrusting()
    {
        audioSource.Stop();
        mainFlyParticles.Stop();
    }

    private void StartThrusting()
    {
        rb.AddRelativeForce(Vector3.up * Time.deltaTime * thrustValue);

        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(scream);
        }
        if (!mainFlyParticles.isPlaying) mainFlyParticles.Play();
    }

    private void StopRotating()
    {
        rightLegParticles.Pause();
        leftLegParticles.Pause();
    }

    private void RotateRight()
    {
        if (!leftLegParticles.isPlaying) leftLegParticles.Play();
        ApplyRotaiton(-rotationThrust);
    }

    private void RotateLeft()
    {
        if (!rightLegParticles.isPlaying) rightLegParticles.Play();
        ApplyRotaiton(rotationThrust);
    }

    private void ApplyRotaiton(float rotationThisFrame)
    {
        rb.freezeRotation = true; // freezing rotation so we can manualy rotate
        transform.Rotate(Vector3.forward * Time.deltaTime * rotationThisFrame);
        rb.freezeRotation = false; // infreezeing rotation so physics system can take over
    }
}

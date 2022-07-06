using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float delayTime = 1f;
    [SerializeField] AudioClip succes;
    [SerializeField] AudioClip deathSound;

    [SerializeField] ParticleSystem succesParticles;
    [SerializeField] ParticleSystem crashParticles;

    AudioSource audioSource;
    
    bool isTrasitioning = false;
    bool isCollisionDisabled = false;

    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    void Update() {
        ReadDebugKeys();
    }

    private void OnCollisionEnter(Collision other) {
        if (isTrasitioning || isCollisionDisabled) { return; }
        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("It's a friend, go on!");
                break;
            case "Sky":
                Debug.Log("Not so high!");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            case "Start":
                Debug.Log("Don't give up!");
                break;
            default:
                StartDieSequence();
                break;
        }

    }

    void ReadDebugKeys()
    {
        if (Input.GetKey(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKey(KeyCode.C))
        {
            isCollisionDisabled = !isCollisionDisabled; // toggle collision
        }
    }
    void StartSuccessSequence()
    {
        succesParticles.Play();
        isTrasitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(succes);
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", delayTime);
    }
    void StartDieSequence()
    {
        crashParticles.Play();
        isTrasitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(deathSound);
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", delayTime);
    }
    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex+1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
}

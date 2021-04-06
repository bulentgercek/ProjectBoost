using UnityEngine;
using UnityEngine.SceneManagement;

public class RocketCollisionHandler : MonoBehaviour
{
    [SerializeField] float _levelLoadDelay = 2.0f;
    [SerializeField] AudioClip _crashAudio;
    [SerializeField] AudioClip _successAudio;
    [SerializeField] ParticleSystem _crashParticles;
    [SerializeField] ParticleSystem _successParticles;

    RocketMovement _RocketMovement;
    AudioSource _audioSource;

    bool _isTransitioning = false;
    bool _isDamageCollisionsDisable = false;

    void Start()
    {
        // Accessing SRocketMovement script from owner object (Rocket)
        _RocketMovement = GetComponent<RocketMovement>();

        // Accessing audio source component
        _audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision other)
    {
        // if transitioning happening stop switching
        if (_isTransitioning) return;

        switch (other.gameObject.tag)
        {
            case "Friendly":
                break;

            case "Finish":
                StartLevelCompleteSequence();
                break;
                
            default:
                if (_isDamageCollisionsDisable)
                {
                    break;
                }
                StartCrashSequence();
                break;
        }
    }

    public void ToggleDamageCollisions()
    {
        _isDamageCollisionsDisable = !_isDamageCollisionsDisable;
        Debug.Log("Debug Mode : Damage Collisions :" + !_isDamageCollisionsDisable);
    }

    void StartCrashSequence()
    {
        // start transition
        _isTransitioning = true;

        // Stop current audio and play crash audio
        _audioSource.Stop();
        _audioSource.volume = 0.20f;
        _audioSource.PlayOneShot(_crashAudio);
        
        // Play crash particles
        _crashParticles.Play();
        // Stop all engine particles
        _RocketMovement.StopParticles();

        Debug.Log("You died!");

        // Disabling input for player rocket while invoke
        _RocketMovement.DisableInput();

        // Wait
        Invoke("ReloadCurrentLevel", _levelLoadDelay);

    }

    public void StartLevelCompleteSequence()
    {
        // start transition
        _isTransitioning = true;

        // Stop current audio and play success audio
        _audioSource.Stop();
        _audioSource.PlayOneShot(_successAudio);
        
        // Play success particles
        _successParticles.Play();

        // Stop all engine particles
        _RocketMovement.StopParticles();
        
        Debug.Log("Level Completed!");

        // Disabling input for player rocket while invoke
        _RocketMovement.DisableInput();

        // Wait
        Invoke("LoadNextLevel", _levelLoadDelay);
    }

    void ReloadCurrentLevel()
    {
        // Load the current scene index in the build settings
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // end transition
        _isTransitioning = false;

        // Enabling input for player rocket
        _RocketMovement.EnableInput();
    }

    public void LoadNextLevel()
    {
        // Getting current level index
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;

        // Calculating the last level index
        int lastLevelIndex = SceneManager.sceneCountInBuildSettings - 1;

        // Calculating next level index
        int nextLevelIndex = currentLevelIndex < lastLevelIndex ? currentLevelIndex + 1 : 0;

        // Load next level
        SceneManager.LoadScene(nextLevelIndex);

        // end transition
        _isTransitioning = false;

        // Enabling input for player rocket
        _RocketMovement.EnableInput();
    }
}

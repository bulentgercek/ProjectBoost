using UnityEngine;

public class RocketMovement : MonoBehaviour
{
	[SerializeField] float _rotateSpeed = 50.0f;
	[SerializeField] float _thrustPower = 500.0f;
	[SerializeField] AudioClip _mainEngineThrust;
	[SerializeField] ParticleSystem _mainEngineThrustParticles;
	[SerializeField] ParticleSystem _sideThrusterRParticles;
	[SerializeField] ParticleSystem _sideThrusterLParticles;

	Rigidbody _rigidbody;
	AudioSource _audioSource;

	// Start is called before the first frame update : Github Update Test
	void Start()
	{
		_rigidbody = GetComponent<Rigidbody>();
		_audioSource = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update()
	{
		// is GameManager input variable true?
		if (GameManager.isInputEnabled)
		{
			ProcessThrust();
			ProcessRotation();
		}
	}

	void ProcessThrust()
	{
		if (Input.GetKey(KeyCode.Space))
        {
            StartThrusting();
        }
        // Stop playing when space key released
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            StopThrusting();
        }
    }

	void ProcessRotation()
	{
		if (Input.GetKey(KeyCode.D))
        {
            RotateRight();
        }
        else if (Input.GetKey(KeyCode.A))
        {
            RotateLeft();
        }
        else
        {
            StopRotating();
        }
    }

    void StartThrusting()
    {
        // Thrusting the rocket
        _rigidbody.AddRelativeForce(Vector3.up * _thrustPower * Time.deltaTime);

        // Play thrust audio if its not already playing
        if (!_audioSource.isPlaying)
        {
            _audioSource.PlayOneShot(_mainEngineThrust);
        }

        // Play thrust particles if its not already playing
        if (!_mainEngineThrustParticles.isPlaying)
        {
            _mainEngineThrustParticles.Play();
        }
    }

    void StopThrusting()
    {
        _audioSource.Stop();
        _mainEngineThrustParticles.Stop();
    }

    private void RotateLeft()
    {
        ApplyRotation(+_rotateSpeed);

        // Play side thruster particles if its not already playing
        if (!_sideThrusterRParticles.isPlaying)
        {
            _sideThrusterRParticles.Play();
        }
    }

    private void RotateRight()
    {
        ApplyRotation(-_rotateSpeed);

        // Play side thruster particles if its not already playing
        if (!_sideThrusterLParticles.isPlaying)
        {
            _sideThrusterLParticles.Play();
        }
    }

    private void StopRotating()
    {
        _sideThrusterLParticles.Stop();
        _sideThrusterRParticles.Stop();
    }

    void ApplyRotation(float rotationThisFrame)
    {
		// Freeze rotation to manually rotate
		_rigidbody.freezeRotation = true;

		// Rotating the rocket with manual transform
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
		
		// Unfreeze rotation to physic system takeover
		_rigidbody.freezeRotation = false;
    }

	public void DisableInput()
	{
		GameManager.isInputEnabled = false;
	}

	public void EnableInput()
	{
		GameManager.isInputEnabled = true;
	}

	public void StopParticles()
	{
		_mainEngineThrustParticles.Stop();
		_sideThrusterLParticles.Stop();
		_sideThrusterRParticles.Stop();
	}
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
	[SerializeField] float levelLoadDelay;
	[SerializeField] AudioClip crash;
	[SerializeField] AudioClip success;

	[SerializeField] ParticleSystem crashParticles;
	[SerializeField] ParticleSystem successParticles;

    
    
	AudioSource audioSource;
	Collider[] coll;

	bool isTransitioning = false;
	bool collisionDisabled = false;

	void Start() 
    {
		audioSource = GetComponent<AudioSource>();
		coll = GetComponentsInChildren<Collider>();
	}
    
    void Update()
    {
		RespondToDebugsKeys();
	}
    
	private void OnCollisionEnter(Collision other) 
    {
        if(isTransitioning || collisionDisabled)
        {
			return;
		}
        
        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("This thing is friendly");
				break;
             case "Finish":
				StartSuccessSequence();
				break;
            default:
				StartCrashSequence();
				break;
		}
    }

	void RespondToDebugsKeys()
	{
		if (Input.GetKey(KeyCode.L))
		{
			LoadNextLevel();
		}
		else if (Input.GetKey(KeyCode.C))
		{
			collisionDisabled = !collisionDisabled; // toggle collision
            
			/*for (int i = 0; i < coll.Length; i++)
            {
				
				coll[i] = !coll[i];
			}*/
		}
	}

	void StartSuccessSequence()
	{
		isTransitioning = true;
		audioSource.Stop();
		audioSource.PlayOneShot(success);
		successParticles.Play();
		GetComponent<Movement>().enabled = false;
		Invoke("LoadNextLevel", levelLoadDelay);
	}
    
    void StartCrashSequence()
    {
		isTransitioning = true;
		audioSource.Stop();
		audioSource.PlayOneShot(crash);
		crashParticles.Play();
		//to do add particle effect upon crash
		GetComponent<Movement>().enabled = false;
		Invoke("ReloadLevel", levelLoadDelay);
	}
    
    void LoadNextLevel()
    {
		int currentSceneindex = SceneManager.GetActiveScene().buildIndex;
		int nextSceneIndex = currentSceneindex + 1;
        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
			nextSceneIndex = 0;
		}
		SceneManager.LoadScene(nextSceneIndex);
	}
    
    void ReloadLevel()
    {
		int currentSceneindex = SceneManager.GetActiveScene().buildIndex;
		SceneManager.LoadScene(currentSceneindex);
	}
}
using UnityEngine;

[System.Serializable]
public class Sound {

	public string name;
    public AudioClip audioClip;
	public bool loop = false;

    [Range(0f, 1f)]
	public float volume = 0.7f;

	[Range(0f, 1f)]
	public float pitch = 1f;

	[Range(0f, 1f)]
	public float randomVolume = 0.7f;

	[Range(0f, 1f)]
	public float randomPitch = 1f;

	private AudioSource m_AudioSource;

	public void SetSource (AudioSource audioSource)
	{
        m_AudioSource = audioSource;
	}

	public void Play ()
	{
        m_AudioSource.clip = audioClip;
		m_AudioSource.loop = loop;
        m_AudioSource.volume = volume * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f));
        m_AudioSource.pitch = pitch * (1 + Random.Range(-randomPitch / 2f, randomPitch / 2f));
		m_AudioSource.Play();
	}

	public void Stop ()
	{
		m_AudioSource.Stop();
	}
}
public class AudioManager : MonoBehaviour {
	public static AudioManager audioManager;

	[SerializeField]
	private Sound[] m_Sounds;
    private Transform m_Transform;

	void Awake ()
	{
        m_Transform = transform;

        if (audioManager != null)
		{
			Debug.LogError("AUDIOMANAGER: more than one AudioManager in the scene.");
		}
		else 
		{
            audioManager = this;
		}
	}

	void Start ()
	{
		foreach (var sound in m_Sounds)
		{
			GameObject soundGameObject = new GameObject("Sound_" + sound.name);
            soundGameObject.transform.SetParent(m_Transform);
			sound.SetSource(soundGameObject.AddComponent<AudioSource>());
		}
	}

	public static void PlaySound (string name)
	{
		Debug.Log("AUDIOMANAGER: searching sound.");
        foreach (var sound in audioManager.m_Sounds)
        {
			if (sound.name == name)
			{
				Debug.Log("AUDIOMANAGER: sound found.");
				sound.Play();
				return;
			}
        }
		Debug.LogWarning("AUDIOMANAGER: sound not found!");
	}
	public static void StopSound (string name)
	{
        foreach (var sound in audioManager.m_Sounds)
        {
			if (sound.name == name)
			{
				sound.Stop();
				return;
			}
        }
		Debug.LogWarning("AUDIOMANAGER: sound not found!");
	}

}

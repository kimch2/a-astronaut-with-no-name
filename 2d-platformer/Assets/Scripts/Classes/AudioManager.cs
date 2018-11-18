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

	public bool Play ()
	{
		if (m_AudioSource != null) 
		{
			m_AudioSource.clip = audioClip;
			m_AudioSource.loop = loop;
			m_AudioSource.volume = volume * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f));
			m_AudioSource.pitch = pitch * (1 + Random.Range(-randomPitch / 2f, randomPitch / 2f));
			m_AudioSource.Play();
			return true;
		}
		return false;
	}

	public bool Stop ()
	{
        if (m_AudioSource != null) 
		{
			m_AudioSource.Stop();
			return true;
		}
		return false;
	}
}
public class AudioManager : MonoBehaviour {
	public static AudioManager instance;

	[SerializeField]
	private Sound[] m_Sounds;
    private Transform m_Transform;

	void Awake ()
	{
        m_Transform = transform;

        if (instance != null && instance != this)
		{
			Destroy(this.gameObject);
		}
		else 
		{
            instance = this;
			DontDestroyOnLoad(this.gameObject);
		}
		
		//TODO: check this
        foreach (var sound in m_Sounds)
        {
            GameObject soundGameObject = new GameObject("Sound_" + sound.name);
            soundGameObject.transform.SetParent(m_Transform);
            sound.SetSource(soundGameObject.AddComponent<AudioSource>());
        }
	}

	public bool PlaySound (string name)
	{
        foreach (var sound in instance.m_Sounds)
        {
			if (sound.name == name)
			{
				return sound.Play();
			}
        }
		return false;
	}
	public bool StopSound (string name)
	{
        foreach (var sound in instance.m_Sounds)
        {
			if (sound.name == name)
			{
				return sound.Stop();
			}
        }
		return false;
	}

}

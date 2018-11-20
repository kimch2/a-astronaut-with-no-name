using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

	public string pressButtonSound = "PressButtonSound";
	public string hoverButtonSound = "HoverButtonSound";

    private AudioManager m_AudioManager;
	public void Start ()
	{
		m_AudioManager = AudioManager.instance;
		if (m_AudioManager == null)
		{
			Debug.LogError("No AudioManager in the scene");
		}

        m_AudioManager.PlaySound("MenuSoundtrack");
	}

	public void Play () 
	{
		displayButtonPressSound();
		m_AudioManager.StopSound("MenuSoundtrack");
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
	

	public void OnMouseEnter()
	{
        m_AudioManager.PlaySound(hoverButtonSound);
	}

	public void displayButtonPressSound()
	{
        m_AudioManager.PlaySound(pressButtonSound);
	}
}

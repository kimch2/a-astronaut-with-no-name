using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour {

    public string pressButtonSound = "PressButtonSound";
    public string hoverButtonSound = "HoverButtonSound";

    private AudioManager m_AudioManager;

    void Awake()
    {
        m_AudioManager = AudioManager.instance;
        if (m_AudioManager == null)
        {
            Debug.LogError("No AudioManager in the scene");
        }
    }

	public void Exit () 
	{
		Debug.Log("APPLICATION QUIT!");
        displayButtonPressSound();
		Application.Quit();	
	}
	
	public void Retry () 
	{
        displayButtonPressSound();
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

	public Transform playerPrefab;
	public Transform spawnPoint;
	public static GameMaster instance;
	private static CameraShake m_CameraShake;

	[SerializeField]
	private GameObject m_GameOverUI;
    private AudioManager m_AudioManager;
	
    void Awake () 
	{
		if (!instance) 
		{
			instance = GameObject.FindGameObjectWithTag ("GM").GetComponent<GameMaster>();
		}
		if (!m_CameraShake)
		{
			m_CameraShake = gameObject.GetComponent<CameraShake>();
		}
	}

	void Start ()
	{
        m_AudioManager = AudioManager.instance;

        if (m_AudioManager == null)
        {
            Debug.LogError("No AudioManager in the scene");
        }
        m_AudioManager.PlaySound("MainLevelSoundtrack");
	}

	public static void GameOver ()
	{
		instance.m_GameOverUI.SetActive(true);
        instance.m_AudioManager.StopSound("MainLevelSoundtrack");
	}

	public static void ShakeCamera (float amount, float length)
	{
		m_CameraShake.Shake (amount, length);
	}
}

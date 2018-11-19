using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

	public float money;
	public string mainLevelSoundtrackName = "MainLevelSoundtrack";
	public Transform playerPrefab;
	public Transform spawnPoint;
	public static GameMaster instance;
	public delegate void PauseCallback(bool active);
	public PauseCallback onPause;

    private AudioManager m_AudioManager;
	private CameraShake m_CameraShake;
	[SerializeField] private GameObject m_UpgradeMenu;
	[SerializeField] private GameObject m_GameOverUI;
	[SerializeField] private GameObject m_OnGameUI;
	[SerializeField] private float m_StartingMoney = 100f;
	

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
        m_AudioManager.PlaySound(mainLevelSoundtrackName);
		
		money = m_StartingMoney;
	}

	void Update ()
	{
		//TODO: don't hardcode this
		if (Input.GetKeyDown(KeyCode.U))
		{
            ToggleUpgradeMenu();
		}
	}

	private void ToggleUpgradeMenu ()
	{
        m_UpgradeMenu.SetActive(!m_UpgradeMenu.activeSelf);
        m_OnGameUI.SetActive(!m_UpgradeMenu.activeSelf);
        onPause.Invoke(m_UpgradeMenu.activeSelf);
	}

	public void GameOver ()
	{
		instance.m_GameOverUI.SetActive(true);
        instance.m_AudioManager.StopSound(instance.mainLevelSoundtrackName);
	}

	public void ShakeCamera (float amount, float length)
	{
        m_CameraShake.Shake (amount, length);
	}
}

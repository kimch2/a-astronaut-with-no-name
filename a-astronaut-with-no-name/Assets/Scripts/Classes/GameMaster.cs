using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

	public string mainLevelSoundtrackName = "MainLevelSoundtrack";
	public Transform playerPrefab;
	public Transform spawnPoint;
	public static GameMaster instance;
	public delegate void PauseCallback(bool active);
	public PauseCallback onPause;
	public int xp 
	{
		get { return m_Xp; }
	}

   	 private AudioManager m_AudioManager;
	private CameraShake m_CameraShake;
    	[SerializeField] private int m_Xp;
	[SerializeField] private GameObject m_UpgradeMenu;
	[SerializeField] private GameObject m_GameOverUI;
	[SerializeField] private GameObject m_OnGameUI;
	[SerializeField] private int m_StartingXp = 0;
	

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

        m_Xp = m_StartingXp;
	}

	void Update ()
	{
		//TODO: don't hardcode this
		if (Input.GetKeyDown(KeyCode.U) && !instance.m_GameOverUI.activeSelf)
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

	public void AddXP(int amount)
	{
        m_Xp += amount;
	}

	public void SubtractXP(int amount)
	{
        m_Xp -= amount;
	}

	public void GameOver ()
	{
		instance.m_GameOverUI.SetActive(true);
        m_OnGameUI.SetActive(false);
        instance.m_AudioManager.StopSound(instance.mainLevelSoundtrackName);
	}

	public void ShakeCamera (float amount, float length)
	{
        m_CameraShake.Shake (amount, length);
	}
}

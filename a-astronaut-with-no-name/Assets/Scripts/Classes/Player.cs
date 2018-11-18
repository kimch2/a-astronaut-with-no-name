using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable<int> {

	[System.Serializable]
	public class Stats {
        public int maxHealth = 100;
        public int currentHealth
        {
            get { return m_CurrentHealth; }
            set { m_CurrentHealth = Mathf.Clamp(value, 0, maxHealth); }
        }

        private int m_CurrentHealth;

        public void Init()
        {
            currentHealth = maxHealth;
        }
    }

	public int fallBoundary = -20;
    public string deathSoundVoice = "DeathVoice";
	public Stats stats = new Stats();

    [Header("Optional: ")]
    [SerializeField]
    private StatusIndicator statusIndicator;
    private AudioManager m_AudioManager;

    void Start()
    {
        stats.Init();
        setHealthStatus();
        
        m_AudioManager = AudioManager.instance;
        if (m_AudioManager == null)
        {
            Debug.LogError("No AudioManager in the scene");
        }
    }

    void Update () 
    {
		if (transform.position.y <= fallBoundary) {
            Kill();
		}
	}

	public void Damage (int damage) 
    {
        stats.currentHealth -= damage;

		setHealthStatus();
		if (stats.currentHealth <= 0) {
			Kill();
		}
	}

	public void Kill() 
    {
        m_AudioManager.PlaySound(deathSoundVoice);
        Destroy(gameObject);
        GameMaster.GameOver();
	}

    void setHealthStatus()
    {
        if (statusIndicator != null)
        {
            statusIndicator.SetHealth(stats.currentHealth, stats.maxHealth);
        }
    }
	
}

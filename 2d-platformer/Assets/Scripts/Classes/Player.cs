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
	public Stats stats = new Stats();

    [Header("Optional: ")]
    [SerializeField]
    private StatusIndicator statusIndicator;

    void Start()
    {
        stats.Init();
        setHealthStatus();
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

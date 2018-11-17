using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable<int>
{

    [System.Serializable]
    public class EnemyStats
    {
        public int maxHealth = 100;
        public int currentHealth
        {
            get { return m_CurrentHealth; }
            set { m_CurrentHealth = Mathf.Clamp(value, 0, maxHealth); }
        }

        private int m_CurrentHealth;

        public void Init ()
        {
            currentHealth = maxHealth;
        } 
    }

    public EnemyStats enemyStats = new EnemyStats();

    [Header("Optional: ")]
    [SerializeField]
    private StatusIndicator statusIndicator;

    void Start ()
    {
        enemyStats.Init();
        setHealthStatus();
    }

    public void Damage (int damage)
    {
        enemyStats.currentHealth -= damage;
        
        setHealthStatus();

        if (enemyStats.currentHealth <= 0)
        {
            Kill();
        }
    }

    public void Kill ()
    {
        Destroy(gameObject);
    }

    void setHealthStatus() 
    {
        if (statusIndicator != null)
        {
            statusIndicator.SetHealth(enemyStats.currentHealth, enemyStats.maxHealth);
        }
    }

}

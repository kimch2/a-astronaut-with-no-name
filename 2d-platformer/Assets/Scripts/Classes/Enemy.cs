using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable<int> {

    [System.Serializable]
    public class Stats
    {
        public int maxHealth = 100;
        public int damage = 40;
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

    public float cameraDeathShakeAmount = 0.1f;
    public float cameraDeathShakeLength = 0.1f;
    public Transform deathParticles;
    public Stats stats = new Stats();

    [Header("Optional: ")]
    [SerializeField]
    private StatusIndicator statusIndicator;
    private Transform m_Transform;

    void Awake()
    {
        m_Transform = transform;
    }

    void Start ()
    {
        stats.Init();
        setHealthStatus();

        if (deathParticles == null)
        {
            Debug.LogError("ENEMY: No death particles!");
        }
    }

    public void Damage (int damage)
    {
        stats.currentHealth -= damage;
        
        setHealthStatus();

        if (stats.currentHealth <= 0)
        {
            Kill();
        }
    }
    
    public void Kill ()
    {
        Transform currentDeathParticle = (Transform) Instantiate (deathParticles, m_Transform.position, Quaternion.identity);
        Destroy(currentDeathParticle.gameObject, 1f);
        GameMaster.ShakeCamera(cameraDeathShakeAmount, cameraDeathShakeLength);
        Destroy(gameObject);
    }

    void setHealthStatus() 
    {
        if (statusIndicator != null)
        {
            statusIndicator.SetHealth(stats.currentHealth, stats.maxHealth);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.collider.GetComponent<Player>();
        if (player != null) 
        {
            player.Damage(stats.damage);
        } 
    }

}

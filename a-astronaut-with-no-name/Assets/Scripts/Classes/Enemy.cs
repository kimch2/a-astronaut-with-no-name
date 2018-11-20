using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
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

    public int xpGain = 10;
    public float cameraDeathShakeAmount = 0.1f;
    public float cameraDeathShakeLength = 0.1f;
    public string deathSound = "Explosion";
    public Transform deathParticles;
    public Stats stats = new Stats();

    [Header("Optional: ")]
    [SerializeField]
    private StatusIndicator statusIndicator;
    private Transform m_Transform;
    private AudioManager m_AudioManager;
    private GameMaster m_GameMaster;
    private Rigidbody2D m_RigidBody;
    

    void Awake()
    {
        m_Transform = transform;
        m_RigidBody = GetComponent<Rigidbody2D>();
    }

    void Start ()
    {
        stats.Init();
        setHealthStatus();

        if (deathParticles == null)
        {
            Debug.LogError("No death particles!");
        }

        m_AudioManager = AudioManager.instance;
        if (m_AudioManager == null)
        {
            Debug.LogError("No AudioManager in the scene");
        }

        m_GameMaster = GameMaster.instance;
        if (m_GameMaster == null)
        {
            Debug.LogError("No GameMaster in the scene");
        }
        m_GameMaster.onPause += OnPause;
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

    void OnPause(bool active)
    {
        if (m_RigidBody != null)
        {
            if (active)
            {
                m_RigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
            }
            else
            {
                m_RigidBody.constraints = RigidbodyConstraints2D.None;
                m_RigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }
    }

    public void Kill ()
    {
        m_AudioManager.PlaySound(deathSound);

        Transform currentDeathParticle = (Transform) Instantiate (deathParticles, m_Transform.position, Quaternion.identity);
        Destroy(currentDeathParticle.gameObject, 1f);

        m_GameMaster.AddXP(xpGain);
        m_GameMaster.ShakeCamera(cameraDeathShakeAmount, cameraDeathShakeLength);

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
            Kill();
        } 
    }

}

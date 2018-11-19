using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
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
    [SerializeField] private StatusIndicator statusIndicator;
    private AudioManager m_AudioManager;
    private GameMaster m_GameMaster;
    private Rigidbody2D m_RigidBody;

    void Awake ()
    {
        m_RigidBody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        stats.Init();
        setHealthStatus();
        
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

    void Update () 
    {
		if (transform.position.y <= fallBoundary) {
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
        m_GameMaster.GameOver();
	}

    void setHealthStatus()
    {
        if (statusIndicator != null)
        {
            statusIndicator.SetHealth(stats.currentHealth, stats.maxHealth);
        }
    }
	
}

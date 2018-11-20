using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour, IDamageable<int> {

	public int fallBoundary = -20;
    public string deathSoundVoice = "DeathVoice";

    [Header("Optional: ")] 
    [SerializeField] private StatusIndicator statusIndicator;
    private AudioManager m_AudioManager;
    private GameMaster m_GameMaster;
    private PlayerStats m_PlayerStats;
    private Rigidbody2D m_RigidBody;

    void Awake ()
    {
        m_RigidBody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        m_GameMaster = GameMaster.instance;
        if (m_GameMaster == null) Debug.LogError("No GameMaster in the scene");
        m_GameMaster.onPause += OnPause;

        m_AudioManager = AudioManager.instance;
        if (m_AudioManager == null) Debug.LogError("No AudioManager in the scene");

        m_PlayerStats = PlayerStats.instance;

        if (m_PlayerStats == null) Debug.LogError("No PlayerStats in the scene");

        setHealthStatus();

        InvokeRepeating("Heal", 1f/m_PlayerStats.healthRegenerateRate, 1f/m_PlayerStats.healthRegenerateRate);
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
        m_PlayerStats.currentHealth -= damage;

		setHealthStatus();
		if (m_PlayerStats.currentHealth <= 0) {
			Kill();
		}
	}

    void Heal()
    {
        m_PlayerStats.currentHealth += m_PlayerStats.healthRegenerateAmount;
        setHealthStatus();
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
            statusIndicator.SetHealth(m_PlayerStats.currentHealth, m_PlayerStats.maxHealth);
        }
    }
	
}

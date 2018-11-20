using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

	public float fireRate = 0f;
	public int damage = 100;
	public float range = 100f;
	public float effectSpawnRate = 10;
	public float cameraShakeAmount = .1f;
	public float cameraShakeLength = .1f;
	public float muzzleFlashLifeTime = 0.02f;
	public string shootSound = "DefaultShoot";
	public Transform bulletTrail;
	public Transform hitPrefab;
	public Transform muzzleFlash;
	public LayerMask whatToHit;

	private float m_TimeToFire = 0;
	private float m_TimeToSpawnEffect = 0;
	private Transform m_FirePoint;
    private AudioManager m_AudioManager;
    private GameMaster m_GameMaster;
    private PlayerStats m_PlayerStats;
 
    void Awake () 
	{
		m_FirePoint = transform.Find ("FirePoint");
		if (!m_FirePoint) 
		{
			Debug.LogError ("Gun does not have a firepoint");
		}
	}

	void Start()
	{
        m_PlayerStats = PlayerStats.instance;
        if (m_PlayerStats == null)
        {
            Debug.LogError("No PlayerStats in the scene");
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
		if (fireRate == 0) 
		{
			if (Input.GetButtonDown ("Fire1")) Shoot();
		} 
		else 
		{
            if (Input.GetButton ("Fire1") && Time.time > m_TimeToFire) {
				m_TimeToFire = Time.time + 1 / (fireRate * m_PlayerStats.weaponFireRateMultiplier);
				Shoot();
			}
		}

        m_AudioManager = AudioManager.instance;

        if (m_AudioManager == null)
        {
            Debug.LogError("No AudioManager in the scene");
        }

    }

    void OnPause(bool active)
    {
        this.enabled = !active;
    }

	void Shoot () 
	{
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 target = new Vector2 (mousePosition.x, mousePosition.y);
        Vector2 firePointPosition = new Vector2 (m_FirePoint.position.x, m_FirePoint.position.y);
        RaycastHit2D hit = Physics2D.Raycast (firePointPosition, target - firePointPosition, range, whatToHit);

		if (hit.collider != null) 
		{
            IDamageable<int> hitTarget = hit.collider.GetComponent<IDamageable<int>>();
			if (hitTarget != null) 
			{
                hitTarget.Damage ((int) (damage * m_PlayerStats.weaponDamageMultiplier));
			}
		}

        if (Time.time >= m_TimeToSpawnEffect)
        {
            Vector3 hitPosition;
			Vector3 hitNormal;

			if (hit.collider == null) 
			{
				hitPosition = (mousePosition - firePointPosition) * 30;
                hitNormal = Vector3.zero;
			} 
			else 
			{
				hitPosition = hit.point;
				hitNormal =  hit.normal;
			}

            Effect(hitPosition, hitNormal);
            m_TimeToSpawnEffect = Time.time + 1 / effectSpawnRate;
        }
	}

	void Effect(Vector3 hitPosition, Vector3 hitNormal) 
	{
		Transform trail = (Transform) Instantiate (bulletTrail, m_FirePoint.position, m_FirePoint.rotation);

		LineRenderer line = trail.GetComponent<LineRenderer>();

        if (line != null)
		{
			line.SetPosition(0, m_FirePoint.position);			
			line.SetPosition(1, hitPosition);			
		}

		Destroy(trail.gameObject, 0.02f);

		if (hitNormal != Vector3.zero) 
		{
			Transform hitParticle = (Transform) Instantiate (hitPrefab, hitPosition, Quaternion.FromToRotation (Vector3.right, hitNormal));
			Destroy (hitParticle.gameObject, 1f);
		}

    	Transform currentMuzzleFlash = (Transform) Instantiate(muzzleFlash, m_FirePoint.position, m_FirePoint.rotation);
		currentMuzzleFlash.parent = m_FirePoint;
		float size = Random.Range (0.6f, 0.9f);
		currentMuzzleFlash.localScale = new Vector3(size, size, size);

		Destroy(currentMuzzleFlash.gameObject, muzzleFlashLifeTime);

        m_AudioManager.PlaySound(shootSound);
        m_GameMaster.ShakeCamera (cameraShakeAmount, cameraShakeLength);
	}
}

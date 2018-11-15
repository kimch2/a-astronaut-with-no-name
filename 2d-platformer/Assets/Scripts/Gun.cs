using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

	public float fireRate = 0f;
	public float damage = 100f;
	public float range = 100f;
	public float effectSpawnRate = 10;
	public float muzzleFlashLifeTime = 0.02f;
	public Transform bulletTrail;
	public Transform muzzleFlash;
	public LayerMask whatToHit;


	private float m_TimeToFire = 0;
	private float m_TimeToSpawnEffect = 0;
	private Transform m_FirePoint;
    private Vector3 m_MousePosition;
    private Vector2 m_Target;
    private Vector2 m_FirePointPosition;
    private RaycastHit2D m_Hit;
    private Transform m_CurrentMuzzleFlash;


    void Awake () {
		m_FirePoint = transform.Find ("FirePoint");
		if (!m_FirePoint) {
			Debug.LogError ("Gun does not have a firepoint");
		}
	}

	void Update () {
		if (fireRate == 0) {
			if (Input.GetButtonDown ("Fire1")) Shoot();
		} else {
            if (Input.GetButton ("Fire1") && Time.time > m_TimeToFire) {
				m_TimeToFire = Time.time + 1 / fireRate;
				Shoot();
			}
		}
	}

	void Shoot () {
        m_MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		
		m_Target = new Vector2 (m_MousePosition.x, m_MousePosition.y);

		m_FirePointPosition = new Vector2 (m_FirePoint.position.x, m_FirePoint.position.y);

		m_Hit = Physics2D.Raycast (m_FirePointPosition, m_Target - m_FirePointPosition, range, whatToHit);

		if (Time.time >= m_TimeToSpawnEffect) {
			Effect();
            m_TimeToSpawnEffect = Time.time + 1 / effectSpawnRate;
		}

		Debug.DrawLine (m_FirePointPosition, (m_Target - m_FirePointPosition) * range, Color.cyan);
		if (m_Hit.collider) {
			Debug.DrawLine (m_FirePointPosition, m_Hit.point, Color.red);
			Debug.Log("We m_Hit" + m_Hit.collider.name + "and did" + damage + " damage");
		}
	}

	void Effect() {
		Instantiate (bulletTrail, m_FirePoint.position, m_FirePoint.rotation);
        m_CurrentMuzzleFlash = (Transform) Instantiate(muzzleFlash, m_FirePoint.position, m_FirePoint.rotation);

		m_CurrentMuzzleFlash.parent = m_FirePoint;

		float size = Random.Range (0.6f, 0.9f);

		m_CurrentMuzzleFlash.localScale = new Vector3(size, size, size);

		Destroy(m_CurrentMuzzleFlash.gameObject, muzzleFlashLifeTime);
	}
}

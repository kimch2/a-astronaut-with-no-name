using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

	public float fireRate = 0f;
	public float damage = 100f;
	public float range = 100f;
	public LayerMask whatToHit;
	private float m_TimeToFire = 0;
	private Transform m_FirePoint;

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
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 target = new Vector2 (mousePosition.x, mousePosition.y);
		Vector2 firePointPosition = new Vector2 (m_FirePoint.position.x, m_FirePoint.position.y);
		RaycastHit2D hit = Physics2D.Raycast (firePointPosition, target - firePointPosition, range, whatToHit);
		Debug.DrawLine (firePointPosition, (target - firePointPosition) * range, Color.cyan);
		if (hit.collider) {
			Debug.DrawLine (firePointPosition, hit.point, Color.red);
			Debug.Log("We hit" + hit.collider.name + "and did" + damage + " damage");
		}
	}
}

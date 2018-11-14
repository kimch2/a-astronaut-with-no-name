using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmRotation : MonoBehaviour {

	public int offset = 90;
	private Vector3 m_MouseToArmDifference;
	private float m_RotationZ;
    private Camera m_MainCamera;

	void Awake() {
        m_MainCamera = Camera.main;
	}
    void Update () {
		m_MouseToArmDifference = m_MainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        m_MouseToArmDifference.Normalize();
        m_RotationZ = Mathf.Atan2(m_MouseToArmDifference.y, m_MouseToArmDifference.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0f, 0f, m_RotationZ + offset);
	}
}

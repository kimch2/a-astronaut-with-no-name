using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmRotation : MonoBehaviour {

	public int offset = 90;
    private Camera m_MainCamera;

	void Awake() {
        m_MainCamera = Camera.main;
	}
    void Update () {
        Vector3 mouseToArmDifference = m_MainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        mouseToArmDifference.Normalize();

        float rotationZ = Mathf.Atan2(mouseToArmDifference.y, mouseToArmDifference.x) * Mathf.Rad2Deg;
		
		transform.rotation = Quaternion.Euler(0f, 0f, rotationZ + offset);
	}
}

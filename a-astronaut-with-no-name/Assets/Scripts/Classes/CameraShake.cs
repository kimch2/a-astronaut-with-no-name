using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

	public Transform mainCamera;
	private float m_ShakeAmount = 0;

	void Awake () {
		if (mainCamera == null)
		{
			mainCamera = Camera.main.transform;
		}
	}

	public void Shake (float amount, float length)
	{
        m_ShakeAmount = amount;
		InvokeRepeating ("BeginShake", 0, 0.01f);
		Invoke ("StopShake", length);
	}

	void BeginShake () 
	{
		if (m_ShakeAmount > 0) 
		{
			Vector3 cameraPosition = mainCamera.position;

			float shakeAmountX = Random.value * m_ShakeAmount * 2 - m_ShakeAmount;
			float shakeAmountY = Random.value * m_ShakeAmount * 2 - m_ShakeAmount;

            cameraPosition.x += shakeAmountX;
            cameraPosition.y += shakeAmountY;

			mainCamera.position = cameraPosition;
		}
	}

	void StopShake () 
	{
		CancelInvoke ("BeginShake");
		mainCamera.localPosition = Vector3.zero;
	}
	
}

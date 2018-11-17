using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour {

	public Transform[] backgrounds;
	public float smoothing = 1f;
	private Transform m_MainCamera;
	private Vector3 m_PreviousCameraPosition;
	private float[] m_ParallaxScales;

	void Awake () {
		m_MainCamera = Camera.main.transform;
	}

	void Start () {
        SetPreviousCameraPosition();
        m_ParallaxScales = new float[backgrounds.Length];
        int cont = 0;
        foreach (var background in backgrounds)
        {
            m_ParallaxScales[cont] = background.position.z * -1;
            cont++;
        }
	}
	
	void Update () {
        int cont = 0;
        foreach (var background in backgrounds)
        {
            float parallax = (m_PreviousCameraPosition.x - m_MainCamera.position.x) * m_ParallaxScales[cont];

            float backgroundTargetPositionX = background.position.x + parallax;

            Vector3 backgroundTargetPosition = new Vector3(backgroundTargetPositionX, background.position.y, background.position.z);

            background.position = Vector3.Lerp(background.position, backgroundTargetPosition, smoothing * Time.deltaTime);

            cont++;
        }
        SetPreviousCameraPosition();
	}

    void SetPreviousCameraPosition() {
        m_PreviousCameraPosition = m_MainCamera.position;
    }

}

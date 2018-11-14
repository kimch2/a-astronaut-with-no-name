using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour {

	public Transform[] backgrounds;
	public float smoothing = 1f;
	private Transform m_MainCamera;
	private Vector3 m_PreviousCameraPosition;
	private float[] m_ParallaxScales;
    private float m_Parallax;
    private float m_BackgroundTargetPositionX;
    private Vector3 m_BackgroundTargetPosition;

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
            m_Parallax = (m_PreviousCameraPosition.x - m_MainCamera.position.x) * m_ParallaxScales[cont];

            m_BackgroundTargetPositionX = background.position.x + m_Parallax;

            m_BackgroundTargetPosition = new Vector3(m_BackgroundTargetPositionX, background.position.y, background.position.z);

            background.position = Vector3.Lerp(background.position, m_BackgroundTargetPosition, smoothing * Time.deltaTime);

            cont++;
        }
        SetPreviousCameraPosition();
	}

    void SetPreviousCameraPosition() {
        m_PreviousCameraPosition = m_MainCamera.position;
    }

}

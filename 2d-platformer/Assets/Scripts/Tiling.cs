using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (SpriteRenderer))]
public class Tiling : MonoBehaviour {
	
	public int offsetX = 2;
	public bool hasRightTiling = false;
	public bool hasLeftTiling = false;
	public bool reverseScale = false;
	private float m_SpriteWidth = 0f;
	private Camera m_MainCamera;
	private Transform m_Transform;
    private float m_TilingPositionX;
    private Vector3 m_TilingPosition;
    private Transform m_NewTiling;

	void Awake() {
		m_MainCamera = Camera.main;
		m_Transform = transform;
	}

	void Start () {
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
		m_SpriteWidth = spriteRenderer.sprite.bounds.size.x;
	}
	
	void Update () {
		if (hasLeftTiling == false || hasRightTiling == false) 
		{
			float cameraHorizontalExtend = m_MainCamera.orthographicSize * Screen.width / Screen.height;
			float edgeVisiblePositionRight = (m_Transform.position.x + m_SpriteWidth / 2) - cameraHorizontalExtend;
			float edgeVisiblePositionLeft = (m_Transform.position.x - m_SpriteWidth / 2) + cameraHorizontalExtend;

			if (m_MainCamera.transform.position.x >= (edgeVisiblePositionRight - offsetX) && hasRightTiling == false) 
			{
				MakeNewTiling(1);
				hasRightTiling = true;
			}
			else if (m_MainCamera.transform.position.x <= (edgeVisiblePositionLeft + offsetX) && hasLeftTiling == false)
			{
				MakeNewTiling(-1);
				hasLeftTiling = true;
			}

		}
	}

	void MakeNewTiling (int direction) {
		m_TilingPositionX = m_Transform.position.x + m_SpriteWidth * direction;
		m_TilingPosition =  new Vector3(m_TilingPositionX, m_Transform.position.y, m_Transform.position.z);
		m_NewTiling = (Transform) Instantiate (m_Transform, m_TilingPosition, m_Transform.rotation);

		if (reverseScale == true) 
		{
			m_NewTiling.localScale = new Vector3 (m_NewTiling.localScale.x * -1, m_NewTiling.localScale.y, m_NewTiling.localScale.z);		
		}

        m_NewTiling.parent = m_Transform.parent;

		if (direction > 0) 
		{
			m_NewTiling.GetComponent<Tiling>().hasLeftTiling = true;
		}
		else 
		{
			m_NewTiling.GetComponent<Tiling>().hasRightTiling = true;
		}
	}
}

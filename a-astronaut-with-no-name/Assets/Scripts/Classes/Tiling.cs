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
		float tilingPositionX = m_Transform.position.x + m_SpriteWidth * direction;
        Vector3 tilingPosition = new Vector3(tilingPositionX, m_Transform.position.y, m_Transform.position.z);

    	Transform newTiling = (Transform) Instantiate (m_Transform, tilingPosition, m_Transform.rotation);

		if (reverseScale == true) 
		{
			newTiling.localScale = new Vector3 (newTiling.localScale.x * -1, newTiling.localScale.y, newTiling.localScale.z);		
		}

        newTiling.parent = m_Transform.parent;

		if (direction > 0) 
		{
			newTiling.GetComponent<Tiling>().hasLeftTiling = true;
		}
		else 
		{
			newTiling.GetComponent<Tiling>().hasRightTiling = true;
		}
	}
}

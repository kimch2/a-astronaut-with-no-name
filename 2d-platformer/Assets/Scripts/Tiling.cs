using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (SpriteRenderer))]
public class Tiling : MonoBehaviour {

	
	public int OffsetX = 2;
	public bool HasRightTiling = false;
	public bool HasLeftTiling = false;
	public bool ReverseScale = false;

	private float spriteWidth = 0f;
	private Camera mainCamera;
	private Transform myTransform;

	void Awake() {
		mainCamera = Camera.main;
		myTransform = transform;
	}

	void Start () {
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
		spriteWidth = spriteRenderer.sprite.bounds.size.x;
	}
	
	void Update () {
		if (HasLeftTiling == false || HasRightTiling == false) 
		{
			float cameraHorizontalExtend = mainCamera.orthographicSize * Screen.width / Screen.height;

			float edgeVisiblePositionRight = (myTransform.position.x + spriteWidth / 2) - cameraHorizontalExtend;
			float edgeVisiblePositionLeft = (myTransform.position.x - spriteWidth / 2) + cameraHorizontalExtend;

			if (mainCamera.transform.position.x >= (edgeVisiblePositionRight - OffsetX) && HasRightTiling == false) 
			{
				MakeNewTiling(1);
				HasRightTiling = true;
			}
			else if (mainCamera.transform.position.x <= (edgeVisiblePositionLeft + OffsetX) && HasLeftTiling == false)
			{
				MakeNewTiling(-1);
				HasLeftTiling = true;
			}

		}
	}

	void MakeNewTiling (int direction) {
		float tilingPositionX = myTransform.position.x + spriteWidth * direction;

		Vector3 tilingPosition =  new Vector3(tilingPositionX, myTransform.position.y, myTransform.position.z);

		Transform newTiling = (Transform) Instantiate (myTransform, tilingPosition, myTransform.rotation);

		if (ReverseScale == true) 
		{
			newTiling.localScale = new Vector3 (newTiling.localScale.x * -1, newTiling.localScale.y, newTiling.localScale.z);		
		}

		newTiling.transform.parent = myTransform.parent;

		if (direction > 0) 
		{
			newTiling.GetComponent<Tiling>().HasLeftTiling = true;
		}
		else 
		{
			newTiling.GetComponent<Tiling>().HasRightTiling = true;
		}
	}
}

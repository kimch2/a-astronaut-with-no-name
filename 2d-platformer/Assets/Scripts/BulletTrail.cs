using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrail : MonoBehaviour {
	
	public int speed = 230;
	public int lifeTime = 1;
	void Update () {
		transform.Translate (Vector3.right * Time.deltaTime * speed);
		Destroy (gameObject, lifeTime);
	}
}

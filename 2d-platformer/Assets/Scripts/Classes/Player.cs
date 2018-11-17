﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable<int> {

	[System.Serializable]
	public class PlayerStats {
		public int health = 100;
	}

	public int fallBoundary = -20;
	public PlayerStats playerStats = new PlayerStats();

	void Update () {
		if (transform.position.y <= fallBoundary) {
            Kill();
		}
	}

	public void Damage (int damage) {
		playerStats.health -= damage;

		if (playerStats.health <= 0) {
			Kill();
		}
	}

	public void Kill() {
        Destroy(gameObject);
        GameMaster.SpawnPlayer();
	}
	
}
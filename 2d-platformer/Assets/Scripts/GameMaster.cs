using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

	public float spawnDelay = 2f;
	public float spawnParticleLifetime = 3f;
	public Transform playerPrefab;
	public Transform spawnPoint;
	public static GameMaster gm;

    void Start() {
		if (!gm) {
			gm = GameObject.FindGameObjectWithTag ("GM").GetComponent<GameMaster>();
		}
	}

	public IEnumerator RespawnPlayer () {
		yield return new WaitForSeconds(spawnDelay);
		Instantiate (playerPrefab, spawnPoint.position, spawnPoint.rotation);

	}
	
	public static void KillPlayer (Player player) {
		Destroy (player.gameObject);
		gm.StartCoroutine (gm.RespawnPlayer());
	}
}

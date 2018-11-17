using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

	public Transform playerPrefab;
	public Transform spawnPoint;
	public static GameMaster gameMaster;

    void Start() {
		if (!gameMaster) 
		{
			gameMaster = GameObject.FindGameObjectWithTag ("GM").GetComponent<GameMaster>();
		}
	}

	public static void SpawnPlayer () {
		Instantiate (gameMaster.playerPrefab, gameMaster.spawnPoint.position, gameMaster.spawnPoint.rotation);
	}
}

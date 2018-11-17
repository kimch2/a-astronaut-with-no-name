using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

	public Transform playerPrefab;
	public Transform spawnPoint;
	public static GameMaster gameMaster;
	private static CameraShake m_CameraShake;

    void Awake () 
	{
		if (!gameMaster) 
		{
			gameMaster = GameObject.FindGameObjectWithTag ("GM").GetComponent<GameMaster>();
		}
		if (!m_CameraShake)
		{
			m_CameraShake = gameObject.GetComponent<CameraShake>();
		}
	}

	public static void SpawnPlayer () 
	{
		Instantiate (gameMaster.playerPrefab, gameMaster.spawnPoint.position, gameMaster.spawnPoint.rotation);
	}

	public static void ShakeCamera (float amount, float length)
	{
		m_CameraShake.Shake (amount, length);
	}
}

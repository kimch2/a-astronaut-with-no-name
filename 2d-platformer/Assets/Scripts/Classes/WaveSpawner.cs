using UnityEngine;
using System.Collections;

public class WaveSpawner : MonoBehaviour {

	public enum SpawnState { SPAWNING, WAITING, COUNTING };

	[System.Serializable]
	public class Wave
	{
		public string name;
		public Transform enemy;
		public int count;
		public float rate;
	}

	public float timeBetweenWaves = 5f;
    public Wave[] waves;
    public Transform[] spawnPoints;

	public int nextWave
	{
		get { return m_NextWave + 1; }
	}

	public float waveCountdown
	{
		get { return m_WaveCountdown; }
	}

	public SpawnState state
	{
		get { return m_State; }
	}

	private int m_NextWave = 0;
	private float m_WaveCountdown;
	private float m_SearchCountdown = 1f;
	private SpawnState m_State = SpawnState.COUNTING;

	void Start()
	{
		if (spawnPoints.Length == 0)
		{
			Debug.LogError("No spawn points referenced.");
		}

		m_WaveCountdown = timeBetweenWaves;
	}

	void Update()
	{
		if (m_State == SpawnState.WAITING)
		{
			if (!EnemyIsAlive())
			{
				WaveCompleted();
			}
			else
			{
				return;
			}
		}

		if (m_WaveCountdown <= 0)
		{
			if (m_State != SpawnState.SPAWNING)
			{
				StartCoroutine( SpawnWave ( waves[m_NextWave] ) );
			}
		}
		else
		{
			m_WaveCountdown -= Time.deltaTime;
		}
	}

	void WaveCompleted()
	{
		Debug.Log("Wave Completed!");

		m_State = SpawnState.COUNTING;
		m_WaveCountdown = timeBetweenWaves;

		if (m_NextWave + 1 > waves.Length - 1)
		{
			m_NextWave = 0;
			Debug.Log("ALL WAVES COMPLETE! Looping...");
		}
		else
		{
			m_NextWave++;
		}
	}

	bool EnemyIsAlive()
	{
		m_SearchCountdown -= Time.deltaTime;
		if (m_SearchCountdown <= 0f)
		{
			m_SearchCountdown = 1f;
			if (GameObject.FindGameObjectWithTag("Enemy") == null)
			{
				return false;
			}
		}
		return true;
	}

	IEnumerator SpawnWave(Wave _wave)
	{
		Debug.Log("Spawning Wave: " + _wave.name);
		m_State = SpawnState.SPAWNING;

		for (int i = 0; i < _wave.count; i++)
		{
			SpawnEnemy(_wave.enemy);
			yield return new WaitForSeconds( 1f/_wave.rate );
		}

		m_State = SpawnState.WAITING;

		yield break;
	}

	void SpawnEnemy(Transform _enemy)
	{
		Debug.Log("Spawning Enemy: " + _enemy.name);

		Transform _sp = spawnPoints[ Random.Range (0, spawnPoints.Length) ];
		Instantiate(_enemy, _sp.position, _sp.rotation);
	}

}

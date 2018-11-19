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
	private bool m_AllowToSpawnEnemy = true;
	private SpawnState m_State = SpawnState.COUNTING;
    private GameMaster m_GameMaster;


	void Start()
	{
		if (spawnPoints.Length == 0)
		{
			Debug.LogError("No spawn points referenced.");
		}

		m_WaveCountdown = timeBetweenWaves;

        m_GameMaster = GameMaster.instance;
        if (m_GameMaster == null)
        {
            Debug.LogError("No GameMaster in the scene");
        }

        m_GameMaster.onPause += OnPause;
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

    void OnPause(bool active)
    {
        m_AllowToSpawnEnemy = !active;
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

	IEnumerator SpawnWave(Wave wave)
	{
		Debug.Log("Spawning Wave: " + wave.name);
		m_State = SpawnState.SPAWNING;

		for (int i = 0; i < wave.count; i++)
		{
            yield return new WaitUntil(() => m_AllowToSpawnEnemy);
			SpawnEnemy(wave.enemy);
			yield return new WaitForSeconds( 1f/wave.rate );
		}

		m_State = SpawnState.WAITING;

		yield break;
	}

	void SpawnEnemy(Transform enemy)
	{
		Transform randomSpawnPoint = spawnPoints[ Random.Range (0, spawnPoints.Length) ];
		Instantiate(enemy, randomSpawnPoint.position, randomSpawnPoint.rotation);
	}

}

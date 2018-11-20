using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveSpawner : MonoBehaviour {

	public enum SpawnState { SPAWNING, WAITING, COUNTING };

	[System.Serializable]
	public class Wave
	{
		public Transform enemy;
		public int count;
		public float rate;
	}

	public float timeBetweenWaves = 5f;
	public int WaveCompletedXpDrop = 5;
    public Transform[] spawnPoints;
	public Wave initialWave;
	public float waveDifficultyIncreaseRate;

	public int currentWave
	{
		get { return m_CurrentWave + 1; }
	}

	public float waveCountdown
	{
		get { return m_WaveCountdown; }
	}

	public SpawnState state
	{
		get { return m_State; }
	}

	private int m_CurrentWave = 0;
	private float m_WaveCountdown;
	private float m_SearchCountdown = 1f;
	private bool m_AllowToSpawnEnemy = true;
    private List<Wave> m_Waves = new List<Wave>();
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

		m_Waves.Add(initialWave);

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
				StartCoroutine(SpawnWave(m_Waves[m_CurrentWave]));
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
		m_State = SpawnState.COUNTING;
		m_WaveCountdown = timeBetweenWaves;

		m_GameMaster.AddXP((int) (WaveCompletedXpDrop * waveDifficultyIncreaseRate));

		Wave wave = new Wave {
			enemy = m_Waves[m_CurrentWave].enemy,
			rate = m_Waves[m_CurrentWave].rate * waveDifficultyIncreaseRate,
			count = (int) (m_Waves[m_CurrentWave].count + waveDifficultyIncreaseRate)
		};

        m_Waves.Add(wave);
		m_CurrentWave++;
		
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

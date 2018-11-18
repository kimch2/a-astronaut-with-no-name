using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour {

	[SerializeField]
	private WaveSpawner m_Spawner;

	[SerializeField]
	private Animator m_Animator;

    [SerializeField]
	private Text m_WaveText;


	void Start () 
	{
		if (m_Spawner == null) {
			Debug.LogError("WAVEUI: No waveSpawner referenced!");
			this.enabled = false;
		}
		if (m_Animator == null) {
			Debug.LogError("WAVEUI: No animator referenced!");
			this.enabled = false;
		}
		if (m_WaveText == null) {
			Debug.LogError("WAVEUI: No waveText referenced!");
			this.enabled = false;
		}
	}
	
	void Update () 
	{
		UpdateIncomingUI();
	}

	void UpdateIncomingUI ()
	{
		if (m_Spawner.state == WaveSpawner.SpawnState.COUNTING)
		{
			m_Animator.SetBool ("WaveIncoming", true);
		}
		else 
		{
			m_Animator.SetBool ("WaveIncoming", false);
		}
	}
}

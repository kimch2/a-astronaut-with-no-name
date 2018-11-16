
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (Seeker))]
public class EnemyIA : MonoBehaviour {
	
	[HideInInspector]
	public bool reachTarget = false;

	public float updateRate = 2f;
	public float speed = 300f;
	public float maxWaypointDistance = 3f;
	public Path path;
	public ForceMode2D fMode;
	public Transform target;
    private float m_Distance;
	private int m_CurrentWaypoint = 0;
    private Vector3 m_Direction;
	private Seeker m_Seeker;
	private Rigidbody2D m_Rigidbody2D;
	

	void Awake () {
        m_Seeker = GetComponent<Seeker>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
	}

	void Start () {
		if (!target) {
			Debug.LogError("No player found");
			return;
		}

		m_Seeker.StartPath (transform.position, target.position, OnPathDelegate);

		StartCoroutine (UpdatePath ());

	}

	void FixedUpdate () {
		if (!target) return;
		if (path == null) return;

		if (m_CurrentWaypoint >= path.vectorPath.Count) {
			if (reachTarget) return;

            reachTarget = true;
			return;
		}

		reachTarget = false;

		m_Direction = (path.vectorPath[m_CurrentWaypoint] - transform.position).normalized;
        m_Direction *= speed * Time.fixedDeltaTime;
		m_Rigidbody2D.AddForce (m_Direction, fMode);

        m_Distance = Vector3.Distance(transform.position, path.vectorPath[m_CurrentWaypoint]);

		if (m_Distance < maxWaypointDistance) {
			m_CurrentWaypoint++;
			return;
		}

	}

	void OnPathDelegate (Path p) {
		if (p.error) {
			Debug.LogError(p.error);
			return;
		}
		path = p;
		m_CurrentWaypoint = 0;

	}

	IEnumerator UpdatePath () {
		if (!target) {
			//TODO: search player
			yield return false;
		}

		m_Seeker.StartPath(transform.position, target.position, OnPathDelegate);

		yield return new WaitForSeconds (1f/updateRate);
        StartCoroutine(UpdatePath ());
    }

}

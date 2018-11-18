
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
	public float searchTargetRate = .5f;
	public string targetTag = "Player";
	public Path path;
	public ForceMode2D fMode;
	public Transform target;
	private int m_CurrentWaypoint = 0;
	private Seeker m_Seeker;
	private Rigidbody2D m_Rigidbody2D;
    private bool m_SearchingForTarget = false;


    void Awake () {
        m_Seeker = GetComponent<Seeker>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
	}

	void Start () {
		if (target == null) 
		{
            if (!m_SearchingForTarget) 
			{
                m_SearchingForTarget = true;
				StartCoroutine (SearchTarget());
			}
			return;
		}

        m_Seeker.StartPath (transform.position, target.position, OnPathDelegate);

		StartCoroutine (UpdatePath ());

	}

	void FixedUpdate () {
        if (target == null)
        {
            if (!m_SearchingForTarget)
            {
                m_SearchingForTarget = true;
                StartCoroutine(SearchTarget());
            }
            return;
        }
		if (path == null) return;
		if (m_CurrentWaypoint >= path.vectorPath.Count) 
		{
			if (reachTarget) return;

            reachTarget = true;
			return;
		}
		reachTarget = false;

        Vector3 direction = (path.vectorPath[m_CurrentWaypoint] - transform.position).normalized;
        direction *= speed * Time.fixedDeltaTime;

		m_Rigidbody2D.AddForce (direction, fMode);

        float distance = Vector3.Distance(transform.position, path.vectorPath[m_CurrentWaypoint]);
		if (distance < maxWaypointDistance) 
		{
			m_CurrentWaypoint++;
			return;
		}

	}

	void OnPathDelegate (Path p) {
		if (p.error) return;
		path = p;
		m_CurrentWaypoint = 0;
	}

	IEnumerator UpdatePath () {
        if (target == null)
        {
            if (!m_SearchingForTarget)
            {
                m_SearchingForTarget = true;
                StartCoroutine(SearchTarget());
            }
            yield break;
        }

        m_Seeker.StartPath(transform.position, target.position, OnPathDelegate);

		yield return new WaitForSeconds (1f/updateRate);
        StartCoroutine(UpdatePath ());
    }

	IEnumerator SearchTarget () {
		GameObject targetObject = GameObject.FindGameObjectWithTag (targetTag);
		if (targetObject == null) 
		{
			yield return new WaitForSeconds (searchTargetRate);
            StartCoroutine(SearchTarget()); 
		} 
		else 
		{
			target = targetObject.transform;
			m_SearchingForTarget = false;
            StartCoroutine(UpdatePath());
            yield break;
		}
	}

}

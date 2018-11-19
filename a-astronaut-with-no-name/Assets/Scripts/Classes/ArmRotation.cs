using UnityEngine;

public class ArmRotation : MonoBehaviour {

	public int offset = 90;
    private Camera m_MainCamera;
    private GameMaster m_GameMaster;

	void Awake() 
    {
        m_MainCamera = Camera.main;
	}

    void Start() 
    {
        m_GameMaster = GameMaster.instance;
        if (m_GameMaster == null)
        {
            Debug.LogError("No GameMaster in the scene");
        }

        m_GameMaster.onPause += OnPause;
    }

    void Update () 
    {
        Vector3 mouseToArmDifference = m_MainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        mouseToArmDifference.Normalize();

        float rotationZ = Mathf.Atan2(mouseToArmDifference.y, mouseToArmDifference.x) * Mathf.Rad2Deg;
		
		transform.rotation = Quaternion.Euler(0f, 0f, rotationZ + offset);
	}
    
    void OnPause(bool active)
    {
        this.enabled = !active;
    }
}

using UnityEngine;
using UnityEngine.UI;

public class StatusIndicator : MonoBehaviour {

	[SerializeField]
	private RectTransform healthBarRect;
	private Image m_healthBarImage;


	void Start () 
	{
		if (healthBarRect == null)
		{
			Debug.LogError ("STATUS INDICATOR: No health bar object referenced!");
		}
	}

	public void SetHealth (int currentHealth, int maxHealth)
	{
		float value = (float) currentHealth / maxHealth;
		healthBarRect.localScale = new Vector3(value, healthBarRect.localScale.y, healthBarRect.localScale.z);
		if (m_healthBarImage == null) m_healthBarImage = healthBarRect.GetComponent<Image>();
        m_healthBarImage.color = Color.Lerp(Color.red, Color.green, value * 1.2f);
	}
	
}

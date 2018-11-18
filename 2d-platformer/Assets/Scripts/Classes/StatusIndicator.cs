using UnityEngine;
using UnityEngine.UI;

public class StatusIndicator : MonoBehaviour {

	[SerializeField]
	private RectTransform healthBarRect;
	private Image m_HealthBarImage;

	void Start () 
	{
		if (healthBarRect == null)
		{
			Debug.LogError ("STATUS INDICATOR: No health bar object referenced!");
		}
	}

	public void SetHealth (int currentHealth, int maxHealth)
	{
		float health = (float) currentHealth / maxHealth;
		healthBarRect.localScale = new Vector3(health, healthBarRect.localScale.y, healthBarRect.localScale.z);

		if (m_HealthBarImage == null) m_HealthBarImage = healthBarRect.GetComponent<Image>();

        m_HealthBarImage.color = Color.Lerp(Color.red, Color.green, health * 1.2f);
	}
	
}

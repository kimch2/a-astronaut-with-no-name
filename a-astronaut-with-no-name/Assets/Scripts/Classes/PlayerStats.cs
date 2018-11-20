using UnityEngine;

public class PlayerStats : MonoBehaviour {

    public int maxHealth = 100;
    public float healthRegenerateRate = 2f;
    public int healthRegenerateAmount = 1;
   	public float maxSpeed = 10f;                    
   	public float jumpForce = 600f;   
    public float weaponDamageMultiplier = 1f;            
    public float weaponFireRateMultiplier = 1f;            
    public static PlayerStats instance;
    public int currentHealth
    {
        get 
		{ 
			return m_CurrentHealth; 
		}
        set 
		{ 
			m_CurrentHealth = Mathf.Clamp(value, 0, maxHealth); 
		}
    }


    private int m_CurrentHealth;

    public void Awake()
    {
		if (instance == null) instance = this;
        currentHealth = maxHealth;
    }
}


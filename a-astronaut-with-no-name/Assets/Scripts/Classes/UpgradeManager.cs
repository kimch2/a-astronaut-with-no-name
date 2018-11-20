using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour {

    public int jumpIncreasePerUpgrade = 100;
    public float speedIncreasePerUpgrade = 1f;
    public int healthIncreasePerUpgrade = 10;
    public int jumpCostPerUpgrade = 35;
    public int speedCostPerUpgrade = 20;
    public int healthCostPerUpgrade = 30;
    public string pressButtonSound = "BonusSound";
    public string noXPButtonSound = "EmptyClickSound";
    public string hoverButtonSound = "HoverButtonSound";

    [SerializeField] private Text m_HealthText;
    [SerializeField] private Text m_SpeedText;
    [SerializeField] private Text m_JumpText;
    [SerializeField] private Text m_HealthButtonText;
    [SerializeField] private Text m_SpeedButtonText;
    [SerializeField] private Text m_JumpButtonText;
	private PlayerStats m_PlayerStats;
    private AudioManager m_AudioManager;
    private GameMaster m_GameMaster;

	void Awake () 
	{
        m_PlayerStats = PlayerStats.instance;
        if (m_PlayerStats == null)
        {
            Debug.LogError("No PlayerStats in the scene");
        }

        m_AudioManager = AudioManager.instance;
        if (m_AudioManager == null)
        {
            Debug.LogError("No AudioManager in the scene");
        }

        m_GameMaster = GameMaster.instance;
        if (m_GameMaster == null)
        {
            Debug.LogError("No GameMaster in the scene");
        }
    }
    
	void OnEnable()
	{
		UpdateValues();
	}
    public void OnMouseEnter()
    {
        m_AudioManager.PlaySound(hoverButtonSound);
    }

    public void displayButtonPressSound()
    {
        m_AudioManager.PlaySound(pressButtonSound);
    }
    public void displayNoXPSound()
    {
        m_AudioManager.PlaySound(noXPButtonSound);
    }
	
	void UpdateValues () 
	{
        m_HealthText.text 	= 	"HEALTH: " 	+ 	m_PlayerStats.maxHealth.ToString();
		m_SpeedText.text 	= 	"SPEED: " 	+ 	m_PlayerStats.maxSpeed.ToString();
		m_JumpText.text 	= 	"JUMP: " 	+ 	m_PlayerStats.jumpForce.ToString();
        
        m_HealthButtonText.text = "LEVEL UP ("+ healthCostPerUpgrade.ToString() + ")";
        m_SpeedButtonText.text  = "LEVEL UP (" + speedCostPerUpgrade.ToString() + ")";
        m_JumpButtonText.text   = "LEVEL UP (" + jumpCostPerUpgrade.ToString() + ")";
	}

	public void UpgradeHealth ()
	{
		if (m_GameMaster.xp < healthCostPerUpgrade) 
		{
			displayNoXPSound();
			return;
		}
		m_PlayerStats.maxHealth += healthIncreasePerUpgrade;
        m_GameMaster.SubtractXP(healthCostPerUpgrade);
		UpdateValues();
		displayButtonPressSound();
	}

	public void UpgradeSpeed ()
	{
        if (m_GameMaster.xp < speedCostPerUpgrade)
        {
            displayNoXPSound();
            return;
        }
		m_PlayerStats.maxSpeed += speedIncreasePerUpgrade;
        m_GameMaster.SubtractXP(speedCostPerUpgrade);
		UpdateValues();
		displayButtonPressSound();
	}

	public void UpgradeJump ()
	{
        if (m_GameMaster.xp < jumpCostPerUpgrade)
        {
            displayNoXPSound();
            return;
        }
		m_PlayerStats.jumpForce += jumpIncreasePerUpgrade;
        m_GameMaster.SubtractXP(jumpCostPerUpgrade);
		UpdateValues();
		displayButtonPressSound();
	}
}

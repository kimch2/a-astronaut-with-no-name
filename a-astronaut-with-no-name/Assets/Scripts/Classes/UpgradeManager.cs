using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour {

    public int xpCost = 1;
    public string pressButtonSound = "BonusSound";
    public string noXPButtonSound = "EmptyClickSound";
    public string hoverButtonSound = "HoverButtonSound";

    [SerializeField] private Text m_HealthText;
    [SerializeField] private Text m_SpeedText;
    [SerializeField] private Text m_JumpText;
    [SerializeField] private Text m_DamageText;
    [SerializeField] private Text m_RateText;
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
		m_DamageText.text 	= 	"DAMAGE: " 	+ 	m_PlayerStats.weaponDamageMultiplier.ToString();
		m_RateText.text 	= 	"RATE: " 	+  	m_PlayerStats.weaponFireRateMultiplier.ToString();
	}

	public void UpgradeHealth ()
	{
		if (m_GameMaster.xp <= 0) 
		{
			displayNoXPSound();
			return;
		}
		m_PlayerStats.maxHealth++;
        m_GameMaster.SubtractXP(xpCost);
		UpdateValues();
		displayButtonPressSound();
	}

	public void UpgradeSpeed ()
	{
        if (m_GameMaster.xp <= 0)
        {
            displayNoXPSound();
            return;
        }
		m_PlayerStats.maxSpeed++;
        m_GameMaster.SubtractXP(xpCost);
		UpdateValues();
		displayButtonPressSound();
	}

	public void UpgradeJump ()
	{
        if (m_GameMaster.xp <= 0)
        {
            displayNoXPSound();
            return;
        }
		m_PlayerStats.jumpForce++;
        m_GameMaster.SubtractXP(xpCost);
		UpdateValues();
		displayButtonPressSound();
	}
}

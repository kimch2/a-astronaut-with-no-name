using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class XPCounterUI : MonoBehaviour {

    private GameMaster m_GameMaster;
	private Text moneyText;

	void Awake ()
	{
        moneyText = GetComponent<Text>();
	}

	void Start()
    {
        m_GameMaster = GameMaster.instance;
        if (m_GameMaster == null)
        {
            Debug.LogError("No GameMaster in the scene");
        }

    }

    void Update()
    {
		moneyText.text = "XP: " + m_GameMaster.xp.ToString();
    }
}
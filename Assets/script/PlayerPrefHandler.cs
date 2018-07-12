using UnityEngine;
using System.Collections;




public class PlayerPrefHandler : MonoBehaviour {

	public static PlayerPrefHandler instance;

	public const string keyMusic = "MusicOn";
	public const string keySound = "SoundOn";
	public const string keyPause = "statPause";
	public const string keyUserName = "UserName";
	public const string keyToken = "Token";
	//public const string keyLoginActive = "statLoginActive";
	public const string keyHalAsal = "hal_asal";
	public const string keyCoin = "coins";
	public const string keyUserTiket = "tiket_game_user";
	public const string keyCombo = "comboMax";
	public const string keyGameId = "gameId";
	public const string keyCurrentScore = "currentScore";
	public const string keyLevelAktif = "gamelevelaktif";
	public const string keyLastLogin = "lastLogin";
	//public const string keyWaktuCountLogin = "waktuCountLogin";
	public const string keyRateUs = "rateus";
	public const string keyClawFreeEnergyTimer = "clawFreeEnergyTimer";
	public const string keyFreeCoinTimer = "timerfreecoin";
	public const string keyFreeCoinsCount = "FreeCoinsCount";

	public const string keyBomb = "BombCount";
	public const string keyLaser = "LaserCount";
	public const string keyCharm = "CharmCount";
	public const string keyWand = "WandCount";
	public const string keyBlackHole = "BlackHoleCount";
	public const string keyPowerupCount = "PowerupCount";

	public const string keyBombFlag = "BombFlag";
	public const string keyLaserFlag = "LaserFlag";
	public const string keyCharmFlag = "CharmFlag";
	public const string keyWandFlag = "WandFlag";
	public const string keyHoleFlag = "HoleFlag";

	public const string keyEnergyAch = "EnergyAchUnlocked";

	public const string keyTotalAchCount = "TotalAchCount";

	void Awake () {
		if (!instance)
		{
			DontDestroyOnLoad(this.gameObject);
			instance = this;
		}
		else if (instance != this)
		{
			DestroyImmediate(this.gameObject);
		}
	}

	// Use this for initialization
	void Start () {

	}

	void OnDestroy()
	{
		if (instance == this)
			instance = null;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

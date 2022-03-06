using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WaveSpawner : MonoBehaviour {

	[Header("Spawn Attributes")]
		public float timeBetweenWaves = 5f;
		public float spawnDelay = 0.5f;

	[Header("Base Enemy Attributes")]
		public float baseSpeedConst = 1f;
		public float baseHPConst = 100f;
		public float SpeedWaveConst = 1.1f;
		public float HPWaveConst = 1.1f;

	[Header("Define The EnemyPrefab Array")]
		public GameObject[] enemyPrefab;

	[Header("Define The Waves Enemies")]
		public string[] ArrayEnemies;

	[Header("Text Boxes for UI")]
		public GameObject CooldownText;
		public GameObject CooldownNum;
	[Space(5)]
		public GameObject WaveText;
		public GameObject WaveNum;

	//hidden from inspector
	private int waveNumber = 1;
	private int moduleIndex = 0;
	private float baseSpeed;
	private float baseHP;
	private Text waveNumberText;
	private Text waveCountdownText;
	private Transform[] spawnPoint;
	private GameObject masterTower;
	private GameObject[] thisWaveSpawnEnemies;
	private SoulsCounter soulsConter;
	private WayPointsScript[] wayPoints;
	private MasterTowerScript masterTowerScript;
	private Text warningWaveText;
	private ActionManager actionManager;

	public void SetModule(Transform spawnpoint, WayPointsScript waypoint)
    {
		spawnPoint [moduleIndex] = spawnpoint;
		wayPoints [moduleIndex] = waypoint;
		moduleIndex++;
	}

    public float GetWave()
    {
        return waveNumber;
    }

    public void StartNextWave()
    {
        waveNumber++;
        AdjustDifficulty();
        UpdateSoul();
        UpdateLifes();
        StartCoroutine(SpawnWave());
    }

    //Update the User Interface with wave and time remain for next wave information
    public void UpdateUI(float countdown)
    {
        if (IsInCorrectScene())
        {
            waveCountdownText.text = Mathf.Round(countdown).ToString();
            waveNumberText.text = Mathf.Round(waveNumber - 1).ToString();
            warningWaveText.text = Mathf.Round(waveNumber - 1).ToString();
        }
    }

	public float GetBaseSpeed()
    {
		return baseSpeed;
	}

	public float GetBaseHP()
    {
		return baseHP;
	}

	private void AdjustArray()
    {
		int sizeArrEn;
		int waveNumIdx;
		if ( waveNumber > ArrayEnemies.Length ) 
        {
			KillPlayer ();
			waveNumIdx = ArrayEnemies.Length - 1;
			sizeArrEn = ArrayEnemies [waveNumIdx].Length;
		} 
        else
        {
			waveNumIdx = waveNumber - 2;
			sizeArrEn = ArrayEnemies [waveNumIdx].Length;
		}
		int[] auxArr = new int[sizeArrEn];
		int auxArrIdx = 0;
		int indexVal = 0;
		int mult = 1;
		int actualVal = 0;
		for ( int i = sizeArrEn-1; i >= 0; i -- )
        {
			actualVal = (ArrayEnemies[waveNumIdx][i] - '0');
			if(actualVal == ( ',' - '0' ) ){
				auxArr[auxArrIdx] = indexVal;
				auxArrIdx++;
				mult = 1;
				indexVal = 0;
				continue;
			}
			else 
            {
				indexVal += actualVal * mult;
				mult = mult * 10;
			}
		}
		auxArr[auxArrIdx] = indexVal;
		thisWaveSpawnEnemies = new GameObject[auxArrIdx + 1];
		int j = 0; 
		for (int i = auxArrIdx; i >= 0; i--) 
        {
			thisWaveSpawnEnemies[j] = enemyPrefab[ auxArr[i] - 1 ];
			j++;
		}
	}

	private void KillPlayer()
    {
		for (int i = 0; i < 10; i++)
			AdjustDifficulty ();
	}

	private void AdjustDifficulty()
    {
		baseSpeed *= SpeedWaveConst;
		baseSpeed = Mathf.Clamp (baseSpeed, 1f, 2f);
		baseHP *= HPWaveConst;
	}

	private void Awake()
    {
		moduleIndex = 0;
		spawnPoint = new Transform[4];
		wayPoints = new WayPointsScript[4];
	}

	private void Start()
    {
		masterTower = GameObject.Find ("MasterTower");
		soulsConter = GetComponent<SoulsCounter>();
		masterTowerScript = masterTower.GetComponent<MasterTowerScript>();
        actionManager = gameObject.GetComponent<ActionManager>();
        baseSpeed = baseSpeedConst;
        baseHP = baseHPConst;
        if (IsInCorrectScene() == false) return;
        waveNumberText = WaveText.GetComponent<Text>();
        waveCountdownText = CooldownNum.GetComponent<Text>();
        warningWaveText = GameObject.Find("WaveWarningNum").GetComponent<Text>();


		if (!CooldownText)	Debug.LogError("fuck 1");
		if (!CooldownNum)	Debug.LogError("fuck 2");
		if (!WaveText)		Debug.LogError("fuck 3");
		if (!WaveNum)		Debug.LogError("fuck 4");
	}

    private bool IsInCorrectScene()
    {
        return !(SceneManager.GetActiveScene().buildIndex != 0 && string.Equals(SceneManager.GetActiveScene().name, "MainMenu"));
    }

	//Instantiate the Enemy and set the waypoints
	private void EnemySpawn(GameObject enemyPrefab)
    {
		for (int i = 0; i < 4; i++) 
        {
			GameObject enemyGameObj = (GameObject)Instantiate (enemyPrefab, spawnPoint [i].position, spawnPoint [i].rotation);
			enemyGameObj.GetComponent<Enemy> ().SetWayPoints (wayPoints [i]);
            actionManager.IncrementEnemyCount();
		}
	}

	/// <summary>
	/// Updates the soul.
	/// </summary>
	private void UpdateSoul()
    {
        if (IsInCorrectScene())
            soulsConter.SetWave (waveNumber - 1);
	}

	/// <summary>
	/// Updates the lifes.
	/// </summary>
	private void UpdateLifes()
    {
        if (IsInCorrectScene())
            masterTowerScript.NewWave ();
	}

    //Coroutine for the spawn, it delays spawnDelay for each instantiation
    private IEnumerator SpawnWave()
    {
        AdjustArray();
        actionManager.SetSpawnTrue();
        for (int i = 0; i < thisWaveSpawnEnemies.Length; i++)
        {  
            EnemySpawn(thisWaveSpawnEnemies[i]);
            yield return new WaitForSeconds(spawnDelay);
        }
        actionManager.SetSpawnFalse();
    }
}
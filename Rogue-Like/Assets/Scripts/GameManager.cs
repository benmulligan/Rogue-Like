using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour {


	public float levelStartDelay = 2.0f;
	public float turnDelay = 0.1f;
	public static GameManager instance = null;
	public BoardManager boardScript;
	public int playerFoodPoints = 100;
	[HideInInspector] public bool playersTurn = true;

	private Text levelText;
	private GameObject levelImage;
	private bool doingSetup;
	private int level = 0;
	private List<Enemy> enemies;
	private bool enemiesMoving;

	// Use this for initialization
	void Awake () {

		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);
		this.enemies = new List<Enemy> ();
		this.boardScript = GetComponent<BoardManager> ();
	}

	private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
		Debug.Log ("OnLevelFinishedLoading");
		this.level++;
		this.InitGame ();
	}

	// hook onto the scene loaded event
	void OnEnable() {
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
	}

	void OnDisable() {
		SceneManager.sceneLoaded -= OnLevelFinishedLoading;
	}

	void InitGame() {
		Debug.Log ("--- INIT");
		this.doingSetup = true;

		this.levelImage = GameObject.Find ("LevelImage");
		this.levelText = GameObject.Find ("LevelText").GetComponent<Text>();
		this.levelText.text = "Day " + this.level;
		this.levelImage.SetActive (true);

		this.enemies.Clear ();
		this.boardScript.SetupScene (this.level);

		Invoke ("HideLevelImage", this.levelStartDelay);
	}

	void HideLevelImage() {
		this.doingSetup = false;
		this.levelImage.SetActive (false);
	}

	public void GameOver() {
		this.levelText.text = "Day " + this.level + ": You died.";
		this.levelImage.SetActive (true);

		this.enabled = false;
	}

	// Update is called once per frame
	void Update () {

		if (this.playersTurn || this.enemiesMoving || doingSetup) {
			return; 
		}

		StartCoroutine (this.MoveEnemies ());
		
	}

	public void AddEnemyToList(Enemy script)
	{
		this.enemies.Add (script);
	}


	private IEnumerator MoveEnemies() {
		this.enemiesMoving = true;

		yield return new WaitForSeconds(this.turnDelay);

		if (this.enemies.Count == 0) {
			yield return new WaitForSeconds(this.turnDelay);

		}

		for (int i = 0; i < this.enemies.Count; i++) {
			this.enemies[i].MoveEnemy();
			yield return new WaitForSeconds(this.enemies[i].moveTime);
		}

		this.playersTurn = true;
		this.enemiesMoving = false;
	}

}

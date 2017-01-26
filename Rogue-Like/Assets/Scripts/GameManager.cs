using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public float turnDelay = 0.1f;
	public static GameManager instance = null;
	public BoardManager boardScript;
	public int playerFoodPoints = 100;
	[HideInInspector] public bool playersTurn = true;

	private int level = 3;
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
		this.InitGame ();
	}

	void InitGame() {
		this.enemies.Clear ();
		this.boardScript.SetupScene (this.level);
	
	}

	public void GameOver() {
		this.enabled = false;
	}

	// Update is called once per frame
	void Update () {

		if (this.playersTurn || this.enemiesMoving) {
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

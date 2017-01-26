using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	public BoardManager boardScript;
	public int playerFoodPoints = 100;
	[HideInInspector] public bool playersTurn = true;

	private int level = 3;

	// Use this for initialization
	void Awake () {

		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);
		this.boardScript = GetComponent<BoardManager> ();
		this.InitGame ();
	}

	void InitGame() {
		this.boardScript.SetupScene (this.level);
	
	}

	public void GameOver() {
	}

	// Update is called once per frame
	void Update () {
		
	}
}

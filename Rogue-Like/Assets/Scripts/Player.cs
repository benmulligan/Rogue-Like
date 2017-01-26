using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MovingObject {

	public int wallDamage = 1;
	public int pointsForFood = 10;
	public int pointsForSoda = 20;
	public int restartLevelDelay = 1;
	public Text foodText;

	private Animator animator;
	private int food;



	// Use this for initialization
	protected override void Start () {
		this.animator = this.GetComponent<Animator> ();
		this.food = GameManager.instance.playerFoodPoints;
		this.foodText.text = "Food: " + this.food;

		base.Start ();
	}

	private void Update() {
		if (GameManager.instance.playersTurn == false) {
			return; 
		}

		int horizontal = (int)Input.GetAxisRaw ("Horizontal");
		int vertical = (int)Input.GetAxisRaw ("Vertical");

		if (horizontal != 0) {
			vertical = 0; 
		}

		if (horizontal != 0 || vertical != 0) {
			AttemptMove<Wall> (horizontal, vertical);
		}

	}

	private void OnTriggerEnter2D (Collider2D other) {

		if (other.tag == "Exit") {
			Invoke ("Restart", this.restartLevelDelay);
		} else if (other.tag == "Food") {
			this.food += this.pointsForFood;
			this.foodText.text = "+" + this.pointsForFood + " | Food: " + this.food;
			other.gameObject.SetActive (false);
		} else if (other.tag == "Soda") {
			this.food += this.pointsForSoda;
			this.foodText.text = "+" + this.pointsForSoda + " | Food: " + this.food;
			other.gameObject.SetActive (false);
		}
	}


	protected override void OnCantMove<T> (T component)
	{
		Wall hitWall = component as Wall;
		hitWall.DamageWall (this.wallDamage);

		animator.SetTrigger ("playerChop");
	}

	private void Restart() {
		Debug.Log ("Restart");
		SceneManager.LoadScene (0);
	}

	public void LoseFood(int x) {
		this.animator.SetTrigger ("playerHit");
		this.food -= x;
		this.foodText.text = "-" + x + " | Food: " + this.food;

		this.CheckIfGameOver ();
	}

	private void OnDisable() {
		GameManager.instance.playerFoodPoints = this.food;
	}

	protected override void AttemptMove <T> (int xDir, int yDir) {
		Debug.Log ("Moving");
		this.food--;
		this.foodText.text = "Food: " + this.food;

		base.AttemptMove<T> (xDir, yDir);

		RaycastHit2D hit;
		this.CheckIfGameOver ();

		GameManager.instance.playersTurn = false;
	}

	private void CheckIfGameOver () {
		if (this.food <= 0) {
			GameManager.instance.GameOver ();
		}
	}
}

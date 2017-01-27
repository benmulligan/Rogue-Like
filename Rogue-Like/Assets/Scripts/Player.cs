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

	// UI
	public Text foodText;


	// Audio
	public AudioClip moveSound1;
	public AudioClip moveSound2;
	public AudioClip eatSound1;
	public AudioClip eatSound2;
	public AudioClip drinkSound1;
	public AudioClip drinkSound2;
	public AudioClip gameOverSound;


	private Animator animator;

	// State
	private int food;

	private Vector2 touchPos = -Vector2.one;

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

		int horizontal = 0;
		int vertical = 0;

		#if UNITY_STANDALONE || UNITY_WEBPLAYER
		horizontal = (int)Input.GetAxisRaw ("Horizontal");
		vertical = (int)Input.GetAxisRaw ("Vertical");

		if (horizontal != 0) {
			vertical = 0; 
		}

		#else 

		if (Input.touchCount > 0) {
			Touch t = Input.touches[0];
			if (t.phase == TouchPhase.Began) {
				this.touchPos = t.position;
			} else if (t.phase == TouchPhase.Ended && t.position.x >= 0) {
				float deltaX = t.position.x - this.touchPos.x;
				float deltaY = t.position.y - this.touchPos.y;
				this.touchPos.x = -1; // prevents this from repeating without a new touch event

				// figure out what direction the user attempted to input
				if (Mathf.Abs(deltaX) > Mathf.Abs(deltaY)) {
					horizontal = deltaX > 0 ? 1 : -1;
				} else {
					vertical = deltaY > 0 ? 1 : -1;
				}
			}
		}

		#endif

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
			SoundManager.instance.RandomizeSfx (this.eatSound1, this.eatSound2);
			other.gameObject.SetActive (false);
		} else if (other.tag == "Soda") {
			this.food += this.pointsForSoda;
			this.foodText.text = "+" + this.pointsForSoda + " | Food: " + this.food;
			SoundManager.instance.RandomizeSfx (this.drinkSound1, this.drinkSound2);
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
		this.food--;
		this.foodText.text = "Food: " + this.food;

		base.AttemptMove<T> (xDir, yDir);

		RaycastHit2D hit;
		if (Move (xDir, yDir, out hit)) {
			SoundManager.instance.RandomizeSfx (this.moveSound1, this.moveSound2);
		}

		this.CheckIfGameOver ();

		GameManager.instance.playersTurn = false;
	}

	private void CheckIfGameOver () {
		if (this.food <= 0) {
			GameManager.instance.GameOver ();
			SoundManager.instance.PlaySingle (this.gameOverSound);
			SoundManager.instance.musicSource.Stop ();
		}
	}
}

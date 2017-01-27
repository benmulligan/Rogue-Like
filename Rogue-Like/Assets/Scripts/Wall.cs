using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

	public Sprite dmgSprite;
	public int hp = 4;

	// Audio
	public AudioClip chopSound1;
	public AudioClip chopSound2;

	private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Awake () {
		this.spriteRenderer = this.GetComponent<SpriteRenderer> ();
	}

	public void DamageWall(int dmg) {

		if (dmg > 0) {
			spriteRenderer.sprite = dmgSprite;
		}

		hp -= dmg;
		SoundManager.instance.RandomizeSfx (this.chopSound1, this.chopSound2);
		this.gameObject.SetActive (this.hp > 0);
	}
}

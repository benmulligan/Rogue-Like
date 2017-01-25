using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

	public Sprite dmgSprite;
	public int hp = 4;

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
		this.gameObject.SetActive (this.hp > 0);
	}
}

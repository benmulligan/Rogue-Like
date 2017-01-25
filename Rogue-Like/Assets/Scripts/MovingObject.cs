using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour {

	public float moveTime = 0.1f;
	public LayerMask blockingLayer;

	private BoxCollider2D boxCollider;
	private Rigidbody2D rb2d;
	private float inverseMoveTime;

	// Use this for initialization
	protected virtual void Start () {
		this.boxCollider = GetComponent<BoxCollider2D> ();
		this.rb2d = GetComponent<Rigidbody2D> ();
		this.inverseMoveTime = 1.0f / this.moveTime;
	}

	protected bool Move (int xDir, int yDir, out RaycastHit2D hit) {
		Vector2 start = transform.position;
		Vector2 end = start + new Vector2 (xDir, yDir);

		this.boxCollider.enabled = false;
		hit = Physics2D.Linecast (start, end, this.blockingLayer);
		this.boxCollider.enabled = true;

		if (hit.transform == null) {
			StartCoroutine (SmoothMovement (end));
			return true;
		}

		return false;
	}

	protected IEnumerator SmoothMovement(Vector3 end) {
		float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

		while (sqrRemainingDistance > float.Epsilon) {
			Vector3 newPos = Vector3.MoveTowards (rb2d.position, end, inverseMoveTime * Time.deltaTime);
			rb2d.MovePosition (newPos);
			sqrRemainingDistance = (transform.position - end).sqrMagnitude;
			yield return null;
		}
	}

	protected virtual void AttemptMove <T> (int xDir, int yDir) where T:Component {
		RaycastHit2D hit;
		bool canMove = this.Move (xDir, yDir, out hit);

		if (hit.transform == null) {
			return;
		}

		T hitComponent = hit.transform.GetComponent<T> ();
		if (!canMove && hitComponent != null) {
			this.OnCantMove (hitComponent);
		}
	}

	protected abstract void OnCantMove <T> (T component) where T:Component;

}

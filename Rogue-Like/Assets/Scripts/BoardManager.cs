using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;


public class BoardManager : MonoBehaviour {

	[Serializable]
	public class Count
	{
		public int minimum;
		public int maximum;


		public Count (int min, int max) {
			minimum = min;
			maximum = max;
		}
	}

	public int columns = 8;
	public int rows = 8;
	public Count wallCount = new Count(5,9);
	public Count foodCount = new Count(1,5);
	public GameObject exit;
	public GameObject[] floorTiles;
	public GameObject[] outerWallTiles;
	public GameObject[] wallTiles;
	public GameObject[] enemyTiles;
	public GameObject[] foodTiles;

	private Transform boardHolder;
	private List<Vector3> gridPositions = new List<Vector3>();

	void InitializeList() {
		this.gridPositions.Clear ();

		for (int r = 1; r < this.rows - 1; r++) {
			for (int c = 1; c < this.columns - 1; c++) {
				this.gridPositions.Add (new Vector3 (c, r, 0));
			}
		}
	}

	void BoardSetup() {
		this.boardHolder = new GameObject ("Board").transform;

		for (int r = -1; r < this.rows + 1; r++) {
			for (int c = -1; c < this.columns + 1; c++) {
				GameObject toInstantiate = this.floorTiles [Random.Range (0, this.floorTiles.Length)];


				if (r < 0 || r > this.rows - 1 || c < 0 || c > this.columns - 1) {
					toInstantiate = this.outerWallTiles [Random.Range (0, this.outerWallTiles.Length)];
				}

				GameObject instance = Instantiate (toInstantiate, new Vector3 (c, r, 0), Quaternion.identity) as GameObject;
				instance.transform.SetParent (this.boardHolder);
			}
		}

	}

	Vector3 RandomPosition() {
		int randomIndex = Random.Range (0, this.gridPositions.Count);
		Vector3 randomPosition = this.gridPositions [randomIndex];
		this.gridPositions.RemoveAt (randomIndex);
		return randomPosition;
	}

	void LayoutObjectAtRandom(GameObject[] tiles, int min, int max) {
		int objectCount = Random.Range (min, max);

		for (int i = 0; i < objectCount; i++) {
			Vector3 pos = RandomPosition ();
			GameObject tileChoice = tiles[Random.Range(0, tiles.Length)];
			Instantiate (tileChoice, pos, Quaternion.identity);
		}
	
	}

	public void SetupScene(int level) {
		this.BoardSetup ();
		this.InitializeList ();
		this.LayoutObjectAtRandom (this.wallTiles, this.wallCount.minimum, this.wallCount.maximum);
		this.LayoutObjectAtRandom (this.foodTiles, this.foodCount.minimum, this.foodCount.maximum);
		int numEnemies = (int)System.Math.Log (level, 2f);
		this.LayoutObjectAtRandom (this.enemyTiles, numEnemies, numEnemies);

		Instantiate (this.exit, new Vector3 (this.columns - 1, this.rows - 1, 0), Quaternion.identity);
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour {
	public GameObject[] enemyPre;
	GameObject enemy;
	private float del=0;//生成間隔
	private int enemyFamily=0;
	Vector3 enemyPos;
	private float posZ;//-5〜3.5

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		AddEnemy ();
	}

	void AddEnemy(){
		del += Time.deltaTime;
		if (del > 2) {
			enemyFamily = Random.Range (0,3);
			posZ = Random.Range (-5,3.6f);
			enemyPos = new Vector3 (-35,0,posZ);
			enemy = (GameObject)Instantiate (enemyPre [enemyFamily],enemyPos, Quaternion.identity);
			del=0;
		}
	}
}

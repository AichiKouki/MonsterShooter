using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour {
	public GameObject[] enemyPre;
	GameObject enemy;
	private float del=0;//生成間隔
	private int ran=0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		del += Time.deltaTime;
		if (del > 1) {
			AddEnemy ();
			del = 0;
		}
	}

	void AddEnemy(){
		ran = Random.Range (0,3);
		enemy = (GameObject)Instantiate (enemyPre[ran],transform.position,Quaternion.identity);
	}
}

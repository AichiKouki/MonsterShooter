using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	//掴まれたり投げられる挙動を示すため
	PlayerController playerController;
	GameObject target;
	GameObject playerCatcher;
	public bool catched=false;
	public bool thrown=false;

	private float dif;//自分のプレイヤーの距離


	Rigidbody rigid;

	bool once=false;//Updateの中で一度だけ処理したいものがあるから

	//投げられた時の位置修正
	Vector3 thrownPos;

	private bool threw = false;//投げられたかどうかのフラグ

	//掴まれてるときはパーティクル生成
	public GameObject effectPre;
	GameObject effect;



	// Use this for initialization
	void Start () {
		rigid = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		//Start関数のタイミングでは、以下の処理をする上で、プレイヤーは始まってから生成されるので、最初から配置されてるenemyがStartで取得しようとしたら取得できないのは明確
		if (once == false) {
			target= GameObject.FindWithTag("Player");
			playerCatcher=GameObject.FindWithTag("PlayerCatcher");
			once = true;
		}

		//プレイヤーに向かって歩く処理
		dif=transform.position.x-target.transform.position.x;
		//Debug.Log (dif);
		//自分(敵)がプレイヤーから遠かったら歩く処理。
		if (dif < -0.5) {
			Walk ();
		}
		//Debug.Log (thrown);

		//掴まれた時の処理
		if (catched == true && thrown==false) {
			gameObject.transform.position = playerCatcher.transform.position;
		}
	}

	void Walk(){
		//ターゲットの方に向く処理
		if (thrown == false) {
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (target.transform.position - transform.position), 0.3f);//ターゲットの方に少しずつ向きが変わる
			transform.Translate (new Vector3 (0, 0, 2f * Time.deltaTime));
		}

	}

	//敵を掴む処理(プレイヤーから呼ばれる)
	public bool Catched(){
		Debug.Log ("プレイヤーにキャッチされてます");
		catched = true;
		//捕まれてる時にエフェクトを自分の位置に生成させる
		effect=(GameObject)Instantiate(effectPre,transform.position,Quaternion.identity);
		return catched;
	}

	//敵を投げる処理(プレイヤーから呼ばれる)
	public  bool Thrown(){
		
		if (threw==false) {//まだ投げられたなかったら処理
			Debug.Log ("投げた");
			Destroy (effect);//投げられたタイミングで、掴まれてた時のエフェクトを削除する
			thrown = true;
			thrownPos = new Vector3 (transform.position.x, 0, transform.position.z);//捕まれてる時は少し上に位置するので、投げる時は地面と同じ高さにしてからAddForceする。
			transform.position = thrownPos;
			rigid.AddForce (-500, 0, 0);//自分自身にAddForceして、投げられたように演出
			threw=true;
		}
		return thrown;
	}
}

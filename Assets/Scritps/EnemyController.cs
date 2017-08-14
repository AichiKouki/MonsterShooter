using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
	//掴まれたり投げられる挙動を示すため
	PlayerController playerController;
	GameObject target;
	GameObject playerCatcher;
	public bool catched=false;
	public bool thrown=false;

	//状態管理
	private bool dead=false;

	//音声関連
	AudioSource aud;
	public AudioClip[] se;

	//アニメーション関連
	Animator animator;

	private float dif;//自分のプレイヤーのきょい

	bool once=false;//Updateの中で一度だけ処理したいものがあるから

	//投げられた時の位置修正
	Vector3 thrownPos;

	private bool threw = false;//投げられたかどうかのフラグ

	//掴まれてるときはパーティクル生成
	public GameObject[] effectPre;
	GameObject effect;

	//時間差で自分をデストロイ
	private float del;

	//コンボを表示するため
	GameController gameController;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		aud = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//Start関数のタイミングでは、以下の処理をする上で、プレイヤーは始まってから生成されるので、最初から配置されてるenemyがStartで取得しようとしたら取得できないのは明確
		if (once == false) {
			target= GameObject.FindWithTag("Player");
			once = true;
		}
		//Debug.Log (playerCatcher.transform.position);
		//プレイヤーに向かって歩く処理
		//Debug.Log (dif);
		//自分(敵)がプレイヤーから遠かったら歩く処理。
		if (Vector3.Distance (this.transform.position,target.transform. position) >  0.5f) {
			Walk ();
		}

		//やられたら時間差で削除
		if (dead == true) {
			del += Time.deltaTime;
			if (del > 3) {
				del = 0;
				Destroy (gameObject);
			}
		}

		gameController = GameObject.Find ("GameController").GetComponent<GameController>();
	}

	//ターゲットの方に向く処理
	void Walk(){
		if (thrown == false && dead==false) {
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (target.transform.position - transform.position), 3.0f);//ターゲットの方に少しずつ向きが変わる
			transform.Translate (new Vector3 (0, 0, 2f * Time.deltaTime));
		}

	}

	//プレイヤーに投げられたら処理
	public void Catched(){
		effect = (GameObject)Instantiate (effectPre [0], transform.position, Quaternion.identity);
	}

	//プレイヤーに投げられたら
	public void Thrown(){
		thrown = true;
	}
		
	//敵と敵がぶつかった時の処理
	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Enemy") {
			if (other.gameObject.GetComponent<EnemyController> ().thrown == true) {
				dead = true;
				animator.Play ("Dead");
				gameController.ComboCountStart ();
				effect = (GameObject)Instantiate (effectPre [1], transform.position, Quaternion.identity);
				aud.PlayOneShot (se[0]);//ヒットした時効果音再生
			}
		}
	}
}

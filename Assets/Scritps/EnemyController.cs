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

	//掴まれてるときはパーティクル生成
	public GameObject[] effectPre;
	GameObject effect;

	//時間差で自分をデストロイ
	private float deleting_time;//投げれれていない敵がぶつかったら
	private float thrown_deleting_time;

	//コンボを表示するため
	GameController gameController;

	//自分が倒されたことをGeneratorに知らせる(敵を作成する数は決まっているので)
	EnemyGenerator enemyGenerator;

	//攻撃関連
	private float attack_time;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		aud = GetComponent<AudioSource> ();
		enemyGenerator = GameObject.Find ("EnemyGenerator").GetComponent<EnemyGenerator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//Start関数のタイミングでは、以下の処理をする上で、プレイヤーは始まってから生成されるので、最初から配置されてるenemyがStartで取得しようとしたら取得できないのは明確
		if (once == false) {
			target= GameObject.FindWithTag("Player");
			gameController = GameObject.Find ("GameController").GetComponent<GameController>();
			once = true;
		}

		//自分(敵)がプレイヤーから遠かったら歩く処理。
		if (Vector3.Distance (this.transform.position, target.transform.position) > 1.0f) {
			Walk ();
		} else {//プレイヤーが近かったら、攻撃を開始
			if (catched == false) {
				Do_Attack ();//雑魚キャラの攻撃処理
			}
		}

		//やられたら時間差で削除
		deleting_at_hours_difference();
	}

	//ターゲットの方に向く処理
	void Walk(){
		if (thrown == false && dead==false) {
			//transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (target.transform.position - transform.position), 3.0f);//ターゲットの方に少しずつ向きが変わる
			transform.rotation=Quaternion.Euler(0,90,0);
			transform.Translate (new Vector3 (0, 0, 2f* Time.deltaTime));
		}

	}

	//攻撃処理
	void Do_Attack(){
		attack_time += Time.deltaTime;
		if (attack_time > 3) {
			attack_time=0;
			animator.Play ("Attack");
		}
	}

	//プレイヤーに投げられたら処理(PlayerController側で呼ばれる)
	public void Catched(){
		effect = (GameObject)Instantiate (effectPre [0], transform.position, Quaternion.identity);//掴まれたことがわかるエフェクトを作成。
		catched = true;//掴まれたことを示すフラグをtrueにする。
	}

	//プレイヤーに投げられたら
	public void Thrown(){
		thrown = true;//投げられたことを示すフラグをtrueにする。
	}
		
	//敵と敵がぶつかった時の処理
	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Enemy") {
			if (other.gameObject.GetComponent<EnemyController> ().thrown == true) {//敵同士でぶつかってる時、その敵は投げられた敵か判別するための処理
				dead = true;
				animator.Play ("Dead");
				gameController.ComboCountStart ();
				effect = (GameObject)Instantiate (effectPre [1], transform.position, Quaternion.identity);
				aud.PlayOneShot (se[0]);//ヒットした時効果音再生
				enemyGenerator.fell_down_enemy_number++;//Generatorに自分が倒されたことを知らせる
			}
		}
	}

	//攻撃を受けたら、時間差で削除
	void deleting_at_hours_difference(){
		//敵と敵がぶつかった時の死亡処理
		if (dead == true) {
			deleting_time += Time.deltaTime;
			if (deleting_time > 1) {
				deleting_time = 0;
				Destroy (gameObject);
			}
		}

		//プレイヤーに投げられた時の死亡処理。
		if (thrown == true) {
			thrown_deleting_time += Time.deltaTime;
			if (thrown_deleting_time > 5) {//投げられてから、5秒後に削除する。
				thrown_deleting_time = 0;
				Destroy (gameObject);
			}
		}
	}

	//アニメーションイベント
	//攻撃を開始した時のタイミング
	void SmallKnightAttacked1(){
		gameObject.tag="Attacked_Enemy";//単純にぶつかっただけでダメージを与えないように、攻撃アニメーションが再生された時だけタグの名前を変更。
	}

	//攻撃が終わった時の処理。タグの名前を元に戻して、元の通常アニメーションに戻す。
	void SmallKnightEndAttack(){
		gameObject.tag = "Enemy";
		animator.Play ("Idle");
	}
}

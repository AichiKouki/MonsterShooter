using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	//public GameObject playerCollider;
	EnemyController enemy;
	private bool collied = false;
	public bool catched=false;
	public bool thrown=false;

	private int func;//機能切り替え

	//音声関連
	AudioSource aud;
	public AudioClip[] se;

	//下の魔法陣みたいなエフェクト生成
	public GameObject underEffectPre;
	GameObject underEffect;
	private bool spawn=false;//スポーンされたかどうか

	//敵への攻撃関連
	Rigidbody rigid;

	//Hp関連
	private int hp=10;

	void Start () {
		aud = GetComponent<AudioSource> ();
		MakeUnderEffect ();//下の魔法陣的なものを生成する処理
	}
	
	// Update is called once per frame
	void Update () { 
		//Debug.Log (hp);
		//スペースキーを押して、敵をキャッチしたり投げたりする処理
		if(Input.GetKeyDown(KeyCode.Space) && collied==true){
			//1回目のスペースキー押したら、敵を掴んで、二回目にスペースキーを押したら敵を投げる処理
			if (func == 0) {
				aud.PlayOneShot (se[0]);
				catched = true;
				enemy.Catched ();
				func++;
			}else if (func == 1) {
				thrown = true;
				aud.PlayOneShot (se[1]);
				Thrown_Button ();//投げた敵を、投げられた敵に設定
				func = 0;
			}
		}

		//敵を掴む処理
		if (catched == true) {
			enemy.transform.position = transform.position;
		}

		//敵を投げる処理
		if (catched == true && thrown == true) {
			catched = false;
			thrown = false;
			rigid.AddForce (-500, 0, 0);//自分自身にAddForceして、投げられたように演出
		}

		//エフェクトをプレイヤーに追従させる処理
		if(spawn==true){
			underEffect.transform.position=transform.position;
		}
		
	}

	//PlayerCatcherオブジェクトにもアタッチしているので、PlayerColliderオブジェクトだけに処理をさせたいので
	void MakeUnderEffect(){
		if (gameObject.tag == "PlayerCollider") {
			underEffect = (GameObject)Instantiate (underEffectPre, transform.position, Quaternion.identity);
			underEffect.transform.rotation = Quaternion.Euler (-90,0,0);
			spawn = true;
		}
	}

	//子要素のオブジェクトのOnTriggerを取得している
	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Enemy") {
			collied = true;
			//Debug.Log ("敵にぶつかりました");
			enemy = other.gameObject.GetComponent<EnemyController> ();
			rigid = other.gameObject.GetComponent<Rigidbody> ();
			//catched = enemy.Catched ();
			//Debug.Log (catched);
		} 

		if (other.gameObject.tag == "Attacked_Enemy") {
			Debug.Log ("敵の攻撃を受けた");
			hp--;
		}
	}

	//敵を投げる処理
	void Thrown_Button(){
		collied = false;
		enemy.Thrown ();
	}
}

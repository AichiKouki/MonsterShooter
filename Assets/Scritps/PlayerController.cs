using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	//public GameObject playerCollider;
	Enemy enemy;
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

	void Start () {
		aud = GetComponent<AudioSource> ();
		MakeUnderEffect ();//下の魔法陣的なものを生成する処理
	}
	
	// Update is called once per frame
	void Update () { 

		//スペースキーを押して、敵をキャッチしたり投げたりする処理
		if(Input.GetKeyDown(KeyCode.Space) && collied==true){
			//1回目のスペースキー押したら、敵を掴んで、二回目にスペースキーを押したら敵を投げる処理
			if (func == 0) {
				Debug.Log ("スペースキー");
				aud.PlayOneShot (se[0]);
				catched = enemy.Catched ();
				Debug.Log (catched);
				func++;
			}else if (func == 1) {
				Debug.Log ("Gキー");
				Thrown_Button ();
				func = 0;
			}
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

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Enemy") {
			collied = true;
			Debug.Log ("敵にぶつかりました");
			enemy = other.gameObject.GetComponent<Enemy> ();
			//catched = enemy.Catched ();
			//Debug.Log (catched);
		}
	}

	//敵を投げる処理
	void Thrown_Button(){
		collied = false;
		enemy.Thrown ();
	}
}

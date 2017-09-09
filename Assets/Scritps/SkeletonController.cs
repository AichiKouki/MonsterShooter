using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//骸骨ボスのコントローラースクリプト
public class SkeletonController : MonoBehaviour {

	//移動処理関連
	GameObject target;//ユーザーがそうさするプレイヤー

	//攻撃処理関連
	private float attack_time;//攻撃のインターバル関連処理
	private bool attacked = false;//攻撃をしたかどうかのフラグ

	void Start () {
		target = GameObject.FindWithTag ("Player");//ユーザーがそうさするプレイヤーを取得する
	}

	void FixedUpdate () {
		//プレイヤーに向き続ける処理
		LookAtPlayer();

		//攻撃処理関連
		Attack();
	}

	//ユーザーに向き続ける処理
	void LookAtPlayer(){
		//スケルトンは、プレイヤーの方向に向き続ける処理。
		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (target.transform.position - transform.position), 3.0f);//ターゲットの方に少しずつ向きが変わる
	}

	//ボスの攻撃処理
	void Attack(){
		//攻撃処理関連
		attack_time+=Time.deltaTime;//攻撃のインターバル
		if (attack_time > 5) {
			attack_time = 0;//攻撃のインターバルをリセット
		}

	}
}

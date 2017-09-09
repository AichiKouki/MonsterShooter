using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour {
	//適を作成関連
	public GameObject[] enemyPre;
	GameObject enemy;
	private float del=0;//生成間隔
	private int enemyFamily=0;//敵の種類
	Vector3 enemyPos;//敵を作成する場所
	private float posZ;//-5〜3.5
	private int enemy_number=0;//雑魚キャラの数設定
	public int fell_down_enemy_number=0;//雑魚キャラが倒された数
	private int spawn_enemy_count=0;//生成した回数。1回目は一体。2回目はにたい、3回目は3体と生成することにとって、ボーリングのピンのように三角形の形になる。

	//ボス関連
	public GameObject[] effectPre;//複数のエフェクトを利用するので、配列としてエフェクトを格納する。
	GameObject effect;//1個目のエフェクト
	Vector3 effectPos;//2個目のエフェクト
	public GameObject bossPre;//ボスのプレファブ
	GameObject boss;
	Vector3 bossPos;
	private bool bossOnce = false;//ボスの作成処理が、Updateの中にあるので、一度だけの処理のために用意
	private bool bossDead = false;//ボスが倒されたかどうかを判定するために使う

	void Start () {
		
	}

	void FixedUpdate () {
		AddEnemy ();//敵を作成するための関数。
	}

	//雑魚キャラを生成する処理
	void AddEnemy(){
		enemyFamily = 0;//雑魚キャラの種類を設定。今は固定しているが、ランダムに設定してもいい。
		if (bossDead==false) {//雑魚キャラが倒された数が50より小さかったら雑魚キャラを生成
			del += Time.deltaTime;
			//二秒後ごとに的を作成。
			if (del > 2) {
				if (spawn_enemy_count == 0) {//1回目の生成
					enemyPos = new Vector3 (-10, 0, -1);//真ん中に生成
					enemy = (GameObject)Instantiate (enemyPre [enemyFamily], enemyPos, Quaternion.identity);

					spawn_enemy_count++;
					enemy_number+=1;//生成した敵の数
					del = 0;//インターバルリセット処理
				} else if (spawn_enemy_count==1) {
					enemyPos = new Vector3 (-10, 0, -2);//左側
					enemy = (GameObject)Instantiate (enemyPre [enemyFamily], enemyPos, Quaternion.identity);

					enemyPos = new Vector3 (-10, 0, 0);//右側に生成
					enemy = (GameObject)Instantiate (enemyPre [enemyFamily], enemyPos, Quaternion.identity);

					spawn_enemy_count++;
					enemy_number+=2;//生成した敵の数
					del = 0;//インターバルリセット処理
				} else {
					enemyPos = new Vector3 (-10, 0, -2);//左側
					enemy = (GameObject)Instantiate (enemyPre [enemyFamily], enemyPos, Quaternion.identity);

					enemyPos = new Vector3 (-10, 0, -1);//真ん中に生成
					enemy = (GameObject)Instantiate (enemyPre [enemyFamily], enemyPos, Quaternion.identity);

					enemyPos = new Vector3 (-10, 0, 0);//右側に生成
					enemy = (GameObject)Instantiate (enemyPre [enemyFamily], enemyPos, Quaternion.identity);

					spawn_enemy_count=0;//次の処理から、真ん中だけ敵を生成したいので、リセット。
					enemy_number+=3;//生成した敵の数
					del = 0;//インターバルリセット処理
				}
			}
		} 

		//ボスキャラを生成する条件を満たしたら、ボスを生成する。
		if(fell_down_enemy_number==10){//ボスキャラ生成
			if (bossOnce == false) {//Updateの中で処理をしているので、一度だけ処理をする。
				StartCoroutine ("Creation_boss");
				bossOnce = true;
			}
		}
	}

	//コルーチンで実際にボスを生成する処理
	IEnumerator Creation_boss(){
		//一回目の繰り返しではエフェクト、2回目の繰り返しでは別のエフェクト、3回目のエフェクトでは実際にスケルトンを作成している。
		for(int i=0;i<3;i++){
		effectPos=new Vector3(-8,3,0);//エフェクtの位置を初期化
			bossPos = new Vector3 (-8,0,0);//ボスの作成位置を初期化
			if(i<3) effect=(GameObject)Instantiate(effectPre[i],effectPos,Quaternion.identity);//二回はエフェクトを作成
			if (i == 2) boss = (GameObject)Instantiate (bossPre,bossPos,Quaternion.identity);//繰り返しの三回目でボスを作成

			yield return new WaitForSeconds(1.5f);//1.5秒ずつ処理する
			Destroy (effect);//次の処理にいく前に、エフェクトを削除する。
		}
	}
}

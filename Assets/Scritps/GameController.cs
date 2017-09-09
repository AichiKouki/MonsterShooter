using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameController : MonoBehaviour {
	public Text comboLabel;
	public int comboCount = 0;
	private float del;//コンボしなくなった時間を計算。一定時間コンボしなくなったらコンボカウントリセットとかする
	private bool comboStart=false;//コンボが始まったかどうか

	//フォントのフェードイン、フェードアウト処理
	private float blackValue;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		fade_in_and_fade_out ();
		comboLabel.text=comboCount+"コンボ！";
	}

	//フェードインとフェードアウト処理
	void fade_in_and_fade_out(){
		comboLabel.color=new Color(255.0f/255,146.0f/255,8.0f/255,blackValue/255);
		if (comboStart == true) {
			//Debug.Log ("フェードイン処理");
			del += Time.deltaTime;
			if (blackValue < 255) blackValue += 8.0f;//フェードイン処理。255までと制限するのは、無限に増えていくので、あとで減らそうとしても間に合わなくなるから
			if (del > 5) {//コンボを5秒間続かない時間を作ってしまうとコンボリセット
				del = 0;
				comboStart = false;
				comboCount = 0;
			}
		} else {
			if(blackValue>0)blackValue -= 5.0f;//0までの制限しているのは、無限に減っていくので、あとで減らそうとしても間に合わなくなるから
		}
	}

	//コンボし始めたことを知らせる関数
	public void ComboCountStart(){
		this.comboStart = true;
		this.comboCount++;
		this.del = 0;//コンボをするたびに、コンボリセットのための時間を0に戻す
		StartCoroutine("LabelChange");
	}

	//コンボラベルの一瞬大きくなる演出
	IEnumerator LabelChange(){
		for (int i = 0; i < 2; i++) {
			if(i==0) comboLabel.fontSize = 40; 
			else comboLabel.fontSize = 30;
			yield return new WaitForSeconds (0.1f);
		}
	}
}

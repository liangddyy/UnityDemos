/// <summary>
/// Item
/// this script use for control effect item(ex. duration item,effect item)
/// </summary>

using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {
	
	public float scoreAdd; //add money if item = coin
	public int decreaseLife; //decrease life if item = obstacle 
	public int itemID; //item id
	public float duration; // duration item
	public float itemEffectValue; // effect value(if item star = speed , if item multiply = multiply number)
	public ItemRotate itemRotate; // rotate item
	public GameObject effectHit; // effect when hit item
	
	[HideInInspector]
	public bool itemActive;
	
	public enum TypeItem{
		Null, Coin, Obstacle, Obstacle_Roll, ItemJump, ItemSprint, ItemMagnet, ItemMultiply
	}
	
	public TypeItem typeItem;
	
	[HideInInspector]
	public bool useAbsorb = false;
	
	public static Item instance;
	
	void Start(){
		instance = this;	
	}
	
	//Set item effect
	public void ItemGet(){
		if(GameAttribute.gameAttribute.deleyDetect == false){
			if(typeItem == TypeItem.Coin){
				HitCoin();
				//Play sfx when get coin
				SoundManager.instance.PlayingSound("GetCoin");
			}else if(typeItem == TypeItem.Obstacle){
				HitObstacle();
				//Play sfx when get hit
				SoundManager.instance.PlayingSound("HitOBJ");
			}else if(typeItem == TypeItem.Obstacle_Roll){
				if(Controller.instace.isRoll == false){
					HitObstacle();
					//Play sfx when get hit
					SoundManager.instance.PlayingSound("HitOBJ");
				}
			}else if(typeItem == TypeItem.ItemSprint){
				Controller.instace.Sprint(itemEffectValue,duration);
				//Play sfx when get item
				SoundManager.instance.PlayingSound("GetItem");
				HideObj();
				initEffect(effectHit);
			}else if(typeItem == TypeItem.ItemMagnet){
				Controller.instace.Magnet(duration);
				//Play sfx when get item
				SoundManager.instance.PlayingSound("GetItem");
				HideObj();
				initEffect(effectHit);
			}else if(typeItem == TypeItem.ItemJump){
				Controller.instace.JumpDouble(duration);
				//Play sfx when get item
				SoundManager.instance.PlayingSound("GetItem");
				HideObj();
				initEffect(effectHit);
			}else if(typeItem == TypeItem.ItemMultiply){
				Controller.instace.Multiply(duration);
				GameAttribute.gameAttribute.multiplyValue = itemEffectValue;
				//Play sfx when get item
				SoundManager.instance.PlayingSound("GetItem");
				HideObj();
				initEffect(effectHit);
			}
		}
	}
	
	//Coin method
	private void HitCoin(){
		if(Controller.instace.isMultiply == false){
			GameAttribute.gameAttribute.coin += scoreAdd;
		}else{
			GameAttribute.gameAttribute.coin += (scoreAdd)*GameAttribute.gameAttribute.multiplyValue;
		}
		initEffect(effectHit);
		HideObj();
	}
	
	//Obstacle method
	private void HitObstacle(){
		if(GameAttribute.gameAttribute.ageless == false){
			if(Controller.instace.timeSprint <= 0){
				GameAttribute.gameAttribute.life -= decreaseLife;
				GameAttribute.gameAttribute.ActiveShakeCamera();
			}else{
				HideObj();
				GameAttribute.gameAttribute.ActiveShakeCamera();
			}
			
		}
	}
	
	//Spawn effect method
	private void initEffect(GameObject prefab){
		GameObject go = (GameObject) Instantiate(prefab, Controller.instace.transform.position, Quaternion.identity);
		go.transform.parent = Controller.instace.transform;
		go.transform.localPosition = new Vector3(go.transform.localPosition.x, go.transform.localPosition.y+0.5f, go.transform.localPosition.z);	
	}
	
	//Magnet method
	public IEnumerator UseAbsorb(GameObject targetObj){
		bool isLoop = true;
		useAbsorb = true;
		while(isLoop){
			this.transform.position = Vector3.Lerp(this.transform.position, targetObj.transform.position, GameAttribute.gameAttribute.speed*2f * Time.smoothDeltaTime);
			if(Vector3.Distance(this.transform.position, targetObj.transform.position) < 0.6f){
				isLoop = false;	
				SoundManager.instance.PlayingSound("GetCoin");
				HitCoin();
			}
			yield return 0;
		}
		Reset();
		StopCoroutine("UseAbsorb");
		yield return 0;
	}
	
	public void HideObj(){
		if(useAbsorb == false){
			this.transform.parent = null;
			this.transform.localPosition = new Vector3(-100,-100,-100);
		}
	}
	
	public void Reset(){
		itemActive = false;
		this.transform.position = new Vector3(-100,-100,-100);
		this.transform.parent = null;
		useAbsorb = false;
	}
}

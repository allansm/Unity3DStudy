using UnityEngine;
using System.Collections;

public class EnemyLife : BaseLife {
	public GameObject poof;
	public GameObject enemy;
	// Use this for initialization
	void Start () {
		base.Start();
	}
	
	// Update is called once per frame
	void Update () {
		if(lastHp != null && lastHp != getCurrentHp()){
			StartCoroutine(slowReceiveDamage());
		}
		lastHp = getCurrentHp();
		try{
			base.Update();
			death();
		}catch(System.Exception e){
			
		}
	}
	void death(){
		if(getCurrentHp() <=0 && this.gameObject.GetComponent<EnemyBehaviour>().canDestroy){
			GameObject o = Instantiate(poof,new Vector3(transform.position.x,transform.position.y+5,transform.position.z),poof.transform.rotation) as GameObject;
			o.SetActive(true);
			Destroy(this.gameObject.GetComponent<EnemyBehaviour>());
			enemy.SetActive(false);
			StartCoroutine(startDestroy());
		}
	}
	IEnumerator startDestroy(){
		yield return new WaitForSeconds(5f);
		Destroy(this.gameObject);
	}
	void slowMotion(){
		Time.timeScale = 0.1f;
		Time.fixedDeltaTime = 0.02f * Time.timeScale;
	}
	void normalMotion(){
		Time.timeScale = 1f;
		Time.fixedDeltaTime = 0.02f * Time.timeScale;
	}
	float lastHp;
	IEnumerator slowReceiveDamage(){
		slowMotion();
		yield return new WaitForSeconds(0.05f);
		normalMotion();
	}
}

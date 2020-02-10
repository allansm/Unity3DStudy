using UnityEngine;
using System.Collections;

public abstract class BaseLife : MonoBehaviour {
	private float currentHp;
	public float maxHp;
	public bool invincible;
	// Use this for initialization
	protected void Start () {
		currentHp = maxHp;
	}
	
	// Update is called once per frame
	protected void Update () {
	
	}
	public void applyDamage(float damage){
		if(!invincible){
			this.currentHp-= damage;
		}
	}
	public float getCurrentHp(){
		return this.currentHp;
	}
}

using UnityEngine;
using System.Collections;

public class CharacterLife : BaseLife {

	// Use this for initialization
	void Start () {
		base.Start();
	}
	
	// Update is called once per frame
	void Update () {
		base.Update();
	}
	public void death(Animator ani){
		if(getCurrentHp() <=0){
			//Destroy(this.gameObject);
			ani.SetBool("death",true);
		}
	}
}

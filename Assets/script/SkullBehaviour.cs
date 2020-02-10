using UnityEngine;
using System.Collections;

public class SkullBehaviour : EnemyBehaviour {
	public bool blocking;
	public bool blocked;
	public float countAttack = 0;
	public GameObject blockParticle;
    public AudioClip idleAudio;
    public AudioClip blockAudio;
	// Update is called once per frame
	void Start(){
		base.Start();
	}
	IEnumerator debug(){
		yield return new WaitForSeconds(0f);
		print("debugging..");
		if(ani.GetBool("attack") && ani.GetBool("blocking")){
			print("atkblock");
		}
		if(ani.GetBool("running") && ani.GetBool("blocking")){
			print("runblock");
		}
		if(ani.GetBool("attack") && def == 100){
			print("atkdef100");
		}
		if(ani.GetBool("running") && blocking){
			print("runblock");
		}
		if(ani.GetBool("running") && def == 100){
			print("rundef100");
		}
		if(ani.GetBool("attack") && blocking){
			print("attackblock");
		}
	}
	void Update () {
		//StartCoroutine(debug());
		if(!ani.GetBool("attack")){
			StartCoroutine(startBlocking());
		}
		if(!blocking && !ani.GetBool("blocking")){
			base.Update();
		}
		if(ani.GetBool("attack")){
			countAttack += 1 *Time.deltaTime;
		}
		if(countAttack >= 0.01f){
			blocked = false;
			countAttack = 0;
		}
	}
	bool flag = true;
	//start block at certain distance 
	IEnumerator startBlocking(){
		yield return new WaitForSeconds(0.1f);
		if(getDistance() < 10 && !blocked){
			//canMove = false;
			blocking = true;
			def = 100;
			ani.SetBool("blocking",true);
			ani.SetBool("running",false);
			lookAtTarget(player);
			if(flag){
				StartCoroutine(startAttack());
				flag = false;
			}
			StartCoroutine(stopBlocking());
		}
		if(getDistance() > 10 && !blocked){
			blocking = false;
			//canMove = true;
			def = 0;
			ani.SetBool("blocking",false);
		}
	}
	//interrupt block after receive attack
	IEnumerator stopBlocking(){
		if(ani.GetBool("hit") && ani.GetBool("blocking")){
			blockParticle.SetActive(true);
            AudioSource src = GetComponent<AudioSource>();
            src.clip = blockAudio;
            src.Play();
			yield return new WaitForSeconds(0.5f);
            src.clip = idleAudio;
            src.Play();
			blocked = true;
			blocking = false;
			def = 0;
			ani.SetBool("blocking",false);
			blockParticle.SetActive(false);
			//flag = true;
		}
	}
	//interrupt block after 2 sec
	IEnumerator startAttack(){
		yield return new WaitForSeconds(2f);
		blocked = true;
		blocking = false;
		def = 0;
		ani.SetBool("blocking",false);
		flag = true;
	}
	float getDistance(){
		return Vector3.Distance(transform.position,player.position);
	}
}
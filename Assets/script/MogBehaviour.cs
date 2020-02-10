using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MogBehaviour : EnemyBehaviour{
	private bool stopAll;
    public GameObject attackArea;
    // Start is called before the first frame update
    void Start(){
        base.Start();
    }

    // Update is called once per frame
    void Update(){
		if(!stopAll && canMove){
			lookAtTarget(player);
			follow();
		}else{
            ani.SetBool("running",false);
        }
		//attack();
		mogAttack();
		debugNavMesh();
    }
	void mogAttack(){
		cooldown += 1*Time.deltaTime;
		if(Vector3.Distance(player.position,transform.position) <=20 && cooldown >5){
			cooldown = 0;
			lookAtTarget(player);
			ani.SetTrigger("attack");
			StartCoroutine(impulse());
			//if(target != null){
				//StartCoroutine(target.gameObject.GetComponent<CharacterBehaviour>().doDamage(transform));
                //StartCoroutine(target.gameObject.GetComponent<PlayerInput>().doDamage(transform));
				//push(player);
				//StartCoroutine(pushAfter());
			//}
			//push(player);
		}
	}
	IEnumerator impulse(){
		stopAll = true;
		yield return new WaitForSeconds(0.5f);
        attackArea.SetActive(true);
		gameObject.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0,0,5000f));
		yield return new WaitForSeconds(2f);
		stopAll = false;
        attackArea.SetActive(false);
	}
}

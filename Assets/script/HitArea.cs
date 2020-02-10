using UnityEngine;
using System.Collections;

public class HitArea : AI {
    public bool player;
    public bool enemy;
    public bool hit;
    public bool hideOnCollide;
    public float time;
    public bool destroyAfter;
    public string imuneEnemy;
    public bool canPush;
    public float pushForce;
    
    void Update(){
        if(destroyAfter){
            StartCoroutine(destroyAfterTime());
        }
    }
    
    void OnTriggerEnter(Collider c){
        if(hit){
            if(!c.isTrigger){
                if(c.gameObject.tag == "enemy" && enemy){
                    if(imuneEnemy != c.gameObject.name){
                        StartCoroutine(c.gameObject.GetComponent<EnemyBehaviour>().doDamage());
                        StartCoroutine(hide());
                        if(canPush){
                            push(c.gameObject.transform);
                        }
                    }
                }
                if(c.gameObject.tag == "Player" && player){
                    //StartCoroutine(c.gameObject.GetComponent<PlayerBehaviour>().pIn.doDamage(transform));
                    StartCoroutine(c.gameObject.GetComponent<PlayerBehaviour>().pIn.doAreaDamage());
                    StartCoroutine(hide());
                    if(canPush){
                        push(c.gameObject.transform,pushForce);
                    }
                }
            }
        }
	}
    IEnumerator hide(){
        yield return new WaitForSeconds(0.5f);
        if(hideOnCollide){
            this.gameObject.SetActive(false);
        }
    }
    IEnumerator destroyAfterTime(){
        yield return new WaitForSeconds(time);
        Destroy(transform.root.gameObject);
    }
    public IEnumerator stopHitAt(float time){
        yield return new WaitForSeconds(time);
        hit = false;
    }
	void OnTriggerExit(Collider collision){
		
	}
}

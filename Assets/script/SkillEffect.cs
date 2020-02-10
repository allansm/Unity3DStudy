using UnityEngine;
using System.Collections;

public class SkillEffect : AI {
	public GameObject fireball;
	public GameObject orb;
	public GameObject explosion;
	public bool canMove;
    public GameObject healPart;
    public GameObject player;
    public bool canHeal;
    public bool executeHeal;
    public bool executeMagic;
    public CameraShake cs;
    public GameObject attack;
    public GameObject attack1;
    public GameObject attack2;
	//public GameObject explosion;
	float vel = 2f;
    public Transform target;
    public float dist;
    bool release = true;
    bool release2 = true;
    void Start(){

    }
    void Update(){
        //StartCoroutine(healPosChange());
    }
    public void healPosChange(){
        //yield return new WaitForSeconds(0f);
        healPart.transform.position = new Vector3(player.transform.position.x,player.transform.position.y-2f,player.transform.position.z);
    }
    public IEnumerator attackEffect(Animator ani){
        if(ani.GetCurrentAnimatorStateInfo(0).IsName("attack1") && release){
            release = false;
            attack.transform.position = player.transform.position;
            attack.transform.rotation = player.transform.rotation;
            attack1.SetActive(true);
            yield return new WaitForSeconds(1f);
            attack1.SetActive(false);
            release = true;
        }
        else if(ani.GetCurrentAnimatorStateInfo(0).IsName("attack2") && release2){
            release2 = false;
            attack.transform.position = player.transform.position;
            attack.transform.rotation = player.transform.rotation;
            attack2.SetActive(true);
            yield return new WaitForSeconds(1f);
            attack2.SetActive(false);
            release2 = true;
        }
    }
    float lastPos = 0;
    bool canVerifyExplosion;
	public void fireBall(){
		if(/*Input.GetKeyDown("i")*/ executeMagic){
            lastPos = 0;
            canVerifyExplosion = true;
            float force = 0;
            executeMagic = false;
			canMove = true;
			fireball.SetActive(true);
			vel = 2f;
			if(explosion != null){
				//Destroy(explosion);
			}
            StartCoroutine(stopOrbMovement());
            try{
                target = orb.gameObject.GetComponent<OrbBehaviour>().target.transform;
                force = dist*19;
            }catch(System.Exception e){
                force = 1000; 
            }
			fireball.transform.position = new Vector3(orb.transform.position.x,orb.transform.position.y+5,orb.transform.position.z);
            if(target != null){
                orb.gameObject.GetComponent<OrbBehaviour>().lookAtTarget(target);
                player.gameObject.GetComponent<PlayerBehaviour>().pIn.lookAtTarget(target);
                dist = Vector3.Distance(orb.transform.position,target.position);
            }else{
                orb.transform.rotation = player.transform.rotation;
            }
            fireball.transform.rotation = orb.transform.rotation;
			explosion.SetActive(false);
            print("force:"+force);
			fireball.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0,0,vel*force));
		}
		if(fireball.transform.position.y == lastPos && canVerifyExplosion){
            StartCoroutine(shake());
            StartCoroutine(explode());
            print("boom");
            lastPos = 0;
            canVerifyExplosion = false;
		}
        lastPos = fireball.transform.position.y;
    }
    IEnumerator stopOrbMovement(){
        yield return new WaitForSeconds(0.01f);
        orb.gameObject.GetComponent<OrbBehaviour>().stop = true;
        yield return new WaitForSeconds(1f);
        orb.gameObject.GetComponent<OrbBehaviour>().stop = false;
    }
	public void fireBallPos(){
		if(!canMove){
			fireball.SetActive(false);
			fireball.transform.position = new Vector3(orb.transform.position.x,orb.transform.position.y+4,orb.transform.position.z);
		}
	}
	IEnumerator explode(){
		explosion.SetActive(true);
		yield return new WaitForSeconds(1f);
        fireball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //orb.gameObject.GetComponent<OrbBehaviour>().stop = false;
		canMove = false;
        fireball.SetActive(false);
        explosion.SetActive(false);
	}
    IEnumerator shake(){
        cs.shaking = true;
        yield return new WaitForSeconds(0);
    }
	public void heal(){
		if(/*Input.GetKeyDown("space")*/executeHeal && canHeal){
            executeHeal = false;
            canHeal = false;
			//healPart.transform.position = new Vector3(player.transform.position.x,player.transform.position.y-2f,player.transform.position.z);
            StartCoroutine(healStart());
		}
	}
    IEnumerator healStart(){
        yield return new WaitForSeconds(0.5f);
        healPart.SetActive(true);
        yield return new WaitForSeconds(1f);
        healPart.SetActive(false);
    }
	void OnCollisionEnter(Collision c){
        //print(c.gameObject.name);
        //StartCoroutine(shake());
        //StartCoroutine(explode());
    }
}
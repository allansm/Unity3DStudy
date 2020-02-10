using UnityEngine;
using System.Collections;

public abstract class PlayerActions : BasePlayer { 
    public Animator ani;
	public float velocity;
	public float x;
	public float y;
	public bool transformation = false;
	public Collider target;
	public GameObject charac;
	public GameObject transf;
	public bool canMove = true;
	public float cooldown;
    public bool canChange;
    public float hpLoss;
    public float hpRecover;
    public float mpLoss;
    public float currentHp;
    public float currentMp;
	public Rigidbody rigidbody;
    IEnumerator pushAfter(Collider target){
		yield return new WaitForSeconds(0.5f);
		if(target != null){
			push(target.transform);
		}
	}
    public void attack(){
        if(cooldown > 0.5f){
            canMove = false;
            cooldown = 0;
            ani.SetTrigger("attack");
            
            if(target != null){
                try{
                    lookAtTarget(target.transform);
                    StartCoroutine(target.GetComponent<EnemyBehaviour>().doDamage());
                    StartCoroutine(pushAfter(target));
                }catch(System.Exception e){

                }
            }
        }
	}
    IEnumerator evasion(){
        //if(cooldown > 1){
		  yield return new WaitForSeconds(0.2f);
		  rigidbody.AddRelativeForce(new Vector3(0,0.2f*800f,Vector3.forward.z*-700f));
        //}
	}
    public void evade(){
        if(cooldown > 1){
            cooldown = 0;
            ani.SetTrigger("evade");
            StartCoroutine(evasion());
        }
	}
    public void heal(float currentMp,SkillEffect skill){
        if(cooldown > 1 && currentMp >= 25){
            skill.executeHeal = true;
            canMove = false;
            cooldown = 0;
            ani.SetTrigger("heal");
            hpRecover += 25;
            mpLoss += 25;
        }
	}
    public void magic(SkillEffect skill){
        if(cooldown > 1 && currentMp >= 33.3f){
            canMove = false;
            cooldown = 0;
            ani.SetTrigger("magic");
            mpLoss = 33.3f;
            //skill.executeMagic = true;
            StartCoroutine(waitToExecuteMagic(skill));
        }
    }
    IEnumerator waitToExecuteMagic(SkillEffect skill){
        yield return new WaitForSeconds(0.5f);
        skill.executeMagic = true;
    }
    public void changeForm(){
		if(canChange){
			if(transformation){
				transf.SetActive(true);
				charac.SetActive(false);
				ani = transf.GetComponent<Animator>();
			}else{
				transf.SetActive(false);
				charac.SetActive(true);
				ani = charac.GetComponent<Animator>();
			}
		}
        if(transformation){
            hpLoss += (5f*(currentHp/100))*Time.deltaTime;
        }
	}
    public void change(){
        if(cooldown > 1){
            if(!transformation){
                transformation = true;
            }
            else{
                transformation = false;
            }
        }
    }
    public IEnumerator doDamage(Transform direction){
		yield return new WaitForSeconds(0.5f);
		canMove = false;
		lookAtTarget(direction);
		ani.SetBool("running",false);
		ani.ResetTrigger("attack");
		ani.SetTrigger("hit");
		//characterlife.applyDamage(11.1f);
        hpLoss += 11.1f;
		StartCoroutine(returnMovement(0.5f));
	}
    public IEnumerator doAreaDamage(){
		yield return new WaitForSeconds(0f);
		canMove = false;
		//lookAtTarget(direction);
		ani.SetBool("running",false);
		ani.ResetTrigger("attack");
		ani.SetTrigger("hit");
		//characterlife.applyDamage(11.1f);
        hpLoss += 11.1f;
		StartCoroutine(returnMovement(0.5f));
	}
	public void cancelAnimations(){
		ani.SetBool("running",false);
		ani.ResetTrigger("hit");
		//ani.ResetTrigger("attack");
	}
	public void stopAction(){
		cancelAnimations();
		x = 0;
		y = 0;
	}
    bool canReset = true;
    public void reset(){
        if(!canMove && canReset){
			stopAction();
			StartCoroutine(returnMovement(1f));
		}
    }
	public IEnumerator returnMovement(float time){
        canReset = false;
        print("reset");
		yield return new WaitForSeconds(time);
		canMove = true;
        canReset = true;
	}
    public bool death(float currentHp){
		if(currentHp <=0){
			//Destroy(this.gameObject);
			ani.SetBool("death",true);
            return true;
		}
        return false;
	}
}

using UnityEngine;
using System.Collections;

public class CharacterBehaviour : AI {
	
	public Animator ani;
	public float velocity;
	private float x;
	private float y;
	public bool transformation = false;
	private Collider target;
	public GameObject charac;
	public GameObject transf;
	private bool canMove = true;
	private float cooldown;
	public CharacterLife characterlife;
	public GameObject poof;
	private Rigidbody rigidbody;
    public SkillEffect skillEffect;
	// Use this for initialization
	void Start () {
		velocity = 10;
		transf.SetActive(false);
		rigidbody = gameObject.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        runEffects();
        
		evade();
		heal();
		test2();
		change();
		attack();
		if(canMove && !ani.GetBool("attack") && !ani.GetBool("hit")){
			move();
		}
		if(!canMove){
			stopAction();
			StartCoroutine(returnMovement(1f));
		}
		characterlife.death(ani);
	}
	bool release;
	void move(){
		release = y==0?true:false;
		ani.SetBool("running",false);
		y = Input.GetAxis("Vertical");
		x = Input.GetAxis("Horizontal");
		if(y > 0 && canMove){
			y = y * velocity * Time.deltaTime;
			transform.Translate(0,0,y);
			ani.SetBool("running",true);
		}
		if(y < 0 && canMove){
			y = y * velocity * Time.deltaTime;
			if(release){
				transform.Rotate(0,180,0);
				release = false;
			}
			transform.Translate(0,0,y*-1);
			ani.SetBool("running",true);
		}
		if(x != 0 && canMove){
			x*= velocity*10 * Time.deltaTime;
			if(y == 0){
				transform.Translate(0,0,Vector3.forward.z*velocity*Time.deltaTime);
			}
			transform.Rotate(0,x,0);
			ani.SetBool("running",true);
		}
	}
	void attack(){
		cooldown++;
		if(Input.GetKeyDown("j") && transformation && cooldown > 7 && !ani.GetBool("hit")){
			canMove = false;
			cooldown = 0;
			ani.SetTrigger("attack");
			if(target != null){
				try{
					lookAtTarget(target.transform);
					StartCoroutine(target.GetComponent<EnemyBehaviour>().doDamage());
					StartCoroutine(pushAfter());
				}catch(System.Exception e){
					
				}
			}
		}
	}
	IEnumerator pushAfter(){
		yield return new WaitForSeconds(0.5f);
		if(target != null){
			push(target.transform);
		}
	}
	void evade(){
		if(Input.GetKeyDown("space")  && transformation && cooldown > 20){
			cooldown = 0;
			ani.SetTrigger("evade");
			StartCoroutine(evasion());
		}
	}
	void heal(){
		if(Input.GetKeyDown("space")  && !transformation && cooldown > 100){
            cooldown = 0;
			ani.SetTrigger("heal");
		}
	}
	bool tes = false;
	void test2(){
		if(Input.GetKeyDown("l")){
			lookAtTarget(GameObject.FindWithTag("enemy").transform);
		}
	}
	IEnumerator evasion(){
		yield return new WaitForSeconds(0.2f);
		//transform.Translate(0,0,-0.5f);
		rigidbody.AddRelativeForce(new Vector3(0,0.2f*800f,Vector3.forward.z*-700f));
		//GetComponent<Rigidbody>().rotation = transform.rotation;
	}
	void change(){
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
			if(Input.GetKeyDown("x") && !transformation){
				poof.SetActive(true);
				transformation = true;
				StartCoroutine(poofTime());
			}
			else if(Input.GetKeyDown("x") && transformation){
				poof.SetActive(true);
				transformation = false;
				StartCoroutine(poofTime());
			}
		}
	}
	public bool canChange = true;
	IEnumerator poofTime(){
		yield return new WaitForSeconds(0.01f);
		canChange = false;
		yield return new WaitForSeconds(3f);
		poof.SetActive(false);
		canChange = true;
	}
	public IEnumerator doDamage(Transform direction){
		yield return new WaitForSeconds(0.5f);
		canMove = false;
		lookAtTarget(direction);
		ani.SetBool("running",false);
		ani.ResetTrigger("attack");
		ani.SetTrigger("hit");
		characterlife.applyDamage(11.1f);
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
	public IEnumerator returnMovement(float time){
		yield return new WaitForSeconds(time);
		canMove = true;
	}
    void runEffects(){
        skillEffect.fireBallPos();
		skillEffect.fireBall();
        if(!transformation){
            if(cooldown > 100){
                skillEffect.canHeal = true;
            }
            if(cooldown < 100){
                skillEffect.canHeal = false;
            }
            skillEffect.heal();
        }
    }
	void OnTriggerStay(Collider c){
		if(c.gameObject.tag == "enemy"){
			target = c;
		}
	}
	void OnTriggerExit(Collider collision){
		target = null;
	}
}
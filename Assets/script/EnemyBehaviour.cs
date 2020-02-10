using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public abstract class EnemyBehaviour : AI {
	public Transform player;
	public float velocity;
	public Animator ani;
	public Collider target;
	public EnemyLife enemylife;
	public float cooldown = 0;
	public float def = 0;
	public Material mat;
	public bool canDestroy = true;
	public bool canMove = true;
	public NavMeshAgent nav;
	public GameObject hitEffect;
	public Vector3 relativeHitPos;
	IEnumerator test(){
		while(true){
			if(GetComponent<Rigidbody>().isKinematic){
				GetComponent<Rigidbody>().isKinematic = false;
				print("disable kinematic");
			}else{
				GetComponent<Rigidbody>().isKinematic = true;
				print("enable kinematic");
			}
			yield return new WaitForSeconds(1f);
		}
	}
	protected void Start(){
		StartCoroutine("test");
		findPlayer();
		nav = gameObject.AddComponent(typeof(NavMeshAgent)) as NavMeshAgent;
		nav.speed = velocity * 50;
		nav.acceleration = 60;
		nav.stoppingDistance = 5;
	}
	// Update is called once per frame
	protected void Update () {
		print("updating");
		
		//velocity = 0.5f;
		//rotateAtTarget(player,velocity);
		lookAtTarget(player);
		if(canMove){
			follow();
			attack();
		}else{
			//stopAction();
			StartCoroutine(returnMovement(1f));
		}
		debugNavMesh();
	}
	Vector3 lastp;
	bool debug = true;
	public void attack(){
		cooldown += 1*Time.deltaTime;
		if(Vector3.Distance(player.position,transform.position) <=8 && cooldown >1){
			cooldown = 0;
			lookAtTarget(player);
			ani.SetTrigger("attack");
			if(target != null){
				//StartCoroutine(target.gameObject.GetComponent<CharacterBehaviour>().doDamage(transform));
                StartCoroutine(target.gameObject.GetComponent<PlayerInput>().doDamage(transform));
				//push(player);
				StartCoroutine(pushAfter());
			}
			//push(player);
		}
	}
	IEnumerator pushAfter(){
		yield return new WaitForSeconds(0.5f);
		push(player);
	}
	IEnumerator dcAc(){
		nav.enabled = false;
		print("running");
		yield return new WaitForSeconds(0.5f);
		nav.enabled = true;
	}
	IEnumerator redoDebug(float time){
		yield return new WaitForSeconds(time);
		debug = true;
		print("redo");
	}
	public void debugNavMesh(){
		if(lastp == transform.position && debug){
			StartCoroutine(dcAc());
			debug = false;
			StartCoroutine(redoDebug(10f));
		}
		lastp = transform.position;
	}
	public void follow(){
		ani.SetBool("running",false);
		if(/*Vector3.Distance(player.position,transform.position) <=1000 &&*/ target == null && !ani.GetBool("attack")){
			//followTarget(player,velocity);
			followTarget(player);
			ani.SetBool("running",true);
		}else{
			GetComponent<Rigidbody>().velocity = Vector3.zero;
			GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		}
	}
	IEnumerator colorChange(){
		canDestroy = false;
		Color c = mat.GetColor("_Color");
		mat.SetColor("_Color",Color.red);
		yield return new WaitForSeconds(0.5f);
		mat.SetColor("_Color",c);
		canDestroy = true;
	}
	public IEnumerator doDamage(){
		yield return new WaitForSeconds(0.5f);
		canMove = false;
		ani.SetBool("running",false);
		ani.ResetTrigger("attack");
		ani.SetTrigger("hit");
		StartCoroutine(slowReceiveDamage());
		float d = 33.3f-def;
		d = d < 0 ? 0:d;
		if(d > 0){
			//StartCoroutine(colorChange());
			GameObject hf = Instantiate(hitEffect,new Vector3(transform.position.x+relativeHitPos.x,transform.position.y+relativeHitPos.y,transform.position.z+relativeHitPos.z),hitEffect.transform.rotation) as GameObject;
			hf.SetActive(true);
		}
		enemylife.applyDamage(d);
		StartCoroutine(returnMovement(0.5f));
	}
	public void cancelAnimations(){
		ani.SetBool("running",false);
		//ani.ResetTrigger("hit");
		//ani.ResetTrigger("attack");
	}
	public void stopAction(){
		cancelAnimations();
	}
	public IEnumerator returnMovement(float time){
		yield return new WaitForSeconds(time);
		canMove = true;
	}
	public void findPlayer(){
		player = GameObject.FindWithTag("Player").transform;
	}
	void OnTriggerStay(Collider c){
		if(c.gameObject.tag == "Player"){
			target = c;
		}
	}
	void OnTriggerExit(Collider collision){
		target = null;
	}
	void slowMotion(){
		Time.timeScale = 0.01f;
		Time.fixedDeltaTime = 0.02f * Time.timeScale;
	}
	void normalMotion(){
		Time.timeScale = 1f;
		Time.fixedDeltaTime = 0.02f * Time.timeScale;
	}
	float lastHp;
	IEnumerator slowReceiveDamage(){
		yield return new WaitForSeconds(0.3f);
		slowMotion();
		yield return new WaitForSeconds(0.003f);
		normalMotion();
	}
	void runSlow(){
		
	}
}

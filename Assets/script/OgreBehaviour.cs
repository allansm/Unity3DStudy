using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgreBehaviour : EnemyBehaviour{
    public GameObject skill;
    public GameObject attackPos;
    public GameObject cam;
    public bool cancelAttack;
    private Coroutine coroutine;
    // Start is called before the first frame update
    void Start(){
		base.Start();
    }

    // Update is called once per frame
    void Update(){
        //velocity = 0.5f;
        lookAtTarget(player);
        follow();
        //StartCoroutine(attackCancel());
         if(ani.GetBool("hit")){
            //cancelAttack = true;
            StopCoroutine(coroutine);
            print("attack cancel");
            ani.SetBool("attack",false);
        }
		
        ogreAttack();
		debugNavMesh();
        //base.Update();
    }
    void ogreAttack(){
        cooldown += 1*Time.deltaTime;
		if(Vector3.Distance(player.position,transform.position) <=15 && cooldown >5){
			cooldown = 0;
			lookAtTarget(player);
            coroutine = StartCoroutine(attackSync());
        }
    }
    IEnumerator attackSync(){
        ani.SetBool("attack",true);
        yield return new WaitForSeconds(2f);
        GameObject inst = Instantiate(skill,attackPos.transform.position,attackPos.transform.rotation) as GameObject;
        inst.GetComponentInChildren<HitArea>().imuneEnemy = this.gameObject.name;
        inst.SetActive(true);
        if(cam != null){
            cam.GetComponent<CameraShake>().shaking = true;
        }
        ani.SetBool("attack",false);
    }
    IEnumerator attackCancel(){
        if(ani.GetBool("hit")){
            //cancelAttack = true;
            StopCoroutine(coroutine);
            print("attack cancel");
            ani.SetBool("attack",false);
        }
        yield return new WaitForSeconds(0f);
    }
}

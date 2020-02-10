using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForTest : MonoBehaviour{
    Collider col;
    GameObject grab;
    public Animator ani;
    public Transform handPos;
    public GameObject lastObj;
    // Start is called before the first frame update
    void Start(){
        
    }
	int tri = 0;
	void slowMotion(){
		float slow = 0.2f;
		float normal = 1f;
		if(Input.GetKeyDown("r")){
			if(tri == 0){
				Time.timeScale = slow;
				tri++;
				print("slow "+Time.timeScale);
			}else{
				Time.timeScale = normal;
				tri = 0;
				print("normal "+Time.timeScale);
			}
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
		}
	}
    // Update is called once per frame
    void Update(){
		slowMotion();
        ani = gameObject.GetComponent<PlayerBehaviour>().pIn.ani;
        liftTest();
        interruptHit();
    }
    void interruptHit(){
        if(lastObj != null){
            if(lastObj.GetComponent<Rigidbody>().velocity == Vector3.zero){
                lastObj.GetComponent<HitArea>().hit = false;
            }
        }
    }
    void liftTest(){
        if(grab != null){
            //grab.transform.position = new Vector3(transform.position.x,transform.position.y+5,transform.position.z-1);
            
            StartCoroutine(test());
            if(Input.GetKeyDown("j") && grab != null){
                GameObject fire = grab;
                grab = null;
                gameObject.GetComponent<PlayerBehaviour>().pIn.canMove = false;
                ani.SetTrigger("throw");
                ani.SetBool("grab",false);
                StartCoroutine(test2(fire.GetComponent<Rigidbody>()));//.AddRelativeForce(new Vector3(0,0,700*2));
                lastObj = fire;
            }
        }
        if(Input.GetKeyDown("j") && col != null){
            gameObject.GetComponent<PlayerBehaviour>().pIn.canMove = false;
            ani.ResetTrigger("throw");
            grab = col.gameObject;
            grab.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            gameObject.GetComponent<PlayerBehaviour>().pIn.lookAtTarget(grab.transform);
            col = null;
            print("lift");
            ani.SetBool("grab",true);
        }
    }
    IEnumerator test(){
        yield return new WaitForSeconds(0);
        if(grab != null  && !ani.GetCurrentAnimatorStateInfo(2).IsName("none")){
            grab.transform.position = new Vector3(handPos.position.x,handPos.position.y+1.5f,handPos.position.z);
            grab.transform.eulerAngles = new Vector3(20,transform.eulerAngles.y,transform.eulerAngles.z);
        }
    }
    IEnumerator test2(Rigidbody phys){
        print("fire");
        yield return new WaitForSeconds(0.1f);
        phys.velocity = Vector3.zero;
        //phys.constraints = RigidbodyConstraints.FreezeRotation;
        phys.AddRelativeForce(new Vector3(0,0,2000*2));
        StartCoroutine(test6(phys));
        lastObj.GetComponent<HitArea>().hit = true;
    }
    IEnumerator test6(Rigidbody r){
        yield return new WaitForSeconds(0.25f);
        r.constraints = RigidbodyConstraints.None;
    }
    void OnTriggerStay(Collider c){
        if(c.gameObject.tag == "lift" && grab == null){
            print("catched");
            col = c;
        }
    }
    void OnTriggerExit(Collider c){
        col = null;
    }
}

using UnityEngine;
using System.Collections;

public class OrbBehaviour :AI {
	public Transform player;
	public float velocity;
    public bool stop;
    public Collider target;
	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
		try{
            if(!stop){
                rotateAtTarget(player,velocity);
                follow();
            }
		}catch(System.Exception e){
			
		}
	}
	public void follow(){ 
		if(Vector3.Distance(player.position,transform.position) >5){
			followTarget(player,velocity);
		}
	}
    public IEnumerator direction(Transform target){
        yield return new WaitForSeconds(0.01f);
        lookAtTarget(target);
    }
    void OnTriggerStay(Collider c){
        if(c.gameObject.tag == "enemy"){
            this.target = c;
        }
    }
    void OnTriggerExit(Collider c){
        this.target = null;
    }
}

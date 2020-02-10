using UnityEngine;
using System.Collections;

public abstract class BasePlayer : AI {
    private bool release;
	public void move(float velocity,float x,float y,bool canMove,Animator ani){
        if(canMove && !ani.GetBool("attack") && !ani.GetBool("hit") && !ani.GetBool("heal") && !ani.GetBool("evade")){
            release = y==0?true:false;
            print("release"+release+" y"+y);
            ani.SetBool("running",false);
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
	}
}

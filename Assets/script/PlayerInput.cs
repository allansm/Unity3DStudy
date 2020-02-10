using UnityEngine;
using System.Collections;

public class PlayerInput : PlayerActions {
    void Start(){
        
    }
    public void axisXY(){
        y = Input.GetAxis("Vertical");
		x = Input.GetAxis("Horizontal");
        move(velocity,x,y,canMove,ani);
    }
    public void keyJ(){
        if(Input.GetKeyDown("j") && transformation  && !ani.GetBool("hit")){
            attack();
        }
    }
    public void keySpace(SkillEffect skill,float currentMp){
        if(Input.GetKeyDown("space")){
            if(transformation){
                evade();
            }else{
                //skill.executeHeal = true;
                heal(currentMp,skill);
            }
        }
    }
    public void keyI(SkillEffect skill){
        if(Input.GetKeyDown("i")){
            magic(skill);
            //StartCoroutine(test(skill));
        }
    }
    public IEnumerator test(SkillEffect skill){
        yield return new WaitForSeconds(0.5f);
        skill.executeMagic = true;
    }
    public void keyX(){
        if(Input.GetKeyDown("x")){
            change();
        }
    }
}

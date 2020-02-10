using UnityEngine;
using System.Collections;

public class PlayerBehaviour : GeneralStatus {
    public SkillEffect skillEffect;
    public PlayerInput pIn;
	void Start () {
        pIn.rigidbody = gameObject.GetComponent<Rigidbody>();
        base.Start();
	}
	
	void Update () {
        StartCoroutine(skillEffect.attackEffect(pIn.ani));
        if(!pIn.death(getCurrentHp())){
            base.Update();
            pIn.currentHp = getCurrentHp();
            pIn.currentMp = getCurrentMp();
            runEffects();
            pIn.velocity = velocity;
            pIn.cooldown += getCooldown();
            resetCooldown();
            pIn.changeForm();
            executeInputs();
            pIn.reset();
            if(pIn.hpLoss > 0){
                applyDamage(pIn.hpLoss);
                pIn.hpLoss = 0;
            }
            if(pIn.hpRecover > 0){
                recoverHp(pIn.hpRecover);
                pIn.hpRecover = 0;
            }
            if(pIn.mpLoss > 0){
                reduceMp(pIn.mpLoss);
                pIn.mpLoss = 0;
            }
        }
	}
    void executeInputs(){
        pIn.axisXY();
        pIn.keyJ();
        pIn.keySpace(skillEffect,getCurrentMp());
        pIn.keyI(skillEffect);
        pIn.keyX();
    }
   void runEffects(){
        skillEffect.fireBallPos();
		skillEffect.fireBall();
        skillEffect.healPosChange();
        if(!pIn.transformation){
            //if(pIn.cooldown > 1){
                skillEffect.canHeal = true;
            //}
            /*if(pIn.cooldown < 1){
                skillEffect.canHeal = false;
            }*/
            skillEffect.heal();
        }
    }
	void OnTriggerStay(Collider c){
		if(c.gameObject.tag == "enemy"){
			pIn.target = c;
		}
	}
	void OnTriggerExit(Collider collision){
		pIn.target = null;
	}
}

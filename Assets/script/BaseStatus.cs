using UnityEngine;
using System.Collections;

public abstract class BaseStatus : MonoBehaviour {
	public float maxHp;
    public float maxMp;
    private float currentHp;
    private float currentMp;
    private float cooldown;
    public float damage;
    public float velocity;
	public bool invincible;
	
    protected void Start(){
        currentHp = maxHp;
        currentMp = maxMp;
        cooldown = 0;
    }
    protected void Update(){
        countCooldown();
        recoverMp((1/(currentHp/100))*Time.deltaTime);
    }
    public void applyDamage(float damage){
		if(!invincible){
			this.currentHp-= damage;
		}
	}
    public void recoverHp(float hp){
        if(this.currentHp < this.maxHp){
            this.currentHp+= hp;
            this.currentHp = (this.currentHp > this.maxHp)?this.maxHp:this.currentHp;
        }else{
            this.currentHp = maxHp;
        }
    }
	public float getCurrentHp(){
		return this.currentHp;
	}
    
    public void reduceMp(float mp){
		if(!invincible){
			this.currentMp-= mp;
		}
	}
    public void recoverMp(float Mp){
        if(this.currentMp < this.maxMp){
            this.currentMp+= Mp;
        }else{
            this.currentMp = maxMp;
        }
    }
	public float getCurrentMp(){
		return this.currentMp;
	}
    
    void countCooldown(){
        cooldown += 1*Time.deltaTime;
    }
    public void resetCooldown(){
        cooldown = 0;
    }
    public float getCooldown(){
        return this.cooldown;
    }
}

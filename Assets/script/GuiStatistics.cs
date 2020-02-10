using UnityEngine;
using System.Collections;

public class GuiStatistics : MonoBehaviour {
	public CharacterLife cl;
    public PlayerBehaviour player;
    public Animator ani;
    public float x;
    public float y;
    public int width;
    public int height;
    public bool mobile;
    public float st;
	void Start(){
		//cl = GameObject.FindWithTag("Player").GetComponent<CharacterLife>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerBehaviour>();
        
	}
    void Update(){
        st += 1*Time.deltaTime; 
        if(mobile){
            width = Screen.width;
            height = Screen.height;
            
            if(x != 0 || y != 0){
                player.pIn.move(player.pIn.velocity,x,y,player.pIn.canMove,player.pIn.ani);
                x =0;
                y =0;
            }
        }
    }
	void OnGUI(){
		GUI.Label(new Rect(10,10,100,20),"current hp:");
		//GUI.Label(new Rect(110,10,100,20),""+cl.getCurrentHp());
        GUI.Label(new Rect(110,10,100,20),""+player.getCurrentHp());
		GUI.Label(new Rect(10,30,100,20),"current mp:");
		//GUI.Label(new Rect(110,10,100,20),""+cl.getCurrentHp());
        GUI.Label(new Rect(110,30,100,20),""+player.getCurrentMp());
        GUI.Label(new Rect(10,50,100,20),"time:");
		//GUI.Label(new Rect(110,10,100,20),""+cl.getCurrentHp());
        GUI.Label(new Rect(110,50,100,20),""+st);
		if(mobile){
            if(GUI.RepeatButton(new Rect(width*0.125f,height*0.8f,width*0.1f,height*0.07f),"up")){
                //player.pIn.move(player.pIn.velocity,0,1,player.pIn.canMove,player.pIn.ani);
                y = 1;
            }
            if(GUI.RepeatButton(new Rect(width*0.125f*1.6f,height*0.9f,width*0.1f,height*0.07f),"left")){
                //player.pIn.move(player.pIn.velocity,1,0,player.pIn.canMove,player.pIn.ani);
                x = 1;
            }
            if(GUI.RepeatButton(new Rect((width*0.125f)*0.3f,height*0.9f,width*0.1f,height*0.07f),"right")){
                //player.pIn.move(player.pIn.velocity,-1,0,player.pIn.canMove,player.pIn.ani);
                x = -1;
            }
            if(GUI.Button(new Rect(width*0.6f,height*0.9f,width*0.1f,height*0.07f),"action")){
                //player.pIn.move(player.pIn.velocity,-1,0,player.pIn.canMove,player.pIn.ani);
                //x = -1;
                player.pIn.attack();
            }
            if(player.pIn.transformation){
                if(GUI.Button(new Rect(width*0.7f,height*0.9f,width*0.1f,height*0.07f),"evade")){
                    player.pIn.evade();
                }
            }else{
                if(GUI.Button(new Rect(width*0.7f,height*0.9f,width*0.1f,height*0.07f),"heal")){
                    //player.skillEffect.executeHeal = true;
                    player.pIn.heal(player.getCurrentMp(),player.skillEffect);
                }
            }
            if(GUI.Button(new Rect(width*0.8f,height*0.9f,width*0.1f,height*0.07f),"transform")){
               player.pIn.change();
            }
            if(GUI.Button(new Rect(width*0.9f,height*0.9f,width*0.1f,height*0.07f),"magic")){
                player.pIn.magic(player.skillEffect);
                //StartCoroutine(player.pIn.test(player.skillEffect));
            }
        }
	}
}

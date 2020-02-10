using UnityEngine;
using System.Collections;

public class AudioTest : MonoBehaviour {
    public AudioClip attack1;
    public AudioClip attack2;
    public Animator ani;
    int count = 0;
	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
       
        ani = gameObject.GetComponent<PlayerBehaviour>().pIn.ani;
        
        if(ani.GetBool("attack") && count ==0){
           AudioSource src = GetComponent<AudioSource>();
           src.clip = attack1;
           src.Play();
           count++;
        }   
        else if(ani.GetBool("attack")&& count >0){
            AudioSource src = GetComponent<AudioSource>();
           src.clip = attack2;
           src.Play();
           count = 0;
        }
	}
}

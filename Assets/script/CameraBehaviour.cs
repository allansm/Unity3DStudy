using UnityEngine;
using System.Collections;

public class CameraBehaviour : AI {
	public Transform player;
	public float velocity = 9;
	public Animator ani;
	public float cooldown;
	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
		try{
			followTarget(player,velocity);
			rotateAtTarget(player,velocity);
		}catch(System.Exception e){
			
		}
	}
}

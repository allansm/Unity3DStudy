using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObstacleTest : AI{
	NavMeshAgent nav;
	public GameObject player;
    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
		nav = GetComponent<NavMeshAgent>();
        nav.SetDestination(player.transform.position);
		rotateAtTarget(player.transform,0.5f);
    }
}

using UnityEngine;
using System.Collections;

public class RestrictArea : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnCollisionEnter(Collision c){
        Destroy(c.gameObject);
    }
}

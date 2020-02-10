using UnityEngine;
using System.Collections;

public class SunBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(-1f*Time.deltaTime,0,0);
	}
}

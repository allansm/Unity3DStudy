using UnityEngine;
using System.Collections;

public class WarpTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter(Collider c){
		if(c.gameObject.tag == "Player"){
			//EditorSceneManager.OpenScene("scenetest");
			//SceneManager.LoadScene("scenetest",null);
			//Instantiate(Resources.Load("prefab/test/scenetest"));
			print("foi");
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepDustBehaviour : MonoBehaviour{
    public GameObject obj;
    public GameObject effect;
    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        detectMovement();
    }
    void detectMovement(){
        if(obj.GetComponent<Rigidbody>().velocity.z != 0){
            effect.SetActive(true);
        }else{
            effect.SetActive(false);
        }
    }
}

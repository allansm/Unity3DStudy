using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour{
    public GameObject enemy;
    private int countEnemy = 0;
    public int maxSize;
	public bool stopAfterFill;
    public float spawnTime;
	
	// Use this for initialization
	void Start () {
        o = new GameObject[maxSize];
	}
	
	// Update is called once per frame
    float size = 0;
	void Update () {
        if(countEnemy < size){
	       StartCoroutine(brota());
        }
        /*if(o[0] == null && o[1] == null){
            countEnemy = 0;
        }*/
        if(t > spawnTime && size < maxSize){
			print("time:"+t+" spawntime:"+spawnTime);
            t = 0;
            size++;
        }
        countTime();
        if(size == maxSize && verifyEnemy() && !stopAfterFill){
            size = 0;
            countEnemy = 0;
        }
	}
    float t = 0;
    void countTime(){
        t+=Time.deltaTime;
		
    }
    bool verifyEnemy(){
        bool flag = false;
        for(int i =0;i<maxSize;i++){
            flag = (o[i] == null)?true:false;
            if(flag == false){
                return flag;
            }
        }
        return flag;
    }
    GameObject[] o;
    IEnumerator brota(){
        o[countEnemy] = Instantiate(enemy,transform.position,transform.rotation) as GameObject;
        o[countEnemy].SetActive(true);
		o[countEnemy].GetComponent<EnemyBehaviour>().velocity *= 1+(2*Time.deltaTime);
        Vector3 newScale = o[countEnemy].transform.localScale;
        if(countEnemy%2 == 0){
            newScale *= 1+(2*Time.deltaTime/2);
        }else{
            newScale *= 1-(2*Time.deltaTime/2);  
        }
        o[countEnemy].transform.localScale = newScale;
        countEnemy++;
        yield return new WaitForSeconds(3f);
    }
}

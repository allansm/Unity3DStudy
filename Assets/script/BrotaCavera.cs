using UnityEngine;
using System.Collections;

public class BrotaCavera : MonoBehaviour {
    public GameObject cavera;
    public int countCavera = 0;
	// Use this for initialization
	void Start () {
        o = new GameObject[11];
	}
	
	// Update is called once per frame
    float size = 2;
	void Update () {
        if(countCavera < size){
	       StartCoroutine(brota());
        }
        /*if(o[0] == null && o[1] == null){
            countCavera = 0;
        }*/
        if(t > 30 && size < 11){
            t = 0;
            size++;
        }
        countTime();
        if(size == 11 && verifyCavera()){
            size = 2;
            countCavera = 0;
        }
	}
    float t = 0;
    void countTime(){
        t+=Time.deltaTime*1;
    }
    bool verifyCavera(){
        bool flag = false;
        for(int i =0;i<11;i++){
            flag = (o[i] == null)?true:false;
            if(flag == false){
                return flag;
            }
        }
        return flag;
    }
    GameObject[] o;
    IEnumerator brota(){
        o[countCavera] = Instantiate(cavera,transform.position,transform.rotation) as GameObject;
        o[countCavera].SetActive(true);
        countCavera++;
        yield return new WaitForSeconds(3f);
    }
}

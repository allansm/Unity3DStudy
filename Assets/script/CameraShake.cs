
using UnityEngine;
using System.Collections;

public class CameraShake: MonoBehaviour {
	public float shakeIntensity = 0.05f;
	public float shakeSpeed = 0.005f;
	public static bool shakeScreen = false;     

	private float shakeFactor;
	private Vector3 originalPosition;
	private Quaternion originalRotation;
	public bool shaking;
	private int cont = 0;
	private Vector3 defaultPosition;
	private Quaternion defaultRotation;
	public bool shaking2;
	void Start(){
		defaultPosition = transform.position;
		defaultRotation = transform.rotation;
	}

	void Update(){
		/*originalRotation.x  = 0;
		originalRotation.y =0;
		originalRotation.z=0;
		originalRotation.w=0;*/
		//originalPosition = defaultPosition;
		//originalRotation = defaultRotation;
        //print(cont);
        originalPosition = transform.position;
        originalRotation = transform.rotation;
		if (/*Input.GetKeyDown(KeyCode.P)*/shaking) {
			//print ("entrei");

			if (cont++ > 0) {
				ShakeCamera ();
				Shake ();
			}
			if (cont++ > 45) {
				shaking = false;
				cont = 0;
			}
			//shaking = false;
		} /*else {
			cont = 0;
		}*//* else {
			transform.position = defaultPosition;
			transform.rotation = defaultRotation;
		}*/
		//shaking = false;
	}

	private void ShakeCamera()
	{
		if (shakeFactor > 0)
		{
			transform.position = originalPosition + Random.insideUnitSphere * shakeFactor;
			transform.rotation = new Quaternion(
				originalRotation.x + Random.Range(-shakeFactor, shakeFactor) * .2f,
				originalRotation.y + Random.Range(-shakeFactor, shakeFactor) * .2f,
				originalRotation.z + Random.Range(-shakeFactor, shakeFactor) * .2f,
				originalRotation.w + Random.Range(-shakeFactor, shakeFactor) * .2f);
			shakeFactor -= shakeSpeed;
		}
	}

	public void Shake()
	{
		originalPosition = transform.position;
		originalRotation = transform.rotation;
		shakeFactor = shakeIntensity;
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Reverse the effect of the Rift's neck rotation model, because we
//already have that same model implicitly built in to ZED's inside out tracking

public class OffsetNeckModel : MonoBehaviour {

	private Transform camera;
	private Vector3 oldTransform;
	private Quaternion oldRotation;

	// Use this for initialization
	void Start () {
		//ASSUMPTION: only 1 child, if more than one, refer to it by name
		camera = gameObject.transform.GetChild(0);
		oldTransform = Vector3.zero;

	}

	// Update is called once per frame
	void Update () {
		transform.localPosition -= (camera.localPosition - oldTransform);
		oldTransform = camera.localPosition;



	}


}

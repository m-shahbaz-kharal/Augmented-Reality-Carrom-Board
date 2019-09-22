using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomColliderBehaviour : MonoBehaviour {
	[SerializeField]
	public static float YPositiveLimit = 0.14f, YNegitiveLimit = -10f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float YVal = Mathf.Clamp (transform.localPosition.y, YNegitiveLimit, YPositiveLimit);
		if (transform.localPosition.y < YNegitiveLimit || transform.localPosition.y > YPositiveLimit) {
			transform.localPosition = new Vector3 (transform.localPosition.x, YVal, transform.localPosition.z);
		}
	}
}

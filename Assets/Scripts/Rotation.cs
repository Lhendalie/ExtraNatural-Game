using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {
        transform.Rotate(Vector3.forward * +90);
    }
}

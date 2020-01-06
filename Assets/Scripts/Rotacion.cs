using UnityEngine;
using System.Collections;

public class Rotacion : MonoBehaviour {
    public Vector3 angularSpeed;
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(angularSpeed * Time.deltaTime);
	}
}

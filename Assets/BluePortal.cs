using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluePortal : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Renderer>().material.color = this.gameObject.name == "Blue Portal" ? new Color(0, 0, 255) : new Color(255, 140, 0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision) {
        var otherPortal = GameObject.Find(this.gameObject.name == "Blue Portal" ? "Orange Portal" : "Blue Portal").transform;
        collision.gameObject.transform.position = otherPortal.position + otherPortal.up * .4f;
        var newAngle = otherPortal.up * collision.relativeVelocity.magnitude;
        collision.rigidbody.AddForce(newAngle, ForceMode.Impulse);
    }
}

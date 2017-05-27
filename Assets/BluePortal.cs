using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluePortal : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Renderer>().material.color = new Color(0, 0, 255);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision) {
        //collision.collider.
        Debug.Log(collision.gameObject.name);
        var collisionObjectPosition = collision.gameObject.transform.position;
        collision.gameObject.transform.position = new Vector3(collisionObjectPosition.x, collisionObjectPosition.y, collisionObjectPosition.z + 2);
        collision.rigidbody.AddForce(collision.impulse, ForceMode.Impulse);
    }
}

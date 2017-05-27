using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour {
    private bool isGrabbed = false;

	// Use this for initialization
	void Start () {
        GetComponent<Renderer>().material.color = new Color(0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
        if (isGrabbed) {
            var headPosition = Camera.main.transform.position;
            var gazeDirection = Camera.main.transform.forward;
            this.gameObject.transform.position = headPosition + gazeDirection * 2;
            this.gameObject.transform.rotation = Camera.main.transform.rotation;
            Destroy(this.gameObject.GetComponent<Rigidbody>());
        }		
        else {
            this.gameObject.AddComponent<Rigidbody>();
        }
    }

    void OnSelect () {
        GetComponent<Renderer>().material.color = new Color(255, 0, 0);
    }

    void OnDeselect() {
        GetComponent<Renderer>().material.color = new Color(0, 0, 0);
    }

    void OnGrab() {
        isGrabbed = !isGrabbed;
    }
}

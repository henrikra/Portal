using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Renderer>().material.color = new Color(0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnSelect () {
        GetComponent<Renderer>().material.color = new Color(255, 0, 0);
    }

    void OnDeselect() {
        GetComponent<Renderer>().material.color = new Color(0, 0, 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA.Input;

public class Cursor : MonoBehaviour {
    private GameObject lastHitGameObject;
    GestureRecognizer recognizer;

	// Use this for initialization
	void Start () {
        recognizer = new GestureRecognizer();
        recognizer.TappedEvent += ToggleCubeGrab;
        recognizer.StartCapturingGestures();
	}

    private void ToggleCubeGrab(InteractionSourceKind source, int tapCount, Ray headRay) {
        if (lastHitGameObject != null) {
            lastHitGameObject.SendMessage("OnGrab");
        }
    }

    void resetSelection () {
        if (lastHitGameObject != null) {
            lastHitGameObject.SendMessage("OnDeselect");
            lastHitGameObject = null;
        }
    }
	
	// Update is called once per frame
	void Update () {
        var headPosition = Camera.main.transform.position;
        var gazeDirection = Camera.main.transform.forward;

        RaycastHit hitInfo;
        if (Physics.Raycast(headPosition, gazeDirection, out hitInfo)) {
            if (hitInfo.distance <= 2) {
                hitInfo.transform.gameObject.SendMessage("OnSelect");
                lastHitGameObject = hitInfo.transform.gameObject;
            }
            else {
                resetSelection();
            }
        }
        else {
            resetSelection();
        }
		if (Input.GetKeyUp(KeyCode.Space)) {
            InteractionSourceKind lol = new InteractionSourceKind();
            Ray lol2 = new Ray();
            ToggleCubeGrab(lol, 2, lol2);
        }
	}
}

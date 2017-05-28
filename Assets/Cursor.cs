using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA.Input;

public class Cursor : MonoBehaviour {
    private GameObject lastHitGameObject;
    GestureRecognizer recognizer;
    private bool shouldShootBluePortal = true;

	// Use this for initialization
	void Start () {
        recognizer = new GestureRecognizer();
        recognizer.TappedEvent += ToggleCubeGrab;
        recognizer.StartCapturingGestures();
	}

    private void ToggleCubeGrab(InteractionSourceKind source, int tapCount, Ray headRay) {
        if (lastHitGameObject) {
            lastHitGameObject.SendMessage("OnGrab");
        }
        else {
            var headPosition = Camera.main.transform.position;
            var gazeDirection = Camera.main.transform.forward;
            RaycastHit hitInfo;
            if (Physics.Raycast(headPosition, gazeDirection, out hitInfo)) {
                if (shouldShootBluePortal && GameObject.Find("Blue Portal")) {
                    Destroy(GameObject.Find("Blue Portal"));
                }
                else if (!shouldShootBluePortal && GameObject.Find("Orange Portal")) {
                    Destroy(GameObject.Find("Orange Portal"));
                }
                var newPortal = GameObject.CreatePrimitive(PrimitiveType.Cube);
                newPortal.transform.localScale = new Vector3(.6f, .02f, .9f);
                newPortal.transform.position = hitInfo.point;
                newPortal.GetComponent<Renderer>().material.color = shouldShootBluePortal ? new Color(0, 0, 255) : new Color(255, 140, 0);
                newPortal.transform.rotation = hitInfo.transform.rotation;
                newPortal.name = shouldShootBluePortal ? "Blue Portal" : "Orange Portal";
                newPortal.AddComponent<BluePortal>();

                shouldShootBluePortal = !shouldShootBluePortal;
            }
        }
    }

    void resetSelection () {
        if (lastHitGameObject) {
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
                if (lastHitGameObject && lastHitGameObject != hitInfo.transform.gameObject) {
                    lastHitGameObject.SendMessage("OnDeselect");
                }
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
            ToggleCubeGrab(new InteractionSourceKind(), 1, new Ray());
        }
	}
}

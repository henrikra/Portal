using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour {
    private MeshRenderer meshRenderer;
    private GameObject lasthHitGameObject;
	// Use this for initialization
	void Start () {
        meshRenderer = gameObject.GetComponentInChildren<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        var headPosition = Camera.main.transform.position;
        var gazeDirection = Camera.main.transform.forward;

        RaycastHit hitInfo;
        if (Physics.Raycast(headPosition, gazeDirection, out hitInfo)) {
            if (hitInfo.distance <= 2) {
                Debug.Log(hitInfo.distance);
                hitInfo.transform.gameObject.SendMessage("OnSelect");
                lasthHitGameObject = hitInfo.transform.gameObject;
            }
            else {
                lasthHitGameObject.SendMessage("OnDeselect");
            }
        }
        else {
            lasthHitGameObject.SendMessage("OnDeselect");
        }
		
	}
}

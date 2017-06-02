using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VR.WSA.Input;
using UnityEngine.Windows.Speech;

public class Cursor : MonoBehaviour {
    private GameObject lastHitGameObject;
    GestureRecognizer recognizer;
    private bool shouldShootBluePortal = true;
    AudioSource audioSource;
    AudioClip portalGunShooting;

    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    // Use this for initialization
    void Start () {
        recognizer = new GestureRecognizer();
        recognizer.TappedEvent += ToggleCubeGrab;
        recognizer.StartCapturingGestures();

        audioSource = this.gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialize = true;
        audioSource.spatialBlend = 1.0f;
        audioSource.dopplerLevel = 0.0f;
        audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
        portalGunShooting = Resources.Load<AudioClip>("PortalGunShooting");

        keywords.Add("Give cube", () => {
            SpawnCube();
        });
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
    }

    void SpawnCube() {
        var newCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        newCube.transform.localScale = new Vector3(.2f, .2f, .2f);
        newCube.transform.position = Camera.main.transform.position;
        newCube.AddComponent<Rigidbody>();
        newCube.AddComponent<Cube>();
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args) {
        System.Action keywordAction;
        if (keywords.TryGetValue(args.text, out keywordAction)) {
            keywordAction.Invoke();
        }
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
                newPortal.transform.localScale = new Vector3(.55f, .02f, .95f);
                newPortal.transform.position = hitInfo.point;
                newPortal.GetComponent<Renderer>().material.color = shouldShootBluePortal ? new Color(0, 0, 255) : new Color(255, 140, 0);
                newPortal.transform.rotation = hitInfo.transform.rotation;
                newPortal.name = shouldShootBluePortal ? "Blue Portal" : "Orange Portal";
                newPortal.AddComponent<BluePortal>();

                shouldShootBluePortal = !shouldShootBluePortal;

                audioSource.clip = portalGunShooting;
                audioSource.Play();
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
        else if (Input.GetKeyUp(KeyCode.KeypadEnter)) {
            SpawnCube();
        }
    }
}

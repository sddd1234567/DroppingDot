using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    [SerializeField]
    Cat cat;
    [SerializeField]
    GameObject wall;
    

	void Start () {
		
	}
	

	void Update () {
		
	}

    void FixedUpdate() {
        if (Mathf.Abs((cat.transform.position.y-3.5f) - transform.position.y) > 0)
            smoothlyMove();
        //transform.SetPositionAndRotation(new Vector3(0, cat.transform.position.y - 3.5f, -10), transform.rotation);
        wall.transform.SetPositionAndRotation(new Vector3(wall.transform.position.x, transform.position.y, 90), wall.transform.rotation);
    }

    void smoothlyMove() {
        transform.SetPositionAndRotation(new Vector3(0, Mathf.Lerp(transform.position.y, cat.transform.position.y-3.5f,8 * Time.fixedDeltaTime),-10), transform.rotation);
    }
}

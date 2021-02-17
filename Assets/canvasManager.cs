using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class canvasManager : MonoBehaviour {
    public CanvasScaler canvasScaler;
	// Use this for initialization
	void Start () {
        canvasScaler = GetComponent<CanvasScaler>();
        if (((float)Screen.width / (float)Screen.height) > 9f / 16f)
            canvasScaler.matchWidthOrHeight = 1;
        else
            canvasScaler.matchWidthOrHeight = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

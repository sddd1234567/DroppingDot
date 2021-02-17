using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEManager : MonoBehaviour {
    public GameObject buttonSE;
    public GameObject passSE;
    public GameObject loseSE;

    public void playSE(GameObject SE) {
        Instantiate(SE);
    }
	
}

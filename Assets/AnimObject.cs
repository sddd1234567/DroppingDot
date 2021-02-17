using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimObject : MonoBehaviour {

    public void setNotActive() {
        gameObject.SetActive(false);
    }

    public void destroy() {
        Destroy(gameObject);
    }
}

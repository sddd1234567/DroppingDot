using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviveConfirm : MonoBehaviour {

	public void confirm()
    {
        GameManager.instance.revive();
        Destroy(gameObject);
    }

    public void cancel() {
        Destroy(gameObject);
    }
}

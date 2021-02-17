using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour {

    public void onReviveButtonClicked() {
        if (GameManager.instance.reviveTime <= 0 || GameManager.instance.playerInfo.heart <= 0)
            return;
        GameManager.instance.seManager.playSE(GameManager.instance.seManager.buttonSE);
        GameManager.instance.uiManager.createUIObject(GameManager.instance.uiManager.reviveConfirmPanel);
    }
}

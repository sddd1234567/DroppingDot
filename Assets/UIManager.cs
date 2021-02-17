using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public GameObject canvas;

    public Text score;
    public Text bestScore;
    public Text reviveTime;
    public Text heart;
    public Text finalScore;
    public Text videoWaitTime;
    public Text addHeartText;

    public GameObject startButton;
    public GameObject restartButton;
    public GameObject reviveButton;
    public GameObject heartObj;
    public GameObject watchVideoButton;
    public GameObject adBlueHeart;
    public GameObject googlePlayServicesButton;
    public GameObject reviveConfirmPanel;
    public GameObject loadingAdsUI;
    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    void FixedUpdate() {
        if (GameManager.instance.isGameStart)
            score.text = GameManager.instance.catController.nowFloor.ToString();
    }

    public void updateBestScore(int sc) {
        bestScore.text = "BEST：" + sc;
    }

    public void setUIActive(GameObject ui, bool ac) {
        ui.SetActive(ac);
    }

    public void setText(Text text, string content)
    {
        text.text = content;
    }

    public void disActiveAllUI()
    {
        setUIActive(googlePlayServicesButton, false);
        setUIActive(startButton, false);
        setUIActive(restartButton, false);
        setUIActive(reviveButton, false);
        setUIActive(bestScore.gameObject, false);
        setUIActive(heartObj, false);
        setUIActive(finalScore.gameObject, false);
        setUIActive(watchVideoButton, false);
    }

    public void uiGameOver() {
        setUIActive(score.gameObject, false);
        setUIActive(restartButton, true);
        setUIActive(reviveButton, true);
        setUIActive(bestScore.gameObject, true);
        setUIActive(heartObj, true);
        setUIActive(watchVideoButton, true);
        setText(finalScore, score.text);
        setUIActive(finalScore.gameObject, true);
        setUIActive(googlePlayServicesButton, true);
    }

    public void updatePlayerInfoUI(int heartNum)
    {
        setText(heart, heartNum.ToString());
    }

    public void addHeartTextAct(int addNum) {
        addHeartText.text = "+" + addNum.ToString();
        addHeartText.gameObject.SetActive(true);
    }

    public GameObject createUIObject(GameObject obj) {

        GameObject UIObj = Instantiate(obj, canvas.transform);
        UIObj.transform.position = obj.transform.position;
        UIObj.transform.localScale = obj.transform.localScale;

        return UIObj;
    }

}

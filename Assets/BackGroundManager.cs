using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundManager : MonoBehaviour {

    [SerializeField]
    SpriteRenderer bg;

    [SerializeField]
    public List<Color> levelColor;

    int nowColorNum = 0;

    public void changeBGColor() {
        nowColorNum++;
        if (nowColorNum >= levelColor.Count)
            nowColorNum = 0;
        StartCoroutine(changeColor(levelColor[nowColorNum]));
    }

    IEnumerator changeColor(Color targetColor) {
        yield return null;
        float countTime = 0;
        while (countTime < 1.5f)
        {
            yield return null;
            countTime += Time.deltaTime;
            bg.color = Color.Lerp(bg.color, targetColor, Time.deltaTime);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour {
    public GameObject shop;

    void Start() {
        shop.SetActive(true);
    }

    public void buyheart30() {
        GameManager.instance.givePlayerHeart(30);
    }

    public void buyheart100()
    {
        GameManager.instance.givePlayerHeart(100);
    }

    public void buyheart250()
    {
        GameManager.instance.givePlayerHeart(250);
    }

    public void buyheart350()
    {
        GameManager.instance.givePlayerHeart(350);
    }
}

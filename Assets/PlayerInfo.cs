using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour {
    public int heart;

    public void addHeart(int num)
    {
        heart += num;
    }

    public void useHeart(int num) {
        heart -= num;
    }
}



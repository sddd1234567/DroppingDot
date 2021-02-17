using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CatTrigger : MonoBehaviour {
    [SerializeField]
    Cat cat;

    float nowFloorY;

    void Update() {
        if (cat.state == 2 && transform.position.y < (nowFloorY - 0.3f))
        {
            cat.changeCatState(1);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Water" && cat.state == 1)
        {
            if (col.GetComponent<Floor>())
            {
                Floor fl = col.GetComponent<Floor>();
                if (fl.floorNum >= cat.nowFloor)
                {
                    cat.nowFloor++;
                    GameManager.instance.seManager.playSE(GameManager.instance.seManager.passSE);
                    nowFloorY = col.transform.position.y;
                    GameManager.instance.floorManager.addFloorWall(fl.floorNum,1);                    
                }
                cat.changeCatState(2);
            }            
        }
    }

   /* [MethodImpl(MethodImplOptions.Synchronized)]
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Water" && cat.state == 2)
        {
                cat.changeCatState(1);
        }
    }*/

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.GetComponent<Floor>())
        {
            Floor fl = col.gameObject.GetComponent<Floor>();
            if (fl.floorNum >= cat.nowFloor)
            {
                if (col.gameObject.tag == "Ground")
                {
                    if (GameManager.instance.isGameStart)
                    {
                        GameManager.instance.gameOver(transform.position);
                        GameManager.instance.seManager.playSE(GameManager.instance.seManager.loseSE);
                        cat.rig.constraints = RigidbodyConstraints2D.FreezeAll;
                        cat.changeCatState(0);
                        GameManager.instance.floorManager.addFloorWall(fl.floorNum, 0);
                    }                    
                }
            }
        }
    }

}

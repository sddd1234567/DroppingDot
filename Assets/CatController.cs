using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CatController : MonoBehaviour {
    [SerializeField]
    GameObject cat;

    public float speed;

    public int nowFloor = 0;


    Rigidbody2D catRig;
    public Cat catScript;

    public FloorManager floorManager;
    int lastState;

    float nowDropSpeed = 0;

    public List<Color> catColor;

    int nowCatColor = 0;


    void Awake() {
        catRig = cat.GetComponent<Rigidbody2D>();
        catScript = cat.GetComponent<Cat>();
    }


	// Use this for initialization
	void Start () {
        lastState = catScript.state;
    }
	
	// Update is called once per frame
	void Update () {
        catStateJudge();
        passFloor();
    }

    void FixedUpdate() {
        // onCatStateChange(state);
        
        
    }
    

    public void setCatActive(bool act)
    {
        cat.SetActive(act);
    }     

    public void catHorizonSpeed(float xValue) {
        if (catScript.state != 0)
            catRig.velocity = (new Vector2(xValue * speed, catRig.velocity.y));
    }
    
    public void passFloor() {
        if (nowFloor == catScript.nowFloor)
            return;
        if (nowFloor < 100)
        {
            speed += 0.9f;
            nowDropSpeed += 0.1f;
        }
        nowFloor++;
        floorManager.createFloor();
        if (floorManager.floors.Count > 5)
        {
            floorManager.deleteFloor(nowFloor - 6);
        }
            
        if (nowFloor % 5 == 0)
        {
            GameManager.instance.backGroundManager.changeBGColor();
            changeCatColor();
        }
    }

    public void changeCatColor() {
        nowCatColor++;
        if (nowCatColor > 6)
            nowCatColor = 0;
        StartCoroutine(smoothlyChangeColor(catColor[nowCatColor]));
    }

    IEnumerator smoothlyChangeColor(Color targetColor) {
        yield return null;
        float countTime = 0;
        while (countTime < 1.5f)
        {
            yield return null;
            countTime += Time.deltaTime;
            catScript.spriteRenderer.color = Color.Lerp(catScript.spriteRenderer.color, targetColor, 1.5f * Time.deltaTime);
        }
    }

    void catStateJudge()
    {
        if (catScript.state != lastState)
        {
            onCatStateChange(catScript.state);
            lastState = catScript.state;
        }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    void onCatStateChange(int statee)
    {
        if (statee == 0)
        {
            catRig.velocity = Vector2.zero;            
        }
        else if (statee == 1)
        {
            catRig.gravityScale = 1;
        }
        else if (statee == 2)
        {
            catRig.gravityScale = 0;
            catRig.velocity = (Vector2.down * 0.8f) + (Vector2.down * nowDropSpeed) + Vector2.right * (catRig.velocity.x);
        }
    }

    public void moveCatPosition(Vector3 pos) {
        cat.transform.SetPositionAndRotation(pos, cat.transform.rotation);
    }

    public void forceChanceCatState(int state) {
        catScript.state = state;
    }

}

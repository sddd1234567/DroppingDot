using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Cat : MonoBehaviour {
    public Rigidbody2D rig;
    public int state;                   // 0 = stop   1 = dropping  2 = water

    public int nowFloor = 0;
    public SpriteRenderer spriteRenderer;
    

    void Awake() {
        rig = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
	
	void Start () {

    }

    void FixedUpdate() {

        if (rig.velocity.y > 0)
        {
            rig.velocity = Vector2.right * rig.velocity.x;
        }
    }
	
	
	void Update () {        

        
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void changeCatState(int statee)
    {
        if (state != 0 || !GameManager.instance.isGameStart)
            state = statee;
    }
}

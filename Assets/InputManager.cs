using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    [SerializeField]
    CatController catController;
#pragma warning disable 0219

    void FixedUpdate() {
        accelerateInput();
    }

    void accelerateInput() {
        float xValue = Input.acceleration.x;
        if (Mathf.Abs(xValue) >= 0.008f)
        {
            if (xValue > 0)
                xValue -= 0.008f;
            else
                xValue += 0.008f;
            catController.catHorizonSpeed(xValue);
        }
            

        float xValue2 = Input.GetAxis("Horizontal");
        #if UNITY_EDITOR
        catController.catHorizonSpeed(xValue2);
        #endif
    }   
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour {
    public void destroyScene(string sceneName) {
        GameManager.instance.changeGameState(0);
        SceneManager.UnloadSceneAsync(sceneName);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public UIManager uiManager;
    public FloorManager floorManager;
    public AnimationManager animationManager;
    public CatController catController;
    public BackGroundManager backGroundManager;
    public PlayerInfo playerInfo;
    public AdManager adManager;
    public SEManager seManager;

    public bool isGameStart;
    private int bestScore = 0;
    private int gamesPlayed = 0;

    public int reviveTime = 3;

    int gameState = 0;       //0 = maingamescene     1 = shopscene

    public bool isTryingLogin;

    void Awake() {        
        instance = this;
        Application.targetFrameRate = 60;
    }


    void Update() {
        onEscapeClicked();
    }

	void Start () { 
        loadLocalBestScore();
        loadPlayerInfo();
        loadGamesPlayed();
	}

    public void gameOver(Vector3 endPos) {
        catController.catScript.transform.SetPositionAndRotation(endPos, catController.catScript.transform.rotation);
        Screen.sleepTimeout = SleepTimeout.SystemSetting;      
        saveBestScore(catController.nowFloor);
        isGameStart = false;
        uiManager.uiGameOver();
    }

    public void onStartButtonClicked() {
        gameStart();
        seManager.playSE(seManager.buttonSE);
    }

    public void onRestartButtonClicked() {
        seManager.playSE(seManager.buttonSE);
        SceneManager.LoadScene("MainGameScene");
    }

    public void loadLocalBestScore() {
        bestScore = PlayerPrefs.GetInt("BestScore", 0);
        uiManager.updateBestScore(bestScore);
    }

    public void saveBestScore(int score) {   
        if (score > bestScore)
        {
            if (Social.localUser.authenticated)
            {
                    PlayServicesManager.instance.addScoreToLeaderboard(score);
                    PlayServicesManager.instance.checkScoreAchievement(score);                
            }

            bestScore = score;
            PlayerPrefs.SetInt("BestScore", score);
            PlayerPrefs.Save();
            uiManager.updateBestScore(score);                
        }
    }

    public void loadPlayerInfo()
    {
        playerInfo.heart = PlayerPrefs.GetInt("Heart", 0);
        uiManager.updatePlayerInfoUI(playerInfo.heart);
    }

    public void savePlayerInfo() {
        PlayerPrefs.SetInt("Heart", playerInfo.heart);
        PlayerPrefs.Save();
    }

    public void revive() {        
        reviveTime--;
        playerInfo.useHeart(1);
        updatePlayerInfo();
        uiManager.setText(uiManager.reviveTime, "(" + reviveTime + "/3)");
        gameStart();
        StartCoroutine(waitToRevive());
        floorManager.deleteFloor(catController.nowFloor -1);
        floorManager.deleteFloorWall(catController.nowFloor);
        catController.moveCatPosition(new Vector3(0, floorManager.floorStartY + (catController.nowFloor - 1) * -10, catController.catScript.transform.position.z));    
        uiManager.setUIActive(uiManager.score.gameObject, true);
        adManager.stopShowingAds();
    }

    IEnumerator waitToRevive() {
        yield return new WaitForSeconds(0.8f);
        catController.forceChanceCatState(1);
        catController.catScript.rig.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void gameStart()
    {
        if (isTryingLogin || adManager.isShowingAd)
            return;
        adManager.stopShowingAds();
        seManager.playSE(seManager.buttonSE);
        uiManager.disActiveAllUI();
        adManager.StopAllCoroutines();
        Screen.sleepTimeout = SleepTimeout.NeverSleep;      
        catController.setCatActive(true);
        catController.catScript.changeCatState(1);
        isGameStart = true;
        gamesPlayed++;
        saveGamesPlayed(gamesPlayed);
    }

    public void updatePlayerInfo() {
        uiManager.updatePlayerInfoUI(playerInfo.heart);
        savePlayerInfo();
    }

    public void givePlayerHeart(int num) {
        playerInfo.addHeart(num);
        updatePlayerInfo();
        uiManager.addHeartTextAct(num);
    }

    public void openShop() {
        gameState = 1;
        SceneManager.LoadScene("ShopScene", LoadSceneMode.Additive);
        seManager.playSE(seManager.buttonSE);
    }

    public void changeGameState(int state) {
        gameState = state;
    }

    public void onEscapeClicked() {
        if (!Input.GetKeyDown(KeyCode.Escape))
            return;


        if (gameState == 0)
            Application.Quit();
        else if (gameState == 1)
        {
            SceneManager.UnloadSceneAsync("ShopScene");
            gameState = 0;
        }
    }

    public void ShowLeaderboardUI() {
        PlayServicesManager.instance.showLeaderboardUI();
    }

    public void ShowAchievementsUI()
    {
        PlayServicesManager.instance.showAchievementsUI();
    }

    public bool isFirstTime() {
        if (!PlayerPrefs.HasKey("isFirstTime"))
        {            
            return true;
        }
        else
            return false;
    }

    public void loadGamesPlayed()
    {
        gamesPlayed = PlayerPrefs.GetInt("GamesPlayed", 0);
        saveGamesPlayed(gamesPlayed);
    }
    
    public void saveGamesPlayed(int gamesCount) {
        PlayerPrefs.SetInt("GamesPlayed", gamesCount);
        PlayServicesManager.instance.checkGamesAchievement(gamesCount);
    }
}

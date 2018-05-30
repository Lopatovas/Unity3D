using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public GameObject EndGameScreen;
    public Text endGameText;
    private bool gameHasEnded = false;
    public Movement movement;

    public void EndGame()
    {
        if(gameHasEnded == false)
        {
            gameHasEnded = true;
            UpdateInfoText();
            EndGameScreen.SetActive(true);
            Time.timeScale = 0f;

        }
    }

    public void UpdateInfoText()
    {
        endGameText.text = "Congratulations, you finished the track " + movement.position + "!";
    }
}

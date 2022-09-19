using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndManager : MonoBehaviour
{

    public int numberOfGoodAnswer = 1;

    public void checkGameEnd() {
        Debug.Log("number of good answers: " + this.numberOfGoodAnswer);

        this.numberOfGoodAnswer--;

        if (numberOfGoodAnswer == 0)
        {
            GameEndPanelHandler panel = FindObjectOfType<GameEndPanelHandler>();
            panel.OpenGameEndPanel();
        }
    }
}

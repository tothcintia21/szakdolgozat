using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndPanelHandler : MonoBehaviour
{
    public GameObject gameEndPanel = null;

    public void OpenGameEndPanel() {
        Debug.Log("Open panel");

        this.gameEndPanel.SetActive(true);
    }
}

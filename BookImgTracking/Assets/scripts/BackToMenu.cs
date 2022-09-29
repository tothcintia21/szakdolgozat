using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenu : MonoBehaviour
{
    public void BackToMainMenu() {
        Debug.Log("Back to");
        SceneManager.LoadScene(0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    DropdownListManager dropdownHandler;

    private void Start()
    {
        dropdownHandler = GetComponent<DropdownListManager>();
    }
    public void LoadGame() {
        var diff = dropdownHandler.getDiffDropdownsValue();
        var pos = dropdownHandler.getPosDropdownsValue();
        var speed = dropdownHandler.getSpeedDropdownsValue();

        var gameState = diff + ',' + pos + ',' + speed;

        switch(gameState)
        {
            case "Könnyû,Szemben,Lassú":
                SceneManager.LoadScene(2);
                Debug.Log("Load scene 2");
                break;
            case "Könnyû,Szemben,Normál":
                SceneManager.LoadScene(3);
                Debug.Log("Load scene 3");
                break;
            case "Könnyû,Szemben,Gyors":
                SceneManager.LoadScene(4);
                Debug.Log("Load scene 4");
                break;
            case "Normál,Szemben,Lassú":
                SceneManager.LoadScene(5);
                Debug.Log("Load scene 5");
                break;
            case "Normál,Szemben,Normál":
                SceneManager.LoadScene(6);
                Debug.Log("Load scene 6");
                break;
            case "Normál,Szemben,Gyors":
                SceneManager.LoadScene(7);
                Debug.Log("Load scene 7");
                break;
            case "Nehéz,Szemben,Lassú":
                SceneManager.LoadScene(8);
                Debug.Log("Load scene 8");
                break;
            case "Nehéz,Szemben,Normál":
                SceneManager.LoadScene(9);
                Debug.Log("Load scene 9");
                break;
            case "Nehéz,Szemben,Gyors":
                SceneManager.LoadScene(10);
                Debug.Log("Load scene 10");
                break;
            case "Könnyû,Körben,Lassú":
                SceneManager.LoadScene(11);
                Debug.Log("Load scene 11");
                break;
            case "Könnyû,Körben,Normál":
                SceneManager.LoadScene(12);
                Debug.Log("Load scene 12");
                break;
            case "Könnyû,Körben,Gyors":
                SceneManager.LoadScene(13);
                Debug.Log("Load scene 13");
                break;
            case "Normál,Körben,Lassú":
                SceneManager.LoadScene(14);
                Debug.Log("Load scene 14");
                break;
            case "Normál,Körben,Normál":
                SceneManager.LoadScene(15);
                Debug.Log("Load scene 15");
                break;
            case "Normál,Körben,Gyors":
                SceneManager.LoadScene(16);
                Debug.Log("Load scene 16");
                break;
            case "Nehéz,Körben,Lassú":
                SceneManager.LoadScene(17);
                Debug.Log("Load scene 17");
                break;
            case "Nehéz,Körben,Normál":
                SceneManager.LoadScene(18);
                Debug.Log("Load scene 18");
                break;
            case "Nehéz,Körben,Gyors":
                SceneManager.LoadScene(19);
                Debug.Log("Load scene 19");
                break;
            default:
                SceneManager.LoadScene(1);
                Debug.Log("Load scene 1");
                break;
        }
    }
}

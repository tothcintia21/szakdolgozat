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
            case "K�nny�,Szemben,Lass�":
                SceneManager.LoadScene(2);
                Debug.Log("Load scene 2");
                break;
            case "K�nny�,Szemben,Norm�l":
                SceneManager.LoadScene(3);
                Debug.Log("Load scene 3");
                break;
            case "K�nny�,Szemben,Gyors":
                SceneManager.LoadScene(4);
                Debug.Log("Load scene 4");
                break;
            case "Norm�l,Szemben,Lass�":
                SceneManager.LoadScene(5);
                Debug.Log("Load scene 5");
                break;
            case "Norm�l,Szemben,Norm�l":
                SceneManager.LoadScene(6);
                Debug.Log("Load scene 6");
                break;
            case "Norm�l,Szemben,Gyors":
                SceneManager.LoadScene(7);
                Debug.Log("Load scene 7");
                break;
            case "Neh�z,Szemben,Lass�":
                SceneManager.LoadScene(8);
                Debug.Log("Load scene 8");
                break;
            case "Neh�z,Szemben,Norm�l":
                SceneManager.LoadScene(9);
                Debug.Log("Load scene 9");
                break;
            case "Neh�z,Szemben,Gyors":
                SceneManager.LoadScene(10);
                Debug.Log("Load scene 10");
                break;
            default:
                SceneManager.LoadScene(1);
                Debug.Log("Load scene 1");
                break;
        }
    }
}

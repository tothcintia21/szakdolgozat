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
            case "K�nny�,K�rben,Lass�":
                SceneManager.LoadScene(11);
                Debug.Log("Load scene 11");
                break;
            case "K�nny�,K�rben,Norm�l":
                SceneManager.LoadScene(12);
                Debug.Log("Load scene 12");
                break;
            case "K�nny�,K�rben,Gyors":
                SceneManager.LoadScene(13);
                Debug.Log("Load scene 13");
                break;
            case "Norm�l,K�rben,Lass�":
                SceneManager.LoadScene(14);
                Debug.Log("Load scene 14");
                break;
            case "Norm�l,K�rben,Norm�l":
                SceneManager.LoadScene(15);
                Debug.Log("Load scene 15");
                break;
            case "Norm�l,K�rben,Gyors":
                SceneManager.LoadScene(16);
                Debug.Log("Load scene 16");
                break;
            case "Neh�z,K�rben,Lass�":
                SceneManager.LoadScene(17);
                Debug.Log("Load scene 17");
                break;
            case "Neh�z,K�rben,Norm�l":
                SceneManager.LoadScene(18);
                Debug.Log("Load scene 18");
                break;
            case "Neh�z,K�rben,Gyors":
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

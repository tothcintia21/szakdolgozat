using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartDEMO : MonoBehaviour
{
    // Start is called before the first frame update
    public void LoadGame() {
        SceneManager.LoadScene(20);
    }
}

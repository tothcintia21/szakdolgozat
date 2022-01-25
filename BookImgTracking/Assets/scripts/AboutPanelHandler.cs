using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AboutPanelHandler : MonoBehaviour
{
   public GameObject Panel;

   public void ClosePanel(){
        if(Panel != null){

        bool isActive = Panel.activeSelf;

        Panel.SetActive(!isActive);
        }
    }
}

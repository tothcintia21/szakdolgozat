using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetTaskText: MonoBehaviour
{
    public AudioSource waveTaskSound = null;
    public AudioSource squareTaskSound = null;
    public AudioSource starTaskSound = null;
    public AudioSource moonTaskSound = null;
    private string getTextForTask(string prefabName) { 
        string character = prefabName.ToCharArray()[0].ToString() + prefabName.ToCharArray()[1].ToString();
        return character;
    }

    public void playTaskDescriptiveText(string prefabName) {
        switch (this.getTextForTask(prefabName))
        {
            case "m_":
                this.moonTaskSound.Play();
                break;
            case "s_":
                this.squareTaskSound.Play();
                break;
            case "st":
                this.starTaskSound.Play();
                break;
            case "w_":
                this.waveTaskSound.Play();
                break;
        }
    }
}

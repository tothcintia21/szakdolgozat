using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundManager: MonoBehaviour
{
    public AudioSource[] test1 = null;

    public void playSound(bool isCorrect) {
        //if (isCorrect)
        
        var randNumber = this.randInt(3);
        test1[randNumber].Play();

        //else
    }

    public int randInt(int intervalEnd) {
        int randomNumber = Random.Range(0, intervalEnd);
        return randomNumber;
    }
}

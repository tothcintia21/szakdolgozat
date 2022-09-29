using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundManager: MonoBehaviour
{
    public AudioSource[] soundsOfGoodWork = null;
    public AudioSource[] soundsOfNeedToEncourage = null;

    public void playSound(bool isCorrect) {
       
        if (isCorrect)
        {
            var randNumber = this.randInt(this.soundsOfGoodWork.Length);
            soundsOfGoodWork[randNumber].Play();
        }
        else {
            var randNumber = this.randInt(this.soundsOfNeedToEncourage.Length);
            soundsOfNeedToEncourage[randNumber].Play();
        }
    }

    public int randInt(int intervalEnd) {
        int randomNumber = Random.Range(0, intervalEnd);
        return randomNumber;
    }
}

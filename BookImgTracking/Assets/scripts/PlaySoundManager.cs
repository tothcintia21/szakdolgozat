using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundManager : MonoBehaviour
{
    public AudioSource[] test1;

    public void playSound(bool isCorrect) {
        if (isCorrect)
        {
            for (int i = 0; i < test1.Length; i++) { 
                test1[i].Stop();
            }

            var randNumber = this.randInt(3);
            test1[randNumber].Play();
        }
    }

    public int randInt(int intervalEnd) {
        int randomNumber = Random.Range(0, intervalEnd);
        return randomNumber;
    }
}

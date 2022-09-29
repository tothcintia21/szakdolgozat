using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoSceneSoundManager : MonoBehaviour
{
    //4mp
    public AudioSource introSound = null;

    //10mp
    public AudioSource structureSound = null;

    //7mp
    public AudioSource shapesDisplay = null;

    //5mp
    public AudioSource howToDisplayShapes = null;
    
    //11mp
    public AudioSource ruleSound = null;
    
    //9mp
    public AudioSource informationFromDiff = null;
    
    //3mp
    public AudioSource introduceTheScreen = null;
    
    //11mp
    public AudioSource houseSound = null;
    
    //8mp
    public AudioSource questionmarkSound = null;
    
    //14mp
    public AudioSource howToPlaySound = null;
    
    //3mp
    public AudioSource howToInteractSound = null;
    
    //1mp
    public AudioSource goodWorkSound = null;
    
    //7mp
    public AudioSource howToSolveSound = null;
    
    //10mp
    public AudioSource checkTaskSound = null;
    
    //2mp
    public AudioSource endOfIntroSound = null;

    public void soundOfInfromationFromDiff()
    {
        this.informationFromDiff.Play();
    }

    public void soundOfScreenIntroductory() {
        this.introduceTheScreen.Play();    
    }

    public void soundOfHowToDisplayShapes() {
        this.howToDisplayShapes.Play();
    }

    public void soundOfDisplayShapes()
    {
        this.shapesDisplay.Play();
    }
    public void playIntro()
    { 
        this.introSound.Play();
    }
     public void playSoundOfStructure()
    { 
        this.structureSound.Play();
    }
     public void playSoundOfRule()
    { 
        this.ruleSound.Play();
    }
     public void playSoundOfHouse()
    { 
        this.houseSound.Play();
    }
     public void playSoundOfQuestionmark()
    { 
        this.questionmarkSound.Play();
    }
     public void playSoundOfHowToPlay()
    { 
        this.howToPlaySound.Play();
    }
     public void playSoundOfHowToInteract()
    { 
        this.howToInteractSound.Play();
    }
     public void playSoundOfGoodWork()
    { 
        this.goodWorkSound.Play();
    }
     public void playSoundOfHowToSolve()
    { 
        this.howToSolveSound.Play();
    }
     public void playSoundOfCheckTask()
    { 
        this.checkTaskSound.Play();
    }
     public void playSoundOfEndOfIntro()
    { 
        this.endOfIntroSound.Play();
    }

}

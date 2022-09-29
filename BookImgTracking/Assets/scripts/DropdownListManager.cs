using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DropdownListManager : MonoBehaviour
{
    public TMP_Dropdown dificultyDropdownList;
    public TMP_Dropdown speedDropdownList;
    public TMP_Dropdown posDropdownList;

    public string getDiffDropdownsValue()
    {
        return dificultyDropdownList.options[dificultyDropdownList.value].text; 
    }

    public string getSpeedDropdownsValue()
    {
        return speedDropdownList.options[speedDropdownList.value].text;
    }

    public string getPosDropdownsValue()
    {
        return posDropdownList.options[posDropdownList.value].text;
    }
}

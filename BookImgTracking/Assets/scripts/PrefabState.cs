using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabState : MonoBehaviour
{
    [SerializeField] public bool isSelected = false;
    [SerializeField] public bool thisIsTheGoodSolution = false;
    [SerializeField] public Color originalColor = Color.black;
    public Vector3 originalPosition;
}

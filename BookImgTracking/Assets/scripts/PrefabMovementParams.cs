using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabMovementParams : MonoBehaviour
{
    [SerializeField] public float speed = 3;
    [SerializeField] public Vector3 startPos = new Vector3(-2, 0, 0);
    [SerializeField] public Vector3 endPos = new Vector3(2, 0, 0);
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchObjectController : MonoBehaviour
{
    private Vector2 touchPosition = default;

    [SerializeField]
    PrefabState[] prefab;

    [SerializeField]
    private Camera arCamera;

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            touchPosition = touch.position;

            Debug.Log(touchPosition);

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = arCamera.ScreenPointToRay(touch.position);
                RaycastHit hitObject;

                if (Physics.Raycast(ray, out hitObject))
                {
                    
                    PrefabState prefabState = hitObject.transform.GetComponent<PrefabState>();

                    if (prefabState != null)
                    {
                        Debug.Log("nem null!");
                        ChangeSelectedObject(prefabState);
                    }
                    else
                    {
                        Debug.Log("null");
                    }
                }
            }
        }
    }

    void ChangeSelectedObject(PrefabState selected)
    {
        
        foreach (PrefabState current in prefab)
        {
            MeshRenderer meshRenderer = current.GetComponent<MeshRenderer>();
            if (selected != current)
            {
                current.isSelected = false;
                meshRenderer.material.color = Color.gray;
            }
            else
            {
                current.isSelected = true;
                meshRenderer.material.color = Color.green;
            }
        }
    }
}

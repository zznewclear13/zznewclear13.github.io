using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRay : MonoBehaviour
{
    private GameObject currentKey;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit raycastHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out raycastHit))
            {
                if(raycastHit.transform != null)
                {
                    OnClickGameObject(raycastHit.transform.gameObject);
                }
            }
        }
        */

        if(Input.GetMouseButton(0))
        {
            RaycastHit raycastHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out raycastHit))
            {
                if (raycastHit.transform != null)
                {
                    if(raycastHit.transform.gameObject != currentKey)
                    {
                        //按到了新的按键
                        StopHoldingGameObject(currentKey);
                        OnHoldGameObject(raycastHit.transform.gameObject);
                        currentKey = raycastHit.transform.gameObject;
                    }
                }
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            StopHoldingGameObject(currentKey);
            currentKey = null;
        }

    }

    private void OnClickGameObject(GameObject targetGameObject)
    {
        PlaySound playSound = targetGameObject.GetComponent<PlaySound>();
        if (playSound != null)
        {
            playSound.Play();
        }
    }

    private void OnHoldGameObject(GameObject targetGameObject)
    {
        PlaySound playSound = targetGameObject.GetComponent<PlaySound>();
        if (playSound != null)
        {
            playSound.OnHold();
        }
    }

    private void StopHoldingGameObject(GameObject targetGameObject)
    {
        if(targetGameObject != null)
        {
            PlaySound playSound = targetGameObject.GetComponent<PlaySound>();
            if (playSound != null)
            {
                playSound.StopHolding();
            }
        }
    }

}

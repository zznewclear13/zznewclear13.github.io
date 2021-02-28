using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraRay : MonoBehaviour
{

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                RaycastHit raycastHit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out raycastHit))
                {
                    if (raycastHit.transform != null)
                    {
                        OnClickGameObject(raycastHit.transform.gameObject);
                    }
                }
            }
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


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARCameraTouch : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        InputTouch();
    }

    private static void InputTouch()
    {
        RaycastHit raycast_touch = new RaycastHit();
        for (int i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetTouch(i).phase.Equals(TouchPhase.Began))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
                if (Physics.Raycast(ray, out raycast_touch))
                {
                    raycast_touch.transform.gameObject.SendMessage("OnMouseDown");
                }

            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatingBG : MonoBehaviour
{
    public Transform resetPoint;
    public Transform startPoint;
 
	// Update is called once per frame
	void Update ()
    {
        if (transform.position.y < resetPoint.position.y)
        {
            RepositionBackground();
        }	
	}

    private void RepositionBackground()
    {
        this.transform.position = startPoint.position;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxBoundDebugMode : MonoBehaviour {

    public GameObject left;
    public GameObject right;
    public GameObject top;
    public GameObject bottom;
        

	void Start () {
        if (!LaunchPlayerShip.isPlayerShipDebugMode)
        {
            left.SetActive(false);
            right.SetActive(false);
            top.SetActive(false);
            bottom.SetActive(false);
        }
	}
	

}

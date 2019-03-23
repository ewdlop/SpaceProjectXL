using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLaserBeam : MonoBehaviour {

    public GameObject playership;

	void LateUpdate ()
    {
        if (playership != null)
        {
            float playShipFacingAngle = playership.transform.eulerAngles.z;
            transform.position = new Vector3(playership.transform.position.x, playership.transform.position.y, -0.03f) 
                + 0.78f * new Vector3(Mathf.Cos((playShipFacingAngle + 90f) * Mathf.Deg2Rad), Mathf.Sin((playShipFacingAngle + 90f) * Mathf.Deg2Rad), 0f);
            transform.eulerAngles = new Vector3(playShipFacingAngle - 90f, -90f, 90f);
        }
        
	}
}

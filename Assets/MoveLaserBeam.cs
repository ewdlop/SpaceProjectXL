using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLaserBeam : MonoBehaviour {

    public GameObject playership;
	void LateUpdate () {
        transform.position = new Vector3(playership.transform.position.x, playership.transform.position.y + 0.7759998f, -0.03f);
        if (playership == null)
        {
            gameObject.SetActive(false);
        }
	}
}

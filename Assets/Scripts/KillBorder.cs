using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBorder : MonoBehaviour {

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag != "BossLaser")
            Destroy(other.gameObject);
    }

}

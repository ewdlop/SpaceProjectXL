using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour {


    public float duration;

	void Start () {
        Destroy(this.gameObject, duration);
        
    }
	


}

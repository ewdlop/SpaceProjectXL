using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : MonoBehaviour {

    public float speed = 15.0f;

    public abstract void movement(GameObject movingObject, GameObject targetObject);
}

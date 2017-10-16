using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : Movement {

    // Object will charge at a target

    public override void movement(GameObject movingObject, GameObject targetObject)
    {
        Vector2 direction = targetObject.transform.position - movingObject.transform.position;

        float facingAngle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        movingObject.transform.eulerAngles = new Vector3(0, 0, facingAngle + 90f);

        movingObject.GetComponent<Rigidbody2D>().velocity =
            speed * new Vector2(1.0f/direction.x, 1.0f/direction.y);
    }
}

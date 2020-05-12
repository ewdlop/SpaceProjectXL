using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatCameraeInMainMenu : MonoBehaviour
{

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        float range = Random.Range(0f, 1f);
        if(range > 0.5f)
            rb.AddTorque(new Vector3(0f, 1f, 0f));
        else
            rb.AddTorque(new Vector3(0f, -1f, 0f));

    }

}

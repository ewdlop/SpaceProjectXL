using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddGravity : MonoBehaviour
{

    Rigidbody2D PlanetRigiBbody;


    public GameObject initGravity;
    public float acceleration;
    public bool isPlanetFixed;
    
    float gravitationalConstant = 0;

    void Start()
    {
        initGravity = GameObject.Find("InitGravity");
        gravitationalConstant = initGravity.GetComponent<InitGravity>().gravitationalConstant;
        //add this planet to the plaents list
        initGravity.GetComponent<InitGravity>().planets.Add(gameObject);
        //need to use Rigidbody in order to use AddForce().
        PlanetRigiBbody = GetComponent<Rigidbody2D>();
    }


    void Update()
    {

        if (LaunchPlayerShip.isPlayerShipLaunched&& !isPlanetFixed)//disable gravity among planets;
        {
            //sum over all accerelation due to other planets
            //For the math:
            //Rigidboy2D wont allow Z rip;
            //https://physics.stackexchange.com/questions/17285/split-gravitational-force-into-x-y-and-z-componenets
            foreach (GameObject planet in initGravity.GetComponent<InitGravity>().planets)
            {
                if (gameObject != planet)
                {
                    float mass = planet.GetComponent<Rigidbody2D>().mass;
                    float gameObjectMass = GetComponent<Rigidbody2D>().mass;
                    float xDifference = planet.transform.position.x - gameObject.transform.position.x;
                    float yDifference = planet.transform.position.y - gameObject.transform.position.y;

                    float distance = Mathf.Sqrt(Mathf.Pow(xDifference, 2) + Mathf.Pow(yDifference, 2));
                    float distancePower3 = Mathf.Pow(distance, 3);

                    Vector3 gravity = new Vector2(gravitationalConstant * mass * xDifference / distancePower3, gravitationalConstant * mass * yDifference / distancePower3);
                    acceleration = gravity.magnitude;
                    //there is no acceleration for 2DForceMode rip
                    PlanetRigiBbody.AddForce(new Vector2(gameObjectMass * gravity.x, gameObjectMass * gravity.y), ForceMode2D.Impulse);
                }

            }
        }

    }
}
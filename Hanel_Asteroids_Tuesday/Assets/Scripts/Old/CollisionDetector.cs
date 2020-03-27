using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour {


	// Use this for initialization
	void Start () {
		//zoop
	}
	
	// Update is called once per frame
	void Update () {
        //zoop
	}

    public bool CircleCollision(GameObject ship, GameObject planet)
    {
        //radius of spaceship circle
        float sRadius = (ship.GetComponent<SpriteRenderer>().bounds.size.x) / 2;

        //radius of planet circle
        float pRadius = (planet.GetComponent<SpriteRenderer>().bounds.size.x) / 2;

        //distance between the two
        float dist = Vector3.Distance(ship.GetComponent<Transform>().position, planet.GetComponent<Transform>().position);

        //if the distance is less than the addition of the radii of the two objects, they must be touching
        if (dist < (sRadius + pRadius))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
}

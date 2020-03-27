using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTest : MonoBehaviour {

    public GameObject ship = new GameObject();

    CollisionDetector detect;
    AsteroidGenerator ag;

    List<GameObject> asteroids = new List<GameObject>();


    // Use this for initialization
    void Start () {
        detect = this.GetComponent<CollisionDetector>();
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        Circle();
        asteroids = ag.getAsteroids();
    }

    //method for activating circle collider
    private void Circle()
    {
        for(int i = 0; i < asteroids.Count; i++)
        {
            if (detect.CircleCollision(ship, asteroids[i]))
            {
                ship.GetComponent<SpriteRenderer>().color = Color.red;
                asteroids[i].GetComponent<SpriteRenderer>().color = Color.red;
            }
            else
            {
                ship.GetComponent<SpriteRenderer>().color = Color.white;
                asteroids[i].GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
        
    }


}

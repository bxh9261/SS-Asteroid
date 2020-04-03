using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Deceleration, wrap, immortality, and respawn added by Brad Hanel, the rest by IGME 202
 * Ship.cs: Controls movements of the ship
 */
public class Ship : MonoBehaviour
{

    // Unnecessary
    //public float speed;                       // Speed of the vehicle, not needed anymore

    // Necessary
    public float accelRate;                     // Small, constant rate of acceleration
    public float decelRate;                     // for deceleration, should be less than one
    public Vector3 vehiclePosition;             // Local vector for movement calculation
    public Vector3 direction;                   // Way the vehicle should move
    public Vector3 velocity;                    // Change in X and Y
    public Vector3 acceleration;                // Small accel vector that's added to velocity
    //public Vector3 deceleration;
    public float angleOfRotation;               // 0 
    public float maxSpeed;                      // 0.5 per frame, limits mag of velocity

    public GameObject shipSprite;
    AsteroidManager am;
    SceneManager manage;

    public bool immortal;

	// Use this for initialization
	void Start ()
    {
        //instantiating stuff
        am = GameObject.Find("Scene Manager").GetComponent<AsteroidManager>();
        manage = GameObject.Find("Scene Manager").GetComponent<SceneManager>();

        vehiclePosition = new Vector3(0, 0, 0);     // Or you could say Vector3.zero
        direction = new Vector3(1, 0, 0);           // Facing right
        velocity = new Vector3(0, 0, 0);            // Starting still (no movement)

        //makes a ship. If you haven't noticed, nothing is dragged on to the screen, things are entirely instantiated by code
        GameObject ship = Instantiate(shipSprite, vehiclePosition, Quaternion.Euler(0,0,90));
        manage.ship = ship;

        //make immortal for 3 seconds
        StartCoroutine(MakeImmortal());

        if (decelRate >= 1)
        {
            Debug.Log("Deceleration rate greater than one, strange results expected.");
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        RotateVehicle();

        Drive();

        SetTransform();

        Wrap();
        
    }

    /// <summary>
    /// Changes / Sets the transform component
    /// </summary>
    public void SetTransform()
    {
            // Rotate vehicle sprite
            manage.ship.transform.rotation = Quaternion.Euler(0, 0, angleOfRotation);

            // Set the transform position
            manage.ship.transform.position = vehiclePosition;
        
    }

    /// <summary>
    /// 
    /// </summary>
    public void Drive()
    {
        // Accelerate
        // Small vector that's added to velocity every frame
        acceleration = accelRate * direction;


        // We used to use this, but acceleration will now increase the vehicle's "speed"
        // Velocity will remain intact from one frame to the next
        //velocity = direction * speed;             // Unnecessary
        if (Input.GetKey(KeyCode.UpArrow))  //I like WASD
        {
            velocity += acceleration;
        }
        else
        {
            velocity = velocity * decelRate;
        }

        // Limit velocity so it doesn't become too large
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        // Add velocity to vehicle's position
        vehiclePosition += velocity;

        //Debug.Log(vehiclePosition);
    }

    public void RotateVehicle()
    {
        // Player can control direction
        // Left arrow key = rotate left by 2 degrees
        // Right arrow key = rotate right by 2 degrees
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            angleOfRotation += 2;
            direction = Quaternion.Euler(0, 0, 2) * direction;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            angleOfRotation -= 2;
            direction = Quaternion.Euler(0, 0, -2) * direction;
        }
    }

    public void Wrap()
    {
        // if car goes past the camera bounds, it emerges on the opposite side of the screen

        //https://answers.unity.com/questions/230190/how-to-get-the-width-and-height-of-a-orthographic.html
        Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;


        //if right, go to left side
        if(vehiclePosition.x >= (width/2))
        {
            vehiclePosition.x = -width/2;
        }

        //if left, go to right side
        if(vehiclePosition.x < -width / 2)
        {
            vehiclePosition.x = (width / 2);
        }

        //if top, go bottom
        if (vehiclePosition.y >= (height / 2))
        {
            vehiclePosition.y = -height/ 2;
        }

        //if bottom, go top
        if (vehiclePosition.y < -height / 2)
        {
            vehiclePosition.y = (height / 2);
        }
    }

    //respawn
    public void ResetShip()
    {

        vehiclePosition = new Vector3(0, 0, 0);     // Or you could say Vector3.zero
        //direction = new Vector3(1, 0, 0);           // Facing right -- ONLY DO AT START
        velocity = new Vector3(0, 0, 0);            // Starting still (no movement)

        Destroy(manage.ship);

        GameObject ship = Instantiate(shipSprite, vehiclePosition, Quaternion.Euler(0, 0, 90));
        manage.ship = ship;
        //shipSprite = AddComponent<Ship>;

        StartCoroutine(MakeImmortal());
    }


    //ship is immortal for 3 seconds on spawn/respawn to avoid spawning directly on an asteroid and dying
    public IEnumerator MakeImmortal()
    {
        //https://docs.unity3d.com/ScriptReference/WaitForSeconds-ctor.html
        immortal = true;
        manage.ship.GetComponent<SpriteRenderer>().color = Color.green;
        manage.aud[3].Play();
        yield return new WaitForSeconds(1f);
        manage.ship.GetComponent<SpriteRenderer>().color = Color.yellow;
        manage.aud[3].Play();
        yield return new WaitForSeconds(1f);
        manage.ship.GetComponent<SpriteRenderer>().color = Color.red;
        manage.aud[3].Play();
        yield return new WaitForSeconds(1f);
        immortal = false;
        manage.ship.GetComponent<SpriteRenderer>().color = Color.white;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidManager : MonoBehaviour {

    //for controlling asteroid position
    public Vector3 position;
    public float speed;
    public Vector3 velocity;
    public float rotation;

    //for giving asteroids different sprites
    public List<GameObject> asteroidSprites;
    public List<GameObject> babyAsteroidSprites;

    //accessing scene manager and Ship variables/methods
    SceneManager manage;
    Ship ship;

    //number of lives is here, just since most collisions also happen here
    public int lives;

    //velocities of asteroids
    List<Vector3> velocities;
    List<Vector3> babyVelocities;

    


    // Use this for initialization
    void Start () {
        //instantiating stuff yay
        manage = GameObject.Find("Scene Manager").GetComponent<SceneManager>();
        ship = GameObject.Find("Scene Manager").GetComponent<Ship>();
        velocities = new List<Vector3>();
        babyVelocities = new List<Vector3>();

        lives = 3;

        //there are always 5 big asteroids on screen (except for 3 second respawn time)
        for(int i = 0; i < 5; i++)
        {
            AsteroidSpawner(i);
        }

	}
	
	// Update is called once per frame
	void Update () {
        //I use circle collision since the objects rotate or are round
        Circle();

        //for loop for moving & wrapping the big asteroids
        for(int i = 0; i < manage.asteroids.Count; i++)
        {
            if (manage.asteroids[i] != null)
            {
                SetTransform(manage.asteroids[i], i);
                Wrap(manage.asteroids[i]);
            }
                
        }

        //for loops for moving & wrapping the lil asteroids
        for(int j = 0; j < manage.babyAsteroids.Count; j++)
        {
            if(manage.babyAsteroids[j] != null)
            {
                BabySetTransform(manage.babyAsteroids[j], j);
                Wrap(manage.babyAsteroids[j]);
            }
            
        }
	}

    //moves asteroids
    public void SetTransform(GameObject ast, int i)
    {
        // Set the transform position
        ast.transform.position += velocities[i];
    }

    //moves lil asteroids
    public void BabySetTransform(GameObject ast, int j)
    {
        // Set the transform position
        ast.transform.position += babyVelocities[j];
    }

    //method for detecting circle collision, same one made for collisions practice exercise
    public bool CircleCollision(GameObject shipObj, GameObject planet)
    {
        //I didn't like the false collisions on the ship with dividing by 2 so I divided by 3 instead

        //third of circumference of ship
        float sRadius = (shipObj.GetComponent<SpriteRenderer>().bounds.size.x) / 3;

        //radius of asteroid
        float pRadius = (planet.GetComponent<SpriteRenderer>().bounds.size.x) / 2;

        //distance between the two
        float dist = Vector3.Distance(shipObj.GetComponent<Transform>().position, planet.GetComponent<Transform>().position);

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

    //I'll admit this is a bit of a mess
    /*
     * For collisions between:
     * 1. Ship and big asteroid
     * 2. Ship and lil asteroid
     * 3. Bullet and big asteroid
     * 4. Bullet and lil asteroid
     */ 
    private void Circle()
    {
        //goes through asteroids checking for bullet or ship collisions
        for (int i = 0; i < manage.asteroids.Count; i++)
        {

            if (manage.asteroids[i] != null)
            {
                if (!ship.immortal)
                {
                    //ship vs big asteroid
                    if (CircleCollision(manage.ship, manage.asteroids[i]))
                    {
                        lives--;
                        //Debug.Log(lives);
                        ship.ResetShip();
                    }


                }
                
                //big asteroid vs bullet
                for (int j = 0; j < manage.bullets.Count; j++)
                {
                    if(manage.bullets[j] != null)
                    {
                        if (CircleCollision(manage.bullets[j], manage.asteroids[i]))
                        {
                            //boom sound
                            manage.aud[0].Play();

                            //make points
                            manage.Points += 20;
                            //Debug.Log(manage.Points);

                            Destroy(manage.bullets[j]);
                            //make some baby asteroids
                            GameObject b1 = Instantiate(babyAsteroidSprites[i], manage.asteroids[i].transform.position, manage.asteroids[i].transform.rotation);
                            GameObject b2 = Instantiate(babyAsteroidSprites[i], manage.asteroids[i].transform.position, manage.asteroids[i].transform.rotation);

                            //add them to list
                            manage.babyAsteroids.Add(b1);
                            manage.babyAsteroids.Add(b2);

                            //baby asteroid velocity in same general direction of old asteroid
                            Vector3 tempB1 = new Vector3(velocities[i].x - 0.01f, velocities[i].y + 0.01f, velocities[i].z);
                            Vector3 tempB2 = new Vector3(velocities[i].x + 0.01f, velocities[i].y - 0.01f, velocities[i].z);

                            //add to velocity list
                            babyVelocities.Add(tempB1);
                            babyVelocities.Add(tempB2);

                            //destroy old asteroid
                            Destroy(manage.asteroids[i]);

                            //make more asteroids come in after a certain amount of time
                            StartCoroutine(SpawnNewAsteroid(manage.asteroids[i], i));

                        }
                    }
                    
                }
            }
        }

        //loop through baby asteroids looking for collisions
        for(int k = 0; k < manage.babyAsteroids.Count; k++)
        {
            if (manage.babyAsteroids[k] != null)
            {
                if (!ship.immortal)
                {
                    //ship vs baby asteroid
                    if (CircleCollision(manage.ship, manage.babyAsteroids[k]))
                    {
                        lives--;
                        //Debug.Log(lives);
                        ship.ResetShip();
                    }


                }

                //baby asteroid vs bullet
                for (int m = 0; m < manage.bullets.Count; m++)
                {
                    if(manage.bullets[m] != null)
                    {
                        if (CircleCollision(manage.bullets[m], manage.babyAsteroids[k]))
                        {
                            //little boom
                            manage.aud[1].Play();

                            //kill bullet, kill asteroid
                            Destroy(manage.bullets[m]);
                            Destroy(manage.babyAsteroids[k]);
                            manage.Points += 50;
                            //Debug.Log(manage.Points);
                        }
                    }
                    
                }
            }
                
        }

    }
     
    public void AsteroidSpawner(int index)
    {
        //This is copy-pasted a few times in the code, but it crashed when I tried to make it global
        //https://answers.unity.com/questions/230190/how-to-get-the-width-and-height-of-a-orthographic.html
        Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;

        //random position
        position = new Vector3(Random.Range(-width / 2, width / 2), Random.Range(-height / 2, height / 2), 0);

        //random rotation
        rotation = Random.Range(0, 2 * Mathf.PI);

        //make asteroid, add it to list
        GameObject ast = Instantiate(asteroidSprites[index], position, Quaternion.Euler(0, 0, rotation));
        manage.asteroids.Add(ast);

        //speed gets faster for each level
        speed = 0.02f*manage.Level;
        velocity = new Vector3(Mathf.Cos(rotation) * speed, Mathf.Sin(rotation) * speed, 0); 

        //velocity list
        velocities.Add(velocity);
    }

    public void Wrap(GameObject ast)
    {
        // if car goes past the camera bounds, it emerges on the opposite side of the screen

        //https://answers.unity.com/questions/230190/how-to-get-the-width-and-height-of-a-orthographic.html
        Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;


        //if right, go to left side
        if (ast.transform.position.x >= (width / 2))
        {
            ast.transform.position = new Vector3(-width / 2, ast.transform.position.y, ast.transform.position.z);
        }

        //if left, go to right side
        if (ast.transform.position.x < -width / 2)
        {
            ast.transform.position = new Vector3(width / 2, ast.transform.position.y, ast.transform.position.z);
        }

        //if top, go bottom
        if (ast.transform.position.y >= (height / 2))
        {
            ast.transform.position = new Vector3(ast.transform.position.x, -height/2, ast.transform.position.z);
        }

        //if bottom, go top
        if (ast.transform.position.y < -height / 2)
        {
            ast.transform.position = new Vector3(ast.transform.position.x, height / 2, ast.transform.position.z);
        }
    }


    //after 3 seconds, new asteroid spawns
    public IEnumerator SpawnNewAsteroid(GameObject ast, int index)
    {
        
        //https://answers.unity.com/questions/230190/how-to-get-the-width-and-height-of-a-orthographic.html
        Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;


        //https://docs.unity3d.com/ScriptReference/WaitForSeconds-ctor.html
        yield return new WaitForSeconds(3f);

        //they spawn on the edge, to create the illusion that they're coming in from the top
        position = new Vector3(Random.Range(-width / 2, width / 2), height / 2, 0);
        rotation = Random.Range(0, 2 * Mathf.PI);
        manage.asteroids[index] = Instantiate(asteroidSprites[index], position, Quaternion.Euler(0, 0, rotation));

    }

    //speed up every level. music gets pitched up too.
    public void SpeedUP()
    {
        for(int i = 0; i < 5; i++)
        {
            velocities[i] = velocities[i] * 1.4f;
            manage.aud[4].pitch += 0.01f;
        }
    }

}

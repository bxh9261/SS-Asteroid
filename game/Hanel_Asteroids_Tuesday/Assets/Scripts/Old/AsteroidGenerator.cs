using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidGenerator : MonoBehaviour
{

    public GameObject asteroid;
    public List<GameObject> asteroids;

    // Start is called before the first frame update
    void Start()
    {
         AsteroidSpawner(); // I originally accidentally put this in update and oh boy that was a journey
    }

    // Update is called once per frame
    void Update()
    {
       //zoop
    }

    public void AsteroidSpawner()
    {
        Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;

        for (int i = 0; i < 5; i++)
        {
            Instantiate(asteroid, new Vector3(Random.Range(-width / 2, width / 2), Random.Range(-height / 2, height / 2), 0), Quaternion.identity);
            asteroids.Add(asteroid);
        }
    }

    public List<GameObject> getAsteroids()
    {
        return asteroids;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour {

    //bullet locations and movements
    public Vector3 position;
    public float speed;
    public Vector3 velocity;
    public float bulletRotation;

    public GameObject bulletSprite;

    //bullet velocities
    List<Vector3> velocities;

    //communicating with other objects. "Hi other objects!" the bullet says.
    Ship s;
    SceneManager manage;

    //timing mechanism
    bool canShoot;

    // Use this for initialization
    void Start () {
        manage = GameObject.Find("Scene Manager").GetComponent<SceneManager>();
        s = GameObject.Find("Scene Manager").GetComponent<Ship>();

        velocities = new List<Vector3>();

        canShoot = true;
    }
	
	// Update is called once per frame
	void Update () {

        //see comment for spawnbullets
        SpawnBullets();
        
        //move bullets
        for(int i = 0; i < manage.bullets.Count; i++)
        {
            if(manage.bullets[i] != null)
            {
                DoNotWrap(manage.bullets[i]);
                manage.bullets[i].transform.position += velocities[i];
            }
        }
        

	}

    //spawns bullets when player shoots
    void SpawnBullets()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (canShoot)
            {
                //pew
                manage.aud[2].Play();

                //make bullet
                bulletRotation = s.angleOfRotation - 90;
                GameObject bullet = Instantiate(bulletSprite, manage.ship.transform.position, Quaternion.Euler(0, 0, bulletRotation));
                manage.bullets.Add(bullet);

                //MATH
                velocity = new Vector3(Mathf.Cos(bulletRotation * (Mathf.PI / 180) + (Mathf.PI / 2)) * 0.25f, Mathf.Sin(bulletRotation * (Mathf.PI / 180) + (Mathf.PI / 2)) * 0.25f, 0);
                velocities.Add(velocity);

                //timing mechanism
                canShoot = false;
                StartCoroutine(Wait());

            }

        }
        

    }



     public void DoNotWrap(GameObject bull)
    {
        // if car goes pbull the camera bounds, it emerges on the opposite side of the screen

        //https://answers.unity.com/questions/230190/how-to-get-the-width-and-height-of-a-orthographic.html
        Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;


        //before I had this, there were so many bullets floating around in nowhere that unity crashed
        //it was a time
        if (bull.transform.position.x >= (width / 2) ||
            (bull.transform.position.x < -width / 2) ||
            (bull.transform.position.y >= (height / 2)) ||
            (bull.transform.position.y < -height / 2))
        {
            Destroy(bull);
        }
    }

    //bullet timing mechanism
    public IEnumerator Wait()
    {
        //https://docs.unity3d.com/ScriptReference/WaitForSeconds-ctor.html
        yield return new WaitForSeconds(0.5f);
        canShoot = true;
    }


}

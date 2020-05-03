using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;
using FMOD.Studio;

public class StoryManager : MonoBehaviour {

    //boxes for text
    public GUIStyle johnPetrov;
    public GUIStyle speakerName;

    const int DIST_FROM_SIDE = 75;

    //for progressing through the story
    int enterPresses;

    //for stars, create illusion ship is moving
    public GameObject starSprite;
    Vector3 position;
    Vector3 velocity;
    public List<GameObject> stars;
    GameObject star;

    //some rectangles for text boxes
    Rect textRec;
    Rect nameRec;

    //opening title screen words
    public GUIStyle tScreen;
    public GUIStyle smallerText;
    
    //the ship. doesn't have movement control yet
    GameObject ship;

    //holds the story and who says what. I originally did this as a switch statement and it was really long and stupid
    string[] script;
    string[] names;

    #region audio
    //music and bloop sound
    public AudioSource[] sounds;
    FMOD.Studio.EventInstance dialogue;
    FMOD.Studio.EventInstance proceed;
    FMOD.Studio.EventInstance skip;
    FMOD.Studio.EventInstance dialogueSnap;
    GameObject music;

    //FMOD
    [Range(0.0f, 1.0f)]
    public float volume;
    #endregion


    // Use this for initialization
    void Start ()
    {
        //initializing
        dialogue = FMODUnity.RuntimeManager.CreateInstance("event:/Dialogue/Dialogue");
        dialogueSnap = FMODUnity.RuntimeManager.CreateInstance("snapshot:/Dialogue Playing");
        proceed = FMODUnity.RuntimeManager.CreateInstance("event:/FX/Proceed");
        skip = FMODUnity.RuntimeManager.CreateInstance("event:/FX/Skip");
        sounds = GetComponents<AudioSource>();
        music = GameObject.Find("Music");
        textRec = new Rect(DIST_FROM_SIDE, Screen.height / 2 + DIST_FROM_SIDE, Screen.width - DIST_FROM_SIDE * 2, Screen.height / 2 - DIST_FROM_SIDE * 3);
        nameRec = new Rect(DIST_FROM_SIDE, Screen.height / 2, Screen.width / 4, DIST_FROM_SIDE);
        stars = new List<GameObject>();

        //makes script
        GetScript();

        //get ship
        ship = GameObject.Find("Ship");

        //first one is free
        enterPresses = -1;
        
        //for some movement illusion
        SpawnStars();
        velocity = new Vector3(Mathf.Cos(5 * Mathf.PI/4) * 0.3f, Mathf.Sin(5 * Mathf.PI / 4) * 0.3f, 0);
    }
	
	// Update is called once per frame
	void Update ()
    {
        //for some movement illusion
        for (int i = 0; i < 30; i++)
        {
            ShipMovementIllusion(stars[i]);
            Wrap(stars[i]);
        }
            
        //for progressing the story
        if (Input.GetKeyDown(KeyCode.Return))
        {
            enterPresses++;
            if (enterPresses >= 0)
            {
                dialogue.setParameterByName("DialogueC", enterPresses);
                dialogueSnap.start();
            }
            StartCoroutine(Dialogue());
        }

        //for skipping the story
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            StartCoroutine(Skip());
        }
	}

    public IEnumerator Dialogue()
    {
        proceed.start();
        //sound
        yield return new WaitForSeconds(0.2f);
        dialogue.start();
    }

    public IEnumerator Skip()
    {
        skip.start();
        yield return new WaitForSeconds(0.5f);
        dialogue.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        dialogueSnap.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }

    //make 3 random stars
    public void SpawnStars()
    {
        //here's this again
        //https://answers.unity.com/questions/230190/how-to-get-the-width-and-height-of-a-orthographic.html
        Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;



        for (int i = 0; i < 30; i++)
        {
            position = new Vector3(Random.Range(-width / 2, width / 2), Random.Range(-height / 2, height / 2), 0);
            star = Instantiate(starSprite, position, Quaternion.identity);
            stars.Add(star);
            //Debug.Log(star);
        }
    }



    //move the stars
    public void ShipMovementIllusion(GameObject str)
    {
        
            str.transform.position += velocity;
    }

    //wrap the stars
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
            ast.transform.position = new Vector3(ast.transform.position.x, -height / 2, ast.transform.position.z);
        }

        //if bottom, go top
        if (ast.transform.position.y < -height / 2)
        {
            ast.transform.position = new Vector3(ast.transform.position.x, height / 2, ast.transform.position.z);
        }
    }


    //progress through the story
    private void OnGUI()
    {
        //title screen
        if(enterPresses == -1)
        {
            ship.transform.position = new Vector3(0, 0, 0);
            GUI.Box(new Rect((Screen.width) / 2 - (Screen.width) / 8, (Screen.height) / 16, (Screen.width) / 4, (Screen.height) / 4), "ASTEROIDS", tScreen);
            GUI.Box(new Rect((Screen.width) / 2 - (Screen.width) / 8, 13 * (Screen.height) / 16, (Screen.width) / 4, (Screen.height) / 4), "Created by Brad Hanel\nPress ENTER to proceed", smallerText);
        }

        //22 lines in the story
        else if(enterPresses <= 21)
        {
            ship.transform.position = new Vector3(0, 2, 0);
            GUI.Box(textRec, script[enterPresses], johnPetrov);
            GUI.Box(nameRec, names[enterPresses], speakerName);
            GUI.Box(new Rect((Screen.width) / 2 - (Screen.width) / 8, 14 * (Screen.height) / 16, (Screen.width) / 4, (Screen.height) / 4), "ENTER to proceed, ESC to skip", smallerText);
        }

        //after the last line, game starts
        else
        {
            dialogueSnap.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
        }

    }

    //script for the opening story
    public void GetScript()
    {
        script = new string[] 
        {   "Hooray! Another space mission! I’m so excited!",
            "Da, it's very exciting John. Space is very cool, and very big. Almost as big as Russia. ",
            "Uh, Petrov? I think space is actually bigger than Russia.",
            "Don’t say that, John! If you said that in Russia, they would crush your head into Borscht!", //sounds[5]
            "Sorry!",
            "*bzzt bzzt*",
            "Hey guys! This is Steve from mission control! How’s the weather up there?",
            "It’s great! Clear skies!",
            "Da, it's very nice. When do we get to big red planet? ", //10
            "Wait, big red planet? Aren’t you going to Mars? The big orange one? With the spot on it?",
            "That’s Jupiter you moldy pierogi!",
            "Steve, what the heck, how do you work at NASA?",
            "Uhhh…",
            "Wait… was that an asteroid?", //15
            "Oh no! Steve! Steve! We’re going through the asteroid belt!",
            "Uhhh…",
            "Give us something Stephen! ",
            "Steve?", //20
            "I got it! I'm now giving your ship manual controls. Use the UP arrow to accelerate, and the LEFT and RIGHT arrows to rotate. I’ve also given you access to the laser. Use SPACE to shoot it. ",
            "Oh my gosh! We have a LASER? That’s so cool! We got this! Thanks Steve!",
            "Steve, you can program a ship but you don’t know the difference between mars and Jupiter?",
            "Uhhh…. We’re breaking up! Uhhh… bzzt… bzzt… Good luck! Bzzt… bye!"
        };

        names = new string[]
        {   "John",
            "Petrov",
            "John",
            "Petrov", //5
            "John",
            " ",
            "Steve",
            "John",
            "Petrov", //10
            "Steve",
            "Petrov",
            "John",
            "Steve",
            "Petrov", //15
            "John",
            "Steve",
            "Petrov",
            "John", //20
            "Steve",
            "John",
            "Petrov",
            "Steve"
        };
    }



}

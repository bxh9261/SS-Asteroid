using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class GameOver : MonoBehaviour {

    //big "GAME OVER"
    public GUIStyle gmOver;

    //every other piece of text
    public GUIStyle initials;

    //more cool star effect
    public GameObject starSprite;
    Vector3 position;
    Vector3 velocity;
    public List<GameObject> stars;
    GameObject star;

    //this is to avoid a comma in someone's name messing things up. I think I made it so you can't type one, but just in case, you definitely can't type \xE001
    const char DEL = '\xE001';
    List<string> hs;

    //name input
    string initInput;

    //if they've input and pressed enter
    bool input;

    //from game
    int score;

    //for some reason it was double-saving scores. this fixed it.
    bool ranOnce;


    // Use this for initialization
    void Start() {

        //initializing
        score = PlayerPrefs.GetInt("Score", 5);
        hs = new List<string>();
        initInput = "";
        input = false;

        //stars -- same as other scenes
        stars = new List<GameObject>();
        SpawnStars();
        velocity = new Vector3(Mathf.Cos(5 * Mathf.PI / 4) * 0.3f, Mathf.Sin(5 * Mathf.PI / 4) * 0.3f, 0);

        //sets default high scores if no text exists
        defaultHighScores();
    }

    // Update is called once per frame
    void Update() {
        //stars -- same as other scenes
        for (int i = 0; i < 30; i++)
        {
            ShipMovementIllusion(stars[i]);
            Wrap(stars[i]);
        }

        //go back to main menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Story");
        }

    }

    //stars -- same as other scenes
    public void SpawnStars()
    {
        //https://answers.unity.com/questions/230190/how-to-get-the-width-and-height-of-a-orthographic.html
        Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;



        for (int i = 0; i < 30; i++)
        {
            position = new Vector3(Random.Range(-width / 2, width / 2), Random.Range(-height / 2, height / 2), 0);
            star = Instantiate(starSprite, position, Quaternion.identity);
            stars.Add(star);

        }
    }
    //stars -- same as other scenes
    public void ShipMovementIllusion(GameObject str)
    {

        str.transform.position += velocity;
    }
    //stars -- same as other scenes
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

    public void OnGUI()
    {
        Event e = Event.current;

        //Check the type of the current key stroke to make sure it's a letter
        //only 3 initials
        if (initInput.Length < 3)
        {
            if (e.type == EventType.KeyDown &&
            e.keyCode.ToString().Length == 1 &&
            char.IsLetter(e.keyCode.ToString()[0]))
            {
                initInput += e.keyCode;
            }
        }

        //adds spaces if they give less than three letters (crashes otherwise)
        if (Input.GetKeyDown(KeyCode.Return))
        {
            while (initInput.Length < 3)
            {
                initInput += " ";
            }

            //put high score in list
            if (!ranOnce)
            {
                HighScores();
                ranOnce = true;
            }
            
            input = true;
        }

        //initial entering screen for game over
        if (!input)
        {
            GUI.Box(new Rect((Screen.width) / 2 - (Screen.width) / 8, (Screen.height) / 16, (Screen.width) / 4, (Screen.height) / 4), "GAME OVER", gmOver);
            GUI.Box(new Rect((Screen.width) / 2 - (Screen.width) / 8, 13 * (Screen.height) / 16, (Screen.width) / 4, (Screen.height) / 4), "Enter Initials: " + initInput + "\nENTER to submit", initials);
        }

        //shows high scores
        else
        {
            //reads high score document, puts into list on screen
            StreamReader sr = new StreamReader("highscore.txt");
            for (int j = 0; j < 5; j++)
            {
                hs.Add(sr.ReadLine());
            }
            sr.Close();

            GUI.Box(new Rect((Screen.width) / 2 - (Screen.width) / 8, (Screen.height) / 16, (Screen.width) / 4, (Screen.height) / 4), "HIGH SCORES", gmOver);
            GUI.Box(new Rect((Screen.width) / 2 - (Screen.width) / 8, 5 * (Screen.height) / 16, (Screen.width) / 4, (Screen.height) / 4), 
                hs[0].Substring(0, 3) + "..........." + hs[0].Substring(4, hs[0].Length - 4) + "\n" +
                hs[1].Substring(0, 3) + "..........." + hs[1].Substring(4, hs[1].Length - 4) + "\n" +
                hs[2].Substring(0, 3) + "..........." + hs[2].Substring(4, hs[2].Length - 4) + "\n" +
                hs[3].Substring(0, 3) + "..........." + hs[3].Substring(4, hs[3].Length - 4) + "\n" +
                hs[4].Substring(0, 3) + "..........." + hs[4].Substring(4, hs[4].Length - 4) + "\n", initials);
            GUI.Box(new Rect((Screen.width) / 2 - (Screen.width) / 8, 14 * (Screen.height) / 16, (Screen.width) / 4, (Screen.height) / 4), "ESC to return", initials);
        }

    }

    //makes a text file if one does not exist
    private void defaultHighScores()
    {
        if (!File.Exists("highscore.txt"))
        {
            StreamWriter hsw = new StreamWriter("highscore.txt");
            for (int i = 0; i < 5; i++)
            {
                hsw.WriteLine("AAA" + DEL + "0");
                hs.Add("AAA" + DEL + "0");
            }
            hsw.Close();
        }
    }

    //makes high score list on screen and also in text file
    //Some of this code I (not my teammates since I was responsible for this part) made preivously for my GDAPS2 game -- if that's not allowed just don't factor this in grading since I have other stuff for the above and beyond
    private void HighScores()
    {
        //creates array of current high scores
        hs.Clear();
        StreamReader sr = new StreamReader("highscore.txt");
        for (int j = 0; j < 5; j++)
        {
            hs.Add(sr.ReadLine());
        }
        sr.Close();

        //adds new high score to list if it's higher than any of the old ones
        for (int i = 0; i < 5; i++)
        {
            string[] scorelist = new string[2];

            scorelist = hs[i].Split(DEL);
            int s = int.Parse(scorelist[1]);

            if (score > s)
            {
                List<string> temp = new List<string>();
                if (i == 0)
                {
                    temp.Add(initInput + DEL + score);
                    temp.Add(hs[0]);
                    temp.Add(hs[1]);
                    temp.Add(hs[2]);
                    temp.Add(hs[3]);
                }
                if (i == 1)
                {
                    temp.Add(hs[0]);
                    temp.Add(initInput + DEL + score);
                    temp.Add(hs[1]);
                    temp.Add(hs[2]);
                    temp.Add(hs[3]);
                }
                if (i == 2)
                {
                    temp.Add(hs[0]);
                    temp.Add(hs[1]);
                    temp.Add(initInput + DEL + score);
                    temp.Add(hs[2]);
                    temp.Add(hs[3]);
                }
                if (i == 3)
                {
                    temp.Add(hs[0]);
                    temp.Add(hs[1]);
                    temp.Add(hs[2]);
                    temp.Add(initInput + DEL + score);
                    temp.Add(hs[3]);
                }
                if (i == 4)
                {
                    temp.Add(hs[0]);
                    temp.Add(hs[1]);
                    temp.Add(hs[2]);
                    temp.Add(hs[3]);
                    temp.Add(initInput + DEL + score);
                }
                hs = temp;
                break;
            }
        }
        //writes this back to the text file
        StreamWriter sw = new StreamWriter("highscore.txt");
        for (int i = 0; i < 5; i++)
        {
            sw.WriteLine(hs[i]);
        }
        sw.Close();
    }
}

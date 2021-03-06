﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class SceneManager : MonoBehaviour {

    //lists of objects, referenced frequently by many many things
    public List<GameObject> asteroids;
    public List<GameObject> bullets;
    public List<GameObject> babyAsteroids;

    //game has many levels
    int level;

    //ship
    public GameObject ship;

    //for communicating with other scripts
    AsteroidManager am;
    BulletManager bm;

    //gui for lives, points, level
    public GUIStyle status;
    public GUIStyle pauStatus;

    int points;

    #region audio
    //holds pew pews, booms, and music
    public AudioSource[] aud;

    FMOD.Studio.EventInstance Respawn;
    FMOD.Studio.EventInstance BigExplosion;
    FMOD.Studio.EventInstance SmallExplosion;
    FMOD.Studio.EventInstance Oof;
    FMOD.Studio.EventInstance Pew;
    FMOD.Studio.EventInstance ShipExplosion;
    FMOD.Studio.EventInstance pauSnap;
    FMODUnity.StudioEventEmitter musicEmitter;
    #endregion


    //stars and their movement
    public GameObject starSprite;
    Vector3 position;
    Vector3 velocity;
    public List<GameObject> stars;
    GameObject star;

    public bool paused;

    

    [Range(0.0f, 1.0f)]
    public float volume;
    private Bus masterBus;
    private Bus musicBus;

    // Use this for initialization
    void Start () {
        #region audio
        Respawn = FMODUnity.RuntimeManager.CreateInstance("event:/FX/Respawn");
        BigExplosion = FMODUnity.RuntimeManager.CreateInstance("event:/FX/BigExplosion");
        SmallExplosion = FMODUnity.RuntimeManager.CreateInstance("event:/FX/SmallExplosion");
        ShipExplosion = FMODUnity.RuntimeManager.CreateInstance("event:/FX/ShipExplosion");
        Oof = FMODUnity.RuntimeManager.CreateInstance("event:/FX/Oof");
        Pew = FMODUnity.RuntimeManager.CreateInstance("event:/FX/Pew");
        pauSnap = FMODUnity.RuntimeManager.CreateInstance("snapshot:/Paused");
        #endregion


        paused = false;
        
        
        aud = GetComponents<AudioSource>();
        musicEmitter = GetComponent<FMODUnity.StudioEventEmitter>();
        musicEmitter.SetParameter("Level", 0.0f);
        am = GameObject.Find("Scene Manager").GetComponent<AsteroidManager>();
        bm = GameObject.Find("Scene Manager").GetComponent<BulletManager>();

        //start at level 1 with no points
        level = 1;
        points = 0;

        //for decor. this is a fine dining establishment with only the finest decor
        //sorry I've been coding for like 6 hours ignore that
        SpawnStars();
    }

    // Update is called once per frame
    void Update()
    {
        LevelChanger();

        if (am.lives <= 0)
        {
            //to send score to game over scene
            PlayerPrefs.SetInt("Score", points);
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver");
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            paused = !paused;
            if (paused)
            {
                pauSnap.start();
            }
            else
            {
                pauSnap.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            }
        }
    }

    //other methods can access points without me using a global variable
    public int Points
    {
        get
        {
            return points;
        }
        set
        {
            points = value;
        }
    }

    //other methods can access levels without me using a global variable
    public int Level
    {
        get
        {
            return level;
        }
    }

    //go to next level
    public void LevelChanger()
    {
        if (points >= 1000 * level)
        {
            level++;
            //Debug.Log(level);
            am.SpeedUP();
        }
        musicEmitter.SetParameter("Level", (float)level-1);

    }

    //the status bar
    public void OnGUI()
    {

        GUI.Box(new Rect(10,10,150,100), "Score: " + points, status);
        
        GUI.Box(new Rect(10, 60, 150, 100), "Level: " + level, status);

        GUI.Box(new Rect(10, 110, 150, 100), "Lives: " + am.lives, status);

        if (paused)
        {
            GUI.Box(new Rect(10, 160, 300, 300), "PAUSED", pauStatus);
        }


        
    }

    //for decor, make stars, spawn 30 randomly
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
            //Debug.Log(star);
        }
    }

    public FMOD.Studio.EventInstance GetFMODAudio(string title)
    {
        switch (title)
        {
            case "Respawn":
                return Respawn;
                break;
            case "BigExplosion":
                return BigExplosion;
                break;
            case "SmallExplosion":
                return SmallExplosion;
                break;
            case "Oof":
                return Oof;
                break;
            case "Pew":
                return Pew;
                break;
            case "ShipExplosion":
                return ShipExplosion;
                break;
            default:
                return Respawn;
                break;
        }
    }

}

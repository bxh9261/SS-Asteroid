## Introduction

S.S. Asteroid began as my Asteroids project for IGME-202. The game builds upon the initial requirements of that class with a main menu, story, and high score list. The story stars John and Petrov, two brave astronauts who were accidentally sent into the asteroid belt. They must avoid asteroids in order to survive and complete their mission.

## Analysis of Requirements

The game is comprised of three scenes, Story, Main Game, and Game Over. The Game Over scene is mostly just an interface displaying high scores, so the sound requirements for that scene are minimal. The story mode’s main requirement beyond interface sounds is the dialogue, so I had two find three voice actors to fill the roles of John, Petrov, and Steve. Most of the assets are necessary for the Main Game scene. The game begins with the ship in an immortal state, so the player doesn’t die immediately upon entering the game. This state must be communicated to the user through audio. The user is then given controls of the ship, so audio feedback as the user maneuvers the ship will enhance that experience. The user can move around the ship, shoot lasers at asteroids, and get hit by asteroids, all of which should be shown with audio assets. For additional immersion, music plays in the background, with increasing intensity as the game progresses. Quiet ambience plays throughout. 

## Sound Assets 

FX:

Woosh.wav- played randomly as ambience, as if asteroids are whooshing by. I made this by recording my air purifier then messing with the pitch and volume.

Big-explosion.wav- Used as large asteroids are destroyed. A pitched down version is used to signify the ship being hit with an asteroid.

Small-explosion.wav- Used as small asteroids are destroyed. 

321.wav- Used as a countdown communicating immortal ship state. Recorded by me, added reverb.

Engine.wav & Engine2.wav – Used to communicate movement of the ship. Engine2 created in ReaSynth.

Interface:

Proceed.wav- Used for proceeding through the interface. Made with ReaSynth.

Skip.wav- When the user hits ESC to skip to the next section. Also made with ReaSynth.

Music:

Game.wav is an improved version of a much worse audio track that I put in my IGME-202 game. Menus.wav is an improved version of a rough draft tune that didn’t make the cut. I began by exporting the MIDI from my old FL Studio files and bringing them into Reaper, then I added additional reverb and synths. The tracks improved immensely since I don’t think I even knew what reverb was back when I made music for my project in IGME-202.

Dialogue:

21 of these asset files are the main story dialogue, the other 3 are the “Oof!” and “Ow!” sounds played when the ship gets hit by an asteroid, giving some life to the game, by continuing the story that John and Petrov are in the ship. I had a lot of fun with asset production for this. Ken’s voice stayed relatively the same since the files he sent me were very clear. Nate’s microphone wasn’t the greatest so I added some normalization and ReaFir to improve the sound quality and cancel out background noise. I recorded my own voice with a Tonor BM 700 but also did some work to remove a little background noise and normalize my volume. I added an EQ with both a lowpass and highpass filter to section out only a small chunk of frequencies and give my voice a walkie-talkie/radio effect. 

## FMOD

Engine- This audio combines the two engine.wav files and loops them continuously. The result is a low grumbling engine effect. The Velocity parameter increases the volume and pitch as the ship ramps up in velocity, and the Turning parameter is used as a boolean, I used a flanger to give a choppy effect to the engine as the ship spins. 

Oof- I used a multi-instrument to randomize the three “oof!” voices.

Game Over- The event has “experimental” in the title because I was just messing around with the idea of slowly lowering the pitch but I ended up liking it. Just a neat little fading out of the music on the game over screen, after the effect ends you reach the only part of the game with no music, to bring attention to the ambience as if you’re floating around lost in space.

Level Music- The Level parameter increases based on the level of the game, raising the pitch 1 semitone each level to ramp up the excitement.

Space Sounds- The ambient effect loops and has three additional sounds that play at random. The asteroid “whooshing” effect and the two asteroid explosion sounds, though much quieter. The scattered asteroid noises give the illusion the asteroid belt extends beyond what the player can see.

Dialogue- A discrete parameter is used to progress through the dialogue and prevent needing to make 21 different events.
The project is ordered into 4 banks- Ambience, FX, Music, and Dialogue.

## Mixing

I added snapshots for pausing as well as for when the dialogue is playing, to lower the music to either bring attention to the dialogue or create a paused effect. I cut out the engine sound completely since the way it is set up it’d play continuously throughout the pause. FX/Dialogue sounds were given the highest volumes and priority, since they’re the most important. The thing about one-shot sounds is you only get one-shot to hear them, so they need to be loud and clear. Then the music was a little quieter than that, since it’s important to set the tone of the game, but it shouldn’t muddy out the FX. The ambience is very quiet since it should be heard but doesn’t need to be actively listened to. During mixing is when I also dropped a lot of reverb off of the explosion sounds, since after shooting many asteroids the whole thing sounded very muddy. Further small adjustments included raising the volume of Nate’s voice lines that were quieter than mine and Ken’s.

## Mastering

I set up a Loudness Meter on the Master Bus to get the LUFS value. The original value was around -19 LUFS and I was aiming for -23, so I dropped down the volume of everything. I tested out the game by running Skyrim and a fighting game called Them’s Fightin’ Herds. The latter had very similar audio levels, Skyrim was a little quieter but I didn’t do anything in Skyrim during my comparison that would be particularly loud. (Also, according to this, Skyrim’s level is -26 https://www.stephenschappler.com/2013/07/26/listening-for-loudness-in-video-games/). I further lowered some of the FX and dialogue while raising the music and ambience in tiny increments (to avoid messing up the mixing) since the difference between the audio levels when no FX were playing vs when they were was a bit too drastic. 

## Sources

John was voiced by Ken Nepomuceno

Petrov was voiced by Nate Glod

Pew.wav was voiced by Mo AlHameli

Upsweep.wav (used alongside 3,2,1 voice) by sandyrb https://freesound.org/people/sandyrb/sounds/82985/

Ambience.wav by Tristan Lohengrin (who seems to have since deactivated his freesound account) https://freesound.org/people/Tristan_Lohengrin/sounds/258348/

Big-explosion.wav and small-explosion.wav began as Explosion1.wav by alphatrooper18 https://freesound.org/people/alphatrooper18/sounds/362423/

Engine.wav began as Rocket Boost Engine Loop.wav by qubodup https://freesound.org/people/qubodup/sounds/146770/

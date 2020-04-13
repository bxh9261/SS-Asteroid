## Overview

I will be using my “Asteroids” project from IGME-202. That project had an “above and beyond” part of the assignment, so I added a “background story” of sorts featuring two characters in a spaceship and a mission control worker who accidentally leads them into the asteroid belt. I figured this would be a good thing to add sound to, as well as the gameplay and menus. Much of the gameplay had sound previously, but they were mostly all part of a sound pack from the asset store, so I’ll scrap them for ones developed by me. 

## FX

The gameplay previously had SFX from a retro sounds pack, but I’ll be developing my own. I’ll need an explosion sounds for the asteroids, a “pew” sound for shooting a bullet, and sounds for when the ship is hit/respawning. I’ll also add various “space” noises with a heavy reverb, to give the illusion various asteroids are moving around in the distance, and use FMOD to play them randomly.

## Dialog
I’ll get voice actors to record lines for each line of the “story” in the opening bit. To sell the idea that John and Petrov are still in the ship during gameplay, I may add some voice lines for when the ship gets hit, or some banter to play at random moments during gameplay. I currently have my friend Nate Glod willing to do Petrov’s voice, and he says he has “a pretty good microphone,” I just need to find a John and Steve. 

## Ambience
I spent a fair amount of time watching videos about the sound of space, like https://www.youtube.com/watch?v=-MmWeZHsQzs, to get a good feel of what space sound like. It seems there’s a lot of echo. The FX of whooshing around asteroids can be added onto this and become more frequent and louder as the game progressive.

## Music
The game is based on a retro game, so 8-bit music makes sense. I’ll have a theme that plays during the main menu and gameplay, and another that plays during game over (think the Tetris theme and the Tetris game over music, but my own melody). The music could get gradually pitched up relative to the level the player is on for more intensity.

## Interface
The player has two options for all the menus, to progress forward, and skip to the game. I was thinking a simple sequence of 8-bit notes pitched up would work for the progression and perhaps re-using the “pew” sound to skip. There could also be a sound for typing letters on the high score menu and another for backspacing.

## Sources (so far)
https://freesound.org/people/alphatrooper18/sounds/362423/
https://freesound.org/people/Tristan_Lohengrin/sounds/258348/

## Milestone 3 Update
For this milestone, I added code for the FMOD "Level" parameter in Unity. Enter and Skip (short diddies with ReaSynth), and woosh (one of the scattered ambient sounds, I recorded my desk fan and manipulated the pitch, volume, and reverb). I put sounds into a scatterer instrument in FMOD and built it into the "Game Over" scene in Unity. I got the dialogue from Nate and Ken and recorded my own audio (Tonor BM-700). I added compression and ReaFir to improve audio quality and then added a high and low pass filter on my own audio to give a "walkie talkie" effect on my voice. I rendered the files but haven't yet added dialogue to Unity.
# Unity Space Invaders Tutorial

## Introduction
We will be making a Space Invaders clone with voxel graphics in Unity.

## Tools .......
* Git client: [GitHub Desktop](https://desktop.github.com/) 
* Code Editor: [Visual Studio Code](https://code.visualstudio.com/Download)
  * Install Plugin: **C# for Visual Studio Code**
  * Auto Save!
    * CTRL+SHIFT+P - "Settings" (Open Settings UI)
    * Search for Save
    * Change Auto-Save value to OnFocusChange
* Unity: [Unity Hub](https://unity3d.com/get-unity/download)

## What we'll learn
* Start simple and build up
* Use prefabs for everything in Unity
* Moving sprites with the keyboard
* Moving NPCs on their own
* Collisions
* Keeping score
* Remembering the high scores for a leaderboard page
* Nested prefabs
* Convert simple sprites to voxel based assemblies
* Physics simulator driven explosions

## Instructions

### A: Getting Set-up
1. Open unity hub and create a new project
    * Select the **3D URP** template.
    * If this is your first time using the template, you'll have to download it
    * Give your project a name like "Space Invaders" and take note of the location where the files will be created.
    * Press Create Project

1. Organize your editor   
    * Edit - Preferences - External Tools - External Script Editor = **Visual Studio Code**, then press *Regenerate Project Files*
    * Edit - Preferences - Colors - Playmode Tint - Pick a color
    * Get familiar with the different panels
    * From the top menu select: Window - Layouts - **2 by 3**
    * In the Scene Editor, rotate the scene so the green arrow is up, and the red arrow is to the right
    * Window - Package Manager - Unity Registry - Search - 2D Sprite

### B: The Player
1. Everything is a Prefab, and everything is grouped in Folders
   * In the Heirarchy's Assets, Create a Folder in the Project called *Player*
   * Open this folder and do: Assets - Create - Prefab
   * Name the prefab *Player Prefab*
   * Double-click the prefab to start editing it
   * Game Object - 3D Object - Quad
   * Asset - Create - Material.  Name it "Player Material"
   * Edit the Base Map color, then drag the material onto the quad
   * Exit the prefab editor.  Note that the player is not in the game window yet.
   * Drag the player prefab from the Project to the Heirarcy
                  
1. Player movement
   * Right-click in the Assets/Player folder and select Create - C# Script. Name it "MovePlayer"
   * Drag that script onto the *Player* prefab object
   * Double-click the script to open the code editor
   * Inside the Update function add these lines
        ~~~
            float dx = Input.GetAxis("Horizontal");
            transform.Translate(dx, 0, 0);
        ~~~
   * Swtich back to the unity editor (code should auto-save)
   * Press the *Play* button  and try moving

1. Player sprite
   * In the Assets, create a folder called **Sprites**
   * Download the sprites image [here](https://github.com/GeekyMonkey/UnitySpaceInvaders/raw/master/ReferenceImages/Space%20Invaders%20Sprites.png) and save it into your Assets/Sprites folder
   * Click on the image and in the *Inspector* change the *Texture Type* to **Sprite** and the *Sprite Mode* to **Multiple**, and change *Filter Mode* to **Point**

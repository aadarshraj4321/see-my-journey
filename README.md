# MISSION: HIRE [YOUR NAME]
### An Interactive First-Person Resume
```bash
https://see-my-journey.vercel.app/
```
---

## [CLICK HERE TO PLAY THE LIVE DEMO]([https://see-my-journey.vercel.app/])

---

## What is This?

This isn't just a portfolio. It's a statement. It's a fully playable first-person shooter built from the ground up in **Unity**, where the traditional "resume" has been transformed into an interactive experience. Instead of reading about my skills, you'll engage with them. Instead of a list of projects, you'll fight through them.

The goal was to create a memorable, engaging, and technically impressive demonstration of my abilities as a developer, combining robust programming with a deep understanding of game design and user experience.

## Core Features & "Wow" Moments

This project is packed with features designed to create a dynamic and satisfying gameplay loop.

*   **Section-Based Progression:** The entire portfolio is structured as a single-scene game with multiple "sections" (About Me, Projects, Work Experience, etc.), separated by dynamic transitions.
*   **Dynamic Wave Encounters:** A powerful `EncounterManager` controls the entire game flow, spawning sequential waves of enemies with different configurations for each portfolio item.
*   **Interactive UI System:** All portfolio information is displayed on a clean, professional UI that appears dynamically. This includes:
    *   An interactive "walk-up" system for discovering lore and information points.
    *   Pop-up panels after wave completions with clickable links to GitHub and live project demos.
*   **Advanced Enemy AI:**
    *   **Ground Units:** Standard enemies that use Unity's NavMesh system to intelligently navigate the environment and attack the player.
    *   **Flying Drones:** Flying enemies with custom movement logic that hover and engage the player from the air.
*   **Deeply Interactive Physics World:** The environment is not static. It's a sandbox designed to be interacted with:
    *   **Explosive Barrels:** Classic red barrels that can be shot, triggering a physics-based explosion that deals area-of-effect damage and sends nearby objects flying.
    *   **Destructible Hazards:** Moving cars that navigate the level, acting as lethal hazards to the player but can also be destroyed with enough firepower.
    *   **Pushable Props:** Any object with a `Rigidbody` can be pushed and knocked around the world by weapon fire, adding to the environmental chaos.
*   **Complete Gameplay Loop:**
    *   **Player Health & Damage:** A full health and damage system, including on-screen "hurt" effects and sounds.
    *   **Death & Retry System:** When the player dies, the game pauses and offers a "Retry" screen that seamlessly restarts the current section.
    *   **Health Pickups:** Food prefabs that can be collected to restore health, managed by a timed respawn system.
*   **"Juice" and Polish:** The game is packed with effects to make it feel satisfying and professional:
    *   **Screen Shake** on enemy impacts.
    *   **Slow-Motion** effect on the final kill of a wave.
    *   **Particle Effects** for hits, deaths, explosions, and transitions.
    *   **Dynamic Audio System** with a persistent background music playlist and unique voiceover lines for each wave.

## Technology Stack

*   **Game Engine:** Unity (2022.X or later)
*   **Language:** C#
*   **Platform:** WebGL
*   **Deployment:** Vercel
*   **Version Control:** Git & GitHub

## Getting Started & Running Locally

To run this project on your own machine, follow these steps:

1.  **Prerequisites:**
    *   Unity Hub
    *   Unity Editor (Version 2022.X or newer recommended)
    *   Git

2.  **Clone the Repository:**
    ```bash
    git clone https://github.com/aadarshraj4321/see-my-journey
    ```

3.  **Open in Unity:**
    *   Open Unity Hub.
    *   Click "Open" or "Add project from disk".
    *   Navigate to the cloned repository folder and open it. Unity will import the project (this may take a few minutes).

4.  **Run the Game:**
    *   Once the project is open, find the **`MainMenu`** scene in your `Assets/Scenes` folder.
    *   Open the `MainMenu` scene.
    *   Press the **Play** button at the top of the editor.

## Deployment

This project is built for the web using Unity's WebGL platform and is automatically deployed via **Vercel** whenever a new commit is pushed to the `main` branch.

## About The Author

My name is **Aadarsh Raj**, a passionate and skilled developer with a love for creating interactive and memorable experiences. I thrive on solving complex problems and turning ambitious ideas into polished realities.

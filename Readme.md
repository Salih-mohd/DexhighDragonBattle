# Dragon Battle — Unity Technical Assessment

A 2.5D top-down dragon battle prototype.

The project features a player-controlled dragon fighting an AI-controlled dragon using three abilities: Fire Attack, Tail Attack, and Fly Attack.

## Unity Version

Unity 6 — 6.0.79f1

## Controls

- **WASD** — Move
- **Q** — Fire Attack
- **E** — Tail Attack
- **R** — Fly Attack

## How to Play

Control the player dragon and defeat the AI-controlled enemy dragon using the three available abilities.

Each ability has its own damage value, range, and cooldown. Use movement and positioning to avoid enemy attacks and choose the right ability based on distance.



## Asset Sources

All external assets used in this project were obtained from free sources.

### Unity Asset Store
Used for:
- Dragon model and animations
- Environment assets
- Visual effects

Asset names/links:
- (Free)StylizedVFX Fire Pack
- Dragon for Boss Monster : HP
- RPG Poly Pack - Lite

### Pixabay
Used for:
- Sound effects

## AI Usage Note

### AI Tool Used

- ChatGPT

### How AI Was Used

ChatGPT was used throughout development for:
- Planning the project structure and implementation order
- Writing and refining C# code
- Debugging issues during development
- Reviewing implementation approaches before adding new systems

The project was developed step by step. Each feature was implemented and tested before moving on to the next one, rather than generating the entire solution at once.

### Example of an AI Mistake and How It Was Fixed

While implementing the damage number popup system, the initial AI-generated approach used `Instantiate()` and `Destroy()` every time a popup was shown.

Since damage popups are short-lived and reused frequently, I decided this was not the best approach.

I replaced it with a custom generic object pooling system using a `Queue<T>`. The pool creates a fixed number of popup objects initially and reuses them instead of repeatedly creating and destroying GameObjects.

This reduced unnecessary runtime allocations and also made the popup system reusable for similar temporary objects in the future.

### How AI Improved the Workflow

Using AI helped speed up planning, implementation, and debugging. It was especially useful for breaking larger features into smaller steps and exploring implementation options quickly.

### Playable Build

The Windows build is available from the **Releases** section of this GitHub repository.

1. Open the latest release.
2. Download the Windows build `.zip` file.
3. Extract the ZIP.
4. Open the extracted folder.
5. Run `DragonBattle.exe`.
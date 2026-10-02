# Thaka Platformer

A short 2.5D platformer made for the Thaka International Unity developer task.
One level, about 2–3 minutes long: run, jump, stomp enemies, collect coins, reach the flag.

## How to play

| Action | Keyboard | Gamepad |
|---|---|---|
| Move | A / D or ← / → | Left stick or D-pad |
| Jump | Space, W or ↑ | South button (A / Cross) |
| Pause | Esc | Start |

- Land on top of an enemy to stomp it. Touching it from any other side costs 1 HP.
- You have 4 HP. After a hit you're knocked back and invulnerable for a second (the character blinks).
- Falling into a pit kills you straight away.
- Coins are optional; the HUD shows how many of the 55 you have.
- Flags along the way are checkpoints. They turn green when reached and save your progress.
- The gold flag at the end finishes the level and takes you back to the main menu.

**Main menu:** New Game starts over (and wipes the save), Continue picks up from the last checkpoint
(disabled when there's no save), Quit closes the game.

## Running the project

- Unity **6000.0.67f1**, URP with the 3D Universal Renderer.
- Open `Assets/_Project/Scenes/MainMenu` and press Play. Build order is `MainMenu` (0), `Level01` (1).
- Pressing Play directly in `Level01` also works and starts a fresh run.
- Packages are all Unity first-party: URP, Input System, uGUI, Test Framework.

**Tests:** *Thaka → Tests* opens a small window with a "Run All Tests" button. The same tests show up in
*Window → General → Test Runner → EditMode*. They cover the run state, the save service, the state machine,
the stomp rule and the chaser's detection.

## Project layout

```
Assets/_Project/
  Scripts/          runtime code (Thaka.Platformer assembly)
    Core/           GameFlow – moving between menu and level, New Game / Continue
    Session/        LevelState (the run's state) and GameSession (wires the level together)
    Persistence/    SaveData, the JSON save service, PersistentId
    Player/         input, movement, health, contact with enemies and pickups
    Enemies/        EnemyController base, Patroller, Chaser, their states and patrol paths
    AI/             a small generic state machine
    Level/          Checkpoint, KillZone, LevelGoal
    Collectibles/   Coin
    CameraRig/      side-on follow camera
    UI/             HUD, main menu, pause menu
    Visuals/        drives the character animations
    Audio/          sound effects
    Config/         ScriptableObjects with tuning values (PlayerConfig, EnemyConfig)
  Editor/           ID validator and the Tests window
  Tests/EditMode/   unit tests
  Art/, Audio/      third-party models, animations and sounds (see Credits.txt)
  Prefabs/, Materials/, Scenes/, Config/
```

## How it works

### Game state

`LevelState` is a plain C# class (not a MonoBehaviour) that holds everything about the current run:
HP, coins, which coins were collected, which enemies were defeated and the last checkpoint.
Gameplay writes to it, the HUD listens to its events, and the save system takes snapshots of it.
Keeping it out of the scene is what makes it easy to unit test.

`GameSession` is the one object in the level that connects everything. On load it builds the
`LevelState` (fresh or restored from the save), hides coins and enemies that are already gone,
puts the player at the saved spot, and subscribes to the level objects' events.

### Saving and loading

- Saving follows the Memento pattern: `LevelState` creates a snapshot (`SaveData`) and can restore
  itself from one; `ISaveService` only stores snapshots and never looks inside them.
- The save is a single JSON file in `%USERPROFILE%/AppData/LocalLow/Thaka/Thaka Platformer/save.json`.
- It's written to a temp file first and then swapped in, so a crash mid-save can't corrupt the old save.
  A corrupt or old-version file is ignored instead of crashing, and Continue is disabled for it.
- Coins, enemies and checkpoints are identified by a `PersistentId` (a GUID stored in the scene), so the
  save can say "this coin was collected". An editor script makes sure no two objects share an ID
  (duplicating an object copies its ID) whenever the scene is saved or Play is pressed.

### Enemies

Both enemies run on a small state machine (`AI/StateMachine`). States don't know about each other;
the transitions are declared in one place by each enemy type.

- **Patroller:** a single Patrol state that walks between the ends of its path and pauses at each end.
- **Chaser:** Patrol ↔ Chase. It starts chasing when the player is within 5 units and gives up beyond 7.
  The gap between the two stops it from flickering at the edge. While chasing it stays on its own path, so
  it can't follow the player off a ledge.
- `EnemyController` is the shared base class: it runs the state machine, handles being stomped and
  raises a `Defeated` event. New enemy types only declare their states.

### Player contact

Unity's trigger callbacks turned out to arrive a few frames late, which made fast stomps register as
side hits. Instead, the player does its own overlap check every frame (after everything has moved)
and decides:

- anything that implements `IPlayerTrigger` (coins, checkpoints, kill zone, goal) gets told it was touched;
- for enemies, it's a stomp if the player is falling and their feet were above the enemy's top on the
  previous frame. Otherwise it's a hit.

Level objects and enemies only raise events; `GameSession` reacts to them. The HUD, the sound effects
and the animations work the same way, by listening rather than being called.

## Design decisions

The brief left a few things open, so these are the choices I made:

- **2.5D.** 3D models and camera, but movement is locked to left/right. It keeps the level readable
  and makes stomping reliable.
- **Dying restores the last checkpoint exactly**, including the HP you had when you reached it, plus
  the coins and enemies at that point. It's the same code path as Continue.
  (The brief only says "respawns at the last checkpoint", so full HP on respawn would also be valid.)
- **Progress is saved only at checkpoints**, plus once at the very start so dying before the first
  checkpoint has somewhere to go back to. Going to the main menu from the pause menu doesn't save.
- **Reaching a checkpoint also marks all the ones before it**, and a reached checkpoint never saves
  again, so walking back through an earlier flag can't overwrite later progress.
- **Finishing the level deletes the save**, so Continue is disabled until you start a new run.
- **After a hit** the player is knocked back and invulnerable for 1 second. Without that, standing in
  an enemy would drain all 4 HP in four frames.
- **Tuning values live in ScriptableObjects** (`PlayerConfig`, `PatrollerConfig`, `ChaserConfig`) and
  can be changed in the inspector, even during Play.

## Third-party assets

All CC0. Details and links are in `Assets/_Project/Credits.txt`.

- Characters and animations: *Animated Characters 2* by Kenney.
- Sound effects: *The Essential Retro Video Game Sound Effects Collection* by Juhani Junkala.

Everything else (code, level, materials, UI) was made for this project.

## Known limitations

- Enemy positions aren't saved. Enemies that are still alive start their patrol again after a load.
- There's one save slot.
- Checkpoint order is based on position, which assumes the level goes left to right (it does).
- The UI uses Unity's legacy Text component rather than TextMeshPro, to keep the project free of
  extra imported resources.
- Characters are a single model with different skins; enemies are the same model scaled down.

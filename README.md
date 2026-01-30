# ShiftRunner3D

![MIT License](https://img.shields.io/badge/license-MIT-blue.svg)
![Unity](https://img.shields.io/badge/Unity-2021.x-green.svg)
![C#](https://img.shields.io/badge/C%23-67.3%25-purple.svg)

ShiftRunner3D is a 3D endless runner built with Unity, focused on modular environment tiles, pattern‑based obstacle spawning, and responsive feedback through UI, sound, and camera effects.

## 🎮 Features

- **Endless Tile-Based Track** with dynamic difficulty scaling
- **Pattern-Driven Obstacle Spawning** for varied and replayable runs
- **Lane-Based Player Movement** with jumping mechanics
- **Milestone System** that ramps up challenge and adjusts music pitch over time
- **Audio Feedback** - Lane change SFX, jump sounds, milestone audio, and dynamic music
- **Game Over Flow** with restart functionality and UI click sounds
- **Camera Shake** and collision feedback utilities
- **Object Pooling** for optimized performance
- **Clean, Modular Architecture** that's easy to extend

---

## 📐 Architecture

### High-Level System Overview

```
┌─────────────────────────────────────────────────────────────────┐
│                         SHIFTRUNNER3D                            │
│                     Game Architecture                            │
└─────────────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────────────┐
│                        GAME LAYER                                 │
│  ┌────────────────┐         ┌─────────────────┐                 │
│  │  GameManager   │────────▶│  AudioManager   │                 │
│  │                │         │                 │                 │
│  │ - runDistance  │         │ - PlaySfx()     │                 │
│  │ - milestones   │         │ - PlayMusic()   │                 │
│  │ - gameState    │         │ - AdjustPitch() │                 │
│  └────────┬───────┘         └─────────────────┘                 │
└───────────┼───────────────────────────────────────────────────────┘
            │
    ┌───────┼────────┬──────────────┬──────────────┐
    │       │        │              │              │
    ▼       ▼        ▼              ▼              ▼
┌───────────────────────────────────────────────────────────────────┐
│  ENVIRONMENT    OBSTACLES      PLAYER         UI        UTILS     │
└───────────────────────────────────────────────────────────────────┘
```

### Core Components

#### 1. **Game Manager (Orchestrator)**
- Owns the current game state (running, game over, paused)
- Tracks metrics (distance, score, milestones)
- Increases difficulty at milestones
- Triggers milestone SFX and adjusts music pitch
- Coordinates between all subsystems

#### 2. **Environment System**
- **GroundTileSpawner**: Manages tile pool/queue and spawns tiles ahead of player
- **GroundTile**: Base tile class
- **GroundTileWithObstacles**: Extended tile that includes obstacle patterns
- **CameraFollow**: Smooth camera tracking with offset
- **Difficulty-Aware Logic**: Selects tile types based on current milestone

#### 3. **Obstacle System**
- **ObstaclePatternSpawner**: Controls pattern timing via `spawnInterval`
- **ObstaclePattern**: Defines lane combinations for reusable patterns
- **ObstacleSpawner**: Instantiates obstacles from patterns
- **ObstacleMove**: Handles obstacle movement toward player
- **ObstacleCleanup**: Recycles obstacles using object pooling

#### 4. **Player System**
- **PlayerMovement**: Lane switching, jumping, input handling (new Input System)
- **PlayerAnimation**: Animator state management for run/jump/idle
- **PlayerCollision**: Collision detection, ground checks, game over triggers
- Plays audio feedback for lane changes and jumps

#### 5. **UI System**
- **GameUIController**: Updates HUD (score, distance), game over panel
- Restart button with click SFX
- Subscribes to GameManager events for state changes

#### 6. **Utilities**
- **CameraShake**: Visual feedback for collisions and impacts
- **ObjectPool**: Generic object pooling for obstacles and tiles
- **AudioManager**: Centralized audio management with pitch control

---

## 🗂️ Project Structure

```
ShiftRunner3D/
├── Assets/
│   ├── Materials/
│   ├── Models/
│   ├── Prefabs/
│   │   ├── Environment/
│   │   ├── Obstacles/
│   │   └── Player/
│   ├── Scenes/
│   │   └── Main.unity
│   ├── Scripts/
│   │   ├── Environment/
│   │   │   ├── Ground/
│   │   │   │   ├── GroundTile.cs
│   │   │   │   ├── GroundTileSpawner.cs
│   │   │   │   └── GroundTileWithObstacles.cs
│   │   │   └── CameraFollow.cs
│   │   ├── Game/
│   │   │   ├── AudioManager.cs
│   │   │   └── GameManager.cs
│   │   ├── Obstacles/
│   │   │   ├── ObstacleCleanup.cs
│   │   │   ├── ObstacleMove.cs
│   │   │   ├── ObstaclePattern.cs
│   │   │   ├── ObstaclePatternSpawner.cs
│   │   │   └── ObstacleSpawner.cs
│   │   ├── Player/
│   │   │   ├── PlayerAnimation/
│   │   │   ├── PlayerCollision.cs
│   │   │   └── PlayerMovement.cs
│   │   ├── UI/
│   │   │   └── GameUIController.cs
│   │   └── Utils/
│   │       ├── CameraShake.cs
│   │       └── ObjectPool.cs
│   └── TextMesh Pro/
├── Packages/
├── ProjectSettings/
├── .gitignore
├── LICENSE
└── README.md
```

---

## 🚀 Getting Started

### Prerequisites

- **Unity 2021.x or later**
- **TextMesh Pro** (via Unity Package Manager)
- **Input System** package (new Unity Input System)

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/AE707/ShiftRunner3D.git
   cd ShiftRunner3D
   ```

2. Open the project in Unity Hub

3. Let Unity import all assets and packages

4. Open the **Main scene** (`Assets/Scenes/Main.unity`)

5. Press **Play** to start the endless run

---

## 🤝 Contributing

We welcome contributions! Whether it's bug fixes, new features, or documentation improvements, your help is appreciated.

### How to Contribute

1. **Fork the repository**
   ```bash
   git clone https://github.com/YOUR_USERNAME/ShiftRunner3D.git
   ```

2. **Create a feature branch**
   ```bash
   git checkout -b feature/AmazingFeature
   ```

3. **Make your changes**
   - Follow the existing code style and conventions
   - Write clear, descriptive commit messages
   - Add comments for complex logic
   - Test your changes thoroughly

4. **Commit your changes**
   ```bash
   git add .
   git commit -m "Add: Implemented power-up system with speed boost"
   ```

5. **Push to your fork**
   ```bash
   git push origin feature/AmazingFeature
   ```

6. **Open a Pull Request**
   - Provide a clear description of the changes
   - Reference any related issues
   - Include screenshots/videos if applicable

### Commit Message Convention

Use clear, descriptive commit messages:

- `Add: New feature or functionality`
- `Fix: Bug fix`
- `Update: Improvements to existing code`
- `Refactor: Code restructuring without changing behavior`
- `Docs: Documentation updates`
- `Style: Code formatting, no logic changes`

**Examples:**
```
Add: Implement shield power-up with duration timer
Fix: Obstacle spawning interval not resetting properly
Update: Improve lane switching smoothness
Refactor: Extract difficulty logic into DifficultyManager
Docs: Add architecture diagram to README
```

### Code Style Guidelines

- **Naming Conventions:**
  - Classes: `PascalCase` (e.g., `PlayerMovement`)
  - Methods: `PascalCase` (e.g., `SpawnNextTile()`)
  - Private fields: `camelCase` (e.g., `currentLane`)
  - Public fields: `camelCase` (e.g., `jumpForce`)

- **Unity Best Practices:**
  - Use `SerializeField` for private fields shown in Inspector
  - Cache component references in `Awake()` or `Start()`
  - Avoid using `Find()` or `GetComponent()` in `Update()`
  - Use object pooling for frequently instantiated objects
  - Prefer events/delegates over `SendMessage()`

- **Performance:**
  - Profile your changes if they affect runtime performance
  - Use object pooling for obstacles and tiles
  - Minimize allocations in Update loops

### What to Contribute

**Good First Issues:**
- Add sound effects for existing actions
- Create new obstacle patterns
- Design new ground tile variations
- Improve UI animations
- Add particle effects for collisions

---

## ✅ TODO / Roadmap

### 🔴 High Priority

- [ ] **Mobile Controls**
  - Implement touch/swipe controls for mobile devices
  - Add virtual buttons for jump
  - Optimize UI for different screen sizes

- [ ] **Difficulty Balancing**
  - Fine-tune obstacle spawn intervals per difficulty level
  - Adjust player speed progression curve
  - Add difficulty scaling config (JSON/ScriptableObject)

- [ ] **Performance Optimization**
  - Implement object pooling for all obstacles
  - Optimize tile recycling system
  - Profile and reduce garbage collection

### 🟡 Medium Priority

- [ ] **Power-Up System**
  - Speed boost power-up
  - Shield/invincibility power-up
  - Magnet for collectibles
  - Power-up visual effects

- [ ] **Collectibles & Scoring**
  - Add coins/gems to collect during run
  - Implement score multiplier system
  - Create combo system for consecutive collections

- [ ] **High Score Persistence**
  - Save/load high scores (PlayerPrefs or JSON)
  - Display top 5 scores on main menu
  - Add date/time stamps to scores

- [ ] **Visual Polish**
  - Add particle effects for lane changes, jumps, collisions
  - Improve obstacle models and textures
  - Add skybox and environment lighting
  - Implement trail effects for player movement

- [ ] **Audio Enhancements**
  - Add ambient environment sounds
  - Implement audio mixer for volume control
  - Add more varied obstacle collision sounds
  - Background music variations per milestone

### 🟢 Low Priority / Nice to Have

- [ ] **Multiple Characters**
  - Character selection menu
  - Different character models with unique animations
  - Character-specific abilities or stats

- [ ] **Theme/Biome System**
  - Multiple visual themes (cyberpunk, nature, desert, etc.)
  - Theme-specific obstacles and tiles
  - Dynamic theme transitions

- [ ] **Leaderboard Integration**
  - Online leaderboard (Firebase/PlayFab)
  - Friend rankings
  - Daily/weekly challenges

- [ ] **Social Features**
  - Share score to social media
  - Screenshot capture on game over
  - Replay system

- [ ] **Advanced Features**
  - Daily missions/objectives
  - Achievement system
  - Unlockable content
  - Settings menu (audio, graphics quality, controls)

---

## 📝 License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

---

## 🚀 Contact & Support

- **Author**: [AE707](https://github.com/AE707)
- **Repository**: [ShiftRunner3D](https://github.com/AE707/ShiftRunner3D)
- **Issues**: [Report a bug or request a feature](https://github.com/AE707/ShiftRunner3D/issues)

---

**Made with ❤️ and Unity**

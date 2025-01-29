# Bus Jam Clone 🎮

A clone of the popular mobile game **Bus Jam**, built using **Unity** and **C#**. In this traffic puzzle game, players must strategically guide passengers to their buses while overcoming obstacles and managing space efficiently. Each level presents unique challenges, requiring players to clear traffic jams, match passengers to their corresponding buses, and ensure smooth departures to progress through the game.

---

## 📖 About the Project

This project is **not intended for commercial use**. It replicates core gameplay mechanics of Bus Jam while implementing clean architectural practices and efficient development patterns.

---

## 🛠️ Built With

- **Unity 2022.3.19**
- **C#**

### Third-Party Libraries:
- [DoTween](http://dotween.demigiant.com/) - Tweening engine for animations
- [UniTask](https://github.com/Cysharp/UniTask) - Async/await utilities for Unity
- [Yellowpaper.SerializedDictionary](https://github.com/yellowpaper-dev/serialized-dictionary) - Serializable dictionary for Unity

### Custom Implementations:
- Custom **DI (Dependency Injection)** library
- Custom **UTask** library
---
## 🧩 Architecture

This project follows the **Model-View-Presenter (MVP)** architectural pattern for clean separation of concerns and maintainability. The structure is designed to ensure clarity, scalability, and testability across all game systems.

### Key Components

1. **Models**:
   - Models are responsible for storing and managing **game data**.
   - They encapsulate the state and rules of the game without direct interaction with Unity components.
   - Examples include:
     - Grid data for levels
     - Player progress and saved state

2. **Views**:
   - Views handle **visual feedback** and **user interaction**.
   - They are responsible for rendering the game’s UI and scene elements but do not contain any logic.
   - Views react to data changes communicated by the Presenters.
   - Examples:
     - UI elements such as the LevelButton and Fail popup

3. **Presenters**:
   - Presenters act as the **logic layer** between Models and Views.
   - They process user input, update models, and synchronize the views accordingly.
   - Presenters are **high-level managers** of the game’s flow, thanks to their reliance on **Handler classes**.

4. **Handlers**:
   - Handlers are the **abstraction layer** for Presenters.
   - They break down game logic into more manageable units, ensuring Presenters remain clean and focused on orchestration.
   - Handlers handle specific responsibilities, such as:
     - **Interfacing with Factories** to create and destroy game objects


5. **Factories**:
   - Factories are responsible for **creating and destroying game objects** efficiently.
   - They are accessed exclusively through Handlers to maintain abstraction.
   - Examples:
     - BusFactory: Creates new grid buses.
     - DummyFactory: Creates new dummies.

---
### Game Flow

1. **InitialScene**:
   - The project's entry point where the `ProjectContext` is activated.
   - The `ProjectContext` initializes the dependency injection system and ensures core services are ready for use.

2. **GamePresenter**:
   - The **GamePresenter** is initialized as **non-lazy** in the `ProjectContext`.
   - It sets up the core game logic and manages the game's overall flow.

3. **StartScene**:
   - The **GamePresenter** activates the **ScenePresenter**, which loads the **MainScene**.
   - In the **MainScene**, the player can select a level using the **LevelButton**. The button displays the current level or "Finished" if all levels are complete.

4. **LevelScene**:
   - After selecting a level, the **LevelScene** is loaded.
   - The `SceneContext` inside the **LevelScene** is activated, dynamically initializing the level-specific data and systems for that level.
---

This architecture ensures:
- **Separation of Concerns**: Each layer has a specific responsibility, minimizing dependencies and ensuring scalability.
- **Clean Logic**: Presenters remain focused on high-level orchestration, with Handlers managing specific tasks.
- **Efficient Resource Management**: Factories optimize object creation and destruction, ensuring smooth gameplay even in complex scenarios.


---

## 🛠️ Features

- **Dynamic Level Editor**:
  Access the **Level Editor** from **"Tools > Level Editor"** in the Unity Editor. This tool allows you to:
  - Dynamically update the level.
  - Save the level.
  
- **Multiple Levels**:
  Players progress through levels by completing specific objectives in each one.

---

## 🎮 Gameplay Summary

The game is a **level-based puzzle game** where the player must clear all obstacles to win the level. Here’s the gameplay flow and mechanics:

### Flow
1. In the **StartScene**, where the player sees a **LevelButton** displaying the current level.
   - If all levels are completed, the **LevelButton** displays "Finished".
2. Clicking the **LevelButton** loads the **LevelScene** for the current level.
3. After the level:
   - If the player wins:
     - Success panel is shown
     - The game returns to the **MainScene** by clicking the next button
   - If the player loses:
     - Fail panel is shown
     - Replay the level by clicking the retry button
4. Progress is saved locally, ensuring the game resumes from the last level after restarting Unity.

## 📂 Level Structure

Each level is defined by the following properties:
- `level_number`: Number of the level.
- `grid_width`: Width of the grid.
- `grid_height`: Height of the grid.
- `timer`: Seconds that need to be done before it expires
- `bus_count`: Number of the buses
- `bus_order`: List of buses, respectively
- `grid`: List of items in the grid, starting from the bottom-left and progressing horizontally to the top-right.

### Dummy/Bus Definitions
- **r, g, b, y**: Red, Green, Blue, Yellow cubes.
- **emp**: Empty.

---

## 📂 How to Use

Clone the repository:
   ```bash
   git clone https://github.com/fisixus/BusJam-Clone.git

# Knight Hop (Unity Mini-Game-System)

A simple chess training game where you click all valid knight moves before time runs out. Built for a Unity developer position assignment in around 6 hours.

## Gameplay Demo
![gameplay](https://github.com/user-attachments/assets/3c1065bd-1aa3-4a31-91db-c27887a5be20)

## How to Play

1. **Tutorial Mode** (first time only): Valid knight moves are highlighted in yellow. Click all highlighted squares to complete the tutorial.
2. **Main Game**: A knight spawns on a random square. Click every square the knight can legally move to.
3. **Timer**: 15 seconds (configurable) to find all valid moves.
4. **Win Condition**: Click all correct moves before time expires.
5. **Restart**: Use the restart button to play again.

## Project Structure

### The Architecture
I went with a clean separation of concerns using a few key patterns:

**Finite State Machine**: The game has 4 states (Menu, Tutorial, Playing, Results) managed by `GameManager`. State transitions trigger events that other systems listen to.

**Singletons**: Core managers (`GameManager`, `UIManager`, `Timer`, `TutorialManager`) use the singleton pattern for straightforward access in a single-scene context.

**Event-Driven**: Systems communicate through C# events to minimize coupling. The timer broadcasts time events, the game manager broadcasts state changes, and UI components respond accordingly.

**ScriptableObject Config**: All configurable parameters (board size, timer duration, colors) are stored in `GameConfig.asset` for runtime-free adjustments.

### The Files

**Core Logic**:
- `GameManager.cs` - Main controller, handles game loop and state machine
- `GameState.cs` - Enum for the 4 game states
- `Timer.cs` - Countdown system with events
- `GameConfig.cs` - ScriptableObject for all settings

**Board & Gameplay**:
- `BoardManager.cs` - Generates the chessboard, spawns knight
- `ClickableSquare.cs` - Individual square behavior
- `CubeRiseAnimation.cs` - Makes squares rise from below (visual polish)
- `Helpers.cs` - Utility functions (knight move calculations, easing)

**UI**:
- `UIManager.cs` - Controls all UI panels with smooth transitions
- `ScoreDisplay.cs` - Shows "X/Y" score
- `TimerDisplay.cs` - Countdown with color warning
- `StartButton.cs` / `RestartButton.cs` - Button behaviors

**Tutorial**:
- `TutorialManager.cs` - Handles first-time tutorial, highlights valid moves

### Why This Structure?

I wanted to show I can organize code properly without over-engineering it. Each class has one job (SRP). Systems talk through events, not direct references. Everything is testable and extensible without being overly abstract.

## What I'd Improve With More Time

- Namespaces to avoid potential name clashes
- Organize scripts into folders by feature and namespace (UI, Gameplay, Core)
- Add null checks, and remove unused variables or other bloat if any
- Multiple difficulty levels with ScriptableObject configs (bigger boards, faster timers)
- Sound effects and music
- Particle effects when you get moves right
- Better visual feedback (animations, screen shake)
- Ensure UI works on different screen sizes and aspect ratios

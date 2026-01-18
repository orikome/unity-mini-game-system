# Knight Hop (Unity Mini-Game-System)

A simple chess training game where you click all valid knight moves before time runs out. Built for a Unity developer position assignment in around 6 hours.

## Technical Notes

- **Unity Version**: 6000.1.17f
- **Single Scene**: Everything happens in `MainScene`

## How to Play

1. **Tutorial Mode** (first time only): The game shows you the ropes by highlighting valid knight moves in yellow. Click them all to get started.
2. **Main Game**: A knight spawns on a random square. Your job is to click every square the knight can legally move to.
3. **Timer**: You've got 15 seconds (configurable) to find all moves.
4. **Win Condition**: Click all correct moves before time runs out!
5. **Restart**: Hit the restart button and try again!

## Gameplay Demo


## Project Structure

### The Architecture
I went with a clean separation of concerns using a few key patterns:

**Finite State Machine**: The game has 4 states (Menu, Tutorial, Playing, Results) managed by `GameManager`. State transitions trigger events that other systems listen to.

**Singletons**: Core managers (`GameManager`, `UIManager`, `Timer`, `TutorialManager`) use the singleton pattern. I know some people aren't fans, but for a small single-scene game it is simple and easy.

**Event-Driven**: Everything communicates through C# events to keep coupling low. The timer fires events when time's up, the game manager broadcasts state changes, and UI components just listen and react. No messy references everywhere.

**ScriptableObject Config**: All the tunable things (board size, timer duration, colors) lives in `GameConfig.asset` so you can tweak without touching code.

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


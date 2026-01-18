# Knight Hop (Unity Mini-Game-System)

A simple chess training game where you click all valid knight moves before time runs out.

## Gameplay Demo
![gameplay](https://github.com/user-attachments/assets/3c1065bd-1aa3-4a31-91db-c27887a5be20)

## How to Play

1. **Tutorial Mode** (first time only): Valid knight moves are highlighted in yellow. Click all highlighted squares to complete the tutorial.
2. **Main Game**: A knight spawns on a random square. Click every square the knight can legally move to.
3. **Timer**: 15 seconds (configurable) to find all valid moves.
4. **Win Condition**: Click all correct moves before time expires.
5. **Restart**: Use the restart button to play again.

## Project Structure

- **State Machine**: Menu → Tutorial → Playing → Results (managed by `GameManager`)
- **Event-Driven**: Systems communicate via C# events to minimize coupling
- **ScriptableObject Config**: All settings in `GameConfig.asset`
- **Singletons**: `GameManager`, `UIManager`, `Timer`, `TutorialManager`

I wanted to show I can organize code properly without over-engineering it. Each class has one job (SRP). Systems talk through events, not direct references. Everything is testable and extensible without being overly abstract.

### Key Files

| File | Purpose |
|------|---------|
| `GameManager.cs` | Game loop and state transitions |
| `BoardManager.cs` | Board generation, knight spawning |
| `Timer.cs` | Countdown with tick/expire events |
| `TutorialManager.cs` | First-time onboarding flow |
| `UIManager.cs` | Panel transitions and animations |

## What I'd Improve With More Time

- Namespaces to avoid potential name clashes
- Organize scripts into folders by feature and namespace (UI, Gameplay, Core)
- `GameManager` does too much, split out score and move validation
- Tutorial duplicates game logic instead of reusing it
- Multiple difficulty levels with ScriptableObject configs (bigger boards, faster timers)
- Sound effects and music
- Particle effects when you get moves right
- Better visual feedback (animations, screen shake)
- Ensure UI works on different screen sizes and aspect ratios

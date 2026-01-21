SUOMIPELI - Finnish Stereotype Micro-Game Collection
====================================================

A fast-paced mobile game featuring simple micro-games based on Finnish stereotypes.
Inspired by "Dumb Ways to Die" gameplay mechanics.

QUICK START
-----------
1. Open Unity menu: Suomipeli > Setup Wizard
2. Click buttons 1-4 in order to auto-generate prefabs and scene objects
3. Manually create UI (see Setup Instructions page for details)
4. Press Play and enjoy!

WHAT'S INCLUDED
---------------
Core Scripts:
  - GameManager.cs - Main game loop, scoring, difficulty
  - MicroGame.cs - Base class for all micro-games
  - ScoreManager.cs - High score persistence
  - UIManager.cs - UI state management

Micro-Games:
  - BusSeatingMicroGame.cs - Choose seat far from others
  - CoffeeChoiceMicroGame.cs - Always pick coffee!
  - SaunaTemperatureMicroGame.cs - Set temperature (needs UI slider)

Utilities:
  - Editor/SuomiPeliSetupWizard.cs - Automated setup tool
  - GameSetupHelper.cs - Helper functions

GAME MECHANICS
--------------
- Choose correct option based on Finnish social norms
- Time limit decreases every 3 successful games
- Failure = instant game over, restart from beginning
- Score increases by 100 points per success
- High score saved automatically

CURRENT MICRO-GAMES
-------------------
1. Bus Seating: Click the seat furthest from all passengers
2. Coffee Choice: Click coffee (not tea!)

CONTROLS
--------
Mouse Click / Touch - Select objects/options

REQUIREMENTS
------------
- Unity 6000.2.10f1
- TextMeshPro (import via Window > TextMeshPro > Import TMP Essential Resources)
- Universal Render Pipeline (already configured)

DOCUMENTATION
-------------
See "Setup Instructions" page in Bezi for:
  - Detailed manual setup instructions
  - UI creation guide
  - Customization options
  - How to add new micro-games
  - Troubleshooting

PROJECT STRUCTURE
-----------------
/Assets
  /Materials - Generated materials for prefabs
  /Prefabs - Basic prefabs (Seat, Person, Coffee, Tea)
    /MicroGames - Complete micro-game prefabs
  /Scenes - Main game scene
  /Scripts - All C# game code
    /Editor - Unity Editor extensions

CUSTOMIZATION
-------------
Edit GameManager.cs constants:
  - SPEED_INCREASE_INTERVAL (default: 3)
  - TIME_REDUCTION_PER_LEVEL (default: 0.2s)
  - POINTS_PER_SUCCESS (default: 100)
  - initialTimeLimit (default: 5s)
  - minimumTimeLimit (default: 1.5s)

ADDING NEW MICRO-GAMES
----------------------
1. Create new script extending MicroGame
2. Override SetupGame() method
3. Call CompleteGame(true/false) when done
4. Create prefab with your script
5. Add prefab to GameManager's Micro Game Prefabs array

Example template in Setup Instructions page!

NEXT STEPS
----------
- Add sound effects and music
- Create more micro-games (sauna, silence, queue spacing, etc.)
- Replace primitive shapes with proper models/sprites
- Add particle effects
- Implement lives system
- Add achievements

SUPPORT
-------
Check Setup Instructions page for detailed help and troubleshooting.

Built with Unity 6 and ❤️ for Finnish culture!

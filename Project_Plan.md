# Clash of Codes - 4-Week Development Plan

## Project Overview & Refined Idea

I have read the `coding_game.pdf`! Your concept, **Clash of Codes**, is a brilliant blend of real-time strategy (like Clash Royale) and competitive programming.

### The Core Concept

A real-time multiplayer game where players balance **Battle Elixir (BE)** and **Rank Points (RP)**. Players race to solve algorithmic challenges across a Three-Track Node Setup (Easy, Medium, Hard) and deploy disruptive "Spells" to sabotage opponents or defend themselves.

### Tech Stack

- **Backend/Game Server:** ASP.NET Core with **SignalR** (Handles real-time multiplayer lobbies, spell casting, and validations).
- **Database:** SQL Server with Entity Framework Core (Stores user RP, custom loadouts, and the question banks).
- **Frontend:** Blazor Web App (Interactive Server mode is highly recommended here to easily sync real-time state with SignalR).
- **Styling:** HTML, CSS, and some JavaScript Interop for the crazy visual spell effects (like screen flickering or obscuring lines).

### Proposed Modifications for a First Website

1.  **Code Execution (Stage I):** Running untrusted code on your server is dangerous. For your first version, either use a free integration like the **Judge0 API** to execute code safely, or simulate the execution by using regex/string matching against user outputs.
2.  **Blazor Interactive Server + SignalR:** Building a real-time multiplayer game requires web sockets. ASP.NET Core has **SignalR** built-in, which integrates flawlessly with Blazor. This will be the brain of your 1v1 battles.
3.  **Spell Implementations:** Start with UI-based spells (e.g., _Fog of War_, _Syntax Poison_, _EMP_) because they can be achieved purely by toggling CSS classes via Blazor rather than complex compiler hacks.

---

## 4-Week Step-by-Step Guide

### Week 1: Foundations, Database & Users

_Goal: Get your Database, Auth, and basic API setup._

- **Day 1: Project Setup**
  - Create a Blazor Web App (Interactive Server mode).
  - Install Entity Framework Core and SignalR packages.
- **Day 2: Database Design**
  - Design tables: `Users` (Rank Points, Arena Level), `Questions` (Track category, test cases), `MCQs`, and `MatchHistory`.
- **Day 3: Entity Framework Setup**
  - Create C# models for your tables and run EF Core Migrations to build the SQL DB.
- **Day 4: Auth & Login**
  - Integrate ASP.NET Core Identity for secure Login/Registration.
- **Day 5: Question Data Entry**
  - Create a simple page to populate your database with initial algorithm questions and MCQs for the 3 Arenas.
- **Day 6: User Profile & Setup**
  - Create the Profile UI where users see their RP, current Arena, and can build their 4-Spell Loadout.
- **Day 7: Review & Testing**
  - Ensure creating an account, logging in, and editing the Spellbook saves correctly to the database.

### Week 2: Real-time Multiplayer (SignalR) & The Arena UI

_Goal: Connect two players in a live room and build the IDE interface._

- **Day 8: SignalR Basics**
  - Create a `MatchHub` in ASP.NET Core to handle WebSocket connections.
- **Day 9: Matchmaking (Custom Lobbies)**
  - Implement "Room Codes". Allow a player to create a room, generate a code, and have another player join it via SignalR.
- **Day 10: Matchmaking (Ranked Ladder)**
  - Implement a simple queue system in your Hub to pair players with similar Rank Points (RP).
- **Day 11: The Arena UI - Layout**
  - Build the split-screen UI: Your IDE on one side, opponent's progress trackers on the other.
- **Day 12: The Arena UI - 3 Tracks**
  - Bind the Left (Easy), Right (Medium), and Center (Hard/King) tracks to the UI.
- **Day 13: Integrated Code Editor**
  - Embed a code editor like **Monaco Editor** (the engine behind VS Code) into your Blazor app using JS Interop.
- **Day 14: Review & Test Connections**
  - Open two browsers. Join the same ranked queue. Ensure both screens transition to the Arena simultaneously!

### Week 3: Core Loop, Compiling & Economics

_Goal: Make the game playable with Elixir and Code compilation._

- **Day 15: Battle Elixir (BE) System**
  - Implement the passive BE generation (1 BE / 10s, Double Elixir in last 60s) using a Blazor timer.
- **Day 16: Code Compilation (Stage I)**
  - Wire the "Submit Code" button to send code to the backend. Either mock the responses or ping an external execution API (like Judge0).
- **Day 17: MCQ Stream (Stage II)**
  - If the code passes, immediately display the 3-5 MCQ questions over the IDE.
- **Day 18: Track Resolution & Crowns**
  - Award a Crown when Stage I and Stage II are passed. Sync this to the opponent's screen via SignalR!
- **Day 19: The King's Tower Instant Win**
  - Add the logic: If the Center Track is solved, instantly broadcast a "Match Over - 3 Crown Win" event.
- **Day 20: RP "Greed vs. Power" Economy**
  - When the match ends, calculate RP: Base RP + (Unspent Elixir \* Multiplier). Update the DB.
- **Day 21: Full Game Loop Test**
  - Play a full match against yourself. Verify Elixir caps at 10, code submits, MCQs load, and RP is awarded.

### Week 4: The Spellbook & Polish

_Goal: Add the chaos of Spells and deploy the game._

- **Day 22: UI Spells (Offensive)**
  - Implement `Fog of War`, `Syntax Poison`, and `EMP (Flicker)`. Since these are visual, you can trigger CSS class changes on the opponent's screen via SignalR!
- **Day 23: Data Spells (Offensive)**
  - Implement `MCQ Barrage` (injects more questions) and `Bracket Bandit` (manipulate their codebase buffer via JS Interop).
- **Day 24: Defensive Spells**
  - Implement `Garbage Collector` (removes CSS debuffs) and `Firewall` (locks a track).
- **Day 25: Loadout Syncing**
  - Ensure the 4 spells a user picked in Week 1 Day 6 appear on their active hotbar in the Arena, deducting BE when clicked.
- **Day 26: Arena Progressions**
  - Create Visual Themes logic. If average RP is > 2000, load the "Stack Overflow" Arena aesthetic.
- **Day 27: Edge Cases & Disconnects**
  - What happens if a player closes the tab? (SignalR `OnDisconnectedAsync` -> award auto-win to opponent). Check for bugs and exploits.
- **Day 28: Deployment & Celebration!**
  - Publish your database and code to Azure or SmarterASP. Invite a friend and duel!

---

_Tip: The real-time aspect (SignalR) is the heart of this project. Spend extra time in Week 2 understanding how to send payloads between Client A, the Server, and Client B. You've got this!_

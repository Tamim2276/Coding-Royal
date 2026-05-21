# Clash of Codes - Game Design Document & 4-Week Build Guide

## Project Overview

**Clash of Codes** is a multiplayer competitive programming game that adapts the high-stakes, real-time resource management of the _Clash Royale_ format into an algorithmic battleground. Players match globally or challenge friends, balancing fast-paced code composition, MCQs, and disruptive abilities called **Spells**.

### Tech Stack

- **Backend and UI:** ASP.NET Core 8 & Blazor Server
- **Database:** SQL Server Express + Entity Framework (EF) Core
- **Real-Time Multiplayer:** ASP.NET Core SignalR
- **Code Editor:** Monaco Editor (JS Interop)
- **Code Execution:** Judge0 CE API
- **Deployment Target:** Azure App Service

---

## 1. The Core Game Loop & Resource Economy

### Battle Elixir (BE)

Used exclusively during an active match to deploy tactical spells.

- **Capacity limits:** Start at 0 BE, hard-capped at **10 BE**.
- **Passive Generation:** **1 BE / 10s**. In the final 60 seconds (Double Elixir), generates **2 BE / 10s**.
- **Active Generation:**
  - Successful Compilation: **+1 BE**
  - Flawless First-Submission Run: **+3 BE**
  - Correct MCQ Answer: **+1 BE**

### Rank Points (RP) & The Greed vs. Power Economy

Rank Points dictate a user's global ladder standing. The core mechanic requires players to balance spending Elixir against saving it for bonus RP:

- **Final RP Awarded = Match Base Victory RP + (Unspent BE Remaining × δ)**
- **δ = 5** is the recommended scaling multiplier.
- **Loss Streak Protection:** After 3 consecutive losses, the RP penalty for the next match is halved.

---

## 2. Match Structure & Victory Conditions

A standard map has three algorithmic tracks (Left: Easy, Right: Medium, Center: Hard/King's Tower).

- **Left / Right Tracks:** Yield **1 Crown** each on completion.
- **Center Track:** Yields **3 Crowns** forming an absolute victory.

**Node Workflow:**

- **Stage I:** Code Composition (pass public test cases).
- **Stage II:** MCQ Stream (3–5 questions).

---

## 3. The Spellbook Mechanics

For the v1 launch, you will launch with exactly **6 Spells** (3 offensive, 3 defensive). A player brings exactly 4 spells into a match.

### Recommended Spells (v1):

#### Offensive:

1. **Fog of War (2 BE):** Hides compiler line numbers for 45s ("Compilation Failed" only).
2. **Time Warp (3 BE):** Distorts opponent's match timer to spin at double speed for 30s.
3. **MCQ Barrage (5 BE):** Force-injects 2 high-difficulty MCQs into the opponent's validation queue.

#### Defensive:

4. **Lint Shield (2 BE):** Immune to flickering, theme changes, typo curses for 60s.
5. **Ctrl + Z Rollback (4 BE):** Activates a 3s reactive window to restore the previous code buffer when swapped or wiped.
6. **Garbage Collector (5 BE):** Purges all active debuffs, visual distortions, and status impairments from UI.

### Design Improvements:

- **Spell Cooldown System:** 30–60 second cooldown per slot after a spell is cast.
- **Active Debuff Cap:** A player may have at most **2 active offensive debuffs** on the opponent. A third spell replaces the oldest debuff.
- **Spell Notifications (Toasts):** When a spell hits, display a Toast notification (e.g., _"Opponent cast Fog of War - compiler messages hidden for 45s"_).

---

## 4. Matchmaking Models & Environments

- **Ranked Ladder:** Elo-based pairing within ±200 RP.
- **Custom Lobbies:** Room Codes (6 char key) or Direct Invites. Host controls difficulty, topics, and Elixir scaling.
- **Surrender & Disconnect:** Forfeit and opponent wins if disconnected for ≥ 60s.
- **Arena Themes:** (Spaghetti Junction: 0-499, Parse Pit: 500-999, ... Kernel Palace: 5000+). Each new arena unlocks one extra Spellbook slot.

---

## 5. 4-Week Day-by-Day Build Guide

### Week 1 — Foundations (Auth, Database, Basic UI)

- Goal: Set up Blazor Server, EF Core Db, Identity, and basic profile page.

* **Day 1:** Project scaffold. Run `dotnet new blazorserver -o ClashOfCodes`. Push to GitHub.
* **Day 2:** Layout and Navigation. Edit `MainLayout.razor`, add dummy pages.
* **Day 3:** Database Design & EF Core Setup. Define User, Problem, Match models. Run `Init` migration.
* **Day 4:** Database Seeding. Seed problems/MCQs. Build `Admin` Blazor page displaying problems.
* **Day 5:** ASP.NET Identity. Add `Microsoft.AspNetCore.Identity.EntityFrameworkCore`. Add custom fields like `RankPoints`, `BattleElixir`.
* **Day 6:** Register/Login Pages using `EditForm` and `AuthenticationStateProvider`.
* **Day 7:** Profile screen & Spellbook selector. Allow users to select 4 out of 6 base spells.

### Week 2 — Core Game Loop (Editor, Judge0, Economy)

- Goal: Get Monaco editor running, compile code, handle MCQs, and do a solo practice loop.

* **Day 8:** Code Editor. Load Monaco Editor via CDN, use `IJSRuntime` (JS Interop).
* **Day 9:** Code Submission via **Judge0 API**. Subscribe, construct `JudgeService` using `HttpClient`.
* **Day 10:** MCQ Stage. Transition to `McqPanel.razor` after successful compilation. Handle event callbacks.
* **Day 11:** BE Economy & Crowns. System timer for BE. Tracking test cases passed and Crowns logic.
* **Day 12:** Solo Practice Mode. Wire up `GamePage.razor` entirely to play local.
* **Day 13:** Match Result Screen. Calculate final RP (Base RP + Unspent BE \* 5), save to `MatchHistory`.
* **Day 14:** Buffer/Polish. Review progress and read SignalR documentation.

### Week 3 — Spells & Real-time Multiplayer (SignalR)

- Goal: The hardest week. Implement multiplayer state sync and actual spell casting.

* **Day 15:** SignalR Basics. Add `Microsoft.AspNetCore.SignalR`, create `GameHub`. Create simple ping-pong message test.
* **Day 16:** Match State Sync. Broadcast test case pass % and crowns. Store state in server-side `ConcurrentDictionary`.
* **Day 17:** Double Elixir & Match End. Broadcast double elixir at T-60s. Handle match end criteria and broadcast winner.
* **Day 18:** Offensive Spells (UI). Implement handling for _Fog of War_ and _Time Warp_ over SignalR.
* **Day 19:** Offensive Spells (Code). Implement _MCQ Barrage_ injection.
* **Day 20:** Defensive Spells & Hotbar UI. Implement _Lint Shield_, _Garbage Collector_, _Ctrl + Z_, and 30s-60s cooldown ring animation on UI.
* **Day 21:** Matchmaking Queue (`BackgroundService`). Pair players within ±200 RP every 5 seconds.

### Week 4 — Polish & Deployment

- Goal: Arenas, custom lobbies, styling, end-to-end testing, and cloud deployment.

* **Day 22:** Arena Promotion system & Match filtering by arena.
* **Day 23:** Global Leaderboard page. Paginated top 50 users by RP list.
* **Day 24:** Custom Lobbies (Room Generation). 6-character room codes, host configurations.
* **Day 25:** Lobby waiting room UI. Ready buttons, SignalR live chat, Share Room link.
* **Day 26:** Global Styling. Dark mode theme, SVG badges, crown pulse animation.
* **Day 27:** Layout Refinement. CSS Grid for IDE panel and progress panel. Ensure 1280px & 1920px compatibility.
* **Day 28:** E2E Testing. Test two accounts (Chrome/Firefox), cast spells, check scoring. Log bugs.
* **Day 29:** Fix Bugs, Error Handling. Handle Judge0 timeouts, 404/500 error pages.
* **Day 30:** Deployment. Free Azure App Service deployment, configure environment variables for Connection String and Judge0 Key. Celeberate!

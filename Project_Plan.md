# Clash of Codes - Updated Build Guide v3.0

## 1. The Core Game Loop & Resource Economy

### Battle Elixir (BE)

- **Capacity**: Players start at 0 BE; hard cap is 10 BE.
- **Passive generation**: 1 BE every 10 seconds. In the final 60 seconds (Double Elixir mode): 2 BE every 10 seconds.
- **Active generation**:
  - Successful compilation: +1 BE
  - Flawless first-submission run: +3 BE
  - Correct MCQ answer: +1 BE

### Rank Points (RP) & The Greed vs. Power Economy

- **Final RP Awarded** = Match Base Victory RP + (Unspent BE at match end × 5)
- **Loss-streak protection**: After 3 consecutive losses, the RP penalty for the next match is halved.

---

## 2. Match Structure & Victory Conditions

- **Left Track (Easy)**: Yields 1 Crown on completion.
- **Right Track (Medium)**: Yields 1 Crown on completion.
- **Centre Track (Hard)**: King’s Tower — completing this triggers an instant 3-Crown absolute victory, overriding all other conditions.
- **Node Workflow**: Stage I (Code Composition - pass public test cases) -> Stage II (MCQ Stream - 3 to 5 questions).

---

## 3. The Spellbook Mechanics

Players assemble a loadout of exactly 4 Spells (2 Offensive + 2 Defensive).
Recommended v1 launch with 6 spells only (3 offensive + 3 defensive).

**Offensive Spells (Examples)**:

- **Fog of War (2 BE)**: Hides compiler line numbers for 45s.
- **Time Warp (3 BE)**: Opponent’s timer spins at double speed visually for 30s.
- **MCQ Barrage (5 BE)**: Injects 2 high-difficulty MCQ items into opponent’s queue.

**Defensive Spells (Examples)**:

- **Lint Shield (2 BE)**: Immune to flickering, theme changes, typo curses for 60s.
- **Ctrl+Z Rollback (4 BE)**: 3s reactive window restoring code buffer after Code Swap.
- **Garbage Collector (5 BE)**: Purges all active debuffs and visual distortions.

_(Added 30-60 second per-slot cool-downs & a cap of max 2 active offensive debuffs)._

---

## 4. Matchmaking & Arenas

- **Ranked Ladder**: Pairs players within ±200 RP.
- **Custom Lobbies**: 6-character alphanumeric key. Topic filter, difficulty cap, Elixir mode.
- **Arenas**: 10 Tiered Arenas ranging from _Spaghetti Junction_ (0-499 RP) up to _Kernel Palace_ (5000+ RP). Each new arena unlocks an additional spell.

---

## 5. Architecture Reference (Two-Project Setup)

**ClashOfCodes.API (Backend):**

- ASP.NET Core 8 Web API
- EF Core + SQLite
- ASP.NET Identity & JWT token generation
- SignalR Hub (Real-time Game State)
- Judge0 Service integrations

**ClashOfCodes (Frontend):**

- Blazor Server
- HttpClient for all API calls
- Custom AuthStateProvider taking JWT from localStorage
- Monaco Editor (JS Interop)
- SignalR Client for live match states

_Both deployed to separate Azure App Services._

---

## 6. Revised 4-Week Build Plan

### Week 1 — Foundations (Days 5-7)

_(Days 1–4: Models, DbContext, Migrations, SQLite base already completed and migrated to two projects)_

- **Day 5 (API)**: Add Identity, JWT generation, and AuthController endpoints (`/register`, `/login`).
- **Day 6 (Blazor)**: Login/Register UI. Store JWT in `localStorage`. Build `AuthStateProvider`.
- **Day 7 (Both)**: Attach JWT to HTTP requests implicitly via `JwtAuthHandler`. Protect routes and wire up `Profile.razor`.

### Week 2 — Core Game Loop (Days 8-14)

- **Day 8 (API)**: JudgeService wrapping Judge0 API. POST `/api/submit` endpoint.
- **Day 9 (Blazor)**: Monaco Editor embedding. Submit flow passing code and displaying runtime results.
- **Day 10 (API)**: MCQ endpoints (`/api/mcq/{problemId}`) with server-side answers evaluation & BE delta calculations.
- **Day 11 (Blazor)**: MCQ panel, BE bar, and Crown tracker visual components.
- **Day 12 (Blazor)**: Link components up in `Game.razor` via CSS grid layout. Test the solo loop.
- **Day 13 (Both)**: Calculate Match results (Final RP math) server-side and wire up the results screen in Blazor.
- **Day 14 (Both)**: Buffer for bug fixes. Read SignalR documentation prior to Week 3.

### Week 3 — Multiplayer & Spells (Days 15-21)

- **Day 15 (API)**: SignalR `GameHub` and in-memory Match State dictionary.
- **Day 16 (Blazor)**: Connect `HubConnectionBuilder` using JWT. Set up opponent progress bars via WebSockets.
- **Day 17 (Both)**: Double Elixir at T-60s broadcast. Setup full match-end conditions and 60-second disconnect handler.
- **Day 18 (API)**: Create authoritative `CastSpell` hub capability and deduct BE properly.
- **Day 19 (Blazor)**: Hook up Offensive spell UI responses (Fog of War, Syntax Poison, MCQ Barrage bindings).
- **Day 20 (Both)**: Hook up Defensive spell UI and the dynamic Spell Hotbar component.
- **Day 21 (Both)**: Spin up `MatchmakingQueue` BackgroundService to pair queued players & assign problems.

### Week 4 — Polish & Deployment (Days 22-30)

- **Day 22 (Both)**: Hook up Arena threshold promotions & badge animations matching RP numbers.
- **Day 23 (Both)**: Add global paginated Leaderboard via `/api/leaderboard`.
- **Day 24 (API)**: Private Lobby `/api/rooms` generation and routing.
- **Day 25 (Blazor)**: Lobby waiting room UI, live chatting, 'Ready' toggling.
- **Day 26 (Blazor)**: Global visual pass (CSS custom vars, Spell animations, SVG icons).
- **Day 27 (Blazor)**: Keyboard bindings (Ctrl+Enter to submit code), loading spinners during API calls.
- **Day 28 (Both)**: End-to-end parallel testing across dual browsers. Log issues.
- **Day 29 (Both)**: Clean up bugs and harden error boundaries & HTTP timeouts (Judge0 handling).
- **Day 30 (Both)**: Execute deployment steps to dual Azure App Services. Setup secure environment variables.

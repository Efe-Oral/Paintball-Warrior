# CLAUDE.md — Paint Coverage Game (working title)

## What this document is

The complete design spec and build plan for a mobile hybrid-casual game. Every
decision below was settled deliberately during a planning session. Treat locked
decisions as locked. If something here seems wrong, say so before building, do
not silently change it.

---

## Working agreement

**Build one step at a time. Stop after each step and wait for feedback.**

The developer builds, tests in the editor, gives feedback, and only then do we
move to the next step. Do not build ahead. Do not implement features from later
steps because they seem convenient. A step is done when it runs and the
developer has confirmed it feels right.

When a step is complete, state plainly what was built, what to test, and what
the next step would be. Then stop.

---

## Project context

- **Developer:** solo, part-time, experienced with C#, Unity, and game design.
  First mobile game.
- **Goal:** revenue. Build a polished vertical slice, pitch it to a
  hyper-casual / hybrid-casual publisher (Voodoo, Homa Games, SayGames,
  Supersonic). Publisher funds user acquisition in exchange for revenue share.
- **Bar to clear:** roughly 40% day-1 retention, 10% day-7 retention.
- **Genre:** hybrid-casual. Simple core mechanic, thin meta-progression layer.

## Technical environment

- **Unity 6.3 LTS (6000.3)**, 3D.
- **Development on Windows.** No Mac, so no iOS builds for now. Android only.
- **Target:** mobile, portrait orientation, single-screen levels, no scrolling.
- **Input must work two ways during development:**
  - Keyboard (WASD / arrow keys) for fast iteration on the PC.
  - On-screen virtual joystick, testable via Unity's Device Simulator.
  - Use the Input System package with a single action map so both bind to the
    same movement action. No separate code paths.
- **Performance target:** a mid-range Android phone, not a flagship. The
  coverage system is the main performance risk. Profile it early.
- **Art:** grey primitives only. Cubes, capsules, planes. No models, no
  textures, no materials beyond flat colors. Art comes much later and only
  after the core loop is proven fun.

---

## The game in one paragraph

You control a character in a small enclosed arena, seen from a fixed top-down
isometric camera. The character continuously sprays paint in whatever direction
it is moving. The goal is to cover the floor, and any paintable structures, in
paint before the paint tank runs dry. Obstacles block the spray and leave
unpainted pockets behind them, so the real skill is planning an efficient route
and approaching things from the right angles.

---

## Locked design decisions

### Camera
Fixed top-down isometric. **Does not rotate. Does not follow player facing.**
Brawl Stars style. The whole arena fits on screen at once.

### Controls
- Single virtual joystick (plus keyboard during development).
- **Auto-fire while moving.** The gun always sprays in the movement direction.
- No aim stick. No fire button. No control-scheme options or settings toggle.
- The gun's visual direction follows the movement direction.

### Objective
- **Ground coverage:** percentage of the arena floor painted.
- **Structures:** discrete objects (crates, pillars) that must be **fully
  painted** to count. Binary, no partial credit.
- A level is passed at a **coverage threshold of roughly 85-90%** (tunable).
  Never require 100%, that produces unfair, illegible failures.

### Constraint and failure
- **Paint tank only. No timer.**
- One tank per level. **No refill stations in v1.**
- When the tank hits zero below the threshold, the level fails immediately.
- On failure: offer a rewarded ad for a fresh tank. **One continue per level
  attempt.** An ad-rescued run is capped at 1 star. Fail again and restart the
  level.

### Stars
Based on **paint remaining at completion**. Rough shape: 1 star at the
threshold, 2 and 3 for finishing with more paint left. Ad-rescued runs cap at 1
star so stars stay meaningful.

### Weapons
**Sidegrades with mild power creep.** Each gun answers a specific problem the
arena creates: open floor, structure faces, or paint shadows. Later guns have
slightly better raw numbers but keep a real weakness.

| Gun | Role | Trade-off |
|---|---|---|
| Stream Gun | Free starter, medium everything | Deliberately unremarkable baseline |
| Fan Sprayer | Wide cone, fast floor coverage | Heavy overspray, bad on structure faces |
| Pressure Jet | Narrow, long range, most paint-efficient | Slow across open floor |
| Lobber | Arcs over obstacles, big splat | Slow fire rate, imprecise. Only gun that solves shadows without repositioning |
| Scattergun (optional) | Chunky spread, mild upgrade over Stream | Leaves gaps that need a second pass |

- Bought with coins.
- **Selected before the level. No mid-level switching.**
- **No weapon is ever required to clear a level.** Every level must be beatable
  with the Stream Gun. Better guns make it easier and improve star rating, they
  never gate progress.

### Economy
- Coins earned per level, scaled by stars. Replaying an old level for a better
  rating pays more.
- Coins buy weapons only in v1.
- Balance numbers get tuned after there is a playable build. Start generous.

### Ad placements (three, all outside gameplay)
1. **Continue on failure** — out of paint, below threshold. Primary revenue.
2. **Double coins** on the level-complete screen.
3. **Interstitial** every 3-4 levels, non-rewarded, low frequency.

Nothing interrupts active gameplay. This is deliberate: retention is what the
publisher judges, and a game that feels like a toll booth gets uninstalled.

### Content plan
- 15-20 levels, **linear chain**. No world map.
- **Level 1:** no-fail sandbox. Open room, no obstacles, no structures,
  generous tank. One gun. A visible **locked weapon rack** on the wall to plant
  desire. **No text, no popups, no tutorial UI.**
- **Level 2:** adds structures (crates). Still no obstacles.
- **Level 3:** adds obstacles, tall pillars, blocked sightlines, paint shadows.
  Tighter tank.

---

## Technical architecture

### Coverage system (the one real unknown)

**An invisible tile grid, fully decoupled from the visuals.**

- The floor is divided into invisible cells. Each flags painted or unpainted.
  Coverage percentage is a simple count.
- The **visuals are separate**: splat decals with irregular, soft, organic
  edges, random rotation, varied size. The player never sees a grid.
- Cell size is **tunable** and controls percentage *granularity*, not visual
  quality. Too coarse and the counter jumps in chunks and thin unpainted
  slivers read as covered. Too fine and you are tracking too many cells on a
  phone.
- A splat should generously cover whatever cells it flags, so nothing ever
  looks painted but counts as unpainted.
- Do not recount every cell every frame. Use incremental counting.

### Paint shadows
Paint travels in a straight line from the gun. Obstacles block it, leaving
unpainted pockets behind them. This is the core skill of the game, not a bug.

**Level design constraint:** the fixed camera must be able to *show* every
pocket the player needs to fill. A shadow hidden from the camera makes a level
unfair.

### Configuration
**Every tunable number lives in a ScriptableObject or JSON config. Nothing
hardcoded.** Tuning must never require a recompile.

Starting placeholder values (round numbers, all to be balanced later):
- Paint tank: 100 units
- Drain rate: 10 units/sec
- Move speed, spray cone width, spray range: pick sensible defaults
- Coverage threshold: 85%
- Coins per star, gun prices: generous to start

### Analytics
Wire in from day one. Cannot be retrofitted, and retention data is what gets a
publisher to sign. Track: level started, level completed, level failed, ad
offer shown, ad watched, ad declined, session length, and **where players
quit**.

Keep ad SDK calls behind a thin wrapper so the vendor can be swapped in one
file. A publisher will likely mandate their own stack.

---

## Build order

Each step ends with a working, testable build. **Stop and wait for feedback
after every step.**

**Step 1 — The core moment, nothing else.**
Grey floor plane. A capsule that moves with keyboard and virtual joystick.
Auto-fire spray while moving. The coverage tile grid. Splat visuals. A
percentage counter on screen. A paint tank that drains and stops spraying at
zero.

No structures, no obstacles, no menus, no coins, no ads, no levels, no fail
state, no win state. Just a room you can paint until you run dry.

**This step decides whether the game is worth making.** If spraying paint and
watching the number climb is not satisfying within the first minute, nothing
built on top of it will save the game. Do not proceed past this step until the
developer confirms it feels good.

**Step 2 — Win and fail.**
Coverage threshold. Level-complete state. Level-fail state when the tank empties
below threshold. Star calculation from remaining paint. Restart.

**Step 3 — Structures.**
Paintable crates with their own coverage tracking. Binary completion. A clear
visual "done" state for each structure.

**Step 4 — Obstacles and paint shadows.**
Geometry that blocks the spray. Verify shadows are visible from the fixed
camera.

**Step 5 — Level flow.**
Multiple levels in a linear chain. Level loading, progression, persistence of
which levels are unlocked and their star ratings.

**Step 6 — Weapons.**
The gun data model as ScriptableObjects. Pre-level selection UI. Implement the
Stream Gun and Fan Sprayer first, then the rest.

**Step 7 — Coins and shop.**
Coin awards from stars. Persistence. Weapon purchase screen.

**Step 8 — Ads and analytics.**
The three placements behind a wrapper. Analytics events.

**Step 9 — Build out levels 1-20 and tune.**
This is where the paint budget versus surface area balancing actually happens.

---

## Explicitly out of scope

Do not build any of these without being asked:

- Multiplayer or networking of any kind
- An AI rival opponent (planned for a later phase)
- Health, damage, or combat
- Mid-level weapon switching
- Refill stations
- A second control scheme or control settings
- Hazards (rain, wind, fog, puddles)
- Levers, buttons, gates, sequential unlock levels
- Daily rewards, battle pass, cosmetics, IAP
- Real art, models, textures, animation
- Timers as a level constraint
- A world map
- Text tutorials or popup instructions

---

## Things to push back on

If asked to do any of these, say so:

- Requiring 100% coverage to pass a level
- Making any weapon mandatory to progress
- Adding ad placements inside active gameplay
- Hardcoding tunable numbers
- Coupling the coverage grid to the splat visuals
- Building art before the core loop is confirmed fun
- Building more than one step ahead

# Paintball Warrior

> A mobile arena game about covering everything in paint.

<img width="2048" height="1143" alt="Paintball Warrior banner" src="https://github.com/user-attachments/assets/e5054dda-ec6f-46cc-80f0-131c41444aaa" />

---

## What it is

You control a character in a small arena, seen from a fixed top-down isometric
camera. Your paint sprayer fires continuously while you move, so every step you
take leaves color behind you. The goal is simple: cover the floor, and every
crate and pillar in the arena, before your paint tank runs dry.

The catch is that paint travels in a straight line. Obstacles block it, leaving
unpainted pockets tucked behind them that you can only reach by walking around
and spraying from a different angle. So what looks like a game about running
around and making a mess is really a game about planning a route and not
wasting a drop.

## How it plays

- **One thumb.** A single joystick moves you, and the sprayer fires
  automatically in whatever direction you are heading. No aiming, no fire
  button.
- **One resource.** You get one tank of paint per level. When it runs out, it
  runs out.
- **Short levels.** Each one runs 30 to 90 seconds on a single screen, with no
  scrolling and nothing hidden off-camera.
- **Stars for efficiency.** Finishing the level is one thing. Finishing it with
  paint still in the tank is what earns three stars.

## Weapons

Each sprayer answers a different problem the arena creates, and none of them is
strictly better than the others.

| Sprayer | Good at | Bad at |
|---|---|---|
| **Stream Gun** | Everything, a little | Everything, a little |
| **Fan Sprayer** | Blanketing open floor fast | Wasteful, poor on crate faces |
| **Pressure Jet** | Paint efficiency, long range | Slow across open ground |
| **Lobber** | Arcing paint over obstacles | Slow, imprecise |

You pick one before you enter a level and live with the choice.

## Status

**In development.** Currently building the core mechanic as a grey-box
prototype, with no art at all. The reasoning: if running around a grey room
spraying paint at grey cubes is not fun, no amount of art will fix it. Visual
polish comes only once the loop is proven.

### Roadmap

- [ ] Core loop: movement, spraying, coverage tracking, paint tank
- [ ] Win and fail states, star rating
- [ ] Paintable structures
- [ ] Obstacles and paint shadows
- [ ] Level progression
- [ ] Weapon roster and selection
- [ ] Coins and unlocks
- [ ] Art pass
- [ ] Release

### Planned for later

An AI rival painting the same arena against you, environmental hazards like
rain that thins your paint and wind that pulls your spray off course, and
levels with levers and gates that open up sealed sections of the map.

## Built with

- **Unity 6.3 LTS** (6000.3), 3D
- **C#**
- Target platform: Android, portrait orientation

## Development notes

Coverage is tracked on an invisible tile grid that is kept completely separate
from what you see. The splats on screen are decals with soft irregular edges,
so the paint looks organic and chaotic while the math underneath stays cheap
enough to run on a mid-range phone.

Every tunable value (tank size, drain rate, spray width, coverage threshold)
lives in configuration assets rather than in code, so balancing never requires
a recompile.

---

*Made by Efe Oral. Follow along for progress updates.*

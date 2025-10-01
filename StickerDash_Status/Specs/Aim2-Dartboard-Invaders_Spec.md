# Aim2 Dartboard Invaders — Spec & Roadmap v0.1
**Date:** 2025-10-01  
**Repo:** git@github.com:andycann44/Aim2-Dartboard-invaders.git  
**Company/Brand:** Aim2 (sub-brand Aim2Pro for darts)  
**Editor Window Rule:** Window → Aim2Pro → Track Creator → Track Lab (All-in-One) only.

## Overview
Fixed-shooter (arcade homage) built from our Aim2 template. Clean, fast sessions (60–90s), original IP (Johnny + Aim2 style), ethical monetization.

## Current Implementation (MVP runtime bootstrap)
- Bootstrap builds a playable scene at runtime.
- Player: left/right move (A/D or arrows), shoot (Space/mouse), screen-edge clamp.
- Bullet: kinematic, destroys enemies on hit.
- Enemy formation: grid spawns, marches left↔right, steps down at edges.
- HUD mini: shows destroyed count; win state + press **R** to restart.
- URP 2D; simple generated sprites (no imported art yet).

## “Done” Checklist
- [x] Repo created & wired to GitHub
- [x] Visible Meta Files + Force Text (Unity project hygiene)
- [x] Runtime playable loop (player, bullets, invaders, win/restart)
- [x] Specs + New-Chat starter files added (this commit)

## To-Do (next passes)
**Core gameplay**
- [ ] Enemy shots + player hit detection
- [ ] Lives (e.g., 3) + Game Over screen
- [ ] Score system (per kill) + High Score (PlayerPrefs)
- [ ] Speed up as invaders die, wave progression

**UX/UI**
- [ ] Main menu, Pause, Settings
- [ ] Proper HUD (lives, score, wave), fonts/icons (Aim2 style)

**Audio**
- [ ] SFX (shoot, hit, step, win/lose), music loop, simple mixer

**Art**
- [ ] Swap generated shapes for Aim2 sprites (Johnny ship + enemies)
- [ ] Palette + background, small VFX on hit

**Monetization & Growth (stubs first)**
- [ ] Rewarded continue (stub), Remove Ads (non-consumable), Starter Pack
- [ ] “More Games” panel (reads StickerDash_Status/CrossPromo/more_games.json)
- [ ] Basic analytics events (session/level/kill/death/revive)

**Mobile readiness**
- [ ] On-screen controls (left/right/shoot) + aspect handling
- [ ] Android/iOS dev builds; tag v0.1.0

**Compliance**
- [ ] Privacy link + consent (GDPR/ATT), restore purchases (iOS)

## Definition of Done (v0.2 “Playable Mobile”)
- Player lives, enemy fire, game over, score/high score.
- One rewarded continue (stub), Remove Ads button visible.
- Android dev build installs; 60 FPS on test phone; no red console errors.

## Notes
Keep one editor window (Track Lab). Units default to meters; tiles must touch; width 3m unless overridden. Use `./a2p_snap.sh` before risky changes. Tag releases (`v0.1.0`, `v0.2.0`, …) instead of making new repos.

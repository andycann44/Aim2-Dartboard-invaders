# Aim2 Dartboard Invaders — Spec & Roadmap
**Date:** 2025-10-04  
**Repo:** git@github.com:andycann44/Aim2-Dartboard-invaders.git  
**Brand:** Aim2 (Aim2Pro = darts sub-brand)  
**Editor Window:** Window → Aim2Pro → Track Creator → Track Lab (All-in-One)

## Overview
Fixed-shooter homage using our Aim2 template. Short 60–90s sessions, original art/names, ethical monetization.

## Current Implementation
- **Bootstrap runtime scene** (no manual setup).
- **Input System** controls (keyboard/mouse/gamepad).
- **Bullets/enemies use cached sprite** (stability; no runtime texture churn).
- **Enemy Formation**: marches left↔right, steps down at bounds.
- **Level progression**: clearing a wave spawns the next with **+columns** (and **+row every 3 levels**), and higher speed.  
  - Level 1 **slower**: ; ramp:  speed per level.
- **HUD mini**: shows Level N, destroyed/total; restart on **R**.
- **Tools**: Quick Actions (Terminal ⌘T, Save→Commit→Push), Auto-PR (⌘⇧P).
- **Repo hygiene**: Visible Meta Files + Force Text.

## Done ✅
- Repo on GitHub (LFS enabled)
- Input System migration
- Level progression & pacing (slower L1; gentle ramp)
- Sprite cache (stability)
- Unity Tools menus + shortcuts (⌘T, ⌘⇧P)
- Specs/Notes tracked in 

## To-Do ▶
**Core gameplay**
- Enemy shots + player lives + Game Over
- Score per kill + High Score (PlayerPrefs)
- Speed-up per remaining invaders, wave progression polish

**UX/UI**
- Main Menu, Pause, proper HUD (lives/score/level)

**Audio**
- SFX (shoot/hit/step/win/lose), music loop, basic mixer

**Art**
- Replace temp sprites with Aim2 assets (player/enemies/bg/VFX)

**Monetization**
- Rewarded continue (stub→live), Remove Ads, Starter Pack
- “More Games” panel (reads CrossPromo JSON), analytics events

**Mobile**
- On-screen controls + aspect handling, Android/iOS dev builds

**Compliance**
- Privacy link + consent (GDPR/ATT), restore purchases (iOS)

## Definition of Done (v0.2 “Playable Mobile”)
- Lives, enemy fire, Game Over, score/high score
- Rewarded continue (stub) + Remove Ads button visible
- Android dev build @ 60fps, no red console errors

## Changelog
- 2025-10-04: Stability + pacing pass (sprite cache; slower L1, gentler ramp). Spec refreshed.
- 2025-10-02: Tools menus & shortcuts; spec/prompt updates.
- 2025-10-01: Initial MVP + repo/docs.

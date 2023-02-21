---
layout: post
title: "Player Hud Widget"
date: 2022-11-26 14:43 -0300
badge: docs
categories: [Documentation, Overlay, Widgets]
---
The Player Hud Widget is responsible for tracking and displaying the data related to your character's health and stamina, as well as tracking your weapon's sharpness.

## Widget Structure

![player-hud-structure](/Static/player-hud-structure.png)

## Health bar

The health bar can be broken down into 5 pieces:

- **Maximum Possible Health:** This is the white bar that's behind all the other bars, it represents your maximum possible health, in Monster Hunter Rise, this is the bar that shows how much your actual health can be expanded to by consuming the green birds.
- **Max Health:** This is represented by the dark bar behind the recoverable health, heal and health.
- **Recoverable health:** This is your health that can be recovered naturally over time.
- **Heal:** This is the bar that will represent the healing when you're consuming an item that can heal your life, it's represented by the light green bar.
- **Health:** This is your actual health, it's represented by the green bar.

## Stamina Bar

The stamina bar can be broken down into 4 pieces:

- **Maximum Possible Stamina:** This is the maximum possible stamina you can have, it's represented by the white bar that's behind all the other bars. In Monster Hunter Rise, this is the bar that shows how much your actual stamina can be expanded to by consuming the yellow birds.
- **Max Stamina:** This is the bar that shows you what's your current maximum stamina, represented by the dark bar behind the stamina bar.
- **Recoverable stamina:** This bar will show how much stamina you can recover by eating rations or meat, it's represented by the hollow bar with the red border around it. This only happens in **Monster Hunter Rise**.
- **Stamina:** This is your actual stamina, it's represented by the orange-ish bar.

## Abnormalities

When your character is suffering from a debuff, your health bar or stamina will change it's color to match the element that is currently active.

<div style="display:flex;">
    <div style="display:block;background-image:linear-gradient(90deg, #CF5F3E, #CF733E);height:30px;width:100px;text-align:center;color:#1b1b1e;">Fire</div> 
    <div style="display:block;background-image:linear-gradient(90deg, #563ECF, #7445DE);height:30px;width:100px;text-align:center;color:#1b1b1e;">Poison</div> 
    <div style="display:block;background-image:linear-gradient(90deg, #7F0F29, #CF3E5F);height:30px;width:100px;text-align:center;color:#1b1b1e;">Bleed</div> 
    <div style="display:block;background-image:linear-gradient(90deg, #deb100, #ffd942);height:30px;width:100px;text-align:center;color:#1b1b1e;">Nat. Heal</div> 
    <div style="display:block;background-image:linear-gradient(90deg, #4973d6, #87abff);height:30px;width:100px;text-align:center;color:#1b1b1e;">Water</div> 
    <div style="display:block;background-image:linear-gradient(90deg, #6bb7e3, #93d1f5);height:30px;width:100px;text-align:center;color:#1b1b1e;">Ice</div> 
    <div style="display:block;background-image:linear-gradient(90deg, #5e4800, #a67f00);height:30px;width:100px;text-align:center;color:#1b1b1e;">Effluvia</div> 
</div>

## Sharpness

HunterPie tracks your sharpness, and displays it in a gauge. The colors for each level are the following:

<div style="display:flex;">
    <div style="display:block;background:#D13232;height:30px;width:100px;text-align:center;color:#1b1b1e;">Red</div> 
    <div style="display:block;background:#F78B00;height:30px;width:100px;text-align:center;color:#1b1b1e;">Orange</div> 
    <div style="display:block;background:#F7E024;height:30px;width:100px;text-align:center;color:#1b1b1e;">Yellow</div> 
    <div style="display:block;background:#5BF77F;height:30px;width:100px;text-align:center;color:#1b1b1e;">Green</div> 
    <div style="display:block;background:#49B2E3;height:30px;width:100px;text-align:center;color:#1b1b1e;">Blue</div> 
    <div style="display:block;background:#EAF5F4;height:30px;width:100px;text-align:center;color:#1b1b1e;">White</div> 
    <div style="display:block;background:#cd55e6;height:30px;width:100px;text-align:center;color:#1b1b1e;">Purple</div> 
</div>

It's also possible to override those colors with theme files, in case you want custom colors for each level.

```xml
<Color x:Key="WIDGET_SHARPNESS_BROKEN">#000000</Color>
<Color x:Key="WIDGET_SHARPNESS_RED">#D13232</Color>
<Color x:Key="WIDGET_SHARPNESS_ORANGE">#F78B00</Color>
<Color x:Key="WIDGET_SHARPNESS_YELLOW">#F7E024</Color>
<Color x:Key="WIDGET_SHARPNESS_GREEN">#5BF77F</Color>
<Color x:Key="WIDGET_SHARPNESS_BLUE">#49B2E3</Color>
<Color x:Key="WIDGET_SHARPNESS_WHITE">#EAF5F4</Color>
<Color x:Key="WIDGET_SHARPNESS_PURPLE">#cd55e6</Color>
```
{: file='SharpnessColors.xaml'}

## In-Game HUD

To avoid cluttering the screen, you can disable the in-game HUD:

### Monster Hunter Rise

![in-game-hud-settings-no-hp](/Static/in-game-hud-settings-no-hp.png)
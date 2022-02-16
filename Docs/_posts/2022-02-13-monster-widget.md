---
layout: post
title: "Monster Widget"
date: 2022-02-14 19:03 -0300
badge: docs
math: true
categories: [Documentation, Overlay, Widgets]
---
The Monster Widget is responsible to track and display all the big monsters information. It is one of the most complex widgets and each single component is designed to be simple without losing information.

## Widget Structure

![monster-widget-structure](/Static/monster-widget-structure.png)

### Monster Parts

Parts are represented by the blue, yellow and red bars under the ailments component. Their visual representation depends on what type of part it is:

Color | Represents | Description
:----:|:----------:|--------------------
<ion-icon name="prism" style="fill:#22aae1;"/> | `Flinch` | Flinch values represent when the monster is going to flinch or get staggered.
<ion-icon name="prism" style="fill:#fdc45b;"/> | `Break`  | Break values represent when a part is about to break.
<ion-icon name="prism" style="fill:#e53737;"/> | `Sever` | Severable parts are the ones that can be cut off from a monster.
<ion-icon name="prism" style="fill:#7f7f7f;"/> | `Broken` | A part will become Grey when it's either broken or severed. 

When a part has all three values, the priority order is **always** `Sever` > `Break` > `Flinch` and the exact values displayed under the part health bar will follow that priority.

> By default, HunterPie only shows monsters's parts when you lock/focus on the monster using the in-game lock-on system.
{: .prompt-info }

### Monster Ailments

Ailments are statuses and debuffs you can inflict on a monster, HunterPie supports all of them, however, some of them might display as `Unknown` since ailments are mapped manually and require testing.

They're designed to be as simple to read as possible, displaying Build Up, Duration and also how many times that Ailment has been activated on that monster. Each ailment has it's own individual color to make it easier to know what ailment has been inflicted without having to read it's name.

> By default, HunterPie only shows monsters's ailments when you lock/focus on the monster using the in-game lock-on system.
{: .prompt-info }

### Targeting a monster

Having to target a monster to see their information is part of HunterPie's design to avoid cluttering the screen with multiple monsters information, to target a monster, all you need to do is use the in-game lock-on system. 

> If you have targeting system disabled in-game, this will not work. You must have either **Target** or **Focus** enabled.
{: .prompt-warning }

### Orientation

Monster Widget supports two orientations, `Vertical` and `Horizontal`


Orientation | Description
:----------:|:--------------------
Vertical    | Monster health bars will be placed on top of each other in the order they spawn
Horizontal  | Monster healht bars will be placed side by side in the order they spawn

![horizontal-bars-demo](/Static/horizontal-bars-demo.jpg) *Horizontally aligned bars*

### Dynamic Resizing

Dynamic resizing is one of the Monster Widget's features, it's very useful when your widget is in the `Horizontal` mode, it tries to calculate the health bar's width dynamically instead of having a static width based on how many monsters the widget is displaying at that moment.
The width is calculated based on the width set as `Minimum Width`, using the following formula:

$$ dynWidth = min + ((3 - n) * {min \over 4}) $$ 

- **min:** Minimum Width
- **n:** Number of monsters visible

So, if you set the minimum width as `300`, each possible case will result in these dynamic widths:

Monsters visible | Width (px)
:---------------:|:----------------
3                | 300
2                | 375
1                | 450

> **Note:** Even if the dynamic width is higher or lower than the maximum and minimum width respectively, the visual width will not go above/below those widths.
{: .prompt-note }
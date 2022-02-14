---
layout: post
title: "Abnormality Tray Widget"
date: 2022-02-13 13:23 -0300
badge: docs
categories: [Documentation, Overlay, Widgets]
---
The Abnormality Tray widget allows you to track your buff and debuff durations, it's useful when playing Hunting Horn, or when you just want to know how long a certain buff will last. HunterPie lets you create as many trays as you want, allowing you to separate abnormalities in different trays.

> Keep in mind the more widgets you have on your screen, the more it impacts performance.
{: .prompt-danger }

## Creating a new tray

To create a new tray, open HunterPie's settings in the `Abnormality Trays` tab and click on the <ion-icon name="add-circle"/>, in case you want to delete a specific tray, click on the <ion-icon name="remove-circle"></ion-icon>, it will prompt you with a confirmation so you can delete the tray.

> If you're creating new bars while the game is running, you'll have to restart HunterPie in order for it to visually create the bar widget.
{: .prompt-warning }

## Configuring the tray

To configure your new tray, click on the <ion-icon name="settings-sharp"/>. It will open a new window that lets you configure all the tray settings individually.

![abnormality-tray-config](/Static/abnormality-tray-config.png)

### Enabling abnormalities

In the configuration window, there's a panel with all the available abnormalities, clicking on one of them will automatically enable tracking that abnormality in that individual tray.

Icon | Description
:---:|-------------
<ion-icon name="checkmark-circle" style="fill:#66e2a7;"> | Enabled
<ion-icon name="checkmark-circle"/> | Disabled

### Orientation

Orientation is the setting that indicates which direction your tray will grow to fit all abnormalities, setting it to `Horizontal` will make the tray grow sideways until it reaches its maximum width, on the other hand, `Vertical` will make the bar grow downwards until it reaches the maximum height.

### Maximum Size

Setting a maximum size to your tray will cause it to not grow infinitely, wrapping new abnormalities downwards (if the orientation is `Horizontal`) or sideways (if the orientation is `Vertical`).
---
layout: post
title: "Chat Widget"
date: 2022-03-31 23:54 -0300
badge: docs
math: true
categories: [Documentation, Overlay, Widgets]
---
The Chat Widget is responsible to display in-game messages from other players.

## Widget Structure

![chat-widget-structure](/Static/chat-widget-structure.png)

### Why does it exist?

Monster Hunter: Rise's built-in chat was made for Switch and not for PC, making it really difficult to read messages and also keep track of them. Whenever you get a message, it stays on screen for about 10 seconds and then fades away, if you want to read the message again, you are obligated to go to your in-game settings, open the chat messages and then open the chat, which can become very annoying if you play online with friends often.

HunterPie's chat solves this issue by showing **only** the player messages, and it also shows the widget whenever you press <kbd>Enter</kbd> to type in the chat.

> **Warning:** Preset messages will not be shown in the chat.
{: .prompt-danger }
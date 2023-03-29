---
layout: post
title: "The development of HunterPie"
description: "Talking a little bit about the development of HunterPie."
date: 2023-03-28 02:43 -0300
badge: blog
pin: true
categories: [Blog, UI]
image:
    src: /Static/evolution-of-ui.png
---

> If you enjoy HunterPie and want to support its development, consider supporting on [Patreon](https://www.patreon.com/HunterPie)!
{: .prompt-support }

## Some context

I have been actively developing and maintaining HunterPie and all its dependencies for about 4 years now, the reason why I'm writing this post is not only to show the progress of HunterPie itself, but to also document the progress I had as a software engineer myself.

### How it all started...

It all started back in 2018 with Monster Hunter World, at the time there was 2 overlays that people used: [Smart Hunter](https://www.nexusmods.com/monsterhunterworld/mods/793) and [Hello World](https://www.nexusmods.com/monsterhunterworld/mods/142); As someone who is interested in UI and good looking applications, none of them really stood out to me, not only that, but both of them were pretty limited and not really user friendly, in order for you to customize them, you would need to open JSON files in your text editor.

With that in mind, I decided to make my own, not only to make something that would look better to me, but that would also help me learn more about software engineering, reverse engineering, UI/UX principles and so on.

## First Iterations

### HunterPy

HunterPy was made in Python (hence the **Py** at the end), a dynamic programming language that is really easy to understand due to its simple syntax, however, is not the best language to make good looking UIs and also not the best language to deal with memory reading, the heart of HunterPie. Despite those, I learned a lot while developing it and used a lot of the fundamentals to implement the next iteration of HunterPie.

After some time working on HunterPy, I had a simple overlay working pretty alright, all it had was monster health bars but it was enough to get some friends to use it and test it for me.

![hunterpy](/Static/hunterpy.png){: width="500" } *HunterPy user interface*

Some key features that I always wanted to keep were:

- Discord Rich Presence
- The overlay
- A console
- Settings that you could interact with, instead of a `configuration.json` file

One of the problems with making windows with Python is that you can't just compile the whole software into a single executable due to how Python works, there are some solutions but they generate a bunch of extra files or huge binaries that would be really painful to distribute if I ever wanted other people to use it. 

That's when I decided to rewrite everything in another language, something I could compile and just distribute in a nice little package without worrying too much about the package size.

### The first iteration...

The reason why HunterPie is called HunterPie is because it didn't make sense to keep the *Py* suffix anymore as the application was no longer being developed in Python, but I really liked how HunterPy sounded, so I just changed the suffix to keep the same sound without making a reference to Python.

The next iteration of HunterPie's user interface looked like this:

![hunterpie alpha](/Static/hunterpie-v1-alpha.png){: width="500" } *HunterPie v1 alpha client*
![hunterpie alpha settings](/Static/hunterpie-v1-alpha-settings.png){: width="500" } *HunterPie v1 alpha client settings*

The monster widget looked like this:

![hunterpie alpha monster widget](/Static/hunterpie-v1-alpha-monster-widget.png){: width="500" } *HunterPie v1 alpha Monster Widget* 

This iteration had not only the monster health bar, but also the Harvest Box tracker and Specialized Tool trackers.

### ...the next iteration...

The next iteration happened 4 months after the first release of HunterPie v1.0.0.0. I learned a lot more about the UI framework I was using and completely designed the client, not only that but I also created my own UI components for the settings window. This next iteration also had more features like: full localization support so other people could localize HunterPie to their languages; a damage meter widget and the abnormalities tracker widget.

![hunterpie v1 2nd iteration](/Static/hunterpie-v1-iteration-2.png){: width="500" } *HunterPie v1 client*
![hunterpie v1 2nd iteration settings](/Static/hunterpie-v1-iteration-2-settings.png){: width="500" } *HunterPie v1 client settings*

The two main widgets of HunterPie looked like this:

![hunterpie v1 2nd iteration monster widget](/Static/monster-widget-iteration-2.png){: width="500" } *HunterPie v1 Monster Widget*

![hunterpie v1 2nd iteration damage meter](/Static/damage-meter-v1.png){: width="500" } *HunterPie v1 Damage Meter Widget*

The design for the Damage Meter was inspired by [Shinra Meter](https://github.com/neowutran/ShinraMeter), a TERA damage meter overlay that I used a lot when playing the game back in the days.

### ...and the last iteration

The last iteration of HunterPie v1 happened a few months after the previous one, at that point HunterPie had many new features, new widgets for the overlay, it could export the player's current build to Honey Hunters, export all the charms and decorations your character had, it also had plugins support, HunterPie Native was also a thing, a way that HunterPie was able to communicate directly with the game which allowed many pretty cool things to happen.

In this iteration the design structure of the client didn't change much, but the colors and the settings page did. 

I wanted a cleaner design for the client, and changing the colors to a baby blue instead of red made things look more modern and less aggressive, not only the accent color changed but HunterPie's logo also changed to be white instead of red.

![hunterpie v1 last iteration](/Static/hunterpie-v1-last-iteration.png){: width="500"} *HunterPie v1 last iteration client*

Adding a blur effect to the background also made the client to look more modern, similar to Window's native acryllic effect.

As for the settings page, having every setting in a single component was getting way too cluttered and messy, so I added different tabs for each one of the features, the settings were also aligned in a way so the setting label would be anchored to the left and the actual control for that setting would be on the right-most side, that was a way to solve the weird alignment of the previous iteration.

![hunterpie v1 last iteration](/Static/hunterpie-v1-last-iteration-settings.png){: width="500"} *HunterPie v1 last iteration client settings*

The main widgets design also changed, the Monster Widget had another design overhaul to look more simple and modern:

![hunterpie v1 last iteration monster widget](/Static/hunterpie-v1-last-iteration-monster-widget.png){: width="500" } *HunterPie v1 last iteration Monster Widget*

And the damage meter got a damage graph!

![hunterpie v1 last iteration damage meter](/Static/hunterpie-v1-last-iteration-damage-meter.png){: width="500" } *HunterPie v1 last iteration Damage Meter Widget*

### HunterPie v2

The reason why I started rewriting HunterPie from scratch to what would become v2 is because its predecessor had a really messy code base, it got to a point where adding new features was really hard and adding support for the upcoming game (Monster Hunter Rise) would be actually impossible. Everything in v1 was heavily tied to Monster Hunter World, not only that but the whole UI was strongly tied to HunterPie's core code.

So for v2 I decided to fix some issues legacy had:

- **The client UI had to look more modern and clean:** I don't think legacy's UI was bad, but I don't think it was great either.
- **The settings page to be generated automatically:** This is one of legacy's biggest issues for me as a developer, everytime I added new settings I had to also create the UI components for it manually.
- **The settings should be saved automatically on change:** Legacy couldn't detect if the settings were changed, so the user had to click on the `Save` button everytime they made changes.
- **Support any Monster Hunter game:** Legacy could only support Monster Hunter World.
- **Write the code in a way that Core code is separate from the UI code:** Legacy code didn't have this distinction, core code could access the UI'd and vice-versa.

I will explain each one of those points and how I solved them in a later post, but here's the result:

![hunterpie v2](/Static/hunterpie-v2.png){: width="500" } *HunterPie v2 client*

The first thing you can notice is how the colors are much more consistent, they make sense together, it's no longer a mix of [warm and cold colors](https://artincontext.org/warm-colors/#:~:text=Report%20Ad-,What%20Are%20Warm%20Colors%3F,hue%2C%20will%20also%20be%20warm.). The icons are also perfectly aligned now, and they're all consistent and match the rest of the UI.

As for the settings window, I kept the same idea and structure of its predecessor, however it is now possible to search for settings by their label. HunterPie v2 is also capable of automatically saving the settings whenever something changes, so there's no need for a `Save` button anymore.

![hunterpie v2 settings](/Static/hunterpie-v2-settings.png){: width="500" } *HunterPie v2 client settings*

Now for the two most used overlay widget, the monster widget got a design overhaul to match the rest of the client UI, in fact, all widgets have gotten a design overhaul to make the UI consistent amongst themselves:

![hunterpie v2 monster widget](/Static/hunterpie-v2-monster-widget.png){: width="500" } *HunterPie v2 Monster Widget*

And the damage meter had a lot of UI improvements too:

![hunterpie v2 damage meter](/Static/hunterpie-v2-damage-meter.png) *HunterPie v2 Damage Meter Widget*

## Final Considerations

There were a lot of enhancements to HunterPie's user interface throghout time, most of the things were just trial and error, see what works and whatnot. Some came with time and experience after I started working as a software engineer and started having more contact with real world software.

The user interface is just the tip of the iceberg for HunterPie, there's a lot going on behind it, a lot of time and effort put into researching the games, designing and coding HunterPie's UI, coding internal things used in the core of HunterPie, coding the native part that is required for some features, coding the backend, maintaining all the infrastructure, etc.

And of course, each one of those individual pieces that make HunterPie work the way it does deserve their own post.

> **Fun fact:** I have spent +2,1K hours on HunterPie (Client and Server) and a couple hundred hours reverse engineering both Monster Hunter World and Monster Hunter Rise. Which is kinda insane, honestly.
{:.prompt-support}
---
layout: post
title: "Troubleshooting"
date: 2022-11-26 14:43 -0300
badge: docs
categories: [Documentation, Issues]
---

If you're having an issue with HunterPie like crash or freezes, do the following:

> If you're having freezing issues or crashes, skip to [Memory Dump](#memory-dump)
{: .prompt-danger }

## Memory Dump

Debugging a software without any data that can be analyzed is really hard, so if your HunterPie is *constantly* freezing, you can try generating a memory dump of HunterPie's process so I can analyze it and see if there's anything weird going on. To generate a memory dump, do the following:

1. Open the command prompt by opening Window's search and typing `cmd`
2. Run the following command
```powershell
dotnet tool install --global dotnet-gcdump
```
3. Once the tool finishes installing, open Windows's Task Manager (<kbd>Ctrl</kbd>+<kbd>Shift</kbd>+<kbd>Esc</kbd>), click on the `Details` tab and look for a `HunterPie.exe`, the details tab should have a column labeled as `PID`, you'll need to grab HunterPie's PID.
4. Run the following command, replacing `<PID>` with HunterPie's actual PID that you got from the previous step:
```powershell
dotnet gcdump collect -p <PID>
```

It will print something like:
```
Writing gcdump to '<PATH_TO_GCDUMP_HERE>'...
    Finished writing <NUMBER_OF_BYTES> bytes.
```

Now that you've dumped HunterPie's memory to a `.gcdump` file, just send that file to me on Discord: **Haato#0001** or in [HunterPie's official server](https://discord.gg/5pdDq4Q).
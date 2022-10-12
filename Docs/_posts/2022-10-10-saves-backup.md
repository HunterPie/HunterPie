---
layout: post
title: "HunterPie Account - Saves Backup"
date: 2022-10-10 02:33 -0300
badge: docs
categories: [Documentation, Account, Feature]
---

> This feature **can** be disabled in your account settings.
{:.prompt-note}

One of the features of having a [HunterPie Account](/posts/account) is that HunterPie will **automatically** backup your save and upload it to the cloud, this way you can play Monster Hunter games without worrying about your save getting corrupted.

The backups are available under your account details screen.

## How it works?

HunterPie will automatically handle backups of your game saves, the limit on how often and how many backups you can have at once depends on your type of account.

Account   | Number of Backups (total) | Rate limit
----------|---------------------------|-------------
Standard  | 2 backups (total)         | Once every 72 hours
Supporter | 5 backups (total)         | Once every 24 hours 

![backup-demo](/Static/backup-demo.png) *Backup list*

- You can download the backup file by clicking on the <ion-icon name="cloud-download" style="fill:#929495;"/> button, HunterPie will download the file into the `HunterPie/Backups` folder as a `.zip` file.
- To open the Backups folder quickly, you can also press the <ion-icon name="folder-open" style="fill:#929495;"/> button.
- In case you want to manually delete a backup file, just click on the <ion-icon name="trash" style="fill:#929495;"/> button.

> **Warning:** Keep in mind that deleting a backup file manually **WILL NOT** reset your rate limit.
{:.prompt-warning}
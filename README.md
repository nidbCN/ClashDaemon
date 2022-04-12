# ClashDaemon
clash windows service running background

## Installation

Download clash binary file

Build ClashDaemon

Save clash and ClashDaemon binary files to a sult path

Launch PowerShell/cmd in AdminMode, and type:

```
sc create Clash binPath=<ClashDaemon path> type=own start=auto displayname=Clash
```

## Configuration

Edit appSetting.json

```json
  "ExecuteOption": {
    "ClashPath": "",
    "ClashArguments": ""
  }
```

`ClashPath` point to your clash.exe, and fill `ClashArguments` with cli arguments

**ATTATION: WE DON NOT NO WHERE CLASH LOAD ITS CONFIG FILE WHEN USING WINDOWS SERVICE, PLEASE ADD `-d <path>` OPTION**

## Start

Launch windows services manager, and start "clash" service

## TODOs

1. Change clash warning log to level info, warning will create lots of log on windows events manager
2. Add more options to appSettings.json, and generate cli options automatic
3. Build clash source to C share libary, and use .NET p/invoke instead of clash executeable file

## RoadPath

~咕咕咕~

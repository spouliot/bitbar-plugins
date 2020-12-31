# Flite Plugin

[Bitbar](https://github.com/matryer/bitbar) plugin for [Flite](https://www.flitesense.com/) compatible controllers.

## Requirements

* macOS 10.13+
* [Bitbar](https://github.com/matryer/bitbar)
* [.NET 5.0](https://dotnet.microsoft.com/download/dotnet/5.0)

## Build

```bash
$ dotnet publish
Microsoft (R) Build Engine version 16.8.0+126527ff1 for .NET
Copyright (C) Microsoft Corporation. All rights reserved.

  Determining projects to restore...
  Restored /Users/poupou/git/spouliot/bitbar-plugins/flite/flite.csproj (in 194 ms).
  flite -> /Users/poupou/git/spouliot/bitbar-plugins/flite/bin/Debug/net5.0/flite.dll
  flite -> /Users/poupou/git/spouliot/bitbar-plugins/flite/bin/Debug/net5.0/publish/
```

## Setup

Example script `~/bitbar/flite.10s.sh`

```bash
#!/bin/bash
/usr/local/share/dotnet/dotnet ~/git/spouliot/bitbar-plugins/flite/bin/Debug/net5.0/publish/flite.dll ~/git/spouliot/bitbar-plugins/flite/flite.ini
```

The `.10s.` part of script name tells bitbar to execute it every 10 seconds.

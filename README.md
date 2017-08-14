# Unity-Console

[![Codacy Badge](https://api.codacy.com/project/badge/Grade/f8a05421a37846c2a7a2156284a773ad)](https://www.codacy.com/app/javierbullrich/Unity-Console?utm_source=github.com&utm_medium=referral&utm_content=Bullrich/Unity-Console&utm_campaign=badger)

An in-game console for Unity games

## How to add a method

Be sure to have the Prefab "Console Prefab" on the Scene.

To add a method simply type: 
```csharp
Blue.GameConsole.AddAction(method, buttonDescription);
```

Currently, the console accept four types of delegates:
 - Void
 - Bool
 - Int
 - Float

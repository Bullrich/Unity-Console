# Unity-Console
An in-game console for Unity games

[![Codacy Badge](https://api.codacy.com/project/badge/Grade/f8a05421a37846c2a7a2156284a773ad)](https://www.codacy.com/app/javierbullrich/Unity-Console?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=Bullrich/Unity-Console&amp;utm_campaign=Badge_Grade)

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
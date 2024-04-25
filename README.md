# FelipePlayFab
The best PlayFab Unity utility

# How to Use
1. Import the FelipePlayFab Package:
2. In any script where you want to use the FelipePlayFab functions, add
```
using FelipePlayFab;
```
at the top.

# Getting a Player ID:
To retrieve the player's PlayFab ID, you can call FelipePlayFab.PlayerID(callback).
For Example:
```
FelipePlayFab.PlayerID(playerId =>
{
    Debug.Log("Player ID: " + playerId);
});
```

# Getting a Player Account Age:
To calculate the player's account age, you can call FelipePlayFab.AccountAge(callback).
For Example:
```
FelipePlayFab.AccountAge(accountAge =>
{
    Debug.Log("Account Age: " + accountAge.Days + " days");
});
```




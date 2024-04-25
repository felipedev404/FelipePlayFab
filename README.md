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
To get the player's PlayFab ID, you can call FelipePlayFab.PlayerID(callback).
For Example:
```
FelipePlayFab.PlayerID(playerId =>
{
    Debug.Log("Player ID: " + playerId);
});
```

# Getting a Player Account Age:
To get the player's account age, you can call FelipePlayFab.AccountAge(callback).
For Example:
```
FelipePlayFab.AccountAge(accountAge =>
{
    Debug.Log("Account Age: " + accountAge.Days + " days");
});
```
# Getting a Player Login Count:
To get the number of logins for the player, you can call FelipePlayFab.LoginCount(callback).
For Example:
```
FelipePlayFab.LoginCount(loginCount =>
{
    Debug.Log("Login Count: " + loginCount);
});

```
# Purchasing Items:
To purchase items, you can call FelipePlayFab.PurchaseItem(itemId, onSuccess, onError).
For Example:
```
string itemId = "YourItemIdHere";
FelipePlayFab.PurchaseItem(itemId, 
    onSuccess: result =>
    {
        Debug.Log("Item purchased successfully!");
    },
    onError: error =>
    {
        Debug.LogError("Failed to purchase item: " + error.ErrorMessage);
    });

```

# PlayFabLogin



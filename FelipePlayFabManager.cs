using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

namespace FelipePlayFab
{
    public static class FelipePlayFabManager
    {
        public static string Currency = "BB";

        public static void PlayerID(Action<string> callback)
        {
            var request = new GetAccountInfoRequest();
            PlayFabClientAPI.GetAccountInfo(request, result =>
            {
                callback?.Invoke(result.AccountInfo.PlayFabId);
            }, error => { callback?.Invoke(null); });
        }

        public static void FelipePlayFabAccountAge(Action<TimeSpan> callback)
        {
            var request = new GetAccountInfoRequest();
            PlayFabClientAPI.GetAccountInfo(request, result =>
            {
                DateTime created = result.AccountInfo.Created;
                TimeSpan age = DateTime.UtcNow - created;
                callback?.Invoke(age);
            }, error => { callback?.Invoke(TimeSpan.Zero); });
        }

        public static void FelipePlayFabLoginCount(Action<int> callback)
        {
            var request = new GetPlayerStatisticsRequest();
            PlayFabClientAPI.GetPlayerStatistics(request, result =>
            {
                int loginCount = 0;
                foreach (var stat in result.Statistics)
                {
                    if (stat.StatisticName == "LoginCount")
                    {
                        loginCount = stat.Value;
                        break;
                    }
                }
                callback?.Invoke(loginCount);
            }, error => { callback?.Invoke(0); });
        }

        public static void FelipePlayFabPurchaseItem(string itemId, Action<PurchaseItemResult> onSuccess, Action<PlayFabError> onError)
        {
            var request = new PurchaseItemRequest
            {
                ItemId = itemId,
                VirtualCurrency = Currency,
                Price = 1
            };
            PlayFabClientAPI.PurchaseItem(request, onSuccess, onError);
        }
    }
}

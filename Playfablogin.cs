using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;
using Photon.VR;
using FelipePlayFab;

public class Playfablogin : MonoBehaviour
{
    [Header("PlayFab Settings")]
    public static Playfablogin instance;
    public string MyPlayFabID;
    public string CatalogName;
    public PlayFabSharedSettings Settings;

    [Header("Felipe Play Fab")]
    public TextMeshPro LoginCount;
    public TextMeshPro PlayerID;
    public TextMeshPro AccountAge;

    [Header("Special Items")]
    public List<GameObject> specialitems;
    public List<GameObject> disableitems;
    [Header("Currency")]
    public string CurrencyName;
    public TextMeshPro currencyText;
    public int coins;

    [Header("Banned Config")]
    public GameObject[] StuffToDisable;
    public GameObject[] StuffToEnable;
    public MeshRenderer[] StuffToMaterialChange;
    public Material MaterialToChangeToo;
    public TextMeshPro[] BanTimes;
    public TextMeshPro[] BanReasons;

    [Header("MOTD")]
    public TextMeshPro MOTDText;
    [Header("Username")]
    public TextMeshPro UserName;
    public string StartingUsername;
    public string Name;
    public bool UpdateName;

    [Header("Dont Destroy On Load")]
    public GameObject[] DDOLObjects;

    public void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Login();
        FetchFelipePlayFabInformation();
    }

    public void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnError);
    }

    public void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("logging in");
        GetAccountInfoRequest infoRequest = new GetAccountInfoRequest();
        PlayFabClientAPI.GetAccountInfo(infoRequest, AccountInfoSuccess, OnError);
        StartCoroutine(DDOLStuff());
        GetVirtualCurrencies();
        GetMOTD();
    }

    public void AccountInfoSuccess(GetAccountInfoResult result)
    {
        MyPlayFabID = result.AccountInfo.PlayFabId;

        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
        (result) =>
        {
            foreach (var item in result.Inventory)
            {
                if (item.CatalogVersion == CatalogName)
                {
                    foreach (var specialItem in specialitems)
                    {
                        if (specialItem.name == item.ItemId)
                        {
                            specialItem.SetActive(true);
                        }
                    }
                    foreach (var disableItem in disableitems)
                    {
                        if (disableItem.name == item.ItemId)
                        {
                            disableItem.SetActive(false);
                        }
                    }
                }
            }
        },
        (error) =>
        {
            Debug.LogError(error.GenerateErrorReport());
        });
    }

    async void Update()
    {
    }

    public void GetVirtualCurrencies()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), OnGetUserInventorySuccess, OnError);
    }

    void OnGetUserInventorySuccess(GetUserInventoryResult result)
    {
        coins = result.VirtualCurrency["BB"];
        currencyText.text = "You Have : " + coins.ToString() + CurrencyName;
    }

    private void OnError(PlayFabError error)
    {
        if (error.Error == PlayFabErrorCode.AccountBanned)
        {
            PhotonVRManager.Manager.Disconnect();
            foreach (GameObject obj in StuffToDisable)
            {
                obj.SetActive(false);
            }
            foreach (GameObject obj in StuffToEnable)
            {
                obj.SetActive(true);
            }
            foreach (MeshRenderer rend in StuffToMaterialChange)
            {
                rend.material = MaterialToChangeToo;
            }
            foreach (var item in error.ErrorDetails)
            {
                foreach (TextMeshPro BanTime in BanTimes)
                {
                    if (item.Value[0] == "Indefinite")
                    {
                        BanTime.text = "Permanent Ban";
                    }
                    else
                    {
                        string playFabTime = item.Value[0];
                        DateTime unityTime;
                        try
                        {
                            unityTime = DateTime.ParseExact(playFabTime, "yyyy-MM-dd'T'HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                            TimeSpan timeLeft = unityTime.Subtract(DateTime.UtcNow);
                            int hoursLeft = (int)timeLeft.TotalHours;
                            BanTime.text = string.Format("Hours Left: {0}", hoursLeft);
                        }
                        catch (FormatException ex)
                        {
                            Debug.LogErrorFormat("Failed to parse PlayFab time '{0}': {1}", playFabTime, ex.Message);
                        }
                    }
                }
                foreach (TextMeshPro BanReason in BanReasons)
                {
                    BanReason.text = string.Format("Reason: {0}", item.Key);
                }
            }
        }
        else
        {
            Login();
        }
    }

    public void GetMOTD()
    {
        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(), MOTDGot, OnError);
    }

    public void MOTDGot(GetTitleDataResult result)
    {
        if (result.Data == null || !result.Data.ContainsKey("MOTD"))
        {
            Debug.Log("No MOTD");
            return;
        }
        MOTDText.text = result.Data["MOTD"];
    }

    IEnumerator DDOLStuff()
    {
        Scene scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        yield return new WaitForSeconds(0.1f);
        foreach (GameObject Obj in DDOLObjects)
        {
            UnityEngine.Object.DontDestroyOnLoad(Obj);
        }
    }

    void FetchFelipePlayFabInformation()
    {
        FelipePlayFabManager.FelipePlayFabLoginCount(count =>
        {
            LoginCount.text = "Login Count: " + count.ToString();
        });

        FelipePlayFabManager.PlayerID(id =>
        {
            PlayerID.text = "Player ID: " + id;
        });

        FelipePlayFabManager.FelipePlayFabAccountAge(age =>
        {
            AccountAge.text = "Account Age: " + age.Days + " days";
        });
    }
}
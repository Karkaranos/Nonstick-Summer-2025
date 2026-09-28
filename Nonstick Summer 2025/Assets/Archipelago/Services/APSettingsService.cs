using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class APSettingsService : Service
{
    #region Singleton
    public static APSettingsService Instance;
    protected override void InitializeSingleton()
    {
        if (Instance != null)
            Destroy(this.gameObject);
        else
            Instance = this;
    }
    #endregion

    private Dictionary<string, object> settingsProfile;

    protected async override Task ThisInitialize()
    {
        ArchipelagoManager.Instance.OnArchipelagoConnected.AddListener(OnArchipelagoConnected);

        await Task.CompletedTask;
    }

    public void OnArchipelagoConnected()
    {
        //TODO: implement settings
        //ArchipelagoManager.Instance.session.

        //testing delete pls
        settingsProfile = ArchipelagoManager.Instance.session.DataStorage.GetSlotData();

        Debug.Log("<color=green>Settings loaded:</color>");
        Debug.Log(StaticUtilities.GetDictionaryDisplay(settingsProfile));
    }

}

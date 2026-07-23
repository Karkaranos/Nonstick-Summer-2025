using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class APRelationshipService : Service
{
    #region Singleton
    public static APRelationshipService Instance;
    protected override void InitializeSingleton()
    {
        if (Instance != null)
            Destroy(this.gameObject);
        else
            Instance = this;
    }
    #endregion

    private RelationshipStatus currentRelationshipStatus;

    protected async override Task ThisInitialize()
    {
        ArchipelagoManager.Instance.OnArchipelagoConnected.AddListener(OnArchipelagoConnected);

        await Task.CompletedTask;
    }

    public void OnArchipelagoConnected()
    {
        //TODO: implement settings
        //ArchipelagoManager.Instance.session.
    }

    public void LoadRelationshipStatusThisScene()
    {
        // this is so scuffed lol
        int moment_index;
        try
        {
            string moment_name = SceneManager.GetActiveScene().name;
            char last = moment_name[moment_name.Length - 1];
            moment_index = last - '0';
        }
        catch
        {
            return;
        }

        if (moment_index < 0 || moment_index > 5)
            return;

        Debug.Log($"Relationship stats, moment: {moment_index}");
        var relationshipStats = APSaveDataService.Instance.GetRelationshipStatsForMomentBeginning(moment_index);
        RelationshipManager.SetCharacterRelationships(relationshipStats);

        Debug.Log($"<color=magenta>Loaded Relationships from moment {relationshipStats.moment}</color>");
        Debug.Log($"<color=magenta>Mom</color>: {relationshipStats.MomRelationship.currentValue}");
        Debug.Log($"<color=magenta>Grandma</color>: {relationshipStats.GrandmaRelationship.currentValue}");
        Debug.Log($"<color=magenta>Cousin</color>: {relationshipStats.CousinRelationship.currentValue}");
        Debug.Log($"<color=magenta>Uncle</color>: {relationshipStats.UncleRelationship.currentValue}");

        ArchipelagoManager.Instance.OnRelationshipsUpdated.Invoke(); 
    }

    public void SaveRelationshipStatusThisScene()
    {
        // this is so scuffed lol
        int moment_index;
        try
        {
            string moment_name = SceneManager.GetActiveScene().name;
            char last = moment_name[moment_name.Length - 1];
            moment_index = last - '0';
        }
        catch
        {
            return;
        }

        RelationshipStatus relationshipStatus = new RelationshipStatus(moment_index);
        relationshipStatus.set = true;
        relationshipStatus.MomRelationship = RelationshipManager.characterRelationships[Character.Mom];
        relationshipStatus.GrandmaRelationship = RelationshipManager.characterRelationships[Character.Grandma];
        relationshipStatus.CousinRelationship = RelationshipManager.characterRelationships[Character.Cousin];
        relationshipStatus.UncleRelationship = RelationshipManager.characterRelationships[Character.Uncle];

        APSaveDataService.Instance.SetRelationshipStatus(relationshipStatus, moment_index);   
    }
}

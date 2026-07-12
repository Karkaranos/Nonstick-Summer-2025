using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Linq;


public class APDeckService : Service
{
    #region Singleton
    public static APDeckService Instance;
    protected override void InitializeSingleton()
    {
        if (Instance != null)
            Destroy(this.gameObject);
        else
            Instance = this;
    }
    #endregion

    [Header("Templates")]
    public ArchipelagoItem[] CardItems;
    public ArchipelagoItem[] ModifierItems => modifierTemplates.Select(m=> m.apItem).ToArray();

    public CardData[] cardTemplates;
    public ModifierData[] modifierTemplates;

    //public ModifierStamp[] stampTemplates;

    protected async override Task ThisInitialize()
    {
        //ArchipelagoManager.Instance.OnArchipelagoConnected.AddListener(OnArchipelagoConnected);

        await Task.CompletedTask;
    }

    public void RefreshDeckInventory()
    {
        DeckManager.ClearDeck();

        foreach(var cardType in CardItems)
        {
            int count = APInventoryService.Instance.GetItemCount(cardType);
            for(int i=0; i<count; i++)
            {
                DeckManager.AddCard(GetCardTemplate(cardType).CopyCard());
            }
        }

        ModifierManager.ClearDeck();
        foreach(var modifierType in ModifierItems)
        {
            int count = APInventoryService.Instance.GetItemCount(modifierType);
            for (int i = 0; i < count; i++)
            {
                Debug.Log($"Adding {modifierType}");
                ModifierManager.AddCard(GetModifierTemplate(modifierType));
            }
        }
    }

    private CardData GetCardTemplate(ArchipelagoItem item)
    {
        if (item == ArchipelagoItem.WittyQuestionCard)
            return cardTemplates.First(c => c.Emotion == CardEmotion.Charming && c.Intention == CardIntention.Question);

        if (item == ArchipelagoItem.WittyStatementCard)
            return cardTemplates.First(c => c.Emotion == CardEmotion.Charming && c.Intention == CardIntention.Expression);

        if (item == ArchipelagoItem.SappyQuestionCard)
            return cardTemplates.First(c => c.Emotion == CardEmotion.Sappy && c.Intention == CardIntention.Question);

        if (item == ArchipelagoItem.SappyStatementCard)
            return cardTemplates.First(c => c.Emotion == CardEmotion.Sappy && c.Intention == CardIntention.Expression);

        if (item == ArchipelagoItem.AssertiveQuestionCard)
            return cardTemplates.First(c => c.Emotion == CardEmotion.Assertive && c.Intention == CardIntention.Question);

        if (item == ArchipelagoItem.AssertiveStatementCard)
            return cardTemplates.First(c => c.Emotion == CardEmotion.Assertive && c.Intention == CardIntention.Expression);

        return null;
    }

    private ModifierData GetModifierTemplate(ArchipelagoItem item)
    {
        return modifierTemplates.First(m => m.apItem == item);
    }
}

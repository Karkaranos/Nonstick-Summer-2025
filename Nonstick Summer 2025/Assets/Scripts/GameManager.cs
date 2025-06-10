using NaughtyAttributes;
using UnityEngine;

/*****************************************************************************
* File Name :         GameManager.cs
* Author :            Toby, Cade, Sky
* Creation Date :     ???
*
* Brief Description : The GameManager is to be treated more like a system instantiator. 
* It will initialize all of its subsystems in Start, and it should ideally not have any 
* more logic to it than that. See description for more.
*
* TODO:
* 
* 
*****************************************************************************/
public class GameManager : Singleton<GameManager>
{
    public static Transform playerTransformRef;
    public static Camera playerCameraRef;

    public static UITransitionManager UITransitionManagerReference;
    public static CardStyleManager CardStyleManagerReference;
    public static DeckManager DeckManagerReference;
    public static DialogueManager DialogueManagerReference;
    public static RelationshipManager RelationshipManagerReference;

    public static int MaxCardsVisibleInDeck = 7;

    [Foldout("Card Styles")] [SerializeField] private CardValueStyle 
        Card_YellowStyle, Card_RedStyle, Card_BlueStyle,
        Card_Intention1Style, Card_Intention2Style, Card_Intention3Style; 
    //[Foldout("Card Styles")] [SerializeField] private CardValueStyle Card_RedColor;
    //[Foldout("Card Styles")] [SerializeField] private CardValueStyle Card_BlueColor;

    [Header("Intention Sprites")]
    [Foldout("Card Styles")] [SerializeField] private Sprite Card_Intention1Sprite;
    [Foldout("Card Styles")] [SerializeField] private Sprite Card_Intention2Sprite;
    [Foldout("Card Styles")] [SerializeField] private Sprite Card_Intention3Sprite;

    [Header("Dialogue Manager")]
    [Foldout("Social Battery")] [SerializeField] private int _defaultEnergy=5;
    [Foldout("Social Battery")] [SerializeField] private int _energyGainedPerRound=1;
    [Foldout("Social Battery")][SerializeField] private int _energyGainedIfSilent = 2;
    [Foldout("Social Battery")] [SerializeField] private int _maxEnergy=10;
    [Foldout("Dialogue Manager")] [SerializeField] private int _defaultCardsInHand=3;

    [Header("Relationship Manager")]
    [Foldout("Relationship Manager")] [SerializeField] private RelationshipStats grandmaStartingValue;
    [Foldout("Relationship Manager")] [SerializeField] private RelationshipStats uncleStartingValue;
    [Foldout("Relationship Manager")] [SerializeField] private RelationshipStats cousinStartingValue;
    [Foldout("Relationship Manager")] [SerializeField] private RelationshipStats momStartingValue;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        playerTransformRef = FindFirstObjectByType<PlayerMovement>()?.transform;
        playerCameraRef = FindFirstObjectByType<PlayerCamera>()?.playerCamera;

        UITransitionManagerReference = UITransitionManagerReference ?? new UITransitionManager();
        CardStyleManagerReference = CardStyleManagerReference ?? new CardStyleManager(Card_YellowStyle, Card_RedStyle, Card_BlueStyle,
            Card_Intention1Style, Card_Intention2Style, Card_Intention3Style,
            Card_Intention1Sprite, Card_Intention2Sprite, Card_Intention3Sprite);
        DeckManagerReference = DeckManagerReference ?? new DeckManager();
        DialogueManagerReference = DialogueManagerReference ?? new DialogueManager(_defaultEnergy, _energyGainedPerRound, _energyGainedIfSilent, _maxEnergy, _defaultCardsInHand);
        RelationshipManagerReference = RelationshipManagerReference ?? new RelationshipManager(grandmaStartingValue, uncleStartingValue, cousinStartingValue, momStartingValue);
    }
}

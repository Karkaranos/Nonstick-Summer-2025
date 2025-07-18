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
*****************************************************************************/
public class GameManager : Singleton<GameManager>
{
    public static Transform playerTransformRef;
    public static Camera playerCameraRef;

    // these variables mostly just exist to keep each sub-manager in memory
    public static UITransitionManager UITransitionManagerReference;
    public static CardStyleManager CardStyleManagerReference;
    public static DeckManager DeckManagerReference;
    public static DialogueManager DialogueManagerReference;
    public static RelationshipManager RelationshipManagerReference;
    public static ModifierManager ModifierManagerReference;
    public static Objectives ObjectiveReference;

    public static int MaxCardsVisibleInDeck = 100; // why is this here?

    [Foldout("Card Styles")] [SerializeField] private CardValueStyle 
        Card_CharmingStyle, Card_AssertiveStyle, Card_SappyStyle,
        Card_ExpressionStyle, Card_ObservationStyle, Card_QuestionStyle = new CardValueStyle(Color.white,"");
    //TODO: move these to different script probably
    public Color StampTooltipColor = new Color(1, 0.8f, 0.1f);
    public Color PositiveEnergyColor = Color.green;
    public Color NegativeEnergyColor = Color.red;
    public Color NeutralEnergyColor = Color.gray;

    [Tooltip("The initial cards in the players hand at the very beginning of the game")]
    [Foldout("Card Values"), SerializeField] private CardData[] startingCards;
    [Tooltip("The initial modifiers in the players hand at the very beginning of the game")]
    [Foldout("Card Values"), SerializeField] private ModifierData[] startingModifiers;

    [Header("Social Battery")]
    [Foldout("Combat"),SerializeField] private int _defaultEnergy=5;
    [Foldout("Combat"),SerializeField] private int _energyGainedPerRound=1;
    [Foldout("Combat"),SerializeField] private int _energyGainedIfSilent = 2;
    [Foldout("Combat"),SerializeField] private int _maxEnergy=10;
    [Foldout("Combat"),SerializeField] private int _drawButtonEnergyCost = 2;
    [Foldout("Combat"),SerializeField] private float _energyGainedPerDiscard = 1;
    [Header("Cards")]
    [Foldout("Combat"),SerializeField] private int _cardsDrawnPerRound=1;
    [Foldout("Combat"),SerializeField] public static int DefaultCardsInHand=5; // why is this hardcoded?

    [Header("Relationship Manager")]
    [Foldout("Relationship Manager")] [SerializeField] private RelationshipStats grandmaStartingValue;
    [Foldout("Relationship Manager")] [SerializeField] private RelationshipStats uncleStartingValue;
    [Foldout("Relationship Manager")] [SerializeField] private RelationshipStats cousinStartingValue;
    [Foldout("Relationship Manager")] [SerializeField] private RelationshipStats momStartingValue;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        UITransitionManagerReference = UITransitionManagerReference ?? new UITransitionManager();
        CardStyleManagerReference = CardStyleManagerReference ?? new CardStyleManager(Card_CharmingStyle, Card_AssertiveStyle, Card_SappyStyle,
            Card_ExpressionStyle, Card_ObservationStyle, Card_QuestionStyle);
        DeckManagerReference = DeckManagerReference ?? new DeckManager(startingCards);
        DialogueManagerReference = DialogueManagerReference ?? new DialogueManager(_defaultEnergy, _energyGainedPerRound, _energyGainedIfSilent, 
            _maxEnergy, DefaultCardsInHand, _cardsDrawnPerRound, _drawButtonEnergyCost, _energyGainedPerDiscard);
        RelationshipManagerReference = RelationshipManagerReference ?? new RelationshipManager(grandmaStartingValue, uncleStartingValue, cousinStartingValue, momStartingValue);
        ModifierManagerReference = ModifierManagerReference ?? new ModifierManager(startingModifiers);
        ObjectiveReference = FindFirstObjectByType<Objectives>();

    }

    /// <summary>
    /// Runs after the first scene has finished loading
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void AfterSceneLoad()
    {
        playerTransformRef = FindFirstObjectByType<PlayerMovement>()?.transform;
        playerCameraRef = FindFirstObjectByType<PlayerCamera>()?.playerCamera;

    }
}

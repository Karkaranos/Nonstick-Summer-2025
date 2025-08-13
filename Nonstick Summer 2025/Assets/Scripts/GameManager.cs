using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public static Transform PlayerTransformRef => playerTransformRef ?? Instance.RefreshPlayerTransform();
    private static Transform playerTransformRef;
    public static Camera PlayerCameraRef => playerCameraRef ?? Instance.RefreshPlayerCamera();
    private static Camera playerCameraRef;

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
        Card_StatementStyle, Card_QuestionStyle = new CardValueStyle(Color.white,"");
    //TODO: move these to different script probably
    [Foldout("Card Styles"), ShowAssetPreview(16,16), SerializeField]
    private Sprite BlankCard, YellowCardBack, RedCardBack, BlueCardBack;
    [Foldout("Card Styles")] public Color StampTooltipColor = new Color(1, 0.8f, 0.1f);
    [Foldout("Card Styles")] public Color PositiveEnergyColor = Color.green;
    [Foldout("Card Styles")] public Color NegativeEnergyColor = Color.red;
    [Foldout("Card Styles")] public Color NeutralEnergyColor = Color.gray;

    [Tooltip("The initial cards in the players hand at the very beginning of the game")]
    [Foldout("Card Values"), SerializeField] private CardData[] startingCards;
    [Tooltip("The initial modifiers in the players hand at the very beginning of the game")]
    [Foldout("Card Values"), SerializeField] private ModifierData[] startingModifiers;

    [Header("Social Battery")]
    [Foldout("Combat"),SerializeField] private int _defaultEnergy=5;
    [Foldout("Combat"),SerializeField] private int _energyGainedPerRound=1;
    [Foldout("Combat"),SerializeField] private int _energyGainedIfSilent = 3;
    [Foldout("Combat"),SerializeField] private int _maxEnergy=10;
    [Foldout("Combat"),SerializeField] private int _drawButtonEnergyCost = 2;
    [Foldout("Combat"),SerializeField] private float _energyGainedPerDiscard = 1;
    [Header("Cards")]
    [Foldout("Combat"),SerializeField] private int _cardsDrawnPerRound=1;
    [Foldout("Combat"),SerializeField] public static int DefaultCardsInHand=4; // why is this hardcoded?

    [Header("Relationship Manager")]
    [Foldout("Relationship Manager")] [SerializeField] private RelationshipStats grandmaStartingValue;
    [Foldout("Relationship Manager")] [SerializeField] private RelationshipStats uncleStartingValue;
    [Foldout("Relationship Manager")] [SerializeField] private RelationshipStats cousinStartingValue;
    [Foldout("Relationship Manager")] [SerializeField] private RelationshipStats momStartingValue;

    public static float Sensitivity = .4f;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(transform.parent == null ? this.gameObject : transform.parent.gameObject);


        UITransitionManagerReference = UITransitionManagerReference ?? new UITransitionManager();
        CardStyleManagerReference = CardStyleManagerReference ?? new CardStyleManager(Card_CharmingStyle, Card_AssertiveStyle, Card_SappyStyle,
            Card_StatementStyle, Card_QuestionStyle, BlankCard, YellowCardBack, RedCardBack, BlueCardBack);
        DeckManagerReference = DeckManagerReference ?? new DeckManager(startingCards);
        DialogueManagerReference = DialogueManagerReference ?? new DialogueManager(_defaultEnergy, _energyGainedPerRound, _energyGainedIfSilent, 
            _maxEnergy, DefaultCardsInHand, _cardsDrawnPerRound, _drawButtonEnergyCost, _energyGainedPerDiscard);
        RelationshipManagerReference = RelationshipManagerReference ?? new RelationshipManager(grandmaStartingValue, uncleStartingValue, cousinStartingValue, momStartingValue);
        ModifierManagerReference = ModifierManagerReference ?? new ModifierManager(startingModifiers);
        ObjectiveReference = FindFirstObjectByType<Objectives>();

    }

    void OnEnable()
    {

        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    void OnDisable()
    {

        SceneManager.sceneLoaded -= OnSceneLoaded;

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        DialogueManager.CurrentEnergy = _defaultEnergy;

        if (scene.name.Contains("1"))
        {
            DeckManagerReference = null;
            RelationshipManagerReference = null;
            ModifierManagerReference = null;
            DeckManagerReference = new DeckManager(startingCards);

            grandmaStartingValue.currentValue = 25;
            uncleStartingValue.currentValue = 25;
            cousinStartingValue.currentValue = 25;
            momStartingValue.currentValue = 25;
            RelationshipManagerReference = new RelationshipManager(grandmaStartingValue, uncleStartingValue, cousinStartingValue, momStartingValue);

            ModifierManagerReference = new ModifierManager(startingModifiers);
        }

        ObjectiveReference = FindFirstObjectByType<Objectives>();

    }


    private Camera RefreshPlayerCamera()
    {
        if(playerCameraRef != null)
            return playerCameraRef;

        playerCameraRef = FindFirstObjectByType<PlayerCamera>().playerCamera;
        return playerCameraRef;
    }

    private Transform RefreshPlayerTransform()
    {
        if (playerTransformRef != null)
            return playerTransformRef;

        playerTransformRef = FindFirstObjectByType<PlayerMovement>().transform;
        return playerTransformRef;
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

using NaughtyAttributes;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public static Transform playerTransformRef;
    public static Camera playerCameraRef;

    public static UITransitionManager UITransitionManagerReference;
    public static CardStyleManager CardStyleManagerReference;

    [Header("Emotion Colors")]
    [Foldout("Card Styles")] [SerializeField] private Color Card_YellowColor = Color.yellow;
    [Foldout("Card Styles")] [SerializeField] private Color Card_RedColor = Color.red;
    [Foldout("Card Styles")] [SerializeField] private Color Card_BlueColor = Color.blue;

    [Header("Intention Sprites")]
    [Foldout("Card Styles")] [SerializeField] private Sprite Card_Intention1Sprite;
    [Foldout("Card Styles")] [SerializeField] private Sprite Card_Intention2Sprite;
    [Foldout("Card Styles")] [SerializeField] private Sprite Card_Intention3Sprite;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        playerTransformRef = FindFirstObjectByType<PlayerMovement>()?.transform;
        playerCameraRef = FindFirstObjectByType<PlayerCamera>()?.playerCamera;

        UITransitionManagerReference = new UITransitionManager();
        CardStyleManagerReference = new CardStyleManager(Card_YellowColor, Card_RedColor, Card_BlueColor, 
            Card_Intention1Sprite, Card_Intention2Sprite, Card_Intention3Sprite);
    }
}

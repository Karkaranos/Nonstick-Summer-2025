/*************************************************
                        DreamSequence
Author Names :          Sky, Toby
Date Created :          ?, 2025
Date Modified :         July 19, 2025
Brief Description :     
***************************************************/

using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;

public class DreamSequence : MonoBehaviour
{
    [SerializeField] [Required] private TMP_Text statementField;
    [SerializeField] [Required] private TMP_Text questionField;

    [SerializeField, Required] private Button confirmButton;

    [SerializeField] private string statement = "You are lost in thought.";
    [SerializeField] private string question = "How do you feel?";

    [Header("Options")]
    [SerializeField] private PersonalityOption yellowOption;
    [SerializeField] private PersonalityOption redOption;
    [SerializeField] private PersonalityOption blueOption;

    [SerializeField] private GameObject confirmation;

    private CardEmotion selectedEmotion = CardEmotion.NotSelected;
    private Material currentMat;

    public void Initialize()
    {
        statementField.text = statement;
        questionField.text = question;

        TMP_Text textOption1 = yellowOption.button.GetComponentInChildren<TMP_Text>();
        textOption1.text = yellowOption.ButtonText;

        TMP_Text textOption2 = redOption.button.GetComponentInChildren<TMP_Text>();
        textOption2.text = redOption.ButtonText;

        TMP_Text textOption3 = blueOption.button.GetComponentInChildren<TMP_Text>();
        textOption3.text = blueOption.ButtonText;

        DreamMaterial = Background.material;

        NeutralColors = GetShaderProperties(NeutralMaterial);
        YellowEmotionColors = GetShaderProperties(YellowMaterial);
        BlueEmotionColors = GetShaderProperties(BlueMaterial);
        RedEmotionColors = GetShaderProperties(RedMaterial);
        selectedEmotionShaderData = NeutralColors;

        yellowOption.button.image.color = yellowOption.ButtonColor;
        redOption.button.image.color = redOption.ButtonColor;
        blueOption.button.image.color = blueOption.ButtonColor;

        yellowOption.button.onClick.AddListener(OnCharmingChosen);
        redOption.button.onClick.AddListener(OnAssertiveChosen);
        blueOption.button.onClick.AddListener(OnSappyChosen);

        confirmButton.gameObject.SetActive(false);
        confirmButton.onClick.AddListener(OnConfirmPressed);

        SetShaderProperties(NeutralColors);
    }

    public void OnCharmingChosen()
    {
        if(transitionShaderCoroutine != null)
        {
            StopCoroutine(transitionShaderCoroutine);
            selectedEmotionShaderData = GetShaderProperties(DreamMaterial); // get current state
        }

        transitionShaderCoroutine = StartCoroutine(TransitionShaderProperties(selectedEmotionShaderData, YellowEmotionColors));
        selectedEmotion = CardEmotion.Charming;
        confirmButton.gameObject.SetActive(true);
    }

    public void OnSappyChosen()
    {
        if (transitionShaderCoroutine != null)
        {
            StopCoroutine(transitionShaderCoroutine);
            selectedEmotionShaderData = GetShaderProperties(DreamMaterial); // get current state
        }

        transitionShaderCoroutine = StartCoroutine(TransitionShaderProperties(selectedEmotionShaderData, BlueEmotionColors));
        selectedEmotion = CardEmotion.Sappy;
        confirmButton.gameObject.SetActive(true);
    }

    public void OnAssertiveChosen()
    {
        if (transitionShaderCoroutine != null)
        {
            StopCoroutine(transitionShaderCoroutine);
            selectedEmotionShaderData = GetShaderProperties(DreamMaterial); // get current state
        }

        transitionShaderCoroutine = StartCoroutine(TransitionShaderProperties(selectedEmotionShaderData, RedEmotionColors));
        selectedEmotion = CardEmotion.Assertive;
         confirmButton.gameObject.SetActive(true);
    }

    private void OnConfirmPressed()
    {
        MoodManager.SetDreamSequenceCost(selectedEmotion);
        UITransitionManager.CloseMenu(false, false);

        var newMenu = UITransitionManager.OpenMenu(confirmation).GetComponent<SocialBatteryNotifCanvas>();
        newMenu.Initialize(selectedEmotion, Background.material);


    }    

    /*************************************************
    Author Names :          Cade Naylor
    Date Created :          June 19, 2025
    Date Modified :         June 19, 2025
    Brief Description :     Stores information for interactable object questions
    ***************************************************/
    [System.Serializable]
    private class PersonalityOption
    {
        [Tooltip("Option text"), ResizableTextArea] public string ButtonText;
        [Tooltip("An optional tint for the button. Leave white if not")] public Color ButtonColor = Color.white;
        public Button button;
    }

    #region Shader Town

    [Header("Background Shader")]
    [SerializeField, Required] private Image Background;
    [SerializeField, Required] private Material NeutralMaterial, YellowMaterial, RedMaterial, BlueMaterial;
    [SerializeField] private float transitionColorSeconds = 0.3f;
    [SerializeField] private float transitionBlobSpeedSeconds = 10f;

    private Material DreamMaterial;
    private DreamShaderDataCollection NeutralColors, YellowEmotionColors, RedEmotionColors, BlueEmotionColors;
    private DreamShaderDataCollection selectedEmotionShaderData;

    private Coroutine transitionShaderCoroutine;

    private IEnumerator TransitionShaderProperties(DreamShaderDataCollection from, DreamShaderDataCollection to)
    {
        float timeStarted = Time.time;
        float timeElapsed;
        do
        {
            timeElapsed = Time.time - timeStarted;
            float t = timeElapsed / transitionColorSeconds;

            // nightmare code nightmare code nightmare code

            if(timeElapsed < transitionColorSeconds)
            {
                // all these if statements are here bc i cant imagine setting the materials is super efficient

                if (from.Layer1.color != to.Layer1.color) DreamMaterial.SetColor("_Color_1", Color.Lerp(from.Layer1.color, to.Layer1.color, t));
                if (from.Layer1.BlobSize != to.Layer1.BlobSize) DreamMaterial.SetFloat("_Blob_Size_1", Mathf.Lerp(from.Layer1.BlobSize, to.Layer1.BlobSize, t));
                if (from.Layer1.MinMaxOpacity != to.Layer1.MinMaxOpacity) DreamMaterial.SetVector("_Min_Max_Opacity_1", Vector2.Lerp(from.Layer1.MinMaxOpacity, to.Layer1.MinMaxOpacity, t));

                if (from.Layer2.color != to.Layer2.color) DreamMaterial.SetColor("_Color_2", Color.Lerp(from.Layer2.color, to.Layer2.color, t));
                if (from.Layer2.BlobSize != to.Layer2.BlobSize) DreamMaterial.SetFloat("_Blob_Size_2", Mathf.Lerp(from.Layer2.BlobSize, to.Layer2.BlobSize, t));
                if (from.Layer2.MinMaxOpacity != to.Layer2.MinMaxOpacity) DreamMaterial.SetVector("_Min_Max_Opacity_2", Vector2.Lerp(from.Layer2.MinMaxOpacity, to.Layer2.MinMaxOpacity, t));

                if (from.Layer3.color != to.Layer3.color) DreamMaterial.SetColor("_Color_3", Color.Lerp(from.Layer3.color, to.Layer3.color, t));
                if (from.Layer3.BlobSize != to.Layer3.BlobSize) DreamMaterial.SetFloat("_Blob_Size_3", Mathf.Lerp(from.Layer3.BlobSize, to.Layer3.BlobSize, t));
                if (from.Layer3.MinMaxOpacity != to.Layer3.MinMaxOpacity) DreamMaterial.SetVector("_Min_Max_Opacity_3", Vector2.Lerp(from.Layer3.MinMaxOpacity, to.Layer3.MinMaxOpacity, t));

                if (from.BackgroundColor != to.BackgroundColor) DreamMaterial.SetColor("_Background_Color", Color.Lerp(from.BackgroundColor, to.BackgroundColor, t));
            }
            if (from.Layer1.Speed != to.Layer1.Speed) DreamMaterial.SetFloat("_Speed_1", Mathf.Lerp(from.Layer1.Speed, to.Layer1.Speed, timeElapsed / transitionBlobSpeedSeconds));
            if (from.Layer2.Speed != to.Layer2.Speed) DreamMaterial.SetFloat("_Speed_2", Mathf.Lerp(from.Layer2.Speed, to.Layer2.Speed, timeElapsed / transitionBlobSpeedSeconds));
            if (from.Layer3.Speed != to.Layer3.Speed) DreamMaterial.SetFloat("_Speed_3", Mathf.Lerp(from.Layer3.Speed, to.Layer3.Speed, timeElapsed / transitionBlobSpeedSeconds));

            // properties that take longer to transition

            yield return null;
        }
        while (timeElapsed < transitionColorSeconds || timeElapsed < transitionBlobSpeedSeconds);

        selectedEmotionShaderData = to;
        transitionShaderCoroutine = null;
    }

    private void SetShaderProperties(DreamShaderDataCollection data)
    {
        // nightmare code nightmare code nightmare code

        DreamMaterial.SetColor("_Color_1", data.Layer1.color);
        DreamMaterial.SetFloat("_Speed_1", data.Layer1.Speed);
        DreamMaterial.SetFloat("_Blob_Size_1", data.Layer1.BlobSize);
        DreamMaterial.SetVector("_Min_Max_Opacity_1", data.Layer1.MinMaxOpacity);

        DreamMaterial.SetColor("_Color_2", data.Layer2.color);
        DreamMaterial.SetFloat("_Speed_2", data.Layer2.Speed);
        DreamMaterial.SetFloat("_Blob_Size_2", data.Layer2.BlobSize);
        DreamMaterial.SetVector("_Min_Max_Opacity_2", data.Layer2.MinMaxOpacity);

        DreamMaterial.SetColor("_Color_3", data.Layer3.color);
        DreamMaterial.SetFloat("_Speed_3", data.Layer3.Speed);
        DreamMaterial.SetFloat("_Blob_Size_3", data.Layer3.BlobSize);
        DreamMaterial.SetVector("_Min_Max_Opacity_3", data.Layer3.MinMaxOpacity);

        DreamMaterial.SetColor("_Background_Color", data.BackgroundColor);

        selectedEmotionShaderData = data;
    }

    private static DreamShaderDataCollection GetShaderProperties(Material material)
    {
        return new DreamShaderDataCollection()
        {
            Layer1 = new DreamShaderData()
            {
                Speed = material.GetFloat("_Speed_1"),
                color = material.GetColor("_Color_1"),
                MinMaxOpacity = material.GetVector("_Min_Max_Opacity_1"),
                BlobSize = material.GetFloat("_Blob_Size_1")
            },
            Layer2 = new DreamShaderData()
            {
                Speed = material.GetFloat("_Speed_2"),
                color = material.GetColor("_Color_2"),
                MinMaxOpacity = material.GetVector("_Min_Max_Opacity_2"),
                BlobSize = material.GetFloat("_Blob_Size_2")
            },
            Layer3 = new DreamShaderData()
            {
                Speed = material.GetFloat("_Speed_3"),
                color = material.GetColor("_Color_3"),
                MinMaxOpacity = material.GetVector("_Min_Max_Opacity_3"),
                BlobSize = material.GetFloat("_Blob_Size_3")
            },
            BackgroundColor = material.GetColor("_Background_Color")
        };
    }

    /*****************************************************************************
    * File Name :         DreamShaderDataCollection.cs
    * Author :            Toby
    * Creation Date :     July 19, 2025
    *
    * Brief Description : big data holder guy
    *****************************************************************************/

    [System.Serializable]
    private class DreamShaderDataCollection
    {
        public DreamShaderData Layer1 = new DreamShaderData();
        public DreamShaderData Layer2 = new DreamShaderData();
        public DreamShaderData Layer3 = new DreamShaderData();
        public Color BackgroundColor = Color.white;
    }

    /*****************************************************************************
    * File Name :         DreamShaderData.cs
    * Author :            Toby
    * Creation Date :     July 19, 2025
    *
    * Brief Description : little data holder guy
    *****************************************************************************/

    [System.Serializable]
    private class DreamShaderData
    {
        public Color color = Color.white;
        [MinMaxSlider(0, 1)]
        public Vector2 MinMaxOpacity = new Vector2(0.85f, 1f);
        public float Speed = 0.2f;
        public float BlobSize = 0.6f;
    }

    #endregion
}

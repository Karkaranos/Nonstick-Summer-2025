using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: Header("Music")]
    [field: SerializeField] public EventReference HouseBGM { get; private set; }
    [field: SerializeField] public EventReference TitleBGM { get; private set; }
    [field: SerializeField] public EventReference CombatBGM { get; private set; }
    [field: SerializeField] public EventReference ReflectionBGM { get; private set; }
    [field: Header("SFX")]
    [field: SerializeField] public EventReference WalkSFX { get; private set; }
    [field: SerializeField] public EventReference InteractSFX { get; private set; }
    [field: SerializeField] public EventReference CharacterReactGenericSFX { get; private set; }
    [field: SerializeField] public EventReference CardHoverSFX { get; private set; }
    [field: SerializeField] public EventReference CardSelectSFX { get; private set; }
    [field: SerializeField] public EventReference CardPlaySFX { get; private set; }
    [field: SerializeField] public EventReference PosRelationSFX { get; private set; }
    [field: SerializeField] public EventReference NegRelationSFX { get; private set; }
    [field: SerializeField] public EventReference UIClickSFX { get; private set; }
    [field: SerializeField] public EventReference UIHoverSFX { get; private set; }
    [field: SerializeField] public EventReference Stamp { get; private set; }
    [field: SerializeField] public EventReference TV { get; private set; }
    [field: SerializeField] public EventReference UncleReact { get; private set; }
    [field: SerializeField] public EventReference GrandmaReact { get; private set; }
    [field: SerializeField] public EventReference MomReact { get; private set; }
    [field: SerializeField] public EventReference CousinReact { get; private set; }
    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("There is more than one FMODEvents in the scene");
        }
        instance = this;
    }
}
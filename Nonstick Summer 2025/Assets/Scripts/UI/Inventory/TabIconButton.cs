using NaughtyAttributes;
using UnityEngine;

public class TabIconButton : Singleton<TabIconButton>   
{
    [Required] public RectTransform rectTransform;

    [Required, SerializeField] private RectTransform notification;


}

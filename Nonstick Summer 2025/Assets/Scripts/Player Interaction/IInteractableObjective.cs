/*************************************************
Author Names :          Cade
Date Created :          July 8, 2025
Date Modified :         July 8, 2025
Brief Description :     Creates a child interface to more readily support in-world 
                        objects the player interacts with that contain objectives
***************************************************/

public interface  IInteractableObjective : IInteractable
{
    /// <summary>
    /// Added to the interface to make this easier
    /// Function called during Objective initialization to initialize this as relating to objectives or not
    /// </summary>
    /// <param name="status"></param>
    void SetIsObjective(bool status);

    /// <summary>
    /// Added to the interface to make this easier
    /// If this object relates to objectives, it sets whether this can or cannot be interacted with
    /// </summary>
    void ClearBlocker();
}

using UnityEngine;

public class BedroomDoorBehavior : MonoBehaviour
{
    private bool _playerHasLeft = false;
    [HideInInspector] public bool BossDefeated = false;
    [SerializeField] private int _nextSceneIndex;

    /// <summary>
    /// Occurs when an object enters this trigger
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerMovement>())
        {
            if (!_playerHasLeft)
            {
                GameManager.ObjectiveReference.MetCondition(ObjectiveConditions.LEAVE_BEDROOM);
                _playerHasLeft = true;
            }
            if (BossDefeated)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(_nextSceneIndex);
            }
        }
    }
}

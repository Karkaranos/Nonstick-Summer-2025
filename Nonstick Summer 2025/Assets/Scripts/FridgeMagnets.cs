using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FridgeMagnets : MonoBehaviour
{
    [SerializeField] private Transform letterM;
    [SerializeField] private Transform letterO;
    [SerializeField] private Transform letterM2;

    [SerializeField] private Transform firstLetterPosition;
    [SerializeField] private Transform secondLetterPosition;
    [SerializeField] private Transform thirdLetterPosition;
    public void ChangeMagnetPosition(string letterOrder)
    {
        if (letterOrder == "MOM")
        {
            //it's already this way!
            Debug.Log("MOM");
        }

        else if (letterOrder == "OWW")
        {
            letterO.position = firstLetterPosition.position;

            letterM.position = secondLetterPosition.position;
            letterM.Rotate(0, 0, 180);

            letterM2.position = thirdLetterPosition.position;
            letterM2.Rotate(0, 0, 90);

            Debug.Log("OWW");
        }

        else if (letterOrder == "WOW")
        {
            letterM.position = firstLetterPosition.position;
            letterM.Rotate(0, 0, 90);

            letterO.position = secondLetterPosition.position;

            letterM2.position = thirdLetterPosition.position;
            letterM2.Rotate(0, 0, 90);

            Debug.Log("WOW");
        }
    }


}

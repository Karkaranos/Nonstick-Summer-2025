using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FridgeMagnets : MonoBehaviour
{
    [SerializeField] private GameObject letterM;
    [SerializeField] private GameObject letterO;
    [SerializeField] private GameObject letterM2;

    [SerializeField] private Transform firstLetterPosition;
    [SerializeField] private Transform secondLetterPosition;
    [SerializeField] private Transform thirdLetterPosition;

    private void Start()
    {
        InteractableObjectBehavior IOB = GetComponent<InteractableObjectBehavior>();
        InteractableObjectCanvas IOC = IOB.CanvasToOpen.GetComponent<InteractableObjectCanvas>();
        IOC.Button1.onClick.AddListener(() => ChangeMagnetPosition("MOM"));
        IOC.Button2.onClick.AddListener(() => ChangeMagnetPosition("OWW"));
        IOC.Button3.onClick.AddListener(() => ChangeMagnetPosition("WOW"));
    }

    private void ChangeMagnetPosition(string letterOrder)
    {
        Debug.Log("ugh");
        if (letterOrder == "MOM")
        {
            //it's already this way!
        }

        else if (letterOrder == "OWW")
        {
            letterO.transform.position = firstLetterPosition.position;

            letterM.transform.position = secondLetterPosition.position;
            letterM.transform.Rotate(0, 0, 180);

            letterM2.transform.position = thirdLetterPosition.position;
            letterM2.transform.Rotate(0, 0, 90);
        }

        else if (letterOrder == "WOW")
        {
            letterM.transform.position = firstLetterPosition.position;
            letterM.transform.Rotate(0, 0, 90);

            letterO.transform.position = secondLetterPosition.position;

            letterM2.transform.position = thirdLetterPosition.position;
            letterM2.transform.Rotate(0, 0, 90);
        }
    }


}

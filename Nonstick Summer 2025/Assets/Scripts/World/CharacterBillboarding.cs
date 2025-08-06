/*****************************************************************************
* File Name :         CharacterBillboarding.cs
* Author :            Jay
* Creation Date :     July 31 2025
*
* Brief Description :  ensures that the Character' sprites will always face the camera
* this probably didn't need to be a whole script lmaooooo
* 
*****************************************************************************/
using System.Collections;
using UnityEngine;

public class CharacterBillboarding : MonoBehaviour
{

    void Update()
    {

        transform.LookAt(Camera.main.transform.position, Vector3.up);

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Door : MonoBehaviour,IInteractable
{
    public GameObject character,tpPosition;
    public bool isIndoor;
    public string getName()
    {
        return "Kapi";
    }
    public void Interactable()
    {
        character.gameObject.transform.position = tpPosition.transform.position;
        if (isIndoor)
        {
        }
        else
        {
        }
    }
}

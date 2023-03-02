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
        character.gameObject.transform.position = tpPosition.transform.position + Vector3.up/2;
        GameObject.Find("TargettedCam").transform.position = transform.position + Vector3.up * 5/2;
        if (isIndoor)
        {
        }
        else
        {
        }
    }
}

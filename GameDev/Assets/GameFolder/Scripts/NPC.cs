using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour,IInteractable
{
    public delegate void NpcEvent(VIDE_Assign dialog);
    public static NpcEvent npcDialog;
    [SerializeField]
    string Npc_name;

    void IInteractable.Interact()
    {
        Debug.Log(Npc_name);
        npcDialog(GetComponent<VIDE_Assign>());
    }
    public string getName()
    {
        return Npc_name;
    }
}

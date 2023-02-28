using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using VIDE_Data;
using UnityEngine.UI;
public class DialogControl : MonoBehaviour
{
    GameObject playerUI, diyalogUI;
    TMP_Text npcMessage, npcName;
    public TMP_Text[] diyalogSecenek;
    private void OnEnable()
    {
        NPC.npcDialog += DialogStart;
    }
    private void OnDisable()
    {
        NPC.npcDialog -= DialogStart;

        if (diyalogUI != null)
            DialogStop(null);
    }
    private void Start()
    {
        diyalogUI = GameObject.FindGameObjectWithTag("Canvas").transform.Find("Dialog").gameObject;
        npcMessage = diyalogUI.transform.Find("NPCPanel").Find("Message").Find("NpcText").GetComponent<TMP_Text>();
        npcName = diyalogUI.transform.Find("NPCPanel").Find("Name").Find("NpcName").GetComponent<TMP_Text>();
        playerUI = diyalogUI.transform.Find("PlayerPanel").Find("Scrolling").Find("Viewport").Find("Content").gameObject;

        diyalogUI.SetActive(false);
    }
    private void Update()
    {
        if (VD.isActive)
        {
            if(Input.anyKeyDown && !VD.nodeData.isPlayer)
            {
                VD.Next();
            }
        }
    }
    void DialogStart(VIDE_Assign npcDialog)
    {
        VD.OnNodeChange += UIUpdate;
        VD.OnEnd += DialogStop;
        VD.BeginDialogue(npcDialog);
        npcName.text = npcDialog.alias;
        diyalogUI.SetActive(true);
        Time.timeScale = 0;
    }
    void UIUpdate(VD.NodeData data)
    {
        if (data.isPlayer)
        {
            for (int i = 0; i < diyalogSecenek.Length; i++)
            {
                if(i < data.comments.Length)
                {
                    diyalogSecenek[i].transform.parent.gameObject.SetActive(true);
                    diyalogSecenek[i].text = data.comments[i];
                }
                else
                {
                    diyalogSecenek[i].transform.parent.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            foreach (Transform button in playerUI.transform)
            {
                button.gameObject.GetComponent<Button>().gameObject.SetActive(false);
            }
            npcMessage.text = data.comments[data.commentIndex];
        }
    }
    void DialogStop(VD.NodeData data)
    {
        VD.OnNodeChange -= UIUpdate;
        VD.OnEnd -= DialogStop;
        VD.EndDialogue();
        diyalogUI.SetActive(false);
        Time.timeScale = 1;
    }
    public void PlayerSecenek(int Secenek)
    {
        VD.nodeData.commentIndex = Secenek;
        if (Input.GetMouseButtonUp(0))
        {
            VD.Next();
        }
    }
}

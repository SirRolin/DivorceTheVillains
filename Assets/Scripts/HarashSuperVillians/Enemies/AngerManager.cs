using System.Collections.Generic;
//using Unity.VisualScripting;
using System;
using UnityEngine;
using UnityEngine.UI;

public class AngerManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> angerNPCs = new();
    public event Action OnCheckForWin;
    public List<GameObject> GetAngerNPCs(){
        return angerNPCs;
    }
    public void AddAngerNPC(GameObject npc){
        angerNPCs.Add(npc);
        AddAngerNPCHelper(npc);
    }

    private void AddAngerNPCHelper(GameObject npc){
        if(!npc.TryGetComponent<Anger>(out Anger angst)){
            angst = npc.AddComponent<Anger>();
        }
        angst.OnUpdate += UpdateFill;
        angst.barFill = AddProgressBarToUI(npc);
        angst.OnCheckForWin += CheckForWin;
    }

    [SerializeField]
    private GameObject AngerBarBlueprint;

    private Image AddProgressBarToUI(GameObject npc){
        GameObject angerBar = Instantiate(AngerBarBlueprint, transform);
        Image barFill = angerBar.transform.GetChild(0).GetComponent<Image>();
        UpdateFill(barFill, 0, 0);
        return barFill;
    }

    private void UpdateFill(Image barFill, float current, float max){
        barFill.fillAmount = max == 0 ? 0 : current / max;
    }

    private void CheckForWin()
    {
        OnCheckForWin?.Invoke();
    }


    void Start(){
        foreach (GameObject npc in angerNPCs)
        {
            AddAngerNPCHelper(npc);
        }
    }

}

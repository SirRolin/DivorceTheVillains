using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AngerManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> angerNPCs = new();
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
    }

    [SerializeField]
    private GameObject AngerBarBlueprint;

    private Image AddProgressBarToUI(GameObject npc){
        GameObject angerBar = Instantiate(AngerBarBlueprint, transform);
        Image barFill = angerBar.transform.GetChild(0).GetComponent<Image>();
        Debug.Log(barFill);
        UpdateFill(barFill, 0, 0);
        return barFill;
    }

    private void UpdateFill(Image barFill, float current, float max){
        barFill.fillAmount = max == 0 ? 0 : current / max;
    }


    void Start(){
        foreach (GameObject npc in angerNPCs)
        {
            AddAngerNPCHelper(npc);
        }
    }

}

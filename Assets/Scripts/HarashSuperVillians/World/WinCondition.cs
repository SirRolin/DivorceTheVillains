using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCondition : MonoBehaviour
{
    public AngerManager AngerManager;
    public string WinScene;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AngerManager.OnCheckForWin += CheckIfWon;

    }


    private void CheckIfWon()
    {
        bool result = true;
        foreach (GameObject angerObj in AngerManager.GetAngerNPCs())
        {
            Anger angst = angerObj.GetComponent<Anger>();
            if (angst.GetAnger() <= 0)
            {
                result = false;
                break;
            }
        }
        //win condition act
        if (result)
        {
            SceneManager.LoadScene(WinScene);
        }

    }

}

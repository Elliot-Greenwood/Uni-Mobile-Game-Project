using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    
    [SerializeField] GameObject ReturnButtonUI;
    [SerializeField] GameObject GameModeUI_GM;
    [SerializeField] GameObject NormalSweeperSelectionUI_NMS;
    [SerializeField] GameObject BookLevelSelectionUI_TS;
    [SerializeField] GameObject Book1;
    [SerializeField] GameObject LevelSelectionUI;


    bool InMainSelection = true;
    bool InUILayer1 = false;
    bool InUILayer2 = false;



    void Start()
    {
        
    }
    void Update()
    {
        
    }





    public void ReturnButton()
    {
        
    }

    




    public void OpenUILayer1()
    {
        InUILayer1 = true;
    }
    public void OpenUILayer2()
    {
        InUILayer2 = true;
    }



    public void LoadLevel(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }



}

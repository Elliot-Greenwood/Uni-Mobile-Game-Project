using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    //[SerializeField] GameObject GameTitle;
    [SerializeField] GameObject ReturnButtonUI;
    [SerializeField] GameObject MainMenuSelection;
    [SerializeField] GameObject NormalSweeperSelection;
    [SerializeField] GameObject BookLevelSelection;
    [SerializeField] GameObject TS_Book1;
    


    bool InMainSelection = true;
    bool InUILayer1 = false;
    bool InUILayer2 = false;


    private void Start()
    {
        MainMenuSelection.SetActive(true);
        ReturnButtonUI.SetActive(false);
    }

    public void ReturnButton()
    {
        
        if (InUILayer1 && !InUILayer2)
        {
            InUILayer1 = false;
            ReturnButtonUI.SetActive(false);
            MainMenuSelection.SetActive(true);

            if (NormalSweeperSelection.activeInHierarchy)
            {
                NormalSweeperSelection.SetActive(false);
            }
            if (BookLevelSelection.activeInHierarchy)
            {
                BookLevelSelection.SetActive(false);
            }
        }

        if (InUILayer2 && InUILayer1)
        {
            InUILayer2 = false; 
            ReturnButtonUI.SetActive(true);

            if (TS_Book1.activeInHierarchy)
            {
                TS_Book1.SetActive(false);
                BookLevelSelection.SetActive(true);
            }

        }
    }

    public void OpenNormalSpeeperUI()
    {
        InUILayer1 = true;
        ReturnButtonUI.SetActive(true);
        NormalSweeperSelection.SetActive(true);
        MainMenuSelection.SetActive(false);

    }
    public void OpenTreasureSpeeperUI()
    {
        InUILayer1 = true;
        ReturnButtonUI.SetActive(true);
        BookLevelSelection.SetActive(true);
        MainMenuSelection.SetActive(false);
    }

    public void OpenBook1UI()
    {
        InUILayer2 = true;
        TS_Book1.SetActive(true);
        BookLevelSelection.SetActive(false);
    }



}

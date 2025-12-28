using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] GameObject GameTitle;
    [SerializeField] GameObject ReturnButtonUI;
    [SerializeField] GameObject MainMenuSelection;
    [SerializeField] GameObject SettingsMenu_UI;
    [SerializeField] GameObject NormalSweeperSelection;
    [SerializeField] GameObject BookLevelSelection;
    [SerializeField] GameObject TS_Book1;
    [SerializeField] Text GameCurrentcyText;

    [SerializeField] Toggle VibrationToggle;
    [SerializeField] Toggle GyroToggle;
    [SerializeField] Toggle AudioToggle;


    bool InMainSelection = true;
    bool InUILayer1 = false;
    bool InUILayer2 = false;

    
    
    private void Start()
    {
        if (PlayerPrefs.GetInt("GameCurrency") == 0) //create memory storage
        {
            PlayerPrefs.SetInt("GameCurrency", 0);
            PlayerPrefs.Save();
        }


        if (PlayerPrefs.GetInt("AudioINT") == 0)
        {
            AudioToggle.isOn = true;
        }
        else
        {
            AudioToggle.isOn = false;
        }
        if (PlayerPrefs.GetInt("VibrationINT") == 0)
        {
            VibrationToggle.isOn = true;
        }
        else
        {
            VibrationToggle.isOn = false;
        }
        if (PlayerPrefs.GetInt("GyroINT") == 0)
        {
            GyroToggle.isOn = true;
        }
        else
        {
            GyroToggle.isOn = false;
        }



        MainMenuSelection.SetActive(true);
        ReturnButtonUI.SetActive(false);

        GameCurrentcyText.text = PlayerPrefs.GetInt("GameCurrency").ToString();
    }

    public void ReturnButton()
    {
        if (PlayerPrefs.GetInt("VibrationINT") == 0)
        {
            PhoneVibration.ButtonVibration();
        }

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
            if (SettingsMenu_UI.activeInHierarchy)
            {
                SettingsMenu_UI.SetActive(false);
            }
        }

        if (InUILayer2 && InUILayer1)
        {
            InUILayer2 = false; 
            ReturnButtonUI.SetActive(true);

            if (TS_Book1.activeInHierarchy)
            {
                GameTitle.SetActive(true);
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

        PlayerPrefs.SetInt("GameMode", 1);
        PlayerPrefs.Save();

    }
    public void OpenTreasureSpeeperUI()
    {
        InUILayer1 = true;
        ReturnButtonUI.SetActive(true);
        BookLevelSelection.SetActive(true);
        MainMenuSelection.SetActive(false);
        
        PlayerPrefs.SetInt("GameMode", 2);
        PlayerPrefs.Save();
    }

    public void OpenBook1UI()
    {
        GameTitle.SetActive(false);
        InUILayer2 = true;
        TS_Book1.SetActive(true);
        BookLevelSelection.SetActive(false);
    }
    public void OpenSettingsMenu()
    {
        GameTitle.SetActive(false);
        InUILayer1 = true;
        ReturnButtonUI.SetActive(true);
        SettingsMenu_UI.SetActive(true);
    }


    public void ToggleVibration()
    {
        if (VibrationToggle.isOn)
        {
            //Debug.Log("Vib ON");
            PlayerPrefs.SetInt("VibrationINT", 0);
        }
        else
        {
            //Debug.Log("Vib OFF");
            PlayerPrefs.SetInt("VibrationINT", 1);
        }
        PlayerPrefs.Save();
    }

    public void ToggleGyro()
    {
        if (GyroToggle.isOn)
        {
            //Debug.Log("GYR OFF");
            PlayerPrefs.SetInt("GyroINT", 0);
        }
        else
        {
            //Debug.Log("GYR OFF");
            PlayerPrefs.SetInt("GyroINT", 1);
        }
        PlayerPrefs.Save();
    }

    

        public void ToggleAudio()
    {
        if (AudioToggle.isOn)
        {
            PlayerPrefs.SetInt("AudioINT", 0);
        }
        else
        {
            PlayerPrefs.SetInt("AudioINT", 1);
        }
        PlayerPrefs.Save();
    }


}

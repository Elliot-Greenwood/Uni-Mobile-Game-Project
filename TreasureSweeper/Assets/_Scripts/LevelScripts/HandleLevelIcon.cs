using UnityEngine;
using UnityEngine.UI;

public class HandleLevelIcon : MonoBehaviour
{
    [SerializeField] string LevelName;

    [SerializeField] Image ThisImage;
    [SerializeField] Sprite NotUnlocked;
    [SerializeField] Sprite Uncovered;


    void Start()
    {
        //ThisImage = GetComponentInChildren<Image>();
        
        if (PlayerPrefs.GetInt(LevelName) == 1)
        {
            ThisImage.sprite = Uncovered;
        }
        else
        {
            ThisImage.sprite = NotUnlocked;
        }

    }


}

using UnityEngine;
using UnityEngine.UI;

public class HandleLevelIcon : MonoBehaviour
{
    [SerializeField] string LevelName;

    Image ThisImage;

    void Start()
    {
        ThisImage = GetComponent<Image>();
        
        if (PlayerPrefs.GetInt(LevelName) == 1)
        {
            ThisImage.color = Color.white;
        }

    }


}

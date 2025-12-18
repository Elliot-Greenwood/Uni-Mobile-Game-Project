using UnityEngine;

public class SetLevelCompletion : MonoBehaviour
{
    [SerializeField] string LevelName;
    public void CompleteLevel()
    {
        PlayerPrefs.SetInt(LevelName, 1);
        PlayerPrefs.Save();
    }
}

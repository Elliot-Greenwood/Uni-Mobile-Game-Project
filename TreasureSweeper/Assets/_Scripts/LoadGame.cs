using UnityEngine;

public class LoadGame : MonoBehaviour
{
    [SerializeField] LoadingScreenScript loadingScreenScript;
    void Start()
    {
        loadingScreenScript.LoadScene("MainMenu");
    }

}

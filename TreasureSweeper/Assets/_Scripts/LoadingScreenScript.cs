using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreenScript : MonoBehaviour
{

    [SerializeField] GameObject LoaderUI;
    [SerializeField] Image progressUI;

    public void LoadScene(string scene)
    {
        Time.timeScale = 1;
        StartCoroutine(LoadSceneCoroutine(scene));

    }

    public IEnumerator LoadSceneCoroutine(string scene)
    {
        progressUI.fillAmount = 0;
        LoaderUI.SetActive(true);

        AsyncOperation SyncOperator = SceneManager.LoadSceneAsync(scene);
        SyncOperator.allowSceneActivation = false;
        float progress = 0;
        while (!SyncOperator.isDone)
        {
            progress = Mathf.MoveTowards(progress, SyncOperator.progress, Time.deltaTime);
            progressUI.fillAmount = progress;
            if (progress >= 0.9f)
            {
                progressUI.fillAmount = 1;
                SyncOperator.allowSceneActivation = true;
            }
            yield return null;


        }


    }

}
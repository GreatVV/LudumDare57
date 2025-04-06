using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField]
    CanvasGroup canvasGroup;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ClearStatic()
    {
        if (Instance)
        {
            SceneManager.sceneLoaded -= Instance.OnSceneLoaded;
        }
        Instance = default;
    } 

    private void Start()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private async void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        while (canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= Time.deltaTime * FadeSpeed;
            await Awaitable.NextFrameAsync();
        }
        gameObject.SetActive(false);
    }

    public float FadeSpeed = 1f;
    public static LoadingScreen Instance;

    public async void FadeIn()
    {
        gameObject.SetActive(true);
        while (canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += Time.deltaTime * FadeSpeed;
            await Awaitable.NextFrameAsync();
        }

        SceneManager.LoadScene(0);
    }
}
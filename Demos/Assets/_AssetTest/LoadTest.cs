using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssetBundles;

public class LoadTest : MonoBehaviour {
    const string variantSceneAssetBundle = "v/testscene";
    const string variantSceneName = "testScene";

    private string[] activeVariants;
    private bool bundlesLoaded;     
    void Awake()
    {
        activeVariants = new string[1];
        bundlesLoaded = false;
    }
	// Use this for initialization
	void Start () {
	    StartCoroutine(BeginExample());

	}

    IEnumerator BeginExample()
    {
        yield return StartCoroutine(Initialize());

        // Set active variants.
        AssetBundleManager.ActiveVariants = activeVariants;

        // 加载测试场景
        yield return StartCoroutine(InitializeLevelAsync(variantSceneName, true));
    }
    // Initialize the downloading url and AssetBundleManifest object.
    protected IEnumerator Initialize()
    {
        DontDestroyOnLoad(gameObject);

        InitializeSourceURL();

        var request = AssetBundleManager.Initialize();

        if (request != null)
            yield return StartCoroutine(request);
    }

    void InitializeSourceURL()
    {
#if ENABLE_IOS_ON_DEMAND_RESOURCES
        if (UnityEngine.iOS.OnDemandResources.enabled)
        {
            // 资源地址
            AssetBundleManager.overrideBaseDownloadingURL += OverrideDownloadingURLForLocalBundles;
            AssetBundleManager.SetSourceAssetBundleURL("odr://");
            return;
        }
#endif
#if DEVELOPMENT_BUILD || UNITY_EDITOR
        AssetBundleManager.SetDevelopmentAssetBundleServer();
        return;
#else
        AssetBundleManager.SetSourceAssetBundleURL(Application.dataPath + "/");
        return;
#endif
    }
    List<string> localBundles = new List<string> { "v/mycube" };
    protected string OverrideDownloadingURLForLocalBundles(string baseAssetBundleName)
    {
        if (localBundles.Contains(baseAssetBundleName))
            return "res://";
        return null;
    }
    protected IEnumerator InitializeLevelAsync(string levelName, bool isAdditive)
    {
        float startTime = Time.realtimeSinceStartup;

        // Load level from assetBundle.
        AssetBundleLoadOperation request = AssetBundleManager.LoadLevelAsync(variantSceneAssetBundle, levelName, isAdditive);
        if (request == null)
            yield break;

        yield return StartCoroutine(request);

        float elapsedTime = Time.realtimeSinceStartup - startTime;
        Debug.Log("Finished loading scene " + levelName + " in " + elapsedTime + " seconds");
    }
}

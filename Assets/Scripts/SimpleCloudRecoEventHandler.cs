using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using Vuforia;


public class MetaDatos
{
    public string nombre;
    public string nFragmento;
    public string url;

    public static MetaDatos CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<MetaDatos>(jsonString);
    }

}

public class SimpleCloudRecoEventHandler : MonoBehaviour
{
    CloudRecoBehaviour mCloudRecoBehaviour;
    private bool mIsScanning = false;
    private string mTargetMetadata = "";
    MetaDatos metaDatosVuforia;

    public ImageTargetBehaviour ImageTargetTemplate;

    void Start()
    {
    }
    // Register cloud reco callbacks
    void Awake()
    {
        mCloudRecoBehaviour = GetComponent<CloudRecoBehaviour>();
        mCloudRecoBehaviour.RegisterOnInitializedEventHandler(OnInitialized);
        mCloudRecoBehaviour.RegisterOnInitErrorEventHandler(OnInitError);
        mCloudRecoBehaviour.RegisterOnUpdateErrorEventHandler(OnUpdateError);
        mCloudRecoBehaviour.RegisterOnStateChangedEventHandler(OnStateChanged);
        mCloudRecoBehaviour.RegisterOnNewSearchResultEventHandler(OnNewSearchResult);
    }
    //Unregister cloud reco callbacks when the handler is destroyed
    void OnDestroy()
    {
        mCloudRecoBehaviour.UnregisterOnInitializedEventHandler(OnInitialized);
        mCloudRecoBehaviour.UnregisterOnInitErrorEventHandler(OnInitError);
        mCloudRecoBehaviour.UnregisterOnUpdateErrorEventHandler(OnUpdateError);
        mCloudRecoBehaviour.UnregisterOnStateChangedEventHandler(OnStateChanged);
        mCloudRecoBehaviour.UnregisterOnNewSearchResultEventHandler(OnNewSearchResult);
    }
    public void OnInitialized(CloudRecoBehaviour cloudRecoBehaviour)
    {
        Debug.Log("Cloud Reco initialized");
    }

    public void OnInitError(CloudRecoBehaviour.InitError initError)
    {
        Debug.Log("Cloud Reco init error " + initError.ToString());
    }

    public void OnUpdateError(CloudRecoBehaviour.QueryError updateError)
    {
        Debug.Log("Cloud Reco update error " + updateError.ToString());
    }
    public void OnStateChanged(bool scanning)
    {
        mIsScanning = scanning;

        if (scanning)
        {
            mCloudRecoBehaviour.ClearObservers();
        }
    }

    // Here we handle a cloud target recognition event
    public void OnNewSearchResult(CloudRecoBehaviour.CloudRecoSearchResult cloudRecoSearchResult )
    {
        // Store the target metadata
        metaDatosVuforia = MetaDatos.CreateFromJSON(cloudRecoSearchResult.MetaData);

        // nFragmento del metadata a int
        int fragmentNumber;
        if (!int.TryParse(metaDatosVuforia.nFragmento, out fragmentNumber))
        {
            Debug.LogWarning("nFragmento no válido: " + metaDatosVuforia.nFragmento);
            return;
        }

        // Solo acepta el target si coincide con el fragmento actual
        if (fragmentNumber != GameManager.Instance.GetCurrentHintIndex + 1) // +1 porque index empieza en 0
        {
            Debug.Log("Target ignorado: no corresponde al fragmento actual");
            return;
        }

        if (ImageTargetTemplate)
        {
            /* Enable the new result with the same ImageTargetBehaviour: */
            ObserverBehaviour observer = mCloudRecoBehaviour.EnableObservers(cloudRecoSearchResult, ImageTargetTemplate.gameObject);

            // Si hemos llegado hasta aquí, es correcto -> instanciar
            StartCoroutine(GetAssetBundle(metaDatosVuforia.url, observer.transform));
        }

        // Stop the scanning by disabling the behaviour
        mCloudRecoBehaviour.enabled = false;
    }

    IEnumerator GetAssetBundle(string url, Transform parent)
    {
        UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);

            string[] allAssetNames = bundle.GetAllAssetNames();
            string gameObjectName = Path.GetFileNameWithoutExtension(allAssetNames[0]).ToString();
            
            GameObject objectFound = bundle.LoadAsset(gameObjectName) as GameObject;

             Instantiate(objectFound, parent);

        }
    }
}

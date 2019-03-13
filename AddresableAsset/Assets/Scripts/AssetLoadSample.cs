using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement;
using System.Collections.Generic;
using System.Collections;

public class AssetLoadSample : MonoBehaviour
{
    [Header("Img")]
    [SerializeField]
    Image img = null;

    [Header("表示する画像名")]
    [SerializeField]
    string spriteName = null;

    [Header("ロードボタン")]
    [SerializeField]
    Button loadBtn;

    [Header("Referenceロードボタン")]
    [SerializeField]
    Button referenceLoadBtn;

    [Header("AllReleaseボタン")]
    [SerializeField]
    Button allReleaseBtn;

    [Header("アセット生成ボタン")]
    [SerializeField]
    Button instantiateAssetBtn = null;

    [Header("Asset参照（Sprite）")]
    [SerializeField]
    private AssetReferenceSprite referenceSprite = null;

    [Header("InstantiateAsset参照（GameObject）")]
    [SerializeField]
    private AssetReferenceGameObject instantiateReferenceGameObject = null;

    /// <summary>
    /// アセットリスト。
    /// </summary>
    List<Sprite> assetList = new List<Sprite>();

    /// <summary>
    /// 生成アセットリスト。
    /// </summary>
    List<GameObject> instantiateAssetObjList = new List<GameObject>();

    void Start()
    {
        loadBtn.onClick.AddListener(Load);
        referenceLoadBtn.onClick.AddListener(ReferenceLoad);
        allReleaseBtn.onClick.AddListener(AllReleaseAsset);
        instantiateAssetBtn.onClick.AddListener(InstantiateAsset);
    }

    /// <summary>
    /// 読み込み。
    /// </summary>
    void Load()
    {
        if(spriteName == null)
        {
            return;
        }

        Addressables.LoadAsset<Sprite>(spriteName)
                    .Completed += (op) => {
                        img.sprite = op.Result;
                        img.SetNativeSize();
                        assetList.Add(op.Result);
                    };
    }

    /// <summary>
    /// 参照読み込み。
    /// </summary>
    void ReferenceLoad()
    {
        if(referenceSprite == null)
        {
            return;
        }

        Addressables.LoadAsset<Sprite>(referenceSprite)
                    .Completed += (op) => {
                        img.sprite = op.Result;
                        img.SetNativeSize();
                        assetList.Add(op.Result);
                    };
    }

    /// <summary>
    /// アセットを生成。
    /// </summary>
    void InstantiateAsset()
    {
        if(instantiateReferenceGameObject == null)
        {
            return;
        }

        Addressables.Instantiate(instantiateReferenceGameObject)
                    .Completed += (op) => {
                        var resultObj = op.Result;
                        resultObj.transform.SetParent(transform);
                        resultObj.transform.localPosition = Vector3.zero;
                        resultObj.transform.localScale = Vector3.one;
                        instantiateAssetObjList.Add(op.Result);
                    };

    }


    /// <summary>
    /// 全ての参照を解放。
    /// </summary>
    void AllReleaseAsset()
    {
        for(var i = 0; i < assetList.Count; ++i)
        {
            Addressables.ReleaseAsset(assetList[i]);
        }

        for(var i = 0; i < instantiateAssetObjList.Count; ++i)
        {
            Addressables.ReleaseInstance(instantiateAssetObjList[i]);
        }
    }
}

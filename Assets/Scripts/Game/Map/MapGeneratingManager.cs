using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneratingManager : MonoBehaviour
{
    #region [var]

    #region [01. Instance]
    /// <summary>
    /// インスタンス
    /// </summary>
    public static MapGeneratingManager Instance { get; private set; }
    #endregion
    
    #region [02. Reference]
    /// <summary>
    /// MapCollector
    /// </summary>
    [SerializeField]
    private MapCollector mapCollector;
    #endregion
    
    #region [03. Map Data]
    /// <summary>
    /// MapList : Start
    /// </summary>
    [SerializeField] 
    private List<GameObject> mapList = new List<GameObject>();
    public List<GameObject> MapList
    {
        get => mapList;
    }
    #endregion
    
    #region [04. Trasform]
    /// <summary>
    /// 生成MapのRoot
    /// </summary>
    [SerializeField] 
    public Transform mapRoot;
    #endregion
    
    #region [05. Information]
    /// <summary>
    /// 最大MapCollectNum
    /// </summary>
    [SerializeField] 
    private int maxTotalMapCollectNum;
    public int MaxTotalMapCollectNum
    {
        get => maxTotalMapCollectNum;
    }
    #endregion

    #region [06. Coroutine]
    /// <summary>
    /// ドア数が０になったマップの数
    /// </summary>
    private int allDoorClosedMapCount = 0;
    /// <summary>
    /// マップ生成状態トリガー
    /// </summary>
    private bool isMapGeneratingFinished = false;
    #endregion

    #region [07. Callback]
    /// <summary>
    /// Map生成終了コールバック
    /// </summary>
    private Action onFinishedMapGenerating;
    #endregion
    
    #endregion

    
    
    #region [func]
    /// <summary>
    /// コンストラクタ
    /// </summary>
    private void Start()
    {
        // インスタンス
        Instance = this;
        // 破棄不可
        DontDestroyOnLoad(this.gameObject);
    }

    /// <summary>
    /// 自動生成開始：一つ目のマップを生成
    /// </summary>
    public void StartGenerating(Action onFinished = null)
    {
        Debug.LogFormat("Map Generating Started", DColor.cyan);
        
        // 生成
        var randomNum = UnityEngine.Random.Range(0, this.mapList.Count);
        var instancedMap = Instantiate(this.mapList[randomNum], this.mapRoot);
        // リストに追加
        this.mapCollector.AddMapToList(instancedMap);
        
        onFinished?.Invoke();
    }

    /// <summary>
    /// 個々のNextMapGenerateControllerが生成を終了した場合、カウントアップ
    /// </summary>
    public void AddAllDoorClosedMapCount()
    {
        this.allDoorClosedMapCount++;
    }
    
    /// <summary>
    /// マップ生成終了判定のコルーチン開始準備
    /// </summary>
    public void WaitForMapGeneratingFinishAsync()
    {
        // コルーチンスタート
        GlobalCoroutine.Play(this.WaitForFinishGenerating(), "CheckMapGeneratingFinished", null);
    }

    /// <summary>
    /// マップ生成終了を判定
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitForFinishGenerating()
    {
        while (!this.isMapGeneratingFinished)
        {
            if (this.allDoorClosedMapCount == this.mapCollector.collectedMapList.Count)
            {
                // トリガーオフ
                this.isMapGeneratingFinished = true;
                
                if (this.mapCollector.CurrentTotalMapCollectNum < maxTotalMapCollectNum)
                    // マップ生成データリセット
                    this.ResetMapGeneratingData();
                else
                    // マップ生成シーケンス終了、次のシーケンスに移行
                    this.MoveToNextSequence();
            }
            
            yield return null;
        }
    }

    /// <summary>
    /// 次のシーケンスに移行
    /// </summary>
    private void MoveToNextSequence()
    {
        Debug.LogFormat("Map Generating Has Done", DColor.cyan);

        GlobalCoroutine.Stop("CheckMapGeneratingFinished");
        this.allDoorClosedMapCount = 0;
        this.isMapGeneratingFinished = false;
        
        // マップ生成終了コールバック
        this.onFinishedMapGenerating?.Invoke();
    }

    /// <summary>
    /// マップ生成終了コールバック
    /// </summary>
    /// <param name="onCompleted"></param>
    public void MapGeneratingFinished(Action onCompleted)
    {
        this.onFinishedMapGenerating = onCompleted;
    }

    /// <summary>
    /// マップ生成データリセット
    /// </summary>
    private void ResetMapGeneratingData()
    {
        Debug.LogFormat("Reset Map Generating Data ", DColor.cyan);
        
        // コルーチン関連データの初期化
        GlobalCoroutine.Stop("CheckMapGeneratingFinished");
        this.allDoorClosedMapCount = 0;
        this.isMapGeneratingFinished = false;
        
        // MapCollectorのデータをリセット
        this.mapCollector.ResetData();

        Debug.LogFormat("Reset Done ", DColor.cyan);
        // マップ生成再開
        this.RestartMapGenerating();
    }

    /// <summary>
    /// マップ生成を再開
    /// </summary>
    private void RestartMapGenerating()
    {
        this.StartGenerating(this.WaitForMapGeneratingFinishAsync);
    }

    // /// <summary>
    // /// マップ生成再開のデバッグ用　　　＊＊＊＊＊　注：使用しないときはコメントアウトすること　＊＊＊＊＊
    // /// </summary>
    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.F4))
    //     {
    //         this.ResetMapGeneratingData();
    //     }
    // }
    #endregion
}

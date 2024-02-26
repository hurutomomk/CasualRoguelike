using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    #region [var]

    #region [01. Instance]
    
    /// <summary>
    /// インスタンス
    /// </summary>
    public static GameManager Instance { get; private set; }
    
    #endregion

    #region [02. 参照]
    
    /// <summary>
    /// MapGeneratingManager
    /// </summary>
    [SerializeField]
    private MapGeneratingManager mapGeneratingManager;

    /// <summary>
    /// SpawnManager
    /// </summary>
    [SerializeField]
    private SpawnManager spawnManager;
    
    #endregion
    
    #endregion
    
    
    
    #region [func]

    #region [00. コンストラクタ]
    /// <summary>
    /// コンストラクタ
    /// </summary>
    private void Awake()
    {
        // fps制限
        Application.targetFrameRate = 60;
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    private void Start()
    {
        // インスタンス
        Instance = this;
        // 破棄不可
        DontDestroyOnLoad(this.gameObject);
        
        // 画面スリープ不可
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        
        // Logo表示
        this.Logo();
    }
    #endregion
    
    
    
    #region [01. Logo画面]
    /// <summary>
    /// Logo表示
    /// </summary>
    public void Logo()
    {
        // Logo表示
        
        // Title表示
        this.Title();
    }
    #endregion
    
    
    
    #region [02. Title画面]
    /// <summary>
    /// Title表示
    /// </summary>
    public void Title()
    {
        // 臨時
        this.MapGeneratingSequence();

        // 臨時
        this.TransitionEffectOnTitleToStage();
    }
    #endregion
    
    
    
    #region [03. Transition Effect]
    /// <summary>
    /// TransitionEffect再生：TitleToStage
    /// </summary>
    public void TransitionEffectOnTitleToStage()
    {
        this.mapGeneratingManager.MapGeneratingFinished(() =>
        {
            // Spawnシーケンス
            this.SpawnSequence(() =>
            {
                // MapEvent Setting シーケンス
                this.MapEventSettingSequence(() =>
                {
                    Debug.LogFormat("MapEvent Setting Sequence Has Finished", DColor.cyan);
                });
            });
        });
    }
    #endregion
    
    
    
    #region [04. Map Generating Sequence]
    /// <summary>
    /// Map自動生成シーケンス
    /// </summary>
    public void MapGeneratingSequence()
    {
        // Map生成開始
        this.mapGeneratingManager.StartGenerating(this.mapGeneratingManager.WaitForMapGeneratingFinishAsync);
    }
    #endregion
    

    
    #region [05. Spawn Sequence]
    /// <summary>
    /// 各種GameObjectのSpawnシーケンス
    /// </summary>
    public void SpawnSequence(Action onFinished)
    {
        // PlayerをSpawn
        this.spawnManager.SpawnPlayerAtFirstTime(() =>
        {
            onFinished?.Invoke();
        });
    } 

    #endregion
    
    
    
    #region [06. MapEvent Setting Sequence]
    /// <summary>
    /// 各種GameObjectのSpawnシーケンス
    /// </summary>
    public void MapEventSettingSequence(Action onFinished)
    {
        
        
        onFinished?.Invoke();
    } 

    #endregion
    
    #endregion
}

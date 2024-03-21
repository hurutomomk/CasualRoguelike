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
    /// TitleController
    /// </summary>
    [SerializeField]
    private TitleController titleController;
    /// <summary>
    /// TransitionEffectController
    /// </summary>
    [SerializeField]
    private TransitionEffectController transitionEffectController;
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
    /// <summary>
    /// MapEventManager
    /// </summary>
    [SerializeField]
    private MapEventManager mapEventManager;
    /// <summary>
    /// PlayerStatusManager
    /// </summary>
    [SerializeField]
    private PlayerStatusManager playerStatusManager;
    
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
        // Title表示
        this.titleController.ShowTitle();
    }
    #endregion
    
    
    
    #region [03. Transition Effect]
    /// <summary>
    /// TransitionEffect再生：TitleToStage
    /// </summary>
    public void TransitionEffectOnTitleToStage()
    {
        // TransitionInEffect再生
        this.transitionEffectController.PlayInEffect(() =>
        {
            // Title非表示
            this.titleController.SetStageInfo(false);
            this.titleController.SetBackground(false);
            
            // Map自動生成シーケンス
            this.MapGeneratingSequence();
        
            // Map生成終了
            MapGeneratingManager.Instance.MapGeneratingFinished(() =>
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
                
                // TransitionOutEffect再生
                this.transitionEffectController.PlayOutEffect();
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
            // PlayerのStatusをセット
            this.playerStatusManager.SetPlayerStatus();
            
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
        this.mapEventManager.SetEvent(() =>
        {
            
            
            onFinished?.Invoke();
        });
    } 

    #endregion
    
    #endregion
}

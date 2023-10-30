using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region [var]

    #region [01. Instance]
    
    /// <summary>
    /// インスタンス
    /// </summary>
    public static GameManager Instance { get; private set; }
    
    #endregion

    #region [01. Instance]
    
    /// <summary>
    /// MapGeneratingManager
    /// </summary>
    [SerializeField]
    private MapGeneratingManager mapGeneratingManager;
    
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
        this.MapGeneratingSequence();
    }
    #endregion
    
    
    
    #region [03. Transition Effect]
    /// <summary>
    /// TransitionEffect再生：TitleToStage
    /// </summary>
    public void TransitionEffectOnTitleToStage()
    {
        
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
    

    
    
    
    #endregion
}

using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerMovementManager : MonoBehaviour
{
    #region [var]

    #region [00. コンストラクタ]

    /// <summary>
    /// インスタンス
    /// </summary>
    public static PlayerMovementManager Instance { get; private set; }
   
    [Header(" --- Reference")]
    /// <summary>
    /// UIButtonController
    /// </summary>
    [SerializeField]
    private UIButtonController uIbuttonController;
    /// <summary>
    /// UIDialogController
    /// </summary>
    [SerializeField]
    private UIDialogController uIDialogController;
    
    #endregion
    
    #region [02. メイン制御]
   
    [Header(" --- コルーチン")]
    /// <summary>
    /// 一時停止および再開用のIEnumerator
    /// </summary>
    private IEnumerator coroutine;
    
    [Header(" --- Map Info")]
    /// <summary>
    /// Playerが止まったMapのMapInfo
    /// </summary>
    private MapInfo mapInfo = null;
    #endregion

    #region [03. Map Info 取得関連]
    /// <summary>
    /// 移動可能方向のトリガー
    /// </summary>
    [Header(" --- Player移動関連")]
    [SerializeField] 
    private bool canMoveToNorth = false;
    public bool CanMoveToNorth { get => canMoveToNorth; }   
    [SerializeField] 
    private bool canMoveToEast = false;
    public bool CanMoveToEast { get => canMoveToEast; }   
    [SerializeField] 
    private bool canMoveToSouth = false;
    public bool CanMoveToSouth { get => canMoveToSouth; }   
    [SerializeField] 
    private bool canMoveToWest = false;
    public bool CanMoveToWest { get => canMoveToWest; }
    #endregion
    
    #endregion
    
    
    
    #region [func]

    #region [00. コンストラクタ]
    /// <summary>
    /// コンストラクタ
    /// </summary>
    private void Start()
    {
        // インスタンス
        Instance = this;

        // データ初期化
        this.InitData();
    }
    
    /// <summary>
    /// 各種データを初期化
    /// </summary>
    private void InitData()
    {
        this.mapInfo = null;
    }
    
    #endregion
    
    #region [01. メイン制御]

    #region [001. Player Turn]
    /// <summary>
    /// 移動ボタン押下時の処理
    /// </summary>
    /// <param name="directionStr"></param>
    public void OnClickMoveButton(string directionStr)
    {
        // PlayerTurnコルーチン開始
        this.PlayerTurnAsync(directionStr);
    }

    /// <summary>
    /// PlayerTurnコルーチン開始
    /// </summary>
    /// <param name="directionStr"></param>
    public void PlayerTurnAsync(string directionStr)
    {
        // コルーチンスタート
        if (this.coroutine != null)
            this.coroutine = null;
        coroutine = this.PlayerTurn(directionStr);
        StartCoroutine(coroutine);
    }

    /// <summary>
    /// PlayerTurnコルーチン
    /// </summary>
    /// <param name="directionStr"></param>
    /// <returns></returns>
    IEnumerator PlayerTurn(string directionStr)
    {
        Debug.LogFormat($"Coroutine [PlayerTurn] Activated", DColor.white);

        // 各種データを初期化
        this.InitData();
        
        // ボタンタッチ無効
        this.uIbuttonController.DisableButtonTouch();
        
        // Player移動開始
        PlayerMovementController.Instance.PlayerMove(directionStr, () =>
        {
            // Playerの現在座標
            var playerPos = PlayerMovementController.Instance.transform.position;
            // Player座標と一致するMapを検索
            foreach (var map in MapCollector.Instance.collectedMapList)
            {
                // 一致した場合
                if (map.transform.position == playerPos)
                {
                    // MapのMapInfoを更新
                    this.mapInfo = map.GetComponent<MapInfo>();
                }
            }
                
            // MapEventを消化したMapをOpenStateに変更
            this.mapInfo.SetMapSpriteToOpenState();
        });
        
        // Player移動終了まで待機
        yield return new WaitForSeconds(2f);
        
        // PlayerTurnコルーチン停止
        this.StopPlayerTurnCoroutine(directionStr);
    }

    /// <summary>
    /// PlayerTurnコルーチン停止
    /// </summary>
    private void StopPlayerTurnCoroutine(string directionStr)
    {
        DOVirtual.DelayedCall(.1f, () =>
        {
            StopCoroutine(this.coroutine);
            this.coroutine = null;
            
            //　CheckEventコルーチン開始
            this.CheckEventAsync();
        });
    }

    /// <summary>
    /// 再生中PlayerTurnコルーチンの一時停止
    /// </summary>
    public void StopPlayerTurnCoroutineAtMoment()
    {
        StopCoroutine(this.coroutine);
    }

    /// <summary>
    /// 一時停止中のPlayerTurnコルーチンの再開
    /// </summary>
    public void StartPlayerTurnCoroutineAgain()
    {
        StartCoroutine(this.coroutine);
    }
    #endregion
    
    
    
    #region [003. Check Event Turn]
    /// <summary>
    /// CheckEventコルーチン開始
    /// </summary>
    public void CheckEventAsync()
    {
        // コルーチンスタート
        if (this.coroutine != null)
            this.coroutine = null;
        coroutine = this.CheckEvent();
        StartCoroutine(coroutine);
    }

    /// <summary>
    /// CheckEventコルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator CheckEvent()
    {
        Debug.LogFormat($"Coroutine [CheckEvent] Activated", DColor.white);
        
        // ItemDialogを1回のみ表示するためのトリガー
        bool isEventDialogOpened = false;

        // Event発生ありの場合、EventDialogを表示
        if (this.mapInfo != null && this.mapInfo.IsMapEventFinished == false)
        {
            Debug.LogFormat("Event Checking", DColor.cyan);
            
            if (!isEventDialogOpened)
            {
                // MapEventDialogを表示（初回のみ）
                this.uIDialogController.ShowEventDialog(
                    this.uIDialogController.Dialog_Event.transform
                    , this.mapInfo
                    , () =>
                    {
                        // 消化したMapEventをFinishedStateに変更
                        this.mapInfo.SetMapEventToFinishedState();
                    });
                    
                isEventDialogOpened = true;
            }
        }

        // Event発生なしの場合、即Loopを終了
        if (this.mapInfo != null && this.mapInfo.IsMapEventFinished == true)
        {
            
        }
        
        // Player移動終了まで待機
        yield return new WaitForSeconds(1f);
        
        // CheckEventコルーチン停止
        this.StopCheckEventCoroutine();
    }
    
    /// <summary>
    /// CheckEventコルーチン停止
    /// </summary>
    void StopCheckEventCoroutine()
    {
        DOVirtual.DelayedCall(.1f, () =>
        {
            StopCoroutine(this.coroutine);
            this.coroutine = null;
            
            DOVirtual.DelayedCall(.2f, () =>
            {
                // ボタンタッチ有効
                this.uIbuttonController.EnableButtonTouchExceptMovementButton();
            });
        });
    }
    #endregion
    
    #endregion
    
    #region [04. MapInfo取得]
    
    /// <summary>
    /// 現在座標のMapInfoを取得
    /// </summary>
    public void GetMapInfo()
    {
        // Playerの現在座標
        var playerPos = PlayerMovementController.Instance.transform.position;
        
        // 生成済みMapリストと比較
        this.CompareWithMapInfo(playerPos);
    }
    
    /// <summary>
    /// 生成済みMapリストと比較
    /// </summary>
    /// <param name="playerPos"></param>
    private void CompareWithMapInfo(Vector3 playerPos)
    {
        foreach (var map in MapCollector.Instance.collectedMapList)
        {
            if (map.transform.position == playerPos)
            {
                // 各トリガーをセット
                var info = map.gameObject.GetComponent<MapInfo>();
                this.canMoveToNorth = info.CanMoveToNorth;
                this.canMoveToEast = info.CanMoveToEast;
                this.canMoveToSouth = info.CanMoveToSouth;
                this.canMoveToWest = info.CanMoveToWest;

                // トリガーによって各MovementButtonの表示切り替え
                this.SetMovementButton();
                
                return;
            }
        }
    }

    /// <summary>
    /// トリガーによって各MovementButtonの表示切り替え
    /// </summary>s
    private void SetMovementButton()
    {
        this.uIbuttonController.SetEachMovementButtonEnableState(this.uIbuttonController.NorthButton, this.canMoveToNorth);
        this.uIbuttonController.SetEachMovementButtonEnableState(this.uIbuttonController.EastButton, this.canMoveToEast);
        this.uIbuttonController.SetEachMovementButtonEnableState(this.uIbuttonController.SouthButton, this.canMoveToSouth);
        this.uIbuttonController.SetEachMovementButtonEnableState(this.uIbuttonController.WestButton, this.canMoveToWest);
    }
    
    #endregion
    
    #endregion
}

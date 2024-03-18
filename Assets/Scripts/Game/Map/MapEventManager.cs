using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MapEventManager : MonoBehaviour
{
    #region [var]

    [Header(" --- Reference")]
    /// <summary>
    /// UIDialogController
    /// </summary>
    [SerializeField]
    private UIDialogController uiDialogController;
    /// <summary>
    /// PlayerStatusManager
    /// </summary>
    [SerializeField]
    private PlayerStatusManager playerStatusManager;
    
    [Header(" --- Setting Events")]
    /// <summary>
    /// ExitDoor Prefab
    /// </summary>
    [SerializeField]
    private GameObject exitDoorPrefab;
    /// <summary>
    /// DoorKey Prefab
    /// </summary>
    [SerializeField]
    private GameObject doorKeyPrefab;
    /// <summary>
    /// Enemy Prefab
    /// </summary>
    [SerializeField]
    private GameObject enemyPrefab;
    /// <summary>
    /// Shrine Prefab
    /// </summary>
    [SerializeField]
    private GameObject shrinePrefab;

    /// <summary>
    /// ExitDoorOpen関連
    /// </summary>
    private MapEventController exitDoorMapEventController;
    private bool isExitDoorLogShown = false;
    public bool IsExitDoorLogShown { get => this.isExitDoorLogShown; }
    
    private bool isExitDoorOpened= false;
    public bool IsExitDoorOpened { get => this.isExitDoorOpened; }
    
    /// <summary>
    /// Enemy および　Shrineの生成数関連
    /// </summary>
    private int totalMapCollectNum;
    private int enemyNum;
    
    /// <summary>
    /// ExitDoorOpen関連
    /// </summary>
    private MapEventController lootShrineMapEventController;
    
    [Header(" --- Looting Shrine List")]
    /// <summary>
    /// Common Shrine List
    /// </summary>
    [SerializeField]
    private List<Shrine> commonShrineList = new List<Shrine>();
    /// <summary>
    /// Epic Shrine List
    /// </summary>
    [SerializeField]
    private List<Shrine> epicShrineList = new List<Shrine>();
    /// <summary>
    /// Legend Shrine List
    /// </summary>
    [SerializeField]
    private List<Shrine> legendShrineList = new List<Shrine>();

    /// <summary>
    /// Shrineの各ランク毎の選定率
    /// </summary>
    [SerializeField]
    private float commonRate = 0f;
    [SerializeField]
    private float epicRate = 0f;
    
    #endregion
    
    
    
    
    #region [func]
    /// <summary>
    /// コンストラクタ
    /// </summary>
    private void Start()
    {
        // 破棄不可
        DontDestroyOnLoad(this.gameObject);
    }

    #region [01. Setting Events]

    /// <summary>
    /// Map上にMapEventを生成
    /// </summary>
    /// <param name="onFinished"></param>
    public void SetEvent(Action onFinished)
    {
        // 初期化
        this.totalMapCollectNum = 0;
        this.enemyNum = 0;
        
        // ExitDoorを生成
        this.SetExitDoor(() =>
        {
            // DoorKeyを生成
            this.SetDoorKey(() =>
            {
                // Enemyを生成
                this.SetEnemy(() =>
                {
                    // Shrineを生成
                    this.SetShrine(() =>
                    {
                        onFinished?.Invoke();
                    });
                });
            });
        });
    }

    #region [001. SetExitDoor]
    /// <summary>
    /// ExitDoorを生成
    /// </summary>
    /// <param name="onFinished"></param>
    private void SetExitDoor(Action onFinished)
    {
        // Map選定
        var collectedMapList = MapCollector.Instance.collectedMapList;
        
        for (int num = 0; num < 1; num++)
        {
            // Map選定
            var randomNum = UnityEngine.Random.Range(0, collectedMapList.Count);
            var mapInfo = collectedMapList[randomNum].GetComponent<MapInfo>();
            
            // PlayerがSpawnしているMapか、既にMapEventが生成されたMapだった場合、やり直し
            if (mapInfo.IsPlayerAlreadySpawned || mapInfo.IsMapEventSet)
            {
                num -= 1;
                continue;
            }
            
            // PlayerがSpawnされていない、且、MapEventが生成されていない場合
            if(!mapInfo.IsPlayerAlreadySpawned && !mapInfo.IsMapEventSet)
            {
                // ExitDoorを生成
                var exitDoorObj = Instantiate(this.exitDoorPrefab, mapInfo.MapEventRoot);
                // ExitDoorのMapEventControllerを個別に記録
                this.exitDoorMapEventController = exitDoorObj.GetComponent<MapEventController>();
                
                // セット済みトリガー
                mapInfo.SetMapEventSettingTriggerOn();
                // MapをOpenStateに変更
                mapInfo.SetMapSpriteToOpenState();
                // MapEventControllerをセット
                mapInfo.SetMapEventController(this.exitDoorMapEventController);
                // MapのGameObject名の後ろにEvent名を追加
                mapInfo.SetEventNameOnMapName("ExitDoor");
            }
        }

        onFinished?.Invoke();
    }

    #endregion
    
    
    
    #region [002. SetDoorKey]
    /// <summary>
    /// DoorKeyを生成
    /// </summary>
    /// <param name="onFinished"></param>
    private void SetDoorKey(Action onFinished)
    {
        // Map選定
        var collectedMapList = MapCollector.Instance.collectedMapList;
        
        for (int num = 0; num < 1; num++)
        {
            // Map選定
            var randomNum = UnityEngine.Random.Range(0, collectedMapList.Count);
            // 該当MapのMapInfo
            var mapInfo = collectedMapList[randomNum].GetComponent<MapInfo>();
            
            // PlayerがSpawnしているMapか、既にMapEventが生成されたMapだった場合、やり直し
            if (mapInfo.IsPlayerAlreadySpawned || mapInfo.IsMapEventSet)
            {
                num -= 1;
                continue;
            }
            
            // PlayerがSpawnされていない、且、MapEventが生成されていない場合
            if(!mapInfo.IsPlayerAlreadySpawned && !mapInfo.IsMapEventSet)
            {
                // DoorKeyを生成
                var doorKeyObj = Instantiate(this.doorKeyPrefab, mapInfo.MapEventRoot);
                
                // セット済みトリガー
                mapInfo.SetMapEventSettingTriggerOn();
                // MapEventControllerをセット
                mapInfo.SetMapEventController(doorKeyObj.GetComponent<MapEventController>());
                // MapのGameObject名の後ろにEvent名を追加
                mapInfo.SetEventNameOnMapName("DoorKey");
            }
        }

        onFinished?.Invoke();
    }

    #endregion

    
    
    #region [003. SetEnemy]
    /// <summary>
    /// Enemyを生成
    /// </summary>
    /// <param name="onFinished"></param>
    private void SetEnemy(Action onFinished)
    {
        // Map選定
        var collectedMapList = MapCollector.Instance.collectedMapList;
        this.totalMapCollectNum = MapCollector.Instance.CurrentTotalMapCollectNum;
        this.enemyNum = totalMapCollectNum / 3;
        
        for (int num = 0;  num < enemyNum - 1; num++)
        {
            // Map選定
            var randomNum = UnityEngine.Random.Range(0, collectedMapList.Count);
            // 該当MapのMapInfo
            var mapInfo = collectedMapList[randomNum].GetComponent<MapInfo>();
            
            // PlayerがSpawnしているMapか、既にMapEventが生成されたMapだった場合、やり直し
            if (mapInfo.IsPlayerAlreadySpawned || mapInfo.IsMapEventSet)
            {
                num -= 1;
                continue;
            }
            
            // PlayerがSpawnされていない、且、MapEventが生成されていない場合
            if(!mapInfo.IsPlayerAlreadySpawned && !mapInfo.IsMapEventSet)
            {
                // DoorKeyを生成
                var enemyObj = Instantiate(this.enemyPrefab, mapInfo.MapEventRoot);
                
                // セット済みトリガー
                mapInfo.SetMapEventSettingTriggerOn();
                // MapEventControllerをセット
                mapInfo.SetMapEventController(enemyObj.GetComponent<MapEventController>());
                // MapのGameObject名の後ろにEvent名を追加
                mapInfo.SetEventNameOnMapName("Enemy");
            }
        }

        onFinished?.Invoke();
    }
    #endregion
    
    
    
    #region [004. SetShrine]
    /// <summary>
    /// Shrineを生成
    /// </summary>
    /// <param name="onFinished"></param>
    private void SetShrine(Action onFinished)
    {
        // Map選定
        var collectedMapList = MapCollector.Instance.collectedMapList;
        var shrineNum = this.totalMapCollectNum - this.enemyNum;
        
        for (int num = 0;  num < shrineNum - 1 ; num++)
        {
            // Map選定
            var randomNum = UnityEngine.Random.Range(0, collectedMapList.Count);
            // 該当MapのMapInfo
            var mapInfo = collectedMapList[randomNum].GetComponent<MapInfo>();
            
            // PlayerがSpawnしているMapか、既にMapEventが生成されたMapだった場合、やり直し
            if (mapInfo.IsPlayerAlreadySpawned || mapInfo.IsMapEventSet)
            {
                num -= 1;
                continue;
            }
            
            // PlayerがSpawnされていない、且、MapEventが生成されていない場合
            if(!mapInfo.IsPlayerAlreadySpawned && !mapInfo.IsMapEventSet)
            {
                // DoorKeyを生成
                var shrineObj = Instantiate(this.shrinePrefab, mapInfo.MapEventRoot);
                // LootBoxのMapEventControllerを個別に記録
                this.lootShrineMapEventController = shrineObj.GetComponent<MapEventController>();
                
                // セット済みトリガー
                mapInfo.SetMapEventSettingTriggerOn();
                // MapEventControllerをセット
                mapInfo.SetMapEventController(shrineObj.GetComponent<MapEventController>());
                // MapのGameObject名の後ろにEvent名を追加
                mapInfo.SetEventNameOnMapName("Shrine");
                
                // LootBoxから出るItemを前もって抽選
                this.lootShrineMapEventController.SetLootedShrine(this.LootingShrine());
            }
        }

        onFinished?.Invoke();
    }
    #endregion
    
    #endregion

    
    
    #region [02. Looting Shrine]
    /// <summary>
    /// Shrine Looting
    /// </summary>
    private Shrine LootingShrine()
    {
        Shrine shrine = null;
        
        // Item選定
        float randomLotNum = UnityEngine.Random.Range(0, 100);
        if (randomLotNum <= this.commonRate)
        {
            int randomShrineNum = UnityEngine.Random.Range(0, this.commonShrineList.Count);
            shrine = this.commonShrineList[randomShrineNum];
        }
        else if (this.commonRate < randomLotNum && randomLotNum <= this.commonRate + this.epicRate)
        {
            int randomShrineNum = UnityEngine.Random.Range(0, this.epicShrineList.Count);
            shrine = this.epicShrineList[randomShrineNum];
        }
        else if (this.commonRate + this.epicRate < randomLotNum && randomLotNum <= 100f)
        {
            int randomShrineNum = UnityEngine.Random.Range(0, this.legendShrineList.Count);
            shrine = this.legendShrineList[randomShrineNum];
        }
        
        return shrine;
    }

    #endregion
    
    

    #region [03. Event Execution]
    /// <summary>
    /// MapEvent実行
    /// </summary>
    /// <param name="targetMapEvent"></param>
    /// <param name="targetMapEventController"></param>
    public void DoWhatMapEventDoes(MapEvent targetMapEvent, MapEventController targetMapEventController)
    {
        Debug.LogFormat($"this MapEvent is ::: {targetMapEvent.eventName} :::", DColor.cyan);
        
        switch (targetMapEvent.eventID)
        {
            case 0:
                
                break;
            case 1:
                // Open ExitDoor 
                this.SetExitDoorToOpenState();
                // DoorKey Count +1 
                this.playerStatusManager.IncreaseDoorKeyCount();
                break;
            case 2:
                BattleManager.Instance.InstantiateEnemy();
                
                // BattleDialog表示：PlayerBattleDialog
                this.uiDialogController.
                    ShowBattleDialog(this.uiDialogController.Dialog_Battle.transform, () =>
                    {
                        // Battle開始アニメーションの再生
                        BattleManager.Instance.StartBattleAnim(0);
                    });

                DOVirtual.DelayedCall(0.15f, () =>
                {
                    // カメラアニメーションを再生
                    PlayerMovementController.Instance.PlayCameraAnimOnBattleBegin(() => { });
                });
                    
                Debug.LogFormat("Player Battle 開始", DColor.cyan);
                break;
            case 3:
                this.playerStatusManager.AddStatusBonus(targetMapEventController.LootedShrine);
                break;
        }
    }
    
    /// <summary>
    /// ExitDoorを開いた状態に変更
    /// </summary>
    private void SetExitDoorToOpenState()
    {
        this.SetExitDoorLogBoolState(true);
        this.SetExitDoorBoolState(true);
        this.exitDoorMapEventController.SetExitDoorSpriteToFinishedSprite();
        
        // TODO:: ExitDoorのEvent実行を有効化し、該当マップ到着時StageClear処理を開始
    }

    /// <summary>
    /// ExitDoorLogが開いたか否かのトリガー
    /// </summary>
    /// <param name="state"></param>
    public void SetExitDoorLogBoolState(bool state)
    {
        this.isExitDoorLogShown = state;
    }

    /// <summary>
    /// ExitDoorが開いたか否かのトリガー
    /// </summary>
    /// <param name="state"></param>
    public void SetExitDoorBoolState(bool state)
    {
        this.isExitDoorOpened = state;
    }
    #endregion
    
    #endregion
}

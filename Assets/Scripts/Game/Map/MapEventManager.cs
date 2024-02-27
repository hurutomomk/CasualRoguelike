using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEventManager : MonoBehaviour
{
    #region [var]

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
        

        onFinished?.Invoke();
    }
    #endregion

    #endregion
    
    #endregion
}

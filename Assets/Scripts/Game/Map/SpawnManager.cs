using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnManager : MonoBehaviour
{
    #region [var]
    
    #region [01.Spawn Player]
    
    [Header(" --- Spawn Player 関連")]
    /// <summary>
    /// PlayerRoot Transform
    /// </summary>
    [SerializeField]
    private Transform playerRootTransform;
    /// <summary>
    /// Player Prefab
    /// </summary>
    [SerializeField]
    private GameObject playerPrefab;
    
    #endregion
    
    #endregion
    
    
    #region [func]

    #region [00.コンストラクタ]
    
    /// <summary>
    /// コンストラクタ
    /// </summary>
    private void Start()
    {
        // 破棄不可
        DontDestroyOnLoad(this.gameObject);
    }
    
    #endregion

    #region [01.Spawn Player]
    
    /// <summary>
    /// Spawn Player
    /// </summary>
    /// <param name="onFinished"></param>
    public void SpawnPlayerAtFirstTime(Action onFinished)
    {
        // Map選定
        var collectedMapList = MapCollector.Instance.collectedMapList;
        var randomNum = UnityEngine.Random.Range(0, collectedMapList.Count);
        var mapInfo = collectedMapList[randomNum].GetComponent<MapInfo>();
        
        if (!mapInfo.IsEnemyAlreadySpawned)
        {
            // Playerを生成
            var playerObj = Instantiate(this.playerPrefab, this.playerRootTransform);

            // Playerの座標を記録
            playerObj.transform.position = mapInfo.transform.position;
            
            // 生成済みトリガー
            mapInfo.SetPlayerSpawnTriggerOn();
            // MapをOpenStateに変更
            mapInfo.SetMapSpriteToOpenState();
            // MapEventが発生しないようにEvent終了トリガーをセット
            mapInfo.SetMapEventFinishedTriggerOn();
            // MapのGameObject名の後ろにEvent名を追加
            mapInfo.SetEventNameOnMapName("PlayerStartPoint");
        }
        
        onFinished?.Invoke();
    }

    #endregion
    
    #endregion
}

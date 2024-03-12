using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapEventController : MonoBehaviour
{
    #region [var]

    [Header(" --- Map Event Common")]
    /// <summary>
    /// ScriptableIbject上のMapEvent情報 
    /// </summary>
    [SerializeField]
    private MapEvent mapEvent;
    public MapEvent MapEvent { get => this.mapEvent; }
    /// <summary>
    /// EventPrefabの画像表示
    /// </summary>
    [SerializeField]
    private SpriteRenderer eventSprite;
    /// <summary>
    /// EventFinished時適応するEventSpriteのアルファ値
    /// </summary>
    [SerializeField]
    private float eventSpriteFinishedAlpha = 0.35f;

    [Header(" --- Looted Shrine")]
    /// <summary>
    /// LootBoxから出たアイテム
    /// </summary>
    private Shrine lootedShrine;
    public Shrine LootedShrine { get => this.lootedShrine; }
    #endregion


    #region [func]

    /// <summary>
    /// コンストラクタ
    /// </summary>
    private void Awake()
    {
        this.eventSprite.sprite = this.mapEvent.eventSprite_Start;
    }

    /// <summary>
    /// Event終了後、表示画像を終了Stateのものに変更
    /// </summary>
    public void SetSpriteToFinishedSprite()
    {
        // MapEventがExitDoorの場合は処理しない
        if (this.mapEvent.name.ToUpper() == "EXITDOOR")
            return;
        
        // 画像および色を変更
        this.eventSprite.sprite = mapEvent.eventSprite_Finished;
        var color = this.eventSprite.color;
        this.eventSprite.color = new Color(color.r, color.g, color.b, this.eventSpriteFinishedAlpha);
    }
    
    /// <summary>
    /// DoorKey取得後、ExitDoorの表示画像を変更
    /// </summary>
    public void SetExitDoorSpriteToFinishedSprite()
    {
        this.eventSprite.sprite = mapEvent.eventSprite_Change;
    }

    /// <summary>
    /// LootingしたShrineを保存
    /// </summary>
    /// <param name="shrine"></param>
    public void SetLootedShrine(Shrine shrine)
    {
        this.lootedShrine = shrine;
    }
    #endregion
}

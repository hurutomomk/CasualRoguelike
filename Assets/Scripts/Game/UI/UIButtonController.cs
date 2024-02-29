using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonController : MonoBehaviour
{
    #region [var]

    #region [01. 参照]
    [Header(" --- Reference")]
    
    #endregion

    #region [02. ボタン]

    [Header(" --- Buttons")]
    /// <summary>
    /// セッティングメニューボタン
    /// </summary>
    [SerializeField]
    private Button settingButton;
    /// <summary>
    /// 移動モード切り替えボタン
    /// </summary>
    [SerializeField]
    private Button movementModeToggleButton;
    /// <summary>
    /// プレイヤーの移動ボタン
    /// </summary>
    [SerializeField]
    private Button[] movementButtonsForPlayer;
    /// <summary>
    /// プレイヤーの移動ボタン:北
    /// </summary>
    [SerializeField]
    private Button northButton;
    public Button NorthButton { get => this.northButton; }
    /// <summary>
    /// プレイヤーの移動ボタン:東
    /// </summary>
    [SerializeField]
    private Button eastButton;
    public Button EastButton { get => this.eastButton; }
    /// <summary>
    /// プレイヤーの移動ボタン:南
    /// </summary>
    [SerializeField]
    private Button southButton;
    public Button SouthButton { get => this.southButton; }
    /// <summary>
    /// プレイヤーの移動ボタン:西
    /// </summary>
    [SerializeField]
    private Button westButton;
    public Button WestButton { get => this.westButton; }
    /// <summary>
    /// カメラの移動ボタン
    /// </summary>
    [SerializeField]
    private Button[] movementButtonsForCamera;
    #endregion

    #region [03. ボタンオブジェクト]
    [Header(" --- Button Objects")]
    /// <summary>
    /// プレイヤー移動ボタンのGameObject
    /// </summary>
    [SerializeField]
    private GameObject movementButtonObjForPlayer;
    [SerializeField]
    private GameObject movementButtonGroupTransformForPlayer;
    /// <summary>
    /// カメラ移動ボタンのGameObject
    /// </summary>
    [SerializeField]
    private GameObject movementButtonObjForCamera;
    [SerializeField]
    private GameObject movementButtonGroupTransformForCamera;
    #endregion
    
    #region [04. ボタンオブジェクトリスト]
    [Header(" --- Button Object List")]
    /// <summary>
    /// HUD表示切り替え対象のGameObjectリスト
    /// </summary>
    [SerializeField]
    private GameObject[] buttonObjsForHUDActivation;
    #endregion
    
    #region [05. Image]
    [Header(" --- Button Object Image")]
    /// <summary>
    /// 移動モード切り替えボタンのImage
    /// </summary>
    [SerializeField]
    private Image movementModeToggleButtonImage;
    
    [Header(" --- Button Image List")]
    /// <summary>
    /// 押下可否切り替え時変更対象ののボタンイメージリスト
    /// </summary>
    [SerializeField]
    private Image[] buttonImagesForDisable;
    #endregion

    #region [04. トリガー]
    /// <summary>
    /// 移動ボタン表示の切り替えトリガー
    /// </summary>
    private bool isButtonForCameraMovement = false;
    /// <summary>
    /// HUD表示の切り替えトリガー
    /// </summary>
    private bool isHUDActivationOff = false;
    /// <summary>
    /// StatusBox表示の切り替えトリガー
    /// </summary>
    private bool isStatusInfoShown = false;
    #endregion
    
    #region [05. コールバック]
    /// <summary>
    /// 移動終了コールバック
    /// </summary>
    private Action onCompleteMovement;
    #endregion

    #endregion
    
    
    
    #region [func]
    
    #region [01. コンストラクタ]
    
    

    
    #endregion

    #region [02.]
    
    /// <summary>
    /// 移動ボタン押下時の処理
    /// </summary>
    /// <param name="directionStr"></param>
    public void OnClickMoveButton(string directionStr)
    {
        
    }
    
    /// <summary>
    /// DisableButtonTouch
    /// </summary>
    public void DisableButtonTouch()
    {
        // 各種ボタンコンポネントをDisable
        this.settingButton.enabled = false;
        this.movementModeToggleButton.enabled = false;
        foreach (var button in this.movementButtonsForPlayer)　button.enabled = false;
        foreach (var button in this.movementButtonsForCamera)　button.enabled = false;

        // ボタンイメージ変更
        this.SetButtonImageForDisable();
    }
    
    /// <summary>
    /// EnableButtonTouch
    /// </summary>
    public void EnableButtonTouch()
    {
        // 各種ボタンコンポネントをEnable
        this.settingButton.enabled = true;
        this.movementModeToggleButton.enabled = true;
        foreach (var button in this.movementButtonsForPlayer)　button.enabled = true;
        foreach (var button in this.movementButtonsForCamera)　button.enabled = true;
        
        // ボタンイメージ変更
        this.SetButtonImageForEnable();
    }
    
    /// <summary>
    /// ボタンイメージ変更：Disable
    /// </summary>
    private void SetButtonImageForDisable()
    {
        
    }
    
    /// <summary>
    /// ボタンイメージ変更：Enable
    /// </summary>
    private void SetButtonImageForEnable()
    {
        
    }
    
    /// <summary>
    /// ボタンイメージを個別に変更：Enable
    /// </summary>
    public void SetEachMovementButtonEnableState(Button button, bool state)
    {
        button.enabled = state;
        
    }
    
    #endregion
    
    #endregion
}

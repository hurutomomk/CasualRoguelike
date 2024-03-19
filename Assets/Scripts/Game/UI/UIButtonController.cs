using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonController : MonoBehaviour
{
    #region [var]

    #region [01. 参照]
    [Header(" --- Reference")]
    /// <summary>
    /// PlayerMovementController
    /// </summary>
    [SerializeField]
    private PlayerMovementController playerMovementController;
    /// <summary>
    /// UIDialogController
    /// </summary>
    [SerializeField]
    private UIDialogController uIDialogController;
    /// <summary>
    /// BattleManager
    /// </summary>
    [SerializeField]
    private BattleManager battleManager;
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
    /// StatusInfoボタン
    /// </summary>
    [SerializeField]
    private Button statusInfoButton;
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
    /// <summary>
    /// LevelUpボタンのGameObject
    /// </summary>
    [SerializeField]
    private GameObject levelUpButtonObj;
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
    [SerializeField]
    private Image[] buttonImagesForDisableExceptMovementButton;
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

    #region [06. Pointerイメージ]
    /// <summary>
    /// カメラ移動時のポインター
    /// </summary>
    [SerializeField]
    private Image cameraPointer;
    
    #endregion
    
    #endregion
    
    
    
    
    #region [func]
    
    #region [01. コンストラクタ]
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public void SetUIButton(PlayerMovementController playerMovementControllerScript)
    {
        // スクリプトセット
        this.playerMovementController = playerMovementControllerScript;
        
        // 各種ボタンの初期化
        this.SetMovementButtonState(this.isButtonForCameraMovement);
    }
    #endregion

    #region [02. ボタンのステート管理]
    
    /// <summary>
    /// 移動ボタンの表示切り替え
    /// </summary>
    /// <param name="state"></param>
    public void SetMovementButtonState(bool state)
    {
        // カメラ移動ボタンの表示ステート変更
        this.movementButtonObjForCamera.SetActive(state);
        // 移動可能方向のみボタン選択可に変更
        this.EnableButtonTouchExceptMovementButton();
        
        // カメラポインター表示ステート変更
        this.SetCameraPointerAvtivation(state);
        
        // プレイヤー移動ボタンの表示ステート変更
        this.movementButtonObjForPlayer.SetActive(!state);
        this.movementButtonGroupTransformForPlayer.SetActive(!state);
        this.movementButtonGroupTransformForCamera.SetActive(state);
        
        // カメラ座標のリセット
        this.playerMovementController.ResetCameraPosition();
    }
    
    /// <summary>
    /// 移動ボタンのコンポネントEnableステート切り替え
    /// </summary>
    /// <param name="delay"></param>
    private void SetMovementButtonEnableState(float delay)
    {
        // 各種ボタンコンポネントをDisable
        // TODO :: ボタンImageの表示切り替え処理を追加
        this.settingButton.enabled = false;
        this.movementModeToggleButton.enabled = false;
        foreach (var button in this.movementButtonsForPlayer)　button.enabled = false;
        foreach (var button in this.movementButtonsForCamera)　button.enabled = false;
        
        // LevelUpボタンのDisable
        this.SetLevelUpButtonActiveState(false);

        // StatusInfoボタンをDisable
        this.SetStatusInfoButtonActiveState(false);
        
        // ボタンイメージ変更
        this.SetButtonImageForDisable();

        // 遅延処理
        DOVirtual.DelayedCall(delay, () =>
        {
            // 各種ボタンコンポネントをEnable
            // TODO :: ボタンImageの表示切り替え処理を追加
            this.settingButton.enabled = true;
            this.movementModeToggleButton.enabled = true;
            foreach (var button in this.movementButtonsForPlayer)　button.enabled = true;
            foreach (var button in this.movementButtonsForCamera)　button.enabled = true;

            // LevelUpができる状態か否かを判断してLevelUpボタンのActiveStateを切り替え
            this.CheckLevelUpButtonCanActiveOrNot();
            
            // ボタンイメージ変更
            this.SetButtonImageForEnable();
        });
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

        // LevelUpボタンのDisable
        this.SetLevelUpButtonActiveState(false);

        // StatusInfoボタンをDisable
        this.SetStatusInfoButtonActiveState(false);

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
        
        // LevelUpができる状態か否かを判断してLevelUpボタンのActiveStateを切り替え
        this.CheckLevelUpButtonCanActiveOrNot();

        // StatusInfoボタンをEnable
        this.SetStatusInfoButtonActiveState(true);
        
        // ボタンイメージ変更
        this.SetButtonImageForEnable();
        // 移動可能方向のみボタン選択可に変更
        this.EnableButtonTouchExceptMovementButton();
    }
    
    /// <summary>
    /// EnableButtonTouch
    /// </summary>
    public void EnableButtonTouchExceptMovementButton()
    {
        // 各種ボタンコンポネントをEnable
        this.settingButton.enabled = true;
        this.movementModeToggleButton.enabled = true;
        foreach (var button in this.movementButtonsForCamera)　button.enabled = true;
        
        // LevelUpボタンを有効化
        if (PlayerStatusManager.Instance.LevelUpButtonActiveState())
        {
            this.levelUpButtonObj.GetComponent<Button>().enabled = true;
            this.levelUpButtonObj.GetComponent<Image>().color = Color.white;
        }

        // StatusInfoボタンをEnable
        this.SetStatusInfoButtonActiveState(true);
        
        // ボタンイメージ変更
        foreach (var image in this.buttonImagesForDisableExceptMovementButton)
            image.GetComponent<UIButtonImageStateController>().SetEnabledSprite();
        
        // 到着したMapのMapInfoの読み込み
        PlayerMovementManager.Instance.GetMapInfo();
    }
    
    /// <summary>
    /// ボタンイメージ変更：Disable
    /// </summary>
    private void SetButtonImageForDisable()
    {
        foreach (var image in this.buttonImagesForDisable)
            image.GetComponent<UIButtonImageStateController>().SetDisabledSprite();
    }
    
    /// <summary>
    /// ボタンイメージ変更：Enable
    /// </summary>
    private void SetButtonImageForEnable()
    {
        foreach (var image in this.buttonImagesForDisable)
            image.GetComponent<UIButtonImageStateController>().SetEnabledSprite();
    }
    
    /// <summary>
    /// ボタンイメージを個別に変更：Enable
    /// </summary>
    public void SetEachMovementButtonEnableState(Button button, bool state)
    {
        button.enabled = state;
        if (state)
            button.GetComponent<UIButtonImageStateController>().SetEnabledSprite();
        else
            button.GetComponent<UIButtonImageStateController>().SetDisabledSprite();
    }
    
    /// <summary>
    /// LevelUpができる状態か否かを判断してLevelUpボタンのActiveStateを切り替え
    /// </summary>
    public void CheckLevelUpButtonCanActiveOrNot()
    {
        this.SetLevelUpButtonActiveState(PlayerStatusManager.Instance.LevelUpButtonActiveState());
    }
    
    /// <summary>
    ///  LevelUpボタンのStateを切り替え
    /// </summary>
    /// <param name="state"></param>
    public void SetLevelUpButtonActiveState(bool state)
    {
        this.levelUpButtonObj.GetComponent<Button>().enabled = state;
        
        if(state)
            this.levelUpButtonObj.GetComponent<Image>().color = Color.white;
        else
            this.levelUpButtonObj.GetComponent<Image>().color = Color.grey;
    }
    
    public void SetStatusInfoButtonActiveState(bool state)
    {
        this.statusInfoButton.GetComponent<Button>().enabled = state;
        
        if(state)
            this.statusInfoButton.GetComponent<Image>().color = Color.white;
        else
            this.statusInfoButton.GetComponent<Image>().color = Color.grey;
    }
    
    #endregion
    
    #region [03. ボタン押下処理]
    
    /// <summary>
    /// 移動ボタンの表示切り替えボタン押下時の処理
    /// </summary>
    public void OnClickChangeMovementButtonStateButton()
    {
        if (this.isButtonForCameraMovement)
        {
            // カメラオプション変更
            this.playerMovementController.SetCameraOptionOnPlayerMovementMode();
            
            // トリガーをセット
            this.isButtonForCameraMovement = false;
            
            // ボタンImageの表示切り替え
            // TODO :: アルファ時にSprite切り替えに変更
            this.movementModeToggleButtonImage.color = Color.white;
        }
        else
        {
            // カメラオプション変更
            this.playerMovementController.SetCameraOptionOnCameraMovementMode();
            
            // トリガーをセット
            this.isButtonForCameraMovement = true;
            
            // ボタンImageの表示切り替え
            // TODO :: アルファ時にSprite切り替えに変更
            this.movementModeToggleButtonImage.color = Color.green;
        }
        
        // 移動ボタンの表示切り替え
        this.SetMovementButtonState(this.isButtonForCameraMovement);
    }

    /// <summary>
    /// カメラ移動ボタン押下時の処理を中継
    /// </summary>
    /// <param name="directionStr"></param>
    public void OnClickCameraMovementButton(string directionStr)
    {
        // ボタン押下を一時的に無効化
        this.SetMovementButtonEnableState(this.playerMovementController.CameraMoveSpeed);
        // 実際の処理
        this.playerMovementController.OnClickCameraMovementButton(directionStr);
    }
    
    /// <summary>
    /// EventDialog終了ボタン押下時の処理
    /// </summary>
    public void OnClickEventDialogCloseButton(Transform eventDialog)
    {
        // EventDialog非表示
        this.uIDialogController.CloseEventDialog(eventDialog, () =>
        {
            // ボタンタッチ有効
            this.EnableButtonTouchExceptMovementButton();
        } );
    }
    
    /// <summary>
    /// PlayerTurn時のバトル終了ボタン押下時の処理
    /// </summary>
    public void OnClickBattleCloseButtonOnPlayerTurn(Transform battleDialog)
    {
        this.uIDialogController.CloseEventDialog();
        
        // Battle終了
        this.battleManager.EndBattle();
    }
    
    /// <summary>
    /// LevelUpボタン押下時の処理
    /// </summary>
    /// <param name="levelUpDialog"></param>
    public void OnClickLevelUpOpenButton(Transform levelUpDialog)
    {
        // Player Level Up
        PlayerStatusManager.Instance.LevelUp();
        
        // ボタンを無効に変更
        this.levelUpButtonObj.GetComponent<Button>().enabled = false;
        this.levelUpButtonObj.GetComponent<Image>().color = Color.grey;
        
        // BattleDialog非表示
        this.uIDialogController.ShowLevelUpDialog(levelUpDialog, () => { });
    }
    
    /// <summary>
    /// LevelUpDialogの終了ボタン押下時の処理
    /// </summary>
    /// <param name="levelUpDialog"></param>
    public void OnClickLevelUpCloseButton(Transform levelUpDialog)
    {
        // BattleDialog非表示
        this.uIDialogController.CloseLevelUpDialog(levelUpDialog, () => { });
    }
    
    /// <summary>
    /// StatusInfoDialogの表示ボタン押下時の処理
    /// </summary>
    /// <param name="statusInfoDialog"></param>
    public void OnClickStatusInfoDialogOpenButton(Transform statusInfoDialog)
    {
        // BattleDialog非表示
        this.uIDialogController.ShowStatusInfoDialog(statusInfoDialog, () => { });
    }
    
    /// <summary>
    /// StatusInfoDialogの終了ボタン押下時の処理
    /// </summary>
    /// <param name="statusInfoDialog"></param>
    public void OnClickStatusInfoDialogCloseButton(Transform statusInfoDialog)
    {
        // BattleDialog非表示
        this.uIDialogController.CloseStatusInfoDialog(statusInfoDialog, () => { });
    }
    #endregion
    
    #region [04. Pointer表示]
    /// <summary>
    /// カメラ移動時のポインター表示切り替え
    /// </summary>
    /// <param name="state"></param>
    public void SetCameraPointerAvtivation(bool state)
    {
        this.cameraPointer.enabled = state;
    }
    #endregion
    
    #endregion
}

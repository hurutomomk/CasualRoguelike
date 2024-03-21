using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LogoController : MonoBehaviour
{
    #region [var]

    #region [01. UI表示]
    [Header(" --- Common")]
    /// <summary>
    /// BackgroundのGameObject
    /// </summary>
    [SerializeField]
    private GameObject background;
    /// <summary>
    /// CurtainのGameObject
    /// </summary>
    [SerializeField]
    private GameObject curtain;
    
    [Header(" --- Logo Screen")]
    /// <summary>
    /// LogoScreenのGameObject
    /// </summary>
    [SerializeField]
    private GameObject logoScreen;
    
    #endregion
    
    #endregion
    
    
    
    #region [func]

    #region [01. UI表示]
    /// <summary>
    /// Logo表示
    /// </summary>
    public void ShowLogo(Action onFinished)
    {
        this.background.SetActive(true);
        this.curtain.SetActive(true);
        
        this.logoScreen.SetActive(true);

        DOVirtual.DelayedCall(1.5f, () =>
        {
            // TitleCurtainのフェードイン
            this.LogoCurtainOff(() =>
            {
                DOVirtual.DelayedCall(1f, () =>
                {
                    // TitleCurtainのフェードアウト
                    this.LogoCurtainOn(() =>
                    {
                        onFinished?.Invoke();
                    });
                });
            });
        });
    }

    /// <summary>
    /// Logo非表示
    /// </summary>
    public void OffLogo()
    {
        this.background.SetActive(false);
        this.curtain.SetActive(false);
        
        this.logoScreen.SetActive(false);
    }
    
    /// <summary>
    /// TitleCurtainのフェードイン
    /// </summary>
    /// <param name="onFinished"></param>
    private void LogoCurtainOff(Action onFinished)
    {
        this.curtain.GetComponent<Image>()
            .DOFade(0f, 2f)
            .OnComplete(() =>
            {
                this.curtain.SetActive(false);
                
                onFinished?.Invoke();
            });
    }
    
    /// <summary>
    /// TitleCurtainのフェードアウト
    /// </summary>
    /// <param name="onFinished"></param>
    private void LogoCurtainOn(Action onFinished)
    {
        this.curtain.SetActive(true);
        
        this.curtain.GetComponent<Image>()
            .DOFade(1f, 2f)
            .OnComplete(() =>
            {
                onFinished?.Invoke();
            });
    }
    
    #endregion
    
    #endregion
}

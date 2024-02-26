using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEventManager : MonoBehaviour
{
    
    
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

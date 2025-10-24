using System.Collections.Generic;
using UnityEngine;
using Lean.Gui;

public class WeaponIconsView : MonoBehaviour
{
    // 必要な数だけセットしておくこと
    [SerializeField] List<LeanToggle> weaponIcons;


    /// <summary>
    /// カウントを更新する
    /// </summary>
    /// <param name="maxCount">最大カウント</param>
    /// <param name="currentCount">現在のカウント</param>
    public void UpdateCount(int maxCount, int currentCount)
    {
        // maxCountの数だけActiveにする, それ以外はInactiveにする
        for (int i = 0; i < maxCount; i++)
        {
            weaponIcons[i].gameObject.SetActive(true);
        }
        for (int i = maxCount; i < weaponIcons.Count; i++)
        {
            weaponIcons[i].gameObject.SetActive(false);
        }

        // currentCountの数だけOn, それ以外はOffにする
        for (int i = 0; i < currentCount; i++)
        {
            weaponIcons[i].TurnOn();
        }
        for (int i = currentCount; i < maxCount; i++)
        {
            weaponIcons[i].TurnOff();
        }
    }
}

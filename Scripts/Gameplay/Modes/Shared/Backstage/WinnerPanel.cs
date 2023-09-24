using System.Collections.Generic;
using Core.Layout.PlayerControl;
using Core.PlayerControl;
using Kyzlyk.Core;
using TMPro;
using UnityEngine;

namespace Gameplay.Modes.Shared.Backstage
{
    internal sealed class WinnerPanel : MonoBehaviour
    {
        [Header("Variable Winner Info")]
        [SerializeField] private TMP_Text _scoreField;
        [SerializeField] private TMP_Text _startInGroupField;
        [SerializeField] private TMP_Text _winnerTypeField;
        
        public static WinnerPanel Create(WinnerPanel prefab, int groupIndex, int score, WinnerType type)
        {
            WinnerPanel panel = Instantiate(prefab);

            IReadOnlyList<Player> winnerGroup = Singleton<PlayerLayout>.Instance.Groups[groupIndex];

            panel._scoreField.text = score.ToString();
            panel._startInGroupField.text = winnerGroup.Count.ToString();
            panel._winnerTypeField.text = type.ToString();
            
            return panel;
        }

        public enum WinnerType
        {
            Win,
            Defeat
        }
    }
}
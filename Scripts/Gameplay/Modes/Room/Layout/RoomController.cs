using System.Linq;
using Core.Layout;
using Core.Layout.Interactables;
using Gameplay.Layout.Interactable;
using Kyzlyk.Core;
using UnityEngine;
using System.Collections.Generic;
using Core.Layout.PlayerControl;
using Gameplay.Modes.Shared.Backstage;
using Kyzlyk.Debugging.Annotations;

namespace Gameplay.Modes.Room.Layout
{
    [CreateAssetMenu(menuName = SharedConstants.Menu_LayoutControllers + "/Room")]
    public sealed class RoomController : Controller
    {
        [Header("Winning parameters")]
        [SerializeField] private int _scoreToWin;
        [SerializeField] private int _scoreForCapturePoint;
        
        [Header("Winning appearance")]
        [TempForTest][SerializeField] private WinnerPanel _winnerPanelPrefab; 
        
        private int[] _scoreGroups; 
        
        public override void Init()
        {
            _scoreGroups = new int[Singleton<PlayerLayout>.Instance.Groups.Count];
            
            IEnumerable<CapturePoint> capturePoints = Singleton<InteractablesLayout>.Instance
                .Interactables
                .Where(i => i is CapturePoint)
                .Cast<CapturePoint>();

            foreach (CapturePoint capturePoint in capturePoints)
            {
                capturePoint.OnPointCaptured += OnPointCaptured;
            }
        }

        private void OnPointCaptured(CapturePoint capturePoint, CapturePoint.PointOwnerEventArgs args)
        {
            _scoreGroups[capturePoint.OwnerGroupIndex] += _scoreForCapturePoint;
            
            if (CheckWinningGroup(out int groupIndex))
            {
                ThrowWin(groupIndex);
                Exit();
            }
        }

        [ContextMenu("Throw Win")]
        private void ThrowWinTest()
        {
            ThrowWin(Singleton<PlayerLayout>.Instance.MainGroupIndex);
        }
        
        private void ThrowWin(int groupIndex)
        {
            WinnerPanel winnerPanel = WinnerPanel.Create(_winnerPanelPrefab, groupIndex, _scoreGroups[groupIndex], WinnerPanel.WinnerType.Win);
        }
        
        private void ThrowDefeat(int groupIndex)
        {
            WinnerPanel winnerPanel = WinnerPanel.Create(_winnerPanelPrefab, groupIndex, _scoreGroups[groupIndex], WinnerPanel.WinnerType.Defeat);
        }

        private bool CheckWinningGroup(out int groupIndex)
        {
            groupIndex = -1;
            if (_scoreGroups.Length == 0)
                return false;
            
            int maxScoreInGroup = _scoreGroups[0];
            for (int i = 1; i < _scoreGroups.Length; i++)
            {
                if (_scoreGroups[i] > maxScoreInGroup)
                {
                    groupIndex = i;
                    maxScoreInGroup = _scoreGroups[i];
                }
            }

            return maxScoreInGroup >= _scoreToWin;
        }

        private void Exit()
        {
            
        }
        
        protected override void SuspendInternal()
        {
        }

        protected override void ContinueInternal()
        {
        }

        protected override void ReturnControlInternal()
        {
        }
    }
}
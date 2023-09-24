using UnityEngine;
using Core.PlayerControl;
using Kyzlyk.GSystems.InteractiveObjects;
using Kyzlyk.GSystems.InteractiveObjects.PowerUps;
using Kyzlyk.Helpers.Math;
using Core.Layout.Interface;
using Kyzlyk.Core;

namespace Gameplay.PowerUps
{
    public class PowerUp_TrajectoryAppearance : PowerUp<Thrower>, IAffectorController
    {
        [SerializeField] private int _parts;

        private LineRenderer _lineRenderer;

        private bool _paused = true;

        private HUD HUD => Singleton<HUD>.Instance;

        private void RenderTrajectory(int parts, UnitVector vector)
        {
            if (parts == 0)
            {
                _lineRenderer.positionCount = 0;
                return;
            }
            
            Vector3[] path = new Vector3[parts];

            Vector2 nextDirection = vector;
            path[0] = Target.Source.transform.position;

            for (int i = 1; i < parts; i++)
            {
                RaycastHit2D hit = Physics2D.Raycast(path[i - 1], nextDirection, 20, LayerMask.GetMask(SharedConstants.GMaterialLayer));

                //print(hit.normal);
                
                path[i] = hit.point;
                nextDirection = Vector2.Reflect(nextDirection, hit.normal);
            }

            _lineRenderer.positionCount = parts;
            _lineRenderer.SetPositions(path);
        }

        protected override void Pickup()
        {
            HUD.Joystick.OnDraged += Joystick_OnDraged;

            if (DirectTarget.TryGetComponent<LineRenderer>(out var lineRenderer))
            {
                _lineRenderer = lineRenderer;
                _lineRenderer.enabled = true;
            }
            else
            {
                _lineRenderer = Target.Source.AddComponent<LineRenderer>();
            }

            _lineRenderer.startWidth = 0.1f;
            _lineRenderer.endWidth = 0.1f;

            _paused = false;
        }

        private void Joystick_OnDraged(object sender, JoystickEventArgs args)
        {
            if (_paused) return;
            
            RenderTrajectory(_parts, -args.Direction);
        }

        protected override void ResetEffect()
        {
            HUD.Joystick.OnDraged -= Joystick_OnDraged;
            Destroy(gameObject);
        }

        public void Pause()
        {
            _paused = true;
            _lineRenderer.enabled = false;
        }

        public void Resume()
        {
            _paused = false;
            _lineRenderer.enabled = true;
        }
    }
}
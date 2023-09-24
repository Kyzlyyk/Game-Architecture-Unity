using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Core.FX.Animations
{
    public sealed class GAnimator : ScriptableObject
    {
        [SerializeField] private GAnimation[] _animations;

        private Queue<GAnimation> _animationsToPlay;
        private GAnimation _playingAnimation;

        private const int MaxAnimationsInQueue = 4;

        private const string ExceptionMessage = "Cannot play this animation!";

        private void Awake()
        {
            _animationsToPlay = new Queue<GAnimation>(MaxAnimationsInQueue);
        }

        public bool IsPlaying<T>()
            => _playingAnimation.GetType() == typeof(T);

        public bool TryPlay<T>(T animation, bool addToQueueIfNowPlayAnother) where T : GAnimation
        {
            if (_playingAnimation != null)
            {
                if (addToQueueIfNowPlayAnother)
                {
                    if (_animationsToPlay.Count >= MaxAnimationsInQueue)
                    {
                        Debug.LogError($"Cannot play '{typeof(T)}' animation! The max number of animations in the queue has been reached.");
                        return false;
                    }

                    _animationsToPlay.Enqueue(animation);
                }
                else
                {
                    Debug.LogError("Another animation is playing now!");
                    return false;
                }
            }
            else
            {
                _playingAnimation = animation;
                _playingAnimation.OnPlayed += Animation_OnPlayed;
                _playingAnimation.Play();
            }

            return true;
        }

        public void Play<T>(T animation, bool addToQueueIfNowPlayAnother) where T : GAnimation
        {
            if (!TryPlay(animation, addToQueueIfNowPlayAnother))
                throw new Exception(ExceptionMessage);
        }

        public bool TryPlay<T>(bool addToQueueIfNowPlayAnother, out T animation) where T : GAnimation
        {
            animation = GetAnimation<T>();
            return TryPlay(animation, addToQueueIfNowPlayAnother);
        }

        public T Play<T>(bool addToQueueIfNowPlayAnother) where T : GAnimation
        {
            T animation = GetAnimation<T>();
            if (TryPlay(animation, addToQueueIfNowPlayAnother))
                return animation;
            else
                throw new Exception(ExceptionMessage);
        }

        public T GetAnimation<T>() where T : GAnimation
            => _animations.FirstOrDefault(a => a.GetType() == typeof(T)) as T;

        private void Animation_OnPlayed(GAnimation sender, EventArgs e)
        {
            sender.OnPlayed -= Animation_OnPlayed;
            if (_animationsToPlay.TryDequeue(out GAnimation animation))
            {
                _playingAnimation = animation;
                _playingAnimation.OnPlayed += Animation_OnPlayed;
                _playingAnimation.Play();
            }
            else
                _playingAnimation = null;
            
        }
    }
}
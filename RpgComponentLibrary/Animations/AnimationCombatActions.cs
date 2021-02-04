using System;

namespace RpgComponentLibrary.Animations
{
    public class AnimationCombatActions
    {
        public event Action<string> OnSpriteChanged;
        public void TriggerAnimation(string spriteName)
        {
            OnSpriteChanged?.Invoke(spriteName);
        }
    }
}
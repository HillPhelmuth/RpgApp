using System;


namespace RpgComponentLibrary.Animations
{
    public class AnimationMoveActions
    {
        
        public event Action<double, double> OnPositionChanged;

        public void UpdatePosition(double x, double y)
        {
            OnPositionChanged?.Invoke(x, y);
        }
        
    }
}

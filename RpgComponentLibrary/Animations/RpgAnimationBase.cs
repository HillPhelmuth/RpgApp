using Microsoft.AspNetCore.Components;

namespace RpgComponentLibrary.Animations
{
    public abstract class RpgAnimationBase : ComponentBase
    {
        [Parameter]
        public AnimationModel Animation { get; set; }

        [Parameter]
        public CanvasSpecs CanvasSpecs { get; set; }
        
        [Parameter] 
        public int Fps { get; set; } = 7;
        [Parameter]
        public CanvasBackgound CanvasBackgound { get; set; }
    }
}

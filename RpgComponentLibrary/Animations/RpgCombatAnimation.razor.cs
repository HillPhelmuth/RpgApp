using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Excubo.Blazor.Canvas;
using Excubo.Blazor.Canvas.Contexts;
using Microsoft.AspNetCore.Components;

namespace RpgComponentLibrary.Animations
{
    public partial class RpgCombatAnimation : IAsyncDisposable
    {
        private Canvas _canvas;
        
        private Context2D _context2D;
        
        private Timer _timer;
        private int FrameInterval => 1000 / Fps;
        [Parameter] 
        public AnimationCombatActions CombatActions { get; set; } = new();
        protected override async Task OnParametersSetAsync()
        {
            Animation ??= new AnimationModel { Scale = 3 };
            CanvasSpecs ??= new CanvasSpecs(400, 400);
            Animation.Sprites ??= SpriteSets.CombatSprites;
            Animation.CurrentSprite ??= Animation.Sprites["Idle"];
            
            await base.OnParametersSetAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                try
                {
                    CombatActions.OnSpriteChanged += Animate;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
              
                _timer = new Timer(FrameInterval);
                _timer.Stop();
                _context2D = await _canvas.GetContext2DAsync();
                _timer.Elapsed += HandleAnimationLoop;
                _timer.Start();
            }
        }

        private void Animate(string animation)
        {
            Animation.CurrentSprite = Animation.Sprites[animation];
            Animation.Index = 0;
        }

        private async Task AnimationLoop()
        {
            await AnimateNext();
        }
        
        private async Task AnimateNext(string idleSprites = "Idle")
        {
            await _context2D.ClearRectAsync(0, 0, CanvasSpecs.W, CanvasSpecs.H);
            Animation.Index++;
            var frames = Animation.CurrentSprite.Frames;
            if (Animation.Index >= frames.Count)
            {
                Animation.Index = 0;
                Animation.CurrentSprite = Animation.Sprites[idleSprites];
            }

            var imageName = Animation.CurrentSprite?.Name ?? idleSprites;
            var frame = frames[Animation.Index];
            try
            {
                await _context2D.DrawImageAsync(imageName, frame.X, frame.Y, frame.W, frame.H, 0,
                    CanvasSpecs.H - frame.H * Animation.Scale, frame.W * Animation.Scale, frame.H * Animation.Scale);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _timer.Stop();
                throw;
            }
            
        }


        private async void HandleAnimationLoop(object _, ElapsedEventArgs e)
        {
            await AnimationLoop();
            
        }
        public ValueTask DisposeAsync()
        {
            CombatActions.OnSpriteChanged -= Animate;
            _timer.Elapsed -= HandleAnimationLoop;
            _timer.Stop();
            _timer.Dispose();
            return ValueTask.CompletedTask;
        }

       
    }
}

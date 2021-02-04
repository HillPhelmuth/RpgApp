using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using System.Timers;
using Excubo.Blazor.Canvas;
using Excubo.Blazor.Canvas.Contexts;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace RpgComponentLibrary.Animations
{
    public partial class CanvasCSharp : IAsyncDisposable
    {
        // ToDo Turn this into reusable component for RPG animation
        [Inject]
        private IJSRuntime Js { get; set; }

        private IJSObjectReference module;
        private Canvas canvas;
        private Context2D context;
        private Timer timer;
        private int TimerInterval => 1000 / Fps;
        
        private readonly List<string> Logs = new();
        [Parameter]
        public AnimateType AnimateType { get; set; }
        [Parameter]
        public AnimationModel Anim { get; set; } = new() { Scale = 3, MoveSpeed = 4 };
        [Parameter]
        public CanvasSpecs CanvasSpecs { get; set; } = new(600, 800);
        [Parameter]
        public int Fps { get; set; } = 10;

        //[Parameter] 
        //public Dictionary<string, SpriteDataModel> SpriteSet { get; set; } = SpriteSets.OverheadSprites;

        //protected override Task OnInitializedAsync()
        //{
        //    Anim.AllSprites = SpriteSet;
        //    Anim.CurrentSprite = Anim.AllSprites["Right"];
        //    return base.OnInitializedAsync();
        //}

        protected override Task OnParametersSetAsync()
        {
            Anim.Sprites ??= AnimateType == AnimateType.Combat ? SpriteSets.CombatSprites : SpriteSets.OverheadSprites;
            Anim.CurrentSprite ??= AnimateType == AnimateType.Combat ? Anim.Sprites["Idle"] : Anim.Sprites["Right"];
            return base.OnParametersSetAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                
                context = await canvas.GetContext2DAsync();
                timer = new Timer(TimerInterval);
                if (AnimateType == AnimateType.Combat)
                {
                    AnimateCombat();
                }
                else
                {
                    InitOverhead();
                }
                
                timer.Elapsed += HandleAnimationLoop;
                module = await Js.InvokeAsync<IJSObjectReference>("import",
                    "./_content/RpgComponentLibrary/animateInterop.js");
                await module.InvokeVoidAsync("setEventListeners", DotNetObjectReference.Create(this));
                await module.InvokeVoidAsync("ping");
                timer.Start();
            }
        }
        public void AnimateCombat(string animation = "Idle")
        {
            Anim.CurrentSprite = Anim.Sprites[animation];
            Anim.Index = 0;
           
        }
        public void InitOverhead()
        {
            Anim.CurrentSprite = Anim.Sprites["Right"];
        }
        private void Reset()
        {
            Anim.Reset();
            StateHasChanged();
        }
        [JSInvokable]
        public void HandleKeyDown(string key) => Anim.KeyPresses[key] = true;

        [JSInvokable]
        public void HandleKeyUp(string key) => Anim.KeyPresses[key] = false;

        private void AddToLog(string logMessage)
        {
            Logs.Add(logMessage);
            if (Logs.Count > 100)
                Logs.RemoveAt(0);

        }

        private async Task AnimationLoop()
        {
            await context.ClearRectAsync(0, 0, CanvasSpecs.W, CanvasSpecs.H);
            await MoveActions();
        }

        private void Move(double deltaX, double deltaY)
        {
            if (Anim.PosX + deltaX > 0 && Anim.PosX + Anim.FrameWidth() * Anim.MoveSpeed + deltaX < CanvasSpecs.W)
            {
                Anim.PosX += deltaX;
            }
            if (Anim.PosY + deltaY > 0 && Anim.PosY + Anim.FrameHeight() * Anim.MoveSpeed + deltaY < CanvasSpecs.H)
            {
                Anim.PosY += deltaY;
            }
        }
        private async void HandleAnimationLoop(object _, ElapsedEventArgs e)
        {
            if (AnimateType == AnimateType.Combat)
            {
                await CombatActions();
            }
            else
            {
                await AnimationLoop();
            }
            
        }

        #region Actions
        private async Task CombatActions(string idleSprites = "Idle")
        {
            await context.ClearRectAsync(0, 0, CanvasSpecs.W, CanvasSpecs.H);
            Anim.Index++;
            var frames = Anim.CurrentSprite.Frames;
            if (Anim.Index >= frames.Count)
            {
                Anim.Index = 0;
                Anim.CurrentSprite = Anim.Sprites[idleSprites];
            }

            var imageName = Anim.CurrentSprite?.Name ?? idleSprites;
            var frame = frames[Anim.Index];

            await context.DrawImageAsync(imageName, frame.X, frame.Y, frame.W, frame.H, 0,
                CanvasSpecs.H - frame.H * Anim.Scale, frame.W * Anim.Scale, frame.H * Anim.Scale);
        }
        private string imageString = "Right";
        private async Task MoveActions()
        {
            var hasMoved = false;
            if (Anim.KeyPresses["w"])
            {
                Anim.CurrentSprite = Anim.Sprites["Up"];
                imageString = "Up";
                Move(0, -Anim.MoveSpeed);
                hasMoved = true;
            }
            else if (Anim.KeyPresses["s"])
            {
                Anim.CurrentSprite = Anim.Sprites["Down"];
                imageString = "Down";
                Move(0, Anim.MoveSpeed);
                hasMoved = true;
            }

            if (Anim.KeyPresses["a"])
            {
                imageString = "Left";
                Anim.CurrentSprite = Anim.Sprites["Left"];
                Move(-Anim.MoveSpeed, 0);
                hasMoved = true;
            }
            else if (Anim.KeyPresses["d"])
            {
                imageString = "Right";
                Anim.CurrentSprite = Anim.Sprites["Right"];
                Move(Anim.MoveSpeed, 0);
                hasMoved = true;
            }

            if (!hasMoved)
                Anim.Index = 1;
            await InvokeAsync(StateHasChanged);

            Anim.Index++;
            var frames = Anim.CurrentSprite.Frames;
            if (Anim.Index >= frames.Count)
                Anim.Index = 0;


            var frame = Anim.CurrentSprite.Frames[Anim.Index];
            var logMessage = $"sprite: {Anim.CurrentSprite.Name}\r\nframe specs: {JsonSerializer.Serialize(frame)}";
            AddToLog(logMessage);
            await context.DrawImageAsync($"overhead{imageString}", frame.X, frame.Y, frame.W, frame.H, Anim.PosX, Anim.PosY, frame.W * Anim.Scale, frame.H * Anim.Scale);
        }
        #endregion
        public async ValueTask DisposeAsync()
        {
            timer.Elapsed -= HandleAnimationLoop;
            timer.Stop();
            timer.Dispose();
            await module.InvokeVoidAsync("dispose");
            await module.DisposeAsync();
        }
    }
}

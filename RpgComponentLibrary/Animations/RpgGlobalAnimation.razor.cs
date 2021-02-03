using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Timers;
using Excubo.Blazor.Canvas;
using Excubo.Blazor.Canvas.Contexts;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;


namespace RpgComponentLibrary.Animations
{
    public partial class RpgGlobalAnimation
    {
        [Inject]
        private IJSRuntime Js { get; set; }

        private IJSObjectReference module;
        private Canvas canvas;
        private Context2D context;
        private Timer timer;
        private int TimerInterval => 1000 / Fps;
        [Parameter]
        public AnimationMoveActions MoveActions { get; set; }
        private readonly List<string> Logs = new();
        

        protected override Task OnInitializedAsync()
        {
            Animation = new() { Scale = 3, MoveSpeed = 4 };
            CanvasSpecs = new CanvasSpecs(600, 800);
            Fps = 10;
            return base.OnInitializedAsync();
        }

        protected override Task OnParametersSetAsync()
        {
            Animation.Sprites ??= SpriteSets.OverheadSprites;
            Animation.CurrentSprite ??= Animation.Sprites["Right"];
            return base.OnParametersSetAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {

                context = await canvas.GetContext2DAsync();
                timer = new Timer(TimerInterval);
                InitOverhead();
                timer.Elapsed += HandleAnimationLoop;
                module = await Js.InvokeAsync<IJSObjectReference>("import",
                    "./_content/RpgComponentLibrary/animateInterop.js");
                await module.InvokeVoidAsync("setEventListeners", DotNetObjectReference.Create(this));
                await module.InvokeVoidAsync("ping");
            }
        }
       
        public void InitOverhead()
        {
            Animation.CurrentSprite = Animation.Sprites["Right"];
            timer.Start();
        }
        private void Reset()
        {
            Animation.Reset();
            StateHasChanged();
        }
        [JSInvokable]
        public void HandleKeyDown(string key) => Animation.KeyPresses[key] = true;

        [JSInvokable]
        public void HandleKeyUp(string key) => Animation.KeyPresses[key] = false;

        private void AddToLog(string logMessage)
        {
            Logs.Add(logMessage);
            if (Logs.Count > 100)
                Logs.RemoveAt(0);

        }

        private async Task AnimationLoop()
        {
            await context.ClearRectAsync(0, 0, CanvasSpecs.W, CanvasSpecs.H);
            await HandleMoves();
        }

        private void Move(double deltaX, double deltaY)
        {
            if (Animation.PosX + deltaX > 0 && Animation.PosX + Animation.FrameWidth() * Animation.MoveSpeed + deltaX < CanvasSpecs.W)
            {
                Animation.PosX += deltaX;
            }
            if (Animation.PosY + deltaY > 0 && Animation.PosY + Animation.FrameHeight() * Animation.MoveSpeed + deltaY < CanvasSpecs.H)
            {
                Animation.PosY += deltaY;
            }
        }
        private async void HandleAnimationLoop(object _, ElapsedEventArgs e)
        {
            await AnimationLoop();
        }

        #region Actions

        private string imageString = "Right";
        private async Task HandleMoves()
        {
            var hasMoved = false;
            if (Animation.KeyPresses["w"])
            {
                Animation.CurrentSprite = Animation.Sprites["Up"];
                imageString = "Up";
                Move(0, -Animation.MoveSpeed);
                hasMoved = true;
            }
            else if (Animation.KeyPresses["s"])
            {
                Animation.CurrentSprite = Animation.Sprites["Down"];
                imageString = "Down";
                Move(0, Animation.MoveSpeed);
                hasMoved = true;
            }

            if (Animation.KeyPresses["a"])
            {
                imageString = "Left";
                Animation.CurrentSprite = Animation.Sprites["Left"];
                Move(-Animation.MoveSpeed, 0);
                hasMoved = true;
            }
            else if (Animation.KeyPresses["d"])
            {
                imageString = "Right";
                Animation.CurrentSprite = Animation.Sprites["Right"];
                Move(Animation.MoveSpeed, 0);
                hasMoved = true;
            }

            if (!hasMoved)
                Animation.Index = 1;
            await InvokeAsync(StateHasChanged);
            MoveActions.UpdatePosition(Animation.PosX, Animation.PosY);
            Animation.Index++;
            var frames = Animation.CurrentSprite.Frames;
            if (Animation.Index >= frames.Count)
                Animation.Index = 0;


            var frame = Animation.CurrentSprite.Frames[Animation.Index];
            var logMessage = $"sprite: {Animation.CurrentSprite.Name}\r\nframe specs: {JsonSerializer.Serialize(frame)}";
            AddToLog(logMessage);
            await context.DrawImageAsync($"overhead{imageString}", frame.X, frame.Y, frame.W, frame.H, Animation.PosX, Animation.PosY, frame.W * Animation.Scale, frame.H * Animation.Scale);
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

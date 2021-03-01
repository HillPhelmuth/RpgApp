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
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;


namespace RpgComponentLibrary.Animations
{
    public partial class RpgGlobalAnimation
    {
        [Inject]
        private IJSRuntime Js { get; set; }

        private IJSObjectReference _module;
        private Canvas _canvas;
        private Context2D _context2D;
        private Timer _timer;
        private bool _areImagesLoaded;
        private int TimerInterval => 1000 / Fps;
        private Func<CollisionBlock, bool> IsCollideFunc => blk => Animation.PosX + (Animation.FrameWidth() * Animation.Scale) > blk.X && Animation.PosX < blk.X + blk.W && Animation.PosY + (Animation.FrameHeight() * Animation.Scale) > blk.Y && Animation.PosY < blk.Y + blk.H;
        private readonly Dictionary<string, int> _collisionsPerBlock = new();
        [Parameter]
        public List<CollisionBlock> CollisionBlocks { get; init; } = new();
        [Parameter]
        public string BackgroundImageId { get; set; }
        [Parameter]
        public EventCallback<ValueTuple<double, double>> OnMove { get; set; }
        [Parameter]
        public EventCallback<string> OnCollide { get; set; }
        [Parameter]
        public KeyValuePair<string, string> BackgroundImage { get; set; }
        [Parameter]
        public bool StopTimer { get; set; }
       

        protected override Task OnParametersSetAsync()
        {
            Animation ??= new() { Scale = 3, MoveSpeed = 4 };
            Animation.Sprites ??= SpriteSets.BoyleSprites;
            Animation.CurrentSprite ??= Animation.Sprites["Right"];
            CanvasSpecs ??= new CanvasSpecs(600, 800);
            Fps = Fps == 7 ? 10 : Fps;
            foreach (var block in CollisionBlocks.Where(block => !_collisionsPerBlock.ContainsKey(block.Name)))
            {
                _collisionsPerBlock[block.Name] = 0;
            }
            return base.OnParametersSetAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                foreach (var block in CollisionBlocks.Where(block => !_collisionsPerBlock.ContainsKey(block.Name)))
                {
                    _collisionsPerBlock[block.Name] = 0;
                }
                _context2D = await _canvas.GetContext2DAsync();
                _timer = new Timer(TimerInterval);
                InitOverhead();
                _timer.Elapsed += HandleAnimationLoop;
                _module = await Js.InvokeAsync<IJSObjectReference>("import",
                    "./_content/RpgComponentLibrary/animateInterop.js");
                await _module.InvokeVoidAsync("setEventListeners", DotNetObjectReference.Create(this));
                await _module.InvokeVoidAsync("ping");
                await ResetCanvas();

            }
        }

        public void InitOverhead()
        {
            Animation.CurrentSprite ??= Animation.Sprites["Right"];
            //_timer.Start();
        }
        private void Reset()
        {
            Animation.Reset();
            StateHasChanged();
        }
        [JSInvokable]
        public void HandleKeyDown(string key)
        {
            Animation.KeyPresses[key] = true;
            if (!_timer.Enabled) _timer.Start();
        }

        [JSInvokable]
        public void HandleKeyUp(string key) => Animation.KeyPresses[key] = false;

        private async Task AnimationLoop()
        {
            await ResetCanvas();
            await HandleMoves();
        }

        private async Task ResetCanvas()
        {
            try
            {
                await _context2D.ClearRectAsync(0, 0, CanvasSpecs.W, CanvasSpecs.H);
                await _context2D.DrawImageAsync(BackgroundImage.Key, 0, 0, CanvasSpecs.W, CanvasSpecs.H);
                foreach (var frame in CollisionBlocks)
                {
                    await _context2D.DrawImageAsync(frame.Name, 0, 0, frame.W, frame.H, frame.X, frame.Y, frame.W,
                        frame.H);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex}");
                _timer.Stop();
            }
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
            if (StopTimer)
            {
                _timer.Stop();
                return;
            }
            await AnimationLoop();
        }

        private int imagesLoaded = 0;
        private void OnImageLoad(ProgressEventArgs args)
        {
            imagesLoaded++;
            Console.WriteLine($"Images Loaded:{imagesLoaded}");
            if (!_timer.Enabled) _timer.Start();

        }
        #region Actions

        private string imageString = "Right";
        private async Task HandleMoves()
        {
            var hasMoved = false;
            if (Animation.KeyPresses["w"])
            {
                Animation.CurrentSprite = Animation.Sprites["Up"];
                Move(0, -Animation.MoveSpeed);
                hasMoved = true;
            }
            else if (Animation.KeyPresses["s"])
            {
                Animation.CurrentSprite = Animation.Sprites["Down"];
                Move(0, Animation.MoveSpeed);
                hasMoved = true;
            }

            if (Animation.KeyPresses["a"])
            {
                Animation.CurrentSprite = Animation.Sprites["Left"];
                Move(-Animation.MoveSpeed, 0);
                hasMoved = true;
            }
            else if (Animation.KeyPresses["d"])
            {
                Animation.CurrentSprite = Animation.Sprites["Right"];
                Move(Animation.MoveSpeed, 0);
                hasMoved = true;
            }

            imageString = Animation.CurrentSprite.Name;
            if (!hasMoved)
                Animation.Index = 1;
            await InvokeAsync(StateHasChanged);
            if (hasMoved)
            {
                await OnMove.InvokeAsync((Animation.PosX, Animation.PosY));
                
                if (CollisionBlocks.Any(IsCollideFunc))
                {
                    var collide = CollisionBlocks.FirstOrDefault(IsCollideFunc);
                    _collisionsPerBlock[collide.Name]++;
                    if (_collisionsPerBlock[collide.Name] > collide.MaxCollisions)
                    {
                        _timer.Stop();
                        Console.WriteLine($"collided with {collide.Name}");
                        await OnCollide.InvokeAsync(collide.Name);
                    }
                    
                }
            }

            Console.WriteLine($"Moved to X = {Animation.PosX}, Y = {Animation.PosY}");
            Animation.Index++;
            var frames = Animation.CurrentSprite.Frames;
            if (Animation.Index >= frames.Count)
                Animation.Index = 0;


            var frame = Animation.CurrentSprite.Frames[Animation.Index];
            var logMessage = $"sprite: {Animation.CurrentSprite.Name}\r\nframe specs: {JsonSerializer.Serialize(frame)}";

            try
            {
                await _context2D.DrawImageAsync(imageString, frame.X, frame.Y, frame.W, frame.H, Animation.PosX, Animation.PosY, frame.W * Animation.Scale, frame.H * Animation.Scale);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex}");
                _timer.Stop();
            }
        }

        #endregion
        public async ValueTask DisposeAsync()
        {
            _timer.Elapsed -= HandleAnimationLoop;
            _timer.Stop();
            _timer.Dispose();
            await _module.InvokeVoidAsync("dispose");
            await _module.DisposeAsync();
        }
    }

}

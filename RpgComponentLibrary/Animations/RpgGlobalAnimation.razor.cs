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
        private Func<CollisionBlock, bool> CollidePredicate => blk => Animation.PosX + (Animation.FrameWidth() * Animation.Scale) > blk.X && Animation.PosX < blk.X + blk.W && Animation.PosY + (Animation.FrameHeight() * Animation.Scale) > blk.Y && Animation.PosY < blk.Y + blk.H;
        [Parameter]
        public AnimationMoveActions MoveActions { get; set; }
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
        //private readonly List<string> Logs = new();



        protected override Task OnParametersSetAsync()
        {
            Animation ??= new() { Scale = 3, MoveSpeed = 4 };
            Animation.Sprites ??= SpriteSets.OverheadSprites;
            Animation.CurrentSprite ??= Animation.Sprites["Right"];
            CanvasSpecs ??= new CanvasSpecs(600, 800);
            Fps = Fps == 7 ? 10 : Fps;
            return base.OnParametersSetAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {

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
            await AnimationLoop();
        }

        private int imagesLoaded = 0;
        private void OnImageLoad(ProgressEventArgs args)
        {
            Console.WriteLine($"Type: {args.Type}, Computable: {args.LengthComputable}, Progress: {args.Loaded}/{args.Total}");
            var done = args.LengthComputable && args.Total == args.Loaded;
            if (done) imagesLoaded++;
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
            if (hasMoved)
            {
                await OnMove.InvokeAsync((Animation.PosX, Animation.PosY));
                
                if (CollisionBlocks.Any(CollidePredicate))
                {
                    _timer.Stop();
                    var collide = CollisionBlocks.FirstOrDefault(CollidePredicate);
                    Console.WriteLine($"collided {collide.Name}");
                    await OnCollide.InvokeAsync(collide.Name);
                }
            }

            Console.WriteLine($"Moved to X = {Animation.PosX}, Y = {Animation.PosY}");
            Animation.Index++;
            var frames = Animation.CurrentSprite.Frames;
            if (Animation.Index >= frames.Count)
                Animation.Index = 0;


            var frame = Animation.CurrentSprite.Frames[Animation.Index];
            var logMessage = $"sprite: {Animation.CurrentSprite.Name}\r\nframe specs: {JsonSerializer.Serialize(frame)}";

            await _context2D.DrawImageAsync($"overhead{imageString}", frame.X, frame.Y, frame.W, frame.H, Animation.PosX, Animation.PosY, frame.W * Animation.Scale, frame.H * Animation.Scale);
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

using System;
using System.Collections.Generic;

namespace RpgComponentLibrary.Animations
{
    public class AnimationModel
    {
        public AnimationModel()
        {

        }

        public AnimationModel(Dictionary<string, SpriteDataModel> sprites, int scale = 2, int moveSpeed = 3)
        {
            Sprites = sprites;
            Scale = scale;
            MoveSpeed = moveSpeed;
        }
        public int Index { get; set; }
        public double PosX { get; set; }
        public double PosY { get; set; }
        public int Scale { get; init; } = 2;
        public int MoveSpeed { get; init; } = 3;
        public SpriteDataModel CurrentSprite { get; set; } = new();
        public Dictionary<string, SpriteDataModel> Sprites { get; set; } = new();
        public Dictionary<string, bool> KeyPresses { get; init; } = new()
        {
            { "a", false },
            { "d", false },
            { "w", false },
            { "s", false }
        };
        public Dictionary<string, Action<string>> KeyPressHandlers { get; set; }
        public double FrameWidth() => CurrentSprite?.Frames[Index]?.W ?? 0;
        public double FrameHeight() => CurrentSprite?.Frames[Index]?.H ?? 0;

        public void Reset()
        {
            PosX = 0.0;
            PosY = 0.0;
            Index = 0;
            CurrentSprite = new SpriteDataModel();
        }
    }

    public record CanvasSpecs
    {
        public CanvasSpecs(int height, int width)
        {
            H = height;
            W = width;
        }
        public int H { get; set; }
        public int W { get; set; }
    }

    public enum AnimateType
    {
        Combat,
        OverHead
    }

}

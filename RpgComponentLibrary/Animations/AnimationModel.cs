using System;
using System.Collections.Generic;
using System.Linq;

namespace RpgComponentLibrary.Animations
{
    public class AnimationModel
    {
        public AnimationModel()
        {

        }

        public AnimationModel(Dictionary<string, SpriteDataModel> sprites, string currentSpriteName, int scale = 2, int moveSpeed = 3)
        {
            Sprites = sprites;
            Scale = scale;
            MoveSpeed = moveSpeed;
            CurrentSprite = sprites.Values.FirstOrDefault(x => x.Name == currentSpriteName);
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

    public record CollisionBlock
    {
        public CollisionBlock(string name, string imgSrcUrl, int height = 1, int width = 1, int xPosition = 0,
            int yPosition = 0, int maxCollitions = 1)
        {
            H = height;
            W = width;
            X = xPosition;
            Y = yPosition;
            Name = name;
            ImageUrl = imgSrcUrl;
            MaxCollisions = maxCollitions;
        }
        public string Name { get; set; }
        public int H { get; set; }
        public int W { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string ImageUrl { get; set; }
        public int MaxCollisions { get; set; }

        public override string ToString()
        {
            return $"\r\nBlock {Name}\r\nSize: {H}x{W} Location: {X},{Y}\r\nImageUrl: {ImageUrl}";
        }
    }

}

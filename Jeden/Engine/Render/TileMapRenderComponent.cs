using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;
using SFML.Window;
using Jeden.Engine.Object;

namespace Jeden.Engine.Render
{
    public class TileMapRenderComponent : RenderComponent
    {
        public TileMapRenderComponent(RenderManager renderMgr, GameObject parent) : base(renderMgr, parent)
        {

        }

        public void Set(int mapWidth, int mapHeight, float tileWidth, float tileHeight, Tile[,] tiles)
        {
            MapWidth = mapWidth;
            MapHeight = mapHeight;
            TileWidth = tileWidth;
            TileHeight = tileHeight;
            Tiles = tiles;

            WorldWidth = MapWidth * TileWidth;
            WorldHeight = MapHeight * TileHeight;


        }

        public override FloatRect GetScreenRect(Camera camera)
        {
            FloatRect rect;
            rect.Top = 0;
            rect.Left = 0;
            rect.Width = WorldWidth;
            rect.Height = WorldHeight;
            return rect;
        }

        public int MapWidth { get; private set; }
        public int MapHeight { get; private set; }
        public float TileWidth { get; private set; }
        public float TileHeight { get; private set; }

        public struct Tile
        {
            public Texture Texture;
            public IntRect SubImageRect;
        }

        Tile[,] Tiles;


        public override void Draw(RenderManager renderMgr, Camera camera)
        {

            int xStart = (int)((camera.Center.X - camera.Size.X * 0.5f) / TileWidth);
            int xEnd = (int)(1 + (camera.Center.X + camera.Size.X * 0.5f) / TileWidth);

            var yStart = (int)((camera.Center.Y - camera.Size.Y * 0.5f) / TileHeight);
            var yEnd = (int)(1 + (camera.Center.Y + camera.Size.Y * 0.5f) / TileHeight); ;

            if (xStart < 0)
                xStart = 0;
            if (xEnd > MapWidth - 1)
                xEnd = MapWidth - 1;
            if (yStart < 0)
                yStart = 0;
            if (yEnd > MapHeight - 1)
                yEnd = MapHeight - 1;

            for (var i = xStart; i < xEnd; i++)
            {
                for (var j = yStart; j < yEnd; j++)
                {
                    if (Tiles[i, j].Texture == null)
                        continue;

                    var position = new Vector2f(
                                    TileWidth * i + TileWidth * 0.5f,
                                    TileHeight * j + TileHeight * 0.5f);

                    renderMgr.DrawSprite(Tiles[i, j].Texture, Tiles[i, j].SubImageRect, position + WorldPosition, TileWidth, TileHeight, false, false, Tint, ZIndex);

                }
            }
        }
    }
}

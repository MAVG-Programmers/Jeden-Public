using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;
using SFML.Window;
using Jeden.Engine.Render;
using TiledSharp;
using System.Diagnostics;
using System.IO;

namespace Jeden.Engine.TileMap
{
    class TileMap 
    {
        public int MapWidth { get; private set; }
        public int MapHeight { get; private set; }
        public float TileWidth { get; private set; }
        public float TileHeight { get; private set; }


        TmxMap map;

        Dictionary<TmxTileset, Texture> spriteSheets;
        Dictionary<int, IntRect> subImageRects;
        Dictionary<int, TmxTileset> idSheet;
        List<int[,]> layerID;     // for future use, 1 layer only for now

        float Scale;

        public struct ParallaxSprite
        {
            public Vector2f Position;
            public float Width;
            public float Height;
            public Texture Texture;
            public IntRect SubImageRect;
            public float ParallaxFactor;
            public int ZIndex;
        }

        public struct PhysicsObject
        {
            public Vector2f Position;
            public float Width;
            public float Height;
        }

        public List<ParallaxSprite> ParallaxSprites;
        TileMapRenderComponent.Tile[,] RenderTiles;
        public List<PhysicsObject> PhysicsObjects;

        public TileMap(string mapName, float scale) 
        {
            Scale = scale;

            map = new TmxMap(mapName);
            MapWidth = map.Width;
            MapHeight = map.Height;
            TileWidth = map.TileWidth;
            TileHeight = map.TileHeight;
            RenderTiles = new TileMapRenderComponent.Tile[MapWidth, MapHeight];
            PhysicsObjects = new List<PhysicsObject>();
            ParallaxSprites = new List<ParallaxSprite>();

            // Load spritesheets
            spriteSheets = new Dictionary<TmxTileset, Texture>();
            subImageRects = new Dictionary<int, IntRect>();
            idSheet = new Dictionary<int, TmxTileset>();

            foreach (TmxTileset ts in map.Tilesets)
            {
                String filename = Path.GetFileName(ts.Image.Source);

                //Tiled stores the full path name of the tileset
                var newSheet = new Texture("assets/" + filename);
                spriteSheets.Add(ts, newSheet);

                // Loop hoisting
                var wStart = ts.Margin;
                var wInc = ts.TileWidth + ts.Spacing;
                var wEnd = ts.Image.Width / (ts.TileWidth + ts.Spacing); // don't want an ID for a partial tile

                var hStart = ts.Margin;
                var hInc = ts.TileHeight + ts.Spacing;
                var hEnd = ts.Image.Height / (ts.TileHeight + ts.Spacing);

                // Pre-compute sub image rectangles
                var id = ts.FirstGid;
                for (var h = hStart; h < hEnd; h += 1)
                {
                    for (var w = wStart; w < wEnd; w += 1)
                    {
                        var rect = new IntRect();

                        rect.Left = w * wInc;
                        rect.Width = ts.TileWidth;
                        rect.Height = ts.TileHeight;
                        rect.Top = h * hInc;

                        idSheet.Add(id, ts);
                        subImageRects.Add(id, rect);
                        id += 1;
                    }
                }

                // Ignore properties for now
            }

            // Load id maps
            layerID = new List<int[,]>();
            foreach (TmxLayer layer in map.Layers)
            {
                var idMap = new int[MapWidth, MapHeight];
                foreach (TmxLayerTile t in layer.Tiles)
                {
                    
                    idMap[t.X, t.Y] = t.Gid;
                }
                layerID.Add(idMap);

               // if(layer.Name == "tiles")

                // Ignore properties for now
            }

            foreach (TmxObjectGroup objectGroup in map.ObjectGroups)
            {
                if (objectGroup.Name.ToLower().Contains("objects"))
                {
                    ParseCollisionLayer(objectGroup);
                }

                if(objectGroup.Name.ToLower().Contains("parallax"))
                {
                    ParseParallaxLayer(objectGroup);
                }
            }

            ParseTileLayer();
        }

        public void SetRenderComponent(TileMapRenderComponent tmrc)
        {
            tmrc.Set(MapWidth, MapHeight, TileWidth*Scale, TileHeight*Scale, RenderTiles);
        }

        void ParseTileLayer()
        {
            Debug.Assert(layerID.Count == 1);

            int iStart = 0;
            int iEnd = MapWidth;

            var jStart = 0;
            var jEnd = MapHeight;

            foreach (var idMap in layerID)
            {
                for (var i = iStart; i < iEnd; i++)
                {
                    for (var j = jStart; j < jEnd; j++)
                    {
                        var id = idMap[i, j];

                        if (id == 0)
                        {
                            RenderTiles[i, j].Texture = null;
                            continue;
                        }

                        RenderTiles[i, j].Texture = spriteSheets[idSheet[id]];
                        RenderTiles[i, j].SubImageRect = subImageRects[id];
                    }
                }
            }
        }

        void ParseCollisionLayer(TmxObjectGroup objectGroup)
        {
            foreach (TmxObjectGroup.TmxObject obj in objectGroup.Objects)
            {
                PhysicsObject pobj;
                pobj.Position.X = (obj.X + obj.Width / 2) * Scale; // move xy from top left to center of shape
                pobj.Position.Y = (obj.Y + obj.Height / 2) * Scale;
                pobj.Width = obj.Width * Scale;
                pobj.Height = obj.Height * Scale;

                PhysicsObjects.Add(pobj);
            }
        }

        void ParseParallaxLayer(TmxObjectGroup objectGroup)
        {
            foreach (TmxObjectGroup.TmxObject obj in objectGroup.Objects)
            {
                ParallaxSprite sprite;
                sprite.Texture = spriteSheets[idSheet[obj.Tile.Gid]];
                sprite.Position.X = (obj.X + sprite.Texture.Size.X / 2) * Scale; // move xy from top left to center of shape
                sprite.Position.Y = (obj.Y + sprite.Texture.Size.Y / 2) * Scale;

                sprite.Width = sprite.Texture.Size.X * Scale;
                sprite.Height = sprite.Texture.Size.Y * Scale;
                sprite.SubImageRect = subImageRects[obj.Tile.Gid];


                float parallaxFactor;

                if (objectGroup.Properties.ContainsKey("parallax_factor"))
                {
                    if (float.TryParse(objectGroup.Properties["parallax_factor"], out parallaxFactor))
                        sprite.ParallaxFactor = parallaxFactor;
                    else
                        sprite.ParallaxFactor = 1.0f;
                }
                else
                    sprite.ParallaxFactor = 1.0f;

                if (objectGroup.Properties.ContainsKey("zindex"))
                {
                    int zIndex;
                    if (int.TryParse(objectGroup.Properties["zindex"], out zIndex))
                        sprite.ZIndex = zIndex;
                    else
                        sprite.ZIndex = 0;
                }
                else
                    sprite.ZIndex = 0;

                ParallaxSprites.Add(sprite);
            }
        }
    }

}

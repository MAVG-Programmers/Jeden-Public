﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.Graphics;
using SFML.Window;
using Jeden.Engine.Render;
using TiledSharp;
using System.Diagnostics;

namespace Jeden.Engine.TileMap
{
    class TileMap 
    {
        public int MapWidth { get; private set; }
        public int MapHeight { get; private set; }
        public float TileWidth { get; private set; }
        public float TileHeight { get; private set; }


        TmxMap map;

        Dictionary<TmxTileset, Texture> spriteSheet;
        Dictionary<int, IntRect> subImageRects;
        Dictionary<int, TmxTileset> idSheet;
        List<int[,]> layerID;     // for future use, 1 layer only for now

        struct RenderTile
        {
            public Texture Texture;
            public IntRect SubImageRect;
        }

        TileMapRenderComponent.Tile[,] RenderTiles;

        public TileMap(string mapName) 
        {


            map = new TmxMap(mapName);
            MapWidth = map.Width;
            MapHeight = map.Height;
            TileWidth = map.TileWidth;
            TileHeight = map.TileHeight;
            RenderTiles = new TileMapRenderComponent.Tile[MapWidth, MapHeight];

            // Load spritesheets
            spriteSheet = new Dictionary<TmxTileset, Texture>();
            subImageRects = new Dictionary<int, IntRect>();
            idSheet = new Dictionary<int, TmxTileset>();

            foreach (TmxTileset ts in map.Tilesets)
            {
                var newSheet = new Texture(ts.Image.Source);
                spriteSheet.Add(ts, newSheet);

                // Loop hoisting
                var wStart = ts.Margin;
                var wInc = ts.TileWidth + ts.Spacing;
                var wEnd = ts.Image.Width;

                var hStart = ts.Margin;
                var hInc = ts.TileHeight + ts.Spacing;
                var hEnd = ts.Image.Height;

                // Pre-compute sub image rectangles
                var id = ts.FirstGid;
                for (var h = hStart; h < hEnd; h += hInc)
                {
                    for (var w = wStart; w < wEnd; w += wInc)
                    {
                        var rect = new IntRect();

                        rect.Left = w;
                        rect.Width = ts.TileWidth;
                        rect.Height = ts.TileHeight;
                        rect.Top = h;

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

                // Ignore properties for now
            }

            //!!Only 1 layer supported for now!!.
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

                        RenderTiles[i, j].Texture = spriteSheet[idSheet[id]];
                        RenderTiles[i, j].SubImageRect = subImageRects[id];
                    }
                }
            }
        }

        public void SetRenderComponent(TileMapRenderComponent tmrc)
        {
            tmrc.Set(MapWidth, MapHeight, TileWidth, TileHeight, RenderTiles);
        }

    }
}
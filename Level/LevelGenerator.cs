using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformerGame.GameObjects;
using Microsoft.VisualBasic;
using System.IO;
using PlatformerGame.Level;
using PlatformerGame.GameObjects.CollisionObjects.Impl;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PlatformerGame.GameObjects.CollisionObjects.MovablePhysicsObjects.Actors.StatsActors.Impl;
using Microsoft.Xna.Framework;
using PlatformerGame.GameObjects.CollisionObjects.MovablePhysicsObjects.NonActors.Interactables.NPCS;
using PlatformerGame.GameObjects.VisualObjects;

namespace PlatformerGame.Utils
{
    public class LevelGenerator
    {
        private const int X = 0;
        private const int Y = 1;
        private const String STATE_PREFIX = "***";

        private const String SIZE_STATE_STR = "SIZE";
        private const String LAYOUT_STATE_STR = "LAYOUT";
        private const String END_STATE = "END";

        private ContentManager Content;
        private GraphicsDevice graphics;
        private GameObjectContainer container;
        private Vector2 gameSizeInTiles;
        private Point posPoint = new Point(0, 0);
        private Dictionary<String, Texture2D> textureDict = new Dictionary<String, Texture2D>();

        public LevelGenerator(ContentManager Content, GraphicsDevice device)
        {
            this.Content = Content;
            this.graphics = device;
            this.container = new GameObjectContainer();

            initTextures();
            initObjectsCommonToAllLevels();
        }

        public void initObjectsCommonToAllLevels()
        {
            VisualObject interactionIndicator = new VisualObject(true, Content.Load<Texture2D>("Exclamation_Point"), 1, 1, 0, 0, 100, 100);
            VisualObject dialogBox = new VisualObject(true, Content.Load<Texture2D>("dialogBox"), 1, 1, 0, Resolution.getInternalHeight() - 300, Resolution.getInternalWidth(), 300);
            dialogBox.immuneToCamera = true;
            container.interactionIndicator = interactionIndicator;
            container.dialogBox = dialogBox;
        }

        private void initTextures()
        {
            textureDict.Add(LevelGenConst.SLOPE_UP_TILE, Content.Load<Texture2D>("slope_up"));
            textureDict.Add(LevelGenConst.SLOPE_DOWN_TILE, Content.Load<Texture2D>("slope_down"));
            textureDict.Add(LevelGenConst.SLOW_SLOPE_DOWN_1, Content.Load<Texture2D>("down_slope_slow_1"));
            textureDict.Add(LevelGenConst.SLOW_SLOPE_DOWN_2, Content.Load<Texture2D>("down_slope_slow_2"));
            textureDict.Add(LevelGenConst.SLOW_SLOPE_UP_1, Content.Load<Texture2D>("up_slope_slow_1"));
            textureDict.Add(LevelGenConst.SLOW_SLOPE_UP_2, Content.Load<Texture2D>("up_slope_slow_2"));
            textureDict.Add(LevelGenConst.TILE, Content.Load<Texture2D>("solid_tile"));
        }

        public GameObjectContainer generateLevel(String textFile)
        {
            String line;
            StreamReader reader = new StreamReader(TitleContainer.OpenStream(textFile));

            while ((line = reader.ReadLine()) != null)
            {
                if (line.StartsWith(STATE_PREFIX))
                {
                    String state = line.Replace(STATE_PREFIX, "");
                    Console.WriteLine(state);
                    if (state.Equals(SIZE_STATE_STR))
                    {
                        Console.WriteLine("Adding size info");
                        gameSizeInTiles = parseSize(reader);
                    }
                    else if (state.Equals(LAYOUT_STATE_STR))
                    {
                        Console.WriteLine("Adding layout");
                        parseLayout(reader);
                    }
                }
            }

            //flagTilesNextToSlopes();
            //Collect Garbage
            reader.Close();
            return container;
        }

        private Vector2 parseSize(StreamReader reader)
        {
            int width = Int32.Parse(reader.ReadLine());
            int height = Int32.Parse(reader.ReadLine());
            reader.ReadLine();

            return new Vector2(width, height);
        }

        private void parseLayout(StreamReader reader)
        {
            Tile[][] tiles = initializeTileArray(gameSizeInTiles.X, gameSizeInTiles.Y);
            String line;
            int colNum = 0;

            while ((line = reader.ReadLine()) != null)
            {
                String[] tokens = line.Split(',');
                int rowNum = 0;
                foreach (String symbol in tokens)
                {
                    switch(symbol)
                    {
                        case LevelGenConst.TILE:
                            tiles[rowNum][colNum] = createTile(symbol, colNum, rowNum);
                            break;
                        case LevelGenConst.PLAYER:
                            Console.WriteLine("Added player");
                            container.addPlayer(createPlayer(colNum, rowNum));
                            break;
                        case LevelGenConst.SLOPE_DOWN_TILE:
                            tiles[rowNum][colNum] = (createSlopedTile(symbol, colNum, rowNum, false));
                            break;
                        case LevelGenConst.SLOPE_UP_TILE:
                            tiles[rowNum][colNum] = (createSlopedTile(symbol, colNum, rowNum, true));
                            break;
                        case LevelGenConst.SLOW_SLOPE_DOWN_1:
                            tiles[rowNum][colNum] = createSlowSlopedTile(symbol, colNum, rowNum, false, 1);
                            break;
                        case LevelGenConst.SLOW_SLOPE_DOWN_2:
                            tiles[rowNum][colNum] = createSlowSlopedTile(symbol, colNum, rowNum, false, 2);
                            break;
                        case LevelGenConst.SLOW_SLOPE_UP_1:
                            tiles[rowNum][colNum] = createSlowSlopedTile(symbol, colNum, rowNum, true, 1);
                            break;
                        case LevelGenConst.SLOW_SLOPE_UP_2:
                            tiles[rowNum][colNum] = createSlowSlopedTile(symbol, colNum, rowNum, true, 2);
                            break;
                        case LevelGenConst.NPC:
                            container.addNPC(createNPC(colNum, rowNum));
                            break;
                    }
                    rowNum++;
                }
                colNum++;
            }
            this.container.setTiles(tiles);
        }

        private Tile[][] initializeTileArray(float width, float height)
        {
            Tile[][] arr = new Tile[(int)width][];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = new Tile[(int)height];
            }

            return arr;
        }

        private Tile createSlopedTile(String textureKey, int col, int row, bool slopeUp)
        {
            Texture2D txtre = textureDict[textureKey];
            Point p = getPosition(row, col);

            int angle = slopeUp ? 45 : -45;
            Tile tile = new Tile(txtre, 1, 1, p.X, p.Y, LevelGenConst.TILE_WIDTH, LevelGenConst.TILE_HEIGHT, angle);

            return tile;
        }

        private Tile createSlowSlopedTile(String textureKey, int col, int row, bool slopeUp, int piece)
        {
            Texture2D txtre = textureDict[textureKey];
            Point p = getPosition(row, col);

            float angle = slopeUp ? 26.565f : -26.565f;
            float slopeStartY = piece == 1 ? p.Y : p.Y + (LevelGenConst.TILE_HEIGHT / 2);
            if (slopeUp)
                slopeStartY = piece == 1 ? (p.Y + LevelGenConst.TILE_HEIGHT) : p.Y + (LevelGenConst.TILE_HEIGHT / 2);
            Tile tile = new Tile(txtre, 1, 1, p.X, p.Y, LevelGenConst.TILE_WIDTH, LevelGenConst.TILE_HEIGHT, angle, slopeStartY);

            return tile;
        }

        private Tile createEmptyTile(int col, int row)
        {
            Point p = getPosition(row, col);
            Tile tile = Tile.createEmptyTile(p.X, p.Y, LevelGenConst.TILE_WIDTH, LevelGenConst.TILE_HEIGHT);

            return tile;
        }

        private Point getPosition(int row, int col)
        {
            posPoint.X = row * (LevelGenConst.TILE_WIDTH);
            posPoint.Y = col * (LevelGenConst.TILE_HEIGHT);
            return posPoint;
        }

        private Tile createTile(String textureKey, int col, int row)
        {
            Texture2D texture = textureDict[textureKey];
            Point pos = getPosition(row, col);

            Tile tile = new Tile(texture, 1, 1, pos.X, pos.Y, LevelGenConst.TILE_WIDTH, LevelGenConst.TILE_HEIGHT);
            return tile;
        }

        private Player createPlayer(int col, int row)
        {
            Texture2D texture = Drawer.make2DRect(graphics, LevelGenConst.TILE_WIDTH, LevelGenConst.TILE_HEIGHT, Color.Red);
            Texture2D interact = Content.Load<Texture2D>("Exclamation_Point");
            Point p = getPosition(row, col);

            Player tile = new Player(texture, interact, 1, 1, p.X, p.Y, LevelGenConst.TILE_WIDTH, LevelGenConst.TILE_HEIGHT * 2);
            return tile;
        }

        private NPC createNPC(int col, int row)
        {
            Texture2D texture = Drawer.make2DRect(graphics, LevelGenConst.TILE_WIDTH, LevelGenConst.TILE_HEIGHT, Color.Yellow);
            Point p = getPosition(row, col);

            NPC npc = new NPC(texture, 1, 1, p.X, p.Y, LevelGenConst.TILE_WIDTH, LevelGenConst.TILE_HEIGHT * 2);
            npc.dialog = "This is a test. Fuck this gay earth! This line should span two lines because it's extra long! Actually, scratch " +
            "that: it should actually be three lines long with the addition of this extra smidgen of text!";
            return npc;
        }
    }
}

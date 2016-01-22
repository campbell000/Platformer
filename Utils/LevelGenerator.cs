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

        public LevelGenerator(ContentManager Content, GraphicsDevice device)
        {
            this.Content = Content;
            this.graphics = device;
            this.container = new GameObjectContainer();
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

            flagTilesNextToSlopes();
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
                            tiles[rowNum][colNum] = createTile(colNum, rowNum);
                            break;
                        case LevelGenConst.PLAYER:
                            Console.WriteLine("Added player");
                            container.addPlayer(createPlayer(colNum, rowNum));
                            break;
                        case LevelGenConst.SLOPE_DOWN_TILE:
                            tiles[rowNum][colNum] = (createSlopedTile(colNum, rowNum, false));
                            break;
                        case LevelGenConst.SLOPE_UP_TILE:
                            tiles[rowNum][colNum] = (createSlopedTile(colNum, rowNum, true));
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

        private Tile createSlopedTile(int col, int row, bool slopeUp)
        {
            Texture2D txtre = slopeUp ? Content.Load<Texture2D>("slope_up") : Content.Load<Texture2D>("slope_down");
            int[] pos = getPosition(row, col);

            int angle = slopeUp ? 45 : -45;
            Tile tile = new Tile(txtre, 1, 1, pos[X], pos[Y], LevelGenConst.TILE_WIDTH, LevelGenConst.TILE_HEIGHT, angle);

            return tile;
        }

        private Tile createEmptyTile(int col, int row)
        {
            int[] pos = getPosition(row, col);
            Tile tile = Tile.createEmptyTile(pos[X], pos[Y], LevelGenConst.TILE_WIDTH, LevelGenConst.TILE_HEIGHT);

            return tile;
        }

        private int[] getPosition(int row, int col)
        {
            int [] pos = new int[2];
            pos[0] = row * (LevelGenConst.TILE_WIDTH);
            pos[1] = col * (LevelGenConst.TILE_HEIGHT);
            return pos;
        }

        private Tile createTile(int col, int row)
        {
            Texture2D texture = Content.Load<Texture2D>("solid_tile");
            int[] pos = getPosition(row, col);

            Tile tile = new Tile(texture, 1, 1, pos[X], pos[Y], LevelGenConst.TILE_WIDTH, LevelGenConst.TILE_HEIGHT);
            return tile;
        }

        private Player createPlayer(int col, int row)
        {
            Texture2D texture = Drawer.make2DRect(graphics, LevelGenConst.TILE_WIDTH, LevelGenConst.TILE_HEIGHT, Color.Red);
            //Texture2D texture = Drawer.make2DRect(graphics, LevelGenConst.TILE_WIDTH, LevelGenConst.TILE_HEIGHT, Color.Red);
            int[] pos = getPosition(row, col);

            Player tile = new Player(texture, 1, 1, pos[X], pos[Y], LevelGenConst.TILE_WIDTH, LevelGenConst.TILE_HEIGHT);
            return tile;
        }

        /**
         * We need to flag all tiles next to slopes so that, when moving horizontally, physics objects
         * do not clip them, impeding the physics object's movement.
         **/
        private void flagTilesNextToSlopes()
        {
            for (int i = 0; i < gameSizeInTiles.X; i++)
            {
                for (int j = 0; j < gameSizeInTiles.Y; j++)
                {
                    Tile tile = container.getTileAt(i, j);
                    if (tile != null && tile.isSloped)
                    {
                        Tile leftTile = container.getTileAt(i - 1, j);
                        Tile leftTop = container.getTileAt(i-1, j-1);
                        if (leftTile != null && !tile.isSloped && leftTop != null &&leftTop.isSloped)
                            leftTile.isSolid = false;

                        Tile rightTile = container.getTileAt(i + 1, j);
                        Tile rightTop = container.getTileAt(i + 1, j - 1);
                        if (rightTile != null && !rightTile.isSloped &&  rightTop != null && rightTop.isSloped)
                        {
                            rightTile.isSolid = false;
                        }
                    }
                }
            }
        }
    }
}

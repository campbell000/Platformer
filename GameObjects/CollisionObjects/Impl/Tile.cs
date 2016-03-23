using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using PlatformerGame.Utils;

namespace PlatformerGame.GameObjects.CollisionObjects.Impl
{
    public class Tile : CollisionObject
    {
        public const float ONE_TILE_SLOPE_ANGLE = 45;
        public const float TWO_TILE_SLOPE_AMFLE = 26.57f;
        public const float THREE_TILE_SLOPE_ANGLE = 18.43f;

        public Boolean isSloped { get; set; }
        public Point slopeStart { get; set; }
        public Point slopeEnd { get; set; }
        public bool isNextToSlope { get; set; }
        public float slopeAngle { get; set; }

        public Tile(Texture2D texture, int rows, int columns, float x, float y, float width, float height) : 
            base(texture, rows, columns, x, y, width, height)
        {
            isSloped = false;
            slopeStart = new Point(0,0);
            slopeEnd = new Point(0,0);
            slopeAngle = 0;
        }

        public static Tile createEmptyTile(float x, float y, float width, float height)
        {
            Tile tile = new Tile(null, 0, 0, x, y, width, height);
            tile.isSolid = false;
            tile.active = false;
            return tile;
        }

        public Tile(Texture2D texture, int rows, int columns, float x, float y, float width, float height, float slope) : 
            base(texture, rows, columns, x, y, width, height)
        {
            isSloped = true;
            slopeStart = new Point(0, 0);
            slopeEnd = new Point(0, 0);

            if (slope > 0) //Tile that goes like this => /
                initUpSlopeTile(slope);
            else //Tile that goes like this => \
                initDownSlopeTile(slope);

            isNextToSlope = false;
            slopeAngle = slope;
        }

        public Tile(Texture2D texture, int rows, int columns, float x, float y, float width, float height, float slope, float slopeStartY) :
            base(texture, rows, columns, x, y, width, height)
        {
            isSloped = true;
            slopeStart = new Point(0, 0);
            slopeEnd = new Point(0, 0);

            if (slope > 0) //Tile that goes like this => /
                initUpSlopeTile(slope, slopeStartY);
            else //Tile that goes like this => \
                initDownSlopeTile(slope, slopeStartY);

            isNextToSlope = false;
            slopeAngle = slope;
        }

        private void initUpSlopeTile(float slope)
        {
            double radians = slope * (Math.PI / 180);
            FloatRectangle boundingBox = this.getBoundingBox();

            slopeStart.X = boundingBox.Left;
            slopeStart.Y = boundingBox.Bottom;
            double yPos = Math.Tan(radians) * boundingBox.width;
            slopeEnd.X = boundingBox.Right;
            slopeEnd.Y = (float)(boundingBox.Bottom - yPos);
        }

        private void initUpSlopeTile(float slope, float startY)
        {
            double radians = slope * (Math.PI / 180);
            FloatRectangle boundingBox = this.getBoundingBox();

            slopeStart.X = boundingBox.Left;
            slopeStart.Y = startY;
            double yPos = Math.Tan(radians) * boundingBox.width;
            slopeEnd.X = boundingBox.Right;
            slopeEnd.Y = (float)(startY - yPos);
        }

        private void initDownSlopeTile(float slope)
        {
            double radians = Math.Abs(slope) * (Math.PI / 180);
            FloatRectangle boundingBox = this.getBoundingBox();

            this.slopeStart.X = boundingBox.Left;
            this.slopeStart.Y = boundingBox.Top;
            double yPos = Math.Tan(radians) * boundingBox.width;
            this.slopeEnd.X = boundingBox.Right;
            this.slopeEnd.Y= (float)(boundingBox.Top + yPos);
        }

        private void initDownSlopeTile(float slope, float startY)
        {
            double radians = Math.Abs(slope) * (Math.PI / 180);
            FloatRectangle boundingBox = this.getBoundingBox();

            this.slopeStart.X = boundingBox.Left;
            this.slopeStart.Y = startY;
            double yPos = Math.Tan(radians) * boundingBox.width;
            this.slopeEnd.X = boundingBox.Right;
            this.slopeEnd.Y = (float)(startY + yPos);
        }

        public float getSlope()
        {
            return slopeStart.getSlope(slopeEnd);
        }
    }
}

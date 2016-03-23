using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PlatformerGame.Routines;
using PlatformerGame.GameObjects.CollisionObjects.MovablePhysicsObjects.NonActors.Interactables;

namespace PlatformerGame.GameObjects.CollisionObjects.MovablePhysicsObjects.NonActors.Interactables.NPCS
{
    public class NPC : Interactable
    {
        public Routine routine { get; set; }

        public NPC(Texture2D texture, int rows, int columns, float x, float y, float width, float height, Routine routine) : 
            base(texture, rows, columns, x, y, width, height)
        {
            this.routine = routine;
        }

        public NPC(Texture2D texture, int rows, int columns, float x, float y, float width, float height) :
            base(texture, rows, columns, x, y, width, height)
        {
            this.routine = null;
        }

        public NPC(Texture2D texture, int rows, int columns, float x, float y, float width, float height, String dialog) :
            base(texture, rows, columns, x, y, width, height)
        {
            this.routine = null;
            this.dialog = dialog;
        }

        public NPC(Texture2D texture, int rows, int columns, float x, float y, float width, float height, String dialog, Routine routine) :
            base(texture, rows, columns, x, y, width, height)
        {
            this.routine = routine;
            this.dialog = dialog;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;

namespace PlatformerGame.Draw
{
    public class SpriteFontContainer
    {
        public SpriteFont debugFont {get; set;}

        public SpriteFont dialogFont { get; set; }

        public SpriteFontContainer(SpriteFont debug, SpriteFont dialog)
        {
            this.debugFont = debug;
            this.dialogFont = dialog;
        }

        public static SpriteFontContainer initialize(ContentManager content)
        {
            SpriteFont debug = content.Load<SpriteFont>("TestFont");
            SpriteFont dialog = content.Load<SpriteFont>("DialogFont");

            return new SpriteFontContainer(debug, dialog);
        }
    }
}

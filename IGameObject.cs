using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PlatformerGame
{
	public interface IGameObject
	{
		void update(GameTime deltaTime);
	}
}


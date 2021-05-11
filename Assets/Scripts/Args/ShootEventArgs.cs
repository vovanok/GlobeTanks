using System;
using GlobeTanks.Objects;

namespace GlobeTanks.Args
{
	public class ShootEventArgs : EventArgs
	{
		public PlayerView Player { get; private set; }
		public ProjectileView Projectile { get; private set; }

		public ShootEventArgs(PlayerView player, ProjectileView projectile)
		{
			Player = player;
			Projectile = projectile;
		}
	}
}

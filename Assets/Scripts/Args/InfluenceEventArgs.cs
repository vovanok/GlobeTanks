using System;
using GlobeTanks.Objects;

namespace GlobeTanks.Args
{
	public class InfluenceEventArgs : EventArgs
	{
		public ProjectileView Projectile { get; private set; }
		public ProjectileInfluence ProjectileInfluence { get; private set; }

		public InfluenceEventArgs(ProjectileView projectile, ProjectileInfluence projectileInfluence)
		{
			Projectile = projectile;
			ProjectileInfluence = projectileInfluence;
		}
	}
}

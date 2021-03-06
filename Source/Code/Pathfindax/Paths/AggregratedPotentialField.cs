﻿using System.Linq;
using Pathfindax.Graph;

namespace Pathfindax.Paths
{
	public class AggregratedPotentialField : PotentialFieldBase
	{
		public override int Width { get; }
		public override int Height { get; }
		public readonly PotentialFieldBase[] PotentialFields;
		public int NodeCount => GridTransformer.GridSize.X * GridTransformer.GridSize.Y;

		public AggregratedPotentialField(GridTransformer gridTransformer, params PotentialFieldBase[] potentialFields) : base(gridTransformer)
		{
			var firstPotentialField = potentialFields.First();
			TargetNode = firstPotentialField.TargetNode;
			Width = firstPotentialField.Width;
			Height = firstPotentialField.Height;
			TargetWorldPosition = potentialFields.First().TargetWorldPosition;
			PotentialFields = potentialFields;
		}

		public override float GetPotential(int x, int y)
		{
			if (x >= 0 && y >= 0 && x < GridTransformer.GridSize.X && y < GridTransformer.GridSize.Y)
			{
				var potential = 0f;
				for (var i = 0; i < PotentialFields.Length; i++)
				{
					var p = PotentialFields[i].GetPotential(x, y);
					if (float.IsNaN(p)) return float.NaN;
					potential += p;
				}
				return potential;
			}
			return float.NaN;
		}
	}
}
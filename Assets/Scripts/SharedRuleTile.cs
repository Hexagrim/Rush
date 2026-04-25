using UnityEngine;
using UnityEngine.Tilemaps;

public class SharedRuleTile : RuleTile
{
    public override bool RuleMatch(int neighbor, TileBase tile)
    {
        if (neighbor == TilingRule.Neighbor.This) return tile is SharedRuleTile;
        if (neighbor == TilingRule.Neighbor.NotThis) return !(tile is SharedRuleTile);
        return base.RuleMatch(neighbor, tile);
    }
}
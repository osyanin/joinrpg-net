﻿using System.Collections.Generic;
using System.Linq;
using JoinRpg.DataModel;
using JoinRpg.Helpers;

namespace JoinRpg.Domain
{
  public static class ClaimSourceExtensions
  {
    public static bool HasClaimForUser<T>(this T claimSource, int currentUserId) where T : IClaimSource
    {
      return claimSource.Claims.Any(c => c.PlayerUserId == currentUserId && c.IsActive);
    }

    public static IEnumerable<User> GetResponsibleMasters(this IClaimSource @group, bool includeSelf = true)
    {
      return @group.GetParentGroups()
        .UnionIf(@group, includeSelf)
        .Select(g => g.ResponsibleMasterUser)
        .Where(u => u != null)
        .Distinct();
    }

    private static IEnumerable<IClaimSource> GetParentGroups(this IClaimSource @group)
    {
      return @group.FlatTree(g => g.ParentGroups, false);
    }
  }
}
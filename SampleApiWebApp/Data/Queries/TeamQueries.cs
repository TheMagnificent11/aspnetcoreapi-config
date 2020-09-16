using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SampleApiWebApp.Data.Queries
{
    public static class TeamQueries
    {
        public static IQueryable<Domain.Team> GetTeamsByName(this DbSet<Domain.Team> teams, string teamName)
        {
            if (teams is null)
            {
                throw new ArgumentNullException(nameof(teams));
            }

            if (teamName is null)
            {
                throw new ArgumentNullException(nameof(teamName));
            }

            return teams.Where(x => x.Name.Equals(teamName.Trim(), StringComparison.OrdinalIgnoreCase));
        }
    }
}

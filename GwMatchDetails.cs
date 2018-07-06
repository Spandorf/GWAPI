using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace GwApi
{
    public enum GwMatchupTeam
    {
        Red = 0, Blue = 1, Green = 2
    }

    public class GwMatchupDetails
    {
        /// <summary>
        /// Unique identifier for the match. It is in this format:
        /// "R-T" where R is the region (1 = US, 2 = EU) and T is the tier
        /// </summary>
        public readonly string Id;

        /// <summary>
        /// Scores for the match. Access like an array with GwMatchupTeam.
        /// </summary>
        public GwMatchupScore Score { get; internal set; }

        /// <summary>
        /// List of maps in this matchup.
        /// </summary>
        public ReadOnlyCollection<GwMatchupMap> Maps { get; internal set; }

        public GwMatchupDetails(string id)
        {
            Id = id;
        }

        public override bool Equals(object obj)
        {
            var other = obj as GwMatchupDetails;
            return this == other;
        }

        public static bool operator ==(GwMatchupDetails a, GwMatchupDetails b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Score == b.Score && a.Maps.SequenceEqual(b.Maps);
        }

        public static bool operator !=(GwMatchupDetails a, GwMatchupDetails b)
        {
            return !(a == b);
        }
    }

    public class GwMatchupMap
    {
        /// <summary>
        /// Reference to the parent GwMatchupDetails instance.
        /// </summary>
        public readonly GwMatchupDetails Details;

        /// <summary>
        /// Type of the map.
        /// </summary>
        public readonly string Type;

        /// <summary>
        /// Scores for the match. Access like an array with GwMatchupTeam.
        /// </summary>
        public readonly GwMatchupScore Score;

        /// <summary>
        /// List of objectives in this map.
        /// </summary>
        public ReadOnlyCollection<GwMatchupObjective> Objectives { get; internal set; }

        public GwMatchupMap(GwMatchupDetails details, string type, List<int> score)
        {
            Details = details;
            Type = type;
            Score = new GwMatchupScore(score);
        }

        public override bool Equals(object obj)
        {
            var other = obj as GwMatchupMap;
            return this == other;
        }
		
        public static bool operator ==(GwMatchupMap a, GwMatchupMap b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Type == b.Type && a.Score == b.Score && a.Objectives.SequenceEqual(b.Objectives);
        }

        public static bool operator !=(GwMatchupMap a, GwMatchupMap b)
        {
            return !(a == b);
        }
    }

    public class GwMatchupObjective
    {
        /// <summary>
        /// Reference to the parent GwMatchupDetails instance.
        /// </summary>
        public readonly GwMatchupDetails Details;

        /// <summary>
        /// Reference to the parent GwMatchupMap instance.
        /// </summary>
        public readonly GwMatchupMap Map;

        /// <summary>
        /// Unique identifier of the objective.
        /// </summary>
        public readonly string Id;

        /// <summary>
        /// Name of the objective.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Owner of the objective.
        /// </summary>
        public readonly GwMatchupTeam Owner;

        public GwMatchupObjective(GwMatchupDetails details, GwMatchupMap map, string id, string name, GwMatchupTeam owner)
        {
            Details = details;
            Map = map;
            Id = id;
            Name = name;
            Owner = owner;
        }

        public override bool Equals(object obj)
        {
            var other = obj as GwMatchupObjective;
            return this == other;
        }

        public static bool operator ==(GwMatchupObjective a, GwMatchupObjective b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Id == b.Id && a.Owner == b.Owner;
        }

        public static bool operator !=(GwMatchupObjective a, GwMatchupObjective b)
        {
            return !(a == b);
        }
    }

    /// <summary>
    /// Score container. Access like an array with GwMatchupTeam.
    /// </summary>
    public class GwMatchupScore
    {
        private readonly List<int> scores;

        internal GwMatchupScore(List<int> scores)
        {
            this.scores = scores;
        }

        public int this[int i]
        {
            get { return scores[i]; }
        }

        public int this[GwMatchupTeam i]
        {
            get { return scores[(int)i]; }
        }

        public override bool Equals(object obj)
        {
            var other = obj as GwMatchupScore;
            return this == other;
        }

        public static bool operator==(GwMatchupScore a, GwMatchupScore b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.scores.SequenceEqual(b.scores);
        }

        public static bool operator !=(GwMatchupScore a, GwMatchupScore b)
        {
            return !(a == b);
        }
    }
}
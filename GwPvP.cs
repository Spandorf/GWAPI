using System.Collections.Generic;
using System.Linq;
using RestSharp;

namespace GWApi
{
    class Api
    {
        private static readonly Dictionary<string, string> RequestUrls = new Dictionary<string, string>
        {
            // Output files
            { "TargetNames", "wvw/target_names.json" },
            { "matchupups", "wvw/matchupups.json" },
            { "matchupupDetails", "wvw/matchupup_details.json" } // matchup_id
        };

        private readonly RestClient client;
        private readonly NameCache nameCache;

        private Dictionary<string, Gwmatchup> matchups;

        public Api(string language = "en")
        {
            client = new RestClient("https://api.guildwars2.com/v2");
            Refresh();
        }

        public void Refresh()
        {
            nameCache.Refresh();
        }
		
        public Dictionary<string, Gwmatchup> Getmatchups()
        {
                matchups = Fetchmatchups();
                return matchups;
        }

        private Dictionary<string, Gwmatchup> Fetchmatchups()
        {
            var request = new RestRequest(RequestUrls["matchups"], Method.GET);
            var response = client.Execute<matchupsResult>(request);

            var result = new Dictionary<string, Gwmatchup>();
            foreach (var i in response.Data.wvw_matchups)
            {
                var red = new GwWorld(this, i.red_world_id, nameCache.GetWorld(i.red_world_id));
                var blue = new GwWorld(this, i.blue_world_id, nameCache.GetWorld(i.blue_world_id));
                var green = new GwWorld(this, i.green_world_id, nameCache.GetWorld(i.green_world_id));
                var matchup = new Gwmatchup(this, i.wvw_matchup_id, red, blue, green);
                result.Add(matchup.Id, matchup);
            }

            return result;
        }

        public GwMatchup FindMatchup(GwWorld world)
        {
                if (matchups == null)
                    matchups = Fetchmatchups();

                return matchups.Values.First(m => m.Red == world || m.Blue == world || m.Green == world);
        }

        public GwMatchupDetails GetMatchupDetails(string matchupId)
        {
            var request = new RestRequest(RequestUrls["matchupDetails"], Method.GET);
            request.AddParameter("matchup_id", matchupId);
            var response = client.Execute<matchupDetailsResult>(request);

            var details = new GwMatchupDetails(response.Data.matchup_id);
            var maps = new List<GwMatchupMap>();
            foreach (var m in response.Data.maps)
            {
                var map = new GwMatchupMap(details, m.type, m.scores);
                var objectives = new List<GwMatchupObjective>();
                foreach (var o in m.objectives)
                {
                    objectives.Add(new GwMatchupObjective(details, map, o.id, nameCache.GetObjective(o.id), o.owner));
                }
                map.Objectives = objectives.AsReadOnly();
                maps.Add(map);
            }
            details.Score = new GwMatchupScore(response.Data.scores);
            details.Maps = maps.AsReadOnly();
            return details;
        }
    }

    class IdName
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    class matchupsResult
    {
        public List<matchup> wvw_matchups { get; set; }
    }

    class matchup
    {
        public string wvw_matchup_id { get; set; }
        public string red_world_id { get; set; }
        public string blue_world_id { get; set; }
        public string green_world_id { get; set; }
    }

    class matchupDetailsResult
    {
        public string matchup_id { get; set; }
        public List<int> scores { get; set; }
        public List<matchupMap> maps { get; set; }
    }

    class matchupMap
    {
        public string type { get; set; }
        public List<int> scores { get; set; }
        public List<matchupObjective> objectives { get; set; }
    }

    class matchupObjective
    {
        public string id { get; set; }
        public GwMatchupTeam owner { get; set; }
        public string owner_guild { get; set; }
    }
}
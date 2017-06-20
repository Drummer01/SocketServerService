using System.Collections.Generic;
using System.Linq;
using Server.Sock.Ws;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace SocketServer.Modules
{
    class Filter : Module
    {
        public static Filter create(int id)
        {
            Filter filter = null;
            using (var ctx = new DataAccess.DataModel())
            {
                var data = ctx.filter_mapping.Where(fm => fm.fid == id).ToArray();
                filter = new Filter(data.Select(fm => fm.original).ToArray(), data.Select(fm => fm.replacement).ToArray());
            }
            return filter;
        }

        private string pattern;
        private Dictionary<string, string> mapping;

        private Filter(string[] origins, string[] replacements)
        {
            mapping = new Dictionary<string, string>();
            for (int i = 0; i < origins.Length; i++)
            {
                mapping.Add(origins[i], replacements[i]);
            }
        }

        public override void execute(ref Response response)
        {
            response.data.message = Regex.Replace((string)response.data.message, getPattern(), filter);
        }

        private string filter(Match match)
        {
            Debug.WriteLine(match.Value);
            return getReplacent(match.Value);
        }

        private string getReplacent(string match)
        {
            foreach(string key in mapping.Keys)
            {
                if (Regex.IsMatch(match, key)) return mapping[key];
            }
            return string.Empty;
        }

        private string getPattern()
        {
            if(pattern == null)
            {
                pattern = string.Format("({0})", string.Join("|", this.mapping.Keys));
            }
            return pattern;
        }
    }
}

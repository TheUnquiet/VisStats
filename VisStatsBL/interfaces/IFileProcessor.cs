using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisStatsBL.model;

namespace VisStatsBL.interfaces
{
    public interface IFileProcessor
    {
        List<string> LeesSoorten(string filename);
        List<string> LeesHavens(string filename);
        public List<VisStatsDataRecord> LeesMonthlyResults(string filename, List<Vissoort> vissoorten, List<Haven> havens);
    }
}

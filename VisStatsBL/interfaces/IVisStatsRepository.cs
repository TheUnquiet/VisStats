using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisStatsBL.interfaces;
using VisStatsBL.model;

namespace VisStatsBL.interfaces
{
    public interface IVisStatsRepository
    {
        bool HeeftVissoort(Vissoort vis);
        void SchrijfSoort(Vissoort vis);
        bool HeeftHaven(Haven haven);
        void SchrijfHaven(Haven haven);
        bool IsOpgeladen(string filename);
        List<Haven> LeesHavens();
        List<Vissoort> LeesSoorten();
        void SchrijfStatistieken(List<VisStatsDataRecord> data, string filename);
    }
}
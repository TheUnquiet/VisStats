using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisStatsBL.exceptions;
using VisStatsBL.interfaces;
using VisStatsBL.model;

namespace VisStatsBL.managers
{
    public class VisStatsManager
    {
        private IFileProcessor _fileProcessor; // interface zegt wat er moet geimplementeert worden
        private IVisStatsRepository _visstatsreposotory;

        public VisStatsManager(IFileProcessor processor, IVisStatsRepository visrepo)
        {
            this._fileProcessor = processor;
            this._visstatsreposotory = visrepo;
        }

        public void UploadVis(string filename)
        {
            List<string> soorten = _fileProcessor.LeesSoorten(filename);
            List<Vissoort> vissoorten = MaakVissoorten(soorten);

            foreach (Vissoort vis in vissoorten)
            {
                if (!_visstatsreposotory.HeeftVissoort(vis))
                {
                    _visstatsreposotory.SchrijfSoort(vis);
                }
            }
        }

        private List<Vissoort> MaakVissoorten(List<string> soorten)
        {
            Dictionary<string, Vissoort> visSoorten = new();
            foreach (var soort in soorten)
            {
                if (!visSoorten.ContainsKey(soort))
                {
                    try
                    {
                        visSoorten.Add(soort, new Vissoort(soort));
                    }
                    catch (domeinException) { }
                }
            }
            return visSoorten.Values.ToList();
        }

        private List<Haven> MaakHaven(List<string> havens)
        {
            Dictionary<string, Haven> havenDict = new();
            foreach (var haven in havens)
            {
                if (!havenDict.ContainsKey(haven))
                {
                    try
                    {
                        havenDict.Add(haven, new Haven(haven));
                    }
                    catch (domeinException) { }
                }
            }
            return havenDict.Values.ToList();
        }

        public void UploadHaven(string filename)
        {
            List<string> havens = _fileProcessor.LeesHavens(filename);
            List<Haven> havenLijst = MaakHaven(havens);

            foreach (Haven haven in havenLijst)
            {
                if (!_visstatsreposotory.HeeftHaven(haven))
                {
                    _visstatsreposotory.SchrijfHaven(haven);
                }
            }
        }

        public void UploadStatistieken(string filename)
        {
            try
            {
                if (!_visstatsreposotory.IsOpgeladen(filename))
                {
                    List<Haven> havens = _visstatsreposotory.LeesHavens();
                    List<Vissoort> soorten = _visstatsreposotory.LeesSoorten();
                    List<VisStatsDataRecord> data = _fileProcessor.LeesMonthlyResults(filename, soorten, havens);
                    _visstatsreposotory.SchrijfStatistieken(data, filename);
                }
            }
            catch (Exception ex) { throw new domeinException("UploadStatistieken", ex); }
        }

        public List<Haven> GeefHavens()
        {
            try
            {
                return _visstatsreposotory.LeesHavens();
            } catch (Exception ex)
            {
                throw new domeinException("GeefHavens", ex);
            }
        }

        public List<int> GeefJaartallen()
        {
            try
            {
                return _visstatsreposotory.LeesJaartallen();
            } catch (Exception ex)
            {
                throw new domeinException("GeefJaartallen", ex);
            }
        }

        public List<int> GeefMaanden()
        {
            try
            {
                return _visstatsreposotory.LeesJaartallen();
            }
            catch (Exception ex)
            {
                throw new domeinException("GeefJaartallen", ex);
            }
        }

        public List<Vissoort> GeefVissoorten()
        {
            try
            {
                return _visstatsreposotory.LeesSoorten();
            }
            catch (Exception ex)
            {
                throw new domeinException("GeefVissoorten", ex);
            }
        }

        public List<JaarVangst> GeefVangst(int jaar, Haven haven, List<Vissoort> vissoorten, Eenheid eenheid)
        {
            try
            {
                return _visstatsreposotory.LeesStatistieken(jaar, haven, vissoorten, eenheid);
            }
            catch (Exception ex)
            {
                throw new domeinException("GeefVissoorten", ex);
            }
        }

        public List<MaandVangst> GeefMaandVangst(int jaar, int maand, Vissoort soort, Haven haven)
        {
            try
            {
                return _visstatsreposotory.LeesMaandVangst(jaar, maand, soort, haven);
            }
            catch (Exception ex)
            {
                throw new domeinException("GeefMaandVangst", ex);
            }
        }
    }
}

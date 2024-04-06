using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VisStatsBL.interfaces;
using VisStatsBL.model;

namespace VisStatsDL_File
{
    public class FileProcessor : IFileProcessor // waar ifp, fp
    {
        public List<string> LeesHavens(string filename)
        {
            try
            {
                List<string> havens = new List<string>();
                using (StreamReader sr = new StreamReader(filename))
                {
                    string text;
                    while ((text = sr.ReadLine()) != null)
                    {
                        havens.Add(text.Trim());
                    }

                    return havens;
                }
            } catch (Exception ex) { throw new Exception("FileProcessor-LeesHavens", ex); }
        }

        public List<string> LeesSoorten(string filename)
        {
            try
            {
                List<string> soorten = new List<string>();

                using (StreamReader sr = new StreamReader(filename)) // auto opkuis: 'using'
                {
                    string text;
                    while ((text = sr.ReadLine()) != null)
                    {
                        soorten.Add(text.Trim());
                    }

                    return soorten;
                }
            }

            catch (Exception ex) { throw new Exception("FileProcessor-Leessoorten", ex); }
        }

        public List<VisStatsDataRecord> LeesMonthlyResults(string filename, List<Vissoort> vissoorten, List<Haven> havens)
        {
            try
            {
                // dict -> snel zoeken
                Dictionary<string, Vissoort> soortenDictionary = vissoorten.ToDictionary(x => x.Naam, x => x); // lambda
                Dictionary<string, Haven> havenDictionary = havens.ToDictionary(x => x.Naam, x => x); // lambda
                Dictionary<(string, int, int, string), VisStatsDataRecord> data = new(); // haven, jaar, maand, soort
                using (StreamReader sr = new StreamReader(filename))
                {
                    string line;
                    int jaartal = 0, maand = 0;
                    List<string> havensTxt = new List<string>();
                    while ((line = sr.ReadLine()) != null)
                    {
                        // lees tot begin van een maand
                        if (Regex.IsMatch(line, @"^-+\d{6}-+")) // - then 6 digits then -
                        {
                            jaartal = Int32.Parse(Regex.Match(line, @"\d{4}").Value);
                            maand = Int32.Parse(Regex.Match(line, @"(\d{2})-+").Groups[1].Value);
                            havensTxt.Clear();
                        }
                        // lees naam havens
                        else if (line.Contains("Vissoorten|Totaal van de havens"))
                        {
                            string pattern = @"\|([A-Za-z]+)\|";
                            MatchCollection matches = Regex.Matches(line, pattern);
                            foreach (Match match in matches) havensTxt.Add(match.Groups[1].Value);
                        }
                        // lees statistieken
                        else
                        {
                            string[] element = line.Split('|');
                            // eerste element is naam van vissoort
                            if (soortenDictionary.ContainsKey(element[0]))
                            {
                                for(int i = 0; i < havensTxt.Count; i++)
                                {
                                    if (havenDictionary.ContainsKey(havensTxt[i]))
                                    {
                                        if (data.ContainsKey((havensTxt[i], jaartal, maand, element[0])))
                                        {
                                            data[(havensTxt[i], jaartal, maand, element[0])].Gewicht += ParseValue(element[(i * 2) + 3]);
                                            data[(havensTxt[i], jaartal, maand, element[0])].Waarde += ParseValue(element[(i * 2) + 4]);
                                        }
                                        else
                                        {
                                            data.Add((havensTxt[i], jaartal, maand, element[0]),
                                                new VisStatsDataRecord(jaartal, maand, ParseValue(element[(i * 2) + 3]), ParseValue(element[(i * 2) + 4]) , havenDictionary[havensTxt[i]], soortenDictionary[element[0]]));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return data.Values.ToList();
            } 
            catch (Exception ex) { throw new Exception("FileProcessor-LeesMonthlyResults", ex); }
        }

        private double ParseValue(string value)
        {
            if (double.TryParse(value, out double d)) return d;
            else return 0.0;
        }
    }
}

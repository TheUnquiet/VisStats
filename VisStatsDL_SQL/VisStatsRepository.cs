using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisStatsBL.interfaces;
using VisStatsBL.model;

namespace VisStatsDL_SQL
{
    public class VisStatsRepository : IVisStatsRepository
    {
        private string connectionString;

        public VisStatsRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public bool HeeftHaven(Haven haven)
        {
            string SQL = "SELECT COUNT(*) FROM Haven WHERE naam=@naam;";
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = SQL;
                cmd.Parameters.AddWithValue("@naam", haven.Naam);
                int n = (int)cmd.ExecuteScalar();
                if (n > 0) return true; else return false;
            }
        }

        public bool HeeftVissoort(Vissoort vissoort)
        {
            // bestaat het ?
            string SQL = "SELECT COUNT(*) FROM Soort WHERE naam=@naam;"; // 1 = exists 0 = does not exist
            using (SqlConnection conn = new SqlConnection(connectionString)) // maak connn
            using (SqlCommand cmd = conn.CreateCommand()) // maak comm
            {
                conn.Open();
                cmd.CommandText = SQL;
                cmd.Parameters.AddWithValue("@naam", vissoort.Naam);
                int n = (int)cmd.ExecuteScalar();

                if (n > 0 ) return true; else return false;
            }
        }

        public void SchrijfHaven(Haven haven)
        {
            string SQL = "INSERT INTO Haven(naam) VALUES(@naam)";
            using(SqlConnection conn = new SqlConnection(connectionString))
            using(SqlCommand cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = SQL;
                cmd.Parameters.Add(new SqlParameter("@naam", SqlDbType.NChar));
                cmd.Parameters["@naam"].Value = haven.Naam;
                cmd.ExecuteNonQuery();
            }
        }

        public void SchrijfSoort(Vissoort vissoort)
        {
            string SQL = "INSERT INTO Soort(naam) VALUES(@naam)"; // @ parameter
            using(SqlConnection conn = new SqlConnection(connectionString))
            using(SqlCommand cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = SQL;
                cmd.Parameters.Add(new SqlParameter("@naam", SqlDbType.NVarChar));
                cmd.Parameters["@naam"].Value = vissoort.Naam;
                cmd.ExecuteNonQuery();
            }
        }

        public bool IsOpgeladen(string filename)
        {
            string SQL = "SELECT Count(*) FROM upload WHERE filename=@filename";
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = SQL;
                    cmd.Parameters.Add(new SqlParameter("@filename", SqlDbType.NVarChar));
                    cmd.Parameters["@filename"].Value = filename.Substring(filename.LastIndexOf("\\") + 1);
                    int n = (int)cmd.ExecuteScalar();
                    if (n > 0) return true; else return false;
                } catch (Exception ex)
                {
                    throw new Exception("IsOpgeladen", ex);
                }
            }
        }

        public List<Haven> LeesHavens()
        {
            string Sql = "SELECT * FROM Haven";
            List<Haven> havens = new List<Haven>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = Sql;
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                       havens.Add(new Haven((int)reader["id"], (string)reader["naam"]));
                    }
                    return havens;
                }
                catch (Exception ex) { throw new Exception("LeesHavens", ex); }
            }
        }

        public List<Vissoort> LeesSoorten()
        {
            string Sql = "SELECT * FROM Soort";
            List<Vissoort> vissoorten = new List<Vissoort>();
            using(SqlConnection conn = new SqlConnection(connectionString))
            using(SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = Sql;
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        vissoorten.Add(new Vissoort((int)reader["id"], (string)reader["naam"]));
                    }
                    return vissoorten;
                }
                catch(Exception ex) { throw new Exception("LeesSoorten", ex); }
            }
        }

        public void SchrijfStatistieken(List<VisStatsDataRecord> data, string filename)
        {
            string Sqldata = "INSERT INTO VisStats(jaar,maand,haven_id, soort_id,gewicht,waarde) VALUES(@jaar,@maand,@haven_id,@soort_id,@gewicht,@waarde)";
            string SqlUpload = "INSERT INTO Upload(filename,datum,pad) VALUES(@filename,@datum,@pad)";
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = Sqldata;
                    cmd.Transaction = conn.BeginTransaction();
                    cmd.Parameters.Add(new SqlParameter("@jaar", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@maand", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@haven_id", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@soort_id", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@gewicht", SqlDbType.Float));
                    cmd.Parameters.Add(new SqlParameter("@waarde", SqlDbType.Float));
                    foreach (VisStatsDataRecord dataRecord in data)
                    {
                        cmd.Parameters["@jaar"].Value = dataRecord.Jaar;
                        cmd.Parameters["@maand"].Value = dataRecord.Maand;
                        cmd.Parameters["@haven_id"].Value = dataRecord.Haven.Id;
                        cmd.Parameters["@soort_id"].Value = dataRecord.Vissoort.Id;
                        cmd.Parameters["@gewicht"].Value = dataRecord.Gewicht;
                        cmd.Parameters["@waarde"].Value = dataRecord.Waarde;
                        cmd.ExecuteNonQuery();
                    }

                    cmd.CommandText = SqlUpload;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@filename", filename.Substring(filename.LastIndexOf("\\") + 1));
                    cmd.Parameters.AddWithValue("@pad", filename.Substring(0, filename.LastIndexOf("\\") + 1));
                    cmd.Parameters.AddWithValue("@datum", DateTime.Now);
                    cmd.ExecuteNonQuery();
                    cmd.Transaction.Commit();
                } catch (Exception ex)
                {
                    cmd.Transaction.Rollback();
                    throw new Exception("SchrijfStatistieken", ex);
                }
            } 
        }

        public List<MaandVangst> LeesMaandVangst(int jaar, int maand, Vissoort soort, Haven haven)
        {
            
            string sql = $"SELECT vst.maand, vst.jaar, s.naam as naam, SUM(waarde) as totaal FROM VisStats vst LEFT JOIN Soort s on vst.soort_id = s.id INNER JOIN Haven h on vst.haven_id = h.id WHERE vst.maand = @maand AND vst.jaar = @jaar AND s.id = @soort_id AND h.naam = @haven GROUP BY vst.maand, vst.jaar, s.naam;";

            List<MaandVangst> vangst = new();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@soort_id", soort.Id);
                    cmd.Parameters.AddWithValue("@jaar", jaar);
                    cmd.Parameters.AddWithValue("@maand", maand);
                    cmd.Parameters.AddWithValue("@haven", haven.Naam);
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        vangst.Add(new MaandVangst((string)reader["naam"], (double)reader["totaal"], (int)reader["maand"], (int)reader["jaar"]));
                    }
                    return vangst;
                }
                catch (Exception ex)
                {
                    throw new Exception("LeesStatistieken", ex);
                }
            }
        }

        public List<JaarVangst> LeesStatistieken(int jaar, Haven haven, List<Vissoort> vissoorten, Eenheid eenheid)
        {
            string kolom = "";
            switch (eenheid)
            {
                case Eenheid.kg: kolom = "gewicht"; break;
                case Eenheid.euro: kolom = "waarde"; break;
            }
            string paramSoorten = "";
            for (int i = 0; i < vissoorten.Count; i++) paramSoorten += $"@ps{i},";
            paramSoorten = paramSoorten.Remove(paramSoorten.Length - 1);
            string Sql = $"SELECT soort_id,t2.naam soortnaam,jaar,sum({kolom}) totaal,min({kolom}) minimum,max({kolom}) maximum,avg({kolom}) gemiddelde FROM VisStats t1 left join Soort t2 on t1.soort_id=t2.id where jaar=@jaar and soort_id in({paramSoorten}) and haven_id=@haven_id group by soort_id,t2.naam,jaar";
            List<JaarVangst> vangst = new();
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = Sql;
                    cmd.Parameters.AddWithValue("@haven_id", haven.Id);
                    cmd.Parameters.AddWithValue("@jaar", jaar);
                    for (int i = 0; i < vissoorten.Count; i++) cmd.Parameters.AddWithValue($"@ps{i}", vissoorten[i].Id);
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        vangst.Add(new JaarVangst((string)reader["soortnaam"], (double)reader["totaal"], (double)reader["minimum"], (double)reader["maximum"], (double)reader["gemiddelde"]));
                    }
                    return vangst;
                } catch (Exception ex)
                {
                    throw new Exception("LeesStatistieken", ex);
                }
            }
        }

        public List<int> LeesJaartallen()
        {
            string Sql = "SELECT distinct jaar FROM visstats";
            List<int> jaartallen = new List<int>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = Sql;
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        jaartallen.Add((int)reader["jaar"]);
                    }
                    return jaartallen;
                }
                catch (Exception ex)
                {
                    throw new Exception("Leesjaartallen", ex);
                }
            }
        }
    }
}

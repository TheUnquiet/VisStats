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

        bool IsOpgeladen(string filename)
        {
            throw new NotImplementedException();

            // check if filename exist
        }

        List<Haven> LeesHavens()
        {
            throw new NotImplementedException();
        }

        List<Vissoort> LeesSoorten()
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

        void SchrijfStatistieken(List<VisStatsDataRecord> data, string filename)
        {
            throw new NotImplementedException();
        }
    }
}

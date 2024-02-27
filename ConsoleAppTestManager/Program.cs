using System;
using VisStatsBL.interfaces;
using VisStatsBL.managers;
using VisStatsDL_File;
using VisStatsDL_SQL;

namespace ConsoleAppTestManager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string filePath = @"C:\Users\hp\Documents\Lani\vissoorten.txt";
            string connectionString = "Data Source=LAPTOP-RQN2J66V\\" +
                "SQLEXPRESS;Initial Catalog=VisStats;Integrated Security=True;TrustServerCertificate=True";
            IFileProcessor processor = new FileProcessor();
            IVisStatsRepository visrepo = new VisStatsRepository(connectionString);
            VisStatsManager visstatsManager = new VisStatsManager(processor, visrepo);
            visstatsManager.UploadVis(filePath);
        }
    }
}
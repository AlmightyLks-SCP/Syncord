using Dapper;
using SyncordInfo.ServerStats;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;

namespace SyncordBot.Database
{
    public sealed class SyncordDB
    {
        public string PathToDB { get; private set; }

        private readonly string _connectionString = "Data Source=Database/SyncordData.db";
        private List<Type> _dtoTypes;

        public SyncordDB()
        {
            PathToDB = Path.Combine(Directory.GetCurrentDirectory(), "Database", "SyncordData.db");
            if (!Directory.Exists("Database"))
            {
                Directory.CreateDirectory("Database");
            }
            if (!File.Exists(PathToDB))
            {
                SQLiteConnection.CreateFile(PathToDB);
                Console.WriteLine("SyncordData.db created");
            }
            _dtoTypes = new List<Type>()
            {
                typeof(FpsStat),
                typeof(PlayerCountStat),
                typeof(DeathStat),
            };
            CreateTables();
        }

        private void CreateTables()
        {
            using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
            {
                foreach (var dto in _dtoTypes)
                {
                    if (!TableExists(dto.Name, connection))
                    {
                        connection.Execute("create table ");
                    }
                }
            }
        }
        private bool TableExists(string name, SQLiteConnection connection)
        {
            int tableCount = connection.Query( , $"SELECT * FROM sqlite_master WHERE type = 'table' AND name = '{name}'").Count;
            if (tableCount >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SaveFpsStats(List<FpsStat> fpsStats)
        {
            using (IDbConnection connection = new SQLiteConnection(_connectionString))
            {
                foreach (FpsStat stat in fpsStats)
                {
                    connection.Execute("insert into FpsStats(DateTime, Amount, IsIdle) values (@DateTime, @Fps, @IsIdle)", stat);
                }
            }
        }
        public List<FpsStat> LoadFpsStats()
        {
            using (IDbConnection connection = new SQLiteConnection(_connectionString))
            {
                var result = connection.Query<FpsStat>("select * from FpsStats");
                return result.ToList();
            }
        }
    }
}

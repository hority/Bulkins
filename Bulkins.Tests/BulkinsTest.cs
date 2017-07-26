using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using Bulkins;
using System.IO;

namespace Bulkins.Tests
{
    [TestClass]
    public class BulkinsTest
    {
        private string _connectionString;

        [TestInitialize]
        public void Init()
        {
            _connectionString = File.ReadAllText("ConnectionString.txt");
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO DESTINATION_TABLE (FIELD1,FIELD2) VALUES ('1','VALUE1')";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "TRUNCATE TABLE DESTINATION_TABLE";
                    cmd.ExecuteNonQuery();
                }
            }
        }
        
        [TestMethod]
        public void BulkInsertWorks()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                try
                {
                    var bulkins = new BulkInsertOperation();
                    bulkins.From("SourceFileTest.txt").To("DESTINATION_TABLE");
                    conn.ExecuteBulkInsert(bulkins);
                }
                catch(Exception ex)
                {
                    Assert.Fail("Fail:(0){1}", ex.GetType(), ex.Message);
                }
            }

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void ExportToWorks()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT * FROM DESTINATION_TABLE";
                    try
                    {
                        cmd.ExportTo("Result.csv");
                    }
                    catch (Exception ex)
                    {
                        Assert.Fail("Fail:(0){1}", ex.GetType(), ex.Message);
                    }
                }
            }

            Assert.IsTrue(true);
        }
    }
}

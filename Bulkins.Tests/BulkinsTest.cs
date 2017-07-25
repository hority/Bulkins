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
        }

        [TestMethod]
        public void ItWorks()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                try
                { 
                    conn.BulkInsert(c => c.From(new SourceFileInfo("SourceFileTest.txt", true)).To("DESTINATION_TABLE"));
                }
                catch(Exception ex)
                {
                    Assert.Fail("Fail:(0){1}", ex.GetType(), ex.Message);
                }
            }

            Assert.IsTrue(true);
        }
    }
}

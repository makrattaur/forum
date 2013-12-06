using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using System.IO;

namespace AttachDetachDb
{
    class Program
    {
        static string GetDatabasePath()
        {
            string currentDir = Directory.GetCurrentDirectory();
            string root = Directory.GetDirectoryRoot(currentDir);
            string path = Path.Combine(root, @"Documents\ecole\A2013\420_3GB\TP05_files\Forum.mdf");

            return path;
            //return @"K:\Documents\ecole\A2013\420_3GB\TP05_files\Forum.mdf";
        }

        static string connectionString = "Server=(LocalDB)\\v11.0;Initial Catalog=master;Integrated Security=SSPI;";

        static void Attach()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("CREATE DATABASE Forum ON (FILENAME = '" + GetDatabasePath() + "') FOR ATTACH;", conn))
                {
                    //cmd.Parameters.AddWithValue("@dbloc", GetDatabasePath());

                    cmd.ExecuteNonQuery();
                }
            }
        }

        static void Detach()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("sp_detach_db 'Forum';", conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        static void Main(string[] args)
        {
            Attach();
            //Detach();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dbf2csv
{
    class Program
    {
        static void Main(string[] args)
        {
            var files = Directory.GetFiles(".", "*.dbf");
            string delimiter = ConfigurationManager.AppSettings["delimiter"];
            foreach (string file in files)
            {
                Console.Write("Processing " + file + "...");
                using (OleDbConnection connection = new OleDbConnection("Provider=VFPOLEDB.1;Data Source=" + file))
                {
                    connection.Open();
                    
                    OleDbCommand cmd = new OleDbCommand("select * from " + file, connection);
                    DataTable dt = new DataTable();
                    OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                    da.Fill(dt);

                    StringBuilder sb = new StringBuilder();

                    IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().Select(column => column.ColumnName);
                    sb.AppendLine(string.Join(delimiter, columnNames));

                    foreach (DataRow row in dt.Rows)
                    {
                        //strip out single newline and carriage returns that bork things up.
                        IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString().Replace("\r", "").Replace("\n", "").Trim());
                        sb.AppendLine(string.Join(delimiter, fields));
                    }

                    File.WriteAllText(Path.ChangeExtension(file, "csv"), sb.ToString());
                    Console.WriteLine("   - wrote " +dt.Rows.Count+ " lines");
                }
            }

            Console.WriteLine("Done!!! Hope it worked!");
            Console.ReadLine();
        }
    }
}

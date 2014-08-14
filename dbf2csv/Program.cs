using System;
using System.Collections.Generic;
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
                    sb.AppendLine(string.Join(",", columnNames));

                    foreach (DataRow row in dt.Rows)
                    {
                        IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                        sb.AppendLine(string.Join(",", fields));
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

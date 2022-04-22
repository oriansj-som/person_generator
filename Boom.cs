using sqlite_Example;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Person_Generator
{
    class Boom
    {
        static public Task Generate(string program, string database, string release, string from, string tool, string assignee, List<string> extra_cc_people, List<string> extra_bcc_people, bool slow_mode, string sig)
        {
            
            string cmd = string.Format("--from {0} --tool {1} --release {2} --database {3} --assignee {4} --verbose", from, tool, release, database, assignee);
            foreach (string s in extra_cc_people)
            {
                cmd = cmd + " --cc " + s;
            }
            foreach (string s in extra_bcc_people)
            {
                cmd = cmd + " --bcc " + s;
            }
            if(null != sig)
            {
                cmd = cmd + " --sig-file " + sig;
            }
            return Task.Run(() => Launch(program, cmd, slow_mode));
        }

        static private void Launch(string program, string cmd, bool slow_mode)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = program;
            proc.StartInfo.Arguments = cmd;
            proc.StartInfo.UseShellExecute = !slow_mode;
            proc.StartInfo.RedirectStandardOutput = false;
            proc.Start();
            proc.WaitForExit();
        }

        static public void fire(string program, string database, string release, string from, string tool, List<string> extra_cc_people, List<string> extra_bcc_people, bool slow_mode, string sig)
        {
            SQLiteDatabase mydatabase = new SQLiteDatabase(database);

            try
            {
                DataTable ds = mydatabase.GetDataTable("SELECT DISTINCT Username FROM Agile_Teams");
                List<Task> running = new List<Task>();
                foreach (DataRow dr in ds.Rows)
                {
                    string s = dr["Username"].ToString();
                    running.Add(Generate(program, database, release, from, tool, s, extra_cc_people, extra_bcc_people, slow_mode, sig));
                }

                foreach(Task t in running)
                {
                    t.Wait();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Environment.Exit(4);
            }



            mydatabase.SQLiteDatabase_Close();
        }
    }
}

using System;
using System.Collections.Generic;

namespace Person_Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Person_Generator");
            string release = null;
            string tool_generate = null;
            string tool_send = null;
            string sender = null;
            bool fire_email = false;
            string Database = "example.db";
            bool debug = false;
            string signature = null;
            List<string> message_cc = new List<string>();
            List<string> message_bcc = new List<string>();
            int i = 0;
            while (i < args.Length)
            {
                if (match("--release", args[i]))
                {
                    release = args[i + 1];
                    i = i + 2;
                }
                else if (match("--tool-generate", args[i]))
                {
                    tool_generate = args[i + 1];
                    i = i + 2;
                }
                else if (match("--tool-send", args[i]))
                {
                    tool_send = args[i + 1];
                    i = i + 2;
                }
                else if (match("--from", args[i]))
                {
                    sender = args[i + 1];
                    i = i + 2;
                }
                else if (match("--sig-file", args[i]))
                {
                    signature = args[i + 1];
                    i = i + 2;
                }
                else if (match("--database", args[i]))
                {
                    Database = args[i + 1];
                    i = i + 2;
                }
                else if (match("--cc", args[i]))
                {
                    message_cc.Add(args[i + 1]);
                    i = i + 2;
                }
                else if (match("--bcc", args[i]))
                {
                    message_bcc.Add(args[i + 1]);
                    if (!fire_email) Console.WriteLine("Warning bcc does not survive writing to a file");
                    i = i + 2;
                }
                else if (match("--fire-email", args[i]))
                {
                    fire_email = true;
                    i = i + 1;
                }
                else if (match("--debug-mode", args[i]))
                {
                    debug = true;
                    i = i + 1;
                }
                else if (match("--verbose", args[i]))
                {
                    int index = 0;
                    foreach (string s in args)
                    {
                        Console.WriteLine(string.Format("argument {0}: {1}", index, s));
                        index = index + 1;
                    }
                    i = i + 1;
                }
                else
                {
                    Console.WriteLine(string.Format("Unknown argument: {0} received", args[i]));
                    i = i + 1;
                }
            }

            if(null == release)
            {
                Console.WriteLine("You need to provide --release YY.MM.DD");
                Environment.Exit(1);
            }

            if (null == tool_generate)
            {
                Console.WriteLine("You need to provide what tool will generate the messages --tool-generate example.exe");
                Environment.Exit(2);
            }

            if (null == tool_send)
            {
                Console.WriteLine("You need to provide what tool will send/write the messages to outlook files --tool-send example.exe");
                Environment.Exit(3);
            }

            if (null == sender)
            {
                Console.WriteLine("You need to provide who you are --from me@email.domain");
                Environment.Exit(1);
            }

            Boom.fire(tool_generate, Database, release, sender, tool_send, message_cc, message_bcc, debug, signature);
            Console.WriteLine("Generation done");
        }

        static bool match(string a, string b)
        {
            return a.Equals(b, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}

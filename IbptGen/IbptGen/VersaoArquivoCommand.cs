using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IbptGen
{
    public class VersaoArquivoCommand : IBPTCommand
    {
        public VersaoArquivoCommand(string caminhoArquivoDados)
        {
            CaminhoArquivoDados = caminhoArquivoDados;
        }

        public string CaminhoArquivoDados { get; }


        public void Execute()
        {
            if(!File.Exists(CaminhoArquivoDados))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Arquivo de dados não localizado ou não exisste");
                return;
            }

            SQLiteConnectionStringBuilder sb = new SQLiteConnectionStringBuilder();
            sb.DataSource = CaminhoArquivoDados;
            SQLiteConnection conn = new SQLiteConnection(sb.ConnectionString);
            conn.Open();

            string sql = "select versao from ibpt limit 1";

            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            string versao = cmd.ExecuteScalar().ToString();
            conn.Clone();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("VERSAO DO IBPT: " + versao);
            Console.ForegroundColor = ConsoleColor.DarkGray;
        }
    }
}

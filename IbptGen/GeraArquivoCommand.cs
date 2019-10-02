using System;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace IbptGen
{
    public class GeraArquivoCommand : IBPTCommand
    {
        public string CaminhoCsv { get; }

        private SQLiteConnection Conn { get; set; }
        private void AbreConexao()
        {
            string file = $@".\IBPT.sqlite3";
            if (File.Exists(file))
                File.Delete(file);
            SQLiteConnection.CreateFile(file);

            SQLiteConnectionStringBuilder sb = new SQLiteConnectionStringBuilder();
            sb.DataSource = file;

            Conn = new SQLiteConnection(sb.ConnectionString);
            Conn.Open();
        }

        private void CriaTabela()
        {
            string sql = @"CREATE TABLE IBPT (
    ncm       TEXT (40)       NOT NULL,
    federal   DECIMAL (10, 2) NOT NULL,
    estadual  DECIMAL (10, 2) NOT NULL,
    municipal DECIMAL (10, 2) NOT NULL,
    importado DECIMAL (10, 2) NOT NULL,
    versao    TEXT (30)       NOT NULL
);";

            SQLiteCommand cmd = new SQLiteCommand(sql, Conn);
            cmd.ExecuteNonQuery();
        }

        public GeraArquivoCommand(string caminhoCsv)
        {
            CaminhoCsv = caminhoCsv;
        }

        public void Gerar()
        {
            CriaTabela();
        }

        private void PrintProgress(double percent)
        {
            Console.Clear();
            Console.WriteLine($"Criando arquivo de dados...");
            Console.WriteLine($"{percent.ToString("N2")}% concluído");

            string bars = "";
            int barsInt = (((int)percent) / 10);
            for (int i = 0; i < barsInt; i++)
                bars += "==";
            if (barsInt < 10)
                bars += ">";

            Console.WriteLine($"[{bars}]");
        }

        private DataTable GetDataTable()
        {
            string fileName = CaminhoCsv;
            string[] content = File.ReadAllLines(fileName);
            if (content.Length == 0)
                throw new Exception("Não existem dados para serem visualizados");

            var DataTable = new DataTable();
            string[] columns = content[0].Split(';');
            for (int i = 0; i < columns.Length; i++)
            {
                string columnName = columns[i];
                DataTable.Columns.Add(columnName);
            }

            for (int i = 1; i < content.Length; i++)
            {
                string[] rowContent = content[i].Split(';');
                DataTable.Rows.Add(rowContent);
            }

            return DataTable;
        }

        public void Execute()
        {
            AbreConexao();
            CriaTabela();

            var dt = GetDataTable();
            SQLiteTransaction tx = Conn.BeginTransaction();

            string sql = "";
            int c = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var row = dt.Rows[i];

                string ncm = row["codigo"].ToString();
                string federal = row["nacionalfederal"].ToString();
                string estadual = row["estadual"].ToString();
                string municipal = row["municipal"].ToString();
                string importado = row["importadosfederal"].ToString();
                string versao = row["versao"].ToString();

                sql += $@"insert into ibpt(ncm, federal, estadual, municipal, importado, versao) values 
('{ncm}', {federal}, {estadual}, {municipal}, {importado}, '{versao}');
";
                c += 1;
                if (c == 1000)
                {
                    SQLiteCommand cmd = new SQLiteCommand(sql, Conn, tx);
                    cmd.ExecuteNonQuery();
                    c = 0;
                    sql = "";
                    PrintProgress(((double)i / dt.Rows.Count) * 100);
                }
            }

            PrintProgress(100);
            tx.Commit();
            Conn.Close();
            Conn.Dispose();
        }
    }
}

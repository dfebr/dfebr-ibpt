using System;
using System.Data.SQLite;
using System.IO;

namespace DFeBR.IBPT
{
    public class BuscaIBPT
    {
        private string caminhoArquivoDados = null;

        /// <summary>
        /// A busca é baseada em um arquivo de dados
        /// Sqlite3, que deve ser previamente gerado utilizando
        /// a ferramenta de linha de comando "IbptGen.exe"
        /// </summary>
        /// <param name="caminhoArquivo">Caminho do arquivo de dados .sqlite3</param>
        public BuscaIBPT(string caminhoArquivo)
        {
            if (!File.Exists(caminhoArquivo))
                throw new FileNotFoundException("O arquivo de dados não foi localizado ou não existe");

            caminhoArquivoDados = caminhoArquivo;
        }

        /// <summary>
        /// Realiza a busca de aliquotas IBPT
        /// </summary>
        /// <param name="ncm">A busca será realizada mediante o NCM</param>
        public AliquotaIBPT Buscar(string ncm)
        {
            if (!File.Exists(caminhoArquivoDados))
                throw new FileNotFoundException("O arquivo de dados não foi localizado ou não existe");

            Cache<AliquotaIBPT> cached = CacheRepository<AliquotaIBPT>.Get(ncm);
            if (cached != null)
                return cached.Value;

            SQLiteConnectionStringBuilder sb = new SQLiteConnectionStringBuilder();
            sb.DataSource = caminhoArquivoDados;

            AliquotaIBPT result = null;
            using (SQLiteConnection conn = new SQLiteConnection(sb.ConnectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand("select * from ibpt where ncm = @ncmId limit 1", conn);
                cmd.Parameters.AddWithValue("@ncmId", ncm);

                using (var reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    if(reader.HasRows)
                    {
                        result = new AliquotaIBPT();

                        result.NCM = reader.GetString(0);
                        result.AliquotaFederal = reader.GetDecimal(1);
                        result.AliquotaEstadual = reader.GetDecimal(2);
                        result.AliquotaMunicipal = reader.GetDecimal(3);
                        result.AliquotaImportado = reader.GetDecimal(4);
                        result.Versao = reader.GetString(5);
                    }
                }

                conn.Close();
            }

            if (result != null)
                CacheRepository<AliquotaIBPT>.Set(ncm, result, 360);
            return result;
        }
    }
}

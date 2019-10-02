using System;

namespace IbptGen
{
    class Program
    {
        static void PrintSplash()
        {
            string splash = @"*** DFeBR.NET ***
Utilitário de geração do arquivo de dados para o DFeBR-IBPT

Comandos:
    --version caminhoArquivoDados : exibe a versão dos dados IBPT gravados no arquivo de dados
    --gen caminhoCSV : gera um novo arquivo de dados para o DFeBR-IBPT no baseado no CSV oficial do IBPT. O arquivo será gerado no diretório deste executável


";

            Console.WriteLine(splash);
        }

        static void Main(string[] args)
        {
            PrintSplash();

            IBPTCommand cmd = GetCommand(args);
            if (cmd != null)
                cmd.Execute();
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Nenhum parametro foi declarado");
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Pressione uma tecla para encerrar");
            Console.ReadKey();
        }

        static IBPTCommand GetCommand(string[] args)
        {
            if (args.Length == 0)
                return null;

            if(args.Length == 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("A contagem de parametros está incompleta");
                return null;
            }

            if (args[0] == "--gen")
                return new GeraArquivoCommand(args[1]);
            if (args[0] == "--version")
                return new VersaoArquivoCommand(args[1]);
            return null;
        }

        private static void GeraArquivoDados(string caminhoCSV)
        {

        }
    }
}

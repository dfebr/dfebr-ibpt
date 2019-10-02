using Microsoft.VisualStudio.TestTools.UnitTesting;
using DFeBR.IBPT;

namespace DFeBR.IBPT.Tests
{
    [TestClass]
    public class BuscaIBPTTest
    {
        /**
         * REQUISITOS PARA TESTES:
         * 
         * Gerar o arquivo de dados .sqlite3 baseado na tabela
         * IBPT.csv do seu estado.
         * 
         * Aqui, foi usado o IBPT 19.2A do estado do RJ
         * */
        [TestMethod]
        public void DEVE_RETORNAR_ALIQUOTA_6_84()
        {
            BuscaIBPT busca = new BuscaIBPT(@".\IBPT.sqlite3");
            AliquotaIBPT aliquota = busca.Buscar("01013000");

            Assert.IsTrue(aliquota != null);
            Assert.IsTrue(aliquota.AliquotaEstadual == 6.84m);
        }
    }
}

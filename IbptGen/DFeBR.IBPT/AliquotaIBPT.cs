namespace DFeBR.IBPT
{
    public sealed class AliquotaIBPT
    {
        public string NCM { get; internal set; }

        public decimal AliquotaFederal { get; internal set; }

        public decimal AliquotaEstadual { get; internal set; }

        public decimal AliquotaMunicipal { get; internal set; }

        public decimal AliquotaImportado { get; internal set; }

        public string Versao { get; internal set; }

        internal AliquotaIBPT()
        {

        }
    }
}

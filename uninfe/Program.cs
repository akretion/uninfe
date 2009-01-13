using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace uninfe
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ConfigUniNFe oConfig = new ConfigUniNFe();
            oConfig.CarregarDados();

            // Pegar o nome da empresa do certificado para utilizar na verificação do Mutex
            // Wandrey 27/11/2008
            String nomeEmpresaCertificado = "UNINFE";
            if (oConfig.oCertificado != null)
            {
                nomeEmpresaCertificado = oConfig.oCertificado.Subject;
                int nPosI = nomeEmpresaCertificado.ToUpper().IndexOf("CN=");
                if (nPosI >= 0)
                {
                    int nPosF = nomeEmpresaCertificado.ToUpper().IndexOf(",", nPosI);
                    if (nPosF <= 0)
                    {
                        nPosF = nomeEmpresaCertificado.Length;
                    }
                    nomeEmpresaCertificado = nomeEmpresaCertificado.Substring(nPosI + 3, nPosF - 3).Trim().ToUpper();
                }                    
                else                    
                {
                    //Não achou o "CN=" na string do nome do certificado, 
                    //então não será possível pegar o nome da empresa com cnpj
                    nomeEmpresaCertificado = "UNINFE";
                }
            }

            // Verificar se o aplicativo já está rodando. Se estiver vai emitir um aviso e abortar
            // Pois só pode executar o aplicativo uma única vez por certificado/CNPJ.
            // Wandrey 27/11/2008
            System.Threading.Mutex oneMutex = null;
            String nomeMutex = nomeEmpresaCertificado;

            try
            {
                oneMutex = System.Threading.Mutex.OpenExisting(nomeMutex);
            }
            catch (System.Threading.WaitHandleCannotBeOpenedException)
            {

            }

            if (oneMutex == null)
            {
                oneMutex = new System.Threading.Mutex(false, nomeMutex);
            }
            else
            {
                MessageBox.Show("Já existe uma instância do UniNFe em execução.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                oneMutex.Close();

                return;
                //Environment.Exit(0); //Forçar a aplicação ser encerrada; 
            }

            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
            finally 
            {
                if (oneMutex != null)
                {
                    oneMutex.Close();
                }
            }
        }
    }
}

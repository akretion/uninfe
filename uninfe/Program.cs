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
            String nomePastaEnvio = "";
            String nomePastaEnvioDemo = "";
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

                //Acrescentar a pasta de envio ao Mutex, se for diferente ele vai permitir a execução do uninfe
                if (oConfig.vPastaXMLEnvio != "")
                {
                    nomePastaEnvio = oConfig.vPastaXMLEnvio;

                    //Tirar a unidade e os dois pontos do nome da pasta
                    int iPos = nomePastaEnvio.IndexOf(':') + 1;
                    if (iPos >= 0)
                    {
                        nomePastaEnvio = nomePastaEnvio.Substring(iPos, nomePastaEnvio.Length - iPos);
                    }
                    nomePastaEnvioDemo = nomePastaEnvio;

                    //tirar as barras de separação de pastas e subpastas
                    nomePastaEnvio = nomePastaEnvio.Replace("\\", "").Replace("/","").ToUpper();
                }
            }

            // Verificar se o aplicativo já está rodando. Se estiver vai emitir um aviso e abortar
            // Pois só pode executar o aplicativo uma única vez por certificado/CNPJ.
            // Wandrey 27/11/2008
            System.Threading.Mutex oneMutex = null;
            String nomeMutex = nomeEmpresaCertificado.Trim()+nomePastaEnvio.Trim();

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
                MessageBox.Show("Somente uma instância do UniNFe pode ser executada com o seguintes dados configurados:\r\n\r\n"+
                                "Certificado: "+nomeEmpresaCertificado+"\r\n\r\n"+
                                "Pasta Envio: "+nomePastaEnvioDemo+"\r\n\r\n"+
                                "Já tem uma instância com estes dados em execução.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                oneMutex.Close();

                return;
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

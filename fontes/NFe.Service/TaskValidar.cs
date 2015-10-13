using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using NFe.Components;
using NFe.Settings;

namespace NFe.Service
{
    public class TaskValidar : TaskAbst
    {
        public override void Execute()
        {
            int emp = Empresas.FindEmpresaByThread();

            if (NomeArquivoXML.EndsWith(Propriedade.ExtEnvio.EnvCCe_XML, StringComparison.InvariantCultureIgnoreCase) ||
                NomeArquivoXML.EndsWith(Propriedade.ExtEnvio.EnvCancelamento_XML, StringComparison.InvariantCultureIgnoreCase) ||
                NomeArquivoXML.EndsWith(Propriedade.ExtEnvio.EnvManifestacao_XML, StringComparison.InvariantCultureIgnoreCase) ||
                NomeArquivoXML.EndsWith(Propriedade.ExtEnvio.PedEve, StringComparison.InvariantCultureIgnoreCase))
            {
                Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno, Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.ExtEnvio.EnvCCe_XML) + Propriedade.ExtRetorno.retEnvCCe_ERR));
                Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno, Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.ExtEnvio.EnvCancelamento_XML) + Propriedade.ExtRetorno.retCancelamento_ERR));
                Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno, Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.ExtEnvio.EnvManifestacao_XML) + Propriedade.ExtRetorno.retManifestacao_ERR));
                Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno, Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.ExtEnvio.PedEve) + Propriedade.ExtRetorno.Eve_ERR));

                DadosenvEvento eve = new DadosenvEvento();
                EnvEvento(emp, eve);
            }

            if (NomeArquivoXML.EndsWith(Propriedade.ExtEnvio.PedSit_XML, StringComparison.InvariantCultureIgnoreCase))
            {
                Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlErro, Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.ExtEnvio.PedSit_XML)));
                Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno, Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.ExtEnvio.PedSit_XML) + Propriedade.ExtRetorno.Sit_ERR));
                
                DadosPedSit sit = new DadosPedSit();
                PedSit(emp, sit);
            }

            if (NomeArquivoXML.EndsWith(Propriedade.ExtEnvio.PedSta_XML, StringComparison.InvariantCultureIgnoreCase))
            {
                Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno, Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.ExtEnvio.PedSta_XML) + Propriedade.ExtRetorno.Sta_ERR));
                Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno, Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.ExtEnvio.PedSta_XML) + "-ped-sta-ret.xml"));

                DadosPedSta sta = new DadosPedSta();
                PedSta(emp, sta);
            }
        }
    }
}

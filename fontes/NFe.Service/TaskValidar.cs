using NFe.Components;
using NFe.Settings;
using System;
using System.IO;

namespace NFe.Service
{
    public class TaskValidar : TaskAbst
    {
        public override void Execute()
        {
            int emp = Empresas.FindEmpresaByThread();
            ///
            /// danasa: 06-2018
            ConteudoXML.PreserveWhitespace = false;
            ConteudoXML.Load(this.NomeArquivoXML);

            if (NomeArquivoXML.EndsWith(Propriedade.Extensao(Propriedade.TipoEnvio.EnvCCe).EnvioXML, StringComparison.InvariantCultureIgnoreCase) ||
                NomeArquivoXML.EndsWith(Propriedade.Extensao(Propriedade.TipoEnvio.EnvCancelamento).EnvioXML, StringComparison.InvariantCultureIgnoreCase) ||
                NomeArquivoXML.EndsWith(Propriedade.Extensao(Propriedade.TipoEnvio.EnvManifestacao).EnvioXML, StringComparison.InvariantCultureIgnoreCase) ||
                NomeArquivoXML.EndsWith(Propriedade.Extensao(Propriedade.TipoEnvio.PedEve).EnvioXML, StringComparison.InvariantCultureIgnoreCase))
            {
                if (NomeArquivoXML.EndsWith(Propriedade.Extensao(Propriedade.TipoEnvio.EnvCCe).EnvioXML, StringComparison.InvariantCultureIgnoreCase))
                    Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno, Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.EnvCCe).EnvioXML) + Propriedade.ExtRetorno.retEnvCCe_ERR));
                if (NomeArquivoXML.EndsWith(Propriedade.Extensao(Propriedade.TipoEnvio.EnvCancelamento).EnvioXML, StringComparison.InvariantCultureIgnoreCase))
                    Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno, Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.EnvCancelamento).EnvioXML) + Propriedade.ExtRetorno.retCancelamento_ERR));
                if (NomeArquivoXML.EndsWith(Propriedade.Extensao(Propriedade.TipoEnvio.EnvManifestacao).EnvioXML, StringComparison.InvariantCultureIgnoreCase))
                    Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno, Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.EnvManifestacao).EnvioXML) + Propriedade.ExtRetorno.retManifestacao_ERR));
                if (NomeArquivoXML.EndsWith(Propriedade.Extensao(Propriedade.TipoEnvio.PedEve).EnvioXML, StringComparison.InvariantCultureIgnoreCase))
                    Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno, Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedEve).EnvioXML) + Propriedade.ExtRetorno.Eve_ERR));

                DadosenvEvento eve = new DadosenvEvento();
                EnvEvento(emp, eve);
            }

            if (NomeArquivoXML.EndsWith(Propriedade.Extensao(Propriedade.TipoEnvio.PedSit).EnvioXML, StringComparison.InvariantCultureIgnoreCase))
            {
                Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlErro, Path.GetFileName(NomeArquivoXML)));
                Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno, Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedSit).EnvioXML) + Propriedade.ExtRetorno.Sit_ERR));

                DadosPedSit sit = new DadosPedSit();
                PedSit(emp, sit);
            }

            if (NomeArquivoXML.EndsWith(Propriedade.Extensao(Propriedade.TipoEnvio.PedSta).EnvioXML, StringComparison.InvariantCultureIgnoreCase))
            {
                var fn = Functions.ExtrairNomeArq(NomeArquivoXML, Propriedade.Extensao(Propriedade.TipoEnvio.PedSta).EnvioXML);
                Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno, fn + Propriedade.ExtRetorno.Sta_ERR));
                Functions.DeletarArquivo(Path.Combine(Empresas.Configuracoes[emp].PastaXmlRetorno, fn + Propriedade.Extensao(Propriedade.TipoEnvio.PedSta).RetornoXML));//"-ped-sta-ret.xml"));

                DadosPedSta sta = new DadosPedSta();
                PedSta(emp, sta);
            }
        }
    }
}
using Unimake.Business.DFe.Security;
using Unimake.Business.DFe.Utility;
using Unimake.Business.DFe.Xml.NFe;

namespace Unimake.Business.DFe.Servicos.NFCe
{
    public class Autorizacao : NFe.Autorizacao
    {
        public Autorizacao(EnviNFe enviNFe, Configuracao configuracao)
                      : base(enviNFe, configuracao) { }

        public override void Executar()
        {
            new AssinaturaDigital().Assinar(ConteudoXML, Configuracoes.TagAssinatura, Configuracoes.TagAtributoID, Configuracoes.CertificadoDigital, AlgorithmType.Sha1, true, "", "Id");
            EnviNFe = EnviNFe.LerXML<EnviNFe>(ConteudoXML);

            MontarQrCode();

            base.Executar();
        }

        /// <summary>
        /// Definir as propriedades do QRCode e Link da consulta manual da NFCe
        /// </summary>
        private void MontarQrCode()
        {
            for (var i = 0; i < EnviNFe.NFe.Length; i++)
            {
                if (EnviNFe.NFe[i].InfNFeSupl == null)
                {
                    EnviNFe.NFe[i].InfNFeSupl = new InfNFeSupl();

                    var urlQrCode = (Configuracoes.TipoAmbiente == TipoAmbiente.Homologacao ? Configuracoes.UrlQrCodeHomologacao : Configuracoes.UrlQrCodeProducao);
                    var urlChave = (Configuracoes.TipoAmbiente == TipoAmbiente.Homologacao ? Configuracoes.UrlChaveHomologacao : Configuracoes.UrlChaveProducao);
                    var paramLinkQRCode = string.Empty;

                    if (EnviNFe.NFe[i].InfNFe[0].Ide.TpEmis == TipoEmissao.ContingenciaOffLine)
                    {
                        paramLinkQRCode = EnviNFe.NFe[i].InfNFe[0].Chave + "|" +
                            "2" + "|" +
                            ((int)EnviNFe.NFe[i].InfNFe[0].Ide.TpAmb).ToString() + "|" +
                            EnviNFe.NFe[i].InfNFe[0].Ide.DhEmi.ToString("dd") + "|" +
                            EnviNFe.NFe[i].InfNFe[0].Total.ICMSTot.VNFField.Trim() + "|" +
                            Converter.ToHexadecimal(EnviNFe.NFe[i].Signature.SignedInfo.Reference.DigestValue.ToString()) + "|" +
                            Configuracoes.CSCIDToken.ToString();
                    }
                    else
                    {
                        paramLinkQRCode = EnviNFe.NFe[i].InfNFe[0].Chave + "|" +
                            "2" + "|" +
                            ((int)EnviNFe.NFe[i].InfNFe[0].Ide.TpAmb).ToString() + "|" +
                            Configuracoes.CSCIDToken.ToString();
                    }

                    string hashQRCode = Converter.ToSHA1HashData(paramLinkQRCode.Trim() + Configuracoes.CSC, true);

                    EnviNFe.NFe[i].InfNFeSupl.QrCode = urlQrCode + "?p=" + paramLinkQRCode.Trim() + "|" + hashQRCode.Trim();
                    EnviNFe.NFe[i].InfNFeSupl.UrlChave = urlChave;
                }
            }

            //Atualizar a propriedade do XML da NFCe novamente com o conteúdo atual já a tag de QRCode e link de consulta
            ConteudoXML = EnviNFe.GerarXML();
        }
    }
}

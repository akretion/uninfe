using NFe.Components;
using System;
using System.Globalization;
using System.Text;
using System.Xml;

namespace NFe.ConvertTxt
{
    public class nfeRead
    {
        private XmlDocument doc;

        public NFe nfe { get; private set; }

        public string XmlNota { get; private set; }

        public nfeRead()
        {
            nfe = new NFe();
        }

        private DateTime readDate(object element, TpcnResources tag)
        {
            string temp = this.readValue(element, tag).Trim();
            return (temp != "" ? Convert.ToDateTime(temp) : DateTime.MinValue);
        }

        private double readDouble(object element, TpcnResources tag)
        {
            char charSeparator = System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator[0];
            return Convert.ToDouble("0" + this.readValue(element, tag).Replace(".", charSeparator.ToString()));
        }

        private Int32 readInt32(object element, TpcnResources tag)
        {
            return Convert.ToInt32("0" + this.readValue(element, tag));
        }

        private string readValue(object element, TpcnResources tag)
        {
            return this.readValue(element, tag.ToString());
        }

        private string readValue(object element, string tag)
        {
            if(((XmlElement)element).GetElementsByTagName(tag)[0] != null)
                return ReverterFiltroTextoXML(((XmlElement)element).GetElementsByTagName(tag)[0].InnerText);

            Console.WriteLine($"TAG: {tag} nao encontrada no element: {element}");

            return "";
        }

        private string ReverterFiltroTextoXML(string aTexto)
        {
            aTexto = aTexto.Replace("&amp;", "&");
            aTexto = aTexto.Replace("&lt;", "<");
            aTexto = aTexto.Replace("&gt;", ">");
            aTexto = aTexto.Replace("&quot;", "\"");
            aTexto = aTexto.Replace("&#39;", "'");
            return aTexto.Trim();
        }

        public void ReadFromString(string xmlString)
        {
            doc = new XmlDocument();
            try
            {
                doc.LoadXml(xmlString);
                this.XmlNota = doc.OuterXml;

                foreach(XmlNode nodeRoot in doc.ChildNodes)
                {
                    if(nodeRoot.LocalName.Equals("NFe"))
                    {
                        processaNFe(nodeRoot);
                    }
                    if(nodeRoot.LocalName.Equals("nfeProc"))
                    {
                        foreach(XmlNode nodenfeProc in nodeRoot.ChildNodes)
                        {
                            switch(nodenfeProc.LocalName.ToLower())
                            {
                                case "nfe":
                                    processaNFe(nodenfeProc);
                                    break;

                                case "protnfe":
                                    processaProtNfe(nodenfeProc);
                                    break;
                            }
                        }
                    }
                }
                if(!string.IsNullOrEmpty(nfe.ide.dhEmi) && nfe.infNFe.Versao < 3)
                    throw new Exception("Arquivo não é de nota fiscal NF-e");

                if(string.IsNullOrEmpty(nfe.ide.dhEmi) && nfe.infNFe.Versao >= 3)
                    if(nfe.ide.mod == TpcnMod.modNFCe)
                        throw new Exception("Arquivo não é de nota fiscal NFC-e");
                    else
                        throw new Exception("Arquivo não é de nota fiscal NF-e");
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                doc = null;
            }
        }

        public void ReadFromXml(string xmlFilename)
        {
            ReadFromString(System.IO.File.ReadAllText(xmlFilename, Encoding.UTF8));
        }

        DateTime FromHex(string value)
        {
            string result = "";
            for(int i = 0; i < value.Length; i += 2)
            {
                string hs = value.Substring(i, 2);
                result += Convert.ToChar(Convert.ToUInt32(hs, 16));
            }
            return Convert.ToDateTime(result);
        }

        private void processaNFe(XmlNode nodeRoot)
        {
            foreach(XmlNode nodeNFe in nodeRoot.ChildNodes)
            {
                switch(nodeNFe.LocalName.ToLower())
                {
                    case "infnfesupl":
                        //<qrCode><![CDATA[http://www.sefaz.mt.gov.br/nfce/consultanfce?chNFe=51160417625687000153650020000006611000006615&nVersao=100&tpAmb=1&cDest=32622775091&dhEmi=323031362d30342d32375431353a31343a32362d30343a3030&vNF=2.00&vICMS=0.00&digVal=7169746e59786b51753950376e4e716536686e3859596b327354773d&cIdToken=000001&cHashQRCode=C0C48B5D9353FEDDBC40E4ECBF931D32E95D977B]]></qrCode>
                        foreach(XmlNode nodeinfNFe in nodeNFe.ChildNodes)
                        {
                            var minhaCultura = new CultureInfo("pt-BR"); //pt-BR usada como base
                            minhaCultura.NumberFormat.NumberDecimalSeparator = ".";

                            var data = nodeinfNFe.InnerText.Replace("<![CDATA[", "").Replace("]]>", "");
                            nfe.qrCode.Link = data;
                            var split = data.Split(new char[] { '&' });

                            if(split[0].Contains("chNFe"))
                            {
                                foreach(var s in split)
                                {
                                    Console.WriteLine(s);
                                    if(s.Contains("chNFe"))
                                        nfe.qrCode.chNFe = s.Split('?')[1].Substring(6);
                                    else if(s.StartsWith("nVersao")) nfe.qrCode.nVersao = s.Split('=')[1];
                                    else if(s.StartsWith("tpAmb")) nfe.qrCode.tpAmb = (TipoAmbiente)Convert.ToInt16(s.Split('=')[1]);
                                    else if(s.StartsWith("cDest")) nfe.qrCode.cDest = s.Split('=')[1];
                                    else if(s.StartsWith("digVal")) nfe.qrCode.digVal = s.Split('=')[1];
                                    else if(s.StartsWith("cIdToken")) nfe.qrCode.cIdToken = s.Split('=')[1];
                                    else if(s.StartsWith("cHashQRCode")) nfe.qrCode.cHashQRCode = s.Split('=')[1];
                                    else if(s.StartsWith("vICMS")) nfe.qrCode.vICMS = Convert.ToDecimal(s.Split('=')[1], minhaCultura);
                                    else if(s.StartsWith("vNF")) nfe.qrCode.vNF = Convert.ToDecimal(s.Split('=')[1], minhaCultura);
                                    else if(s.StartsWith("dhEmi")) nfe.qrCode.dhEmi = FromHex(s.Split('=')[1]);
                                }
                            }
                            else
                            {
                                split = data.Split(new char[] { '|' });
                                nfe.qrCode.chNFe = split[0].Split('?')[1].Substring(2);
                                nfe.qrCode.nVersao = split[1].PadRight(3, '0');
                                nfe.qrCode.tpAmb = (TipoAmbiente)Convert.ToInt16(split[2]);

                                if(nfe.ide.tpEmis.Equals(TipoEmissao.teNormal))
                                {
                                    nfe.qrCode.cIdToken = split[3].PadLeft(6, '0');
                                    nfe.qrCode.cHashQRCode = split[4];
                                }
                                else if(nfe.ide.tpEmis.Equals(TipoEmissao.teOffLine))
                                {
                                    var dataHoraEmissao = Convert.ToDateTime(nfe.ide.dhEmi);
                                    nfe.qrCode.dhEmi = new DateTime(dataHoraEmissao.Year, dataHoraEmissao.Month, Convert.ToInt16(split[3]));
                                    nfe.qrCode.vNF = Convert.ToDecimal(split[4], minhaCultura);
                                    nfe.qrCode.digVal = split[5];
                                    nfe.qrCode.cIdToken = split[6].PadLeft(6, '0');
                                    nfe.qrCode.cHashQRCode = split[7];
                                }
                            }

                            break;
                        }
                        break;

                    case "infnfe":
                        char charSeparator = System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator[0];
                        nfe.infNFe.Versao = Convert.ToDecimal("0" + nodeNFe.Attributes[TpcnResources.versao.ToString()].Value.Replace(".", charSeparator.ToString()));
                        nfe.infNFe.ID = nodeNFe.Attributes[TpcnResources.Id.ToString()].Value;
                        foreach(XmlNode nodeinfNFe in nodeNFe.ChildNodes)
                        {
                            switch(nodeinfNFe.LocalName)
                            {
                                case "ide":
                                    this.processaIDE(nodeinfNFe);
                                    break;

                                case "emit":
                                    this.processaEMIT(nodeinfNFe);
                                    break;

                                case "dest":
                                    this.processaDEST(nodeinfNFe);
                                    break;

                                case "det":
                                    this.processaPROD(nodeinfNFe);
                                    break;

                                case "total":
                                    this.processaTOTAL(nodeinfNFe);
                                    break;

                                case "transp":
                                    this.processaTRANSP(nodeinfNFe);
                                    break;

                                case "cobr":
                                    this.processaCOBR(nodeinfNFe);
                                    break;

                                case "retirada":
                                    processaRetirada(nodeinfNFe);
                                    break;

                                case "entrega":
                                    processaEntrega(nodeinfNFe);
                                    break;

                                case "autXML":
                                    processaautXML(nodeinfNFe);
                                    break;

                                case "infIntermed":
                                    processaInfIntermed(nodeinfNFe);
                                    break;

                                case "infAdic":
                                    processaInfAdic(nodeinfNFe);
                                    break;

                                case "exporta":
                                    processaExporta(nodeinfNFe);
                                    break;

                                case "compra":
                                    processaCompra(nodeinfNFe);
                                    break;

                                case "cana":
                                    processaCana(nodeinfNFe);
                                    break;

                                case "pag":
                                    processaPag(nodeinfNFe);
                                    break;

                                case "infRespTec":
                                    processaRespTec(nodeinfNFe);
                                    break;
                            }
                        }
                        break;
                }
            }
        }

        private void processaInfIntermed(XmlNode nodeIntIntermed)
        {
            nfe.InfIntermed.CNPJ = readValue(nodeIntIntermed, nameof(InfIntermed.CNPJ));
            nfe.InfIntermed.idCadIntTran = readValue(nodeIntIntermed, nameof(InfIntermed.idCadIntTran));
        }

        /// <summary>
        /// processaPag
        /// </summary>
        /// <param name="nodenfeProc"></param>
        private void processaPag(XmlNode nodenfeProc)
        {
            foreach(XmlNode noder in nodenfeProc.ChildNodes)
            {
                switch(noder.LocalName)
                {
                    case "detPag":
                        LerPag(noder);
                        break;
                }
            }
        }

        private void processaRespTec(XmlNode noderesptec)
        {
            nfe.resptecnico.CNPJ = this.readValue(noderesptec, nameof(RespTecnico.CNPJ));
            nfe.resptecnico.xContato = this.readValue(noderesptec, nameof(RespTecnico.xContato));
            nfe.resptecnico.email = this.readValue(noderesptec, nameof(RespTecnico.email));
            nfe.resptecnico.fone = this.readValue(noderesptec, nameof(RespTecnico.fone));
            nfe.resptecnico.idCSRT = Convert.ToInt32("0" + this.readValue(noderesptec, nameof(RespTecnico.idCSRT)));
            nfe.resptecnico.hashCSRT = this.readValue(noderesptec, nameof(RespTecnico.hashCSRT));
        }

        private void LerPag(XmlNode noder)
        {
            pag pagItem = new pag();

            pagItem.indPag = (TpcnIndicadorPagamento)this.readInt32(noder, TpcnResources.indPag);
            pagItem.tPag = (TpcnFormaPagamento)this.readInt32(noder, TpcnResources.tPag);
            pagItem.vPag = this.readDouble(noder, TpcnResources.vPag);
            pagItem.tpIntegra = this.readInt32(noder, TpcnResources.tpIntegra);
            pagItem.CNPJ = this.readValue(noder, TpcnResources.CNPJ);
            pagItem.tBand = (TpcnBandeiraCartao)this.readInt32(noder, TpcnResources.tBand);
            pagItem.cAut = this.readValue(noder, TpcnResources.cAut);
            pagItem.vTroco = this.readDouble(noder, TpcnResources.vTroco); //Somente para manter compatibilidade do layout TXT, de futuro, em uma mudança geral da SEFAZ, podemos retirar. Wandrey 26/01/2021
            nfe.vTroco = this.readDouble(noder, TpcnResources.vTroco);
            nfe.pag.Add(pagItem);
        }

        /// <summary>
        /// processaProtNfe
        /// </summary>
        /// <param name="nodenfeProc"></param>
        private void processaProtNfe(XmlNode nodenfeProc)
        {
            nfe.protNFe.chNFe = this.readValue(nodenfeProc, TpcnResources.chNFe);
            nfe.protNFe.cStat = this.readInt32(nodenfeProc, TpcnResources.cStat);
            nfe.protNFe.dhRecbto = this.readDate(nodenfeProc, TpcnResources.dhRecbto);
            nfe.protNFe.digVal = this.readValue(nodenfeProc, TpcnResources.digVal.ToString());
            nfe.protNFe.nProt = this.readValue(nodenfeProc, TpcnResources.nProt.ToString());
            nfe.protNFe.tpAmb = (TipoAmbiente)this.readInt32(nodenfeProc, TpcnResources.tpAmb);
            nfe.protNFe.verAplic = this.readValue(nodenfeProc, TpcnResources.verAplic);
            nfe.protNFe.xMotivo = this.readValue(nodenfeProc, TpcnResources.xMotivo);

            nfe.protNFe.cMsg = Convert.ToInt32("0" + this.readValue(nodenfeProc, "cMsg"));
            nfe.protNFe.xMsg = this.readValue(nodenfeProc, "xMsg");
        }

        /// <summary>
        /// processaCana
        /// </summary>
        /// <param name="nodeinfNFe"></param>
        private void processaCana(XmlNode nodeinfNFe)
        {
            nfe.cana.safra = this.readValue(nodeinfNFe, TpcnResources.safra);
            nfe.cana.Ref = this.readValue(nodeinfNFe, TpcnResources.Ref);
            nfe.cana.qTotMes = this.readDouble(nodeinfNFe, TpcnResources.qTotMes);
            nfe.cana.qTotAnt = this.readDouble(nodeinfNFe, TpcnResources.qTotAnt);
            nfe.cana.qTotGer = this.readDouble(nodeinfNFe, TpcnResources.qTotGer);
            nfe.cana.vFor = this.readDouble(nodeinfNFe, TpcnResources.vFor);
            nfe.cana.vTotDed = this.readDouble(nodeinfNFe, TpcnResources.vTotDed);
            nfe.cana.vLiqFor = this.readDouble(nodeinfNFe, TpcnResources.vLiqFor);

            foreach(XmlNode noder in nodeinfNFe.ChildNodes)
            {
                switch(noder.LocalName)
                {
                    case "forDia":
                        {
                            fordia fordiaInfo = new fordia();
                            if(noder.Attributes.Count > 0)
                                fordiaInfo.dia = Convert.ToInt32(noder.Attributes[TpcnResources.dia.ToString()].Value);
                            else
                                fordiaInfo.dia = this.readInt32(noder, TpcnResources.dia);
                            fordiaInfo.qtde = this.readDouble(noder, TpcnResources.qtde);
                            nfe.cana.fordia.Add(fordiaInfo);
                        }
                        break;

                    case "deduc":
                        {
                            deduc deducInfo = new deduc();
                            deducInfo.xDed = this.readValue(noder, TpcnResources.xDed);
                            deducInfo.vDed = this.readDouble(noder, TpcnResources.vDed);
                            nfe.cana.deduc.Add(deducInfo);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// processaCompra
        /// </summary>
        /// <param name="nodeinfNFe"></param>
        private void processaCompra(XmlNode nodeinfNFe)
        {
            nfe.compra.xNEmp = this.readValue(nodeinfNFe, TpcnResources.xNEmp);
            nfe.compra.xPed = this.readValue(nodeinfNFe, TpcnResources.xPed);
            nfe.compra.xCont = this.readValue(nodeinfNFe, TpcnResources.xCont);
        }

        /// <summary>
        /// processaExporta
        /// </summary>
        /// <param name="nodeinfNFe"></param>
        private void processaExporta(XmlNode nodeinfNFe)
        {
            nfe.exporta.UFEmbarq = this.readValue(nodeinfNFe, TpcnResources.UFEmbarq);
            nfe.exporta.xLocEmbarq = this.readValue(nodeinfNFe, TpcnResources.xLocEmbarq);

            // Versao 3.10
            nfe.exporta.UFSaidaPais = this.readValue(nodeinfNFe, TpcnResources.UFSaidaPais);
            nfe.exporta.xLocExporta = this.readValue(nodeinfNFe, TpcnResources.xLocExporta);
            nfe.exporta.xLocDespacho = this.readValue(nodeinfNFe, TpcnResources.xLocDespacho);
        }

        /// <summary>
        /// processaInfAdic
        /// </summary>
        /// <param name="nodeinfNFe"></param>
        private void processaInfAdic(XmlNode nodeinfNFe)
        {
            nfe.InfAdic.infAdFisco = this.readValue(nodeinfNFe, TpcnResources.infAdFisco);
            nfe.InfAdic.infCpl = this.readValue(nodeinfNFe, TpcnResources.infCpl);
            foreach(XmlNode noder in nodeinfNFe.ChildNodes)
            {
                switch(noder.LocalName)
                {
                    case "obsCont":
                        {
                            obsCont obscontInfo = new obsCont();
                            obscontInfo.xCampo = noder.Attributes[TpcnResources.xCampo.ToString()].Value;
                            obscontInfo.xTexto = this.readValue(noder, TpcnResources.xTexto);
                            nfe.InfAdic.obsCont.Add(obscontInfo);
                        }
                        break;

                    case "obsFisco":
                        {
                            obsFisco obsfiscoInfo = new obsFisco();
                            obsfiscoInfo.xCampo = noder.Attributes[TpcnResources.xCampo.ToString()].Value;
                            obsfiscoInfo.xTexto = this.readValue(noder, TpcnResources.xTexto);
                            nfe.InfAdic.obsFisco.Add(obsfiscoInfo);
                        }
                        break;

                    case "procRef":
                        {
                            procRef procrefInfo = new procRef();
                            procrefInfo.nProc = this.readValue(noder, TpcnResources.nProc);
                            procrefInfo.indProc = this.readValue(noder, TpcnResources.indProc);
                            nfe.InfAdic.procRef.Add(procrefInfo);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// processaRetirada
        /// </summary>
        /// <param name="nodeinfNFe"></param>
        private void processaRetirada(XmlNode nodeinfNFe)
        {
            nfe.retirada.CNPJ = this.readValue(nodeinfNFe, TpcnResources.CNPJ);
            nfe.retirada.CPF = this.readValue(nodeinfNFe, TpcnResources.CPF);
            nfe.retirada.xLgr = this.readValue(nodeinfNFe, TpcnResources.xLgr);
            nfe.retirada.nro = this.readValue(nodeinfNFe, TpcnResources.nro);
            nfe.retirada.xCpl = this.readValue(nodeinfNFe, TpcnResources.xCpl);
            nfe.retirada.xBairro = this.readValue(nodeinfNFe, TpcnResources.xBairro);
            nfe.retirada.cMun = this.readInt32(nodeinfNFe, TpcnResources.cMun);
            nfe.retirada.xMun = this.readValue(nodeinfNFe, TpcnResources.xMun);
            nfe.retirada.UF = this.readValue(nodeinfNFe, TpcnResources.UF);
            nfe.retirada.xNome = this.readValue(nodeinfNFe, TpcnResources.xNome);
            nfe.retirada.CEP = this.readValue(nodeinfNFe, TpcnResources.CEP);
            nfe.retirada.cPais = this.readInt32(nodeinfNFe, TpcnResources.cPais);
            nfe.retirada.xPais = this.readValue(nodeinfNFe, TpcnResources.xPais);
            nfe.retirada.fone = this.readValue(nodeinfNFe, TpcnResources.fone);
            nfe.retirada.email = this.readValue(nodeinfNFe, TpcnResources.email);
            nfe.retirada.IE = this.readValue(nodeinfNFe, TpcnResources.IE);
        }

        /// <summary>
        /// processaEntrega
        /// </summary>
        /// <param name="nodeinfNFe"></param>
        private void processaEntrega(XmlNode nodeinfNFe)
        {
            nfe.entrega.CNPJ = this.readValue(nodeinfNFe, TpcnResources.CNPJ);
            nfe.entrega.CPF = this.readValue(nodeinfNFe, TpcnResources.CPF);
            nfe.entrega.xLgr = this.readValue(nodeinfNFe, TpcnResources.xLgr);
            nfe.entrega.nro = this.readValue(nodeinfNFe, TpcnResources.nro);
            nfe.entrega.xCpl = this.readValue(nodeinfNFe, TpcnResources.xCpl);
            nfe.entrega.xBairro = this.readValue(nodeinfNFe, TpcnResources.xBairro);
            nfe.entrega.cMun = this.readInt32(nodeinfNFe, TpcnResources.cMun);
            nfe.entrega.xMun = this.readValue(nodeinfNFe, TpcnResources.xMun);
            nfe.entrega.UF = this.readValue(nodeinfNFe, TpcnResources.UF);
            nfe.entrega.xNome = this.readValue(nodeinfNFe, TpcnResources.xNome);
            nfe.entrega.CEP = this.readValue(nodeinfNFe, TpcnResources.CEP);
            nfe.entrega.cPais = this.readInt32(nodeinfNFe, TpcnResources.cPais);
            nfe.entrega.xPais = this.readValue(nodeinfNFe, TpcnResources.xPais);
            nfe.entrega.fone = this.readValue(nodeinfNFe, TpcnResources.fone);
            nfe.entrega.email = this.readValue(nodeinfNFe, TpcnResources.email);
            nfe.entrega.IE = this.readValue(nodeinfNFe, TpcnResources.IE);
        }

        /// <summary>
        /// processaautXML
        /// </summary>
        /// <param name="nodeinfNFe"></param>
        private void processaautXML(XmlNode nodeinfNFe)
        {
            foreach(XmlNode noder in nodeinfNFe.ChildNodes)
                if(noder.LocalName.Equals(TpcnResources.CNPJ.ToString()))
                    nfe.autXML.Add(new autXML { CNPJ = noder.InnerText });
                else
                    nfe.autXML.Add(new autXML { CPF = noder.InnerText });
        }

        /// <summary>
        /// processaCOBR
        /// </summary>
        /// <param name="nodeinfNFe"></param>
        private void processaCOBR(XmlNode nodeinfNFe)
        {
            foreach(XmlNode noder in nodeinfNFe.ChildNodes)
            {
                if(noder.LocalName.Equals("fat"))
                {
                    nfe.Cobr.Fat.nFat = this.readValue(noder, TpcnResources.nFat);
                    nfe.Cobr.Fat.vOrig = this.readDouble(noder, TpcnResources.vOrig);
                    nfe.Cobr.Fat.vDesc = this.readDouble(noder, TpcnResources.vDesc);
                    nfe.Cobr.Fat.vLiq = this.readDouble(noder, TpcnResources.vLiq);
                }
                if(noder.LocalName.Equals("dup"))
                {
                    Dup dupInfo = new Dup();
                    dupInfo.dVenc = this.readDate(noder, TpcnResources.dVenc);
                    dupInfo.nDup = this.readValue(noder, TpcnResources.nDup);
                    dupInfo.vDup = this.readDouble(noder, TpcnResources.vDup);
                    nfe.Cobr.Dup.Add(dupInfo);
                }
            }
        }

        /// <summary>
        /// processaTRANSP
        /// </summary>
        /// <param name="nodeinfNFe"></param>
        private void processaTRANSP(XmlNode nodeinfNFe)
        {
            foreach(XmlNode noder in nodeinfNFe.ChildNodes)
            {
                switch(noder.LocalName.ToLower())
                {
                    case "modfrete":
                        nfe.Transp.modFrete = (TpcnModalidadeFrete)this.readInt32(nodeinfNFe, TpcnResources.modFrete);
                        break;

                    case "transporta":
                        {
                            nfe.Transp.Transporta.CNPJ = this.readValue(noder, TpcnResources.CNPJ);
                            nfe.Transp.Transporta.CPF = this.readValue(noder, TpcnResources.CPF);
                            nfe.Transp.Transporta.xNome = this.readValue(noder, TpcnResources.xNome);
                            nfe.Transp.Transporta.IE = this.readValue(noder, TpcnResources.IE);
                            nfe.Transp.Transporta.xEnder = this.readValue(noder, TpcnResources.xEnder);
                            nfe.Transp.Transporta.xMun = this.readValue(noder, TpcnResources.xMun);
                            nfe.Transp.Transporta.UF = this.readValue(noder, TpcnResources.UF);
                        }
                        break;

                    case "rettransp":
                        {
                            nfe.Transp.retTransp.vServ = this.readDouble(noder, TpcnResources.vServ);
                            nfe.Transp.retTransp.vBCRet = this.readDouble(noder, TpcnResources.vBCRet);
                            nfe.Transp.retTransp.pICMSRet = this.readDouble(noder, TpcnResources.pICMSRet);
                            nfe.Transp.retTransp.vICMSRet = this.readDouble(noder, TpcnResources.vICMSRet);
                            nfe.Transp.retTransp.CFOP = this.readValue(noder, TpcnResources.CFOP);
                            nfe.Transp.retTransp.cMunFG = this.readInt32(noder, TpcnResources.cMunFG);
                        }
                        break;

                    case "veictransp":
                        {
                            nfe.Transp.veicTransp.placa = this.readValue(noder, TpcnResources.placa);
                            nfe.Transp.veicTransp.UF = this.readValue(noder, TpcnResources.UF);
                            nfe.Transp.veicTransp.RNTC = this.readValue(noder, TpcnResources.RNTC);
                        }
                        break;

                    case "reboque":
                        {
                            Reboque reboqueInfo = new Reboque();
                            reboqueInfo.placa = this.readValue(noder, TpcnResources.placa);
                            reboqueInfo.UF = this.readValue(noder, TpcnResources.UF);
                            reboqueInfo.RNTC = this.readValue(noder, TpcnResources.RNTC);

                            nfe.Transp.Reboque.Add(reboqueInfo);
                        }
                        break;

                    case "vol":
                        {
                            Vol volInfo = new Vol();
                            volInfo.qVol = this.readInt32(noder, TpcnResources.qVol);
                            volInfo.esp = this.readValue(noder, TpcnResources.esp);
                            volInfo.marca = this.readValue(noder, TpcnResources.marca);
                            volInfo.nVol = this.readValue(noder, TpcnResources.nVol);
                            volInfo.pesoL = this.readDouble(noder, TpcnResources.pesoL);
                            volInfo.pesoB = this.readDouble(noder, TpcnResources.pesoB);

                            foreach(XmlNode nodevollacre in ((XmlElement)noder).GetElementsByTagName("lacres"))
                            {
                                Lacres lacresInfo = new Lacres();
                                lacresInfo.nLacre = this.readValue(nodevollacre, TpcnResources.nLacre);
                                volInfo.Lacres.Add(lacresInfo);
                            }
                            nfe.Transp.Vol.Add(volInfo);
                        }
                        break;

                    case "vagao":
                        nfe.Transp.vagao = this.readValue(noder, TpcnResources.vagao);
                        break;

                    case "balsa":
                        nfe.Transp.balsa = this.readValue(noder, TpcnResources.balsa);
                        break;
                }
            }
        }

        /// <summary>
        /// processaTOTAL
        /// </summary>
        /// <param name="nodetotal"></param>
        private void processaTOTAL(XmlNode nodetotal)
        {
            foreach(XmlNode nodeICMSTot in ((XmlElement)nodetotal).GetElementsByTagName("ICMSTot"))
            {
                nfe.Total.ICMSTot.vBC = this.readDouble(nodeICMSTot, TpcnResources.vBC);
                nfe.Total.ICMSTot.vBCST = this.readDouble(nodeICMSTot, TpcnResources.vBCST);
                nfe.Total.ICMSTot.vCOFINS = this.readDouble(nodeICMSTot, TpcnResources.vCOFINS);
                nfe.Total.ICMSTot.vDesc = this.readDouble(nodeICMSTot, TpcnResources.vDesc);
                nfe.Total.ICMSTot.vFrete = this.readDouble(nodeICMSTot, TpcnResources.vFrete);
                nfe.Total.ICMSTot.vICMS = this.readDouble(nodeICMSTot, TpcnResources.vICMS);
                nfe.Total.ICMSTot.vII = this.readDouble(nodeICMSTot, TpcnResources.vII);
                nfe.Total.ICMSTot.vIPI = this.readDouble(nodeICMSTot, TpcnResources.vIPI);
                nfe.Total.ICMSTot.vNF = this.readDouble(nodeICMSTot, TpcnResources.vNF);
                nfe.Total.ICMSTot.vOutro = this.readDouble(nodeICMSTot, TpcnResources.vOutro);
                nfe.Total.ICMSTot.vPIS = this.readDouble(nodeICMSTot, TpcnResources.vPIS);
                nfe.Total.ICMSTot.vProd = this.readDouble(nodeICMSTot, TpcnResources.vProd);
                nfe.Total.ICMSTot.vSeg = this.readDouble(nodeICMSTot, TpcnResources.vSeg);
                nfe.Total.ICMSTot.vST = this.readDouble(nodeICMSTot, TpcnResources.vST);
                nfe.Total.ICMSTot.vTotTrib = this.readDouble(nodeICMSTot, TpcnResources.vTotTrib);
                nfe.Total.ICMSTot.vICMSDeson = this.readDouble(nodeICMSTot, TpcnResources.vICMSDeson);
                nfe.Total.ICMSTot.vICMSUFDest = this.readDouble(nodeICMSTot, TpcnResources.vICMSUFDest);
                nfe.Total.ICMSTot.vICMSUFRemet = this.readDouble(nodeICMSTot, TpcnResources.vICMSUFRemet);
                nfe.Total.ICMSTot.vFCP = this.readDouble(nodeICMSTot, TpcnResources.vFCP);
                nfe.Total.ICMSTot.vFCPST = this.readDouble(nodeICMSTot, TpcnResources.vFCPST);
                nfe.Total.ICMSTot.vFCPSTRet = this.readDouble(nodeICMSTot, TpcnResources.vFCPSTRet);
                nfe.Total.ICMSTot.vIPIDevol = this.readDouble(nodeICMSTot, TpcnResources.vIPIDevol);
            }

            foreach(XmlNode nodeISSQNtot in ((XmlElement)nodetotal).GetElementsByTagName("ISSQNtot"))
            {
                nfe.Total.ISSQNtot.vServ = this.readDouble(nodeISSQNtot, TpcnResources.vServ);
                nfe.Total.ISSQNtot.vBC = this.readDouble(nodeISSQNtot, TpcnResources.vBC);
                nfe.Total.ISSQNtot.vISS = this.readDouble(nodeISSQNtot, TpcnResources.vISS);
                nfe.Total.ISSQNtot.vPIS = this.readDouble(nodeISSQNtot, TpcnResources.vPIS);
                nfe.Total.ISSQNtot.vCOFINS = this.readDouble(nodeISSQNtot, TpcnResources.vCOFINS);

                nfe.Total.ISSQNtot.dCompet = this.readDate(nodeISSQNtot, TpcnResources.dCompet);
                nfe.Total.ISSQNtot.vDeducao = this.readDouble(nodeISSQNtot, TpcnResources.vDeducao);
                nfe.Total.ISSQNtot.vOutro = this.readDouble(nodeISSQNtot, TpcnResources.vOutro);
                nfe.Total.ISSQNtot.vDescIncond = this.readDouble(nodeISSQNtot, TpcnResources.vDescIncond);
                nfe.Total.ISSQNtot.vDescCond = this.readDouble(nodeISSQNtot, TpcnResources.vDescCond);
                nfe.Total.ISSQNtot.vISSRet = this.readDouble(nodeISSQNtot, TpcnResources.vISSRet);
                nfe.Total.ISSQNtot.cRegTrib = (TpcnRegimeTributario)this.readInt32(nodeISSQNtot, TpcnResources.cRegTrib);
            }

            foreach(XmlNode noderetTrib in ((XmlElement)nodetotal).GetElementsByTagName("retTrib"))
            {
                nfe.Total.retTrib.vRetPIS = this.readDouble(noderetTrib, TpcnResources.vRetPIS);
                nfe.Total.retTrib.vRetCOFINS = this.readDouble(noderetTrib, TpcnResources.vRetCOFINS);
                nfe.Total.retTrib.vRetCSLL = this.readDouble(noderetTrib, TpcnResources.vRetCSLL);
                nfe.Total.retTrib.vBCIRRF = this.readDouble(noderetTrib, TpcnResources.vBCIRRF);
                nfe.Total.retTrib.vIRRF = this.readDouble(noderetTrib, TpcnResources.vIRRF);
                nfe.Total.retTrib.vBCRetPrev = this.readDouble(noderetTrib, TpcnResources.vBCRetPrev);
                nfe.Total.retTrib.vRetPrev = this.readDouble(noderetTrib, TpcnResources.vRetPrev);
            }
        }

        /// <summary>
        /// processaPROD
        /// </summary>
        /// <param name="nodedet"></param>
        private void processaPROD(XmlNode nodedet)
        {
            Det detInfo = new Det();
            detInfo.Prod.nItem = Convert.ToInt32(nodedet.Attributes["nItem"].Value);
            detInfo.infAdProd = this.readValue(nodedet, TpcnResources.infAdProd);

            foreach(XmlNode nodedetprod in ((XmlElement)nodedet).GetElementsByTagName("prod"))
            {
                XmlElement ele = nodedetprod as XmlElement;

                detInfo.Prod.cEAN = this.readValue(ele, TpcnResources.cEAN);
                detInfo.Prod.cEANTrib = this.readValue(ele, TpcnResources.cEANTrib);
                detInfo.Prod.CFOP = this.readValue(ele, TpcnResources.CFOP);
                detInfo.Prod.cProd = this.readValue(ele, TpcnResources.cProd);
                detInfo.Prod.EXTIPI = this.readValue(ele, TpcnResources.EXTIPI);
                detInfo.Prod.indTot = (TpcnIndicadorTotal)this.readInt32(ele, TpcnResources.indTot);
                detInfo.Prod.NCM = this.readValue(ele, TpcnResources.NCM);
                detInfo.Prod.NVE = this.readValue(ele, TpcnResources.NVE);
                detInfo.Prod.nItemPed = this.readInt32(ele, TpcnResources.nItemPed);
                detInfo.Prod.qCom = this.readDouble(ele, TpcnResources.qCom);
                detInfo.Prod.qTrib = this.readDouble(ele, TpcnResources.qTrib);
                detInfo.Prod.uCom = this.readValue(ele, TpcnResources.uCom);
                detInfo.Prod.uTrib = this.readValue(ele, TpcnResources.uTrib);
                detInfo.Prod.vDesc = this.readDouble(ele, TpcnResources.vDesc);
                detInfo.Prod.vFrete = this.readDouble(ele, TpcnResources.vFrete);
                detInfo.Prod.vOutro = this.readDouble(ele, TpcnResources.vOutro);
                detInfo.Prod.vProd = this.readDouble(ele, TpcnResources.vProd);
                detInfo.Prod.vSeg = this.readDouble(ele, TpcnResources.vSeg);
                detInfo.Prod.vUnCom = this.readDouble(ele, TpcnResources.vUnCom);
                detInfo.Prod.vUnTrib = this.readDouble(ele, TpcnResources.vUnTrib);
                detInfo.Prod.xPed = this.readValue(ele, TpcnResources.xPed);
                detInfo.Prod.xProd = this.readValue(ele, TpcnResources.xProd);
                detInfo.Prod.nRECOPI = this.readValue(ele, TpcnResources.nRECOPI);
                detInfo.Prod.nFCI = this.readValue(ele, TpcnResources.nFCI);
                detInfo.Prod.CEST = this.readInt32(ele, TpcnResources.CEST);
                if(nfe.infNFe.Versao >= 4)
                {
                    switch(this.readValue(ele, TpcnResources.indEscala))
                    {
                        case "S":
                            detInfo.Prod.indEscala = TpcnIndicadorEscala.ieSomaTotalNFe;
                            break;

                        case "N":
                            detInfo.Prod.indEscala = TpcnIndicadorEscala.ieNaoSomaTotalNFe;
                            break;

                        default:
                            detInfo.Prod.indEscala = TpcnIndicadorEscala.ieNenhum;
                            break;
                    }
                    detInfo.Prod.CNPJFab = this.readValue(ele, TpcnResources.CNPJFab);
                    detInfo.Prod.cBenef = this.readValue(ele, TpcnResources.cBenef);
                }
            }

            #region -->prod->arma

            foreach(XmlNode nodedetArma in ((XmlElement)nodedet).GetElementsByTagName("arma"))
            {
                Arma armaInfo = new Arma();

                armaInfo.descr = this.readValue(nodedetArma, TpcnResources.descr);
                armaInfo.nCano = this.readValue(nodedetArma, TpcnResources.nCano);
                armaInfo.nSerie = this.readValue(nodedetArma, TpcnResources.nSerie);
                armaInfo.tpArma = (TpcnTipoArma)this.readInt32(nodedetArma, TpcnResources.tpArma);

                detInfo.Prod.arma.Add(armaInfo);
            }
            #endregion -->prod->arma

            #region --prod->comb

            foreach(XmlNode nodedetComb in ((XmlElement)nodedet).GetElementsByTagName("comb"))
            {
                detInfo.Prod.comb.cProdANP = this.readInt32(nodedetComb, TpcnResources.cProdANP);
                detInfo.Prod.comb.CODIF = this.readValue(nodedetComb, TpcnResources.CODIF);
                if(nfe.infNFe.Versao < 4)
                    detInfo.Prod.comb.pMixGN = this.readDouble(nodedetComb, TpcnResources.pMixGN);
                else
                {
                    detInfo.Prod.comb.descANP = this.readValue(nodedetComb, TpcnResources.descANP);
                    detInfo.Prod.comb.pGLP = this.readDouble(nodedetComb, TpcnResources.pGLP);
                    detInfo.Prod.comb.pGNi = this.readDouble(nodedetComb, TpcnResources.pGNi);
                    detInfo.Prod.comb.pGNn = this.readDouble(nodedetComb, TpcnResources.pGNn);
                    detInfo.Prod.comb.vPart = this.readDouble(nodedetComb, TpcnResources.vPart);
                }
                detInfo.Prod.comb.qTemp = this.readDouble(nodedetComb, TpcnResources.qTemp);
                detInfo.Prod.comb.UFCons = this.readValue(nodedetComb, TpcnResources.UFCons);
                foreach(XmlNode nodedetCombCIDE in ((XmlElement)nodedetComb).GetElementsByTagName("CIDE"))
                {
                    detInfo.Prod.comb.CIDE.qBCprod = this.readDouble(nodedetCombCIDE, TpcnResources.qBCProd);
                    detInfo.Prod.comb.CIDE.vAliqProd = this.readDouble(nodedetCombCIDE, TpcnResources.vAliqProd);
                    detInfo.Prod.comb.CIDE.vCIDE = this.readDouble(nodedetCombCIDE, TpcnResources.vCIDE);
                }
                foreach(XmlNode nodedetCombEncerrante in ((XmlElement)nodedetComb).GetElementsByTagName("encerrante"))
                {
                    detInfo.Prod.comb.encerrante.nBico = this.readInt32(nodedetCombEncerrante, TpcnResources.nBico);
                    detInfo.Prod.comb.encerrante.nBomba = this.readInt32(nodedetCombEncerrante, TpcnResources.nBomba);
                    detInfo.Prod.comb.encerrante.nTanque = this.readInt32(nodedetCombEncerrante, TpcnResources.nTanque);
                    detInfo.Prod.comb.encerrante.vEncIni = this.readValue(nodedetCombEncerrante, TpcnResources.vEncIni);
                    detInfo.Prod.comb.encerrante.vEncFin = this.readValue(nodedetCombEncerrante, TpcnResources.vEncFin);
                }
            }
            #endregion --prod->comb

            #region --prod->med

            foreach(XmlNode nodedetmed in ((XmlElement)nodedet).GetElementsByTagName("med"))
            {
                Med medInfo = new Med();
                if(nfe.infNFe.Versao < 4)
                {
                    medInfo.dFab = this.readDate(nodedetmed, TpcnResources.dFab);
                    medInfo.dVal = this.readDate(nodedetmed, TpcnResources.dVal);
                    medInfo.nLote = this.readValue(nodedetmed, TpcnResources.nLote);
                    medInfo.qLote = this.readDouble(nodedetmed, TpcnResources.qLote);
                }
                else
                {
                    medInfo.cProdANVISA = this.readValue(nodedetmed, TpcnResources.cProdANVISA);
                    medInfo.xMotivoIsencao = this.readValue(nodedetmed, nameof(medInfo.xMotivoIsencao));
                }
                medInfo.vPMC = this.readDouble(nodedetmed, TpcnResources.vPMC);
                detInfo.Prod.med.Add(medInfo);
            }
            #endregion --prod->med

            #region --prod->rastro

            foreach(XmlNode nodedetrastro in ((XmlElement)nodedet).GetElementsByTagName("rastro"))
            {
                Rastro rastroInfo = new Rastro();
                rastroInfo.dFab = this.readDate(nodedetrastro, TpcnResources.dFab);
                rastroInfo.dVal = this.readDate(nodedetrastro, TpcnResources.dVal);
                rastroInfo.nLote = this.readValue(nodedetrastro, TpcnResources.nLote);
                rastroInfo.qLote = this.readDouble(nodedetrastro, TpcnResources.qLote);
                rastroInfo.cAgreg = this.readValue(nodedetrastro, TpcnResources.cAgreg);
                detInfo.Prod.rastro.Add(rastroInfo);
            }
            #endregion --prod->rastro

            #region --prod->veicProd

            foreach(XmlNode nodedetveic in ((XmlElement)nodedet).GetElementsByTagName("veicProd"))
            {
                detInfo.Prod.veicProd.anoFab = this.readInt32(nodedetveic, TpcnResources.anoFab);
                detInfo.Prod.veicProd.anoMod = this.readInt32(nodedetveic, TpcnResources.anoMod);
                detInfo.Prod.veicProd.cCor = this.readValue(nodedetveic, TpcnResources.cCor);
                detInfo.Prod.veicProd.cCorDENATRAN = this.readInt32(nodedetveic, TpcnResources.cCorDENATRAN);
                detInfo.Prod.veicProd.chassi = this.readValue(nodedetveic, TpcnResources.chassi);
                detInfo.Prod.veicProd.cilin = this.readValue(nodedetveic, TpcnResources.cilin);
                detInfo.Prod.veicProd.cMod = this.readValue(nodedetveic, TpcnResources.cMod);
                detInfo.Prod.veicProd.CMT = this.readValue(nodedetveic, TpcnResources.CMT);
                detInfo.Prod.veicProd.condVeic = this.readValue(nodedetveic, TpcnResources.condVeic);
                detInfo.Prod.veicProd.dist = this.readValue(nodedetveic, TpcnResources.dist);
                detInfo.Prod.veicProd.espVeic = this.readInt32(nodedetveic, TpcnResources.espVeic);
                detInfo.Prod.veicProd.lota = this.readInt32(nodedetveic, TpcnResources.lota);
                detInfo.Prod.veicProd.nMotor = this.readValue(nodedetveic, TpcnResources.nMotor);
                detInfo.Prod.veicProd.nSerie = this.readValue(nodedetveic, TpcnResources.nSerie);
                detInfo.Prod.veicProd.pesoB = this.readValue(nodedetveic, TpcnResources.pesoB);
                detInfo.Prod.veicProd.pesoL = this.readValue(nodedetveic, TpcnResources.pesoL);
                detInfo.Prod.veicProd.pot = this.readValue(nodedetveic, TpcnResources.pot);
                detInfo.Prod.veicProd.tpComb = this.readValue(nodedetveic, TpcnResources.tpComb);
                detInfo.Prod.veicProd.tpOp = this.readValue(nodedetveic, TpcnResources.tpOp);
                detInfo.Prod.veicProd.tpPint = this.readValue(nodedetveic, TpcnResources.tpPint);
                detInfo.Prod.veicProd.tpRest = this.readInt32(nodedetveic, TpcnResources.tpRest);
                detInfo.Prod.veicProd.tpVeic = this.readInt32(nodedetveic, TpcnResources.tpVeic);
                detInfo.Prod.veicProd.VIN = this.readValue(nodedetveic, TpcnResources.VIN);
                detInfo.Prod.veicProd.xCor = this.readValue(nodedetveic, TpcnResources.xCor);
            }
            #endregion --prod->veicProd

            #region --pod->DI

            foreach(XmlNode nodedetDI in ((XmlElement)nodedet).GetElementsByTagName("DI"))
            {
                DI diInfo = new DI();
                diInfo.cExportador = this.readValue(nodedetDI, TpcnResources.cExportador);
                diInfo.dDesemb = this.readDate(nodedetDI, TpcnResources.dDesemb);
                diInfo.dDI = this.readDate(nodedetDI, TpcnResources.dDI);
                diInfo.nDI = this.readValue(nodedetDI, TpcnResources.nDI);
                diInfo.xLocDesemb = this.readValue(nodedetDI, TpcnResources.xLocDesemb);
                diInfo.UFDesemb = this.readValue(nodedetDI, TpcnResources.UFDesemb);

                diInfo.tpViaTransp = (TpcnTipoViaTransp)this.readInt32(nodedetDI, TpcnResources.tpViaTransp);
                diInfo.vAFRMM = this.readDouble(nodedetDI, TpcnResources.vAFRMM);
                diInfo.tpIntermedio = (TpcnTipoIntermedio)this.readInt32(nodedetDI, TpcnResources.tpIntermedio);
                diInfo.CNPJ = this.readValue(nodedetDI, TpcnResources.CNPJ);
                diInfo.UFTerceiro = this.readValue(nodedetDI, TpcnResources.UFTerceiro);

                foreach(XmlNode nodedetDIadi in ((XmlElement)nodedetDI).GetElementsByTagName("adi"))
                {
                    Adi adiInfo = new Adi();

                    adiInfo.cFabricante = this.readValue(nodedetDIadi, TpcnResources.cFabricante);
                    adiInfo.nAdicao = this.readInt32(nodedetDIadi, TpcnResources.nAdicao);
                    adiInfo.nSeqAdi = this.readInt32(nodedetDIadi, TpcnResources.nSeqAdic);
                    adiInfo.vDescDI = this.readDouble(nodedetDIadi, TpcnResources.vDescDI);
                    adiInfo.nDraw = this.readValue(nodedetDIadi, TpcnResources.nDraw.ToString());

                    diInfo.adi.Add(adiInfo);
                }
                detInfo.Prod.DI.Add(diInfo);
            }
            #endregion --pod->DI

            #region -- prod->detExport

            foreach(XmlNode nodedetExport in ((XmlElement)nodedet).GetElementsByTagName("detExport"))
            {
                detInfo.Prod.detExport.Add(new detExport { nDraw = this.readValue(nodedetExport, TpcnResources.nDraw) });
                foreach(XmlNode nodedetExportInd in ((XmlElement)nodedetExport).GetElementsByTagName("exportInd"))
                {
                    detInfo.Prod.detExport[detInfo.Prod.detExport.Count - 1].exportInd.nRE =
                        this.readValue(nodedetExportInd, TpcnResources.nRE);
                    detInfo.Prod.detExport[detInfo.Prod.detExport.Count - 1].exportInd.chNFe =
                        this.readValue(nodedetExportInd, TpcnResources.chNFe);
                    detInfo.Prod.detExport[detInfo.Prod.detExport.Count - 1].exportInd.qExport =
                        this.readDouble(nodedetExportInd, TpcnResources.qExport);
                }
            }
            #endregion -- prod->detExport

            #region -- prod->impostoDevol

            foreach(XmlNode nodedetimpostoDevol in ((XmlElement)nodedet).GetElementsByTagName("impostoDevol"))
            {
                detInfo.impostoDevol.pDevol = this.readDouble(nodedetimpostoDevol, TpcnResources.pDevol);
                foreach(XmlNode nodedetimpostoDevolInd in ((XmlElement)nodedetimpostoDevol).GetElementsByTagName("IPI"))
                {
                    detInfo.impostoDevol.vIPIDevol = this.readDouble(nodedetimpostoDevolInd, TpcnResources.vIPIDevol);
                }
            }
            #endregion -- prod->impostoDevol

            foreach(XmlNode nodedetImposto in ((XmlElement)nodedet).GetElementsByTagName("imposto"))
            {
                detInfo.Imposto.vTotTrib = this.readDouble(((XmlElement)nodedetImposto), TpcnResources.vTotTrib);

                #region -->Imposto->ICMS

                foreach(XmlNode nodedetImpostoICMS in ((XmlElement)nodedetImposto).GetElementsByTagName(TpcnResources.ICMS.ToString()))
                {
                    if(nodedetImpostoICMS.ChildNodes.Count > 0)
                    {
                        XmlNode nodedetImpostoICMS_ = nodedetImpostoICMS.ChildNodes[0];

                        detInfo.Imposto.ICMS.CST = this.readValue(nodedetImpostoICMS_, TpcnResources.CST);
                        detInfo.Imposto.ICMS.CSOSN = this.readInt32(nodedetImpostoICMS_, TpcnResources.CSOSN);
                        detInfo.Imposto.ICMS.modBC = (TpcnDeterminacaoBaseIcms)this.readInt32(nodedetImpostoICMS_, TpcnResources.modBC);
                        detInfo.Imposto.ICMS.modBCST = (TpcnDeterminacaoBaseIcmsST)this.readInt32(nodedetImpostoICMS_, TpcnResources.modBCST);
                        detInfo.Imposto.ICMS.motDesICMS = this.readInt32(nodedetImpostoICMS_, TpcnResources.motDesICMS);
                        detInfo.Imposto.ICMS.orig = (TpcnOrigemMercadoria)this.readInt32(nodedetImpostoICMS_, TpcnResources.orig);
                        detInfo.Imposto.ICMS.pCredSN = this.readDouble(nodedetImpostoICMS_, TpcnResources.pCredSN);
                        detInfo.Imposto.ICMS.pICMS = this.readDouble(nodedetImpostoICMS_, TpcnResources.pICMS);
                        detInfo.Imposto.ICMS.pICMSST = this.readDouble(nodedetImpostoICMS_, TpcnResources.pICMSST);
                        detInfo.Imposto.ICMS.pMVAST = this.readDouble(nodedetImpostoICMS_, TpcnResources.pMVAST);
                        detInfo.Imposto.ICMS.pRedBC = this.readDouble(nodedetImpostoICMS_, TpcnResources.pRedBC);
                        detInfo.Imposto.ICMS.pRedBCST = this.readDouble(nodedetImpostoICMS_, TpcnResources.pRedBCST);
                        detInfo.Imposto.ICMS.UFST = this.readValue(nodedetImpostoICMS_, TpcnResources.UFST);
                        detInfo.Imposto.ICMS.vBC = this.readDouble(nodedetImpostoICMS_, TpcnResources.vBC);
                        detInfo.Imposto.ICMS.pBCOp = this.readDouble(nodedetImpostoICMS_, TpcnResources.pBCOp);
                        detInfo.Imposto.ICMS.vBCST = this.readDouble(nodedetImpostoICMS_, TpcnResources.vBCST);
                        detInfo.Imposto.ICMS.vBCSTDest = this.readDouble(nodedetImpostoICMS_, TpcnResources.vBCSTDest);
                        detInfo.Imposto.ICMS.vBCSTRet = this.readDouble(nodedetImpostoICMS_, TpcnResources.vBCSTRet);
                        detInfo.Imposto.ICMS.vCredICMSSN = this.readDouble(nodedetImpostoICMS_, TpcnResources.vCredICMSSN);
                        detInfo.Imposto.ICMS.vICMS = this.readDouble(nodedetImpostoICMS_, TpcnResources.vICMS);
                        detInfo.Imposto.ICMS.vICMSST = this.readDouble(nodedetImpostoICMS_, TpcnResources.vICMSST);
                        detInfo.Imposto.ICMS.vICMSSTDest = this.readDouble(nodedetImpostoICMS_, TpcnResources.vICMSSTDest);
                        detInfo.Imposto.ICMS.vICMSSTRet = this.readDouble(nodedetImpostoICMS_, TpcnResources.vICMSSTRet);
                        detInfo.Imposto.ICMS.vICMSDeson = this.readDouble(nodedetImpostoICMS_, TpcnResources.vICMSDeson);
                        detInfo.Imposto.ICMS.vICMSOp = this.readDouble(nodedetImpostoICMS_, TpcnResources.vICMSOp);
                        detInfo.Imposto.ICMS.pDif = this.readDouble(nodedetImpostoICMS_, TpcnResources.pDif);
                        detInfo.Imposto.ICMS.vICMSDif = this.readDouble(nodedetImpostoICMS_, TpcnResources.vICMSDif);
                        detInfo.Imposto.ICMS.pFCP = this.readDouble(nodedetImpostoICMS_, TpcnResources.pFCP);
                        detInfo.Imposto.ICMS.vFCP = this.readDouble(nodedetImpostoICMS_, TpcnResources.vFCP);
                        detInfo.Imposto.ICMS.vBCFCP = this.readDouble(nodedetImpostoICMS_, TpcnResources.vBCFCP);
                        detInfo.Imposto.ICMS.vBCFCPST = this.readDouble(nodedetImpostoICMS_, TpcnResources.vBCFCPST);
                        detInfo.Imposto.ICMS.pFCPST = this.readDouble(nodedetImpostoICMS_, TpcnResources.pFCPST);
                        detInfo.Imposto.ICMS.vFCPST = this.readDouble(nodedetImpostoICMS_, TpcnResources.vFCPST);
                        detInfo.Imposto.ICMS.pST = this.readDouble(nodedetImpostoICMS_, TpcnResources.pST);
                        detInfo.Imposto.ICMS.vBCFCPSTRet = this.readDouble(nodedetImpostoICMS_, TpcnResources.vBCFCPSTRet);
                        detInfo.Imposto.ICMS.pFCPSTRet = this.readDouble(nodedetImpostoICMS_, TpcnResources.pFCPSTRet);
                        detInfo.Imposto.ICMS.vFCPSTRet = this.readDouble(nodedetImpostoICMS_, TpcnResources.vFCPSTRet);

                        detInfo.Imposto.ICMS.vBCEfet = this.readDouble(nodedetImpostoICMS_, TpcnResources.vBCEfet);
                        detInfo.Imposto.ICMS.pRedBCEfet = this.readDouble(nodedetImpostoICMS_, TpcnResources.pRedBCEfet);
                        detInfo.Imposto.ICMS.pICMSEfet = this.readDouble(nodedetImpostoICMS_, TpcnResources.pICMSEfet);
                        detInfo.Imposto.ICMS.vICMSEfet = this.readDouble(nodedetImpostoICMS_, TpcnResources.vICMSEfet);
                        detInfo.Imposto.ICMS.vICMSSubstituto = this.readDouble(nodedetImpostoICMS_, TpcnResources.vICMSSubstituto);
                    }
                }
                #endregion -->Imposto->ICMS

                // Inicializa CST como sendo Não tributada e conforme o TIPO entrada ou saida
                // Caso a Tag não seja informada sera gravada com sendo não tributada
                if(nfe.ide.tpNF == TpcnTipoNFe.tnEntrada)
                    detInfo.Imposto.IPI.CST = "53";
                else
                    detInfo.Imposto.IPI.CST = "03";

                #region --Imposto->IPI

                foreach(XmlNode nodedetImpostoIPI in ((XmlElement)nodedetImposto).GetElementsByTagName("IPI"))
                {
                    detInfo.Imposto.IPI.cEnq = this.readValue(nodedetImpostoIPI, TpcnResources.cEnq);
                    detInfo.Imposto.IPI.clEnq = this.readValue(nodedetImpostoIPI, TpcnResources.clEnq);
                    detInfo.Imposto.IPI.CNPJProd = this.readValue(nodedetImpostoIPI, TpcnResources.CNPJProd);
                    detInfo.Imposto.IPI.cSelo = this.readValue(nodedetImpostoIPI, TpcnResources.cSelo);
                    detInfo.Imposto.IPI.qSelo = this.readInt32(nodedetImpostoIPI, TpcnResources.qSelo);

                    foreach(XmlNode nodedetImpostoIPITrib in ((XmlElement)nodedetImpostoIPI).GetElementsByTagName("IPITrib"))
                    {
                        detInfo.Imposto.IPI.CST = this.readValue(nodedetImpostoIPITrib, TpcnResources.CST);
                        detInfo.Imposto.IPI.pIPI = this.readDouble(nodedetImpostoIPITrib, TpcnResources.pIPI);
                        detInfo.Imposto.IPI.pIPI = this.readDouble(nodedetImpostoIPITrib, TpcnResources.pIPI);
                        detInfo.Imposto.IPI.qUnid = this.readDouble(nodedetImpostoIPITrib, TpcnResources.qUnid);
                        detInfo.Imposto.IPI.vBC = this.readDouble(nodedetImpostoIPITrib, TpcnResources.vBC);
                        detInfo.Imposto.IPI.vIPI = this.readDouble(nodedetImpostoIPITrib, TpcnResources.vIPI);
                        detInfo.Imposto.IPI.vUnid = this.readDouble(nodedetImpostoIPITrib, TpcnResources.vUnid);
                    }
                    foreach(XmlNode nodedetImpostoIPInt in ((XmlElement)nodedetImpostoIPI).GetElementsByTagName("IPINT"))
                    {
                        detInfo.Imposto.IPI.CST = this.readValue(nodedetImpostoIPInt, TpcnResources.CST);
                    }
                }
                #endregion --Imposto->IPI

                #region --Imposto->II

                foreach(XmlNode nodedetImpostoII in ((XmlElement)nodedetImposto).GetElementsByTagName("II"))
                {
                    detInfo.Imposto.II.vBC = this.readDouble(nodedetImpostoII, TpcnResources.vBC);
                    detInfo.Imposto.II.vDespAdu = this.readDouble(nodedetImpostoII, TpcnResources.vDespAdu);
                    detInfo.Imposto.II.vII = this.readDouble(nodedetImpostoII, TpcnResources.vII);
                    detInfo.Imposto.II.vIOF = this.readDouble(nodedetImpostoII, TpcnResources.vIOF);
                }
                #endregion --Imposto->II

                #region --Imposto->PIS

                foreach(XmlNode nodedetImpostoPIS in ((XmlElement)nodedetImposto).GetElementsByTagName("PIS"))
                {
                    detInfo.Imposto.PIS.CST = this.readValue(nodedetImpostoPIS, TpcnResources.CST);
                    detInfo.Imposto.PIS.vBC = this.readDouble(nodedetImpostoPIS, TpcnResources.vBC);
                    detInfo.Imposto.PIS.pPIS = this.readDouble(nodedetImpostoPIS, TpcnResources.pPIS);
                    detInfo.Imposto.PIS.vPIS = this.readDouble(nodedetImpostoPIS, TpcnResources.vPIS);
                    detInfo.Imposto.PIS.qBCProd = this.readDouble(nodedetImpostoPIS, TpcnResources.qBCProd);
                    detInfo.Imposto.PIS.vAliqProd = this.readDouble(nodedetImpostoPIS, TpcnResources.vAliqProd);
                }
                #endregion --Imposto->PIS

                #region --Imposto->PISST

                foreach(XmlNode nodedetImpostoPISst in ((XmlElement)nodedetImposto).GetElementsByTagName("PISST"))
                {
                    detInfo.Imposto.PISST.vBC = this.readDouble(nodedetImpostoPISst, TpcnResources.vBC);
                    detInfo.Imposto.PISST.pPis = this.readDouble(nodedetImpostoPISst, TpcnResources.pPIS);
                    detInfo.Imposto.PISST.qBCProd = this.readDouble(nodedetImpostoPISst, TpcnResources.qBCProd);
                    detInfo.Imposto.PISST.vAliqProd = this.readDouble(nodedetImpostoPISst, TpcnResources.vAliqProd);
                    detInfo.Imposto.PISST.vPIS = this.readDouble(nodedetImpostoPISst, TpcnResources.vPIS);
                }
                #endregion --Imposto->PISST

                #region --Imposto->COFINS

                foreach(XmlNode nodedetImpostoCOFINS in ((XmlElement)nodedetImposto).GetElementsByTagName("COFINS"))
                {
                    detInfo.Imposto.COFINS.CST = this.readValue(nodedetImpostoCOFINS, TpcnResources.CST);
                    detInfo.Imposto.COFINS.vBC = this.readDouble(nodedetImpostoCOFINS, TpcnResources.vBC);
                    detInfo.Imposto.COFINS.pCOFINS = this.readDouble(nodedetImpostoCOFINS, TpcnResources.pCOFINS);
                    detInfo.Imposto.COFINS.qBCProd = this.readDouble(nodedetImpostoCOFINS, TpcnResources.qBCProd);
                    detInfo.Imposto.COFINS.vAliqProd = this.readDouble(nodedetImpostoCOFINS, TpcnResources.vAliqProd);
                    detInfo.Imposto.COFINS.vCOFINS = this.readDouble(nodedetImpostoCOFINS, TpcnResources.vCOFINS);
                }
                #endregion --Imposto->COFINS

                #region --Imposto->COFINSST

                foreach(XmlNode nodedetImpostoCOFINSst in ((XmlElement)nodedetImposto).GetElementsByTagName("COFINSST"))
                {
                    detInfo.Imposto.COFINSST.vBC = this.readDouble(nodedetImpostoCOFINSst, TpcnResources.vBC);
                    detInfo.Imposto.COFINSST.pCOFINS = this.readDouble(nodedetImpostoCOFINSst, TpcnResources.pCOFINS);
                    detInfo.Imposto.COFINSST.qBCProd = this.readDouble(nodedetImpostoCOFINSst, TpcnResources.qBCProd);
                    detInfo.Imposto.COFINSST.vAliqProd = this.readDouble(nodedetImpostoCOFINSst, TpcnResources.vAliqProd);
                    detInfo.Imposto.COFINSST.vCOFINS = this.readDouble(nodedetImpostoCOFINSst, TpcnResources.vCOFINS);
                }
                #endregion --Imposto->COFINSST

                #region --Imposto->ICMSUFDest

                foreach(XmlNode nodedetImpostoICMS_ICMSUFDest in ((XmlElement)nodedetImposto).GetElementsByTagName(TpcnResources.ICMSUFDest.ToString()))
                {
                    if(nodedetImpostoICMS_ICMSUFDest.ChildNodes.Count > 0)
                    {
                        XmlNode nodedetImpostoICMS__ICMSUFDest = nodedetImpostoICMS_ICMSUFDest.ChildNodes[0];

                        detInfo.Imposto.ICMS.ICMSUFDest.vBCUFDest = this.readDouble(nodedetImpostoICMS__ICMSUFDest, TpcnResources.vBCUFDest);
                        detInfo.Imposto.ICMS.ICMSUFDest.pICMSUFDest = this.readDouble(nodedetImpostoICMS__ICMSUFDest, TpcnResources.pICMSUFDest);
                        detInfo.Imposto.ICMS.ICMSUFDest.pICMSInter = this.readDouble(nodedetImpostoICMS__ICMSUFDest, TpcnResources.pICMSInter);
                        detInfo.Imposto.ICMS.ICMSUFDest.pICMSInterPart = this.readDouble(nodedetImpostoICMS__ICMSUFDest, TpcnResources.pICMSInterPart);
                        detInfo.Imposto.ICMS.ICMSUFDest.vICMSUFDest = this.readDouble(nodedetImpostoICMS__ICMSUFDest, TpcnResources.vICMSUFDest);
                        detInfo.Imposto.ICMS.ICMSUFDest.vICMSUFRemet = this.readDouble(nodedetImpostoICMS__ICMSUFDest, TpcnResources.vICMSUFRemet);
                        detInfo.Imposto.ICMS.ICMSUFDest.vBCFCPUFDest = this.readDouble(nodedetImpostoICMS__ICMSUFDest, TpcnResources.vBCFCPUFDest);
                    }
                }
                #endregion --Imposto->ICMSUFDest

                #region --Imposto->ISSQN

                foreach(XmlNode nodedetImpostoISSQN in ((XmlElement)nodedetImposto).GetElementsByTagName("ISSQN"))
                {
                    detInfo.Imposto.ISSQN.vBC = this.readDouble(nodedetImpostoISSQN, TpcnResources.vBC);
                    detInfo.Imposto.ISSQN.vAliq = this.readDouble(nodedetImpostoISSQN, TpcnResources.vAliq);
                    detInfo.Imposto.ISSQN.vISSQN = this.readDouble(nodedetImpostoISSQN, TpcnResources.vISSQN);
                    detInfo.Imposto.ISSQN.cMunFG = this.readInt32(nodedetImpostoISSQN, TpcnResources.cMunFG);
                    detInfo.Imposto.ISSQN.cListServ = this.readValue(nodedetImpostoISSQN, TpcnResources.cListServ);
                    detInfo.Imposto.ISSQN.cSitTrib = this.readValue(nodedetImpostoISSQN, TpcnResources.cSitTrib);

                    // 3.10
                    detInfo.Imposto.ISSQN.vDeducao = this.readDouble(nodedetImpostoISSQN, TpcnResources.vDeducao);
                    detInfo.Imposto.ISSQN.vOutro = this.readDouble(nodedetImpostoISSQN, TpcnResources.vOutro);
                    detInfo.Imposto.ISSQN.vDescIncond = this.readDouble(nodedetImpostoISSQN, TpcnResources.vDescIncond);
                    detInfo.Imposto.ISSQN.vDescCond = this.readDouble(nodedetImpostoISSQN, TpcnResources.vDescCond);
                    detInfo.Imposto.ISSQN.vISSRet = this.readDouble(nodedetImpostoISSQN, TpcnResources.vISSRet);
                    detInfo.Imposto.ISSQN.indISS = (TpcnindISS)this.readDouble(nodedetImpostoISSQN, TpcnResources.indISS);
                    detInfo.Imposto.ISSQN.cServico = this.readValue(nodedetImpostoISSQN, TpcnResources.cServico);
                    detInfo.Imposto.ISSQN.cMun = this.readInt32(nodedetImpostoISSQN, TpcnResources.cMun);
                    detInfo.Imposto.ISSQN.cPais = this.readInt32(nodedetImpostoISSQN, TpcnResources.cPais);
                    detInfo.Imposto.ISSQN.nProcesso = this.readValue(nodedetImpostoISSQN, TpcnResources.nProcesso);
                    detInfo.Imposto.ISSQN.indIncentivo = this.readValue(nodedetImpostoISSQN, TpcnResources.indIncentivo) == "1";
                }
                #endregion --Imposto->ISSQN
            }
            nfe.det.Add(detInfo);
        }

        /// <summary>
        /// processaDEST
        /// </summary>
        /// <param name="el"></param>
        private void processaDEST(XmlNode el)
        {
            ///
            /// NFC-e
            ///
            nfe.dest.idEstrangeiro = this.readValue(el, TpcnResources.idEstrangeiro.ToString());

            nfe.dest.CNPJ = this.readValue(el, TpcnResources.CNPJ);
            nfe.dest.CPF = this.readValue(el, TpcnResources.CPF);
            nfe.dest.email = this.readValue(el, TpcnResources.email);
            nfe.dest.IE = this.readValue(el, TpcnResources.IE);
            nfe.dest.ISUF = this.readValue(el, TpcnResources.ISUF);
            nfe.dest.xNome = this.readValue(el, TpcnResources.xNome);
            if((double)nfe.infNFe.Versao >= 3.10)
            {
                nfe.dest.IM = this.readValue(el, TpcnResources.IM);
                nfe.dest.indIEDest = (TpcnindIEDest)Convert.ToInt32("0" + this.readValue(el, TpcnResources.indIEDest));
            }
            foreach(XmlNode nodeEnder in ((XmlElement)el).GetElementsByTagName("enderDest"))
            {
                XmlElement ele = nodeEnder as XmlElement;

                nfe.dest.enderDest.CEP = this.readInt32(ele, TpcnResources.CEP);
                nfe.dest.enderDest.cMun = this.readInt32(ele, TpcnResources.cMun);
                nfe.dest.enderDest.cPais = this.readInt32(ele, TpcnResources.cPais);
                nfe.dest.enderDest.fone = this.readValue(ele, TpcnResources.fone);
                nfe.dest.enderDest.nro = this.readValue(ele, TpcnResources.nro);
                nfe.dest.enderDest.UF = this.readValue(ele, TpcnResources.UF);
                nfe.dest.enderDest.xBairro = this.readValue(ele, TpcnResources.xBairro);
                nfe.dest.enderDest.xCpl = this.readValue(ele, TpcnResources.xCpl);
                nfe.dest.enderDest.xLgr = this.readValue(ele, TpcnResources.xLgr);
                nfe.dest.enderDest.xMun = this.readValue(ele, TpcnResources.xMun);
                nfe.dest.enderDest.xPais = this.readValue(ele, TpcnResources.xPais);
            }
        }

        /// <summary>
        /// processaEMIT
        /// </summary>
        /// <param name="el"></param>
        private void processaEMIT(XmlNode el)
        {
            nfe.emit.CNAE = this.readValue(el, TpcnResources.CNAE);
            nfe.emit.CNPJ = this.readValue(el, TpcnResources.CNPJ);
            nfe.emit.CPF = this.readValue(el, TpcnResources.CPF);
            nfe.emit.CRT = (TpcnCRT)this.readInt32(el, TpcnResources.CRT);
            nfe.emit.IE = this.readValue(el, TpcnResources.IE);
            nfe.emit.IEST = this.readValue(el, TpcnResources.IEST);
            nfe.emit.IM = this.readValue(el, TpcnResources.IM);
            nfe.emit.xFant = this.readValue(el, TpcnResources.xFant);
            nfe.emit.xNome = this.readValue(el, TpcnResources.xNome);

            foreach(XmlNode nodeEnder in ((XmlElement)el).GetElementsByTagName("enderEmit"))
            {
                XmlElement ele = nodeEnder as XmlElement;

                nfe.emit.enderEmit.CEP = this.readInt32(ele, TpcnResources.CEP);
                nfe.emit.enderEmit.cMun = this.readInt32(ele, TpcnResources.cMun);
                nfe.emit.enderEmit.cPais = this.readInt32(ele, TpcnResources.cPais);
                nfe.emit.enderEmit.fone = this.readValue(ele, TpcnResources.fone);
                nfe.emit.enderEmit.nro = this.readValue(ele, TpcnResources.nro);
                nfe.emit.enderEmit.UF = this.readValue(ele, TpcnResources.UF);
                nfe.emit.enderEmit.xBairro = this.readValue(ele, TpcnResources.xBairro);
                nfe.emit.enderEmit.xCpl = this.readValue(ele, TpcnResources.xCpl);
                nfe.emit.enderEmit.xLgr = this.readValue(ele, TpcnResources.xLgr);
                nfe.emit.enderEmit.xMun = this.readValue(ele, TpcnResources.xMun);
                nfe.emit.enderEmit.xPais = this.readValue(ele, TpcnResources.xPais);
            }
        }

        /// <summary>
        /// processaIDE
        /// </summary>
        /// <param name="nodeinfNFe"></param>
        private void processaIDE(XmlNode nodeinfNFe)
        {
            nfe.ide.cDV = this.readInt32(nodeinfNFe, TpcnResources.cDV);
            nfe.ide.cMunFG = this.readInt32(nodeinfNFe, TpcnResources.cMunFG);
            nfe.ide.cNF = this.readInt32(nodeinfNFe, TpcnResources.cNF);
            nfe.ide.cUF = this.readInt32(nodeinfNFe, TpcnResources.cUF);
            nfe.ide.dEmi = this.readDate(nodeinfNFe, TpcnResources.dEmi);
            nfe.ide.dhCont = this.readValue(nodeinfNFe, TpcnResources.dhCont);
            nfe.ide.dSaiEnt = this.readDate(nodeinfNFe, TpcnResources.dSaiEnt);
            nfe.ide.finNFe = (TpcnFinalidadeNFe)this.readInt32(nodeinfNFe, TpcnResources.finNFe);
            nfe.ide.hSaiEnt = this.readDate(nodeinfNFe, TpcnResources.hSaiEnt);
            if(nfe.infNFe.Versao >= 3 && nfe.infNFe.Versao < 4)
                nfe.ide.indPag = (TpcnIndicadorPagamento)this.readInt32(nodeinfNFe, TpcnResources.indPag);
            nfe.ide.mod = (TpcnMod)this.readInt32(nodeinfNFe, TpcnResources.mod);
            nfe.ide.nNF = this.readInt32(nodeinfNFe, TpcnResources.nNF);
            nfe.ide.natOp = this.readValue(nodeinfNFe, TpcnResources.natOp);
            nfe.ide.procEmi = (TpcnProcessoEmissao)this.readInt32(nodeinfNFe, TpcnResources.procEmi);
            nfe.ide.serie = this.readInt32(nodeinfNFe, TpcnResources.serie);
            nfe.ide.tpAmb = (TipoAmbiente)this.readInt32(nodeinfNFe, TpcnResources.tpAmb);
            nfe.ide.tpEmis = (TipoEmissao)this.readInt32(nodeinfNFe, TpcnResources.tpEmis);
            nfe.ide.tpImp = (TpcnTipoImpressao)this.readInt32(nodeinfNFe, TpcnResources.tpImp);
            nfe.ide.tpNF = (TpcnTipoNFe)this.readInt32(nodeinfNFe, TpcnResources.tpNF);
            nfe.ide.verProc = this.readValue(nodeinfNFe, TpcnResources.verProc);
            nfe.ide.xJust = this.readValue(nodeinfNFe, TpcnResources.xJust);
            nfe.ide.dhEmi = (string)this.readValue(nodeinfNFe, TpcnResources.dhEmi);
            nfe.ide.dhSaiEnt = (string)this.readValue(nodeinfNFe, TpcnResources.dhSaiEnt);
            nfe.ide.idDest = (TpcnDestinoOperacao)this.readInt32(nodeinfNFe, TpcnResources.idDest);
            nfe.ide.indFinal = (TpcnConsumidorFinal)this.readInt32(nodeinfNFe, TpcnResources.indFinal);
            nfe.ide.indPres = (TpcnPresencaComprador)this.readInt32(nodeinfNFe, TpcnResources.indPres);
            nfe.ide.indIntermed = (TpcnIntermediario)this.readInt32(nodeinfNFe, TpcnResources.indIntermed);

            foreach(XmlNode nodeNFref in nodeinfNFe.ChildNodes)
            {
                if(nodeNFref.LocalName.Equals("NFref"))
                {
                    foreach(XmlNode nodeNFrefItem in nodeNFref.ChildNodes)
                    {
                        switch(nodeNFrefItem.LocalName)
                        {
                            case "refCTe":
                                nfe.ide.NFref.Add(new NFref("", nodeNFrefItem.InnerText));
                                break;

                            case "refNFe":
                                nfe.ide.NFref.Add(new NFref(nodeNFrefItem.InnerText, ""));
                                break;

                            case "refNF":
                                {
                                    NFref item_refNF = new NFref();
                                    item_refNF.refNF = new refNF();
                                    item_refNF.refNF.AAMM = this.readValue(nodeNFrefItem, TpcnResources.AAMM.ToString());
                                    item_refNF.refNF.CNPJ = this.readValue(nodeNFrefItem, TpcnResources.CNPJ);
                                    item_refNF.refNF.cUF = this.readInt32(nodeNFrefItem, TpcnResources.cUF);
                                    item_refNF.refNF.mod = this.readValue(nodeNFrefItem, TpcnResources.mod);
                                    item_refNF.refNF.nNF = this.readInt32(nodeNFrefItem, TpcnResources.nNF);
                                    item_refNF.refNF.serie = this.readInt32(nodeNFrefItem, TpcnResources.serie);
                                    nfe.ide.NFref.Add(item_refNF);
                                }
                                break;

                            case "refECF":
                                {
                                    NFref item_refECF = new NFref();
                                    item_refECF.refECF = new refECF();
                                    item_refECF.refECF.mod = this.readValue(nodeNFrefItem, TpcnResources.mod);
                                    item_refECF.refECF.nCOO = this.readInt32(nodeNFrefItem, TpcnResources.nCOO);
                                    item_refECF.refECF.nECF = this.readInt32(nodeNFrefItem, TpcnResources.nECF);
                                    nfe.ide.NFref.Add(item_refECF);
                                }
                                break;

                            case "refNFP":
                                {
                                    NFref item_refNFP = new NFref();
                                    item_refNFP.refNFP = new refNFP();
                                    item_refNFP.refNFP.AAMM = this.readValue(nodeNFrefItem, TpcnResources.AAMM.ToString());
                                    item_refNFP.refNFP.CNPJ = this.readValue(nodeNFrefItem, TpcnResources.CNPJ);
                                    item_refNFP.refNFP.CPF = this.readValue(nodeNFrefItem, TpcnResources.CPF);
                                    item_refNFP.refNFP.cUF = this.readInt32(nodeNFrefItem, TpcnResources.cUF);
                                    item_refNFP.refNFP.IE = this.readValue(nodeNFrefItem, TpcnResources.IE);
                                    item_refNFP.refNFP.mod = this.readValue(nodeNFrefItem, TpcnResources.mod);
                                    item_refNFP.refNFP.nNF = this.readInt32(nodeNFrefItem, TpcnResources.nNF);
                                    item_refNFP.refNFP.serie = this.readInt32(nodeNFrefItem, TpcnResources.serie);
                                    nfe.ide.NFref.Add(item_refNFP);
                                }
                                break;
                        }
                    }
                }
            }
        }
    }
}
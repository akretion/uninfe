using System;
using System.Collections.Generic;
using System.Linq;
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

        private DateTime readDate(object element, string tag)
        {
            string temp = this.readValue(element, tag).Trim();
            return (temp != "" ? Convert.ToDateTime(temp) : DateTime.MinValue);
        }

        private double readDouble(object element, string tag)
        {
            char charSeparator = System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator[0];
            return Convert.ToDouble("0" + this.readValue(element, tag).Replace(".", charSeparator.ToString()));
        }

        private Int32 readInt32(object element, string tag)
        {
            return Convert.ToInt32("0" + this.readValue(element, tag));
        }

        private string readValue(object element, string tag)
        {
            if (((XmlElement)element).GetElementsByTagName(tag)[0] != null)
                return ((XmlElement)element).GetElementsByTagName(tag)[0].InnerText;

            return "";
        }

        public void ReadFromString(string xmlString)
        {
            doc = new XmlDocument();
            try
            {
                doc.LoadXml(xmlString);
                this.XmlNota = doc.OuterXml;

                foreach (XmlNode nodeRoot in doc.ChildNodes)
                {
                    if (nodeRoot.LocalName.Equals("NFe"))
                    {
                        processaNFe(nodeRoot);
                    }
                    if (nodeRoot.LocalName.Equals("nfeProc"))
                    {
                        foreach (XmlNode nodenfeProc in nodeRoot.ChildNodes)
                        {
                            switch (nodenfeProc.LocalName.ToLower())
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
                if (nfe.ide.dEmi.Year <= 1)
                    throw new Exception("Arquivo não é de nota fiscal");
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

        private void processaNFe(XmlNode nodeRoot)
        {
            foreach (XmlNode nodeNFe in nodeRoot.ChildNodes)
            {
                switch (nodeNFe.LocalName.ToLower())
                {
                    case "infnfe":
                        nfe.infNFe.ID = nodeNFe.Attributes["Id"].Value;
                        foreach (XmlNode nodeinfNFe in nodeNFe.ChildNodes)
                        {
                            switch (nodeinfNFe.LocalName)
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
                            }
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// processaProtNfe
        /// </summary>
        /// <param name="nodenfeProc"></param>
        private void processaProtNfe(XmlNode nodenfeProc)
        {
            nfe.protNFe.chNFe = this.readValue(nodenfeProc, "chNFe");
            nfe.protNFe.cStat = this.readInt32(nodenfeProc, "cStat");
            nfe.protNFe.dhRecbto = this.readDate(nodenfeProc, "dhRecbto");
            nfe.protNFe.digVal = this.readValue(nodenfeProc, "digVal");
            nfe.protNFe.nProt = this.readValue(nodenfeProc, "nProt");
            nfe.protNFe.tpAmb = (TpcnTipoAmbiente)this.readInt32(nodenfeProc, Properties.Resources.tpAmb);
            nfe.protNFe.verAplic = this.readValue(nodenfeProc, "verAplic");
            nfe.protNFe.xMotivo = this.readValue(nodenfeProc, "xMotivo");
        }

        /// <summary>
        /// processaCana
        /// </summary>
        /// <param name="nodeinfNFe"></param>
        private void processaCana(XmlNode nodeinfNFe)
        {
            nfe.cana.safra = this.readValue(nodeinfNFe, Properties.Resources.safra);
            nfe.cana.Ref = this.readValue(nodeinfNFe, Properties.Resources.Ref);
            nfe.cana.qTotMes = this.readDouble(nodeinfNFe, Properties.Resources.qTotMes);
            nfe.cana.qTotAnt = this.readDouble(nodeinfNFe, Properties.Resources.qTotAnt);
            nfe.cana.qTotGer = this.readDouble(nodeinfNFe, Properties.Resources.qTotGer);
            nfe.cana.vFor = this.readDouble(nodeinfNFe, Properties.Resources.vFor);
            nfe.cana.vTotDed = this.readDouble(nodeinfNFe, Properties.Resources.vTotDed);
            nfe.cana.vLiqFor = this.readDouble(nodeinfNFe, Properties.Resources.vLiqFor);

            foreach (XmlNode noder in nodeinfNFe.ChildNodes)
            {
                switch (noder.LocalName)
                {
                    case "forDia":
                        {
                            fordia fordiaInfo = new fordia();
                            if (noder.Attributes.Count > 0)
                                fordiaInfo.dia = Convert.ToInt32(noder.Attributes[Properties.Resources.dia].Value);
                            else
                                fordiaInfo.dia = this.readInt32(noder, Properties.Resources.dia);
                            fordiaInfo.qtde = this.readDouble(noder, Properties.Resources.qtde);
                            nfe.cana.fordia.Add(fordiaInfo);
                        }
                        break;

                    case "deduc":
                        {
                            deduc deducInfo = new deduc();
                            deducInfo.xDed = this.readValue(noder, Properties.Resources.xDed);
                            deducInfo.vDed = this.readDouble(noder, Properties.Resources.vDed);
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
            nfe.compra.xNEmp = this.readValue(nodeinfNFe, Properties.Resources.xNEmp);
            nfe.compra.xPed = this.readValue(nodeinfNFe, Properties.Resources.xPed);
            nfe.compra.xCont = this.readValue(nodeinfNFe, Properties.Resources.xCont);
        }

        /// <summary>
        /// processaExporta
        /// </summary>
        /// <param name="nodeinfNFe"></param>
        private void processaExporta(XmlNode nodeinfNFe)
        {
            nfe.exporta.UFEmbarq = this.readValue(nodeinfNFe, Properties.Resources.UFEmbarq);
            nfe.exporta.xLocEmbarq = this.readValue(nodeinfNFe, Properties.Resources.xLocEmbarq);
        }

        /// <summary>
        /// processaInfAdic
        /// </summary>
        /// <param name="nodeinfNFe"></param>
        private void processaInfAdic(XmlNode nodeinfNFe)
        {
            nfe.InfAdic.infAdFisco = this.readValue(nodeinfNFe, Properties.Resources.infAdFisco);
            nfe.InfAdic.infCpl = this.readValue(nodeinfNFe, Properties.Resources.infCpl);
            foreach (XmlNode noder in nodeinfNFe.ChildNodes)
            {
                switch (noder.LocalName)
                {
                    case "obsCont":
                        {
                            obsCont obscontInfo = new obsCont();
                            obscontInfo.xCampo = noder.Attributes[Properties.Resources.xCampo].Value;
                            obscontInfo.xTexto = this.readValue(noder, Properties.Resources.xTexto);
                            nfe.InfAdic.obsCont.Add(obscontInfo);
                        }
                        break;

                    case "obsFisco":
                        {
                            obsFisco obsfiscoInfo = new obsFisco();
                            obsfiscoInfo.xCampo = noder.Attributes[Properties.Resources.xCampo].Value;
                            obsfiscoInfo.xTexto = this.readValue(noder, Properties.Resources.xTexto);
                            nfe.InfAdic.obsFisco.Add(obsfiscoInfo);
                        }
                        break;

                    case "procRef":
                        {
                            procRef procrefInfo = new procRef();
                            procrefInfo.nProc = this.readValue(noder, Properties.Resources.nProc);
                            procrefInfo.indProc = this.readValue(noder, Properties.Resources.indProc);
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
            nfe.retirada.CNPJ = this.readValue(nodeinfNFe, Properties.Resources.CNPJ);
            nfe.retirada.CPF = this.readValue(nodeinfNFe, Properties.Resources.CPF);
            nfe.retirada.xLgr = this.readValue(nodeinfNFe, Properties.Resources.xLgr);
            nfe.retirada.nro = this.readValue(nodeinfNFe, Properties.Resources.nro);
            nfe.retirada.xCpl = this.readValue(nodeinfNFe, Properties.Resources.xCpl);
            nfe.retirada.xBairro = this.readValue(nodeinfNFe, Properties.Resources.xBairro);
            nfe.retirada.cMun = this.readInt32(nodeinfNFe, Properties.Resources.cMun);
            nfe.retirada.xMun = this.readValue(nodeinfNFe, Properties.Resources.xMun);
            nfe.retirada.UF = this.readValue(nodeinfNFe, Properties.Resources.UF);
        }

        /// <summary>
        /// processaEntrega
        /// </summary>
        /// <param name="nodeinfNFe"></param>
        private void processaEntrega(XmlNode nodeinfNFe)
        {
            nfe.entrega.CNPJ = this.readValue(nodeinfNFe, Properties.Resources.CNPJ);
            nfe.entrega.CPF = this.readValue(nodeinfNFe, Properties.Resources.CPF);
            nfe.entrega.xLgr = this.readValue(nodeinfNFe, Properties.Resources.xLgr);
            nfe.entrega.nro = this.readValue(nodeinfNFe, Properties.Resources.nro);
            nfe.entrega.xCpl = this.readValue(nodeinfNFe, Properties.Resources.xCpl);
            nfe.entrega.xBairro = this.readValue(nodeinfNFe, Properties.Resources.xBairro);
            nfe.entrega.cMun = this.readInt32(nodeinfNFe, Properties.Resources.cMun);
            nfe.entrega.xMun = this.readValue(nodeinfNFe, Properties.Resources.xMun);
            nfe.entrega.UF = this.readValue(nodeinfNFe, Properties.Resources.UF);
        }

        /// <summary>
        /// processaCOBR
        /// </summary>
        /// <param name="nodeinfNFe"></param>
        private void processaCOBR(XmlNode nodeinfNFe)
        {
            foreach (XmlNode noder in nodeinfNFe.ChildNodes)
            {
                if (noder.LocalName.Equals("fat"))
                {
                    nfe.Cobr.Fat.nFat = this.readValue(noder, Properties.Resources.nFat);
                    nfe.Cobr.Fat.vOrig = this.readDouble(noder, Properties.Resources.vOrig);
                    nfe.Cobr.Fat.vDesc = this.readDouble(noder, Properties.Resources.vDesc);
                    nfe.Cobr.Fat.vLiq = this.readDouble(noder, Properties.Resources.vLiq);
                }
                if (noder.LocalName.Equals("dup"))
                {
                    Dup dupInfo = new Dup();
                    dupInfo.dVenc = this.readDate(noder, Properties.Resources.dVenc);
                    dupInfo.nDup = this.readValue(noder, Properties.Resources.nDup);
                    dupInfo.vDup = this.readDouble(noder, Properties.Resources.vDup);
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
            foreach (XmlNode noder in nodeinfNFe.ChildNodes)
            {
                switch (noder.LocalName.ToLower())
                {
                    case "modfrete":
                        nfe.Transp.modFrete = (TpcnModalidadeFrete)this.readInt32(noder, Properties.Resources.modFrete);
                        break;

                    case "transporta":
                        {
                            nfe.Transp.Transporta.CNPJ = this.readValue(noder, Properties.Resources.CNPJ);
                            nfe.Transp.Transporta.CPF = this.readValue(noder, Properties.Resources.CPF);
                            nfe.Transp.Transporta.xNome = this.readValue(noder, Properties.Resources.xNome);
                            nfe.Transp.Transporta.IE = this.readValue(noder, Properties.Resources.IE);
                            nfe.Transp.Transporta.xEnder = this.readValue(noder, Properties.Resources.xEnder);
                            nfe.Transp.Transporta.xMun = this.readValue(noder, Properties.Resources.xMun);
                            nfe.Transp.Transporta.UF = this.readValue(noder, Properties.Resources.UF);
                        }
                        break;

                    case "rettransp":
                        {
                            nfe.Transp.retTransp.vServ = this.readDouble(noder, Properties.Resources.vServ);
                            nfe.Transp.retTransp.vBCRet = this.readDouble(noder, Properties.Resources.vBCRet);
                            nfe.Transp.retTransp.pICMSRet = this.readDouble(noder, Properties.Resources.pICMSRet);
                            nfe.Transp.retTransp.vICMSRet = this.readDouble(noder, Properties.Resources.vICMSRet);
                            nfe.Transp.retTransp.CFOP = this.readValue(noder, Properties.Resources.CFOP);
                            nfe.Transp.retTransp.cMunFG = this.readInt32(noder, Properties.Resources.cMunFG);
                        }
                        break;

                    case "veictransp":
                        {
                            nfe.Transp.veicTransp.placa = this.readValue(noder, Properties.Resources.placa);
                            nfe.Transp.veicTransp.UF = this.readValue(noder, Properties.Resources.UF);
                            nfe.Transp.veicTransp.RNTC = this.readValue(noder, Properties.Resources.RNTC);
                        }
                        break;

                    case "reboque":
                        {
                            Reboque reboqueInfo = new Reboque();
                            reboqueInfo.placa = this.readValue(noder, Properties.Resources.placa);
                            reboqueInfo.UF = this.readValue(noder, Properties.Resources.UF);
                            reboqueInfo.RNTC = this.readValue(noder, Properties.Resources.RNTC);

                            nfe.Transp.Reboque.Add(reboqueInfo);
                        }
                        break;

                    case "vol":
                        {
                            Vol volInfo = new Vol();
                            volInfo.qVol = this.readInt32(noder, Properties.Resources.qVol);
                            volInfo.esp = this.readValue(noder, Properties.Resources.esp);
                            volInfo.marca = this.readValue(noder, Properties.Resources.marca);
                            volInfo.nVol = this.readValue(noder, Properties.Resources.nVol);
                            volInfo.pesoL = this.readDouble(noder, Properties.Resources.pesoL);
                            volInfo.pesoB = this.readDouble(noder, Properties.Resources.pesoB);

                            foreach (XmlNode nodevollacre in ((XmlElement)noder).GetElementsByTagName("lacres"))
                            {
                                Lacres lacresInfo = new Lacres();
                                lacresInfo.nLacre = this.readValue(nodevollacre, Properties.Resources.nLacre);
                                volInfo.Lacres.Add(lacresInfo);
                            }
                            nfe.Transp.Vol.Add(volInfo);
                        }
                        break;

                    case "vagao":
                        nfe.Transp.vagao = this.readValue(noder, Properties.Resources.vagao);
                        break;

                    case "balsa":
                        nfe.Transp.balsa = this.readValue(noder, Properties.Resources.balsa);
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
            foreach (XmlNode nodeICMSTot in ((XmlElement)nodetotal).GetElementsByTagName("ICMSTot"))
            {
                nfe.Total.ICMSTot.vBC = this.readDouble(nodeICMSTot, Properties.Resources.vBC);
                nfe.Total.ICMSTot.vBCST = this.readDouble(nodeICMSTot, Properties.Resources.vBCST);
                nfe.Total.ICMSTot.vCOFINS = this.readDouble(nodeICMSTot, Properties.Resources.vCOFINS);
                nfe.Total.ICMSTot.vDesc = this.readDouble(nodeICMSTot, Properties.Resources.vDesc);
                nfe.Total.ICMSTot.vFrete = this.readDouble(nodeICMSTot, Properties.Resources.vFrete);
                nfe.Total.ICMSTot.vICMS = this.readDouble(nodeICMSTot, Properties.Resources.vICMS);
                nfe.Total.ICMSTot.vII = this.readDouble(nodeICMSTot, Properties.Resources.vII);
                nfe.Total.ICMSTot.vIPI = this.readDouble(nodeICMSTot, Properties.Resources.vIPI);
                nfe.Total.ICMSTot.vNF = this.readDouble(nodeICMSTot, Properties.Resources.vNF);
                nfe.Total.ICMSTot.vOutro = this.readDouble(nodeICMSTot, Properties.Resources.vOutro);
                nfe.Total.ICMSTot.vPIS = this.readDouble(nodeICMSTot, Properties.Resources.vPIS);
                nfe.Total.ICMSTot.vProd = this.readDouble(nodeICMSTot, Properties.Resources.vProd);
                nfe.Total.ICMSTot.vSeg = this.readDouble(nodeICMSTot, Properties.Resources.vSeg);
                nfe.Total.ICMSTot.vST = this.readDouble(nodeICMSTot, Properties.Resources.vST);
            }

            foreach (XmlNode nodeISSQNtot in ((XmlElement)nodetotal).GetElementsByTagName("ISSQNtot"))
            {
                nfe.Total.ISSQNtot.vServ = this.readDouble(nodeISSQNtot, Properties.Resources.vServ);
                nfe.Total.ISSQNtot.vBC = this.readDouble(nodeISSQNtot, Properties.Resources.vBC);
                nfe.Total.ISSQNtot.vISS = this.readDouble(nodeISSQNtot, Properties.Resources.vISS);
                nfe.Total.ISSQNtot.vPIS = this.readDouble(nodeISSQNtot, Properties.Resources.vPIS);
                nfe.Total.ISSQNtot.vCOFINS = this.readDouble(nodeISSQNtot, Properties.Resources.vCOFINS);
            }

            foreach (XmlNode noderetTrib in ((XmlElement)nodetotal).GetElementsByTagName("retTrib"))
            {
                nfe.Total.retTrib.vRetPIS = this.readDouble(noderetTrib, Properties.Resources.vRetPIS);
                nfe.Total.retTrib.vRetCOFINS = this.readDouble(noderetTrib, Properties.Resources.vRetCOFINS);
                nfe.Total.retTrib.vRetCSLL = this.readDouble(noderetTrib, Properties.Resources.vRetCSLL);
                nfe.Total.retTrib.vBCIRRF = this.readDouble(noderetTrib, Properties.Resources.vBCIRRF);
                nfe.Total.retTrib.vIRRF = this.readDouble(noderetTrib, Properties.Resources.vIRRF);
                nfe.Total.retTrib.vBCRetPrev = this.readDouble(noderetTrib, Properties.Resources.vBCRetPrev);
                nfe.Total.retTrib.vRetPrev = this.readDouble(noderetTrib, Properties.Resources.vRetPrev);
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
            detInfo.infAdProd = this.readValue(nodedet, Properties.Resources.infAdProd);

            foreach (XmlNode nodedetprod in ((XmlElement)nodedet).GetElementsByTagName("prod"))
            {
                XmlElement ele = nodedetprod as XmlElement;

                detInfo.Prod.cEAN = this.readValue(ele, Properties.Resources.cEAN);
                detInfo.Prod.cEANTrib = this.readValue(ele, Properties.Resources.cEANTrib);
                detInfo.Prod.CFOP = this.readValue(ele, Properties.Resources.CFOP);
                detInfo.Prod.cProd = this.readValue(ele, Properties.Resources.cProd);
                detInfo.Prod.EXTIPI = this.readValue(ele, Properties.Resources.EXTIPI);
                detInfo.Prod.indTot = (TpcnIndicadorTotal)this.readInt32(ele, Properties.Resources.indTot);
                detInfo.Prod.NCM = this.readValue(ele, Properties.Resources.NCM);
                detInfo.Prod.nItemPed = this.readInt32(ele, Properties.Resources.nItemPed);
                detInfo.Prod.qCom = this.readDouble(ele, Properties.Resources.qCom);
                detInfo.Prod.qTrib = this.readDouble(ele, Properties.Resources.qTrib);
                detInfo.Prod.uCom = this.readValue(ele, Properties.Resources.uCom);
                detInfo.Prod.uTrib = this.readValue(ele, Properties.Resources.uTrib);
                detInfo.Prod.vDesc = this.readDouble(ele, Properties.Resources.vDesc);
                detInfo.Prod.vFrete = this.readDouble(ele, Properties.Resources.vFrete);
                detInfo.Prod.vOutro = this.readDouble(ele, Properties.Resources.vOutro);
                detInfo.Prod.vProd = this.readDouble(ele, Properties.Resources.vProd);
                detInfo.Prod.vSeg = this.readDouble(ele, Properties.Resources.vSeg);
                detInfo.Prod.vUnCom = this.readDouble(ele, Properties.Resources.vUnCom);
                detInfo.Prod.vUnTrib = this.readDouble(ele, Properties.Resources.vUnTrib);
                detInfo.Prod.xPed = this.readValue(ele, Properties.Resources.xPed);
                detInfo.Prod.xProd = this.readValue(ele, Properties.Resources.xProd);
            }

            #region -->prod->arma
            foreach (XmlNode nodedetArma in ((XmlElement)nodedet).GetElementsByTagName("arma"))
            {
                Arma armaInfo = new Arma();

                armaInfo.descr = this.readValue(nodedetArma, Properties.Resources.descr);
                armaInfo.nCano = this.readInt32(nodedetArma, Properties.Resources.nCano);
                armaInfo.nSerie = this.readInt32(nodedetArma, Properties.Resources.nSerie);
                armaInfo.tpArma = (TpcnTipoArma)this.readInt32(nodedetArma, Properties.Resources.tpArma);

                detInfo.Prod.arma.Add(armaInfo);
            }
            #endregion

            #region --prod->comb
            foreach (XmlNode nodedetComb in ((XmlElement)nodedet).GetElementsByTagName("comb"))
            {
                foreach (XmlNode nodedetCombCIDE in ((XmlElement)nodedetComb).GetElementsByTagName("CIDE"))
                {
                    detInfo.Prod.comb.CIDE.qBCprod = this.readDouble(nodedetCombCIDE, Properties.Resources.qBCProd);
                    detInfo.Prod.comb.CIDE.vAliqProd = this.readDouble(nodedetCombCIDE, Properties.Resources.vAliqProd);
                    detInfo.Prod.comb.CIDE.vCIDE = this.readDouble(nodedetCombCIDE, Properties.Resources.vCIDE);
                }
                detInfo.Prod.comb.CODIF = this.readValue(nodedetComb, Properties.Resources.CODIF);
                detInfo.Prod.comb.cProdANP = this.readInt32(nodedetComb, Properties.Resources.cProdANP);
                detInfo.Prod.comb.qTemp = this.readDouble(nodedetComb, Properties.Resources.qTemp);
                detInfo.Prod.comb.UFCons = this.readValue(nodedetComb, Properties.Resources.UFCons);

                //foreach (XmlNode nodedetCombicmscomb in ((XmlElement)nodedetComb).GetElementsByTagName("ICMSComb"))
                //{
                //detInfo.Prod.comb.ICMS.vBCICMS   = Leitor.rCampo(tcDe2, 'vBCICMS');
                //detInfo.Prod.comb.ICMS.vICMS     = Leitor.rCampo(tcDe2, 'vICMS');
                //detInfo.Prod.comb.ICMS.vBCICMSST = Leitor.rCampo(tcDe2, 'vBCICMSST');
                //detInfo.Prod.comb.ICMS.vICMSST   = Leitor.rCampo(tcDe2, 'vICMSST');
                //}
                //foreach (XmlNode nodedetCombicmsInter in ((XmlElement)nodedetComb).GetElementsByTagName("ICMSInter"))
                //{
                //NFe.Det[i].Prod.comb.ICMSInter.vBCICMSSTDest := Leitor.rCampo(tcDe2, 'vBCICMSSTDest');
                //NFe.Det[i].Prod.comb.ICMSInter.vICMSSTDest := Leitor.rCampo(tcDe2, 'vICMSSTDest');
                //}
                //foreach (XmlNode nodedetCombicmsCons in ((XmlElement)nodedetComb).GetElementsByTagName("ICMSCons"))
                //{
                //NFe.Det[i].Prod.comb.ICMSCons.vBCICMSSTCons := Leitor.rCampo(tcDe2, 'vBCICMSSTCons');
                //NFe.Det[i].Prod.comb.ICMSCons.vICMSSTCons   := Leitor.rCampo(tcDe2, 'vICMSSTCons');
                //NFe.Det[i].Prod.comb.ICMSCons.UFcons        := Leitor.rCampo(tcStr, 'UFCons');
                //}

            }
            #endregion

            #region --prod->med
            foreach (XmlNode nodedetmed in ((XmlElement)nodedet).GetElementsByTagName("med"))
            {
                Med medInfo = new Med();
                medInfo.dFab = this.readDate(nodedetmed, Properties.Resources.dFab);
                medInfo.dVal = this.readDate(nodedetmed, Properties.Resources.dVal);
                medInfo.nLote = this.readValue(nodedetmed, Properties.Resources.nLote);
                medInfo.qLote = this.readDouble(nodedetmed, Properties.Resources.qLote);
                medInfo.vPMC = this.readDouble(nodedetmed, Properties.Resources.vPMC);
                detInfo.Prod.med.Add(medInfo);
            }
            #endregion

            #region --prod->veicProd
            foreach (XmlNode nodedetveic in ((XmlElement)nodedet).GetElementsByTagName("veicProd"))
            {
                detInfo.Prod.veicProd.anoFab = this.readInt32(nodedetveic, Properties.Resources.anoFab);
                detInfo.Prod.veicProd.anoMod = this.readInt32(nodedetveic, Properties.Resources.anoMod);
                detInfo.Prod.veicProd.cCor = this.readValue(nodedetveic, Properties.Resources.cCor);
                detInfo.Prod.veicProd.cCorDENATRAN = this.readInt32(nodedetveic, Properties.Resources.cCorDENATRAN);
                detInfo.Prod.veicProd.chassi = this.readValue(nodedetveic, Properties.Resources.chassi);
                detInfo.Prod.veicProd.cilin = this.readValue(nodedetveic, Properties.Resources.cilin);
                detInfo.Prod.veicProd.cMod = this.readValue(nodedetveic, Properties.Resources.cMod);
                detInfo.Prod.veicProd.CMT = this.readValue(nodedetveic, Properties.Resources.CMT);
                detInfo.Prod.veicProd.condVeic = this.readValue(nodedetveic, Properties.Resources.condVeic);
                detInfo.Prod.veicProd.dist = this.readValue(nodedetveic, Properties.Resources.dist);
                detInfo.Prod.veicProd.espVeic = this.readInt32(nodedetveic, Properties.Resources.espVeic);
                detInfo.Prod.veicProd.lota = this.readInt32(nodedetveic, Properties.Resources.lota);
                detInfo.Prod.veicProd.nMotor = this.readValue(nodedetveic, Properties.Resources.nMotor);
                detInfo.Prod.veicProd.nSerie = this.readValue(nodedetveic, Properties.Resources.nSerie);
                detInfo.Prod.veicProd.pesoB = this.readValue(nodedetveic, Properties.Resources.pesoB);
                detInfo.Prod.veicProd.pesoL = this.readValue(nodedetveic, Properties.Resources.pesoL);
                detInfo.Prod.veicProd.pot = this.readValue(nodedetveic, Properties.Resources.pot);
                detInfo.Prod.veicProd.tpComb = this.readValue(nodedetveic, Properties.Resources.tpComb);
                detInfo.Prod.veicProd.tpOp = this.readValue(nodedetveic, Properties.Resources.tpOp);
                detInfo.Prod.veicProd.tpPint = this.readValue(nodedetveic, Properties.Resources.tpPint);
                detInfo.Prod.veicProd.tpRest = this.readInt32(nodedetveic, Properties.Resources.tpRest);
                detInfo.Prod.veicProd.tpVeic = this.readInt32(nodedetveic, Properties.Resources.tpVeic);
                detInfo.Prod.veicProd.VIN = this.readValue(nodedetveic, Properties.Resources.VIN);
                detInfo.Prod.veicProd.xCor = this.readValue(nodedetveic, Properties.Resources.xCor);
            }
            #endregion

            #region --pod->DI
            foreach (XmlNode nodedetDI in ((XmlElement)nodedet).GetElementsByTagName("DI"))
            {
                DI diInfo = new DI();
                diInfo.cExportador = this.readValue(nodedetDI, Properties.Resources.cExportador);
                diInfo.dDesemb = this.readDate(nodedetDI, Properties.Resources.dDesemb);
                diInfo.dDI = this.readDate(nodedetDI, Properties.Resources.dDI);
                diInfo.nDI = this.readValue(nodedetDI, Properties.Resources.nDI);
                diInfo.xLocDesemb = this.readValue(nodedetDI, Properties.Resources.xLocDesemb);
                diInfo.UFDesemb = this.readValue(nodedetDI, Properties.Resources.UFDesemb);

                foreach (XmlNode nodedetDIadi in ((XmlElement)nodedetDI).GetElementsByTagName("adi"))
                {
                    Adi adiInfo;

                    adiInfo.cFabricante = this.readValue(nodedetDIadi, Properties.Resources.cFabricante);
                    adiInfo.nAdicao = this.readInt32(nodedetDIadi, Properties.Resources.nAdicao);
                    adiInfo.nSeqAdi = this.readInt32(nodedetDIadi, Properties.Resources.nSeqAdic);
                    adiInfo.vDescDI = this.readDouble(nodedetDIadi, Properties.Resources.vDescDI);

                    diInfo.adi.Add(adiInfo);
                }
                detInfo.Prod.DI.Add(diInfo);
            }
            #endregion

            foreach (XmlNode nodedetImposto in ((XmlElement)nodedet).GetElementsByTagName("imposto"))
            {
                #region -->Imposto->ICMS
                foreach (XmlNode nodedetImpostoICMS in ((XmlElement)nodedetImposto).GetElementsByTagName(Properties.Resources.ICMS))
                {
                    if (nodedetImpostoICMS.ChildNodes.Count > 0)
                    {
                        XmlNode nodedetImpostoICMS_ = nodedetImpostoICMS.ChildNodes[0];

                        detInfo.Imposto.ICMS.CST = this.readValue(nodedetImpostoICMS_, Properties.Resources.CST);
                        detInfo.Imposto.ICMS.CSOSN = this.readInt32(nodedetImpostoICMS_, Properties.Resources.CSOSN);
                        detInfo.Imposto.ICMS.modBC = (TpcnDeterminacaoBaseIcms)this.readInt32(nodedetImpostoICMS_, Properties.Resources.modBC);
                        detInfo.Imposto.ICMS.modBCST = (TpcnDeterminacaoBaseIcmsST)this.readInt32(nodedetImpostoICMS_, Properties.Resources.modBCST);
                        detInfo.Imposto.ICMS.motDesICMS = this.readInt32(nodedetImpostoICMS_, Properties.Resources.motDesICMS);
                        detInfo.Imposto.ICMS.orig = (TpcnOrigemMercadoria)this.readInt32(nodedetImpostoICMS_, Properties.Resources.orig);
                        detInfo.Imposto.ICMS.pCredSN = this.readDouble(nodedetImpostoICMS_, Properties.Resources.pCredSN);
                        detInfo.Imposto.ICMS.pICMS = this.readDouble(nodedetImpostoICMS_, Properties.Resources.pICMS);
                        detInfo.Imposto.ICMS.pICMSST = this.readDouble(nodedetImpostoICMS_, Properties.Resources.pICMSST);
                        detInfo.Imposto.ICMS.pMVAST = this.readDouble(nodedetImpostoICMS_, Properties.Resources.pMVAST);
                        detInfo.Imposto.ICMS.pRedBC = this.readDouble(nodedetImpostoICMS_, Properties.Resources.pRedBC);
                        detInfo.Imposto.ICMS.pRedBCST = this.readDouble(nodedetImpostoICMS_, Properties.Resources.pRedBCST);
                        detInfo.Imposto.ICMS.UFST = this.readValue(nodedetImpostoICMS_, Properties.Resources.UFST);
                        detInfo.Imposto.ICMS.vBC = this.readDouble(nodedetImpostoICMS_, Properties.Resources.vBC);
                        detInfo.Imposto.ICMS.vBCOp = this.readDouble(nodedetImpostoICMS_, Properties.Resources.vBCOp);
                        detInfo.Imposto.ICMS.vBCST = this.readDouble(nodedetImpostoICMS_, Properties.Resources.vBCST);
                        detInfo.Imposto.ICMS.vBCSTDest = this.readDouble(nodedetImpostoICMS_, Properties.Resources.vBCSTDest);
                        detInfo.Imposto.ICMS.vBCSTRet = this.readDouble(nodedetImpostoICMS_, Properties.Resources.vBCSTRet);
                        detInfo.Imposto.ICMS.vCredICMSSN = this.readDouble(nodedetImpostoICMS_, Properties.Resources.vCredICMSSN);
                        detInfo.Imposto.ICMS.vICMS = this.readDouble(nodedetImpostoICMS_, Properties.Resources.vICMS);
                        detInfo.Imposto.ICMS.vICMSST = this.readDouble(nodedetImpostoICMS_, Properties.Resources.vICMSST);
                        detInfo.Imposto.ICMS.vICMSSTDest = this.readDouble(nodedetImpostoICMS_, Properties.Resources.vICMSSTDest);
                        detInfo.Imposto.ICMS.vICMSSTRet = this.readDouble(nodedetImpostoICMS_, Properties.Resources.vICMSSTRet);
                    }
                }
                #endregion

                // Inicializa CST com sendo Não tributada e conforme o TIPO entrada ou saida
                // Caso a Tag não seja informada sera gravada com sendo não tributada
                if (nfe.ide.tpNF == TpcnTipoNFe.tnEntrada)
                    detInfo.Imposto.IPI.CST = "53";
                else
                    detInfo.Imposto.IPI.CST = "03";

                #region --Imposto->IPI
                foreach (XmlNode nodedetImpostoIPI in ((XmlElement)nodedetImposto).GetElementsByTagName("IPI"))
                {
                    detInfo.Imposto.IPI.cEnq = this.readValue(nodedetImpostoIPI, Properties.Resources.cEnq);
                    detInfo.Imposto.IPI.clEnq = this.readValue(nodedetImpostoIPI, Properties.Resources.clEnq);
                    detInfo.Imposto.IPI.CNPJProd = this.readValue(nodedetImpostoIPI, Properties.Resources.CNPJProd);
                    detInfo.Imposto.IPI.cSelo = this.readValue(nodedetImpostoIPI, Properties.Resources.cSelo);
                    detInfo.Imposto.IPI.qSelo = this.readInt32(nodedetImpostoIPI, Properties.Resources.qSelo);

                    foreach (XmlNode nodedetImpostoIPITrib in ((XmlElement)nodedetImpostoIPI).GetElementsByTagName("IPITrib"))
                    {
                        detInfo.Imposto.IPI.CST = this.readValue(nodedetImpostoIPITrib, Properties.Resources.CST);
                        detInfo.Imposto.IPI.pIPI = this.readDouble(nodedetImpostoIPITrib, Properties.Resources.pIPI);
                        detInfo.Imposto.IPI.pIPI = this.readDouble(nodedetImpostoIPITrib, Properties.Resources.pIPI);
                        detInfo.Imposto.IPI.qUnid = this.readDouble(nodedetImpostoIPITrib, Properties.Resources.qUnid);
                        detInfo.Imposto.IPI.vBC = this.readDouble(nodedetImpostoIPITrib, Properties.Resources.vBC);
                        detInfo.Imposto.IPI.vIPI = this.readDouble(nodedetImpostoIPITrib, Properties.Resources.vIPI);
                        detInfo.Imposto.IPI.vUnid = this.readDouble(nodedetImpostoIPITrib, Properties.Resources.vUnid);
                    }
                    foreach (XmlNode nodedetImpostoIPInt in ((XmlElement)nodedetImpostoIPI).GetElementsByTagName("IPINT"))
                    {
                        detInfo.Imposto.IPI.CST = this.readValue(nodedetImpostoIPInt, Properties.Resources.CST);
                    }
                }
                #endregion

                #region --Imposto->II
                foreach (XmlNode nodedetImpostoII in ((XmlElement)nodedetImposto).GetElementsByTagName("II"))
                {
                    detInfo.Imposto.II.vBC = this.readDouble(nodedetImpostoII, Properties.Resources.vBC);
                    detInfo.Imposto.II.vDespAdu = this.readDouble(nodedetImpostoII, Properties.Resources.vDespAdu);
                    detInfo.Imposto.II.vII = this.readDouble(nodedetImpostoII, Properties.Resources.vII);
                    detInfo.Imposto.II.vIOF = this.readDouble(nodedetImpostoII, Properties.Resources.vIOF);
                }
                #endregion

                #region --Imposto->PIS
                foreach (XmlNode nodedetImpostoPIS in ((XmlElement)nodedetImposto).GetElementsByTagName("PIS"))
                {
                    detInfo.Imposto.PIS.CST = this.readValue(nodedetImpostoPIS, Properties.Resources.CST);
                    detInfo.Imposto.PIS.vBC = this.readDouble(nodedetImpostoPIS, Properties.Resources.vBC);
                    detInfo.Imposto.PIS.pPIS = this.readDouble(nodedetImpostoPIS, Properties.Resources.pPIS);
                    detInfo.Imposto.PIS.vPIS = this.readDouble(nodedetImpostoPIS, Properties.Resources.vPIS);
                    detInfo.Imposto.PIS.qBCProd = this.readDouble(nodedetImpostoPIS, Properties.Resources.qBCProd);
                    detInfo.Imposto.PIS.vAliqProd = this.readDouble(nodedetImpostoPIS, Properties.Resources.vAliqProd);
                }
                #endregion

                #region --Imposto->PISST
                foreach (XmlNode nodedetImpostoPISst in ((XmlElement)nodedetImposto).GetElementsByTagName("PISST"))
                {
                    detInfo.Imposto.PISST.vBC = this.readDouble(nodedetImpostoPISst, Properties.Resources.vBC);
                    detInfo.Imposto.PISST.pPis = this.readDouble(nodedetImpostoPISst, Properties.Resources.pPIS);
                    detInfo.Imposto.PISST.qBCProd = this.readDouble(nodedetImpostoPISst, Properties.Resources.qBCProd);
                    detInfo.Imposto.PISST.vAliqProd = this.readDouble(nodedetImpostoPISst, Properties.Resources.vAliqProd);
                    detInfo.Imposto.PISST.vPIS = this.readDouble(nodedetImpostoPISst, Properties.Resources.vPIS);
                }
                #endregion

                #region --Imposto->COFINS
                foreach (XmlNode nodedetImpostoCOFINS in ((XmlElement)nodedetImposto).GetElementsByTagName("COFINS"))
                {
                    detInfo.Imposto.COFINS.CST = this.readValue(nodedetImpostoCOFINS, Properties.Resources.CST);
                    detInfo.Imposto.COFINS.vBC = this.readDouble(nodedetImpostoCOFINS, Properties.Resources.vBC);
                    detInfo.Imposto.COFINS.pCOFINS = this.readDouble(nodedetImpostoCOFINS, Properties.Resources.pCOFINS);
                    detInfo.Imposto.COFINS.qBCProd = this.readDouble(nodedetImpostoCOFINS, Properties.Resources.qBCProd);
                    detInfo.Imposto.COFINS.vAliqProd = this.readDouble(nodedetImpostoCOFINS, Properties.Resources.vAliqProd);
                    detInfo.Imposto.COFINS.vCOFINS = this.readDouble(nodedetImpostoCOFINS, Properties.Resources.vCOFINS);
                }
                #endregion

                #region --Imposto->COFINSST
                foreach (XmlNode nodedetImpostoCOFINSst in ((XmlElement)nodedetImposto).GetElementsByTagName("COFINSST"))
                {
                    detInfo.Imposto.COFINSST.vBC = this.readDouble(nodedetImpostoCOFINSst, Properties.Resources.vBC);
                    detInfo.Imposto.COFINSST.pCOFINS = this.readDouble(nodedetImpostoCOFINSst, Properties.Resources.pCOFINS);
                    detInfo.Imposto.COFINSST.qBCProd = this.readDouble(nodedetImpostoCOFINSst, Properties.Resources.qBCProd);
                    detInfo.Imposto.COFINSST.vAliqProd = this.readDouble(nodedetImpostoCOFINSst, Properties.Resources.vAliqProd);
                    detInfo.Imposto.COFINSST.vCOFINS = this.readDouble(nodedetImpostoCOFINSst, Properties.Resources.vCOFINS);
                }
                #endregion

                #region --Imposto->ISSQN
                foreach (XmlNode nodedetImpostoISSQN in ((XmlElement)nodedetImposto).GetElementsByTagName("ISSQN"))
                {
                    detInfo.Imposto.ISSQN.vBC = this.readDouble(nodedetImpostoISSQN, Properties.Resources.vBC);
                    detInfo.Imposto.ISSQN.vAliq = this.readDouble(nodedetImpostoISSQN, Properties.Resources.vAliq);
                    detInfo.Imposto.ISSQN.vISSQN = this.readDouble(nodedetImpostoISSQN, Properties.Resources.vISSQN);
                    detInfo.Imposto.ISSQN.cMunFG = this.readInt32(nodedetImpostoISSQN, Properties.Resources.cMunFG);
                    detInfo.Imposto.ISSQN.cListServ = this.readInt32(nodedetImpostoISSQN, Properties.Resources.cListServ);
                    detInfo.Imposto.ISSQN.cSitTrib = this.readValue(nodedetImpostoISSQN, Properties.Resources.cSitTrib);
                }
                #endregion

            }
            nfe.det.Add(detInfo);
        }

        /// <summary>
        /// processaDEST
        /// </summary>
        /// <param name="el"></param>
        private void processaDEST(XmlNode el)
        {
            nfe.dest.CNPJ = this.readValue(el, Properties.Resources.CNPJ);
            nfe.dest.CPF = this.readValue(el, Properties.Resources.CPF);
            nfe.dest.email = this.readValue(el, Properties.Resources.email);
            nfe.dest.IE = this.readValue(el, Properties.Resources.IE);
            nfe.dest.ISUF = this.readValue(el, Properties.Resources.ISUF);
            nfe.dest.xNome = this.readValue(el, Properties.Resources.xNome);

            foreach (XmlNode nodeEnder in ((XmlElement)el).GetElementsByTagName("enderDest"))
            {
                XmlElement ele = nodeEnder as XmlElement;

                nfe.dest.enderDest.CEP = this.readInt32(ele, Properties.Resources.CEP);
                nfe.dest.enderDest.cMun = this.readInt32(ele, Properties.Resources.cMun);
                nfe.dest.enderDest.cPais = this.readInt32(ele, Properties.Resources.cPais);
                nfe.dest.enderDest.fone = this.readValue(ele, Properties.Resources.fone);
                nfe.dest.enderDest.nro = this.readValue(ele, Properties.Resources.nro);
                nfe.dest.enderDest.UF = this.readValue(ele, Properties.Resources.UF);
                nfe.dest.enderDest.xBairro = this.readValue(ele, Properties.Resources.xBairro);
                nfe.dest.enderDest.xCpl = this.readValue(ele, Properties.Resources.xCpl);
                nfe.dest.enderDest.xLgr = this.readValue(ele, Properties.Resources.xLgr);
                nfe.dest.enderDest.xMun = this.readValue(ele, Properties.Resources.xMun);
                nfe.dest.enderDest.xPais = this.readValue(ele, Properties.Resources.xPais);
            }
        }

        /// <summary>
        /// processaEMIT
        /// </summary>
        /// <param name="el"></param>
        private void processaEMIT(XmlNode el)
        {
            nfe.emit.CNAE = this.readValue(el, Properties.Resources.CNAE);
            nfe.emit.CNPJ = this.readValue(el, Properties.Resources.CNPJ);
            nfe.emit.CPF = this.readValue(el, Properties.Resources.CPF);
            nfe.emit.CRT = (TpcnCRT)this.readInt32(el, Properties.Resources.CRT);
            nfe.emit.IE = this.readValue(el, Properties.Resources.IE);
            nfe.emit.IEST = this.readValue(el, Properties.Resources.IEST);
            nfe.emit.IM = this.readValue(el, Properties.Resources.IM);
            nfe.emit.xFant = this.readValue(el, Properties.Resources.xFant);
            nfe.emit.xNome = this.readValue(el, Properties.Resources.xNome);

            foreach (XmlNode nodeEnder in ((XmlElement)el).GetElementsByTagName("enderEmit"))
            {
                XmlElement ele = nodeEnder as XmlElement;

                nfe.emit.enderEmit.CEP = this.readInt32(ele, Properties.Resources.CEP);
                nfe.emit.enderEmit.cMun = this.readInt32(ele, Properties.Resources.cMun);
                nfe.emit.enderEmit.cPais = this.readInt32(ele, Properties.Resources.cPais);
                nfe.emit.enderEmit.fone = this.readValue(ele, Properties.Resources.fone);
                nfe.emit.enderEmit.nro = this.readValue(ele, Properties.Resources.nro);
                nfe.emit.enderEmit.UF = this.readValue(ele, Properties.Resources.UF);
                nfe.emit.enderEmit.xBairro = this.readValue(ele, Properties.Resources.xBairro);
                nfe.emit.enderEmit.xCpl = this.readValue(ele, Properties.Resources.xCpl);
                nfe.emit.enderEmit.xLgr = this.readValue(ele, Properties.Resources.xLgr);
                nfe.emit.enderEmit.xMun = this.readValue(ele, Properties.Resources.xMun);
                nfe.emit.enderEmit.xPais = this.readValue(ele, Properties.Resources.xPais);
            }
        }

        /// <summary>
        /// processaIDE
        /// </summary>
        /// <param name="nodeinfNFe"></param>
        private void processaIDE(XmlNode nodeinfNFe)
        {
            nfe.ide.cDV = this.readInt32(nodeinfNFe, Properties.Resources.cDV);
            nfe.ide.cMunFG = this.readInt32(nodeinfNFe, Properties.Resources.cMunFG);
            nfe.ide.cNF = this.readInt32(nodeinfNFe, Properties.Resources.cNF);
            nfe.ide.cUF = this.readInt32(nodeinfNFe, Properties.Resources.cUF);
            nfe.ide.dEmi = this.readDate(nodeinfNFe, Properties.Resources.dEmi);
            nfe.ide.dhCont = this.readDate(nodeinfNFe, Properties.Resources.dhCont);
            nfe.ide.dSaiEnt = this.readDate(nodeinfNFe, Properties.Resources.dSaiEnt);
            nfe.ide.finNFe = (TpcnFinalidadeNFe)this.readInt32(nodeinfNFe, Properties.Resources.finNFe);
            nfe.ide.hSaiEnt = this.readDate(nodeinfNFe, Properties.Resources.hSaiEnt);
            nfe.ide.indPag = (TpcnIndicadorPagamento)this.readInt32(nodeinfNFe, Properties.Resources.indPag);
            nfe.ide.mod = Convert.ToInt32(this.readValue(nodeinfNFe, Properties.Resources.mod));
            nfe.ide.nNF = this.readInt32(nodeinfNFe, Properties.Resources.nNF);
            nfe.ide.natOp = this.readValue(nodeinfNFe, Properties.Resources.natOp);
            nfe.ide.procEmi = (TpcnProcessoEmissao)this.readInt32(nodeinfNFe, Properties.Resources.procEmi);
            nfe.ide.serie = Convert.ToInt32(this.readValue(nodeinfNFe, Properties.Resources.serie));
            nfe.ide.tpAmb = (TpcnTipoAmbiente)this.readInt32(nodeinfNFe, Properties.Resources.tpAmb);
            nfe.ide.tpEmis = (TpcnTipoEmissao)this.readInt32(nodeinfNFe, Properties.Resources.tpEmis);
            nfe.ide.tpImp = (TpcnTipoImpressao)this.readInt32(nodeinfNFe, Properties.Resources.tpImp);
            nfe.ide.tpNF = (TpcnTipoNFe)this.readInt32(nodeinfNFe, Properties.Resources.tpNF);
            nfe.ide.verProc = this.readValue(nodeinfNFe, Properties.Resources.verProc);
            nfe.ide.xJust = this.readValue(nodeinfNFe, Properties.Resources.xJust);

            foreach (XmlNode nodeNFref in nodeinfNFe.ChildNodes)
            {
                if (nodeNFref.LocalName.Equals("NFref"))
                {
                    foreach (XmlNode nodeNFrefItem in nodeNFref.ChildNodes)
                    {
                        switch (nodeNFrefItem.LocalName)
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
                                    item_refNF.refNF.AAMM = this.readValue(nodeNFrefItem, Properties.Resources.AAMM);
                                    item_refNF.refNF.CNPJ = this.readValue(nodeNFrefItem, Properties.Resources.CNPJ);
                                    item_refNF.refNF.cUF = this.readInt32(nodeNFrefItem, Properties.Resources.cUF);
                                    item_refNF.refNF.mod = this.readValue(nodeNFrefItem, Properties.Resources.mod);
                                    item_refNF.refNF.nNF = this.readInt32(nodeNFrefItem, Properties.Resources.nNF);
                                    item_refNF.refNF.serie = this.readInt32(nodeNFrefItem, Properties.Resources.serie);
                                    nfe.ide.NFref.Add(item_refNF);
                                }
                                break;

                            case "refECF":
                                {
                                    NFref item_refECF = new NFref();
                                    item_refECF.refECF = new refECF();
                                    item_refECF.refECF.mod = this.readValue(nodeNFrefItem, Properties.Resources.mod);
                                    item_refECF.refECF.nCOO = this.readInt32(nodeNFrefItem, Properties.Resources.nCOO);
                                    item_refECF.refECF.nECF = this.readInt32(nodeNFrefItem, Properties.Resources.nECF);
                                    nfe.ide.NFref.Add(item_refECF);
                                }
                                break;

                            case "refNFP":
                                {
                                    NFref item_refNFP = new NFref();
                                    item_refNFP.refNFP = new refNFP();
                                    item_refNFP.refNFP.AAMM = this.readValue(nodeNFrefItem, Properties.Resources.AAMM);
                                    item_refNFP.refNFP.CNPJ = this.readValue(nodeNFrefItem, Properties.Resources.CNPJ);
                                    item_refNFP.refNFP.CPF = this.readValue(nodeNFrefItem, Properties.Resources.CPF);
                                    item_refNFP.refNFP.cUF = this.readInt32(nodeNFrefItem, Properties.Resources.cUF);
                                    item_refNFP.refNFP.IE = this.readValue(nodeNFrefItem, Properties.Resources.IE);
                                    item_refNFP.refNFP.mod = this.readValue(nodeNFrefItem, Properties.Resources.mod);
                                    item_refNFP.refNFP.nNF = this.readInt32(nodeNFrefItem, Properties.Resources.nNF);
                                    item_refNFP.refNFP.serie = this.readInt32(nodeNFrefItem, Properties.Resources.serie);
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

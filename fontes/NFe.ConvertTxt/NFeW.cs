using NFe.Components;
using NFe.Components;
using NFe.Settings;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace NFe.ConvertTxt
{
    public class NFeW
    {
        public string cMensagemErro { get; set; }
        public string cFileName { get; private set; }
        public string XMLString { get; private set; }

        private XmlDocument doc;
        private XmlNode nodeCurrent = null;
        private TpcnTipoCampo nDecimaisPerc = TpcnTipoCampo.tcDec2;
        private bool convertToOem;

        /// <summary>
        /// GerarXml
        /// </summary>
        /// <param name="NFe"></param>
        public void GerarXml(NFe NFe, string folderDestino)
        {
            doc = new XmlDocument();
            XmlDeclaration node = doc.CreateXmlDeclaration("1.0", "UTF-8", "");

            doc.InsertBefore(node, doc.DocumentElement);
            ///
            /// NFe
            ///
            XmlNode xmlInf = doc.CreateElement("NFe");
            //if (NFe.ide.cUF != 29)  //Bahia
            //{
            //    XmlAttribute xmlVersion = doc.CreateAttribute("xmlns:xsi");
            //    xmlVersion.Value = "http://www.w3.org/2001/XMLSchema-instance";
            //    xmlInf.Attributes.Append(xmlVersion);
            //}
            XmlAttribute xmlVersion1 = doc.CreateAttribute("xmlns");
            xmlVersion1.Value = NFeStrConstants.NAME_SPACE_NFE;// !string.IsNullOrEmpty(Propriedade.nsURI_nfe) ? Propriedade.nsURI_nfe : "http://www.portalfiscal.inf.br/nfe";
            xmlInf.Attributes.Append(xmlVersion1);
            doc.AppendChild(xmlInf);

            string cChave = NFe.ide.cUF.ToString() +
                            NFe.ide.dEmi.Year.ToString("0000").Substring(2) +
                            NFe.ide.dEmi.Month.ToString("00"); //data AAMM

            if(NFe.infNFe.Versao >= 3)
            {
                cChave = NFe.ide.cUF.ToString() +
                         NFe.ide.dhEmi.Substring(2, 2) +
                         NFe.ide.dhEmi.Substring(5, 2); //data AAMM
            }

            long iTmp = Convert.ToInt64("0" + NFe.emit.CNPJ + NFe.emit.CPF);
            cChave += iTmp.ToString("00000000000000");
            cChave += Convert.ToInt32(NFe.ide.mod).ToString("00");

            if(NFe.ide.cNF == 0)
            {
                ///
                /// gera codigo aleatorio
                ///
                NFe.ide.cNF = GerarCodigoNumerico(NFe.ide.nNF);
            }
            string ccChave = cChave +
                             NFe.ide.serie.ToString("000") +
                             NFe.ide.nNF.ToString("000000000") +
                             ((int)NFe.ide.tpEmis).ToString("0") +
                             NFe.ide.cNF.ToString("00000000");

            if(NFe.ide.cDV == 0)
            {
                ///
                /// calcula digito verificador
                ///

                NFe.ide.cDV = GerarDigito(ccChave);
            }
            else
            {
                int ccDV = GerarDigito(ccChave);
                if(NFe.ide.cDV != ccDV)
                {
                    throw new Exception(string.Format("Digito verificador informado, [{0}] é diferente do calculado, [{1}]", NFe.ide.cDV, ccDV));
                }
            }
            cChave += NFe.ide.serie.ToString("000") +
                        NFe.ide.nNF.ToString("000000000") +
                        ((int)NFe.ide.tpEmis).ToString("0") +
                        NFe.ide.cNF.ToString("00000000") +
                        NFe.ide.cDV.ToString("0");
            NFe.infNFe.ID = cChave;

            if(string.IsNullOrEmpty(NFe.resptecnico.hashCSRT) && !string.IsNullOrEmpty(NFe.resptecnico.CNPJ))
            {
                int empresa = Empresas.FindEmpresaByThread();

                if(!string.IsNullOrEmpty(Empresas.Configuracoes[empresa].RespTecIdCSRT) &&
                    !string.IsNullOrEmpty(Empresas.Configuracoes[empresa].RespTecCSRT))
                {
                    NFe.resptecnico.idCSRT = Convert.ToInt32(Empresas.Configuracoes[empresa].RespTecIdCSRT);
                    NFe.resptecnico.hashCSRT = Functions.ToBase64Hex(Criptografia.GetSHA1HashData(Empresas.Configuracoes[empresa].RespTecCSRT + cChave));
                }
            }

            ///
            /// infNFe
            ///
            if(NFe.infNFe.Versao == 0)
            {
                NFe.infNFe.Versao = Convert.ToDecimal(versoes.VersaoXMLNFe.Replace(".", System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator));
            }

            nDecimaisPerc = TpcnTipoCampo.tcDec4;

            XmlElement infNfe = doc.CreateElement("infNFe");

            XmlAttribute infNfeAttr1 = doc.CreateAttribute(TpcnResources.versao.ToString());
            infNfeAttr1.Value = NFe.infNFe.Versao.ToString("0.00").Replace(",", ".");
            infNfe.Attributes.Append(infNfeAttr1);

            XmlAttribute infNfeAttrId = doc.CreateAttribute(TpcnResources.Id.ToString());
            infNfeAttrId.Value = "NFe" + NFe.infNFe.ID;
            infNfe.Attributes.Append(infNfeAttrId);
            xmlInf.AppendChild(infNfe);

            convertToOem = true;

            infNfe.AppendChild(GerarIde(NFe));
            infNfe.AppendChild(GerarEmit(NFe));
            GerarAvulsa(NFe, infNfe);
            XmlNode nodeDest = GerarDest(NFe);
            if(nodeDest != null && nodeDest.HasChildNodes)
            {
                ///danasa 6/2014
                infNfe.AppendChild(nodeDest);
            }
            ///somente grava o nó "dest" se exibir
            GerarRetirada(NFe, infNfe);
            GerarEntrega(NFe, infNfe);
            GerarautXML(NFe, infNfe);
            GerarDet(NFe, infNfe);
            GerarTotal(NFe, infNfe);
            GerarTransp(NFe, infNfe);
            GerarCobr(NFe, infNfe);

            if((NFe.infNFe.Versao >= 3 && NFe.ide.mod == TpcnMod.modNFCe) ||
                (NFe.infNFe.Versao >= 4))
            {
                GerarPag(NFe, infNfe);
            }

            GerarInfIntermed(NFe.InfIntermed, infNfe);
            GerarInfAdic(NFe.InfAdic, infNfe);
            GerarExporta(NFe, NFe.exporta, infNfe);
            GerarCompra(NFe.compra, infNfe);
            GerarCana(NFe.cana, infNfe);
            GerarRespTecnico(NFe.resptecnico, infNfe);

            cFileName = NFe.infNFe.ID + Propriedade.Extensao(Propriedade.TipoEnvio.NFe).EnvioXML;

            if(!string.IsNullOrEmpty(folderDestino))
            {
                if(folderDestino.Substring(folderDestino.Length - 1, 1) == @"\")
                {
                    folderDestino = folderDestino.Substring(0, folderDestino.Length - 1);
                }

                if(!Directory.Exists(Path.Combine(folderDestino, "Convertidos")))
                {
                    ///
                    /// cria uma pasta temporária para armazenar o XML convertido
                    ///
                    System.IO.Directory.CreateDirectory(Path.Combine(folderDestino, "Convertidos"));
                }
                string strRetorno = cFileName;
                strRetorno = Path.Combine(folderDestino, "Convertidos");
                cFileName = Path.Combine(strRetorno, cFileName);
                ///
                /// salva o XML
                strRetorno = cFileName;
                doc.Save(@strRetorno);
            }
            ///
            /// retorna o conteudo do XML da nfe
            XMLString = doc.OuterXml;
        }

        private void GerarInfIntermed(InfIntermed infIntermed, XmlElement root)
        {
            if(string.IsNullOrEmpty(infIntermed.CNPJ))
            {
                return;
            }

            XmlElement rootInfIntermed = doc.CreateElement("infIntermed");
            root.AppendChild(rootInfIntermed);
            nodeCurrent = rootInfIntermed;

            wCampo(infIntermed.CNPJ, TpcnTipoCampo.tcStr, TpcnResources.CNPJ, ObOp.Obrigatorio);
            wCampo(infIntermed.idCadIntTran, TpcnTipoCampo.tcStr, nameof(infIntermed.idCadIntTran), ObOp.Obrigatorio);
        }

        /// <summary>
        /// GerarAvulsa
        /// </summary>
        /// <param name="NFe"></param>
        /// <param name="root"></param>
        private void GerarAvulsa(NFe NFe, XmlElement root)
        {
            if(!string.IsNullOrEmpty(NFe.avulsa.CNPJ))
            {
                XmlElement ELav = doc.CreateElement("avulsa");
                nodeCurrent = ELav;
                root.AppendChild(ELav);

                wCampo(NFe.avulsa.CNPJ, TpcnTipoCampo.tcStr, TpcnResources.CNPJ);
                wCampo(NFe.avulsa.xOrgao, TpcnTipoCampo.tcStr, TpcnResources.xOrgao);
                wCampo(NFe.avulsa.matr, TpcnTipoCampo.tcStr, TpcnResources.matr);
                wCampo(NFe.avulsa.xAgente, TpcnTipoCampo.tcStr, TpcnResources.xAgente);
                wCampo(NFe.avulsa.fone, TpcnTipoCampo.tcStr, TpcnResources.fone);
                wCampo(NFe.avulsa.UF, TpcnTipoCampo.tcStr, TpcnResources.UF);
                wCampo(NFe.avulsa.nDAR, TpcnTipoCampo.tcStr, TpcnResources.nDAR);
                wCampo(NFe.avulsa.dEmi, TpcnTipoCampo.tcDatYYYY_MM_DD, TpcnResources.dEmi);
                wCampo(NFe.avulsa.vDAR, TpcnTipoCampo.tcDec2, TpcnResources.vDAR);
                wCampo(NFe.avulsa.repEmi, TpcnTipoCampo.tcStr, TpcnResources.repEmi);
                wCampo(NFe.avulsa.dPag, TpcnTipoCampo.tcDatYYYY_MM_DD, TpcnResources.dPag, ObOp.Opcional);
            }
        }

        /// <summary>
        /// GerarCobr
        /// </summary>
        /// <param name="Cobr"></param>
        /// <param name="root"></param>
        private void GerarCobr(NFe NFe, XmlElement root)
        {
            Cobr Cobr = NFe.Cobr;

            if(!string.IsNullOrEmpty(Cobr.Fat.nFat) ||
                (Cobr.Fat.vOrig > 0) ||
                (Cobr.Fat.vDesc > 0) ||
                (Cobr.Fat.vLiq > 0) ||
                (Cobr.Dup.Count > 0))
            {
                XmlElement nodeCobr = doc.CreateElement("cobr");
                nodeCurrent = nodeCobr;
                root.AppendChild(nodeCobr);
                //
                //(**)GerarCobrFat;
                //
                if(!string.IsNullOrEmpty(Cobr.Fat.nFat) ||
                    Cobr.Fat.vOrig > 0 ||
                    Cobr.Fat.vDesc > 0 ||
                    Cobr.Fat.vLiq > 0)
                {
                    XmlElement nodeFat = doc.CreateElement("fat");
                    nodeCobr.AppendChild(nodeFat);
                    nodeCurrent = nodeFat;

                    wCampo(Cobr.Fat.nFat, TpcnTipoCampo.tcStr, TpcnResources.nFat);
                    wCampo(Cobr.Fat.vOrig, TpcnTipoCampo.tcDec2, TpcnResources.vOrig, NFe.infNFe.Versao >= 4 ? ObOp.Obrigatorio : ObOp.Opcional);
                    wCampo(Cobr.Fat.vDesc, TpcnTipoCampo.tcDec2, TpcnResources.vDesc, NFe.infNFe.Versao >= 4 ? ObOp.None : ObOp.Opcional);
                    wCampo(Cobr.Fat.vLiq, TpcnTipoCampo.tcDec2, TpcnResources.vLiq, NFe.infNFe.Versao >= 4 ? ObOp.Obrigatorio : ObOp.Opcional);
                }
                //
                //(**)GerarCobrDup;
                //
                foreach(Dup Dup in Cobr.Dup)
                {
                    if(Dup.dVenc.Year > 1 || Dup.vDup > 0 || !string.IsNullOrEmpty(Dup.nDup))
                    {
                        XmlElement nodeDup = doc.CreateElement("dup");
                        nodeCobr.AppendChild(nodeDup);
                        nodeCurrent = nodeDup;

                        wCampo(Dup.nDup, TpcnTipoCampo.tcStr, TpcnResources.nDup, ObOp.Opcional);
                        wCampo(Dup.dVenc, TpcnTipoCampo.tcDatYYYY_MM_DD, TpcnResources.dVenc, ObOp.Opcional);
                        wCampo(Dup.vDup, TpcnTipoCampo.tcDec2, TpcnResources.vDup, ObOp.Obrigatorio);
                    }
                }
            }
        }

        /// <summary>
        /// Gerarpag
        /// </summary>
        /// <param name="nfe"></param>
        /// <param name="root"></param>
        private void GerarPag(NFe nfe, XmlElement root)
        {
            XmlElement nodePag = null;

            if(nfe.pag.Count == 0)
            {
                throw new Exception("Falta definir valores do pagamento, tag <pag>.");
            }

            foreach(pag pagItem in nfe.pag)
            {
                if(nodePag == null || nfe.infNFe.Versao < 4)
                {
                    nodePag = doc.CreateElement("pag"); //YA01
                    root.AppendChild(nodePag);
                }
                nodeCurrent = nodePag;

                if(nfe.infNFe.Versao >= 4)
                {
                    XmlElement nodedetPag = doc.CreateElement("detPag");
                    nodeCurrent = nodedetPag;
                    nodePag.AppendChild(nodedetPag);

                    if(pagItem.indPag != TpcnIndicadorPagamento.ipNone)
                    {
                        wCampo((int)pagItem.indPag, TpcnTipoCampo.tcInt, TpcnResources.indPag, ObOp.Obrigatorio, 0);//YA01b
                    }

                    wCampo((int)pagItem.tPag, TpcnTipoCampo.tcInt, TpcnResources.tPag, ObOp.Obrigatorio, 2);    //YA02
                    wCampo(pagItem.xPag, TpcnTipoCampo.tcStr, TpcnResources.xPag, ObOp.Opcional);    //YA02
                    wCampo(pagItem.vPag, TpcnTipoCampo.tcDec2, TpcnResources.vPag, ObOp.Obrigatorio);           //YA03
                    if(pagItem.tpIntegra != 0)
                    {
                        XmlElement xnodedetPag = doc.CreateElement("card"); //YA04
                        nodeCurrent = xnodedetPag;
                        nodedetPag.AppendChild(xnodedetPag);

                        wCampo(pagItem.tpIntegra, TpcnTipoCampo.tcInt, TpcnResources.tpIntegra, ObOp.Obrigatorio);
                        wCampo(pagItem.CNPJ, TpcnTipoCampo.tcStr, TpcnResources.CNPJ, ObOp.Opcional);           //YA05
                        wCampo((int)pagItem.tBand, TpcnTipoCampo.tcInt, TpcnResources.tBand, ObOp.Opcional, 2); //YA06
                        wCampo(pagItem.cAut, TpcnTipoCampo.tcStr, TpcnResources.cAut, ObOp.Opcional);           //YA07
                    }
                }
                else
                {
                    wCampo((int)pagItem.tPag, TpcnTipoCampo.tcInt, TpcnResources.tPag, ObOp.Obrigatorio, 2);    //YA02
                    wCampo(pagItem.vPag, TpcnTipoCampo.tcDec2, TpcnResources.vPag, ObOp.Obrigatorio);           //YA03

                    //Modificado Samuel 24/01/2018
                    if(pagItem.tpIntegra > 0 || !string.IsNullOrEmpty(pagItem.CNPJ) || pagItem.tBand > 0 || !string.IsNullOrEmpty(pagItem.cAut))
                    {
                        XmlElement xnodePag = doc.CreateElement("card"); //YA04
                        nodeCurrent = xnodePag;
                        nodePag.AppendChild(xnodePag);

                        //Modificado Samuel 24/01/2018
                        wCampo(pagItem.tpIntegra, TpcnTipoCampo.tcInt, TpcnResources.tpIntegra, ObOp.Obrigatorio, 1);
                        wCampo(pagItem.CNPJ, TpcnTipoCampo.tcStr, TpcnResources.CNPJ, ObOp.Opcional); //YA05
                        wCampo((int)pagItem.tBand, TpcnTipoCampo.tcInt, TpcnResources.tBand, ObOp.Opcional, 2);          //YA06
                        wCampo(pagItem.cAut, TpcnTipoCampo.tcStr, TpcnResources.cAut, ObOp.Opcional);                    //YA07
                    }
                }

                nodeCurrent = nodePag;
            }

            wCampo(nfe.vTroco, TpcnTipoCampo.tcDec2, TpcnResources.vTroco, ObOp.Opcional);
        }

        /// <summary>
        /// GerarCodigoNumerico
        /// </summary>
        /// <param name="numeroNF"></param>
        /// <returns></returns>
        public int GerarCodigoNumerico(int numeroNF)
        {
            int codigoRetorno = 0;
            while(codigoRetorno == 0)
            {
                Random rnd = new Random(numeroNF);
                codigoRetorno = Convert.ToInt32(rnd.Next(1, 99999999).ToString("00000000"));
            }
            return codigoRetorno;
        }

        /// <summary>
        /// GerarCana
        /// </summary>
        /// <param name="cana"></param>
        /// <param name="root"></param>
        private void GerarCana(Cana cana, XmlElement root)
        {
            if(!string.IsNullOrEmpty(cana.safra) || !string.IsNullOrEmpty(cana.Ref) ||
                (cana.fordia.Count > 0) || (cana.deduc.Count > 0))
            {
                XmlElement rootCana = doc.CreateElement("cana");
                root.AppendChild(rootCana);
                nodeCurrent = rootCana;

                wCampo(cana.safra, TpcnTipoCampo.tcStr, TpcnResources.safra, ObOp.Opcional);
                wCampo(cana.Ref, TpcnTipoCampo.tcStr, TpcnResources.Ref.ToString().ToLower(), ObOp.Opcional, 0);

                if(cana.fordia.Count > 31)
                {
                    cMensagemErro += "Número máximo de elementos no segmento 'ZC04' excedeu" + Environment.NewLine;
                }

                foreach(fordia item in cana.fordia)
                {
                    XmlElement nodefordia = doc.CreateElement("forDia");
                    XmlAttribute xmlItem = doc.CreateAttribute(TpcnResources.dia.ToString());   //
                    xmlItem.Value = item.dia.ToString();                                        //danasa 3/11/2011
                    nodefordia.Attributes.Append(xmlItem);                                      //
                    rootCana.AppendChild(nodefordia);
                    nodeCurrent = nodefordia;

                    //wCampo(item.dia, TpcnTipoCampo.tcInt, TpcnResources.dia);
                    wCampo(item.qtde, item.qtde_Tipo, TpcnResources.qtde);
                }
                nodeCurrent = rootCana;
                wCampo(cana.qTotMes, cana.qTotMes_Tipo, TpcnResources.qTotMes);
                wCampo(cana.qTotAnt, cana.qTotAnt_Tipo, TpcnResources.qTotAnt);
                wCampo(cana.qTotGer, cana.qTotGer_Tipo, TpcnResources.qTotGer);

                if(cana.deduc.Count > 10)
                {
                    cMensagemErro += "Número máximo de elementos no segmento 'ZC10' excedeu" + Environment.NewLine;
                }

                foreach(deduc item in cana.deduc)
                {
                    XmlElement nodededuc = doc.CreateElement("deduc");
                    rootCana.AppendChild(nodededuc);
                    nodeCurrent = nodededuc;

                    wCampo(item.xDed, TpcnTipoCampo.tcStr, TpcnResources.xDed);
                    wCampo(item.vDed, TpcnTipoCampo.tcDec2, TpcnResources.vDed);
                }
                nodeCurrent = rootCana;
                wCampo(cana.vFor, TpcnTipoCampo.tcDec2, TpcnResources.vFor);
                wCampo(cana.vTotDed, TpcnTipoCampo.tcDec2, TpcnResources.vTotDed);
                wCampo(cana.vLiqFor, TpcnTipoCampo.tcDec2, TpcnResources.vLiqFor);
            }
        }

        /// <summary>
        /// GerarCompra
        /// </summary>
        /// <param name="compra"></param>
        /// <param name="root"></param>
        private void GerarCompra(Compra compra, XmlElement root)
        {
            if(!string.IsNullOrEmpty(compra.xNEmp) || !string.IsNullOrEmpty(compra.xPed) || !string.IsNullOrEmpty(compra.xCont))
            {
                nodeCurrent = doc.CreateElement("compra");
                root.AppendChild(nodeCurrent);

                /// incluida a opcao->opcional
                wCampo(compra.xNEmp, TpcnTipoCampo.tcStr, TpcnResources.xNEmp, ObOp.Opcional);
                wCampo(compra.xPed, TpcnTipoCampo.tcStr, TpcnResources.xPed, ObOp.Opcional);
                wCampo(compra.xCont, TpcnTipoCampo.tcStr, TpcnResources.xCont, ObOp.Opcional);
            }
        }

        /// <summary>
        /// GerarDest
        /// </summary>
        /// <param name="NFe"></param>
        /// <returns></returns>
        private XmlElement GerarDest(NFe NFe)
        {
            XmlElement e0 = doc.CreateElement("dest");
            nodeCurrent = e0;

            if(NFe.infNFe.Versao <= 2 ||
                (NFe.infNFe.Versao >= 3 && (!string.IsNullOrEmpty(NFe.dest.CNPJ) || !string.IsNullOrEmpty(NFe.dest.CPF) || !string.IsNullOrEmpty(NFe.dest.idEstrangeiro))))
            {
                if(NFe.infNFe.Versao >= 3 && !string.IsNullOrEmpty(NFe.dest.idEstrangeiro))
                {
                    if(NFe.dest.idEstrangeiro.Equals("NAO GERAR TAG"))
                    {
                        wCampo("", TpcnTipoCampo.tcStr, TpcnResources.idEstrangeiro, ObOp.Obrigatorio); //E03a
                    }
                    else
                    {
                        wCampo(NFe.dest.idEstrangeiro, TpcnTipoCampo.tcStr, TpcnResources.idEstrangeiro, ObOp.Opcional); //E03a
                    }
                }
                else if(NFe.dest.enderDest.cPais != 1058 && NFe.infNFe.Versao <= 2)
                {
                    wCampo("", TpcnTipoCampo.tcStr, TpcnResources.CNPJ);
                }
                else if(!string.IsNullOrEmpty(NFe.dest.CNPJ))
                {
                    wCampo(NFe.dest.CNPJ, TpcnTipoCampo.tcStr, TpcnResources.CNPJ);
                }
                else if(!string.IsNullOrEmpty(NFe.dest.CPF))
                {
                    wCampo(NFe.dest.CPF, TpcnTipoCampo.tcStr, TpcnResources.CPF);
                }
            }

            if(NFe.ide.mod == TpcnMod.modNFe || (NFe.ide.mod == TpcnMod.modNFCe && !string.IsNullOrEmpty(NFe.dest.xNome)))
            {
                wCampo(NFe.ide.tpAmb == TipoAmbiente.taProducao ? NFe.dest.xNome : "NF-E EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL",
                    TpcnTipoCampo.tcStr,
                    TpcnResources.xNome,
                    (NFe.infNFe.Versao >= 3 && NFe.ide.mod == TpcnMod.modNFCe) ? ObOp.Opcional : ObOp.Obrigatorio);
            }

            if(NFe.ide.mod == TpcnMod.modNFe || (NFe.ide.mod == TpcnMod.modNFCe && !string.IsNullOrEmpty(NFe.dest.enderDest.xLgr))) //danasa: 12/2013
            {
                ///
                /// (**)GerarDestEnderDest(UF);
                ///
                XmlElement e1 = doc.CreateElement("enderDest");
                e0.AppendChild(e1);
                nodeCurrent = e1;

                wCampo(NFe.dest.enderDest.xLgr, TpcnTipoCampo.tcStr, TpcnResources.xLgr);
                wCampo(NFe.dest.enderDest.nro, TpcnTipoCampo.tcStr, TpcnResources.nro);
                wCampo(NFe.dest.enderDest.xCpl, TpcnTipoCampo.tcStr, TpcnResources.xCpl, ObOp.Opcional);
                wCampo(NFe.dest.enderDest.xBairro, TpcnTipoCampo.tcStr, TpcnResources.xBairro);
                wCampo(NFe.dest.enderDest.cMun, TpcnTipoCampo.tcInt, TpcnResources.cMun, ObOp.Obrigatorio, 7);
                wCampo(NFe.dest.enderDest.xMun, TpcnTipoCampo.tcStr, TpcnResources.xMun);
                wCampo(NFe.dest.enderDest.UF, TpcnTipoCampo.tcStr, TpcnResources.UF);
                wCampo(NFe.dest.enderDest.CEP, TpcnTipoCampo.tcInt, TpcnResources.CEP, ObOp.Opcional, 8);
                wCampo(NFe.dest.enderDest.cPais, TpcnTipoCampo.tcInt, TpcnResources.cPais, ObOp.Opcional, 4);
                wCampo(NFe.dest.enderDest.xPais, TpcnTipoCampo.tcStr, TpcnResources.xPais, ObOp.Opcional);
                wCampo(NFe.dest.enderDest.fone, TpcnTipoCampo.tcStr, TpcnResources.fone, ObOp.Opcional);
            }
            ///
            /// </enderDest">
            ///
            nodeCurrent = e0;
            if(e0.HasChildNodes)
            {
                if((double)NFe.infNFe.Versao >= 3.10)
                {
                    wCampo((int)NFe.dest.indIEDest, TpcnTipoCampo.tcInt, TpcnResources.indIEDest, ObOp.Obrigatorio);
                }
                else
                {
                    NFe.dest.indIEDest = TpcnindIEDest.inContribuinte;  //força para a versao 2.00
                }

                switch(NFe.dest.indIEDest)
                {
                    case TpcnindIEDest.inContribuinte:
                    case TpcnindIEDest.inNaoContribuinte:
                        {
                            if(!string.IsNullOrEmpty(NFe.dest.IE) || NFe.infNFe.Versao < 3)
                            {
                                if(NFe.dest.enderDest.UF != "EX" || NFe.dest.enderDest.cPais == 1058)
                                {
                                    if(string.IsNullOrEmpty(NFe.dest.IE))
                                    {
                                        wCampo("", TpcnTipoCampo.tcStr, TpcnResources.IE);
                                    }
                                    else if(!string.IsNullOrEmpty(NFe.dest.IE) || NFe.ide.mod != TpcnMod.modNFCe)
                                    {
                                        wCampo(NFe.dest.IE, TpcnTipoCampo.tcStr, TpcnResources.IE);
                                    }
                                }
                                else
                                {
                                    wCampo("ISENTO", TpcnTipoCampo.tcStr, TpcnResources.IE);
                                }
                            }
                        }
                        break;
                }
                wCampo(NFe.dest.ISUF, TpcnTipoCampo.tcStr, TpcnResources.ISUF, ObOp.Opcional);
                wCampo(NFe.dest.IM, TpcnTipoCampo.tcStr, TpcnResources.IM, ObOp.Opcional);
                wCampo(NFe.dest.email, TpcnTipoCampo.tcStr, TpcnResources.email, ObOp.Opcional);
            }
            return e0;
        }

        /// <summary>
        /// GerarDet
        /// </summary>
        /// <param name="NFe"></param>
        /// <param name="root"></param>
        private void GerarDet(NFe NFe, XmlElement root)
        {
            if(NFe.det.Count > 990)
            {
                cMensagemErro += "Número máximo de itens excedeu o máximo permitido" + Environment.NewLine;
            }

            foreach(Det det in NFe.det)
            {
                XmlElement rootDet = doc.CreateElement("det");
                XmlAttribute xmlItem = doc.CreateAttribute("nItem");
                xmlItem.Value = det.Prod.nItem.ToString();
                rootDet.Attributes.Append(xmlItem);
                root.AppendChild(rootDet);

                ///
                /// Linha do produto
                ///
                XmlElement nodeProd = doc.CreateElement("prod");
                rootDet.AppendChild(nodeProd);
                nodeCurrent = nodeProd;

                convertToOem = (det.Prod.comb.cProdANP > 0 ? false : true);

                wCampo(det.Prod.cProd, TpcnTipoCampo.tcStr, TpcnResources.cProd);
                wCampo(det.Prod.cEAN, TpcnTipoCampo.tcStr, TpcnResources.cEAN);
                wCampo(det.Prod.cBarra, TpcnTipoCampo.tcStr, TpcnResources.cBarra, ObOp.Opcional);

                if(NFe.ide.tpAmb == TipoAmbiente.taHomologacao &&
                    NFe.ide.mod == TpcnMod.modNFCe &&
                    det.Prod.nItem == 1)
                {
                    wCampo("NOTA FISCAL EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL", TpcnTipoCampo.tcStr, TpcnResources.xProd);
                }
                else
                {
                    wCampo(det.Prod.xProd, TpcnTipoCampo.tcStr, TpcnResources.xProd);
                }

                convertToOem = true;
                wCampo(det.Prod.NCM, TpcnTipoCampo.tcStr, TpcnResources.NCM);
                wCampo(det.Prod.NVE, TpcnTipoCampo.tcStr, TpcnResources.NVE, ObOp.Opcional);
                if(det.Prod.CEST > 0)
                {
                    wCampo(det.Prod.CEST.ToString("0000000"), TpcnTipoCampo.tcStr, TpcnResources.CEST);
                }

                if(NFe.infNFe.Versao >= 4)
                {
                    switch(det.Prod.indEscala)
                    {
                        case TpcnIndicadorEscala.ieSomaTotalNFe:
                            wCampo("S", TpcnTipoCampo.tcStr, TpcnResources.indEscala, ObOp.Opcional);
                            break;

                        case TpcnIndicadorEscala.ieNaoSomaTotalNFe:
                            wCampo("N", TpcnTipoCampo.tcStr, TpcnResources.indEscala, ObOp.Opcional);
                            break;

                        default:
                            wCampo("", TpcnTipoCampo.tcStr, TpcnResources.indEscala, ObOp.Opcional);
                            break;
                    }
                    wCampo(det.Prod.CNPJFab, TpcnTipoCampo.tcStr, TpcnResources.CNPJFab, ObOp.Opcional);
                    wCampo(det.Prod.cBenef, TpcnTipoCampo.tcStr, TpcnResources.cBenef, ObOp.Opcional);
                }
                wCampo(det.Prod.EXTIPI, TpcnTipoCampo.tcStr, TpcnResources.EXTIPI, ObOp.Opcional);
                wCampo(det.Prod.CFOP, TpcnTipoCampo.tcStr, TpcnResources.CFOP);
                wCampo(det.Prod.uCom, TpcnTipoCampo.tcStr, TpcnResources.uCom);
                wCampo(det.Prod.qCom, TpcnTipoCampo.tcDec4, TpcnResources.qCom);
                wCampo(det.Prod.vUnCom, det.Prod.vUnCom_Tipo, TpcnResources.vUnCom);
                wCampo(det.Prod.vProd, TpcnTipoCampo.tcDec2, TpcnResources.vProd);
                wCampo(det.Prod.cEANTrib, TpcnTipoCampo.tcStr, TpcnResources.cEANTrib);
                wCampo(det.Prod.cBarraTrib, TpcnTipoCampo.tcStr, TpcnResources.cBarraTrib, ObOp.Opcional);
                wCampo(det.Prod.uTrib, TpcnTipoCampo.tcStr, TpcnResources.uTrib);
                wCampo(det.Prod.qTrib, TpcnTipoCampo.tcDec4, TpcnResources.qTrib);
                wCampo(det.Prod.vUnTrib, det.Prod.vUnTrib_Tipo, TpcnResources.vUnTrib);
                wCampo(det.Prod.vFrete, TpcnTipoCampo.tcDec2, TpcnResources.vFrete, ObOp.Opcional);
                wCampo(det.Prod.vSeg, TpcnTipoCampo.tcDec2, TpcnResources.vSeg, ObOp.Opcional);
                wCampo(det.Prod.vDesc, TpcnTipoCampo.tcDec2, TpcnResources.vDesc, ObOp.Opcional);
                wCampo(det.Prod.vOutro, TpcnTipoCampo.tcDec2, TpcnResources.vOutro, ObOp.Opcional);
                wCampo(det.Prod.indTot, TpcnTipoCampo.tcInt, TpcnResources.indTot);

                #region /// DI

                if(det.Prod.DI.Count > 100)
                {
                    cMensagemErro += "Número máximo de itens DI excedeu o máximo permitido" + Environment.NewLine;
                }

                XmlNode oldNode = nodeCurrent;
                foreach(DI di in det.Prod.DI)
                {
                    XmlElement nodeDI = doc.CreateElement("DI");
                    nodeProd.AppendChild(nodeDI);
                    nodeCurrent = nodeDI;

                    wCampo(di.nDI, TpcnTipoCampo.tcStr, TpcnResources.nDI);
                    wCampo(di.dDI, TpcnTipoCampo.tcDatYYYY_MM_DD, TpcnResources.dDI);
                    wCampo(di.xLocDesemb, TpcnTipoCampo.tcStr, TpcnResources.xLocDesemb);
                    wCampo(di.UFDesemb, TpcnTipoCampo.tcStr, TpcnResources.UFDesemb);
                    wCampo(di.dDesemb, TpcnTipoCampo.tcDatYYYY_MM_DD, TpcnResources.dDesemb);

                    if(NFe.infNFe.Versao >= 3)
                    {
                        wCampo((int)di.tpViaTransp, TpcnTipoCampo.tcInt, TpcnResources.tpViaTransp, ObOp.Opcional);
                        wCampo(di.vAFRMM, TpcnTipoCampo.tcDec2, TpcnResources.vAFRMM, di.tpViaTransp == TpcnTipoViaTransp.tvMaritima ? ObOp.Obrigatorio : ObOp.Opcional);
                        wCampo((int)di.tpIntermedio, TpcnTipoCampo.tcInt, TpcnResources.tpIntermedio, ObOp.Opcional);
                        wCampo(di.CNPJ, TpcnTipoCampo.tcStr, TpcnResources.CNPJ, ObOp.Opcional);
                        wCampo(di.UFTerceiro, TpcnTipoCampo.tcStr, TpcnResources.UFTerceiro, ObOp.Opcional);
                    }
                    wCampo(di.cExportador, TpcnTipoCampo.tcStr, TpcnResources.cExportador);
                    //
                    //GerarDetProdDIadi
                    //
                    if(di.adi.Count > 100)
                    {
                        cMensagemErro += "Número máximo de itens DI->ADI excedeu o máximo permitido" + Environment.NewLine;
                    }

                    if(di.adi.Count == 0)
                    {
                        cMensagemErro += "Número minimo de itens DI->ADI não permitido" + Environment.NewLine;
                    }

                    foreach(Adi adi in di.adi)
                    {
                        XmlElement e1 = doc.CreateElement("adi");
                        nodeDI.AppendChild(e1);
                        nodeCurrent = e1;

                        wCampo(adi.nAdicao, TpcnTipoCampo.tcInt, TpcnResources.nAdicao);
                        wCampo(adi.nSeqAdi, TpcnTipoCampo.tcInt, TpcnResources.nSeqAdic);
                        wCampo(adi.cFabricante, TpcnTipoCampo.tcStr, TpcnResources.cFabricante);
                        wCampo(adi.vDescDI, TpcnTipoCampo.tcDec2, TpcnResources.vDescDI, ObOp.Opcional);
                        wCampo(adi.nDraw, TpcnTipoCampo.tcStr, TpcnResources.nDraw, ObOp.Opcional);
                    }
                }
                nodeCurrent = oldNode;

                #endregion /// DI

                #region /// GerarDetProd->detExport

                if(det.Prod.detExport.Count > 500)
                {
                    cMensagemErro += "Número máximo de itens em 'prod->detExport' excedeu o máximo permitido" + Environment.NewLine;
                }

                foreach(detExport detx in det.Prod.detExport)
                {
                    XmlElement nodeDI = doc.CreateElement("detExport");
                    nodeProd.AppendChild(nodeDI);
                    nodeCurrent = nodeDI;

                    wCampo(detx.nDraw, TpcnTipoCampo.tcStr, TpcnResources.nDraw);

                    if(!string.IsNullOrEmpty(detx.exportInd.nRE))
                    {
                        XmlElement nodeDI2 = doc.CreateElement("exportInd");
                        nodeDI.AppendChild(nodeDI2);
                        nodeCurrent = nodeDI2;

                        wCampo(detx.exportInd.nRE, TpcnTipoCampo.tcStr, TpcnResources.nRE, ObOp.Obrigatorio);
                        wCampo(detx.exportInd.chNFe, TpcnTipoCampo.tcStr, TpcnResources.chNFe, ObOp.Obrigatorio);
                        wCampo(detx.exportInd.qExport, TpcnTipoCampo.tcDec4, TpcnResources.qExport, ObOp.Opcional);
                    }
                }
                nodeCurrent = oldNode;

                #endregion /// GerarDetProd->detExport

                wCampo(det.Prod.xPed, TpcnTipoCampo.tcStr, TpcnResources.xPed, ObOp.Opcional);
                wCampo(det.Prod.nItemPed, TpcnTipoCampo.tcInt, TpcnResources.nItemPed, ObOp.Opcional);

                wCampo(det.Prod.nFCI, TpcnTipoCampo.tcStr, TpcnResources.nFCI, ObOp.Opcional);

                #region /// GerarDetProd->rastro

                if(det.Prod.rastro.Count > 500)
                {
                    cMensagemErro += "Número máximo de itens em 'rastro' excedeu o máximo permitido" + Environment.NewLine;
                }

                foreach(Rastro item in det.Prod.rastro)
                {
                    XmlElement e0 = doc.CreateElement("rastro");
                    nodeProd.AppendChild(e0);
                    nodeCurrent = e0;

                    wCampo(item.nLote, TpcnTipoCampo.tcStr, TpcnResources.nLote);
                    wCampo(item.qLote, TpcnTipoCampo.tcDec3, TpcnResources.qLote);
                    wCampo(item.dFab, TpcnTipoCampo.tcDatYYYY_MM_DD, TpcnResources.dFab);
                    wCampo(item.dVal, TpcnTipoCampo.tcDatYYYY_MM_DD, TpcnResources.dVal);
                    wCampo(item.cAgreg, TpcnTipoCampo.tcStr, TpcnResources.cAgreg);
                }

                #endregion /// GerarDetProd->rastro

                #region /// veiculos

                if(!string.IsNullOrEmpty(det.Prod.veicProd.chassi))
                {
                    oldNode = nodeCurrent;
                    XmlElement nodeVeic = doc.CreateElement("veicProd");
                    nodeProd.AppendChild(nodeVeic);
                    nodeCurrent = nodeVeic;

                    wCampo(det.Prod.veicProd.tpOp, TpcnTipoCampo.tcStr, TpcnResources.tpOp);
                    wCampo(det.Prod.veicProd.chassi, TpcnTipoCampo.tcStr, TpcnResources.chassi);
                    wCampo(det.Prod.veicProd.cCor, TpcnTipoCampo.tcStr, TpcnResources.cCor);
                    wCampo(det.Prod.veicProd.xCor, TpcnTipoCampo.tcStr, TpcnResources.xCor);
                    wCampo(det.Prod.veicProd.pot, TpcnTipoCampo.tcStr, TpcnResources.pot);
                    wCampo(det.Prod.veicProd.cilin, TpcnTipoCampo.tcStr, TpcnResources.cilin);
                    wCampo(det.Prod.veicProd.pesoL, TpcnTipoCampo.tcStr, TpcnResources.pesoL);
                    wCampo(det.Prod.veicProd.pesoB, TpcnTipoCampo.tcStr, TpcnResources.pesoB);
                    wCampo(det.Prod.veicProd.nSerie, TpcnTipoCampo.tcStr, TpcnResources.nSerie);
                    wCampo(det.Prod.veicProd.tpComb, TpcnTipoCampo.tcStr, TpcnResources.tpComb);
                    wCampo(det.Prod.veicProd.nMotor, TpcnTipoCampo.tcStr, TpcnResources.nMotor);
                    wCampo(det.Prod.veicProd.CMT, TpcnTipoCampo.tcStr, TpcnResources.CMT);
                    wCampo(det.Prod.veicProd.dist, TpcnTipoCampo.tcStr, TpcnResources.dist);
                    wCampo(det.Prod.veicProd.anoMod, TpcnTipoCampo.tcInt, TpcnResources.anoMod, ObOp.Obrigatorio, 4);
                    wCampo(det.Prod.veicProd.anoFab, TpcnTipoCampo.tcInt, TpcnResources.anoFab, ObOp.Obrigatorio, 4);
                    wCampo(det.Prod.veicProd.tpPint, TpcnTipoCampo.tcStr, TpcnResources.tpPint);
                    wCampo(det.Prod.veicProd.tpVeic, TpcnTipoCampo.tcInt, TpcnResources.tpVeic);
                    wCampo(det.Prod.veicProd.espVeic, TpcnTipoCampo.tcInt, TpcnResources.espVeic);
                    wCampo(det.Prod.veicProd.VIN, TpcnTipoCampo.tcStr, TpcnResources.VIN);
                    wCampo(det.Prod.veicProd.condVeic, TpcnTipoCampo.tcStr, TpcnResources.condVeic);
                    wCampo(det.Prod.veicProd.cMod, TpcnTipoCampo.tcStr, TpcnResources.cMod);
                    wCampo(det.Prod.veicProd.cCorDENATRAN, TpcnTipoCampo.tcInt, TpcnResources.cCorDENATRAN, ObOp.Obrigatorio, 2);
                    wCampo(det.Prod.veicProd.lota, TpcnTipoCampo.tcInt, TpcnResources.lota);
                    wCampo(det.Prod.veicProd.tpRest, TpcnTipoCampo.tcInt, TpcnResources.tpRest);

                    nodeCurrent = oldNode;
                }

                #endregion /// veiculos

                #region /// medicamentos

                if(det.Prod.med.Count > 500)
                {
                    cMensagemErro += "Número máximo de itens em 'med' excedeu o máximo permitido" + Environment.NewLine;
                }

                foreach(Med med in det.Prod.med)
                {
                    XmlElement e0 = doc.CreateElement("med");
                    nodeProd.AppendChild(e0);
                    nodeCurrent = e0;

                    if(NFe.infNFe.Versao < 4)
                    {
                        wCampo(med.nLote, TpcnTipoCampo.tcStr, TpcnResources.nLote);
                        wCampo(med.qLote, TpcnTipoCampo.tcDec3, TpcnResources.qLote);
                        wCampo(med.dFab, TpcnTipoCampo.tcDatYYYY_MM_DD, TpcnResources.dFab);
                        wCampo(med.dVal, TpcnTipoCampo.tcDatYYYY_MM_DD, TpcnResources.dVal);
                    }
                    else
                    {
                        wCampo(med.cProdANVISA, TpcnTipoCampo.tcStr, TpcnResources.cProdANVISA);
                        wCampo(med.xMotivoIsencao, TpcnTipoCampo.tcStr, nameof(med.xMotivoIsencao), ObOp.Opcional);
                    }
                    wCampo(med.vPMC, TpcnTipoCampo.tcDec2, TpcnResources.vPMC);
                }

                #endregion /// medicamentos

                #region /// armamento

                if(det.Prod.arma.Count > 500)
                {
                    cMensagemErro += "Número máximo de itens em 'arma' excedeu o máximo permitido" + Environment.NewLine;
                }

                foreach(Arma arma in det.Prod.arma)
                {
                    XmlElement nodeArma = doc.CreateElement("arma");
                    nodeProd.AppendChild(nodeArma);
                    nodeCurrent = nodeArma;

                    wCampo(arma.tpArma, TpcnTipoCampo.tcInt, TpcnResources.tpArma);
                    wCampo(arma.nSerie, TpcnTipoCampo.tcStr, TpcnResources.nSerie);
                    wCampo(arma.nCano, TpcnTipoCampo.tcStr, TpcnResources.nCano);
                    wCampo(arma.descr, TpcnTipoCampo.tcStr, TpcnResources.descr);
                }

                #endregion /// armamento

                #region /// combustiveis

                if(det.Prod.comb.cProdANP > 0)
                {
                    XmlElement e0 = doc.CreateElement("comb");
                    nodeProd.AppendChild(e0);
                    nodeCurrent = e0;

                    wCampo(det.Prod.comb.cProdANP, TpcnTipoCampo.tcInt, TpcnResources.cProdANP);
                    if(NFe.infNFe.Versao < 4)
                    {
                        wCampo(det.Prod.comb.pMixGN, TpcnTipoCampo.tcDec4, TpcnResources.pMixGN, ObOp.Opcional);
                    }
                    else
                    {
                        wCampo(det.Prod.comb.descANP, TpcnTipoCampo.tcStr, TpcnResources.descANP);
                        wCampo(det.Prod.comb.pGLP, TpcnTipoCampo.tcDec4, TpcnResources.pGLP, ObOp.Opcional);
                        wCampo(det.Prod.comb.pGNn, TpcnTipoCampo.tcDec4, TpcnResources.pGNn, ObOp.Opcional);
                        wCampo(det.Prod.comb.pGNi, TpcnTipoCampo.tcDec4, TpcnResources.pGNi, ObOp.Opcional);
                        wCampo(det.Prod.comb.vPart, TpcnTipoCampo.tcDec2, TpcnResources.vPart, ObOp.Opcional);
                    }
                    if(!string.IsNullOrEmpty(det.Prod.comb.CODIF))
                    {
                        wCampo(det.Prod.comb.CODIF, TpcnTipoCampo.tcStr, TpcnResources.CODIF);
                    }

                    if(det.Prod.comb.qTemp > 0)
                    {
                        wCampo(det.Prod.comb.qTemp, TpcnTipoCampo.tcDec4, TpcnResources.qTemp);
                    }

                    wCampo(det.Prod.comb.UFCons, TpcnTipoCampo.tcStr, TpcnResources.UFCons);

                    if((det.Prod.comb.CIDE.qBCprod > 0) ||
                        (det.Prod.comb.CIDE.vAliqProd > 0) ||
                        (det.Prod.comb.CIDE.vCIDE > 0))
                    {
                        XmlElement e1 = doc.CreateElement("CIDE");
                        e0.AppendChild(e1);
                        nodeCurrent = e1;

                        wCampo(det.Prod.comb.CIDE.qBCprod, TpcnTipoCampo.tcDec4, TpcnResources.qBCProd);
                        wCampo(det.Prod.comb.CIDE.vAliqProd, TpcnTipoCampo.tcDec4, TpcnResources.vAliqProd);
                        wCampo(det.Prod.comb.CIDE.vCIDE, TpcnTipoCampo.tcDec2, TpcnResources.vCIDE);

                        nodeCurrent = e0;
                    }
                    if(det.Prod.comb.encerrante.nBico > 0 &&
                        det.Prod.comb.encerrante.nTanque > 0 &&
                        !string.IsNullOrEmpty(det.Prod.comb.encerrante.vEncIni) &&
                        !string.IsNullOrEmpty(det.Prod.comb.encerrante.vEncFin))
                    {
                        XmlElement e1 = doc.CreateElement("encerrante");
                        e0.AppendChild(e1);
                        nodeCurrent = e1;

                        wCampo(det.Prod.comb.encerrante.nBico, TpcnTipoCampo.tcInt, TpcnResources.nBico);
                        wCampo(det.Prod.comb.encerrante.nBomba, TpcnTipoCampo.tcInt, TpcnResources.nBomba, ObOp.Opcional);
                        wCampo(det.Prod.comb.encerrante.nTanque, TpcnTipoCampo.tcInt, TpcnResources.nTanque);
                        wCampo(det.Prod.comb.encerrante.vEncIni, TpcnTipoCampo.tcStr, TpcnResources.vEncIni);
                        wCampo(det.Prod.comb.encerrante.vEncFin, TpcnTipoCampo.tcStr, TpcnResources.vEncFin);

                        nodeCurrent = e0;
                    }
                }

                #endregion /// combustiveis

                wCampo(det.Prod.nRECOPI, TpcnTipoCampo.tcStr, TpcnResources.nRECOPI, ObOp.Opcional);

                GerarDetImposto(NFe, det.Imposto, rootDet);

                GerarDetDevol(det.impostoDevol, rootDet);

                nodeCurrent = rootDet;
                wCampo(det.infAdProd, TpcnTipoCampo.tcStr, TpcnResources.infAdProd, ObOp.Opcional);
            }
        }

        /// <summary>
        /// GerarDetImposto
        /// </summary>
        /// <param name="nfe"></param>
        /// <param name="imposto"></param>
        /// <param name="root"></param>
        private void GerarDetImposto(NFe nfe, Imposto imposto, XmlElement root)
        {
            XmlElement nodeImposto = doc.CreateElement("imposto");
            root.AppendChild(nodeImposto);

            nodeCurrent = nodeImposto;
            wCampo(imposto.vTotTrib, TpcnTipoCampo.tcDec2, TpcnResources.vTotTrib, ObOp.Opcional);

            if((double)nfe.infNFe.Versao < 3.10)
            {
                if(!string.IsNullOrEmpty(imposto.ISSQN.cSitTrib))
                {
                    GerarDetImpostoISSQN(nfe, imposto, nodeImposto);
                }
                else
                {
                    GerarDetImpostoICMS(nfe, imposto, nodeImposto);
                    GerarDetImpostoIPI(nfe, imposto.IPI, nodeImposto);
                    if(nfe.det[0].Prod.DI.Count > 0)
                    {
                        GerarDetImpostoII(imposto.II, nodeImposto);
                    }
                }
            }
            else
            {
                GerarDetImpostoICMS(nfe, imposto, nodeImposto);
                GerarDetImpostoIPI(nfe, imposto.IPI, nodeImposto);
                if(nfe.det[0].Prod.DI.Count > 0)
                {
                    GerarDetImpostoII(imposto.II, nodeImposto);
                }

                GerarDetImpostoISSQN(nfe, imposto, nodeImposto);
            }
            GerarDetImpostoPIS(nfe, imposto.PIS, nodeImposto);
            GerarDetImpostoPISST(nfe, imposto.PISST, nodeImposto);
            GerarDetImpostoCOFINS(nfe, imposto.COFINS, nodeImposto);
            GerarDetImpostoCOFINSST(nfe, imposto.COFINSST, nodeImposto);
            GerarDetImpostoICMSUFDest(nfe, imposto, nodeImposto);
        }

        /// <summary>
        /// GerarDetImpostoISSQN
        /// </summary>
        /// <param name="nfe"></param>
        /// <param name="imposto"></param>
        /// <param name="nodeImposto"></param>
        private void GerarDetImpostoISSQN(NFe nfe, Imposto imposto, XmlElement nodeImposto)
        {
            if((imposto.ISSQN.vBC > 0) ||
                (imposto.ISSQN.vAliq > 0) ||
                (imposto.ISSQN.vISSQN > 0) ||
                (imposto.ISSQN.cMunFG > 0) ||
                (!string.IsNullOrEmpty(imposto.ISSQN.cListServ)))
            {
                nodeCurrent = doc.CreateElement("ISSQN");
                nodeImposto.AppendChild(nodeCurrent);

                wCampo(imposto.ISSQN.vBC, TpcnTipoCampo.tcDec2, TpcnResources.vBC);
                wCampo(imposto.ISSQN.vAliq, nDecimaisPerc, TpcnResources.vAliq);
                wCampo(imposto.ISSQN.vISSQN, TpcnTipoCampo.tcDec2, TpcnResources.vISSQN);
                wCampo(imposto.ISSQN.cMunFG, TpcnTipoCampo.tcInt, TpcnResources.cMunFG, ObOp.Obrigatorio, 7);
                wCampo(imposto.ISSQN.cListServ, TpcnTipoCampo.tcStr, TpcnResources.cListServ);
                if((double)nfe.infNFe.Versao >= 3.10)
                {
                    wCampo(imposto.ISSQN.vDeducao, TpcnTipoCampo.tcDec2, TpcnResources.vDeducao, ObOp.Opcional);
                    wCampo(imposto.ISSQN.vOutro, TpcnTipoCampo.tcDec2, TpcnResources.vOutro, ObOp.Opcional);
                    wCampo(imposto.ISSQN.vDescIncond, TpcnTipoCampo.tcDec2, TpcnResources.vDescIncond, ObOp.Opcional);
                    wCampo(imposto.ISSQN.vDescCond, TpcnTipoCampo.tcDec2, TpcnResources.vDescCond, ObOp.Opcional);
                    wCampo(imposto.ISSQN.vISSRet, TpcnTipoCampo.tcDec2, TpcnResources.vISSRet, ObOp.Opcional);
                    wCampo((int)imposto.ISSQN.indISS, TpcnTipoCampo.tcInt, TpcnResources.indISS);
                    wCampo(imposto.ISSQN.cServico, TpcnTipoCampo.tcStr, TpcnResources.cServico, ObOp.Opcional);
                    wCampo(imposto.ISSQN.cMun, TpcnTipoCampo.tcInt, TpcnResources.cMun, ObOp.Opcional);
                    wCampo(imposto.ISSQN.cPais, TpcnTipoCampo.tcInt, TpcnResources.cPais, ObOp.Opcional);
                    wCampo(imposto.ISSQN.nProcesso, TpcnTipoCampo.tcStr, TpcnResources.nProcesso, ObOp.Opcional);
                    wCampo(imposto.ISSQN.indIncentivo ? 1 : 2, TpcnTipoCampo.tcInt, TpcnResources.indIncentivo, ObOp.Opcional);
                }
                else
                {
                    wCampo(imposto.ISSQN.cSitTrib, TpcnTipoCampo.tcStr, TpcnResources.cSitTrib);
                }
            }
        }

        /// <summary>
        /// GerarDetImpostoCOFINS
        /// </summary>
        /// <param name="COFINS"></param>
        /// <param name="nodeImposto"></param>
        private void GerarDetImpostoCOFINS(NFe nfe, COFINS COFINS, XmlElement nodeImposto)
        {
            if(!string.IsNullOrEmpty(COFINS.CST))
            {
                XmlElement node0 = doc.CreateElement("COFINS");

                switch(COFINS.CST)
                {
                    case "01":
                    case "02":
                        {
                            nodeCurrent = doc.CreateElement("COFINSAliq");
                            node0.AppendChild(nodeCurrent);
                            nodeImposto.AppendChild(node0);

                            wCampo(COFINS.CST, TpcnTipoCampo.tcStr, TpcnResources.CST);
                            wCampo(COFINS.vBC, TpcnTipoCampo.tcDec2, TpcnResources.vBC);
                            wCampo(COFINS.pCOFINS, nDecimaisPerc, TpcnResources.pCOFINS);
                            wCampo(COFINS.vCOFINS, TpcnTipoCampo.tcDec2, TpcnResources.vCOFINS);
                        }
                        break;

                    case "03":
                        {
                            nodeCurrent = doc.CreateElement("COFINSQtde");
                            node0.AppendChild(nodeCurrent);
                            nodeImposto.AppendChild(node0);

                            wCampo(COFINS.CST, TpcnTipoCampo.tcStr, TpcnResources.CST);
                            wCampo(COFINS.qBCProd, TpcnTipoCampo.tcDec4, TpcnResources.qBCProd);
                            wCampo(COFINS.vAliqProd, TpcnTipoCampo.tcDec4, TpcnResources.vAliqProd);
                            wCampo(COFINS.vCOFINS, TpcnTipoCampo.tcDec2, TpcnResources.vCOFINS);
                        }
                        break;

                    case "04":
                    case "05":
                    case "06":
                    case "07":
                    case "08":
                    case "09":
                        {
                            nodeCurrent = doc.CreateElement("COFINSNT");
                            node0.AppendChild(nodeCurrent);
                            nodeImposto.AppendChild(node0);

                            wCampo(COFINS.CST, TpcnTipoCampo.tcStr, TpcnResources.CST);
                        }
                        break;

                    case "49":
                    case "50":
                    case "51":
                    case "52":
                    case "53":
                    case "54":
                    case "55":
                    case "56":
                    case "60":
                    case "61":
                    case "62":
                    case "63":
                    case "64":
                    case "65":
                    case "66":
                    case "67":
                    case "70":
                    case "71":
                    case "72":
                    case "73":
                    case "74":
                    case "75":
                    case "98":

                    case "99":
                        {
                            if((COFINS.vBC + COFINS.pCOFINS > 0) && (COFINS.qBCProd + COFINS.vAliqProd > 0))
                            {
                                cMensagemErro += "COFINSOutr: As TAG's <vBC> e <pCOFINS> não podem ser informadas em conjunto com as TAG <qBCProd> e <vAliqProd>" + Environment.NewLine;
                            }

                            nodeCurrent = doc.CreateElement("COFINSOutr");
                            node0.AppendChild(nodeCurrent);
                            nodeImposto.AppendChild(node0);

                            wCampo(COFINS.CST, TpcnTipoCampo.tcStr, TpcnResources.CST);

                            if(COFINS.qBCProd + COFINS.vAliqProd > 0)
                            {
                                wCampo(COFINS.qBCProd, TpcnTipoCampo.tcDec4, TpcnResources.qBCProd);
                                wCampo(COFINS.vAliqProd, TpcnTipoCampo.tcDec4, TpcnResources.vAliqProd);
                            }
                            else
                            {
                                wCampo(COFINS.vBC, TpcnTipoCampo.tcDec2, TpcnResources.vBC);
                                wCampo(COFINS.pCOFINS, nDecimaisPerc, TpcnResources.pCOFINS);
                            }
                            wCampo(COFINS.vCOFINS, TpcnTipoCampo.tcDec2, TpcnResources.vCOFINS);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// GerarDetImpostoCOFINSST
        /// </summary>
        /// <param name="COFINSST"></param>
        /// <param name="nodeImposto"></param>
        private void GerarDetImpostoCOFINSST(NFe nfe, COFINSST COFINSST, XmlElement nodeImposto)
        {
            if((COFINSST.vBC > 0) ||
                (COFINSST.pCOFINS > 0) ||
                (COFINSST.qBCProd > 0) ||
                (COFINSST.vAliqProd > 0) ||
                (COFINSST.vCOFINS > 0))
            {
                if((COFINSST.vBC + COFINSST.pCOFINS > 0) && (COFINSST.qBCProd + COFINSST.vAliqProd > 0))
                {
                    cMensagemErro += "COFINSST: As TAG's <vBC> e <pCOFINS> não podem ser informadas em conjunto com as TAG <qBCProd> e <vAliqProd>" + Environment.NewLine;
                }

                XmlElement node0 = doc.CreateElement("COFINSST");
                nodeCurrent = node0;

                if(COFINSST.vBC + COFINSST.pCOFINS > 0)
                {
                    nodeImposto.AppendChild(node0);

                    wCampo(COFINSST.vBC, TpcnTipoCampo.tcDec2, TpcnResources.vBC);
                    wCampo(COFINSST.pCOFINS, nDecimaisPerc, TpcnResources.pCOFINS);
                    wCampo(COFINSST.vCOFINS, TpcnTipoCampo.tcDec2, TpcnResources.vCOFINS);
                }
                if(COFINSST.qBCProd + COFINSST.vAliqProd > 0)
                {
                    nodeImposto.AppendChild(node0);

                    wCampo(COFINSST.qBCProd, TpcnTipoCampo.tcDec4, TpcnResources.qBCProd);
                    wCampo(COFINSST.vAliqProd, TpcnTipoCampo.tcDec4, TpcnResources.vAliqProd);
                    wCampo(COFINSST.vCOFINS, TpcnTipoCampo.tcDec2, TpcnResources.vCOFINS);
                    wCampo(COFINSST.indSomaCOFINSST, TpcnTipoCampo.tcStr, TpcnResources.indSomaCOFINSST, ObOp.Opcional, 0);
                }
            }
        }

        /// <summary>
        /// GerarDetImpostoICMS
        /// </summary>
        /// <param name="nfe"></param>
        /// <param name="imposto"></param>
        /// <param name="nodeImposto"></param>
        private void GerarDetImpostoICMS(NFe nfe, Imposto imposto, XmlElement nodeImposto)
        {
            if(!string.IsNullOrEmpty(imposto.ICMS.CST))
            {
                if(imposto.ICMS.ICMSst == 1)
                {
                    XmlElement e0 = doc.CreateElement(TpcnResources.ICMS.ToString());
                    nodeCurrent = doc.CreateElement("ICMSST");
                    e0.AppendChild(nodeCurrent);
                    nodeImposto.AppendChild(e0);

                    wCampo(imposto.ICMS.orig, TpcnTipoCampo.tcInt, TpcnResources.orig);
                    wCampo(imposto.ICMS.CST, TpcnTipoCampo.tcStr, TpcnResources.CST);
                    wCampo(imposto.ICMS.vBCSTRet, TpcnTipoCampo.tcDec2, TpcnResources.vBCSTRet);
                    if(nfe.infNFe.Versao >= 4)
                    {
                        wCampo(imposto.ICMS.pST, nDecimaisPerc, TpcnResources.pST);
                        wCampo(imposto.ICMS.vICMSSubstituto, TpcnTipoCampo.tcDec2, TpcnResources.vICMSSubstituto);
                    }
                    wCampo(imposto.ICMS.vICMSSTRet, TpcnTipoCampo.tcDec2, TpcnResources.vICMSSTRet);
                    wCampo(imposto.ICMS.vBCFCPSTRet, TpcnTipoCampo.tcDec2, TpcnResources.vBCFCPSTRet);
                    wCampo(imposto.ICMS.pFCPSTRet, nDecimaisPerc, TpcnResources.pFCPSTRet);
                    wCampo(imposto.ICMS.vFCPSTRet, TpcnTipoCampo.tcDec2, TpcnResources.vFCPSTRet);
                    wCampo(imposto.ICMS.vBCSTDest, TpcnTipoCampo.tcDec2, TpcnResources.vBCSTDest);
                    wCampo(imposto.ICMS.vICMSSTDest, TpcnTipoCampo.tcDec2, TpcnResources.vICMSSTDest);
                    wCampo(imposto.ICMS.pRedBCEfet, nDecimaisPerc, TpcnResources.pRedBCEfet);
                    wCampo(imposto.ICMS.vBCEfet, TpcnTipoCampo.tcDec2, TpcnResources.vBCEfet);
                    wCampo(imposto.ICMS.pICMSEfet, nDecimaisPerc, TpcnResources.pICMSEfet);
                    wCampo(imposto.ICMS.vICMSEfet, TpcnTipoCampo.tcDec2, TpcnResources.vICMSEfet);
                }
                else
                {
                    XmlElement e0 = doc.CreateElement(TpcnResources.ICMS.ToString());
                    if(imposto.ICMS.ICMSPart10 == 1 || imposto.ICMS.ICMSPart90 == 1)
                    {
                        nodeCurrent = doc.CreateElement("ICMSPart");
                    }
                    else
                    {
                        nodeCurrent = doc.CreateElement(TpcnResources.ICMS.ToString() + (imposto.ICMS.CST == "41" || imposto.ICMS.CST == "50" ? "40" : imposto.ICMS.CST));
                    }

                    e0.AppendChild(nodeCurrent);
                    nodeImposto.AppendChild(e0);

                    wCampo(imposto.ICMS.orig, TpcnTipoCampo.tcInt, TpcnResources.orig);
                    wCampo(imposto.ICMS.CST, TpcnTipoCampo.tcStr, TpcnResources.CST);

                    switch(imposto.ICMS.CST)
                    {
                        case "00":
                            wCampo(imposto.ICMS.modBC, TpcnTipoCampo.tcInt, TpcnResources.modBC);
                            wCampo(imposto.ICMS.vBC, TpcnTipoCampo.tcDec2, TpcnResources.vBC);
                            wCampo(imposto.ICMS.pICMS, nDecimaisPerc, TpcnResources.pICMS);
                            wCampo(imposto.ICMS.vICMS, TpcnTipoCampo.tcDec2, TpcnResources.vICMS);
                            if(nfe.infNFe.Versao >= 4)
                            {
                                wCampo(imposto.ICMS.pFCP, nDecimaisPerc, TpcnResources.pFCP, ObOp.Opcional);
                                wCampo(imposto.ICMS.vFCP, TpcnTipoCampo.tcDec2, TpcnResources.vFCP, ObOp.Opcional);
                            }
                            break;

                        case "10":
                            wCampo(imposto.ICMS.modBC, TpcnTipoCampo.tcInt, TpcnResources.modBC);
                            if(imposto.ICMS.ICMSPart10 == 1)
                            {
                                wCampo(imposto.ICMS.pRedBC, TpcnTipoCampo.tcDec2, TpcnResources.pRedBC, ObOp.Opcional);
                            }

                            wCampo(imposto.ICMS.vBC, TpcnTipoCampo.tcDec2, TpcnResources.vBC);
                            wCampo(imposto.ICMS.pICMS, nDecimaisPerc, TpcnResources.pICMS);
                            wCampo(imposto.ICMS.vICMS, TpcnTipoCampo.tcDec2, TpcnResources.vICMS);
                            if(nfe.infNFe.Versao >= 4
                                && imposto.ICMS.vBCFCP + imposto.ICMS.pFCP + imposto.ICMS.vFCP > 0)
                            {
                                wCampo(imposto.ICMS.vBCFCP, TpcnTipoCampo.tcDec2, TpcnResources.vBCFCP);//, ObOp.Opcional);
                                wCampo(imposto.ICMS.pFCP, nDecimaisPerc, TpcnResources.pFCP);//, ObOp.Opcional);
                                wCampo(imposto.ICMS.vFCP, TpcnTipoCampo.tcDec2, TpcnResources.vFCP);//, ObOp.Opcional);
                            }
                            wCampo(imposto.ICMS.modBCST, TpcnTipoCampo.tcInt, TpcnResources.modBCST);

                            if(imposto.ICMS.modBCST == TpcnDeterminacaoBaseIcmsST.dbisMargemValorAgregado)
                            {
                                wCampo(imposto.ICMS.pMVAST, nDecimaisPerc, TpcnResources.pMVAST, ObOp.Obrigatorio);
                            }
                            else
                            {
                                wCampo(imposto.ICMS.pMVAST, nDecimaisPerc, TpcnResources.pMVAST, ObOp.Opcional);
                            }
                            wCampo(imposto.ICMS.pRedBCST, nDecimaisPerc, TpcnResources.pRedBCST, ObOp.Opcional);
                            wCampo(imposto.ICMS.vBCST, TpcnTipoCampo.tcDec2, TpcnResources.vBCST);
                            wCampo(imposto.ICMS.pICMSST, nDecimaisPerc, TpcnResources.pICMSST);
                            wCampo(imposto.ICMS.vICMSST, TpcnTipoCampo.tcDec2, TpcnResources.vICMSST);
                            if(imposto.ICMS.ICMSPart10 == 1)
                            {
                                wCampo(imposto.ICMS.pBCOp, nDecimaisPerc, TpcnResources.pBCOp);
                                wCampo(imposto.ICMS.UFST, TpcnTipoCampo.tcStr, TpcnResources.UFST);
                            }
                            if(nfe.infNFe.Versao >= 4
                                && imposto.ICMS.vBCFCPST + imposto.ICMS.pFCPST + imposto.ICMS.vFCPST > 0)
                            {
                                wCampo(imposto.ICMS.vBCFCPST, TpcnTipoCampo.tcDec2, TpcnResources.vBCFCPST);//, ObOp.Opcional);
                                wCampo(imposto.ICMS.pFCPST, nDecimaisPerc, TpcnResources.pFCPST);//, ObOp.Opcional);
                                wCampo(imposto.ICMS.vFCPST, TpcnTipoCampo.tcDec2, TpcnResources.vFCPST);//, ObOp.Opcional);
                            }

                            if(imposto.ICMS.vICMSSTDeson > 0)
                            {
                                wCampo(imposto.ICMS.vICMSSTDeson, TpcnTipoCampo.tcDec2, TpcnResources.vICMSSTDeson, ObOp.Opcional);
                                wCampo(imposto.ICMS.motDesICMSST, TpcnTipoCampo.tcInt, TpcnResources.motDesICMSST, ObOp.Opcional);
                            }

                            break;

                        case "20":
                            wCampo(imposto.ICMS.modBC, TpcnTipoCampo.tcInt, TpcnResources.modBC);
                            wCampo(imposto.ICMS.pRedBC, nDecimaisPerc, TpcnResources.pRedBC);
                            wCampo(imposto.ICMS.vBC, TpcnTipoCampo.tcDec2, TpcnResources.vBC);
                            wCampo(imposto.ICMS.pICMS, nDecimaisPerc, TpcnResources.pICMS);
                            wCampo(imposto.ICMS.vICMS, TpcnTipoCampo.tcDec2, TpcnResources.vICMS);
                            if(nfe.infNFe.Versao >= 4
                                && imposto.ICMS.vBCFCP + imposto.ICMS.pFCP + imposto.ICMS.vFCP > 0)
                            {
                                wCampo(imposto.ICMS.vBCFCP, TpcnTipoCampo.tcDec2, TpcnResources.vBCFCP);//, ObOp.Opcional);
                                wCampo(imposto.ICMS.pFCP, nDecimaisPerc, TpcnResources.pFCP);//, ObOp.Opcional);
                                wCampo(imposto.ICMS.vFCP, TpcnTipoCampo.tcDec2, TpcnResources.vFCP);//, ObOp.Opcional);
                            }
                            if((double)nfe.infNFe.Versao >= 3.10 && imposto.ICMS.vICMSDeson > 0)
                            {
                                wCampo(imposto.ICMS.vICMSDeson, TpcnTipoCampo.tcDec2, TpcnResources.vICMSDeson);
                                wCampo(imposto.ICMS.motDesICMS, TpcnTipoCampo.tcInt, TpcnResources.motDesICMS);
                            }
                            break;

                        case "30":
                            wCampo(imposto.ICMS.modBCST, TpcnTipoCampo.tcInt, TpcnResources.modBCST);

                            if(imposto.ICMS.modBCST == TpcnDeterminacaoBaseIcmsST.dbisMargemValorAgregado)
                            {
                                wCampo(imposto.ICMS.pMVAST, nDecimaisPerc, TpcnResources.pMVAST, ObOp.Obrigatorio);
                            }
                            else
                            {
                                wCampo(imposto.ICMS.pMVAST, nDecimaisPerc, TpcnResources.pMVAST, ObOp.Opcional);
                            }

                            wCampo(imposto.ICMS.pRedBCST, nDecimaisPerc, TpcnResources.pRedBCST, ObOp.Opcional);
                            wCampo(imposto.ICMS.vBCST, TpcnTipoCampo.tcDec2, TpcnResources.vBCST);
                            wCampo(imposto.ICMS.pICMSST, nDecimaisPerc, TpcnResources.pICMSST);
                            wCampo(imposto.ICMS.vICMSST, TpcnTipoCampo.tcDec2, TpcnResources.vICMSST);
                            if(nfe.infNFe.Versao >= 4
                                && imposto.ICMS.vBCFCPST + imposto.ICMS.pFCPST + imposto.ICMS.vFCPST > 0)
                            {
                                wCampo(imposto.ICMS.vBCFCPST, TpcnTipoCampo.tcDec2, TpcnResources.vBCFCPST);//, ObOp.Opcional);
                                wCampo(imposto.ICMS.pFCPST, nDecimaisPerc, TpcnResources.pFCPST);//, ObOp.Opcional);
                                wCampo(imposto.ICMS.vFCPST, TpcnTipoCampo.tcDec2, TpcnResources.vFCPST);//, ObOp.Opcional);
                            }
                            if((double)nfe.infNFe.Versao >= 3.10 && imposto.ICMS.vICMSDeson > 0)
                            {
                                wCampo(imposto.ICMS.vICMSDeson, TpcnTipoCampo.tcDec2, TpcnResources.vICMSDeson);
                                wCampo(imposto.ICMS.motDesICMS, TpcnTipoCampo.tcInt, TpcnResources.motDesICMS);
                            }
                            break;

                        case "40":
                        case "41":
                        case "50":
                            if((double)nfe.infNFe.Versao >= 3.10)
                            {
                                if(imposto.ICMS.vICMSDeson > 0)
                                {
                                    wCampo(imposto.ICMS.vICMSDeson, TpcnTipoCampo.tcDec2, TpcnResources.vICMSDeson);
                                    wCampo(imposto.ICMS.motDesICMS, TpcnTipoCampo.tcInt, TpcnResources.motDesICMS);
                                }
                            }
                            else
                            {
                                wCampo(imposto.ICMS.vICMS, TpcnTipoCampo.tcDec2, TpcnResources.vICMS, ObOp.Opcional);
                                if(imposto.ICMS.vICMS > 0)
                                {
                                    wCampo(imposto.ICMS.motDesICMS, TpcnTipoCampo.tcInt, TpcnResources.motDesICMS, ObOp.Opcional);
                                }
                            }
                            break;

                        case "51":
                            //Esse bloco fica a critério de cada UF a obrigação das informações, conforme o manual
                            {
                                ObOp obop = ObOp.Opcional;

                                if(imposto.ICMS.pRedBC > 0 ||
                                    imposto.ICMS.vBC > 0 ||
                                    imposto.ICMS.vICMSOp > 0 ||
                                    imposto.ICMS.pDif > 0 ||
                                    imposto.ICMS.vICMSDif > 0 ||
                                    imposto.ICMS.vICMS > 0)
                                {
                                    obop = ObOp.Obrigatorio;
                                }

                                wCampo(imposto.ICMS.modBC, TpcnTipoCampo.tcInt, TpcnResources.modBC, ObOp.Opcional);
                                wCampo(imposto.ICMS.pRedBC, nDecimaisPerc, TpcnResources.pRedBC, obop);
                                wCampo(imposto.ICMS.vBC, TpcnTipoCampo.tcDec2, TpcnResources.vBC, obop);
                                wCampo(imposto.ICMS.pICMS, nDecimaisPerc, TpcnResources.pICMS, obop);
                                wCampo(imposto.ICMS.vICMSOp, TpcnTipoCampo.tcDec2, TpcnResources.vICMSOp, obop);
                                wCampo(imposto.ICMS.pDif, nDecimaisPerc, TpcnResources.pDif, obop);
                                wCampo(imposto.ICMS.vICMSDif, TpcnTipoCampo.tcDec2, TpcnResources.vICMSDif, obop);
                                wCampo(imposto.ICMS.vICMS, TpcnTipoCampo.tcDec2, TpcnResources.vICMS, obop);

                                if(imposto.ICMS.vBCFCP + imposto.ICMS.pFCP + imposto.ICMS.vFCP > 0)
                                {
                                    wCampo(imposto.ICMS.vBCFCP, TpcnTipoCampo.tcDec2, TpcnResources.vBCFCP);
                                    wCampo(imposto.ICMS.pFCP, nDecimaisPerc, TpcnResources.pFCP);
                                    wCampo(imposto.ICMS.vFCP, TpcnTipoCampo.tcDec2, TpcnResources.vFCP);
                                }

                                if(imposto.ICMS.pFCPDif + imposto.ICMS.vFCPDif + imposto.ICMS.vFCPEfet > 0)
                                {
                                    wCampo(imposto.ICMS.pFCPDif, TpcnTipoCampo.tcDec4, TpcnResources.pFCPDif);
                                    wCampo(imposto.ICMS.vFCPDif, TpcnTipoCampo.tcDec2, TpcnResources.vFCPDif);
                                    wCampo(imposto.ICMS.vFCPEfet, TpcnTipoCampo.tcDec2, TpcnResources.vFCPEfet);
                                }
                            }
                            break;

                        case "60":
                            if(imposto.ICMS.vBCSTRet + imposto.ICMS.vICMSSTRet + imposto.ICMS.pST +
                                imposto.ICMS.vBCFCPSTRet + imposto.ICMS.pFCPSTRet + imposto.ICMS.vFCPSTRet +
                                imposto.ICMS.pRedBCEfet + imposto.ICMS.vBCEfet + imposto.ICMS.pICMSEfet + imposto.ICMS.vICMSEfet > 0 || nfe.ide.indFinal == TpcnConsumidorFinal.cfNao)
                            {
                                if(imposto.ICMS.vBCSTRet + imposto.ICMS.pST + imposto.ICMS.vICMSSubstituto + imposto.ICMS.vICMSSTRet > 0 || nfe.ide.indFinal == TpcnConsumidorFinal.cfNao)
                                {
                                    wCampo(imposto.ICMS.vBCSTRet, TpcnTipoCampo.tcDec2, TpcnResources.vBCSTRet);
                                    wCampo(imposto.ICMS.pST, nDecimaisPerc, TpcnResources.pST);
                                    wCampo(imposto.ICMS.vICMSSubstituto, TpcnTipoCampo.tcDec2, TpcnResources.vICMSSubstituto);
                                    wCampo(imposto.ICMS.vICMSSTRet, TpcnTipoCampo.tcDec2, TpcnResources.vICMSSTRet);
                                }

                                if(imposto.ICMS.vBCFCPSTRet + imposto.ICMS.pFCPSTRet + imposto.ICMS.vFCPSTRet > 0)
                                {
                                    wCampo(imposto.ICMS.vBCFCPSTRet, TpcnTipoCampo.tcDec2, TpcnResources.vBCFCPSTRet);//, ObOp.Opcional);
                                    wCampo(imposto.ICMS.pFCPSTRet, nDecimaisPerc, TpcnResources.pFCPSTRet);//, ObOp.Opcional);
                                    wCampo(imposto.ICMS.vFCPSTRet, TpcnTipoCampo.tcDec2, TpcnResources.vFCPSTRet);//, ObOp.Opcional);
                                }

                                if(imposto.ICMS.pRedBCEfet + imposto.ICMS.vBCEfet + imposto.ICMS.pICMSEfet + imposto.ICMS.vICMSEfet > 0)
                                {
                                    wCampo(imposto.ICMS.pRedBCEfet, nDecimaisPerc, TpcnResources.pRedBCEfet);
                                    wCampo(imposto.ICMS.vBCEfet, TpcnTipoCampo.tcDec2, TpcnResources.vBCEfet);
                                    wCampo(imposto.ICMS.pICMSEfet, nDecimaisPerc, TpcnResources.pICMSEfet);
                                    wCampo(imposto.ICMS.vICMSEfet, TpcnTipoCampo.tcDec2, TpcnResources.vICMSEfet);
                                }
                            }
                            break;

                        case "70":
                            wCampo(imposto.ICMS.modBC, TpcnTipoCampo.tcInt, TpcnResources.modBC);
                            wCampo(imposto.ICMS.pRedBC, nDecimaisPerc, TpcnResources.pRedBC);
                            wCampo(imposto.ICMS.vBC, TpcnTipoCampo.tcDec2, TpcnResources.vBC);
                            wCampo(imposto.ICMS.pICMS, nDecimaisPerc, TpcnResources.pICMS);
                            wCampo(imposto.ICMS.vICMS, TpcnTipoCampo.tcDec2, TpcnResources.vICMS);
                            if(nfe.infNFe.Versao >= 4
                                 && imposto.ICMS.vBCFCP + imposto.ICMS.pFCP + imposto.ICMS.vFCP > 0)
                            {
                                wCampo(imposto.ICMS.vBCFCP, TpcnTipoCampo.tcDec2, TpcnResources.vBCFCP);//, ObOp.Opcional);
                                wCampo(imposto.ICMS.pFCP, nDecimaisPerc, TpcnResources.pFCP);//, ObOp.Opcional);
                                wCampo(imposto.ICMS.vFCP, TpcnTipoCampo.tcDec2, TpcnResources.vFCP);//, ObOp.Opcional);
                            }
                            wCampo(imposto.ICMS.modBCST, TpcnTipoCampo.tcInt, TpcnResources.modBCST);

                            if(imposto.ICMS.modBCST == TpcnDeterminacaoBaseIcmsST.dbisMargemValorAgregado)
                            {
                                wCampo(imposto.ICMS.pMVAST, nDecimaisPerc, TpcnResources.pMVAST, ObOp.Obrigatorio);
                            }
                            else
                            {
                                wCampo(imposto.ICMS.pMVAST, nDecimaisPerc, TpcnResources.pMVAST, ObOp.Opcional);
                            }

                            wCampo(imposto.ICMS.pRedBCST, nDecimaisPerc, TpcnResources.pRedBCST, ObOp.Opcional);
                            wCampo(imposto.ICMS.vBCST, TpcnTipoCampo.tcDec2, TpcnResources.vBCST);
                            wCampo(imposto.ICMS.pICMSST, nDecimaisPerc, TpcnResources.pICMSST);
                            wCampo(imposto.ICMS.vICMSST, TpcnTipoCampo.tcDec2, TpcnResources.vICMSST);
                            if(nfe.infNFe.Versao >= 4
                                 && imposto.ICMS.vBCFCPST + imposto.ICMS.pFCPST + imposto.ICMS.vFCPST > 0)
                            {
                                wCampo(imposto.ICMS.vBCFCPST, TpcnTipoCampo.tcDec2, TpcnResources.vBCFCPST);//, ObOp.Opcional);
                                wCampo(imposto.ICMS.pFCPST, nDecimaisPerc, TpcnResources.pFCPST);//, ObOp.Opcional);
                                wCampo(imposto.ICMS.vFCPST, TpcnTipoCampo.tcDec2, TpcnResources.vFCPST);//, ObOp.Opcional);
                            }
                            if((double)nfe.infNFe.Versao >= 3.10 && imposto.ICMS.vICMSDeson > 0)
                            {
                                wCampo(imposto.ICMS.vICMSDeson, TpcnTipoCampo.tcDec2, TpcnResources.vICMSDeson);
                                wCampo(imposto.ICMS.motDesICMS, TpcnTipoCampo.tcInt, TpcnResources.motDesICMS);
                            }

                            if(imposto.ICMS.vICMSSTDeson > 0)
                            {
                                wCampo(imposto.ICMS.vICMSSTDeson, TpcnTipoCampo.tcDec2, TpcnResources.vICMSSTDeson, ObOp.Opcional);
                                wCampo(imposto.ICMS.motDesICMSST, TpcnTipoCampo.tcInt, TpcnResources.motDesICMSST, ObOp.Opcional);
                            }

                            break;

                        case "90":
                            wCampo(imposto.ICMS.modBC, TpcnTipoCampo.tcInt, TpcnResources.modBC);
                            wCampo(imposto.ICMS.vBC, TpcnTipoCampo.tcDec2, TpcnResources.vBC);

                            if(imposto.ICMS.ICMSPart90 == 1)
                            {
                                wCampo(imposto.ICMS.pRedBC, nDecimaisPerc, TpcnResources.pRedBC, ObOp.Opcional);
                            }

                            wCampo(imposto.ICMS.pICMS, nDecimaisPerc, TpcnResources.pICMS);
                            wCampo(imposto.ICMS.vICMS, TpcnTipoCampo.tcDec2, TpcnResources.vICMS);

                            if(nfe.infNFe.Versao >= 4
                                && imposto.ICMS.vBCFCP + imposto.ICMS.pFCP + imposto.ICMS.vFCP > 0)
                            {
                                wCampo(imposto.ICMS.vBCFCP, TpcnTipoCampo.tcDec2, TpcnResources.vBCFCP);//, ObOp.Opcional);
                                wCampo(imposto.ICMS.pFCP, nDecimaisPerc, TpcnResources.pFCP);//, ObOp.Opcional);
                                wCampo(imposto.ICMS.vFCP, TpcnTipoCampo.tcDec2, TpcnResources.vFCP);//, ObOp.Opcional);
                            }

                            if(imposto.ICMS.vBCST > 0 || imposto.ICMS.vICMSST > 0)
                            {
                                wCampo(imposto.ICMS.modBCST, TpcnTipoCampo.tcInt, TpcnResources.modBCST);

                                if(imposto.ICMS.modBCST == TpcnDeterminacaoBaseIcmsST.dbisMargemValorAgregado)
                                {
                                    wCampo(imposto.ICMS.pMVAST, nDecimaisPerc, TpcnResources.pMVAST, ObOp.Obrigatorio);
                                }
                                else
                                {
                                    wCampo(imposto.ICMS.pMVAST, nDecimaisPerc, TpcnResources.pMVAST, ObOp.Opcional);
                                }

                                wCampo(imposto.ICMS.pRedBCST, nDecimaisPerc, TpcnResources.pRedBCST, ObOp.Opcional);
                                wCampo(imposto.ICMS.vBCST, TpcnTipoCampo.tcDec2, TpcnResources.vBCST);
                                wCampo(imposto.ICMS.pICMSST, nDecimaisPerc, TpcnResources.pICMSST);
                                wCampo(imposto.ICMS.vICMSST, TpcnTipoCampo.tcDec2, TpcnResources.vICMSST);
                            }

                            if(nfe.infNFe.Versao >= 4
                                 && imposto.ICMS.vBCFCPST + imposto.ICMS.pFCPST + imposto.ICMS.vFCPST > 0)
                            {
                                wCampo(imposto.ICMS.vBCFCPST, TpcnTipoCampo.tcDec2, TpcnResources.vBCFCPST);//, ObOp.Opcional);
                                wCampo(imposto.ICMS.pFCPST, nDecimaisPerc, TpcnResources.pFCPST);//, ObOp.Opcional);
                                wCampo(imposto.ICMS.vFCPST, TpcnTipoCampo.tcDec2, TpcnResources.vFCPST);//, ObOp.Opcional);
                            }

                            if(imposto.ICMS.ICMSPart90 == 1)
                            {
                                wCampo(imposto.ICMS.pBCOp, nDecimaisPerc, TpcnResources.pBCOp);
                                wCampo(imposto.ICMS.UFST, TpcnTipoCampo.tcStr, TpcnResources.UFST);
                            }
                            else
                            {
                                if((double)nfe.infNFe.Versao >= 3.10 && imposto.ICMS.vICMSDeson > 0)
                                {
                                    wCampo(imposto.ICMS.vICMSDeson, TpcnTipoCampo.tcDec2, TpcnResources.vICMSDeson);
                                    wCampo(imposto.ICMS.motDesICMS, TpcnTipoCampo.tcInt, TpcnResources.motDesICMS);
                                }

                                if(imposto.ICMS.vICMSSTDeson > 0)
                                {
                                    wCampo(imposto.ICMS.vICMSSTDeson, TpcnTipoCampo.tcDec2, TpcnResources.vICMSSTDeson, ObOp.Opcional);
                                    wCampo(imposto.ICMS.motDesICMSST, TpcnTipoCampo.tcInt, TpcnResources.motDesICMSST, ObOp.Opcional);
                                }
                            }

                            break;
                    }
                }
            }

            if(imposto.ICMS.CSOSN > 100)
            {
                XmlElement e0 = doc.CreateElement(TpcnResources.ICMS.ToString());
                switch(imposto.ICMS.CSOSN)
                {
                    case 101: nodeCurrent = doc.CreateElement("ICMSSN101"); break;
                    case 102:
                    case 103:
                    case 300:
                    case 400: nodeCurrent = doc.CreateElement("ICMSSN102"); break;
                    case 201: nodeCurrent = doc.CreateElement("ICMSSN201"); break;
                    case 202:
                    case 203: nodeCurrent = doc.CreateElement("ICMSSN202"); break;
                    case 500: nodeCurrent = doc.CreateElement("ICMSSN500"); break;
                    case 900: nodeCurrent = doc.CreateElement("ICMSSN900"); break;

                    default:
                        throw new Exception(string.Format("O CSOSN {0} informado é incorreto. CSOSN permitidos são: 101, 102, 103, 201, 202, 203, 300, 400, 500 e 900.", imposto.ICMS.CSOSN));
                }
                e0.AppendChild(nodeCurrent);
                nodeImposto.AppendChild(e0);

                wCampo(imposto.ICMS.orig, TpcnTipoCampo.tcInt, TpcnResources.orig);
                wCampo(imposto.ICMS.CSOSN, TpcnTipoCampo.tcInt, TpcnResources.CSOSN);

                switch(imposto.ICMS.CSOSN)
                {
                    case 101:
                        wCampo(imposto.ICMS.pCredSN, nDecimaisPerc, TpcnResources.pCredSN);
                        wCampo(imposto.ICMS.vCredICMSSN, TpcnTipoCampo.tcDec2, TpcnResources.vCredICMSSN);
                        break;

                    case 201:
                        wCampo(imposto.ICMS.modBCST, TpcnTipoCampo.tcInt, TpcnResources.modBCST);

                        if(imposto.ICMS.modBCST == TpcnDeterminacaoBaseIcmsST.dbisMargemValorAgregado)
                        {
                            wCampo(imposto.ICMS.pMVAST, nDecimaisPerc, TpcnResources.pMVAST, ObOp.Obrigatorio);
                        }
                        else
                        {
                            wCampo(imposto.ICMS.pMVAST, nDecimaisPerc, TpcnResources.pMVAST, ObOp.Opcional);
                        }

                        wCampo(imposto.ICMS.pRedBCST, nDecimaisPerc, TpcnResources.pRedBCST, ObOp.Opcional);
                        wCampo(imposto.ICMS.vBCST, TpcnTipoCampo.tcDec2, TpcnResources.vBCST);
                        wCampo(imposto.ICMS.pICMSST, nDecimaisPerc, TpcnResources.pICMSST);
                        wCampo(imposto.ICMS.vICMSST, TpcnTipoCampo.tcDec2, TpcnResources.vICMSST);
                        if(nfe.infNFe.Versao >= 4
                             && imposto.ICMS.vBCFCPST + imposto.ICMS.pFCPST + imposto.ICMS.vFCPST > 0)
                        {
                            wCampo(imposto.ICMS.vBCFCPST, TpcnTipoCampo.tcDec2, TpcnResources.vBCFCPST);//, ObOp.Opcional);
                            wCampo(imposto.ICMS.pFCPST, nDecimaisPerc, TpcnResources.pFCPST);//, ObOp.Opcional);
                            wCampo(imposto.ICMS.vFCPST, TpcnTipoCampo.tcDec2, TpcnResources.vFCPST);//, ObOp.Opcional);
                        }
                        wCampo(imposto.ICMS.pCredSN, nDecimaisPerc, TpcnResources.pCredSN);
                        wCampo(imposto.ICMS.vCredICMSSN, TpcnTipoCampo.tcDec2, TpcnResources.vCredICMSSN);
                        break;

                    case 202:
                    case 203:
                        wCampo(imposto.ICMS.modBCST, TpcnTipoCampo.tcInt, TpcnResources.modBCST);

                        if(imposto.ICMS.modBCST == TpcnDeterminacaoBaseIcmsST.dbisMargemValorAgregado)
                        {
                            wCampo(imposto.ICMS.pMVAST, nDecimaisPerc, TpcnResources.pMVAST, ObOp.Obrigatorio);
                        }
                        else
                        {
                            wCampo(imposto.ICMS.pMVAST, nDecimaisPerc, TpcnResources.pMVAST, ObOp.Opcional);
                        }

                        wCampo(imposto.ICMS.pRedBCST, nDecimaisPerc, TpcnResources.pRedBCST, ObOp.Opcional);
                        wCampo(imposto.ICMS.vBCST, TpcnTipoCampo.tcDec2, TpcnResources.vBCST);
                        wCampo(imposto.ICMS.pICMSST, nDecimaisPerc, TpcnResources.pICMSST);
                        wCampo(imposto.ICMS.vICMSST, TpcnTipoCampo.tcDec2, TpcnResources.vICMSST);
                        if(nfe.infNFe.Versao >= 4 && imposto.ICMS.vBCFCPST + imposto.ICMS.pFCPST + imposto.ICMS.vFCPST > 0)
                        {
                            wCampo(imposto.ICMS.vBCFCPST, TpcnTipoCampo.tcDec2, TpcnResources.vBCFCPST);//, ObOp.Opcional);
                            wCampo(imposto.ICMS.pFCPST, nDecimaisPerc, TpcnResources.pFCPST);//, ObOp.Opcional);
                            wCampo(imposto.ICMS.vFCPST, TpcnTipoCampo.tcDec2, TpcnResources.vFCPST);//, ObOp.Opcional);
                        }
                        break;

                    case 500:
                        if(imposto.ICMS.vBCSTRet + imposto.ICMS.pST + imposto.ICMS.vICMSSubstituto + imposto.ICMS.vICMSSTRet > 0 || nfe.ide.indFinal == TpcnConsumidorFinal.cfNao)
                        {
                            wCampo(imposto.ICMS.vBCSTRet, TpcnTipoCampo.tcDec2, TpcnResources.vBCSTRet);
                            wCampo(imposto.ICMS.pST, TpcnTipoCampo.tcDec4, TpcnResources.pST);
                            wCampo(imposto.ICMS.vICMSSubstituto, TpcnTipoCampo.tcDec2, TpcnResources.vICMSSubstituto);
                            wCampo(imposto.ICMS.vICMSSTRet, TpcnTipoCampo.tcDec2, TpcnResources.vICMSSTRet);
                        }

                        if(imposto.ICMS.vBCFCPSTRet + imposto.ICMS.pFCPSTRet + imposto.ICMS.vFCPSTRet > 0)
                        {
                            wCampo(imposto.ICMS.vBCFCPSTRet, TpcnTipoCampo.tcDec2, TpcnResources.vBCFCPSTRet);
                            wCampo(imposto.ICMS.pFCPSTRet, nDecimaisPerc, TpcnResources.pFCPSTRet);
                            wCampo(imposto.ICMS.vFCPSTRet, TpcnTipoCampo.tcDec2, TpcnResources.vFCPSTRet);
                        }

                        if(imposto.ICMS.pRedBCEfet + imposto.ICMS.vBCEfet + imposto.ICMS.pICMSEfet + imposto.ICMS.vICMSEfet > 0)
                        {
                            wCampo(imposto.ICMS.pRedBCEfet, nDecimaisPerc, TpcnResources.pRedBCEfet);
                            wCampo(imposto.ICMS.vBCEfet, TpcnTipoCampo.tcDec2, TpcnResources.vBCEfet);
                            wCampo(imposto.ICMS.pICMSEfet, nDecimaisPerc, TpcnResources.pICMSEfet);
                            wCampo(imposto.ICMS.vICMSEfet, TpcnTipoCampo.tcDec2, TpcnResources.vICMSEfet);
                        }
                        break;

                    case 900:
                        wCampo(imposto.ICMS.modBC, TpcnTipoCampo.tcInt, TpcnResources.modBC);
                        wCampo(imposto.ICMS.vBC, TpcnTipoCampo.tcDec2, TpcnResources.vBC);
                        wCampo(imposto.ICMS.pRedBC, nDecimaisPerc, TpcnResources.pRedBC, ObOp.Opcional);
                        wCampo(imposto.ICMS.pICMS, nDecimaisPerc, TpcnResources.pICMS);
                        wCampo(imposto.ICMS.vICMS, TpcnTipoCampo.tcDec2, TpcnResources.vICMS);

                        if(imposto.ICMS.modBCST != TpcnDeterminacaoBaseIcmsST.NaoInserirTagNoXML)
                        {
                            wCampo(imposto.ICMS.modBCST, TpcnTipoCampo.tcInt, TpcnResources.modBCST);
                        }

                        if(imposto.ICMS.modBCST == TpcnDeterminacaoBaseIcmsST.dbisMargemValorAgregado)
                        {
                            wCampo(imposto.ICMS.pMVAST, nDecimaisPerc, TpcnResources.pMVAST, ObOp.Obrigatorio);
                        }
                        else
                        {
                            wCampo(imposto.ICMS.pMVAST, nDecimaisPerc, TpcnResources.pMVAST, ObOp.Opcional);
                        }

                        wCampo(imposto.ICMS.pRedBCST, nDecimaisPerc, TpcnResources.pRedBCST, ObOp.Opcional);
                        wCampo(imposto.ICMS.vBCST, TpcnTipoCampo.tcDec2, TpcnResources.vBCST);
                        wCampo(imposto.ICMS.pICMSST, nDecimaisPerc, TpcnResources.pICMSST);
                        wCampo(imposto.ICMS.vICMSST, TpcnTipoCampo.tcDec2, TpcnResources.vICMSST);
                        if(nfe.infNFe.Versao >= 4
                             && imposto.ICMS.vBCFCPST + imposto.ICMS.pFCPST + imposto.ICMS.vFCPST > 0)
                        {
                            wCampo(imposto.ICMS.vBCFCPST, TpcnTipoCampo.tcDec2, TpcnResources.vBCFCPST);//, ObOp.Opcional);
                            wCampo(imposto.ICMS.pFCPST, nDecimaisPerc, TpcnResources.pFCPST);//, ObOp.Opcional);
                            wCampo(imposto.ICMS.vFCPST, TpcnTipoCampo.tcDec2, TpcnResources.vFCPST);//, ObOp.Opcional);
                        }
                        wCampo(imposto.ICMS.pCredSN, nDecimaisPerc, TpcnResources.pCredSN);
                        wCampo(imposto.ICMS.vCredICMSSN, TpcnTipoCampo.tcDec2, TpcnResources.vCredICMSSN);
                        break;
                }
            }
        }

        /// <summary>
        /// GerarDetImpostoICMSUFDest
        /// </summary>
        /// <param name="nfe"></param>
        /// <param name="imposto"></param>
        /// <param name="nodeImposto"></param>
        private void GerarDetImpostoICMSUFDest(NFe nfe, Imposto imposto, XmlElement nodeImposto)
        {
            if(/*(imposto.ICMS.ICMSPart10 == 1 || imposto.ICMS.ICMSPart90 == 1 || imposto.ICMS.ICMSst == 1) &&*/ (double)nfe.infNFe.Versao >= 3.10)
            {
                if(imposto.ICMS.ICMSUFDest.vBCUFDest > 0 ||
                    imposto.ICMS.ICMSUFDest.pFCPUFDest > 0 ||
                    imposto.ICMS.ICMSUFDest.vFCPUFDest > 0 ||
                    imposto.ICMS.ICMSUFDest.pICMSUFDest > 0 ||
                    imposto.ICMS.ICMSUFDest.pICMSInter > 0 ||
                    imposto.ICMS.ICMSUFDest.pICMSInterPart > 0 ||
                    imposto.ICMS.ICMSUFDest.vICMSUFDest > 0 ||
                    imposto.ICMS.ICMSUFDest.vICMSUFRemet > 0 ||
                    imposto.ICMS.ICMSUFDest.vBCFCPUFDest > 0)
                {
                    XmlNode x_nodeCurrent = nodeCurrent;
                    XmlElement ee0 = doc.CreateElement(TpcnResources.ICMSUFDest.ToString());
                    nodeImposto.AppendChild(ee0);
                    nodeCurrent = ee0;

                    wCampo(imposto.ICMS.ICMSUFDest.vBCUFDest, TpcnTipoCampo.tcDec2, TpcnResources.vBCUFDest);
                    if(nfe.infNFe.Versao >= 4)
                    {
                        wCampo(imposto.ICMS.ICMSUFDest.vBCFCPUFDest, TpcnTipoCampo.tcDec2, TpcnResources.vBCFCPUFDest);
                    }

                    wCampo(imposto.ICMS.ICMSUFDest.pFCPUFDest, TpcnTipoCampo.tcDec4, TpcnResources.pFCPUFDest);
                    wCampo(imposto.ICMS.ICMSUFDest.pICMSUFDest, TpcnTipoCampo.tcDec4, TpcnResources.pICMSUFDest);
                    wCampo(imposto.ICMS.ICMSUFDest.pICMSInter, TpcnTipoCampo.tcDec2, TpcnResources.pICMSInter);
                    wCampo(imposto.ICMS.ICMSUFDest.pICMSInterPart, TpcnTipoCampo.tcDec4, TpcnResources.pICMSInterPart);
                    wCampo(imposto.ICMS.ICMSUFDest.vFCPUFDest, TpcnTipoCampo.tcDec2, TpcnResources.vFCPUFDest);
                    wCampo(imposto.ICMS.ICMSUFDest.vICMSUFDest, TpcnTipoCampo.tcDec2, TpcnResources.vICMSUFDest);
                    wCampo(imposto.ICMS.ICMSUFDest.vICMSUFRemet, TpcnTipoCampo.tcDec2, TpcnResources.vICMSUFRemet);

                    nodeCurrent = x_nodeCurrent;
                }
            }
        }

        /// <summary>
        /// GerarDetImpostoII
        /// </summary>
        /// <param name="II"></param>
        /// <param name="nodeImposto"></param>
        private void GerarDetImpostoII(II II, XmlElement nodeImposto)
        {
            //if (II.vII + II.vDespAdu + II.vIOF > 0) //danasa 3/11/2011
            {
                nodeCurrent = doc.CreateElement("II");
                nodeImposto.AppendChild(nodeCurrent);

                wCampo(II.vBC, TpcnTipoCampo.tcDec2, TpcnResources.vBC);
                wCampo(II.vDespAdu, TpcnTipoCampo.tcDec2, TpcnResources.vDespAdu);
                wCampo(II.vII, TpcnTipoCampo.tcDec2, TpcnResources.vII);
                wCampo(II.vIOF, TpcnTipoCampo.tcDec2, TpcnResources.vIOF);
            }
        }

        /// <summary>
        /// GerarDetImpostoIPI
        /// </summary>
        /// <param name="IPI"></param>
        /// <param name="nodeImposto"></param>
        private bool GerarDetImpostoIPI(NFe nfe, IPI IPI, XmlElement nodeImposto)
        {
            if(!string.IsNullOrEmpty(IPI.CST))
            {
                bool CST00495099;

                // variavel CST00495099 usada para Ignorar Tag <IPI>
                // se GerarTagIPIparaNaoTributado = False e CST00495099 = False

                CST00495099 = (IPI.CST == "00" || IPI.CST == "49" || IPI.CST == "50" || IPI.CST == "99");

                XmlElement e0 = doc.CreateElement("IPI");
                nodeImposto.AppendChild(e0);
                nodeCurrent = e0;

                if(nfe.infNFe.Versao < 4)
                {
                    wCampo(IPI.clEnq, TpcnTipoCampo.tcStr, TpcnResources.clEnq, ObOp.Opcional);
                }

                wCampo(IPI.CNPJProd, TpcnTipoCampo.tcStr, TpcnResources.CNPJProd, ObOp.Opcional);
                wCampo(IPI.cSelo, TpcnTipoCampo.tcStr, TpcnResources.cSelo, ObOp.Opcional);
                wCampo(IPI.qSelo, TpcnTipoCampo.tcInt, TpcnResources.qSelo, ObOp.Opcional);
                if(string.IsNullOrEmpty(IPI.cEnq))
                {
                    IPI.cEnq = "999";
                }

                wCampo(IPI.cEnq, TpcnTipoCampo.tcStr, TpcnResources.cEnq);

                if(CST00495099)
                {
                    if((IPI.vBC + IPI.pIPI > 0) && (IPI.qUnid + IPI.vUnid > 0))
                    {
                        cMensagemErro += "IPITrib: As TAG's <vBC> e <pIPI> não podem ser informadas em conjunto com as TAG <qUnid> e <vUnid>" + Environment.NewLine;
                    }

                    nodeCurrent = doc.CreateElement("IPITrib");
                    e0.AppendChild(nodeCurrent);

                    wCampo(IPI.CST, TpcnTipoCampo.tcStr, TpcnResources.CST);
                    if(IPI.qUnid + IPI.vUnid > 0)
                    {
                        wCampo(IPI.qUnid, TpcnTipoCampo.tcDec4, TpcnResources.qUnid);
                        wCampo(IPI.vUnid, TpcnTipoCampo.tcDec4, TpcnResources.vUnid);
                    }
                    else
                    {
                        wCampo(IPI.vBC, TpcnTipoCampo.tcDec2, TpcnResources.vBC);
                        wCampo(IPI.pIPI, nDecimaisPerc, TpcnResources.pIPI);
                    }
                    wCampo(IPI.vIPI, TpcnTipoCampo.tcDec2, TpcnResources.vIPI);
                }
                else //(* Quando CST/IPI for 01,02,03,04,51,52,53,54 ou 55 *)
                {
                    nodeCurrent = doc.CreateElement("IPINT");
                    e0.AppendChild(nodeCurrent);
                    wCampo(IPI.CST, TpcnTipoCampo.tcStr, TpcnResources.CST);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// GerarDetImpostoPIS
        /// </summary>
        /// <param name="PIS"></param>
        /// <param name="nodeImposto"></param>
        private void GerarDetImpostoPIS(NFe nfe, PIS PIS, XmlElement nodeImposto)
        {
            if(!string.IsNullOrEmpty(PIS.CST))
            {
                XmlElement e0 = doc.CreateElement("PIS");

                switch(PIS.CST)
                {
                    case "01":
                    case "02":
                        nodeCurrent = doc.CreateElement("PISAliq");
                        e0.AppendChild(nodeCurrent);
                        nodeImposto.AppendChild(e0);
                        wCampo(PIS.CST, TpcnTipoCampo.tcStr, TpcnResources.CST);
                        wCampo(PIS.vBC, TpcnTipoCampo.tcDec2, TpcnResources.vBC);
                        wCampo(PIS.pPIS, nDecimaisPerc, TpcnResources.pPIS);
                        wCampo(PIS.vPIS, TpcnTipoCampo.tcDec2, TpcnResources.vPIS);
                        break;

                    case "03":
                        nodeCurrent = doc.CreateElement("PISQtde");
                        e0.AppendChild(nodeCurrent);
                        nodeImposto.AppendChild(e0);
                        wCampo(PIS.CST, TpcnTipoCampo.tcStr, TpcnResources.CST);
                        wCampo(PIS.qBCProd, TpcnTipoCampo.tcDec4, TpcnResources.qBCProd);
                        wCampo(PIS.vAliqProd, TpcnTipoCampo.tcDec4, TpcnResources.vAliqProd);
                        wCampo(PIS.vPIS, TpcnTipoCampo.tcDec2, TpcnResources.vPIS);
                        break;

                    case "04":
                    case "05":
                    case "06":
                    case "07":
                    case "08":
                    case "09":
                        nodeCurrent = doc.CreateElement("PISNT");
                        e0.AppendChild(nodeCurrent);
                        nodeImposto.AppendChild(e0);
                        wCampo(PIS.CST, TpcnTipoCampo.tcStr, TpcnResources.CST);
                        break;

                    case "49":
                    case "50":
                    case "51":
                    case "52":
                    case "53":
                    case "54":
                    case "55":
                    case "56":
                    case "60":
                    case "61":
                    case "62":
                    case "63":
                    case "64":
                    case "65":
                    case "66":
                    case "67":
                    case "70":
                    case "71":
                    case "72":
                    case "73":
                    case "74":
                    case "75":
                    case "98":
                    case "99":
                        if((PIS.vBC + PIS.pPIS > 0) && (PIS.qBCProd + PIS.vAliqProd > 0))
                        {
                            cMensagemErro += "PIS: As TAG's <vBC> e <pPIS> não podem ser informadas em conjunto com as TAG <qBCProd> e <vAliqProd>" + Environment.NewLine;
                        }

                        nodeCurrent = doc.CreateElement(TpcnResources.PISOutr.ToString());
                        e0.AppendChild(nodeCurrent);
                        nodeImposto.AppendChild(e0);

                        wCampo(PIS.CST, TpcnTipoCampo.tcStr, TpcnResources.CST);
                        if(PIS.qBCProd + PIS.vAliqProd > 0)
                        {
                            wCampo(PIS.qBCProd, TpcnTipoCampo.tcDec4, TpcnResources.qBCProd);
                            wCampo(PIS.vAliqProd, TpcnTipoCampo.tcDec4, TpcnResources.vAliqProd);
                        }
                        else
                        {
                            wCampo(PIS.vBC, TpcnTipoCampo.tcDec2, TpcnResources.vBC);
                            wCampo(PIS.pPIS, nDecimaisPerc, TpcnResources.pPIS);
                        }
                        wCampo(PIS.vPIS, TpcnTipoCampo.tcDec2, TpcnResources.vPIS);
                        break;
                }
            }
        }

        /// <summary>
        /// GerarDetImpostoPISST
        /// </summary>
        /// <param name="PISST"></param>
        /// <param name="nodeImposto"></param>
        private void GerarDetImpostoPISST(NFe nfe, PISST PISST, XmlElement nodeImposto)
        {
            if((PISST.vBC > 0) ||
              (PISST.pPis > 0) ||
              (PISST.qBCProd > 0) ||
              (PISST.vAliqProd > 0) ||
              (PISST.vPIS > 0))
            {
                if((PISST.vBC + PISST.pPis > 0) && (PISST.qBCProd + PISST.vAliqProd > 0))
                {
                    cMensagemErro += "PISST: As TAG's <vBC> e <pPIS> não podem ser informadas em conjunto com as TAG <qBCProd> e <vAliqProd>)" + Environment.NewLine;
                }

                if(PISST.vBC + PISST.pPis > 0)
                {
                    nodeCurrent = doc.CreateElement("PISST");
                    nodeImposto.AppendChild(nodeCurrent);

                    wCampo(PISST.vBC, TpcnTipoCampo.tcDec2, TpcnResources.vBC);
                    wCampo(PISST.pPis, nDecimaisPerc, TpcnResources.pPIS);
                    wCampo(PISST.vPIS, TpcnTipoCampo.tcDec2, TpcnResources.vPIS);
                }

                if(PISST.qBCProd + PISST.vAliqProd > 0)
                {
                    nodeCurrent = doc.CreateElement("PISST");
                    nodeImposto.AppendChild(nodeCurrent);
                    wCampo(PISST.qBCProd, TpcnTipoCampo.tcDec4, TpcnResources.qBCProd);
                    wCampo(PISST.vAliqProd, TpcnTipoCampo.tcDec4, TpcnResources.vAliqProd);
                    wCampo(PISST.vPIS, TpcnTipoCampo.tcDec2, TpcnResources.vPIS);
                    wCampo(PISST.indSomaPISST, TpcnTipoCampo.tcStr, TpcnResources.indSomaPISST, ObOp.Opcional, 0);
                }
            }
        }

        /// <summary>
        /// GerarRespTecnico
        /// </summary>
        /// <param name="responsavel"></param>
        /// <param name="root"></param>
        private void GerarRespTecnico(RespTecnico responsavel, XmlElement root)
        {
            if(string.IsNullOrEmpty(responsavel.CNPJ))
            {
                return;
            }

            XmlElement rootRespTec = doc.CreateElement("infRespTec");
            root.AppendChild(rootRespTec);
            nodeCurrent = rootRespTec;

            wCampo(responsavel.CNPJ, TpcnTipoCampo.tcStr, TpcnResources.CNPJ, ObOp.Obrigatorio);
            wCampo(responsavel.xContato, TpcnTipoCampo.tcStr, nameof(responsavel.xContato), ObOp.Opcional, 0);
            wCampo(responsavel.email, TpcnTipoCampo.tcStr, TpcnResources.email, ObOp.Opcional);
            wCampo(responsavel.fone, TpcnTipoCampo.tcStr, TpcnResources.fone, ObOp.Opcional);
            wCampo(responsavel.idCSRT, TpcnTipoCampo.tcInt, nameof(responsavel.idCSRT), ObOp.Opcional, 2);
            wCampo(responsavel.hashCSRT, TpcnTipoCampo.tcStr, nameof(responsavel.hashCSRT), ObOp.Opcional, 0);
        }

        /// <summary>
        /// GerarDigito
        /// </summary>
        /// <param name="chave"></param>
        /// <returns></returns>
        public int GerarDigito(string chave)
        {
            int i, j, Digito;
            const string PESO = "4329876543298765432987654329876543298765432";

            chave = chave.Replace("NFe", "");
            if(chave.Length != 43)
            {
                cMensagemErro += string.Format("Erro na composição da chave [{0}] para obter o DV", chave) + Environment.NewLine;
                return 0;
            }
            else
            {
                // Manual Integracao Contribuinte v2.02a - Página: 70 //
                j = 0;
                Digito = -1;
                try
                {
                    for(i = 0; i < 43; ++i)
                    {
                        j += Convert.ToInt32(chave.Substring(i, 1)) * Convert.ToInt32(PESO.Substring(i, 1));
                    }

                    Digito = 11 - (j % 11);
                    if((j % 11) < 2)
                    {
                        Digito = 0;
                    }
                }
                catch
                {
                    Digito = -1;
                }
                if(Digito == -1)
                {
                    cMensagemErro += string.Format("Erro no cálculo do DV da chave [{0}]", chave) + Environment.NewLine;
                }

                return Digito;
            }
        }

        /// <summary>
        /// GerarEmit
        /// </summary>
        /// <param name="NFe"></param>
        /// <returns></returns>
        private XmlElement GerarEmit(NFe NFe)
        {
            XmlElement ELemit = doc.CreateElement("emit");
            nodeCurrent = ELemit;

            if(string.IsNullOrEmpty(NFe.emit.CNPJ) && string.IsNullOrEmpty(NFe.emit.CPF))
            {
                throw new Exception("CNPJ/CPF inválido no segmento [C]");
            }

            if(!string.IsNullOrEmpty(NFe.emit.CNPJ))
            {
                wCampo(NFe.emit.CNPJ, TpcnTipoCampo.tcStr, TpcnResources.CNPJ);
            }
            else
            {
                wCampo(NFe.emit.CPF, TpcnTipoCampo.tcStr, TpcnResources.CPF);
            }

            wCampo(NFe.emit.xNome, TpcnTipoCampo.tcStr, TpcnResources.xNome);
            wCampo(NFe.emit.xFant, TpcnTipoCampo.tcStr, TpcnResources.xFant, ObOp.Opcional);
            ///
            /// <enderEmit>
            ///
            XmlElement el = doc.CreateElement("enderEmit");
            nodeCurrent.AppendChild(el);
            nodeCurrent = el;
            wCampo(NFe.emit.enderEmit.xLgr, TpcnTipoCampo.tcStr, TpcnResources.xLgr);
            wCampo(NFe.emit.enderEmit.nro, TpcnTipoCampo.tcStr, TpcnResources.nro);
            wCampo(NFe.emit.enderEmit.xCpl, TpcnTipoCampo.tcStr, TpcnResources.xCpl, ObOp.Opcional);
            wCampo(NFe.emit.enderEmit.xBairro, TpcnTipoCampo.tcStr, TpcnResources.xBairro);
            wCampo(NFe.emit.enderEmit.cMun, TpcnTipoCampo.tcInt, TpcnResources.cMun, ObOp.Obrigatorio, 7);
            wCampo(NFe.emit.enderEmit.xMun, TpcnTipoCampo.tcStr, TpcnResources.xMun);
            wCampo(NFe.emit.enderEmit.UF, TpcnTipoCampo.tcStr, TpcnResources.UF);
            wCampo(NFe.emit.enderEmit.CEP, TpcnTipoCampo.tcInt, TpcnResources.CEP, ObOp.Opcional, 8);
            wCampo(NFe.emit.enderEmit.cPais, TpcnTipoCampo.tcInt, TpcnResources.cPais, ObOp.Opcional);
            wCampo(NFe.emit.enderEmit.xPais, TpcnTipoCampo.tcStr, TpcnResources.xPais, ObOp.Opcional);
            wCampo(NFe.emit.enderEmit.fone, TpcnTipoCampo.tcStr, TpcnResources.fone, ObOp.Opcional);
            ///
            /// </enderEmit>
            ///
            nodeCurrent = ELemit;
            wCampo(NFe.emit.IE, TpcnTipoCampo.tcStr, TpcnResources.IE);
            wCampo(NFe.emit.IEST, TpcnTipoCampo.tcStr, TpcnResources.IEST, ObOp.Opcional);
            wCampo(NFe.emit.IM, TpcnTipoCampo.tcStr, TpcnResources.IM, ObOp.Opcional);
            if(NFe.emit.IM.Length > 0)
            {
                wCampo(NFe.emit.CNAE, TpcnTipoCampo.tcStr, TpcnResources.CNAE, ObOp.Opcional);
            }

            wCampo(NFe.emit.CRT, TpcnTipoCampo.tcInt, TpcnResources.CRT);

            return ELemit;
        }

        /// <summary>
        /// GerarEntrega
        /// </summary>
        /// <param name="NFe"></param>
        /// <param name="root"></param>
        private void GerarEntrega(NFe NFe, XmlElement root)
        {
            if(!string.IsNullOrEmpty(NFe.entrega.xLgr))
            {
                XmlElement e0 = doc.CreateElement("entrega");
                root.AppendChild(e0);
                nodeCurrent = e0;

                if(string.IsNullOrEmpty(NFe.entrega.CNPJ) && string.IsNullOrEmpty(NFe.entrega.CPF))
                {
                    throw new Exception("CNPJ/CPF inválido no segmento [F]");
                }

                if(!string.IsNullOrEmpty(NFe.entrega.CNPJ))
                {
                    wCampo(NFe.entrega.CNPJ, TpcnTipoCampo.tcStr, TpcnResources.CNPJ);
                }
                else
                {
                    wCampo(NFe.entrega.CPF, TpcnTipoCampo.tcStr, TpcnResources.CPF);
                }

                wCampo(NFe.entrega.xNome, TpcnTipoCampo.tcStr, TpcnResources.xNome, ObOp.Opcional);
                wCampo(NFe.entrega.xLgr, TpcnTipoCampo.tcStr, TpcnResources.xLgr);
                wCampo(NFe.entrega.nro, TpcnTipoCampo.tcStr, TpcnResources.nro);
                wCampo(NFe.entrega.xCpl, TpcnTipoCampo.tcStr, TpcnResources.xCpl, ObOp.Opcional);
                wCampo(NFe.entrega.xBairro, TpcnTipoCampo.tcStr, TpcnResources.xBairro);
                wCampo(NFe.entrega.cMun, TpcnTipoCampo.tcInt, TpcnResources.cMun, ObOp.Obrigatorio, 7);
                wCampo(NFe.entrega.xMun, TpcnTipoCampo.tcStr, TpcnResources.xMun);
                wCampo(NFe.entrega.UF, TpcnTipoCampo.tcStr, TpcnResources.UF);

                wCampo(NFe.entrega.CEP, TpcnTipoCampo.tcStr, TpcnResources.CEP, ObOp.Opcional);
                wCampo(NFe.entrega.cPais, TpcnTipoCampo.tcInt, TpcnResources.cPais, ObOp.Opcional);
                wCampo(NFe.entrega.xPais, TpcnTipoCampo.tcStr, TpcnResources.xPais, ObOp.Opcional);
                wCampo(NFe.entrega.fone, TpcnTipoCampo.tcStr, TpcnResources.fone, ObOp.Opcional);
                wCampo(NFe.entrega.email, TpcnTipoCampo.tcStr, TpcnResources.email, ObOp.Opcional);
                wCampo(NFe.entrega.IE, TpcnTipoCampo.tcStr, TpcnResources.IE, ObOp.Opcional);
            }
        }

        /// <summary>
        /// GerarautoXML
        /// </summary>
        /// <param name="NFe"></param>
        /// <param name="root"></param>
        private void GerarautXML(NFe NFe, XmlElement root)
        {
            for(int i = 0; i < NFe.autXML.Count; ++i)
            {
                nodeCurrent = doc.CreateElement("autXML");
                root.AppendChild(nodeCurrent);

                if(!string.IsNullOrEmpty(NFe.autXML[i].CNPJ))
                {
                    wCampo(NFe.autXML[i].CNPJ, TpcnTipoCampo.tcStr, TpcnResources.CNPJ);
                }
                else
                {
                    wCampo(NFe.autXML[i].CPF, TpcnTipoCampo.tcStr, TpcnResources.CPF);
                }
            }
        }

        /// <summary>
        /// GerarDetDevol
        /// </summary>
        /// <param name="impostoDevol"></param>
        /// <returns></returns>
        private void GerarDetDevol(impostoDevol impostoDevol, XmlElement root)
        {
            if(impostoDevol.pDevol > 0 || impostoDevol.vIPIDevol > 0)
            {
                XmlElement e0 = doc.CreateElement("impostoDevol");
                root.AppendChild(e0);
                nodeCurrent = e0;

                wCampo(impostoDevol.pDevol, TpcnTipoCampo.tcDec2, TpcnResources.pDevol);

                XmlElement e0ipi = doc.CreateElement("IPI");
                e0.AppendChild(e0ipi);
                nodeCurrent = e0ipi;
                wCampo(impostoDevol.vIPIDevol, TpcnTipoCampo.tcDec2, TpcnResources.vIPIDevol);
            }
        }

        /// <summary>
        /// GerarExporta
        /// </summary>
        /// <param name="exporta"></param>
        /// <param name="root"></param>
        private void GerarExporta(NFe nfe, Exporta exporta, XmlElement root)
        {
            if((double)nfe.infNFe.Versao >= 3.10)
            {
                if(!string.IsNullOrEmpty(exporta.UFSaidaPais) || !string.IsNullOrEmpty(exporta.xLocExporta))
                {
                    nodeCurrent = doc.CreateElement("exporta");
                    root.AppendChild(nodeCurrent);

                    wCampo(exporta.UFSaidaPais, TpcnTipoCampo.tcStr, TpcnResources.UFSaidaPais);
                    wCampo(exporta.xLocExporta, TpcnTipoCampo.tcStr, TpcnResources.xLocExporta);
                    wCampo(exporta.xLocDespacho, TpcnTipoCampo.tcStr, TpcnResources.xLocDespacho, ObOp.Opcional);
                }
            }
            else
                if(!string.IsNullOrEmpty(exporta.UFEmbarq) || !string.IsNullOrEmpty(exporta.xLocEmbarq))
            {
                nodeCurrent = doc.CreateElement("exporta");
                root.AppendChild(nodeCurrent);

                wCampo(exporta.UFEmbarq, TpcnTipoCampo.tcStr, TpcnResources.UFEmbarq);
                wCampo(exporta.xLocEmbarq, TpcnTipoCampo.tcStr, TpcnResources.xLocEmbarq);
            }
        }

        /// <summary>
        /// GerarInfAdic
        /// </summary>
        /// <param name="InfAdic"></param>
        /// <param name="root"></param>
        private void GerarInfAdic(InfAdic InfAdic, XmlElement root)
        {
            if((!string.IsNullOrEmpty(InfAdic.infAdFisco)) ||
                (!string.IsNullOrEmpty(InfAdic.infCpl)) ||
                (InfAdic.obsCont.Count > 0) ||
                (InfAdic.obsFisco.Count > 0) ||
                (InfAdic.procRef.Count > 0))
            {
                XmlElement nodeinfAdic = doc.CreateElement("infAdic");
                root.AppendChild(nodeinfAdic);
                nodeCurrent = nodeinfAdic;

                wCampo(InfAdic.infAdFisco, TpcnTipoCampo.tcStr, TpcnResources.infAdFisco, ObOp.Opcional);
                wCampo(InfAdic.infCpl, TpcnTipoCampo.tcStr, TpcnResources.infCpl, ObOp.Opcional);
                //
                //(**)GerarInfAdicObsCont;
                //
                if(InfAdic.obsCont.Count > 10)
                {
                    cMensagemErro += "obsCont: Excedeu o máximo permitido de 10" + Environment.NewLine;
                }

                foreach(obsCont obsCont in InfAdic.obsCont)
                {
                    XmlElement nodeobsCont = doc.CreateElement("obsCont");
                    XmlAttribute xmlItem = doc.CreateAttribute(TpcnResources.xCampo.ToString());
                    xmlItem.Value = obsCont.xCampo;
                    nodeobsCont.Attributes.Append(xmlItem);
                    nodeinfAdic.AppendChild(nodeobsCont);
                    nodeCurrent = nodeobsCont;
                    wCampo(obsCont.xTexto, TpcnTipoCampo.tcStr, TpcnResources.xTexto);
                }
                //
                //(**)GerarInfAdicObsFisco;
                //
                if(InfAdic.obsFisco.Count > 10)
                {
                    cMensagemErro += "obsFisco: Excedeu o máximo permitido de 10" + Environment.NewLine;
                }

                foreach(obsFisco obsFisco in InfAdic.obsFisco)
                {
                    XmlElement nodeobsFisco = doc.CreateElement("obsFisco");
                    XmlAttribute xmlItem = doc.CreateAttribute(TpcnResources.xCampo.ToString());
                    xmlItem.Value = obsFisco.xCampo;
                    nodeobsFisco.Attributes.Append(xmlItem);
                    nodeinfAdic.AppendChild(nodeobsFisco);
                    nodeCurrent = nodeobsFisco;
                    wCampo(obsFisco.xTexto, TpcnTipoCampo.tcStr, TpcnResources.xTexto);
                }
                //
                //(**)GerarInfAdicProcRef;
                //
                foreach(procRef procRef in InfAdic.procRef)
                {
                    XmlElement nodeprocRef = doc.CreateElement("procRef");
                    nodeinfAdic.AppendChild(nodeprocRef);
                    nodeCurrent = nodeprocRef;

                    wCampo(procRef.nProc, TpcnTipoCampo.tcStr, TpcnResources.nProc);
                    wCampo(procRef.indProc, TpcnTipoCampo.tcStr, TpcnResources.indProc);
                }
            }
        }

        /// <summary>
        /// GerarInfNFe
        /// </summary>
        /// <param name="Nfe"></param>
        /// <returns></returns>
        private XmlElement GerarIde(NFe Nfe)
        {
            XmlElement ELide = doc.CreateElement("ide");

            nodeCurrent = ELide;
            wCampo(Nfe.ide.cUF, TpcnTipoCampo.tcInt, TpcnResources.cUF, ObOp.Obrigatorio, 0);
            wCampo(Nfe.ide.cNF, TpcnTipoCampo.tcInt, TpcnResources.cNF, ObOp.Obrigatorio, 8);
            wCampo(Nfe.ide.natOp, TpcnTipoCampo.tcStr, TpcnResources.natOp, ObOp.Obrigatorio, 0);
            wCampo((int)Nfe.ide.mod, TpcnTipoCampo.tcInt, TpcnResources.mod, ObOp.Obrigatorio, 0);
            wCampo(Nfe.ide.serie, TpcnTipoCampo.tcInt, TpcnResources.serie, ObOp.Obrigatorio, 0);
            wCampo(Nfe.ide.nNF, TpcnTipoCampo.tcInt, TpcnResources.nNF, ObOp.Obrigatorio, 0);
            if(Nfe.infNFe.Versao >= 3)
            {
                wCampo(Nfe.ide.dhEmi, TpcnTipoCampo.tcStr, TpcnResources.dhEmi, ObOp.Obrigatorio);
                if(Nfe.ide.mod == TpcnMod.modNFe)
                {
                    wCampo(Nfe.ide.dhSaiEnt, TpcnTipoCampo.tcStr, TpcnResources.dhSaiEnt, ObOp.Opcional);
                }
            }
            else
            {
                wCampo(Nfe.ide.dEmi, TpcnTipoCampo.tcDatYYYY_MM_DD, TpcnResources.dEmi, ObOp.Obrigatorio, 0);
                wCampo(Nfe.ide.dSaiEnt, TpcnTipoCampo.tcDatYYYY_MM_DD, TpcnResources.dSaiEnt, ObOp.Opcional, 0);
                wCampo(Nfe.ide.hSaiEnt, TpcnTipoCampo.tcHor, TpcnResources.hSaiEnt, ObOp.Opcional, 0);
            }
            wCampo((int)Nfe.ide.tpNF, TpcnTipoCampo.tcInt, TpcnResources.tpNF, ObOp.Obrigatorio, 0);
            if(Nfe.infNFe.Versao >= 3)
            {
                wCampo((Nfe.ide.mod == TpcnMod.modNFe ? (int)Nfe.ide.idDest : (int)TpcnDestinoOperacao.doInterna), TpcnTipoCampo.tcInt, TpcnResources.idDest, ObOp.Obrigatorio); //B11a
            }

            wCampo(Nfe.ide.cMunFG, TpcnTipoCampo.tcInt, TpcnResources.cMunFG, ObOp.Obrigatorio, 0);

            if(Nfe.infNFe.Versao < 3)
            {
                GerarIdeNFref(Nfe, ELide);
            }

            nodeCurrent = ELide;
            wCampo((int)Nfe.ide.tpImp, TpcnTipoCampo.tcInt, TpcnResources.tpImp, ObOp.Obrigatorio);
            wCampo((int)Nfe.ide.tpEmis, TpcnTipoCampo.tcInt, TpcnResources.tpEmis, ObOp.Obrigatorio);
            wCampo(Nfe.ide.cDV, TpcnTipoCampo.tcInt, TpcnResources.cDV, ObOp.Obrigatorio);
            wCampo((int)Nfe.ide.tpAmb, TpcnTipoCampo.tcInt, TpcnResources.tpAmb, ObOp.Obrigatorio);
            wCampo((int)Nfe.ide.finNFe, TpcnTipoCampo.tcInt, TpcnResources.finNFe, ObOp.Obrigatorio);
            wCampo((int)Nfe.ide.indFinal, TpcnTipoCampo.tcInt, TpcnResources.indFinal, ObOp.Obrigatorio);//B25a
            wCampo((int)Nfe.ide.indPres, TpcnTipoCampo.tcInt, TpcnResources.indPres, ObOp.Obrigatorio);//B25b

            if(Nfe.ide.indPres == TpcnPresencaComprador.pcPresencial && Nfe.ide.indIntermed != TpcnIntermediario.NaoInserirTagNoXML) //1 Teve uma mudança recente na NT que mudou o critério, 1 também terá que incluir a tag indIntermed, mas de inicio não obrigatório.
            {
                wCampo((int)Nfe.ide.indIntermed, TpcnTipoCampo.tcInt, TpcnResources.indIntermed, ObOp.Obrigatorio);
            }
            else if(Nfe.ide.indPres == TpcnPresencaComprador.pcInternet || //2
               Nfe.ide.indPres == TpcnPresencaComprador.pcTeleatendimento || //3
               Nfe.ide.indPres == TpcnPresencaComprador.pcEntregaDomicilio || //4
               Nfe.ide.indPres == TpcnPresencaComprador.pcOutros) //9
            {
                if(Nfe.ide.indIntermed != TpcnIntermediario.NaoInserirTagNoXML)
                {
                    wCampo((int)Nfe.ide.indIntermed, TpcnTipoCampo.tcInt, TpcnResources.indIntermed, ObOp.Obrigatorio);
                }
            }

            wCampo(Nfe.ide.procEmi, TpcnTipoCampo.tcInt, TpcnResources.procEmi, ObOp.Obrigatorio);
            wCampo(Nfe.ide.verProc, TpcnTipoCampo.tcStr, TpcnResources.verProc, ObOp.Obrigatorio);
            wCampo(Nfe.ide.dhCont, TpcnTipoCampo.tcStr, TpcnResources.dhCont, ObOp.Opcional);
            wCampo(Nfe.ide.xJust, TpcnTipoCampo.tcStr, TpcnResources.xJust, ObOp.Opcional);

            if(Nfe.infNFe.Versao >= 3)
            {
                GerarIdeNFref(Nfe, ELide);
            }

            return ELide;
        }

        /// <summary>
        /// GerarIdeNFref
        /// </summary>
        /// <param name="Nfe"></param>
        /// <param name="ELide"></param>
        private void GerarIdeNFref(NFe Nfe, XmlElement ELide)
        {
            // Gera TAGs referentes a NFe referência
            foreach(NFref refNFe in Nfe.ide.NFref)
            {
                if(!string.IsNullOrEmpty(refNFe.refNFe))
                {
                    XmlElement ep = doc.CreateElement(TpcnResources.NFref.ToString());
                    XmlElement el = doc.CreateElement(TpcnResources.refNFe.ToString());
                    el.InnerText = refNFe.refNFe;
                    ep.AppendChild(el);
                    ELide.AppendChild(ep);
                }
            }
            // Gera TAGs se NÃO for uma NFe referência
            foreach(NFref refNFe in Nfe.ide.NFref)
            {
                if(refNFe.refNF != null)
                {
                    if(refNFe.refNF.nNF > 0)
                    {
                        XmlElement ep = doc.CreateElement(TpcnResources.NFref.ToString());
                        XmlElement el = doc.CreateElement("refNF");
                        ep.AppendChild(el);
                        ELide.AppendChild(ep);
                        nodeCurrent = el;

                        wCampo(refNFe.refNF.cUF, TpcnTipoCampo.tcInt, TpcnResources.cUF, ObOp.Obrigatorio, 2);
                        wCampo(refNFe.refNF.AAMM, TpcnTipoCampo.tcStr, TpcnResources.AAMM, ObOp.Obrigatorio, 0);
                        wCampo(refNFe.refNF.CNPJ, TpcnTipoCampo.tcStr, TpcnResources.CNPJ, ObOp.Obrigatorio, 0);
                        wCampo(refNFe.refNF.mod, TpcnTipoCampo.tcStr, TpcnResources.mod, ObOp.Obrigatorio, 2);
                        wCampo(refNFe.refNF.serie, TpcnTipoCampo.tcInt, TpcnResources.serie, ObOp.Obrigatorio, 0);
                        wCampo(refNFe.refNF.nNF, TpcnTipoCampo.tcInt, TpcnResources.nNF, ObOp.Obrigatorio, 0);
                    }
                }
            }
            foreach(NFref refNFe in Nfe.ide.NFref)
            {
                if(refNFe.refNFP != null)
                {
                    if(refNFe.refNFP.nNF > 0)
                    {
                        XmlElement ep = doc.CreateElement(TpcnResources.NFref.ToString());
                        XmlElement el = doc.CreateElement("refNFP");
                        ep.AppendChild(el);
                        ELide.AppendChild(ep);
                        nodeCurrent = el;

                        wCampo(refNFe.refNFP.cUF, TpcnTipoCampo.tcInt, TpcnResources.cUF, ObOp.Obrigatorio, 2);
                        wCampo(refNFe.refNFP.AAMM, TpcnTipoCampo.tcStr, TpcnResources.AAMM, ObOp.Obrigatorio, 0);
                        if(!string.IsNullOrEmpty(refNFe.refNFP.CNPJ))
                        {
                            wCampo(refNFe.refNFP.CNPJ, TpcnTipoCampo.tcStr, TpcnResources.CNPJ, ObOp.Obrigatorio, 0);
                        }
                        else
                            if(!string.IsNullOrEmpty(refNFe.refNFP.CPF))
                        {
                            wCampo(refNFe.refNFP.CPF, TpcnTipoCampo.tcStr, TpcnResources.CPF, ObOp.Obrigatorio, 0);
                        }

                        wCampo(refNFe.refNFP.IE, TpcnTipoCampo.tcStr, TpcnResources.IE, ObOp.Opcional, 0);
                        wCampo(refNFe.refNFP.mod, TpcnTipoCampo.tcStr, TpcnResources.mod, ObOp.Obrigatorio, 2);
                        wCampo(refNFe.refNFP.serie, TpcnTipoCampo.tcInt, TpcnResources.serie, ObOp.Obrigatorio, 0);
                        wCampo(refNFe.refNFP.nNF, TpcnTipoCampo.tcInt, TpcnResources.nNF, ObOp.Obrigatorio, 0);
                    }
                }
            }
            foreach(NFref refNFe in Nfe.ide.NFref)
            {
                if(!string.IsNullOrEmpty(refNFe.refCTe))
                {
                    if(refNFe.refCTe.Substring(0, 2) != "00")
                    {
                        XmlElement ep = doc.CreateElement(TpcnResources.NFref.ToString());
                        XmlElement el = doc.CreateElement(TpcnResources.refCTe.ToString());
                        el.InnerText = refNFe.refCTe;
                        ep.AppendChild(el);
                        ELide.AppendChild(ep);
                    }
                }
            }
            foreach(NFref refNFe in Nfe.ide.NFref)
            {
                if(refNFe.refECF != null)
                {
                    if(refNFe.refECF.nCOO > 0)
                    {
                        XmlElement ep = doc.CreateElement(TpcnResources.NFref.ToString());
                        XmlElement el = doc.CreateElement("refECF");
                        ep.AppendChild(el);
                        ELide.AppendChild(ep);
                        nodeCurrent = el;

                        wCampo(refNFe.refECF.mod, TpcnTipoCampo.tcStr, TpcnResources.mod, ObOp.Obrigatorio);
                        wCampo(refNFe.refECF.nECF, TpcnTipoCampo.tcInt, TpcnResources.nECF, ObOp.Obrigatorio, 3);
                        wCampo(refNFe.refECF.nCOO, TpcnTipoCampo.tcInt, TpcnResources.nCOO, ObOp.Obrigatorio);
                    }
                }
            }
        }

        /// <summary>
        /// GerarRetirada
        /// </summary>
        /// <param name="NFe"></param>
        /// <param name="root"></param>
        private void GerarRetirada(NFe NFe, XmlElement root)
        {
            if(!string.IsNullOrEmpty(NFe.retirada.xLgr))
            {
                XmlElement el = doc.CreateElement("retirada");
                root.AppendChild(el);
                nodeCurrent = el;

                if(!string.IsNullOrEmpty(NFe.retirada.CNPJ))
                {
                    wCampo(NFe.retirada.CNPJ, TpcnTipoCampo.tcStr, TpcnResources.CNPJ);
                }
                else
                {
                    wCampo(NFe.retirada.CPF, TpcnTipoCampo.tcStr, TpcnResources.CPF);
                }

                wCampo(NFe.retirada.xNome, TpcnTipoCampo.tcStr, TpcnResources.xNome, ObOp.Opcional);
                wCampo(NFe.retirada.xLgr, TpcnTipoCampo.tcStr, TpcnResources.xLgr);
                wCampo(NFe.retirada.nro, TpcnTipoCampo.tcStr, TpcnResources.nro);
                wCampo(NFe.retirada.xCpl, TpcnTipoCampo.tcStr, TpcnResources.xCpl, ObOp.Opcional);
                wCampo(NFe.retirada.xBairro, TpcnTipoCampo.tcStr, TpcnResources.xBairro);
                wCampo(NFe.retirada.cMun, TpcnTipoCampo.tcInt, TpcnResources.cMun, ObOp.Obrigatorio, 7);
                wCampo(NFe.retirada.xMun, TpcnTipoCampo.tcStr, TpcnResources.xMun);
                wCampo(NFe.retirada.UF, TpcnTipoCampo.tcStr, TpcnResources.UF);

                wCampo(NFe.retirada.CEP, TpcnTipoCampo.tcStr, TpcnResources.CEP, ObOp.Opcional);
                wCampo(NFe.retirada.cPais, TpcnTipoCampo.tcInt, TpcnResources.cPais, ObOp.Opcional);
                wCampo(NFe.retirada.xPais, TpcnTipoCampo.tcStr, TpcnResources.xPais, ObOp.Opcional);
                wCampo(NFe.retirada.fone, TpcnTipoCampo.tcStr, TpcnResources.fone, ObOp.Opcional);
                wCampo(NFe.retirada.email, TpcnTipoCampo.tcStr, TpcnResources.email, ObOp.Opcional);
                wCampo(NFe.retirada.IE, TpcnTipoCampo.tcStr, TpcnResources.IE, ObOp.Opcional);
            }
        }

        /// <summary>
        /// GerarTransp
        /// </summary>
        /// <param name="Transp"></param>
        /// <param name="root"></param>
        private void GerarTransp(NFe NFe, XmlElement root)
        {
            Transp Transp = NFe.Transp;

            XmlElement nodeTransp = doc.CreateElement("transp");
            root.AppendChild(nodeTransp);
            nodeCurrent = nodeTransp;

            wCampo(Transp.modFrete, TpcnTipoCampo.tcInt, TpcnResources.modFrete);
            //
            //  (**)GerarTranspTransporta;
            //
            if(!string.IsNullOrEmpty(Transp.Transporta.CNPJ) ||
                !string.IsNullOrEmpty(Transp.Transporta.CPF) ||
                !string.IsNullOrEmpty(Transp.Transporta.xNome) ||
                !string.IsNullOrEmpty(Transp.Transporta.IE) ||
                !string.IsNullOrEmpty(Transp.Transporta.xEnder) ||
                !string.IsNullOrEmpty(Transp.Transporta.xMun) ||
                !string.IsNullOrEmpty(Transp.Transporta.UF))
            {
                XmlElement nodeTransporta = doc.CreateElement("transporta");
                nodeTransp.AppendChild(nodeTransporta);
                nodeCurrent = nodeTransporta;

                if(!string.IsNullOrEmpty(Transp.Transporta.CNPJ))
                {
                    wCampo(Transp.Transporta.CNPJ, TpcnTipoCampo.tcStr, TpcnResources.CNPJ);
                }
                else
                    if(!string.IsNullOrEmpty(Transp.Transporta.CPF))
                {
                    wCampo(Transp.Transporta.CPF, TpcnTipoCampo.tcStr, TpcnResources.CPF);
                }

                wCampo(Transp.Transporta.xNome, TpcnTipoCampo.tcStr, TpcnResources.xNome, ObOp.Opcional);
                wCampo(Transp.Transporta.IE, TpcnTipoCampo.tcStr, TpcnResources.IE, ObOp.Opcional);
                wCampo(Transp.Transporta.xEnder, TpcnTipoCampo.tcStr, TpcnResources.xEnder, ObOp.Opcional);
                wCampo(Transp.Transporta.xMun, TpcnTipoCampo.tcStr, TpcnResources.xMun, ObOp.Opcional);
                wCampo(Transp.Transporta.UF, TpcnTipoCampo.tcStr, TpcnResources.UF, ObOp.Opcional);
            }
            //
            //  (**)GerarTranspRetTransp;
            //
            if((Transp.retTransp.vServ > 0) ||
              (Transp.retTransp.vBCRet > 0) ||
              (Transp.retTransp.pICMSRet > 0) ||
              (Transp.retTransp.vICMSRet > 0) ||
              (!string.IsNullOrEmpty(Transp.retTransp.CFOP)) ||
              (Transp.retTransp.cMunFG > 0))
            {
                XmlElement nodeRetTransporta = doc.CreateElement("retTransp");
                nodeTransp.AppendChild(nodeRetTransporta);
                nodeCurrent = nodeRetTransporta;

                wCampo(Transp.retTransp.vServ, TpcnTipoCampo.tcDec2, TpcnResources.vServ);
                wCampo(Transp.retTransp.vBCRet, TpcnTipoCampo.tcDec2, TpcnResources.vBCRet);
                wCampo(Transp.retTransp.pICMSRet, TpcnTipoCampo.tcDec2, TpcnResources.pICMSRet);
                wCampo(Transp.retTransp.vICMSRet, TpcnTipoCampo.tcDec2, TpcnResources.vICMSRet);
                wCampo(Transp.retTransp.CFOP, TpcnTipoCampo.tcStr, TpcnResources.CFOP);
                wCampo(Transp.retTransp.cMunFG, TpcnTipoCampo.tcStr, TpcnResources.cMunFG);
            }
            //
            //  (**)GerarTranspVeicTransp;
            //
            if(!string.IsNullOrEmpty(Transp.veicTransp.placa) ||
              !string.IsNullOrEmpty(Transp.veicTransp.UF) ||
              !string.IsNullOrEmpty(Transp.veicTransp.RNTC))
            {
                XmlElement nodeveicTransp = doc.CreateElement("veicTransp");
                nodeTransp.AppendChild(nodeveicTransp);
                nodeCurrent = nodeveicTransp;

                wCampo(Transp.veicTransp.placa, TpcnTipoCampo.tcStr, TpcnResources.placa);
                wCampo(Transp.veicTransp.UF, TpcnTipoCampo.tcStr, TpcnResources.UF);
                wCampo(Transp.veicTransp.RNTC, TpcnTipoCampo.tcStr, TpcnResources.RNTC, ObOp.Opcional);
            }
            //
            //(**)GerarTranspReboque;
            //
            int mr = ((double)NFe.infNFe.Versao <= 3.10) ? 2 : 5;
            if(Transp.Reboque.Count > mr)
            {
                cMensagemErro += $"Transp.reboque: Excedeu o máximo permitido de {mr}" + Environment.NewLine;
            }

            foreach(Reboque Reboque in Transp.Reboque)
            {
                XmlElement nodeReboque = doc.CreateElement("reboque");
                nodeTransp.AppendChild(nodeReboque);
                nodeCurrent = nodeReboque;

                wCampo(Reboque.placa, TpcnTipoCampo.tcStr, TpcnResources.placa);
                wCampo(Reboque.UF, TpcnTipoCampo.tcStr, TpcnResources.UF);
                wCampo(Reboque.RNTC, TpcnTipoCampo.tcStr, TpcnResources.RNTC, ObOp.Opcional);
            }

            wCampo(Transp.vagao, TpcnTipoCampo.tcStr, TpcnResources.vagao, ObOp.Opcional);
            wCampo(Transp.balsa, TpcnTipoCampo.tcStr, TpcnResources.balsa, ObOp.Opcional);

            //
            //(**)GerarTranspVol;
            //
            foreach(Vol Vol in Transp.Vol)
            {
                XmlElement nodeVol = doc.CreateElement("vol");
                nodeTransp.AppendChild(nodeVol);
                nodeCurrent = nodeVol;

                wCampo(Vol.qVol, TpcnTipoCampo.tcInt, TpcnResources.qVol, ObOp.Opcional);
                wCampo(Vol.esp, TpcnTipoCampo.tcStr, TpcnResources.esp, ObOp.Opcional);
                wCampo(Vol.marca, TpcnTipoCampo.tcStr, TpcnResources.marca, ObOp.Opcional);
                wCampo(Vol.nVol, TpcnTipoCampo.tcStr, TpcnResources.nVol, ObOp.Opcional);
                wCampo(Vol.pesoL, TpcnTipoCampo.tcDec3, TpcnResources.pesoL, ObOp.Opcional);
                wCampo(Vol.pesoB, TpcnTipoCampo.tcDec3, TpcnResources.pesoB, ObOp.Opcional);
                //(**)GerarTranspVolLacres(i);
                foreach(Lacres lacres in Vol.Lacres)
                {
                    XmlElement nodeVolLacres = doc.CreateElement("lacres");
                    nodeVol.AppendChild(nodeVolLacres);
                    nodeCurrent = nodeVolLacres;

                    wCampo(lacres.nLacre, TpcnTipoCampo.tcStr, TpcnResources.nLacre);
                }
            }
        }

        /// <summary>
        /// GerarTotal
        /// </summary>
        /// <param name="NFe"></param>
        /// <param name="root"></param>
        private void GerarTotal(NFe NFe, XmlElement root)
        {
            XmlElement nodeTotal = doc.CreateElement("total");
            root.AppendChild(nodeTotal);

            #region --ICMSTot

            nodeCurrent = doc.CreateElement("ICMSTot");
            nodeTotal.AppendChild(nodeCurrent);

            wCampo(NFe.Total.ICMSTot.vBC, TpcnTipoCampo.tcDec2, TpcnResources.vBC);
            wCampo(NFe.Total.ICMSTot.vICMS, TpcnTipoCampo.tcDec2, TpcnResources.vICMS);
            if((double)NFe.infNFe.Versao >= 3.10)
            {
                wCampo(NFe.Total.ICMSTot.vICMSDeson, TpcnTipoCampo.tcDec2, TpcnResources.vICMSDeson);

                if(NFe.Total.ICMSTot.vFCPUFDest > 0 || NFe.Total.ICMSTot.vICMSUFDest > 0 || NFe.Total.ICMSTot.vICMSUFRemet > 0)
                {
                    wCampo(NFe.Total.ICMSTot.vFCPUFDest, TpcnTipoCampo.tcDec2, TpcnResources.vFCPUFDest);
                    wCampo(NFe.Total.ICMSTot.vICMSUFDest, TpcnTipoCampo.tcDec2, TpcnResources.vICMSUFDest, ObOp.Opcional);
                    wCampo(NFe.Total.ICMSTot.vICMSUFRemet, TpcnTipoCampo.tcDec2, TpcnResources.vICMSUFRemet, ObOp.Opcional);
                }
                if(NFe.infNFe.Versao >= 4)
                {
                    wCampo(NFe.Total.ICMSTot.vFCP, TpcnTipoCampo.tcDec2, TpcnResources.vFCP);
                }
            }
            wCampo(NFe.Total.ICMSTot.vBCST, TpcnTipoCampo.tcDec2, TpcnResources.vBCST);
            wCampo(NFe.Total.ICMSTot.vST, TpcnTipoCampo.tcDec2, TpcnResources.vST);
            if(NFe.infNFe.Versao >= 4)
            {
                wCampo(NFe.Total.ICMSTot.vFCPST, TpcnTipoCampo.tcDec2, TpcnResources.vFCPST);
                wCampo(NFe.Total.ICMSTot.vFCPSTRet, TpcnTipoCampo.tcDec2, TpcnResources.vFCPSTRet);
            }
            wCampo(NFe.Total.ICMSTot.vProd, TpcnTipoCampo.tcDec2, TpcnResources.vProd);
            wCampo(NFe.Total.ICMSTot.vFrete, TpcnTipoCampo.tcDec2, TpcnResources.vFrete);
            wCampo(NFe.Total.ICMSTot.vSeg, TpcnTipoCampo.tcDec2, TpcnResources.vSeg);
            wCampo(NFe.Total.ICMSTot.vDesc, TpcnTipoCampo.tcDec2, TpcnResources.vDesc);
            wCampo(NFe.Total.ICMSTot.vII, TpcnTipoCampo.tcDec2, TpcnResources.vII);
            wCampo(NFe.Total.ICMSTot.vIPI, TpcnTipoCampo.tcDec2, TpcnResources.vIPI);
            if(NFe.infNFe.Versao >= 4)
            {
                wCampo(NFe.Total.ICMSTot.vIPIDevol, TpcnTipoCampo.tcDec2, TpcnResources.vIPIDevol);
            }

            wCampo(NFe.Total.ICMSTot.vPIS, TpcnTipoCampo.tcDec2, TpcnResources.vPIS);
            wCampo(NFe.Total.ICMSTot.vCOFINS, TpcnTipoCampo.tcDec2, TpcnResources.vCOFINS);
            wCampo(NFe.Total.ICMSTot.vOutro, TpcnTipoCampo.tcDec2, TpcnResources.vOutro);
            wCampo(NFe.Total.ICMSTot.vNF, TpcnTipoCampo.tcDec2, TpcnResources.vNF);
            wCampo(NFe.Total.ICMSTot.vTotTrib, TpcnTipoCampo.tcDec2, TpcnResources.vTotTrib, ObOp.Opcional);

            #endregion --ICMSTot

            #region --ISSQNtot

            if((NFe.Total.ISSQNtot.vServ > 0) ||
                (NFe.Total.ISSQNtot.vBC > 0) ||
                (NFe.Total.ISSQNtot.vISS > 0) ||
                (NFe.Total.ISSQNtot.vPIS > 0) ||
                (NFe.Total.ISSQNtot.vCOFINS > 0))
            {
                nodeCurrent = doc.CreateElement("ISSQNtot");
                nodeTotal.AppendChild(nodeCurrent);

                wCampo(NFe.Total.ISSQNtot.vServ, TpcnTipoCampo.tcDec2, TpcnResources.vServ, ObOp.Opcional);
                wCampo(NFe.Total.ISSQNtot.vBC, TpcnTipoCampo.tcDec2, TpcnResources.vBC, ObOp.Opcional);
                wCampo(NFe.Total.ISSQNtot.vISS, TpcnTipoCampo.tcDec2, TpcnResources.vISS, ObOp.Opcional);
                wCampo(NFe.Total.ISSQNtot.vPIS, TpcnTipoCampo.tcDec2, TpcnResources.vPIS, ObOp.Opcional);
                wCampo(NFe.Total.ISSQNtot.vCOFINS, TpcnTipoCampo.tcDec2, TpcnResources.vCOFINS, ObOp.Opcional);

                if((double)NFe.infNFe.Versao >= 3.10)
                {
                    wCampo(NFe.Total.ISSQNtot.dCompet, TpcnTipoCampo.tcDatYYYY_MM_DD, TpcnResources.dCompet, ObOp.Opcional);
                    wCampo(NFe.Total.ISSQNtot.vDeducao, TpcnTipoCampo.tcDec2, TpcnResources.vDeducao, ObOp.Opcional);
                    //wCampo(NFe.Total.ISSQNtot.vINSS, TpcnTipoCampo.tcDec2, TpcnResources.vINSS, ObOp.Opcional);
                    //wCampo(NFe.Total.ISSQNtot.vIR, TpcnTipoCampo.tcDec2, TpcnResources.vIR, ObOp.Opcional);
                    //wCampo(NFe.Total.ISSQNtot.vCSLL, TpcnTipoCampo.tcDec2, TpcnResources.vCSLL, ObOp.Opcional);
                    wCampo(NFe.Total.ISSQNtot.vOutro, TpcnTipoCampo.tcDec2, TpcnResources.vOutro, ObOp.Opcional);
                    wCampo(NFe.Total.ISSQNtot.vDescIncond, TpcnTipoCampo.tcDec2, TpcnResources.vDescIncond, ObOp.Opcional);
                    wCampo(NFe.Total.ISSQNtot.vDescCond, TpcnTipoCampo.tcDec2, TpcnResources.vDescCond, ObOp.Opcional);
                    wCampo(NFe.Total.ISSQNtot.vISSRet, TpcnTipoCampo.tcDec2, TpcnResources.vISSRet, ObOp.Opcional);
                    //wCampo(NFe.Total.ISSQNtot.indISSRet ? "1":"2", TpcnTipoCampo.tcStr, TpcnResources.indISSRet);
                    //wCampo((int)NFe.Total.ISSQNtot.indISS, TpcnTipoCampo.tcInt, TpcnResources.indISS);
                    //wCampo(NFe.Total.ISSQNtot.cServico, TpcnTipoCampo.tcStr, TpcnResources.cServico, ObOp.Opcional);
                    //wCampo(NFe.Total.ISSQNtot.cMun, TpcnTipoCampo.tcInt, TpcnResources.cMun, ObOp.Opcional);
                    //wCampo(NFe.Total.ISSQNtot.cPais, TpcnTipoCampo.tcInt, TpcnResources.cPais, ObOp.Opcional);
                    //wCampo(NFe.Total.ISSQNtot.nProcesso, TpcnTipoCampo.tcStr, TpcnResources.nProcesso, ObOp.Opcional);
                    //wCampo(NFe.Total.ISSQNtot.vISSRet, TpcnTipoCampo.tcDec2, TpcnResources.vISSRet, ObOp.Opcional);
                    wCampo((int)NFe.Total.ISSQNtot.cRegTrib, TpcnTipoCampo.tcInt, TpcnResources.cRegTrib, ObOp.Opcional);
                    //wCampo(NFe.Total.ISSQNtot.indIncentivo ? 1:2 , TpcnTipoCampo.tcInt, TpcnResources.indIncentivo);
                }
            }

            #endregion --ISSQNtot

            #region --retTrib

            if((NFe.Total.retTrib.vRetPIS > 0) ||
                (NFe.Total.retTrib.vRetCOFINS > 0) ||
                (NFe.Total.retTrib.vRetCSLL > 0) ||
                (NFe.Total.retTrib.vBCIRRF > 0) ||
                (NFe.Total.retTrib.vIRRF > 0) ||
                (NFe.Total.retTrib.vBCRetPrev > 0) ||
                (NFe.Total.retTrib.vRetPrev > 0))
            {
                nodeCurrent = doc.CreateElement("retTrib");
                nodeTotal.AppendChild(nodeCurrent);

                wCampo(NFe.Total.retTrib.vRetPIS, TpcnTipoCampo.tcDec2, TpcnResources.vRetPIS, ObOp.Opcional);
                wCampo(NFe.Total.retTrib.vRetCOFINS, TpcnTipoCampo.tcDec2, TpcnResources.vRetCOFINS, ObOp.Opcional);
                wCampo(NFe.Total.retTrib.vRetCSLL, TpcnTipoCampo.tcDec2, TpcnResources.vRetCSLL, ObOp.Opcional);
                wCampo(NFe.Total.retTrib.vBCIRRF, TpcnTipoCampo.tcDec2, TpcnResources.vBCIRRF, ObOp.Opcional);
                wCampo(NFe.Total.retTrib.vIRRF, TpcnTipoCampo.tcDec2, TpcnResources.vIRRF, ObOp.Opcional);
                wCampo(NFe.Total.retTrib.vBCRetPrev, TpcnTipoCampo.tcDec2, TpcnResources.vBCRetPrev, ObOp.Opcional);
                wCampo(NFe.Total.retTrib.vRetPrev, TpcnTipoCampo.tcDec2, TpcnResources.vRetPrev, ObOp.Opcional);
            }

            #endregion --retTrib
        }

        private void wCampo(object obj, TpcnTipoCampo Tipo, string TAG)
        {
            wCampo(obj, Tipo, TAG, ObOp.Obrigatorio, 0);
        }

        private void wCampo(object obj, TpcnTipoCampo Tipo, string TAG, ObOp Obrigatorio)
        {
            wCampo(obj, Tipo, TAG, Obrigatorio, 0);
        }

        /// <summary>
        /// wCampo
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="Tipo"></param>
        /// <param name="TAG"></param>
        private void wCampo(object obj, TpcnTipoCampo Tipo, TpcnResources TAG)
        {
            wCampo(obj, Tipo, TAG.ToString(), ObOp.Obrigatorio, 0);
        }

        /// <summary>
        /// wCampo
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="Tipo"></param>
        /// <param name="TAG"></param>
        /// <param name="Obrigatorio"></param>
        private void wCampo(object obj, TpcnTipoCampo Tipo, TpcnResources TAG, ObOp Obrigatorio)
        {
            wCampo(obj, Tipo, TAG.ToString(), Obrigatorio, 0);
        }

        /// <summary>
        /// wCampo
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="Tipo"></param>
        /// <param name="TAG"></param>
        /// <param name="Obrigatorio"></param>
        /// <param name="nAlign"></param>
        private void wCampo(object obj, TpcnTipoCampo Tipo, TpcnResources TAG, ObOp Obrigatorio, int nAlign)
        {
            wCampo(obj, Tipo, TAG.ToString(), Obrigatorio, nAlign);
        }

        private void wCampo(object obj, TpcnTipoCampo Tipo, string TAG, ObOp Obrigatorio, int nAlign)
        {
            TAG = TAG.Trim();

            if(obj == null)
            {
                return;
            }

            switch(Tipo)
            {
                case TpcnTipoCampo.tcDec2:
                case TpcnTipoCampo.tcDec4:
                    if((double)obj == -9.99)
                    {
                        return;
                    }

                    break;
            }

            if(Tipo == TpcnTipoCampo.tcDatYYYY_MM_DD || Tipo == TpcnTipoCampo.tcDatHor)
            {
                if(((DateTime)obj).Year == 1)
                {
                    if(Obrigatorio == ObOp.Opcional)
                    {
                        return;
                    }
                }
            }

            if(Obrigatorio == ObOp.Opcional)
            {
                if(Tipo == TpcnTipoCampo.tcInt)
                {
                    if((int)obj == 0)
                    {
                        return;
                    }
                }

                if(Tipo >= TpcnTipoCampo.tcDec2 && Tipo <= TpcnTipoCampo.tcDec10)
                {
                    if((double)obj == 0)
                    {
                        return;
                    }
                }

                if(Tipo == TpcnTipoCampo.tcHor)
                {
                    if((DateTime)obj == DateTime.MinValue)
                    {
                        return;
                    }
                }

                if(obj.ToString().Trim() == "")
                {
                    return;
                }
            }
            XmlElement valueEl1 = doc.CreateElement(TAG);

            switch(Tipo)
            {
                case TpcnTipoCampo.tcDec2:
                case TpcnTipoCampo.tcDec3:
                case TpcnTipoCampo.tcDec4:
                case TpcnTipoCampo.tcDec5:
                case TpcnTipoCampo.tcDec6:
                case TpcnTipoCampo.tcDec7:
                case TpcnTipoCampo.tcDec8:
                case TpcnTipoCampo.tcDec9:
                case TpcnTipoCampo.tcDec10:
                    if(((double)obj) > 0 || Obrigatorio == ObOp.Obrigatorio || Obrigatorio == ObOp.None)
                    {
                        valueEl1.InnerText = ((double)obj).ToString("0." + ("").PadLeft((int)Tipo, '0')).Replace(",", ".");
                    }
                    break;

                case TpcnTipoCampo.tcDatHor:
                    if(((DateTime)obj).Year > 1)
                    {
                        valueEl1.InnerText = ((DateTime)obj).ToString("yyyy-MM-ddTHH:mm:ss");
                    }

                    break;

                case TpcnTipoCampo.tcHor:
                    if(Obrigatorio == ObOp.Opcional && ((DateTime)obj) == DateTime.MinValue)
                    {
                        return;
                    }

                    valueEl1.InnerText = ((DateTime)obj).ToString("HH:mm:ss");
                    break;

                case TpcnTipoCampo.tcDatYYYY_MM_DD:
                    if(((DateTime)obj).Year > 1)
                    {
                        valueEl1.InnerText = ((DateTime)obj).ToString("yyyy-MM-dd");
                    }

                    break;

                case TpcnTipoCampo.tcDatYYYYMMDD:
                    if(((DateTime)obj).Year > 1)
                    {
                        valueEl1.InnerText = ((DateTime)obj).ToString("yyyyMMdd");
                    }

                    break;

                default:
                    if(nAlign > 0)
                    {
                        valueEl1.InnerText = obj.ToString().PadLeft(nAlign, '0');
                    }
                    else if(Tipo == TpcnTipoCampo.tcInt)
                    {
                        if(((int)obj) != 0 || Obrigatorio == ObOp.Obrigatorio)
                        {
                            valueEl1.InnerText = ((int)obj).ToString();
                        }
                    }
                    else if(obj.ToString().Trim() != "")
                    {
                        if(TAG == TpcnResources.xProd.ToString() && !convertToOem)
                        {
                            valueEl1.InnerText = obj.ToString().TrimStart().TrimEnd();
                        }
                        else
                        {
                            valueEl1.InnerText = ConvertToOEM(obj.ToString().TrimStart().TrimEnd());
                        }
                    }
                    break;
            }
            nodeCurrent.AppendChild(valueEl1);
        }

        #region ConverToOEM

        /// <summary>
        /// ConvertToOEM
        /// </summary>
        /// <param name="FBuffer"></param>
        /// <returns></returns>
        public string ConvertToOEM(string FBuffer)
        {
            if(string.IsNullOrEmpty(FBuffer))
            {
                return "";
            }

            if(FBuffer.StartsWith("<![CDATA["))
            {
                return FBuffer;
            }

            FBuffer = FBuffer.Replace("&", "&amp;");
            FBuffer = FBuffer.Replace("<", "&lt;");
            FBuffer = FBuffer.Replace(">", "&gt;");
            FBuffer = FBuffer.Replace("\"", "&quot;");
            FBuffer = FBuffer.Replace("\t", " ");
            FBuffer = FBuffer.Replace("\n", ";");
            FBuffer = FBuffer.Replace("\r", "");
            FBuffer = FBuffer.Replace("'", "&#39;");
            FBuffer = FBuffer.Replace("&amp; lt;", "&lt;");
            FBuffer = FBuffer.Replace("&amp; gt;", "&gt;");
            FBuffer = FBuffer.Replace("&amp; amp;", "&amp;");
            FBuffer = FBuffer.Replace("&amp;amp;", "&amp;");
            FBuffer = FBuffer.Replace("&amp;lt;", "&lt;");
            FBuffer = FBuffer.Replace("&amp;gt;", "&gt;");

            string normalizedString = FBuffer.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            for(int i = 0; i < normalizedString.Length; i++)
            {
                char c = normalizedString[i];
                if(CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    switch(Convert.ToInt16(normalizedString[i]))
                    {
                        case 94:    ///^
                        case 96:    ///`
                        case 126:   ///~
                        case 8216:  ///‘
                        case 8217:  ///’
                        case 161:   ///¡
                        case 162:   ///¢
                        case 163:   ///£
                        case 164:   ///¤
                        case 165:   ///¥
                        case 166:   ///¦
                        case 167:   ///§
                        case 168:   ///¨
                        case 171:   ///«
                        case 172:   ///¬
                        case 169:   ///©
                        case 174:   ///®
                        case 175:   ///¯
                        case 177:   ///±
                        case 180:   ///´
                        case 181:   ///µ
                        case 182:   ///¶
                        case 183:   ///·
                        case 187:   ///»
                        case 188:   ///¼
                        case 189:   ///½
                        case 190:   ///¾
                        case 191:   ///¿
                        case 198:   ///Æ
                        case 208:   ///Ð
                        case 216:   ///Ø
                        case 222:   ///Þ
                        case 223:   ///ß
                        case 230:   ///æ
                        case 240:   ///ð
                        case 247:   ///÷
                        case 248:   ///ø
                        case 254:   ///þ
                        case 184: c = ' '; break; ///¸
                        case 170: c = 'a'; break; ///ª
                        case 176: c = 'o'; break; ///°
                        case 178: c = '2'; break; ///²
                        case 179: c = '3'; break; ///³
                        case 185: c = '1'; break; ///¹
                        case 186: c = 'o'; break; ///º
                        case 732: c = 'Ø'; break; ///˜
                    }
                    stringBuilder.Append(c);
                }
            }
            return stringBuilder.ToString();
        }

        #endregion ConverToOEM

        #region GerarChaveNFe

        /// <summary>
        /// MontaChave
        /// Cria a chave de acesso da NFe
        /// </summary>
        /// <param name="ArqXMLPedido"></param>
        public void GerarChaveNFe(string ArqPedido, bool xml)
        {
            int emp = Empresas.FindEmpresaByThread();

            // XML - pedido
            // Filename: XXXXXXXX-gerar-chave.xml
            // -------------------------------------------------------------------
            // <?xml version="1.0" encoding="UTF-8"?>
            // <gerarChave>
            //      <UF>35</UF>                 //se não informado, assume a da configuração
            //      <tpEmis>1</tpEmis>          //se não informado, assume a da configuração. Wandrey 22/03/2010
            //      <nNF>1000</nNF>
            //      <cNF>0</cNF>                //se não informado, eu gero
            //      <serie>1</serie>
            //      <AAMM>0912</AAMM>
            //      <CNPJ>55801377000131</CNPJ>
            // </gerarChave>
            //
            // XML - resposta
            // Filename: XXXXXXXX-ret-gerar-chave.xml
            // -------------------------------------------------------------------
            // <?xml version="1.0" encoding="UTF-8"?>
            // <retGerarChave>
            //      <chaveNFe>350912</chaveNFe>
            // </retGerarChave>
            //

            // TXT - pedido
            // Filename: XXXXXXXX-gerar-chave.txt
            // -------------------------------------------------------------------
            // UF|35
            // tpEmis|1
            // nNF|1000
            // cNF|0
            // serie|1
            // AAMM|0912
            // CNPJ|55801377000131
            //
            // TXT - resposta
            // Filename: XXXXXXXX-ret-gerar-chave.txt
            // -------------------------------------------------------------------
            // 35091255801377000131550010000000010000176506
            //
            // -------------------------------------------------------------------
            // ERR - resposta
            // Filename: XXXXXXXX-gerar-chave.err

            string ArqXMLRetorno = Empresas.Configuracoes[emp].PastaXmlRetorno + "\\" +
                    (xml ? Functions.ExtrairNomeArq(ArqPedido, Propriedade.Extensao(Propriedade.TipoEnvio.GerarChaveNFe).EnvioXML) + Propriedade.Extensao(Propriedade.TipoEnvio.GerarChaveNFe).RetornoXML :
                           Functions.ExtrairNomeArq(ArqPedido, Propriedade.Extensao(Propriedade.TipoEnvio.GerarChaveNFe).EnvioTXT) + Propriedade.Extensao(Propriedade.TipoEnvio.GerarChaveNFe).RetornoTXT);
            //string ArqERRRetorno = Empresas.Configuracoes[emp].PastaXmlRetorno + "\\" + Path.ChangeExtension(Path.GetFileName(ArqPedido), ".err");
            string ArqERRRetorno = ArqXMLRetorno.Replace((xml ? ".xml" : ".txt"), ".err");

            try
            {
                Functions.DeletarArquivo(ArqXMLRetorno);
                Functions.DeletarArquivo(ArqERRRetorno);
                Functions.DeletarArquivo(Empresas.Configuracoes[emp].PastaXmlErro + "\\" + ArqPedido);

                if(!File.Exists(ArqPedido))
                {
                    throw new Exception("Arquivo " + ArqPedido + " não encontrado");
                }

                //                if (!Auxiliar.FileInUse(ArqPedido))
                //                {
                int serie = 0;
                int tpEmis = Empresas.Configuracoes[emp].tpEmis;
                int nNF = 0;
                int cNF = 0;
                int cUF = Empresas.Configuracoes[emp].UnidadeFederativaCodigo;
                string cAAMM = "0000";
                string cChave = "";
                string cCNPJ = "";
                string cError = "";
                string cMod = "";

                if(xml)
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(ArqPedido);

                    XmlNodeList mChaveList = doc.GetElementsByTagName("gerarChave");

                    foreach(XmlNode mChaveNode in mChaveList)
                    {
                        XmlElement mChaveElemento = (XmlElement)mChaveNode;

                        if(mChaveElemento.GetElementsByTagName(TpcnResources.UF.ToString()).Count != 0)
                        {
                            cUF = Convert.ToInt32("0" + mChaveElemento.GetElementsByTagName(TpcnResources.UF.ToString())[0].InnerText);
                        }

                        if(mChaveElemento.GetElementsByTagName(TpcnResources.tpEmis.ToString()).Count != 0)
                        {
                            tpEmis = Convert.ToInt32("0" + mChaveElemento.GetElementsByTagName(TpcnResources.tpEmis.ToString())[0].InnerText);
                        }

                        if(mChaveElemento.GetElementsByTagName(TpcnResources.nNF.ToString()).Count != 0)
                        {
                            nNF = Convert.ToInt32("0" + mChaveElemento.GetElementsByTagName(TpcnResources.nNF.ToString())[0].InnerText);
                        }

                        if(mChaveElemento.GetElementsByTagName(TpcnResources.cNF.ToString()).Count != 0)
                        {
                            cNF = Convert.ToInt32("0" + mChaveElemento.GetElementsByTagName(TpcnResources.cNF.ToString())[0].InnerText);
                        }

                        if(mChaveElemento.GetElementsByTagName(TpcnResources.serie.ToString()).Count != 0)
                        {
                            serie = Convert.ToInt32("0" + mChaveElemento.GetElementsByTagName(TpcnResources.serie.ToString())[0].InnerText);
                        }

                        if(mChaveElemento.GetElementsByTagName(TpcnResources.AAMM.ToString()).Count != 0)
                        {
                            cAAMM = mChaveElemento.GetElementsByTagName(TpcnResources.AAMM.ToString())[0].InnerText;
                        }

                        if(mChaveElemento.GetElementsByTagName(TpcnResources.CNPJ.ToString()).Count != 0)
                        {
                            cCNPJ = mChaveElemento.GetElementsByTagName(TpcnResources.CNPJ.ToString())[0].InnerText;
                        }

                        if(mChaveElemento.GetElementsByTagName(TpcnResources.mod.ToString()).Count != 0)
                        {
                            cMod = mChaveElemento.GetElementsByTagName(TpcnResources.mod.ToString())[0].InnerText;
                        }
                    }
                }
                else
                {
                    List<string> cLinhas = Functions.LerArquivo(ArqPedido);
                    string[] dados;
                    foreach(string cLinha in cLinhas)
                    {
                        dados = cLinha.Split('|');
                        dados[0] = dados[0].ToUpper();
                        if(dados.GetLength(0) == 1)
                        {
                            cError += "Segmento [" + dados[0] + "] inválido" + Environment.NewLine;
                        }
                        else
                        {
                            switch(dados[0].ToLower())
                            {
                                case "uf":
                                    cUF = Convert.ToInt32("0" + dados[1].Trim());
                                    break;

                                case "tpemis":
                                    tpEmis = Convert.ToInt32("0" + dados[1].Trim());
                                    break;

                                case "nnf":
                                    nNF = Convert.ToInt32("0" + dados[1].Trim());
                                    break;

                                case "cnf":
                                    cNF = Convert.ToInt32("0" + dados[1].Trim());
                                    break;

                                case "serie":
                                    serie = Convert.ToInt32("0" + dados[1].Trim());
                                    break;

                                case "aamm":
                                    cAAMM = dados[1].Trim();
                                    break;

                                case "cnpj":
                                    cCNPJ = dados[1].Trim();
                                    break;

                                case "mod":
                                    cMod = dados[1].Trim();
                                    break;
                            }
                        }
                    }
                }

                if(nNF == 0)
                {
                    cError = "Número da nota fiscal deve ser informado" + Environment.NewLine;
                }

                if(string.IsNullOrEmpty(cAAMM))
                {
                    cError += "Ano e mês da emissão deve ser informado" + Environment.NewLine;
                }

                if(string.IsNullOrEmpty(cCNPJ))
                {
                    cError += "CNPJ deve ser informado" + Environment.NewLine;
                }

                if(cAAMM.Substring(0, 2) == "00")
                {
                    cError += "Ano da emissão inválido" + Environment.NewLine;
                }

                if(Convert.ToInt32(cAAMM.Substring(2, 2)) <= 0 || Convert.ToInt32(cAAMM.Substring(2, 2)) > 12)
                {
                    cError += "Mês da emissão inválido" + Environment.NewLine;
                }

                if(cMod == "")
                {
                    switch(Empresas.Configuracoes[emp].Servico)
                    {
                        case TipoAplicativo.Cte:
                            cMod = ((int)TpcnMod.modCTe).ToString();
                            break;

                        case TipoAplicativo.MDFe:
                            cMod = ((int)TpcnMod.modMDFe).ToString();
                            break;

                        case TipoAplicativo.NFCe:
                            cMod = ((int)TpcnMod.modNFCe).ToString();
                            break;

                        case TipoAplicativo.Nfe:
                            cMod = ((int)TpcnMod.modNFe).ToString();
                            break;

                        default:

                            if(!("55,57,58,65").Contains(cMod))
                            {
                                cError += "Mod inválido. Deve ser '55 p/NFe','57 p/Cte','58 p/MDF-e' ou '65 p/NFC-e'" + Environment.NewLine;
                            }

                            break;
                    }
                }

                if(cError != "")
                {
                    throw new Exception(cError);
                }

                long iTmp = Convert.ToInt64("0" + cCNPJ);
                cChave = cUF.ToString("00") + cAAMM.Trim() + iTmp.ToString("00000000000000") + cMod;

                if(cNF == 0)
                {
                    ///
                    /// gera codigo aleatorio
                    ///
                    cNF = GerarCodigoNumerico(nNF);
                }

                ///
                /// calcula do digito verificador
                ///
                string ccChave = cChave + serie.ToString("000") + nNF.ToString("000000000") + tpEmis.ToString("0") + cNF.ToString("00000000");
                int cDV = GerarDigito(ccChave);
                ///
                /// monta a chave da NFe
                ///
                cChave += serie.ToString("000") + nNF.ToString("000000000") + tpEmis.ToString("0") + cNF.ToString("00000000") + cDV.ToString("0");

                ///
                /// grava o XML/TXT de resposta
                ///
                string vMsgRetorno = (xml ? "<?xml version=\"1.0\" encoding=\"UTF-8\"?><retGerarChave><chaveNFe>" + cChave + "</chaveNFe></retGerarChave>" : cChave);
                File.WriteAllText(ArqXMLRetorno, vMsgRetorno);//, Encoding.Default);

                ///
                /// exclui o XML/TXT de pedido
                ///
                Functions.DeletarArquivo(ArqPedido);
            }
            catch(Exception ex)
            {
                try
                {
                    new Auxiliar().MoveArqErro(ArqPedido);

                    File.WriteAllText(ArqERRRetorno, "Arquivo " + ArqERRRetorno + Environment.NewLine + ex.Message);//, Encoding.Default);
                }
                catch
                {
                    //Se der algum erro na hora de gravar o arquivo de erro para o ERP, infelizmente não vamos poder fazer nada, visto que
                    //pode ser algum problema com a rede, hd, permissões, etc... Wandrey 22/03/2010
                }
            }
        }

        #endregion GerarChaveNFe
    }
}
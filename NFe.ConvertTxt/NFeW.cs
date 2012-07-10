﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Globalization;
using NFe.Settings;
using System.Threading;
using NFe.Components;

namespace NFe.ConvertTxt
{
    public class NFeW
    {
        public string cMensagemErro { get; set; }
        public string cFileName { get; private set; }

        private XmlDocument doc;
        private XmlNode nodeCurrent = null;

        /// <summary>
        /// GerarXml
        /// </summary>
        /// <param name="NFe"></param>
        public void GerarXml(NFe NFe, string cDestino, string cRetorno)
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
            xmlVersion1.Value = "http://www.portalfiscal.inf.br/nfe";
            xmlInf.Attributes.Append(xmlVersion1);
            doc.AppendChild(xmlInf);

            string cChave = NFe.ide.cUF.ToString() +
                            NFe.ide.dEmi.Year.ToString("0000").Substring(2) +
                            NFe.ide.dEmi.Month.ToString("00"); //data AAMM

            Int64 iTmp = Convert.ToInt64("0" + NFe.emit.CNPJ + NFe.emit.CPF);
            cChave += iTmp.ToString("00000000000000") + "55";

            if (NFe.ide.cNF == 0)
            {
                ///
                /// gera codigo aleatorio
                /// 
                NFe.ide.cNF = this.GerarCodigoNumerico(NFe.ide.nNF);
            }
            if (NFe.ide.cDV == 0)
            {
                ///
                /// calcula digito verificador
                /// 
                string ccChave = cChave +
                                 NFe.ide.serie.ToString("000") +
                                 NFe.ide.nNF.ToString("000000000") +
                                 ((int)NFe.ide.tpEmis).ToString("0") +
                                 NFe.ide.cNF.ToString("00000000");

                NFe.ide.cDV = this.GerarDigito(ccChave);
            }
            cChave += NFe.ide.serie.ToString("000") +
                        NFe.ide.nNF.ToString("000000000") +
                        ((int)NFe.ide.tpEmis).ToString("0") +
                        NFe.ide.cNF.ToString("00000000") +
                        NFe.ide.cDV.ToString("0");
            NFe.infNFe.ID = cChave;

            ///
            /// infNFe
            /// 
            XmlElement infNfe = doc.CreateElement("infNFe");

            XmlAttribute infNfeAttr1 = doc.CreateAttribute("versao");
            infNfeAttr1.Value = "2.00";
            infNfe.Attributes.Append(infNfeAttr1);

            XmlAttribute infNfeAttrId = doc.CreateAttribute("Id");
            infNfeAttrId.Value = "NFe" + NFe.infNFe.ID;
            infNfe.Attributes.Append(infNfeAttrId);
            xmlInf.AppendChild(infNfe);

            infNfe.AppendChild(GerarInfNFe(NFe));
            infNfe.AppendChild(GerarEmit(NFe));
            GerarAvulsa(NFe, infNfe);
            infNfe.AppendChild(GerarDest(NFe));
            GerarRetirada(NFe, infNfe);
            GerarEntrega(NFe, infNfe);
            GerarDet(NFe, infNfe);
            GerarTotal(NFe, infNfe);
            GerarTransp(NFe.Transp, infNfe);
            GerarCobr(NFe.Cobr, infNfe);
            GerarInfAdic(NFe.InfAdic, infNfe);
            GerarExporta(NFe.exporta, infNfe);
            GerarCompra(NFe.compra, infNfe);
            GerarCana(NFe.cana, infNfe);

            this.cFileName = NFe.infNFe.ID + "-nfe.xml";
            string strRetorno = this.cFileName;

            if (string.IsNullOrEmpty(cDestino))
            {
                FileInfo oArqDestino = new FileInfo(this.cFileName);
                this.cFileName = oArqDestino.FullName;
            }
            else
            {
                //                if (cDestino.ToLower().IndexOf("convertidos") == -1)
                //                    cDestino += "\\convertidos";

                if (cDestino.Substring(cDestino.Length - 1, 1) == @"\")
                    cDestino = cDestino.Substring(0, cDestino.Length - 1);

                //#if !teste
                if (!Directory.Exists(Path.Combine(cDestino, "Convertidos")))
                {
                    ///
                    /// cria uma pasta temporária para armazenar o XML convertido
                    /// 
                    System.IO.Directory.CreateDirectory(Path.Combine(cDestino, "Convertidos"));
                }
                strRetorno = Path.Combine(cDestino, "Convertidos");
                this.cFileName = Path.Combine(strRetorno, this.cFileName);
                //#else
                //                this.cFileName = Path.Combine(cDestino, this.cFileName);
                //
                //                string retornoUninfe = Path.Combine(cRetorno, Path.GetFileNameWithoutExtension(this.cFileName) + "-ret.xml");
                //                //System.Windows.Forms.MessageBox.Show(retornoUninfe);
                //                if (File.Exists(retornoUninfe))
                //                {
                //File.Delete(retornoUninfe);
                //                    System.Threading.Thread.Sleep(500);
                //                }
                //#endif
            }

            strRetorno = this.cFileName;
            
            //XmlTextWriter xWriter = new XmlTextWriter(@strRetorno, Encoding.UTF8);
            
#if !teste
            //xWriter.Formatting = Formatting.None;
#else
            xWriter.Formatting = Formatting.Indented;
#endif
            //doc.Save(xWriter);
            doc.Save(@strRetorno);
            //xWriter.Close();
        }

        /// <summary>
        /// GerarAvulsa
        /// </summary>
        /// <param name="NFe"></param>
        /// <param name="root"></param>
        private void GerarAvulsa(NFe NFe, XmlElement root)
        {
            if (!string.IsNullOrEmpty(NFe.avulsa.CNPJ))
            {
                XmlElement ELav = doc.CreateElement("avulsa");
                nodeCurrent = ELav;
                root.AppendChild(ELav);

                wCampo(NFe.avulsa.CNPJ, TpcnTipoCampo.tcStr, Properties.Resources.CNPJ);
                wCampo(NFe.avulsa.xOrgao, TpcnTipoCampo.tcStr, Properties.Resources.xOrgao);
                wCampo(NFe.avulsa.matr, TpcnTipoCampo.tcStr, Properties.Resources.matr);
                wCampo(NFe.avulsa.xAgente, TpcnTipoCampo.tcStr, Properties.Resources.xAgente);
                wCampo(NFe.avulsa.fone, TpcnTipoCampo.tcStr, Properties.Resources.fone);
                wCampo(NFe.avulsa.UF, TpcnTipoCampo.tcStr, Properties.Resources.UF);
                wCampo(NFe.avulsa.nDAR, TpcnTipoCampo.tcStr, Properties.Resources.nDAR);
                wCampo(NFe.avulsa.dEmi, TpcnTipoCampo.tcDat, Properties.Resources.dEmi);
                wCampo(NFe.avulsa.vDAR, TpcnTipoCampo.tcDec2, Properties.Resources.vDAR);
                wCampo(NFe.avulsa.repEmi, TpcnTipoCampo.tcStr, Properties.Resources.repEmi);
                wCampo(NFe.avulsa.dPag, TpcnTipoCampo.tcDat, Properties.Resources.dPag, ObOp.Opcional);
            }
        }

        /// <summary>
        /// GerarCobr
        /// </summary>
        /// <param name="Cobr"></param>
        /// <param name="root"></param>
        private void GerarCobr(Cobr Cobr, XmlElement root)
        {
            if (!string.IsNullOrEmpty(Cobr.Fat.nFat) ||
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
                if ((!string.IsNullOrEmpty(Cobr.Fat.nFat) ||
                    (Cobr.Fat.vOrig > 0) ||
                    (Cobr.Fat.vDesc > 0) ||
                    (Cobr.Fat.vLiq > 0)))
                {
                    XmlElement nodeFat = doc.CreateElement("fat");
                    nodeCobr.AppendChild(nodeFat);
                    nodeCurrent = nodeFat;

                    wCampo(Cobr.Fat.nFat, TpcnTipoCampo.tcStr, Properties.Resources.nFat);
                    wCampo(Cobr.Fat.vOrig, TpcnTipoCampo.tcDec2, Properties.Resources.vOrig);
                    if (Cobr.Fat.vDesc > 0)
                        wCampo(Cobr.Fat.vDesc, TpcnTipoCampo.tcDec2, Properties.Resources.vDesc);
                    wCampo(Cobr.Fat.vLiq, TpcnTipoCampo.tcDec2, Properties.Resources.vLiq);
                }
                //
                //(**)GerarCobrDup;
                //
                foreach (Dup Dup in Cobr.Dup)
                {
                    XmlElement nodeDup = doc.CreateElement("dup");
                    nodeCobr.AppendChild(nodeDup);
                    nodeCurrent = nodeDup;

                    wCampo(Dup.nDup, TpcnTipoCampo.tcStr, Properties.Resources.nDup);
                    wCampo(Dup.dVenc, TpcnTipoCampo.tcDat, Properties.Resources.dVenc);
                    wCampo(Dup.vDup, TpcnTipoCampo.tcDec2, Properties.Resources.vDup);
                }
            }
        }

        /// <summary>
        /// GerarCodigoNumerico
        /// </summary>
        /// <param name="numeroNF"></param>
        /// <returns></returns>
        public Int32 GerarCodigoNumerico(Int32 numeroNF)
        {
            Int32 codigoRetorno = 0;
            while (codigoRetorno == 0)
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
            if (!string.IsNullOrEmpty(cana.safra) || !string.IsNullOrEmpty(cana.Ref) ||
                (cana.fordia.Count > 0) || (cana.deduc.Count > 0))
            {
                XmlElement rootCana = doc.CreateElement("cana");
                root.AppendChild(rootCana);
                nodeCurrent = rootCana;

                wCampo(cana.safra, TpcnTipoCampo.tcStr, Properties.Resources.safra, ObOp.Opcional);
                wCampo(cana.Ref, TpcnTipoCampo.tcStr, Properties.Resources.Ref, ObOp.Opcional);

                if (cana.fordia.Count > 31)
                    this.cMensagemErro += "Número máximo de elementos no segmento 'ZC04' excedeu" + Environment.NewLine;

                foreach (fordia item in cana.fordia)
                {
                    XmlElement nodefordia = doc.CreateElement("forDia");
                    XmlAttribute xmlItem = doc.CreateAttribute(Properties.Resources.dia);   //
                    xmlItem.Value = item.dia.ToString();                                    //danasa 3/11/2011
                    nodefordia.Attributes.Append(xmlItem);                                  //
                    rootCana.AppendChild(nodefordia);
                    nodeCurrent = nodefordia;

                    //wCampo(item.dia, TpcnTipoCampo.tcInt, Properties.Resources.dia);
                    wCampo(item.qtde, item.qtde_Tipo, Properties.Resources.qtde);
                }
                nodeCurrent = rootCana;
                wCampo(cana.qTotMes, cana.qTotMes_Tipo, Properties.Resources.qTotMes);
                wCampo(cana.qTotAnt, cana.qTotAnt_Tipo, Properties.Resources.qTotAnt);
                wCampo(cana.qTotGer, cana.qTotGer_Tipo, Properties.Resources.qTotGer);

                if (cana.deduc.Count > 10)
                    this.cMensagemErro += "Número máximo de elementos no segmento 'ZC10' excedeu" + Environment.NewLine;

                foreach (deduc item in cana.deduc)
                {
                    XmlElement nodededuc = doc.CreateElement("deduc");
                    rootCana.AppendChild(nodededuc);
                    nodeCurrent = nodededuc;

                    wCampo(item.xDed, TpcnTipoCampo.tcStr, Properties.Resources.xDed);
                    wCampo(item.vDed, TpcnTipoCampo.tcDec2, Properties.Resources.vDed);
                }
                nodeCurrent = rootCana;
                wCampo(cana.vFor, TpcnTipoCampo.tcDec2, Properties.Resources.vFor);
                wCampo(cana.vTotDed, TpcnTipoCampo.tcDec2, Properties.Resources.vTotDed);
                wCampo(cana.vLiqFor, TpcnTipoCampo.tcDec2, Properties.Resources.vLiqFor);
            }
        }

        /// <summary>
        /// GerarCompra
        /// </summary>
        /// <param name="compra"></param>
        /// <param name="root"></param>
        private void GerarCompra(Compra compra, XmlElement root)
        {
            if (!string.IsNullOrEmpty(compra.xNEmp) || !string.IsNullOrEmpty(compra.xPed) || !string.IsNullOrEmpty(compra.xCont))
            {
                nodeCurrent = doc.CreateElement("compra");
                root.AppendChild(nodeCurrent);

                /// incluida a opcao->opcional
                wCampo(compra.xNEmp, TpcnTipoCampo.tcStr, Properties.Resources.xNEmp, ObOp.Opcional);
                wCampo(compra.xPed, TpcnTipoCampo.tcStr, Properties.Resources.xPed, ObOp.Opcional);
                wCampo(compra.xCont, TpcnTipoCampo.tcStr, Properties.Resources.xCont, ObOp.Opcional);
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

            if (NFe.ide.tpAmb == TpcnTipoAmbiente.taProducao)
            {

            if (NFe.dest.enderDest.UF != "EX" || NFe.dest.enderDest.cPais == 1058)
            {
                if (!string.IsNullOrEmpty(NFe.dest.CNPJ))
                    wCampo(NFe.dest.CNPJ, TpcnTipoCampo.tcStr, Properties.Resources.CNPJ);
                else
                    wCampo(NFe.dest.CPF, TpcnTipoCampo.tcStr, Properties.Resources.CPF);
            }
            else
                wCampo("", TpcnTipoCampo.tcStr, Properties.Resources.CNPJ);

            wCampo(NFe.dest.xNome, TpcnTipoCampo.tcStr, Properties.Resources.xNome);
            }
            else
            {
                if (NFe.dest.enderDest.UF == "EX")
                    wCampo(NFe.dest.CNPJ, TpcnTipoCampo.tcStr, Properties.Resources.CNPJ);
                else
                    wCampo("99999999000191", TpcnTipoCampo.tcStr, Properties.Resources.CNPJ);

                wCampo("NF-E EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL", TpcnTipoCampo.tcStr, Properties.Resources.xNome);
            }
            ///
            /// (**)GerarDestEnderDest(UF);
            /// 
            XmlElement e1 = doc.CreateElement("enderDest");
            e0.AppendChild(e1);
            nodeCurrent = e1;

            wCampo(NFe.dest.enderDest.xLgr, TpcnTipoCampo.tcStr, Properties.Resources.xLgr);
            wCampo(NFe.dest.enderDest.nro, TpcnTipoCampo.tcStr, Properties.Resources.nro);
            wCampo(NFe.dest.enderDest.xCpl, TpcnTipoCampo.tcStr, Properties.Resources.xCpl, ObOp.Opcional);
            wCampo(NFe.dest.enderDest.xBairro, TpcnTipoCampo.tcStr, Properties.Resources.xBairro);
            wCampo(NFe.dest.enderDest.cMun, TpcnTipoCampo.tcInt, Properties.Resources.cMun, ObOp.Obrigatorio, 7);
            wCampo(NFe.dest.enderDest.xMun, TpcnTipoCampo.tcStr, Properties.Resources.xMun);
            wCampo(NFe.dest.enderDest.UF, TpcnTipoCampo.tcStr, Properties.Resources.UF);
            wCampo(NFe.dest.enderDest.CEP, TpcnTipoCampo.tcInt, Properties.Resources.CEP, ObOp.Opcional, 8);
            wCampo(NFe.dest.enderDest.cPais, TpcnTipoCampo.tcInt, Properties.Resources.cPais, ObOp.Opcional, 4);
            wCampo(NFe.dest.enderDest.xPais, TpcnTipoCampo.tcStr, Properties.Resources.xPais, ObOp.Opcional);
            wCampo(NFe.dest.enderDest.fone, TpcnTipoCampo.tcStr, Properties.Resources.fone, ObOp.Opcional);
            ///
            /// </enderDest">
            /// 
            nodeCurrent = e0;
            if (NFe.ide.tpAmb == TpcnTipoAmbiente.taProducao)
            {
            if (NFe.dest.enderDest.UF != "EX" || NFe.dest.enderDest.cPais == 1058)
            {
                wCampo(NFe.dest.IE, TpcnTipoCampo.tcStr, Properties.Resources.IE);
                wCampo(NFe.dest.ISUF, TpcnTipoCampo.tcStr, Properties.Resources.ISUF, ObOp.Opcional);
            }
            else
                wCampo("ISENTO", TpcnTipoCampo.tcStr, Properties.Resources.IE);
            }
            else
            {
                // conforme nota técnica 2011/02
                wCampo("", TpcnTipoCampo.tcStr, Properties.Resources.IE);
            }
            wCampo(NFe.dest.email, TpcnTipoCampo.tcStr, Properties.Resources.email, ObOp.Opcional);

            return e0;
        }

        /// <summary>
        /// GerarDet
        /// </summary>
        /// <param name="NFe"></param>
        /// <param name="root"></param>
        private void GerarDet(NFe NFe, XmlElement root)
        {
            if (NFe.det.Count > 990)
                this.cMensagemErro += "Número máximo de itens excedeu o máximo permitido" + Environment.NewLine;

            foreach (Det det in NFe.det)
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

                wCampo(det.Prod.cProd, TpcnTipoCampo.tcStr, Properties.Resources.cProd);
                wCampo(det.Prod.cEAN, TpcnTipoCampo.tcStr, Properties.Resources.cEAN);
                wCampo(det.Prod.xProd, TpcnTipoCampo.tcStr, Properties.Resources.xProd);
                wCampo(det.Prod.NCM, TpcnTipoCampo.tcStr, Properties.Resources.NCM);
                wCampo(det.Prod.EXTIPI, TpcnTipoCampo.tcStr, Properties.Resources.EXTIPI, ObOp.Opcional);
                wCampo(det.Prod.CFOP, TpcnTipoCampo.tcStr, Properties.Resources.CFOP);
                wCampo(det.Prod.uCom, TpcnTipoCampo.tcStr, Properties.Resources.uCom);
                wCampo(det.Prod.qCom, TpcnTipoCampo.tcDec4, Properties.Resources.qCom);
                wCampo(det.Prod.vUnCom, det.Prod.vUnCom_Tipo, Properties.Resources.vUnCom);
                wCampo(det.Prod.vProd, TpcnTipoCampo.tcDec2, Properties.Resources.vProd);
                wCampo(det.Prod.cEANTrib, TpcnTipoCampo.tcStr, Properties.Resources.cEANTrib);
                wCampo(det.Prod.uTrib, TpcnTipoCampo.tcStr, Properties.Resources.uTrib);
                wCampo(det.Prod.qTrib, TpcnTipoCampo.tcDec4, Properties.Resources.qTrib);
                wCampo(det.Prod.vUnTrib, det.Prod.vUnTrib_Tipo, Properties.Resources.vUnTrib);
                wCampo(det.Prod.vFrete, TpcnTipoCampo.tcDec2, Properties.Resources.vFrete, ObOp.Opcional);
                wCampo(det.Prod.vSeg, TpcnTipoCampo.tcDec2, Properties.Resources.vSeg, ObOp.Opcional);
                wCampo(det.Prod.vDesc, TpcnTipoCampo.tcDec2, Properties.Resources.vDesc, ObOp.Opcional);
                wCampo(det.Prod.vOutro, TpcnTipoCampo.tcDec2, Properties.Resources.vOutro, ObOp.Opcional);
                wCampo(det.Prod.indTot, TpcnTipoCampo.tcInt, Properties.Resources.indTot);

                #region /// DI

                if (det.Prod.DI.Count > 100)
                    this.cMensagemErro += "Número máximo de itens DI excedeu o máximo permitido" + Environment.NewLine;

                XmlNode oldNode = nodeCurrent;
                foreach (DI di in det.Prod.DI)
                {
                    XmlElement nodeDI = doc.CreateElement("DI");
                    nodeProd.AppendChild(nodeDI);
                    nodeCurrent = nodeDI;

                    wCampo(di.nDI, TpcnTipoCampo.tcStr, Properties.Resources.nDI);
                    wCampo(di.dDI, TpcnTipoCampo.tcDat, Properties.Resources.dDI);
                    wCampo(di.xLocDesemb, TpcnTipoCampo.tcStr, Properties.Resources.xLocDesemb);
                    wCampo(di.UFDesemb, TpcnTipoCampo.tcStr, Properties.Resources.UFDesemb);
                    wCampo(di.dDesemb, TpcnTipoCampo.tcDat, Properties.Resources.dDesemb);
                    wCampo(di.cExportador, TpcnTipoCampo.tcStr, Properties.Resources.cExportador);
                    //
                    //GerarDetProdDIadi
                    //
                    if (di.adi.Count > 100)
                        this.cMensagemErro += "Número máximo de itens DI->ADI excedeu o máximo permitido" + Environment.NewLine;

                    if (di.adi.Count == 0)
                        this.cMensagemErro += "Número minimo de itens DI->ADI não permitido" + Environment.NewLine;

                    foreach (Adi adi in di.adi)
                    {
                        XmlElement e1 = doc.CreateElement("adi");
                        nodeDI.AppendChild(e1);
                        nodeCurrent = e1;

                        wCampo(adi.nAdicao, TpcnTipoCampo.tcInt, Properties.Resources.nAdicao);
                        wCampo(adi.nSeqAdi, TpcnTipoCampo.tcInt, Properties.Resources.nSeqAdic);
                        wCampo(adi.cFabricante, TpcnTipoCampo.tcStr, Properties.Resources.cFabricante);
                        wCampo(adi.vDescDI, TpcnTipoCampo.tcDec2, Properties.Resources.vDescDI, ObOp.Opcional);
                    }
                }
                nodeCurrent = oldNode;
                #endregion

                wCampo(det.Prod.xPed, TpcnTipoCampo.tcStr, Properties.Resources.xPed, ObOp.Opcional);
                wCampo(det.Prod.nItemPed, TpcnTipoCampo.tcInt, Properties.Resources.nItemPed, ObOp.Opcional);

                #region /// veiculos
                if (!string.IsNullOrEmpty(det.Prod.veicProd.chassi))
                {
                    oldNode = nodeCurrent;
                    XmlElement nodeVeic = doc.CreateElement("veicProd");
                    nodeProd.AppendChild(nodeVeic);
                    nodeCurrent = nodeVeic;

                    wCampo(det.Prod.veicProd.tpOp, TpcnTipoCampo.tcStr, Properties.Resources.tpOp);
                    wCampo(det.Prod.veicProd.chassi, TpcnTipoCampo.tcStr, Properties.Resources.chassi);
                    wCampo(det.Prod.veicProd.cCor, TpcnTipoCampo.tcStr, Properties.Resources.cCor);
                    wCampo(det.Prod.veicProd.xCor, TpcnTipoCampo.tcStr, Properties.Resources.xCor);
                    wCampo(det.Prod.veicProd.pot, TpcnTipoCampo.tcStr, Properties.Resources.pot);
                    wCampo(det.Prod.veicProd.cilin, TpcnTipoCampo.tcStr, Properties.Resources.cilin);
                    wCampo(det.Prod.veicProd.pesoL, TpcnTipoCampo.tcStr, Properties.Resources.pesoL);
                    wCampo(det.Prod.veicProd.pesoB, TpcnTipoCampo.tcStr, Properties.Resources.pesoB);
                    wCampo(det.Prod.veicProd.nSerie, TpcnTipoCampo.tcStr, Properties.Resources.nSerie);
                    wCampo(det.Prod.veicProd.tpComb, TpcnTipoCampo.tcStr, Properties.Resources.tpComb);
                    wCampo(det.Prod.veicProd.nMotor, TpcnTipoCampo.tcStr, Properties.Resources.nMotor);
                    wCampo(det.Prod.veicProd.CMT, TpcnTipoCampo.tcStr, Properties.Resources.CMT);
                    wCampo(det.Prod.veicProd.dist, TpcnTipoCampo.tcStr, Properties.Resources.dist);
                    wCampo(det.Prod.veicProd.anoMod, TpcnTipoCampo.tcInt, Properties.Resources.anoMod, ObOp.Obrigatorio, 4);
                    wCampo(det.Prod.veicProd.anoFab, TpcnTipoCampo.tcInt, Properties.Resources.anoFab, ObOp.Obrigatorio, 4);
                    wCampo(det.Prod.veicProd.tpPint, TpcnTipoCampo.tcStr, Properties.Resources.tpPint);
                    wCampo(det.Prod.veicProd.tpVeic, TpcnTipoCampo.tcInt, Properties.Resources.tpVeic);
                    wCampo(det.Prod.veicProd.espVeic, TpcnTipoCampo.tcInt, Properties.Resources.espVeic);
                    wCampo(det.Prod.veicProd.VIN, TpcnTipoCampo.tcStr, Properties.Resources.VIN);
                    wCampo(det.Prod.veicProd.condVeic, TpcnTipoCampo.tcStr, Properties.Resources.condVeic);
                    wCampo(det.Prod.veicProd.cMod, TpcnTipoCampo.tcStr, Properties.Resources.cMod);
                    wCampo(det.Prod.veicProd.cCorDENATRAN, TpcnTipoCampo.tcInt, Properties.Resources.cCorDENATRAN, ObOp.Obrigatorio, 2);
                    wCampo(det.Prod.veicProd.lota, TpcnTipoCampo.tcInt, Properties.Resources.lota);
                    wCampo(det.Prod.veicProd.tpRest, TpcnTipoCampo.tcInt, Properties.Resources.tpRest);

                    nodeCurrent = oldNode;
                }
                #endregion

                #region /// medicamentos
                foreach (Med med in det.Prod.med)
                {
                    XmlElement e0 = doc.CreateElement("med");
                    root.AppendChild(e0);
                    nodeCurrent = e0;

                    wCampo(med.nLote, TpcnTipoCampo.tcStr, Properties.Resources.nLote);
                    wCampo(med.qLote, TpcnTipoCampo.tcDec3, Properties.Resources.qLote);
                    wCampo(med.dFab, TpcnTipoCampo.tcDat, Properties.Resources.dFab);
                    wCampo(med.dVal, TpcnTipoCampo.tcDat, Properties.Resources.dVal);
                    wCampo(med.vPMC, TpcnTipoCampo.tcDec2, Properties.Resources.vPMC);
                }
                #endregion

                #region /// armamento
                foreach (Arma arma in det.Prod.arma)
                {
                    XmlElement nodeArma = doc.CreateElement("arma");
                    nodeProd.AppendChild(nodeArma);
                    nodeCurrent = nodeArma;

                    wCampo(arma.tpArma, TpcnTipoCampo.tcInt, Properties.Resources.tpArma);
                    wCampo(arma.nSerie, TpcnTipoCampo.tcInt, Properties.Resources.nSerie);
                    wCampo(arma.nCano, TpcnTipoCampo.tcInt, Properties.Resources.nCano);
                    wCampo(arma.descr, TpcnTipoCampo.tcStr, Properties.Resources.descr);
                }
                #endregion

                #region /// combustiveis
                if ((det.Prod.comb.cProdANP > 0))
                {
                    XmlElement e0 = doc.CreateElement("comb");
                    root.AppendChild(e0);
                    nodeCurrent = e0;

                    wCampo(det.Prod.comb.cProdANP, TpcnTipoCampo.tcInt, Properties.Resources.cProdANP);
                    if (!string.IsNullOrEmpty(det.Prod.comb.CODIF))
                        wCampo(det.Prod.comb.CODIF, TpcnTipoCampo.tcStr, Properties.Resources.CODIF);
                    if (det.Prod.comb.qTemp > 0)
                        wCampo(det.Prod.comb.qTemp, TpcnTipoCampo.tcDec4, Properties.Resources.qTemp);
                    wCampo(det.Prod.comb.UFCons, TpcnTipoCampo.tcStr, Properties.Resources.UFCons);

                    if ((det.Prod.comb.CIDE.qBCprod > 0) ||
                        (det.Prod.comb.CIDE.vAliqProd > 0) ||
                        (det.Prod.comb.CIDE.vCIDE > 0))
                    {
                        XmlElement e1 = doc.CreateElement("CIDE");
                        e0.AppendChild(e1);
                        nodeCurrent = e1;

                        wCampo(det.Prod.comb.CIDE.qBCprod, TpcnTipoCampo.tcDec4, Properties.Resources.qBCProd);
                        wCampo(det.Prod.comb.CIDE.vAliqProd, TpcnTipoCampo.tcDec4, Properties.Resources.vAliqProd);
                        wCampo(det.Prod.comb.CIDE.vCIDE, TpcnTipoCampo.tcDec2, Properties.Resources.vCIDE);

                        nodeCurrent = e0;
                    }

#if lixo
                    if (
                    (det.Prod.comb.CIDE.qBCprod > 0) ||
                    (det.Prod.comb.CIDE.vAliqProd > 0) ||
                    (det.Prod.comb.CIDE.vCIDE > 0) ||
                    (det.Prod.comb.ICMS.vBCICMS > 0) ||
                    (det.Prod.comb.ICMS.vICMS > 0) ||
                    (det.Prod.comb.ICMS.vBCICMSST > 0) ||
                    (det.Prod.comb.ICMS.vICMSST > 0) ||
                    //(det.Prod.comb.ICMSInter.vBCICMSSTDest > 0) ||
                    //(det.Prod.comb.ICMSInter.vICMSSTDest > 0) ||
                    (det.Prod.comb.ICMSCons.vBCICMSSTCons > 0) ||
                    (det.Prod.comb.ICMSCons.vICMSSTCons > 0) ||
                    (!string.IsNullOrEmpty(det.Prod.comb.ICMSCons.UFcons)))
                {
                    //
                    //(**)GerarDetProdCombCIDE(i);
                    //
                    //
                    //(**)GerarDetProdCombICMS(i);
                    //
                    if ((det.Prod.comb.ICMS.vBCICMS > 0) ||
                        (det.Prod.comb.ICMS.vICMS > 0) ||
                        (det.Prod.comb.ICMS.vBCICMSST > 0) ||
                        (det.Prod.comb.ICMS.vICMSST > 0))
                    {
                        XmlElement e2 = doc.CreateElement("ICMSComb");
                        e0.AppendChild(e2);
                        nodeCurrent = e2;

                        wCampo(det.Prod.comb.ICMS.vBCICMS, TpcnTipoCampo.tcDec2, Properties.Resources.vBCICMS);
                        wCampo(det.Prod.comb.ICMS.vICMS, TpcnTipoCampo.tcDec2, Properties.Resources.vICMS);
                        wCampo(det.Prod.comb.ICMS.vBCICMSST, TpcnTipoCampo.tcDec2, Properties.Resources.vBCICMSST);
                        wCampo(det.Prod.comb.ICMS.vICMSST, TpcnTipoCampo.tcDec2, Properties.Resources.vICMSST);
                    }
                    //
                    //(**)GerarDetProdCombICMSInter(i);
                    // 
                    /*
                    if ((comb.ICMSInter.vBCICMSSTDest > 0) || (comb.ICMSInter.vICMSSTDest > 0))
                    {
                        XmlElement e2 = doc.CreateElement("ICMSInter");
                        e0.AppendChild(e2);
                        nodeCurrent = e2;
    
                        wCampo(comb.ICMSInter.vBCICMSSTDest, TpcnTipoCampo.tcDec2, Properties.Resources.vBCICMSSTDest);
                        wCampo(comb.ICMSInter.vICMSSTDest, TpcnTipoCampo.tcDec2, Properties.Resources.vICMSSTDest);
                    }*/
                    //
                    //(**)GerarDetProdCombICMSCons(i);
                    //
                    if ((det.Prod.comb.ICMSCons.vBCICMSSTCons > 0) ||
                        (det.Prod.comb.ICMSCons.vICMSSTCons > 0) ||
                        (!string.IsNullOrEmpty(det.Prod.comb.ICMSCons.UFcons)))
                    {
                        XmlElement e2 = doc.CreateElement("ICMSCons");
                        e0.AppendChild(e2);
                        nodeCurrent = e2;

                        wCampo(det.Prod.comb.ICMSCons.vBCICMSSTCons, TpcnTipoCampo.tcDec2, Properties.Resources.vBCICMSSTCons);
                        wCampo(det.Prod.comb.ICMSCons.vICMSSTCons, TpcnTipoCampo.tcDec2, Properties.Resources.vICMSSTCons);
                        wCampo(det.Prod.comb.ICMSCons.UFcons, TpcnTipoCampo.tcStr, Properties.Resources.UFCons);
                        //if not ValidarUF(nfe.Det[i].Prod.comb.ICMSCons.UFcons) then
                        //  wAlerta("L120", Properties.Resources.UFcons, DSC_UFCONS, ERR_MSG_INVALIDO);
                    }
#endif
                }
                #endregion

                GerarDetImposto(NFe, det.Imposto, rootDet);
                //
                nodeCurrent = rootDet;
                wCampo(det.infAdProd, TpcnTipoCampo.tcStr, Properties.Resources.infAdProd, ObOp.Opcional);
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

            if (!string.IsNullOrEmpty(imposto.ISSQN.cSitTrib))
            {
                if ((imposto.ISSQN.vBC > 0) ||
                    (imposto.ISSQN.vAliq > 0) ||
                    (imposto.ISSQN.vISSQN > 0) ||
                    (imposto.ISSQN.cMunFG > 0) |
                    (imposto.ISSQN.cListServ > 0))
                {
                    nodeCurrent = doc.CreateElement("ISSQN");
                    nodeImposto.AppendChild(nodeCurrent);

                    wCampo(imposto.ISSQN.vBC, TpcnTipoCampo.tcDec2, Properties.Resources.vBC);
                    wCampo(imposto.ISSQN.vAliq, TpcnTipoCampo.tcDec2, Properties.Resources.vAliq);
                    wCampo(imposto.ISSQN.vISSQN, TpcnTipoCampo.tcDec2, Properties.Resources.vISSQN);
                    wCampo(imposto.ISSQN.cMunFG, TpcnTipoCampo.tcInt, Properties.Resources.cMunFG, ObOp.Obrigatorio, 7);
                    wCampo(imposto.ISSQN.cListServ, TpcnTipoCampo.tcInt, Properties.Resources.cListServ);
                    wCampo(imposto.ISSQN.cSitTrib, TpcnTipoCampo.tcStr, Properties.Resources.cSitTrib);
                }
            }
            else
            {
                GerarDetImpostoICMS(nfe, imposto, nodeImposto);
                GerarDetImpostoIPI(imposto.IPI, nodeImposto);
                if (nfe.det[0].Prod.DI.Count > 0)
                GerarDetImpostoII(imposto.II, nodeImposto);
            }
            GerarDetImpostoPIS(imposto.PIS, nodeImposto);
            GerarDetImpostoPISST(imposto.PISST, nodeImposto);
            GerarDetImpostoCOFINS(imposto.COFINS, nodeImposto);
            GerarDetImpostoCOFINSST(imposto.COFINSST, nodeImposto);
        }

        /// <summary>
        /// GerarDetImpostoCOFINS
        /// </summary>
        /// <param name="COFINS"></param>
        /// <param name="nodeImposto"></param>
        private void GerarDetImpostoCOFINS(COFINS COFINS, XmlElement nodeImposto)
        {
            if (!string.IsNullOrEmpty(COFINS.CST))
            {
                XmlElement node0 = doc.CreateElement("COFINS");

                switch (COFINS.CST)
                {
                    case "01":
                    case "02":
                        {
                            nodeCurrent = doc.CreateElement("COFINSAliq");
                            node0.AppendChild(nodeCurrent);
                            nodeImposto.AppendChild(node0);

                            wCampo(COFINS.CST, TpcnTipoCampo.tcStr, Properties.Resources.CST);
                            wCampo(COFINS.vBC, TpcnTipoCampo.tcDec2, Properties.Resources.vBC);
                            wCampo(COFINS.pCOFINS, TpcnTipoCampo.tcDec2, Properties.Resources.pCOFINS);
                            wCampo(COFINS.vCOFINS, TpcnTipoCampo.tcDec2, Properties.Resources.vCOFINS);
                        }
                        break;

                    case "03":
                        {
                            nodeCurrent = doc.CreateElement("COFINSQtde");
                            node0.AppendChild(nodeCurrent);
                            nodeImposto.AppendChild(node0);

                            wCampo(COFINS.CST, TpcnTipoCampo.tcStr, Properties.Resources.CST);
                            wCampo(COFINS.qBCProd, TpcnTipoCampo.tcDec4, Properties.Resources.qBCProd);
                            wCampo(COFINS.vAliqProd, TpcnTipoCampo.tcDec4, Properties.Resources.vAliqProd);
                            wCampo(COFINS.vCOFINS, TpcnTipoCampo.tcDec2, Properties.Resources.vCOFINS);
                        }
                        break;

                    case "04":
                    case "06":
                    case "07":
                    case "08":
                    case "09":
                        {
                            nodeCurrent = doc.CreateElement("COFINSNT");
                            node0.AppendChild(nodeCurrent);
                            nodeImposto.AppendChild(node0);

                            wCampo(COFINS.CST, TpcnTipoCampo.tcStr, Properties.Resources.CST);
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
                            if ((COFINS.vBC + COFINS.pCOFINS > 0) && (COFINS.qBCProd + COFINS.vAliqProd > 0))
                                this.cMensagemErro += "COFINSOutr: As TAG's <vBC> e <pCOFINS> não podem ser informadas em conjunto com as TAG <qBCProd> e <vAliqProd>" + Environment.NewLine;

                            nodeCurrent = doc.CreateElement("COFINSOutr");
                            node0.AppendChild(nodeCurrent);
                            nodeImposto.AppendChild(node0);

                            wCampo(COFINS.CST, TpcnTipoCampo.tcStr, Properties.Resources.CST);

                            if (COFINS.qBCProd + COFINS.vAliqProd > 0)
                            {
                                wCampo(COFINS.qBCProd, TpcnTipoCampo.tcDec4, Properties.Resources.qBCProd);
                                wCampo(COFINS.vAliqProd, TpcnTipoCampo.tcDec4, Properties.Resources.vAliqProd);
                            }
                            else
                            {
                                wCampo(COFINS.vBC, TpcnTipoCampo.tcDec2, Properties.Resources.vBC);
                                wCampo(COFINS.pCOFINS, TpcnTipoCampo.tcDec2, Properties.Resources.pCOFINS);
                            }
                            wCampo(COFINS.vCOFINS, TpcnTipoCampo.tcDec2, Properties.Resources.vCOFINS);
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
        private void GerarDetImpostoCOFINSST(COFINSST COFINSST, XmlElement nodeImposto)
        {
            if ((COFINSST.vBC > 0) ||
                (COFINSST.pCOFINS > 0) ||
                (COFINSST.qBCProd > 0) ||
                (COFINSST.vAliqProd > 0) ||
                (COFINSST.vCOFINS > 0))
            {
                if ((COFINSST.vBC + COFINSST.pCOFINS > 0) && (COFINSST.qBCProd + COFINSST.vAliqProd > 0))
                    this.cMensagemErro += "COFINSST: As TAG's <vBC> e <pCOFINS> não podem ser informadas em conjunto com as TAG <qBCProd> e <vAliqProd>" + Environment.NewLine;

                XmlElement node0 = doc.CreateElement("COFINSST");
                nodeCurrent = node0;

                if (COFINSST.vBC + COFINSST.pCOFINS > 0)
                {
                    nodeImposto.AppendChild(node0);

                    wCampo(COFINSST.vBC, TpcnTipoCampo.tcDec2, Properties.Resources.vBC);
                    wCampo(COFINSST.pCOFINS, TpcnTipoCampo.tcDec2, Properties.Resources.pCOFINS);
                    wCampo(COFINSST.vCOFINS, TpcnTipoCampo.tcDec2, Properties.Resources.vCOFINS);
                }
                if (COFINSST.qBCProd + COFINSST.vAliqProd > 0)
                {
                    nodeImposto.AppendChild(node0);

                    wCampo(COFINSST.qBCProd, TpcnTipoCampo.tcDec4, Properties.Resources.qBCProd);
                    wCampo(COFINSST.vAliqProd, TpcnTipoCampo.tcDec4, Properties.Resources.vAliqProd);
                    wCampo(COFINSST.vCOFINS, TpcnTipoCampo.tcDec2, Properties.Resources.vCOFINS);
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
            if (!string.IsNullOrEmpty(imposto.ICMS.CST))
            {
                if (imposto.ICMS.ICMSst == 1)
                {
                    XmlElement e0 = doc.CreateElement(Properties.Resources.ICMS);
                    nodeCurrent = doc.CreateElement("ICMSST");
                    e0.AppendChild(nodeCurrent);
                    nodeImposto.AppendChild(e0);

                    wCampo(imposto.ICMS.orig, TpcnTipoCampo.tcInt, Properties.Resources.orig);
                    wCampo(imposto.ICMS.CST, TpcnTipoCampo.tcStr, Properties.Resources.CST);
                    wCampo(imposto.ICMS.vBCSTRet, TpcnTipoCampo.tcDec2, Properties.Resources.vBCSTRet);
                    wCampo(imposto.ICMS.vICMSSTRet, TpcnTipoCampo.tcDec2, Properties.Resources.vICMSSTRet);
                    wCampo(imposto.ICMS.vBCSTDest, TpcnTipoCampo.tcDec2, Properties.Resources.vBCSTDest);
                    wCampo(imposto.ICMS.vICMSSTDest, TpcnTipoCampo.tcDec2, Properties.Resources.vICMSSTDest);
                }
                else
                {
                    XmlElement e0 = doc.CreateElement(Properties.Resources.ICMS);
                    if (imposto.ICMS.ICMSPart10 == 1 || imposto.ICMS.ICMSPart90 == 1)
                        nodeCurrent = doc.CreateElement("ICMSPart");
                    else
                        nodeCurrent = doc.CreateElement(Properties.Resources.ICMS + (imposto.ICMS.CST == "41" || imposto.ICMS.CST == "50" ? "40" : imposto.ICMS.CST));
                    e0.AppendChild(nodeCurrent);
                    nodeImposto.AppendChild(e0);

                    wCampo(imposto.ICMS.orig, TpcnTipoCampo.tcInt, Properties.Resources.orig);
                    wCampo(imposto.ICMS.CST, TpcnTipoCampo.tcStr, Properties.Resources.CST);

                    switch (imposto.ICMS.CST)
                    {
                        case "00":
                            wCampo(imposto.ICMS.modBC, TpcnTipoCampo.tcStr, Properties.Resources.modBC);
                            wCampo(imposto.ICMS.vBC, TpcnTipoCampo.tcDec2, Properties.Resources.vBC);
                            wCampo(imposto.ICMS.pICMS, TpcnTipoCampo.tcDec2, Properties.Resources.pICMS);
                            wCampo(imposto.ICMS.vICMS, TpcnTipoCampo.tcDec2, Properties.Resources.vICMS);
                            break;

                        case "10":
                            wCampo(imposto.ICMS.modBC, TpcnTipoCampo.tcStr, Properties.Resources.modBC);
                            if (imposto.ICMS.ICMSPart10 == 1)
                                wCampo(imposto.ICMS.pRedBC, TpcnTipoCampo.tcDec2, Properties.Resources.pRedBC, ObOp.Opcional);
                            wCampo(imposto.ICMS.vBC, TpcnTipoCampo.tcDec2, Properties.Resources.vBC);
                            wCampo(imposto.ICMS.pICMS, TpcnTipoCampo.tcDec2, Properties.Resources.pICMS);
                            wCampo(imposto.ICMS.vICMS, TpcnTipoCampo.tcDec2, Properties.Resources.vICMS);
                            wCampo(imposto.ICMS.modBCST, TpcnTipoCampo.tcStr, Properties.Resources.modBCST);
                            wCampo(imposto.ICMS.pMVAST, TpcnTipoCampo.tcDec2, Properties.Resources.pMVAST, ObOp.Opcional);
                            wCampo(imposto.ICMS.pRedBCST, TpcnTipoCampo.tcDec2, Properties.Resources.pRedBCST, ObOp.Opcional);
                            wCampo(imposto.ICMS.vBCST, TpcnTipoCampo.tcDec2, Properties.Resources.vBCST);
                            wCampo(imposto.ICMS.pICMSST, TpcnTipoCampo.tcDec2, Properties.Resources.pICMSST);
                            wCampo(imposto.ICMS.vICMSST, TpcnTipoCampo.tcDec2, Properties.Resources.vICMSST);
                            if (imposto.ICMS.ICMSPart10 == 1)
                            {
                                wCampo(imposto.ICMS.vBCOp, TpcnTipoCampo.tcDec2, Properties.Resources.vBCOp);
                                wCampo(imposto.ICMS.UFST, TpcnTipoCampo.tcStr, Properties.Resources.UFST);
                            }
                            break;

                        case "20":
                            wCampo(imposto.ICMS.modBC, TpcnTipoCampo.tcStr, Properties.Resources.modBC);
                            wCampo(imposto.ICMS.pRedBC, TpcnTipoCampo.tcDec2, Properties.Resources.pRedBC);
                            wCampo(imposto.ICMS.vBC, TpcnTipoCampo.tcDec2, Properties.Resources.vBC);
                            wCampo(imposto.ICMS.pICMS, TpcnTipoCampo.tcDec2, Properties.Resources.pICMS);
                            wCampo(imposto.ICMS.vICMS, TpcnTipoCampo.tcDec2, Properties.Resources.vICMS);
                            break;

                        case "30":
                            wCampo(imposto.ICMS.modBCST, TpcnTipoCampo.tcStr, Properties.Resources.modBCST);
                            wCampo(imposto.ICMS.pMVAST, TpcnTipoCampo.tcDec2, Properties.Resources.pMVAST, ObOp.Opcional);
                            wCampo(imposto.ICMS.pRedBCST, TpcnTipoCampo.tcDec2, Properties.Resources.pRedBCST, ObOp.Opcional);
                            wCampo(imposto.ICMS.vBCST, TpcnTipoCampo.tcDec2, Properties.Resources.vBCST);
                            wCampo(imposto.ICMS.pICMSST, TpcnTipoCampo.tcDec2, Properties.Resources.pICMSST);
                            wCampo(imposto.ICMS.vICMSST, TpcnTipoCampo.tcDec2, Properties.Resources.vICMSST);
                            break;

                        case "40":
                        case "41":
                        case "50":
                            wCampo(imposto.ICMS.vICMS, TpcnTipoCampo.tcDec2, Properties.Resources.vICMS, ObOp.Opcional);
                            if (imposto.ICMS.vICMS > 0)
                                wCampo(imposto.ICMS.motDesICMS, TpcnTipoCampo.tcInt, Properties.Resources.motDesICMS, ObOp.Opcional);
                            break;

                        case "51":
                            //Esse bloco fica a critério de cada UF a obrigação das informações, conforme o manual
                            wCampo(imposto.ICMS.modBC, TpcnTipoCampo.tcStr, Properties.Resources.modBC, ObOp.Opcional);
                            wCampo(imposto.ICMS.pRedBC, TpcnTipoCampo.tcDec2, Properties.Resources.pRedBC, ObOp.Opcional);
                            wCampo(imposto.ICMS.vBC, TpcnTipoCampo.tcDec2, Properties.Resources.vBC, ObOp.Opcional);
                            wCampo(imposto.ICMS.pICMS, TpcnTipoCampo.tcDec2, Properties.Resources.pICMS, ObOp.Opcional);
                            wCampo(imposto.ICMS.vICMS, TpcnTipoCampo.tcDec2, Properties.Resources.vICMS, ObOp.Opcional);
                            break;

                        case "60":
                            wCampo(imposto.ICMS.vBCSTRet, TpcnTipoCampo.tcDec2, Properties.Resources.vBCSTRet);
                            wCampo(imposto.ICMS.vICMSSTRet, TpcnTipoCampo.tcDec2, Properties.Resources.vICMSSTRet);
                            break;

                        case "70":
                            wCampo(imposto.ICMS.modBC, TpcnTipoCampo.tcStr, Properties.Resources.modBC);
                            wCampo(imposto.ICMS.pRedBC, TpcnTipoCampo.tcDec2, Properties.Resources.pRedBC);
                            wCampo(imposto.ICMS.vBC, TpcnTipoCampo.tcDec2, Properties.Resources.vBC);
                            wCampo(imposto.ICMS.pICMS, TpcnTipoCampo.tcDec2, Properties.Resources.pICMS);
                            wCampo(imposto.ICMS.vICMS, TpcnTipoCampo.tcDec2, Properties.Resources.vICMS);
                            wCampo(imposto.ICMS.modBCST, TpcnTipoCampo.tcStr, Properties.Resources.modBCST);
                            wCampo(imposto.ICMS.pMVAST, TpcnTipoCampo.tcDec2, Properties.Resources.pMVAST, ObOp.Opcional);
                            wCampo(imposto.ICMS.pRedBCST, TpcnTipoCampo.tcDec2, Properties.Resources.pRedBCST, ObOp.Opcional);
                            wCampo(imposto.ICMS.vBCST, TpcnTipoCampo.tcDec2, Properties.Resources.vBCST);
                            wCampo(imposto.ICMS.pICMSST, TpcnTipoCampo.tcDec2, Properties.Resources.pICMSST);
                            wCampo(imposto.ICMS.vICMSST, TpcnTipoCampo.tcDec2, Properties.Resources.vICMSST);
                            break;

                        case "90":
                            wCampo(imposto.ICMS.modBC, TpcnTipoCampo.tcStr, Properties.Resources.modBC);
                            if (imposto.ICMS.ICMSPart90 == 1)
                                wCampo(imposto.ICMS.pRedBC, TpcnTipoCampo.tcDec2, Properties.Resources.pRedBC, ObOp.Opcional);
                            wCampo(imposto.ICMS.vBC, TpcnTipoCampo.tcDec2, Properties.Resources.vBC);
                            wCampo(imposto.ICMS.pRedBC, TpcnTipoCampo.tcDec2, Properties.Resources.pRedBC, ObOp.Opcional);
                            wCampo(imposto.ICMS.pICMS, TpcnTipoCampo.tcDec2, Properties.Resources.pICMS);
                            wCampo(imposto.ICMS.vICMS, TpcnTipoCampo.tcDec2, Properties.Resources.vICMS);
                            wCampo(imposto.ICMS.modBCST, TpcnTipoCampo.tcStr, Properties.Resources.modBCST);
                            wCampo(imposto.ICMS.pMVAST, TpcnTipoCampo.tcDec2, Properties.Resources.pMVAST, ObOp.Opcional);
                            wCampo(imposto.ICMS.pRedBCST, TpcnTipoCampo.tcDec2, Properties.Resources.pRedBCST, ObOp.Opcional);
                            wCampo(imposto.ICMS.vBCST, TpcnTipoCampo.tcDec2, Properties.Resources.vBCST);
                            wCampo(imposto.ICMS.pICMSST, TpcnTipoCampo.tcDec2, Properties.Resources.pICMSST);
                            wCampo(imposto.ICMS.vICMSST, TpcnTipoCampo.tcDec2, Properties.Resources.vICMSST);
                            if (imposto.ICMS.ICMSPart90 == 1)
                            {
                                wCampo(imposto.ICMS.vBCOp, TpcnTipoCampo.tcDec2, Properties.Resources.vBCOp);
                                wCampo(imposto.ICMS.UFST, TpcnTipoCampo.tcStr, Properties.Resources.UFST);
                            }
                            break;
                    }
                }
            }

            if (imposto.ICMS.CSOSN > 100)
            {
                XmlElement e0 = doc.CreateElement(Properties.Resources.ICMS);
                switch (imposto.ICMS.CSOSN)
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
                }
                e0.AppendChild(nodeCurrent);
                nodeImposto.AppendChild(e0);

                wCampo(imposto.ICMS.orig, TpcnTipoCampo.tcInt, Properties.Resources.orig);
                wCampo(imposto.ICMS.CSOSN, TpcnTipoCampo.tcInt, Properties.Resources.CSOSN);

                switch (imposto.ICMS.CSOSN)
                {
                    case 101:
                        wCampo(imposto.ICMS.pCredSN, TpcnTipoCampo.tcDec2, Properties.Resources.pCredSN);
                        wCampo(imposto.ICMS.vCredICMSSN, TpcnTipoCampo.tcDec2, Properties.Resources.vCredICMSSN);
                        break;
                    case 201:
                        wCampo(imposto.ICMS.modBCST, TpcnTipoCampo.tcStr, Properties.Resources.modBCST);
                        wCampo(imposto.ICMS.pMVAST, TpcnTipoCampo.tcDec2, Properties.Resources.pMVAST, ObOp.Opcional);
                        wCampo(imposto.ICMS.pRedBCST, TpcnTipoCampo.tcDec2, Properties.Resources.pRedBCST, ObOp.Opcional);
                        wCampo(imposto.ICMS.vBCST, TpcnTipoCampo.tcDec2, Properties.Resources.vBCST);
                        wCampo(imposto.ICMS.pICMSST, TpcnTipoCampo.tcDec2, Properties.Resources.pICMSST);
                        wCampo(imposto.ICMS.vICMSST, TpcnTipoCampo.tcDec2, Properties.Resources.vICMSST);
                        wCampo(imposto.ICMS.pCredSN, TpcnTipoCampo.tcDec2, Properties.Resources.pCredSN);
                        wCampo(imposto.ICMS.vCredICMSSN, TpcnTipoCampo.tcDec2, Properties.Resources.vCredICMSSN);
                        break;
                    case 202:
                    case 203:
                        wCampo(imposto.ICMS.modBCST, TpcnTipoCampo.tcStr, Properties.Resources.modBCST);
                        wCampo(imposto.ICMS.pMVAST, TpcnTipoCampo.tcDec2, Properties.Resources.pMVAST, ObOp.Opcional);
                        wCampo(imposto.ICMS.pRedBCST, TpcnTipoCampo.tcDec2, Properties.Resources.pRedBCST, ObOp.Opcional);
                        wCampo(imposto.ICMS.vBCST, TpcnTipoCampo.tcDec2, Properties.Resources.vBCST);
                        wCampo(imposto.ICMS.pICMSST, TpcnTipoCampo.tcDec2, Properties.Resources.pICMSST);
                        wCampo(imposto.ICMS.vICMSST, TpcnTipoCampo.tcDec2, Properties.Resources.vICMSST);
                        break;
                    case 500:
                        //wCampo(imposto.ICMS.modBCST, TpcnTipoCampo.tcStr, Properties.Resources.modBCST, false);
                        wCampo(imposto.ICMS.vBCSTRet, TpcnTipoCampo.tcDec2, Properties.Resources.vBCSTRet);
                        wCampo(imposto.ICMS.vICMSSTRet, TpcnTipoCampo.tcDec2, Properties.Resources.vICMSSTRet);
                        break;
                    case 900:
                        wCampo(imposto.ICMS.modBC, TpcnTipoCampo.tcStr, Properties.Resources.modBC);
                        wCampo(imposto.ICMS.vBC, TpcnTipoCampo.tcDec2, Properties.Resources.vBC);
                        wCampo(imposto.ICMS.pRedBC, TpcnTipoCampo.tcDec2, Properties.Resources.pRedBC, ObOp.Opcional);
                        wCampo(imposto.ICMS.pICMS, TpcnTipoCampo.tcDec2, Properties.Resources.pICMS);
                        wCampo(imposto.ICMS.vICMS, TpcnTipoCampo.tcDec2, Properties.Resources.vICMS);
                        wCampo(imposto.ICMS.modBCST, TpcnTipoCampo.tcStr, Properties.Resources.modBCST);
                        wCampo(imposto.ICMS.pMVAST, TpcnTipoCampo.tcDec2, Properties.Resources.pMVAST, ObOp.Opcional);
                        wCampo(imposto.ICMS.pRedBCST, TpcnTipoCampo.tcDec2, Properties.Resources.pRedBCST, ObOp.Opcional);
                        wCampo(imposto.ICMS.vBCST, TpcnTipoCampo.tcDec2, Properties.Resources.vBCST);
                        wCampo(imposto.ICMS.pICMSST, TpcnTipoCampo.tcDec2, Properties.Resources.pICMSST);
                        wCampo(imposto.ICMS.vICMSST, TpcnTipoCampo.tcDec2, Properties.Resources.vICMSST);
                        wCampo(imposto.ICMS.pCredSN, TpcnTipoCampo.tcDec2, Properties.Resources.pCredSN);
                        wCampo(imposto.ICMS.vCredICMSSN, TpcnTipoCampo.tcDec2, Properties.Resources.vCredICMSSN);
                        break;
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

                wCampo(II.vBC, TpcnTipoCampo.tcDec2, Properties.Resources.vBC);
                wCampo(II.vDespAdu, TpcnTipoCampo.tcDec2, Properties.Resources.vDespAdu);
                wCampo(II.vII, TpcnTipoCampo.tcDec2, Properties.Resources.vII);
                wCampo(II.vIOF, TpcnTipoCampo.tcDec2, Properties.Resources.vIOF);
            }
        }

        /// <summary>
        /// GerarDetImpostoIPI
        /// </summary>
        /// <param name="IPI"></param>
        /// <param name="nodeImposto"></param>
        private void GerarDetImpostoIPI(IPI IPI, XmlElement nodeImposto)
        {
            if (!string.IsNullOrEmpty(IPI.CST))
            {
                bool CST00495099;

                // variavel CST00495099 usada para Ignorar Tag <IPI>
                // se GerarTagIPIparaNaoTributado = False e CST00495099 = False

                CST00495099 = (IPI.CST == "00" || IPI.CST == "49" || IPI.CST == "50" || IPI.CST == "99");

                XmlElement e0 = doc.CreateElement("IPI");
                nodeImposto.AppendChild(e0);
                nodeCurrent = e0;

                wCampo(IPI.clEnq, TpcnTipoCampo.tcStr, Properties.Resources.clEnq, ObOp.Opcional);
                wCampo(IPI.CNPJProd, TpcnTipoCampo.tcStr, Properties.Resources.CNPJProd, ObOp.Opcional);
                wCampo(IPI.cSelo, TpcnTipoCampo.tcStr, Properties.Resources.cSelo, ObOp.Opcional);
                wCampo(IPI.qSelo, TpcnTipoCampo.tcInt, Properties.Resources.qSelo, ObOp.Opcional);
                if (IPI.cEnq.Trim() == "")
                    IPI.cEnq = "999";
                wCampo(IPI.cEnq, TpcnTipoCampo.tcStr, Properties.Resources.cEnq);

                if (CST00495099)
                {
                    if ((IPI.vBC + IPI.pIPI > 0) && (IPI.qUnid + IPI.vUnid > 0))
                        this.cMensagemErro += "IPITrib: As TAG's <vBC> e <pIPI> não podem ser informadas em conjunto com as TAG <qUnid> e <vUnid>" + Environment.NewLine;

                    nodeCurrent = doc.CreateElement("IPITrib");
                    e0.AppendChild(nodeCurrent);

                    wCampo(IPI.CST, TpcnTipoCampo.tcStr, Properties.Resources.CST);
                    if (IPI.qUnid + IPI.vUnid > 0)
                    {
                        wCampo(IPI.qUnid, TpcnTipoCampo.tcDec4, Properties.Resources.qUnid);
                        wCampo(IPI.vUnid, TpcnTipoCampo.tcDec4, Properties.Resources.vUnid);
                    }
                    else
                    {
                        wCampo(IPI.vBC, TpcnTipoCampo.tcDec2, Properties.Resources.vBC);
                        wCampo(IPI.pIPI, TpcnTipoCampo.tcDec2, Properties.Resources.pIPI);
                    }
                    wCampo(IPI.vIPI, TpcnTipoCampo.tcDec2, Properties.Resources.vIPI);
                }
                else //(* Quando CST/IPI for 01,02,03,04,51,52,53,54 ou 55 *)
                {
                    nodeCurrent = doc.CreateElement("IPINT");
                    e0.AppendChild(nodeCurrent);
                    wCampo(IPI.CST, TpcnTipoCampo.tcStr, Properties.Resources.CST);
                }
            }
        }

        /// <summary>
        /// GerarDetImpostoPIS
        /// </summary>
        /// <param name="PIS"></param>
        /// <param name="nodeImposto"></param>
        private void GerarDetImpostoPIS(PIS PIS, XmlElement nodeImposto)
        {
            if (!string.IsNullOrEmpty(PIS.CST))
            {
                XmlElement e0 = doc.CreateElement("PIS");

                switch (PIS.CST)
                {
                    case "01":
                    case "02":
                        nodeCurrent = doc.CreateElement("PISAliq");
                        e0.AppendChild(nodeCurrent);
                        nodeImposto.AppendChild(e0);
                        wCampo(PIS.CST, TpcnTipoCampo.tcStr, Properties.Resources.CST);
                        wCampo(PIS.vBC, TpcnTipoCampo.tcDec2, Properties.Resources.vBC);
                        wCampo(PIS.pPIS, TpcnTipoCampo.tcDec2, Properties.Resources.pPIS);
                        wCampo(PIS.vPIS, TpcnTipoCampo.tcDec2, Properties.Resources.vPIS);
                        break;

                    case "03":
                        nodeCurrent = doc.CreateElement("PISQtde");
                        e0.AppendChild(nodeCurrent);
                        nodeImposto.AppendChild(e0);
                        wCampo(PIS.CST, TpcnTipoCampo.tcStr, Properties.Resources.CST);
                        wCampo(PIS.qBCProd, TpcnTipoCampo.tcDec4, Properties.Resources.qBCProd);
                        wCampo(PIS.vAliqProd, TpcnTipoCampo.tcDec4, Properties.Resources.vAliqProd);
                        wCampo(PIS.vPIS, TpcnTipoCampo.tcDec2, Properties.Resources.vPIS);
                        break;

                    case "04":
                    case "06":
                    case "07":
                    case "08":
                    case "09":
                        nodeCurrent = doc.CreateElement("PISNT");
                        e0.AppendChild(nodeCurrent);
                        nodeImposto.AppendChild(e0);
                        wCampo(PIS.CST, TpcnTipoCampo.tcStr, Properties.Resources.CST);
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
                        if ((PIS.vBC + PIS.pPIS > 0) && (PIS.qBCProd + PIS.vAliqProd > 0))
                        {
                            this.cMensagemErro += "PIS: As TAG's <vBC> e <pPIS> não podem ser informadas em conjunto com as TAG <qBCProd> e <vAliqProd>" + Environment.NewLine;
                        }

                        nodeCurrent = doc.CreateElement(Properties.Resources.PISOutr);
                        e0.AppendChild(nodeCurrent);
                        nodeImposto.AppendChild(e0);

                        wCampo(PIS.CST, TpcnTipoCampo.tcStr, Properties.Resources.CST);
                        if (PIS.qBCProd + PIS.vAliqProd > 0)
                        {
                            wCampo(PIS.qBCProd, TpcnTipoCampo.tcDec4, Properties.Resources.qBCProd);
                            wCampo(PIS.vAliqProd, TpcnTipoCampo.tcDec4, Properties.Resources.vAliqProd);
                        }
                        else
                        {
                            wCampo(PIS.vBC, TpcnTipoCampo.tcDec2, Properties.Resources.vBC);
                            wCampo(PIS.pPIS, TpcnTipoCampo.tcDec2, Properties.Resources.pPIS);
                        }
                        wCampo(PIS.vPIS, TpcnTipoCampo.tcDec2, Properties.Resources.vPIS);
                        break;
                }
            }
        }

        /// <summary>
        /// GerarDetImpostoPISST
        /// </summary>
        /// <param name="PISST"></param>
        /// <param name="nodeImposto"></param>
        private void GerarDetImpostoPISST(PISST PISST, XmlElement nodeImposto)
        {
            if ((PISST.vBC > 0) ||
              (PISST.pPis > 0) ||
              (PISST.qBCProd > 0) ||
              (PISST.vAliqProd > 0) ||
              (PISST.vPIS > 0))
            {
                if ((PISST.vBC + PISST.pPis > 0) && (PISST.qBCProd + PISST.vAliqProd > 0))
                    this.cMensagemErro += "PISST: As TAG's <vBC> e <pPIS> não podem ser informadas em conjunto com as TAG <qBCProd> e <vAliqProd>)" + Environment.NewLine;

                if (PISST.vBC + PISST.pPis > 0)
                {
                    nodeCurrent = doc.CreateElement("PISST");
                    nodeImposto.AppendChild(nodeCurrent);

                    wCampo(PISST.vBC, TpcnTipoCampo.tcDec2, Properties.Resources.vBC);
                    wCampo(PISST.pPis, TpcnTipoCampo.tcDec2, Properties.Resources.pPIS);
                    wCampo(PISST.vPIS, TpcnTipoCampo.tcDec2, Properties.Resources.vPIS);
                }
                if (PISST.qBCProd + PISST.vAliqProd > 0)
                {
                    nodeCurrent = doc.CreateElement("PISST");
                    nodeImposto.AppendChild(nodeCurrent);
                    wCampo(PISST.qBCProd, TpcnTipoCampo.tcDec4, Properties.Resources.qBCProd);
                    wCampo(PISST.vAliqProd, TpcnTipoCampo.tcDec4, Properties.Resources.vAliqProd);
                    wCampo(PISST.vPIS, TpcnTipoCampo.tcDec2, Properties.Resources.vPIS);
                }
            }
        }

        /// <summary>
        /// GerarDigito
        /// </summary>
        /// <param name="chave"></param>
        /// <returns></returns>
        public Int32 GerarDigito(string chave)
        {
            int i, j, Digito;
            const string PESO = "4329876543298765432987654329876543298765432";

            chave = chave.Replace("NFe", "");
            if (chave.Length != 43)
            {
                this.cMensagemErro += string.Format("Erro na composição da chave [{0}] para obter o DV", chave) + Environment.NewLine;
                return 0;
            }
            else
            {
                // Manual Integracao Contribuinte v2.02a - Página: 70 //
                j = 0;
                Digito = -1;
                try
                {
                    for (i = 0; i < 43; ++i)
                        j += Convert.ToInt32(chave.Substring(i, 1)) * Convert.ToInt32(PESO.Substring(i, 1));
                    Digito = 11 - (j % 11);
                    if ((j % 11) < 2)
                        Digito = 0;
                }
                catch
                {
                    Digito = -1;
                }
                if (Digito == -1)
                    this.cMensagemErro += string.Format("Erro no cálculo do DV da chave [{0}]", chave) + Environment.NewLine;
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

            if (string.IsNullOrEmpty(NFe.emit.CNPJ) && string.IsNullOrEmpty(NFe.emit.CPF))
                throw new Exception("CNPJ/CPF inválido no segmento [C]");

            if (!string.IsNullOrEmpty(NFe.emit.CNPJ))
                wCampo(NFe.emit.CNPJ, TpcnTipoCampo.tcStr, Properties.Resources.CNPJ);
            else
                wCampo(NFe.emit.CPF, TpcnTipoCampo.tcStr, Properties.Resources.CPF);
            wCampo(NFe.emit.xNome, TpcnTipoCampo.tcStr, Properties.Resources.xNome);
            wCampo(NFe.emit.xFant, TpcnTipoCampo.tcStr, Properties.Resources.xFant, ObOp.Opcional);
            ///
            /// <enderEmit>
            /// 
            XmlElement el = doc.CreateElement("enderEmit");
            nodeCurrent.AppendChild(el);
            nodeCurrent = el;
            wCampo(NFe.emit.enderEmit.xLgr, TpcnTipoCampo.tcStr, Properties.Resources.xLgr);
            wCampo(NFe.emit.enderEmit.nro, TpcnTipoCampo.tcStr, Properties.Resources.nro);
            wCampo(NFe.emit.enderEmit.xCpl, TpcnTipoCampo.tcStr, Properties.Resources.xCpl, ObOp.Opcional);
            wCampo(NFe.emit.enderEmit.xBairro, TpcnTipoCampo.tcStr, Properties.Resources.xBairro);
            wCampo(NFe.emit.enderEmit.cMun, TpcnTipoCampo.tcInt, Properties.Resources.cMun, ObOp.Obrigatorio, 7);
            wCampo(NFe.emit.enderEmit.xMun, TpcnTipoCampo.tcStr, Properties.Resources.xMun);
            wCampo(NFe.emit.enderEmit.UF, TpcnTipoCampo.tcStr, Properties.Resources.UF);
            wCampo(NFe.emit.enderEmit.CEP, TpcnTipoCampo.tcInt, Properties.Resources.CEP, ObOp.Opcional, 8);
            wCampo(NFe.emit.enderEmit.cPais, TpcnTipoCampo.tcInt, Properties.Resources.cPais, ObOp.Opcional);
            wCampo(NFe.emit.enderEmit.xPais, TpcnTipoCampo.tcStr, Properties.Resources.xPais, ObOp.Opcional);
            wCampo(NFe.emit.enderEmit.fone, TpcnTipoCampo.tcStr, Properties.Resources.fone, ObOp.Opcional);
            ///
            /// </enderEmit>
            /// 
            nodeCurrent = ELemit;
            wCampo(NFe.emit.IE, TpcnTipoCampo.tcStr, Properties.Resources.IE);
            wCampo(NFe.emit.IEST, TpcnTipoCampo.tcStr, Properties.Resources.IEST, ObOp.Opcional);
            wCampo(NFe.emit.IM, TpcnTipoCampo.tcStr, Properties.Resources.IM, ObOp.Opcional);
            if (NFe.emit.IM.Length > 0)
                wCampo(NFe.emit.CNAE, TpcnTipoCampo.tcStr, Properties.Resources.CNAE, ObOp.Opcional);
            wCampo(NFe.emit.CRT, TpcnTipoCampo.tcInt, Properties.Resources.CRT);

            return ELemit;
        }

        /// <summary>
        /// GerarEntrega
        /// </summary>
        /// <param name="NFe"></param>
        /// <param name="root"></param>
        private void GerarEntrega(NFe NFe, XmlElement root)
        {
            if (!string.IsNullOrEmpty(NFe.entrega.xLgr))
            {
                XmlElement e0 = doc.CreateElement("entrega");
                root.AppendChild(e0);
                nodeCurrent = e0;

                if (string.IsNullOrEmpty(NFe.entrega.CNPJ) && string.IsNullOrEmpty(NFe.entrega.CPF))
                    throw new Exception("CNPJ/CPF inválido no segmento [F]");

                if (!string.IsNullOrEmpty(NFe.entrega.CNPJ))
                    wCampo(NFe.entrega.CNPJ, TpcnTipoCampo.tcStr, Properties.Resources.CNPJ);
                else
                    wCampo(NFe.entrega.CPF, TpcnTipoCampo.tcStr, Properties.Resources.CPF);
                wCampo(NFe.entrega.xLgr, TpcnTipoCampo.tcStr, Properties.Resources.xLgr);
                wCampo(NFe.entrega.nro, TpcnTipoCampo.tcStr, Properties.Resources.nro);
                wCampo(NFe.entrega.xCpl, TpcnTipoCampo.tcStr, Properties.Resources.xCpl, ObOp.Opcional);
                wCampo(NFe.entrega.xBairro, TpcnTipoCampo.tcStr, Properties.Resources.xBairro);
                wCampo(NFe.entrega.cMun, TpcnTipoCampo.tcInt, Properties.Resources.cMun, ObOp.Obrigatorio, 7);
                wCampo(NFe.entrega.xMun, TpcnTipoCampo.tcStr, Properties.Resources.xMun);
                wCampo(NFe.entrega.UF, TpcnTipoCampo.tcStr, Properties.Resources.UF);
            }
        }

        /// <summary>
        /// GerarExporta
        /// </summary>
        /// <param name="exporta"></param>
        /// <param name="root"></param>
        private void GerarExporta(Exporta exporta, XmlElement root)
        {
            if (!string.IsNullOrEmpty(exporta.UFEmbarq) || !string.IsNullOrEmpty(exporta.xLocEmbarq))
            {
                nodeCurrent = doc.CreateElement("exporta");
                root.AppendChild(nodeCurrent);

                wCampo(exporta.UFEmbarq, TpcnTipoCampo.tcStr, Properties.Resources.UFEmbarq);
                wCampo(exporta.xLocEmbarq, TpcnTipoCampo.tcStr, Properties.Resources.xLocEmbarq);
            }
        }

        /// <summary>
        /// GerarInfAdic
        /// </summary>
        /// <param name="InfAdic"></param>
        /// <param name="root"></param>
        private void GerarInfAdic(InfAdic InfAdic, XmlElement root)
        {
            if ((!string.IsNullOrEmpty(InfAdic.infAdFisco)) ||
                (!string.IsNullOrEmpty(InfAdic.infCpl)) ||
                (InfAdic.obsCont.Count > 1) ||
                (InfAdic.obsFisco.Count > 1) ||
                (InfAdic.procRef.Count > 1))
            {
                XmlElement nodeinfAdic = doc.CreateElement("infAdic");
                root.AppendChild(nodeinfAdic);
                nodeCurrent = nodeinfAdic;

                wCampo(InfAdic.infAdFisco, TpcnTipoCampo.tcStr, Properties.Resources.infAdFisco, ObOp.Opcional);
                wCampo(InfAdic.infCpl, TpcnTipoCampo.tcStr, Properties.Resources.infCpl);
                //
                //(**)GerarInfAdicObsCont;
                //
                if (InfAdic.obsCont.Count > 10)
                    this.cMensagemErro += "obsCont: Excedeu o máximo permitido de 10" + Environment.NewLine;

                foreach (obsCont obsCont in InfAdic.obsCont)
                {
                    XmlElement nodeobsCont = doc.CreateElement("obsCont");
                    XmlAttribute xmlItem = doc.CreateAttribute(Properties.Resources.xCampo);
                    xmlItem.Value = obsCont.xCampo;
                    nodeobsCont.Attributes.Append(xmlItem);
                    nodeinfAdic.AppendChild(nodeobsCont);
                    nodeCurrent = nodeobsCont;
                    wCampo(obsCont.xTexto, TpcnTipoCampo.tcStr, Properties.Resources.xTexto);
                }
                //
                //(**)GerarInfAdicObsFisco;
                //
                if (InfAdic.obsFisco.Count > 10)
                    this.cMensagemErro += "obsFisco: Excedeu o máximo permitido de 10" + Environment.NewLine;

                foreach (obsFisco obsFisco in InfAdic.obsFisco)
                {
                    XmlElement nodeobsFisco = doc.CreateElement("obsFisco");
                    XmlAttribute xmlItem = doc.CreateAttribute(Properties.Resources.xCampo);
                    xmlItem.Value = obsFisco.xCampo;
                    nodeobsFisco.Attributes.Append(xmlItem);
                    nodeinfAdic.AppendChild(nodeobsFisco);
                    nodeCurrent = nodeobsFisco;
                    wCampo(obsFisco.xTexto, TpcnTipoCampo.tcStr, Properties.Resources.xTexto);
                }
                //
                //(**)GerarInfAdicProcRef;
                //
                foreach (procRef procRef in InfAdic.procRef)
                {
                    XmlElement nodeprocRef = doc.CreateElement("procRef");
                    nodeinfAdic.AppendChild(nodeprocRef);
                    nodeCurrent = nodeprocRef;

                    wCampo(procRef.nProc, TpcnTipoCampo.tcStr, Properties.Resources.nProc);
                    wCampo(procRef.indProc, TpcnTipoCampo.tcStr, Properties.Resources.indProc);
                }
            }
        }

        /// <summary>
        /// GerarInfNFe
        /// </summary>
        /// <param name="Nfe"></param>
        /// <returns></returns>
        private XmlElement GerarInfNFe(NFe Nfe)
        {
            XmlElement ELide = doc.CreateElement("ide");

            nodeCurrent = ELide;
            wCampo(Nfe.ide.cUF, TpcnTipoCampo.tcInt, Properties.Resources.cUF, ObOp.Obrigatorio, 0);
            wCampo(Nfe.ide.cNF, TpcnTipoCampo.tcInt, Properties.Resources.cNF, ObOp.Obrigatorio, 8);
            wCampo(Nfe.ide.natOp, TpcnTipoCampo.tcStr, Properties.Resources.natOp, ObOp.Obrigatorio, 0);
            wCampo((int)Nfe.ide.indPag, TpcnTipoCampo.tcInt, Properties.Resources.indPag, ObOp.Obrigatorio, 0);
            wCampo(Nfe.ide.mod, TpcnTipoCampo.tcInt, Properties.Resources.mod, ObOp.Obrigatorio, 0);
            wCampo(Nfe.ide.serie, TpcnTipoCampo.tcInt, Properties.Resources.serie, ObOp.Obrigatorio, 0);
            wCampo(Nfe.ide.nNF, TpcnTipoCampo.tcInt, Properties.Resources.nNF, ObOp.Obrigatorio, 0);
            wCampo(Nfe.ide.dEmi, TpcnTipoCampo.tcDat, Properties.Resources.dEmi, ObOp.Obrigatorio, 0);
            wCampo(Nfe.ide.dSaiEnt, TpcnTipoCampo.tcDat, Properties.Resources.dSaiEnt, ObOp.Opcional, 0);
            wCampo(Nfe.ide.hSaiEnt, TpcnTipoCampo.tcHor, Properties.Resources.hSaiEnt, ObOp.Opcional, 0);
            wCampo((int)Nfe.ide.tpNF, TpcnTipoCampo.tcInt, Properties.Resources.tpNF, ObOp.Obrigatorio, 0);
            wCampo(Nfe.ide.cMunFG, TpcnTipoCampo.tcInt, Properties.Resources.cMunFG, ObOp.Obrigatorio, 0);

            // Gera TAGs referentes a NFe referência
            foreach (NFref refNFe in Nfe.ide.NFref)
            {
                if (!string.IsNullOrEmpty(refNFe.refNFe))
                {
                    XmlElement ep = doc.CreateElement(Properties.Resources.NFref);
                    XmlElement el = doc.CreateElement(Properties.Resources.refNFe);
                    el.InnerText = refNFe.refNFe;
                    ep.AppendChild(el);
                    ELide.AppendChild(ep);
                }
            }
            // Gera TAGs se NÃO for uma NFe referência
            foreach (NFref refNFe in Nfe.ide.NFref)
            {
                if (refNFe.refNF != null)
                {
                    if (refNFe.refNF.nNF > 0)
                    {
                        XmlElement ep = doc.CreateElement(Properties.Resources.NFref);
                        XmlElement el = doc.CreateElement("refNF");
                        ep.AppendChild(el);
                        ELide.AppendChild(ep);
                        nodeCurrent = el;

                        wCampo(refNFe.refNF.cUF, TpcnTipoCampo.tcInt, Properties.Resources.cUF, ObOp.Obrigatorio, 2);
                        wCampo(refNFe.refNF.AAMM, TpcnTipoCampo.tcStr, Properties.Resources.AAMM, ObOp.Obrigatorio, 0);
                        wCampo(refNFe.refNF.CNPJ, TpcnTipoCampo.tcStr, Properties.Resources.CNPJ, ObOp.Obrigatorio, 0);
                        wCampo(refNFe.refNF.mod, TpcnTipoCampo.tcStr, Properties.Resources.mod, ObOp.Obrigatorio, 2);
                        wCampo(refNFe.refNF.serie, TpcnTipoCampo.tcInt, Properties.Resources.serie, ObOp.Obrigatorio, 0);
                        wCampo(refNFe.refNF.nNF, TpcnTipoCampo.tcInt, Properties.Resources.nNF, ObOp.Obrigatorio, 0);
                    }
                }
            }
            foreach (NFref refNFe in Nfe.ide.NFref)
            {
                if (refNFe.refNFP != null)
                {
                    if (refNFe.refNFP.nNF > 0)
                    {
                        XmlElement ep = doc.CreateElement(Properties.Resources.NFref);
                        XmlElement el = doc.CreateElement("refNFP");
                        ep.AppendChild(el);
                        ELide.AppendChild(ep);
                        nodeCurrent = el;

                        wCampo(refNFe.refNFP.cUF, TpcnTipoCampo.tcInt, Properties.Resources.cUF, ObOp.Obrigatorio, 2);
                        wCampo(refNFe.refNFP.AAMM, TpcnTipoCampo.tcStr, Properties.Resources.AAMM, ObOp.Obrigatorio, 0);
                        if (!string.IsNullOrEmpty(refNFe.refNFP.CNPJ))
                            wCampo(refNFe.refNFP.CNPJ, TpcnTipoCampo.tcStr, Properties.Resources.CNPJ, ObOp.Obrigatorio, 0);
                        else
                            if (!string.IsNullOrEmpty(refNFe.refNFP.CPF))
                                wCampo(refNFe.refNFP.CPF, TpcnTipoCampo.tcStr, Properties.Resources.CPF, ObOp.Obrigatorio, 0);
                        wCampo(refNFe.refNFP.IE, TpcnTipoCampo.tcStr, Properties.Resources.IE, ObOp.Opcional, 0);
                        wCampo(refNFe.refNFP.mod, TpcnTipoCampo.tcStr, Properties.Resources.mod, ObOp.Obrigatorio, 2);
                        wCampo(refNFe.refNFP.serie, TpcnTipoCampo.tcInt, Properties.Resources.serie, ObOp.Obrigatorio, 0);
                        wCampo(refNFe.refNFP.nNF, TpcnTipoCampo.tcInt, Properties.Resources.nNF, ObOp.Obrigatorio, 0);
                    }
                }
            }
            foreach (NFref refNFe in Nfe.ide.NFref)
            {
                if (!string.IsNullOrEmpty(refNFe.refCTe))
                {
                    XmlElement ep = doc.CreateElement(Properties.Resources.NFref);
                    XmlElement el = doc.CreateElement(Properties.Resources.refCTe);
                    el.InnerText = refNFe.refCTe;
                    ep.AppendChild(el);
                    ELide.AppendChild(ep);
                }
            }
            foreach (NFref refNFe in Nfe.ide.NFref)
            {
                if (refNFe.refECF != null)
                {
                    if (refNFe.refECF.nCOO > 0)
                    {
                        XmlElement ep = doc.CreateElement(Properties.Resources.NFref);
                        XmlElement el = doc.CreateElement("refECF");
                        ep.AppendChild(el);
                        ELide.AppendChild(ep);
                        nodeCurrent = el;

                        wCampo(refNFe.refECF.mod, TpcnTipoCampo.tcStr, Properties.Resources.mod, ObOp.Obrigatorio, 0);
                        wCampo(refNFe.refECF.nECF, TpcnTipoCampo.tcInt, Properties.Resources.nECF, ObOp.Obrigatorio, 3);
                        wCampo(refNFe.refECF.nCOO, TpcnTipoCampo.tcInt, Properties.Resources.nCOO, ObOp.Obrigatorio, 0);
                    }
                }
            }
            nodeCurrent = ELide;
            wCampo((int)Nfe.ide.tpImp, TpcnTipoCampo.tcInt, Properties.Resources.tpImp, ObOp.Obrigatorio, 0);
            wCampo((int)Nfe.ide.tpEmis, TpcnTipoCampo.tcInt, Properties.Resources.tpEmis, ObOp.Obrigatorio, 0);
            wCampo(Nfe.ide.cDV, TpcnTipoCampo.tcInt, Properties.Resources.cDV, ObOp.Obrigatorio, 0);
            wCampo((int)Nfe.ide.tpAmb, TpcnTipoCampo.tcInt, Properties.Resources.tpAmb, ObOp.Obrigatorio, 0);
            wCampo((int)Nfe.ide.finNFe, TpcnTipoCampo.tcInt, Properties.Resources.finNFe, ObOp.Obrigatorio, 0);
            wCampo(Nfe.ide.procEmi, TpcnTipoCampo.tcStr, Properties.Resources.procEmi, ObOp.Obrigatorio, 0);
            wCampo(Nfe.ide.verProc, TpcnTipoCampo.tcStr, Properties.Resources.verProc, ObOp.Obrigatorio, 0);
            wCampo(Nfe.ide.dhCont, TpcnTipoCampo.tcDatHor, Properties.Resources.dhCont, ObOp.Opcional, 0);
            wCampo(Nfe.ide.xJust, TpcnTipoCampo.tcStr, Properties.Resources.xJust, ObOp.Opcional, 0);

            return ELide;
        }

        /// <summary>
        /// GerarRetirada
        /// </summary>
        /// <param name="NFe"></param>
        /// <param name="root"></param>
        private void GerarRetirada(NFe NFe, XmlElement root)
        {
            if (!string.IsNullOrEmpty(NFe.retirada.xLgr))
            {
                XmlElement el = doc.CreateElement("retirada");
                root.AppendChild(el);
                nodeCurrent = el;

                if (!string.IsNullOrEmpty(NFe.retirada.CNPJ))
                    wCampo(NFe.retirada.CNPJ, TpcnTipoCampo.tcStr, Properties.Resources.CNPJ);
                else
                    wCampo(NFe.retirada.CPF, TpcnTipoCampo.tcStr, Properties.Resources.CPF);
                wCampo(NFe.retirada.xLgr, TpcnTipoCampo.tcStr, Properties.Resources.xLgr);
                wCampo(NFe.retirada.nro, TpcnTipoCampo.tcStr, Properties.Resources.nro);
                wCampo(NFe.retirada.xCpl, TpcnTipoCampo.tcStr, Properties.Resources.xCpl, ObOp.Opcional);
                wCampo(NFe.retirada.xBairro, TpcnTipoCampo.tcStr, Properties.Resources.xBairro);
                wCampo(NFe.retirada.cMun, TpcnTipoCampo.tcInt, Properties.Resources.cMun, ObOp.Obrigatorio, 7);
                wCampo(NFe.retirada.xMun, TpcnTipoCampo.tcStr, Properties.Resources.xMun);
                wCampo(NFe.retirada.UF, TpcnTipoCampo.tcStr, Properties.Resources.UF);
            }
        }

        /// <summary>
        /// GerarTransp
        /// </summary>
        /// <param name="Transp"></param>
        /// <param name="root"></param>
        private void GerarTransp(Transp Transp, XmlElement root)
        {
            XmlElement nodeTransp = doc.CreateElement("transp");
            root.AppendChild(nodeTransp);
            nodeCurrent = nodeTransp;

            wCampo(Transp.modFrete, TpcnTipoCampo.tcInt, Properties.Resources.modFrete);
            //
            //  (**)GerarTranspTransporta;
            //
            if (!string.IsNullOrEmpty(Transp.Transporta.CNPJ) ||
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

                if (!string.IsNullOrEmpty(Transp.Transporta.CNPJ))
                    wCampo(Transp.Transporta.CNPJ, TpcnTipoCampo.tcStr, Properties.Resources.CNPJ);
                else
                    if (!string.IsNullOrEmpty(Transp.Transporta.CPF))
                        wCampo(Transp.Transporta.CPF, TpcnTipoCampo.tcStr, Properties.Resources.CPF);
                wCampo(Transp.Transporta.xNome, TpcnTipoCampo.tcStr, Properties.Resources.xNome, ObOp.Opcional);
                wCampo(Transp.Transporta.IE, TpcnTipoCampo.tcStr, Properties.Resources.IE, ObOp.Opcional);
                wCampo(Transp.Transporta.xEnder, TpcnTipoCampo.tcStr, Properties.Resources.xEnder, ObOp.Opcional);
                wCampo(Transp.Transporta.xMun, TpcnTipoCampo.tcStr, Properties.Resources.xMun, ObOp.Opcional);
                wCampo(Transp.Transporta.UF, TpcnTipoCampo.tcStr, Properties.Resources.UF, ObOp.Opcional);
            }
            //
            //  (**)GerarTranspRetTransp;
            //
            if ((Transp.retTransp.vServ > 0) ||
              (Transp.retTransp.vBCRet > 0) ||
              (Transp.retTransp.pICMSRet > 0) ||
              (Transp.retTransp.vICMSRet > 0) ||
              (!string.IsNullOrEmpty(Transp.retTransp.CFOP)) ||
              (Transp.retTransp.cMunFG > 0))
            {
                XmlElement nodeRetTransporta = doc.CreateElement("retTransp");
                nodeTransp.AppendChild(nodeRetTransporta);
                nodeCurrent = nodeRetTransporta;

                wCampo(Transp.retTransp.vServ, TpcnTipoCampo.tcDec2, Properties.Resources.vServ);
                wCampo(Transp.retTransp.vBCRet, TpcnTipoCampo.tcDec2, Properties.Resources.vBCRet);
                wCampo(Transp.retTransp.pICMSRet, TpcnTipoCampo.tcDec2, Properties.Resources.pICMSRet);
                wCampo(Transp.retTransp.vICMSRet, TpcnTipoCampo.tcDec2, Properties.Resources.vICMSRet);
                wCampo(Transp.retTransp.CFOP, TpcnTipoCampo.tcStr, Properties.Resources.CFOP);
                wCampo(Transp.retTransp.cMunFG, TpcnTipoCampo.tcStr, Properties.Resources.cMunFG);
            }
            //
            //  (**)GerarTranspVeicTransp;
            //
            if (!string.IsNullOrEmpty(Transp.veicTransp.placa) ||
              !string.IsNullOrEmpty(Transp.veicTransp.UF) ||
              !string.IsNullOrEmpty(Transp.veicTransp.RNTC))
            {
                XmlElement nodeveicTransp = doc.CreateElement("veicTransp");
                nodeTransp.AppendChild(nodeveicTransp);
                nodeCurrent = nodeveicTransp;

                wCampo(Transp.veicTransp.placa, TpcnTipoCampo.tcStr, Properties.Resources.placa);
                wCampo(Transp.veicTransp.UF, TpcnTipoCampo.tcStr, Properties.Resources.UF);
                wCampo(Transp.veicTransp.RNTC, TpcnTipoCampo.tcStr, Properties.Resources.RNTC, ObOp.Opcional);
            }
            //
            //(**)GerarTranspReboque;
            //
            if (Transp.Reboque.Count > 2)
                this.cMensagemErro += "Transp.reboque: Excedeu o máximo permitido de 2" + Environment.NewLine;

            foreach (Reboque Reboque in Transp.Reboque)
            {
                XmlElement nodeReboque = doc.CreateElement("reboque");
                nodeTransp.AppendChild(nodeReboque);
                nodeCurrent = nodeReboque;

                wCampo(Reboque.placa, TpcnTipoCampo.tcStr, Properties.Resources.placa);
                wCampo(Reboque.UF, TpcnTipoCampo.tcStr, Properties.Resources.UF);
                wCampo(Reboque.RNTC, TpcnTipoCampo.tcStr, Properties.Resources.RNTC, ObOp.Opcional);
                wCampo(Reboque.vagao, TpcnTipoCampo.tcStr, Properties.Resources.vagao, ObOp.Opcional);
                wCampo(Reboque.balsa, TpcnTipoCampo.tcStr, Properties.Resources.balsa, ObOp.Opcional);
            }

            //
            //(**)GerarTranspVol;
            //
            foreach (Vol Vol in Transp.Vol)
            {
                XmlElement nodeVol = doc.CreateElement("vol");
                nodeTransp.AppendChild(nodeVol);
                nodeCurrent = nodeVol;

                wCampo(Vol.qVol, TpcnTipoCampo.tcInt, Properties.Resources.qVol, ObOp.Opcional);
                wCampo(Vol.esp, TpcnTipoCampo.tcStr, Properties.Resources.esp, ObOp.Opcional);
                wCampo(Vol.marca, TpcnTipoCampo.tcStr, Properties.Resources.marca, ObOp.Opcional);
                wCampo(Vol.nVol, TpcnTipoCampo.tcStr, Properties.Resources.nVol, ObOp.Opcional);
                wCampo(Vol.pesoL, TpcnTipoCampo.tcDec3, Properties.Resources.pesoL, ObOp.Opcional);
                wCampo(Vol.pesoB, TpcnTipoCampo.tcDec3, Properties.Resources.pesoB, ObOp.Opcional);
                //(**)GerarTranspVolLacres(i);
                foreach (Lacres lacres in Vol.Lacres)
                {
                    XmlElement nodeVolLacres = doc.CreateElement("lacres");
                    nodeVol.AppendChild(nodeVolLacres);
                    nodeCurrent = nodeVolLacres;

                    wCampo(lacres.nLacre, TpcnTipoCampo.tcStr, Properties.Resources.nLacre);
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

            wCampo(NFe.Total.ICMSTot.vBC, TpcnTipoCampo.tcDec2, Properties.Resources.vBC);
            wCampo(NFe.Total.ICMSTot.vICMS, TpcnTipoCampo.tcDec2, Properties.Resources.vICMS);
            wCampo(NFe.Total.ICMSTot.vBCST, TpcnTipoCampo.tcDec2, Properties.Resources.vBCST);
            wCampo(NFe.Total.ICMSTot.vST, TpcnTipoCampo.tcDec2, Properties.Resources.vST);
            wCampo(NFe.Total.ICMSTot.vProd, TpcnTipoCampo.tcDec2, Properties.Resources.vProd);
            wCampo(NFe.Total.ICMSTot.vFrete, TpcnTipoCampo.tcDec2, Properties.Resources.vFrete);
            wCampo(NFe.Total.ICMSTot.vSeg, TpcnTipoCampo.tcDec2, Properties.Resources.vSeg);
            wCampo(NFe.Total.ICMSTot.vDesc, TpcnTipoCampo.tcDec2, Properties.Resources.vDesc);
            wCampo(NFe.Total.ICMSTot.vII, TpcnTipoCampo.tcDec2, Properties.Resources.vII);
            wCampo(NFe.Total.ICMSTot.vIPI, TpcnTipoCampo.tcDec2, Properties.Resources.vIPI);
            wCampo(NFe.Total.ICMSTot.vPIS, TpcnTipoCampo.tcDec2, Properties.Resources.vPIS);
            wCampo(NFe.Total.ICMSTot.vCOFINS, TpcnTipoCampo.tcDec2, Properties.Resources.vCOFINS);
            wCampo(NFe.Total.ICMSTot.vOutro, TpcnTipoCampo.tcDec2, Properties.Resources.vOutro);
            wCampo(NFe.Total.ICMSTot.vNF, TpcnTipoCampo.tcDec2, Properties.Resources.vNF);
            #endregion

            #region --ISSQNtot
            if ((NFe.Total.ISSQNtot.vServ > 0) ||
                (NFe.Total.ISSQNtot.vBC > 0) ||
                (NFe.Total.ISSQNtot.vISS > 0) ||
                (NFe.Total.ISSQNtot.vPIS > 0) ||
                (NFe.Total.ISSQNtot.vCOFINS > 0))
            {
                nodeCurrent = doc.CreateElement("ISSQNtot");
                nodeTotal.AppendChild(nodeCurrent);

                wCampo(NFe.Total.ISSQNtot.vServ, TpcnTipoCampo.tcDec2, Properties.Resources.vServ, ObOp.Opcional);
                wCampo(NFe.Total.ISSQNtot.vBC, TpcnTipoCampo.tcDec2, Properties.Resources.vBC, ObOp.Opcional);
                wCampo(NFe.Total.ISSQNtot.vISS, TpcnTipoCampo.tcDec2, Properties.Resources.vISS, ObOp.Opcional);
                wCampo(NFe.Total.ISSQNtot.vPIS, TpcnTipoCampo.tcDec2, Properties.Resources.vPIS, ObOp.Opcional);
                wCampo(NFe.Total.ISSQNtot.vCOFINS, TpcnTipoCampo.tcDec2, Properties.Resources.vCOFINS, ObOp.Opcional);
            }
            #endregion

            #region --retTrib
            if ((NFe.Total.retTrib.vRetPIS > 0) ||
                (NFe.Total.retTrib.vRetCOFINS > 0) ||
                (NFe.Total.retTrib.vRetCSLL > 0) ||
                (NFe.Total.retTrib.vBCIRRF > 0) ||
                (NFe.Total.retTrib.vIRRF > 0) ||
                (NFe.Total.retTrib.vBCRetPrev > 0) ||
                (NFe.Total.retTrib.vRetPrev > 0))
            {
                nodeCurrent = doc.CreateElement("retTrib");
                nodeTotal.AppendChild(nodeCurrent);

                wCampo(NFe.Total.retTrib.vRetPIS, TpcnTipoCampo.tcDec2, Properties.Resources.vRetPIS, ObOp.Opcional);
                wCampo(NFe.Total.retTrib.vRetCOFINS, TpcnTipoCampo.tcDec2, Properties.Resources.vRetCOFINS, ObOp.Opcional);
                wCampo(NFe.Total.retTrib.vRetCSLL, TpcnTipoCampo.tcDec2, Properties.Resources.vRetCSLL, ObOp.Opcional);
                wCampo(NFe.Total.retTrib.vBCIRRF, TpcnTipoCampo.tcDec2, Properties.Resources.vBCIRRF, ObOp.Opcional);
                wCampo(NFe.Total.retTrib.vIRRF, TpcnTipoCampo.tcDec2, Properties.Resources.vIRRF, ObOp.Opcional);
                wCampo(NFe.Total.retTrib.vBCRetPrev, TpcnTipoCampo.tcDec2, Properties.Resources.vBCRetPrev, ObOp.Opcional);
                wCampo(NFe.Total.retTrib.vRetPrev, TpcnTipoCampo.tcDec2, Properties.Resources.vRetPrev, ObOp.Opcional);
            }
            #endregion
        }

        /// <summary>
        /// wCampo
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="Tipo"></param>
        /// <param name="TAG"></param>
        private void wCampo(object obj, TpcnTipoCampo Tipo, string TAG)
        {
            wCampo(obj, Tipo, TAG, ObOp.Obrigatorio, 0);
        }

        /// <summary>
        /// wCampo
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="Tipo"></param>
        /// <param name="TAG"></param>
        /// <param name="Obrigatorio"></param>
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
        /// <param name="Obrigatorio"></param>
        /// <param name="nAlign"></param>
        private void wCampo(object obj, TpcnTipoCampo Tipo, string TAG, ObOp Obrigatorio, int nAlign)
        {
            TAG = TAG.Trim();

            if (obj == null)
                return;

            if (Tipo == TpcnTipoCampo.tcDat || Tipo == TpcnTipoCampo.tcDatHor)
                if (((DateTime)obj).Year == 1)
                    if (Obrigatorio == ObOp.Opcional)
                        return;

            if (Obrigatorio == ObOp.Opcional)
            {
                if (Tipo == TpcnTipoCampo.tcInt)
                    if ((int)obj == 0)
                        return;

                if (Tipo == TpcnTipoCampo.tcDec2 || Tipo == TpcnTipoCampo.tcDec3 || Tipo == TpcnTipoCampo.tcDec4 || Tipo == TpcnTipoCampo.tcDec10)
                    if ((double)obj == 0)
                        return;

                if (Tipo == TpcnTipoCampo.tcHor)
                    if ((DateTime)obj == DateTime.MinValue)
                        return;

                if (obj.ToString().Trim() == "")
                    return;
            }
            XmlElement valueEl1 = doc.CreateElement(TAG);

            switch (Tipo)
            {
                case TpcnTipoCampo.tcDec2:
                    if (((double)obj) > 0 || Obrigatorio == ObOp.Obrigatorio)
                        valueEl1.InnerText = ((double)obj).ToString("0.00").Replace(",", ".");
                    break;

                case TpcnTipoCampo.tcDec3:
                    if (((double)obj) > 0 || Obrigatorio == ObOp.Obrigatorio)
                        valueEl1.InnerText = ((double)obj).ToString("0.000").Replace(",", ".");
                    break;

                case TpcnTipoCampo.tcDec4:
                    if (((double)obj) > 0 || Obrigatorio == ObOp.Obrigatorio)
                        valueEl1.InnerText = ((double)obj).ToString("0.0000").Replace(",", ".");
                    break;

                case TpcnTipoCampo.tcDec10:
                    if (((double)obj) > 0 || Obrigatorio == ObOp.Obrigatorio)
                        valueEl1.InnerText = ((double)obj).ToString("0.0000000000").Replace(",", ".");
                    break;

                case TpcnTipoCampo.tcDatHor:
                    if (((DateTime)obj).Year > 1)
                        valueEl1.InnerText = ((DateTime)obj).ToString("yyyy-MM-ddTHH:mm:ss");
                    break;

                case TpcnTipoCampo.tcHor:
                    if (Obrigatorio == ObOp.Opcional && ((DateTime)obj) == DateTime.MinValue)
                        return;
                    valueEl1.InnerText = ((DateTime)obj).ToString("HH:mm:ss");
                    break;

                case TpcnTipoCampo.tcDat:
                    if (((DateTime)obj).Year > 1)
                        valueEl1.InnerText = ((DateTime)obj).ToString("yyyy-MM-dd");
                    break;

                default:
                    if (nAlign > 0)
                    {
                        valueEl1.InnerText = obj.ToString().PadLeft(nAlign, '0');
                    }
                    else
                        if (Tipo == TpcnTipoCampo.tcInt)
                        {
                            if (((int)obj) != 0 || Obrigatorio == ObOp.Obrigatorio)
                                valueEl1.InnerText = ((int)obj).ToString();
                        }
                        else
                            if (obj.ToString().Trim() != "")
                                valueEl1.InnerText = ConvertToOEM(obj.ToString().TrimStart().TrimEnd());
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
            if (string.IsNullOrEmpty(FBuffer))
                return "";

            if (FBuffer.StartsWith("<![CDATA["))
                return FBuffer;

            String normalizedString = FBuffer.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < normalizedString.Length; i++)
            {
                Char c = normalizedString[i];
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    switch (Convert.ToInt16(normalizedString[i]))
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
        #endregion

        #region GerarChaveNFe
        /// <summary>
        /// MontaChave
        /// Cria a chave de acesso da NFe
        /// </summary>
        /// <param name="ArqXMLPedido"></param>
        public void GerarChaveNFe(string ArqPedido, Boolean xml)
        {
            int emp = new FindEmpresaThread(Thread.CurrentThread).Index;

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

            string ArqXMLRetorno = Empresa.Configuracoes[emp].PastaRetorno + "\\" + (xml ? Functions.ExtrairNomeArq(ArqPedido, Propriedade.ExtEnvio.GerarChaveNFe_XML) + "-ret-gerar-chave.xml" : Functions.ExtrairNomeArq(ArqPedido, Propriedade.ExtEnvio.GerarChaveNFe_TXT) + "-ret-gerar-chave.txt");
            string ArqERRRetorno = Empresa.Configuracoes[emp].PastaRetorno + "\\" + (xml ? Functions.ExtrairNomeArq(ArqPedido, Propriedade.ExtEnvio.GerarChaveNFe_XML) + "-gerar-chave.err" : Functions.ExtrairNomeArq(ArqPedido, Propriedade.ExtEnvio.GerarChaveNFe_TXT) + "-gerar-chave.err");

            try
            {
                Functions.DeletarArquivo(ArqXMLRetorno);
                Functions.DeletarArquivo(ArqERRRetorno);
                Functions.DeletarArquivo(Empresa.Configuracoes[emp].PastaErro + "\\" + ArqPedido);

                if (!File.Exists(ArqPedido))
                {
                    throw new Exception("Arquivo " + ArqPedido + " não encontrado");
                }

                //                if (!Auxiliar.FileInUse(ArqPedido))
                //                {
                int serie = 0;
                int tpEmis = Empresa.Configuracoes[emp].tpEmis;
                int nNF = 0;
                int cNF = 0;
                int cUF = Empresa.Configuracoes[emp].UFCod;
                string cAAMM = "0000";
                string cChave = "";
                string cCNPJ = "";
                string cError = "";

                if (xml)
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(ArqPedido);

                    XmlNodeList mChaveList = doc.GetElementsByTagName("gerarChave");

                    foreach (XmlNode mChaveNode in mChaveList)
                    {
                        XmlElement mChaveElemento = (XmlElement)mChaveNode;

                        if (mChaveElemento.GetElementsByTagName("UF").Count != 0)
                            cUF = Convert.ToInt32("0" + mChaveElemento.GetElementsByTagName("UF")[0].InnerText);

                        if (mChaveElemento.GetElementsByTagName("tpEmis").Count != 0)
                            tpEmis = Convert.ToInt32("0" + mChaveElemento.GetElementsByTagName("tpEmis")[0].InnerText);

                        if (mChaveElemento.GetElementsByTagName("nNF").Count != 0)
                            nNF = Convert.ToInt32("0" + mChaveElemento.GetElementsByTagName("nNF")[0].InnerText);

                        if (mChaveElemento.GetElementsByTagName("cNF").Count != 0)
                            cNF = Convert.ToInt32("0" + mChaveElemento.GetElementsByTagName("cNF")[0].InnerText);

                        if (mChaveElemento.GetElementsByTagName("serie").Count != 0)
                            serie = Convert.ToInt32("0" + mChaveElemento.GetElementsByTagName("serie")[0].InnerText);

                        if (mChaveElemento.GetElementsByTagName("AAMM").Count != 0)
                            cAAMM = mChaveElemento.GetElementsByTagName("AAMM")[0].InnerText;

                        if (mChaveElemento.GetElementsByTagName("CNPJ").Count != 0)
                            cCNPJ = mChaveElemento.GetElementsByTagName("CNPJ")[0].InnerText;
                    }
                }
                else
                {
                    List<string> cLinhas = Functions.LerArquivo(ArqPedido);
                    string[] dados;
                    foreach (string cLinha in cLinhas)
                    {
                        dados = cLinha.Split('|');
                        dados[0] = dados[0].ToUpper();
                        if (dados.GetLength(0) == 1)
                            cError += "Segmento [" + dados[0] + "] inválido" + Environment.NewLine;
                        else
                            switch (dados[0].ToLower())
                            {
                                case "uf":
                                    cUF = Convert.ToInt32("0" + dados[1]);
                                    break;
                                case "tpemis":
                                    tpEmis = Convert.ToInt32("0" + dados[1]);
                                    break;
                                case "nnf":
                                    nNF = Convert.ToInt32("0" + dados[1]);
                                    break;
                                case "cnf":
                                    cNF = Convert.ToInt32("0" + dados[1]);
                                    break;
                                case "serie":
                                    serie = Convert.ToInt32("0" + dados[1]);
                                    break;
                                case "aamm":
                                    cAAMM = dados[1];
                                    break;
                                case "cnpj":
                                    cCNPJ = dados[1];
                                    break;
                            }
                    }
                }

                if (nNF == 0)
                    cError = "Número da nota fiscal deve ser informado" + Environment.NewLine;

                if (string.IsNullOrEmpty(cAAMM))
                    cError += "Ano e mês da emissão deve ser informado" + Environment.NewLine;

                if (string.IsNullOrEmpty(cCNPJ))
                    cError += "CNPJ deve ser informado" + Environment.NewLine;

                if (cAAMM.Substring(0, 2) == "00")
                    cError += "Ano da emissão inválido" + Environment.NewLine;

                if (Convert.ToInt32(cAAMM.Substring(2, 2)) <= 0 || Convert.ToInt32(cAAMM.Substring(2, 2)) > 12)
                    cError += "Mês da emissão inválido" + Environment.NewLine;

                if (cError != "")
                    throw new Exception(cError);

                Int64 iTmp = Convert.ToInt64("0" + cCNPJ);
                cChave = cUF.ToString("00") + cAAMM + iTmp.ToString("00000000000000") + "55";

                if (cNF == 0)
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
                File.WriteAllText(ArqXMLRetorno, vMsgRetorno, Encoding.Default);

                ///
                /// exclui o XML/TXT de pedido
                /// 
                Functions.DeletarArquivo(ArqPedido);
                //                }
            }
            catch (Exception ex)
            {
                try
                {
                    new Auxiliar().MoveArqErro(ArqPedido);

                    File.WriteAllText(ArqERRRetorno, "Arquivo " + ArqERRRetorno + Environment.NewLine + ex.Message, Encoding.Default);
                }
                catch
                {
                    //Se der algum erro na hora de gravar o arquivo de erro para o ERP, infelizmente não vamos poder fazer nada, visto que 
                    //pode ser algum problema com a rede, hd, permissões, etc... Wandrey 22/03/2010
                }
            }
        }
        #endregion
    }
}
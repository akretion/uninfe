using System;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Text;

namespace uninfe
{
    /// <summary>
    /// Classe responsável pela conversão de TXT para XML da Nota Fiscal Eletrônica
    /// TXT no mesmo formato do aplicativo de NFe do estado de São Paulo
    /// </summary>
    class UnitxtTOxmlClass
    {
        public string Retorno { get; private set; }
        public string cMensagemErro { get; private set; }
        /// <summary>
        /// Chave da NFe já calculada e montada
        /// </summary>
        public string ChaveNfe { get; private set; }

        /// <summary>
        /// Converte o arquivo TXT gerado para ser importado através do programa emissor de Nfe SFAZ/SP para XML versão 1.10
        /// </summary>
        /// <param name="cFile">Arquivo TXT a ser convertido</param>
        /// <param name="cDestino">Pasta de destino onde é para ser gerado o XML da NFe</param>
        /// <by>Marcos Paulo Gomes - marcos@delphibr.com.br</by>
        /// <date>16/05/2009</date>
        public void Converter(string cFile, string cDestino)
        {
            //Lê o arquivo texto passado do padrao do software emissor de Nfe Sefaz/SP 
            //Variaveis utilizadas na função

            cMensagemErro = "";
            if (File.Exists(cFile))
            {

            }

            TextReader txt = new StreamReader(cFile);


            try
            {
                System.Reflection.Assembly a = System.Reflection.Assembly.GetEntryAssembly();
                string baseDir = UniNfeInfClass.PastaSchemas() + "\\nfe_v1.10.xsd";

                DataSet dsNfe = new DataSet();
                dsNfe.ReadXmlSchema(baseDir);

                dsNfe.EnforceConstraints = false; //permite campos nulos
                
                string cLinhaTXT;
                string[] dados;
                int iLeitura;
                int iLacre;

                DataRow dremit = dsNfe.Tables["emit"].NewRow();

                DataRow drdest = dsNfe.Tables["dest"].NewRow();

                DataRow drPISOutr;
                DataRow drPISST;
                DataRow drCOFINSOutr;
                DataRow drCOFINSST;
                DataRow drtransporta;
                DataRow drIPITrib;

                string idprod; // Guarda o Id do produto, usado para gravar os dados referente a impostos
                idprod = "";
                drPISOutr = null;
                drPISST = null;
                drCOFINSOutr = null;
                drCOFINSST = null;
                drtransporta = null;
                drIPITrib = null;
                int iControle;
                iControle = 1;
                iLacre = 1;

                string cChave = ""; // Monta string com a chave Nota fiscal
                Int64 iTmp = 0; //Valores temporarios
                int serie = 0;
                int nNF = 0; //Numer Nf
                int cNF = 0; //Código Numérico que compõe a Chave de Acesso
                int cDV = 0; //Dígito Verificador da Chave de Acesso
                int iLinhaLida = 0; //controla a linha que foi lida


                cLinhaTXT = txt.ReadLine();
                dados = cLinhaTXT.Split('|');
                iLinhaLida++;

                if (dados[0] != "NOTA FISCAL")
                {
                    throw new ArgumentException("Este arquivo não é um arquivo de NOTA FISCAL");
                }



                while (cLinhaTXT != null)
                {
                    cLinhaTXT = txt.ReadLine();
                    iLinhaLida++;

                    if (cLinhaTXT == null)
                    {
                        break;
                    }
                    dados = cLinhaTXT.Split('|');
                    dados[0] = dados[0].ToUpper();

                    //no banco de dados progress quando o campo é null ele retorna "?", essa rotina, troca o "?" para ""
                    for (iLeitura = 0; iLeitura <= dados.GetUpperBound(0) - 1; iLeitura++)
                    {
                        if (dados[iLeitura].Trim() == "?")
                            dados[iLeitura] = "";
                        else
                            dados[iLeitura] = dados[iLeitura].Trim();
                    }

                    if (dados[0] == "I")


                        if (valida_elemento(dados[0], dados.GetLength(0)) != "OK")
                        {
                            //causar excecao
                            throw new ArgumentException("Erro na Linha " + iLinhaLida.ToString() + " A Quantidade da campos da linha " + dados[0] + " é diferente da quantidade exigida pelo layout \n\nDica:\n1 - Certifique-se de que não haja | no meio do texto\n2 - Certifique-se de que a linha esteja terminado com |");

                        }

                    if (dados[0] == "A") //tag <infNFe 
                    {
                        DataRow dr = dsNfe.Tables["infNFe"].NewRow();
                        dr["versao"] = dados[1].Trim();
                        dr["id"] = dados[2]; //id
                        dr["infNFe_Id"] = 0;
                        dsNfe.Tables["infNFe"].Rows.Add(dr);


                    }

                    if (dados[0] == "B") //tag <infNFe><ide
                    {
                        DataRow dr = dsNfe.Tables["ide"].NewRow();
                        cChave = "";
                        for (iLeitura = 0; iLeitura <= 18; iLeitura++)
                        {
                            if (iLeitura > 0 && dados[iLeitura] != null && dados[iLeitura].Trim() != "")
                                dr[iLeitura - 1] = dados[iLeitura].Trim();

                        }

                        dr["infNFe_Id"] = 0;
                        dr["ide_Id"] = 0;
                        dsNfe.Tables["ide"].Rows.Add(dr);

                        //Alimenta dados par amontar a strimg de chave de acesso
                        serie = Convert.ToInt32(dr["serie"].ToString());
                        nNF = Convert.ToInt32(dr["nNF"].ToString()); //Numer Nf
                        cNF = Convert.ToInt32(dr["cNF"].ToString()); //Código Numérico que compõe a Chave de Acesso
                        cDV = Convert.ToInt32(dr["cDV"].ToString()); ; //Dígito Verificador da Chave de Acesso


                        cChave = dr["cUF"].ToString() + dr["dEmi"].ToString().Substring(2, 2) +
                                 dr["dEmi"].ToString().Substring(5, 2); //data AAMM



                    }
                    if (dados[0] == "B13" || dados[0] == "B14") //tag <infNFe><ide><refNF>
                    {
                        //esse codigo foi montado dessa forma para que possoa mater a tag <NFref> <refNf>


                        DataRow drNFref = dsNfe.Tables["NFref"].NewRow();
                        drNFref["ide_Id"] = 0;
                        drNFref["NFref_Id"] = iControle; ;
                        if (dados[0] == "B13") //<NFref>
                            drNFref[0] = dados[1]; //caso tenha o segmento B13 preenche o campo chave
                        dsNfe.Tables["NFref"].Rows.Add(drNFref);



                        if (dados[0] == "B14")
                        {
                            DataRow dr = dsNfe.Tables["refNF"].NewRow();

                            for (iLeitura = 0; iLeitura <= 6; iLeitura++)
                            {
                                if (iLeitura > 0 && dados[iLeitura] != null)
                                    dr[iLeitura - 1] = dados[iLeitura].Trim();

                            }
                            dr["NFref_Id"] = iControle;
                            dsNfe.Tables["refNF"].Rows.Add(dr);
                        }
                        iControle = iControle + 1;
                    }

                    if (dados[0].Substring(0, 1) == "C") //tag <infNFe><ide><emit>
                    {

                        if (dados[0] == "C") //tag <infNFe><ide><emit>
                        {

                            for (iLeitura = 0; iLeitura <= 6; iLeitura++)
                            {
                                dremit["IE"] = "";
                                //nao preenche o campo cnpj ou cpf, sera preenchido mais abaixo
                                for (iLeitura = 0; iLeitura <= 6; iLeitura++)
                                {
                                    //nao preenche o campo cnpj ou cpf, sera preenchido mais abaixo
                                    if (iLeitura > 1 & dados[iLeitura] != null && dados[iLeitura - 1].Trim() != "")
                                        dremit[iLeitura] = dados[iLeitura - 1].Trim();

                                }

                            }



                            dremit["infNFe_Id"] = 0;
                            dremit["emit_Id"] = 0;
                        }
                        else if (dados[0] == "C02") //ainda tag <infNFe><ide><emit>, preenche o cnpj
                        {
                            dremit[0] = dados[1];
                            iTmp = Convert.ToInt64(dremit["CNPJ"]);
                            cChave = cChave + iTmp.ToString("00000000000000") + "55";


                        }
                        else if (dados[0] == "C02A") //ainda tag <infNFe><ide><emit>, preenche o cpf
                        {
                            dremit[1] = dados[1];
                        }

                    }
                    if (dados[0] == "C05")
                    {
                        DataRow dr = dsNfe.Tables["enderEmit"].NewRow();

                        dr["emit_Id"] = 0;

                        dsNfe.Tables["emit"].Rows.Add(dremit);

                        for (iLeitura = 0; iLeitura <= 11; iLeitura++)
                        {
                            if (iLeitura > 0 && dados[iLeitura] != null && dados[iLeitura].Trim() != "")
                                dr[iLeitura - 1] = dados[iLeitura].Trim();

                        }
                        dsNfe.Tables["enderEmit"].Rows.Add(dr);


                    }

                    if (dados[0] == "D")
                    {
                        DataRow dr = dsNfe.Tables["avulsa"].NewRow();
                        for (iLeitura = 0; iLeitura <= 11; iLeitura++)
                        {
                            if (iLeitura > 0 & dados[iLeitura] != null && dados[iLeitura].Trim() != "")
                                dr[iLeitura - 1] = dados[iLeitura].Trim();

                        }
                        dr["infNFe_Id"] = 0;
                        dsNfe.Tables["avulsa"].Rows.Add(dr);

                    }

                    if (dados[0] == "E") //tag <infNFe><ide><emit>
                    {
                        dsNfe.Tables["dest"].Columns["IE"].AllowDBNull = true;
                        drdest["IE"] = ""; //deve sempre gerar essa tag mesmo que em branco se nao ha problemas na hora dele inveter o enderdest

                        for (iLeitura = 0; iLeitura <= 4; iLeitura++)
                        {
                            //nao preenche o campo cnpj ou cpf, sera preenchido mais abaixo
                            if (iLeitura > 1 & dados[iLeitura] != null && dados[iLeitura - 1].Trim() != "")
                                drdest[iLeitura] = dados[iLeitura - 1].Trim();

                        }

                        drdest["dest_Id"] = 0;
                        drdest["infNFe_Id"] = 0;


                    }
                    if (dados[0] == "E02") //ainda tag <infNFe><ide><emit>, preenche o cnpj
                    {
                        drdest["CNPJ"] = dados[1];
                        dsNfe.Tables["dest"].Rows.Add(drdest);
                    }
                    if (dados[0] == "E03") //ainda tag <infNFe><ide><emit>, preenche o cpf
                    {
                        drdest["CPF"] = dados[1];
                        dsNfe.Tables["dest"].Rows.Add(drdest);
                    }

                    if (dados[0] == "E05")
                    {
                        DataRow drenderDest = dsNfe.Tables["enderDest"].NewRow();
                        for (iLeitura = 0; iLeitura <= 11; iLeitura++)
                        {
                            if (iLeitura > 0 & dados[iLeitura] != null && dados[iLeitura].Trim() != "")
                                drenderDest[iLeitura - 1] = dados[iLeitura].Trim();

                        }
                        drenderDest["dest_Id"] = 0;
                        dsNfe.Tables["enderDest"].Rows.Add(drenderDest);

                    }

                    if (dados[0] == "F")
                    {
                        DataRow drretirada = dsNfe.Tables["retirada"].NewRow();
                        for (iLeitura = 0; iLeitura <= 8; iLeitura++)
                        {
                            if (iLeitura > 0 & dados[iLeitura] != null && dados[iLeitura].Trim() != "")
                                drretirada[iLeitura - 1] = dados[iLeitura].Trim();

                        }
                        dsNfe.Tables["retirada"].Rows.Add(drretirada);
                    }

                    if (dados[0] == "G")
                    {
                        DataRow drentrega = dsNfe.Tables["entrega"].NewRow();
                        for (iLeitura = 0; iLeitura <= 8; iLeitura++)
                        {
                            if (iLeitura > 0 & dados[iLeitura] != null && dados[iLeitura].Trim() != "")
                                drentrega[iLeitura - 1] = dados[iLeitura].Trim();

                        }
                        drentrega["infNFe_Id"] = 0;
                        dsNfe.Tables["entrega"].Rows.Add(drentrega);
                    }

                    if (dados[0] == "H")
                    {
                        DataRow drdet = dsNfe.Tables["det"].NewRow();
                        drdet["nItem"] = dados[1];
                        if (dados[2].Trim() != "")
                            drdet["infAdProd"] = dados[2];
                        idprod = drdet[0].ToString();
                        drdet["det_Id"] = idprod; //det_Id
                        drdet["infNFe_Id"] = 0;
                        dsNfe.Tables["det"].Rows.Add(drdet);

                    }

                    if (dados[0] == "I")
                    {
                        DataRow drprod = dsNfe.Tables["prod"].NewRow();
                        drprod["cEAN"] = ""; //se nao deixa-lo em branco da erro
                        drprod["CEANTrib"] = ""; //se nao deixa-lo em branco da erro.

                        for (iLeitura = 0; iLeitura <= 18; iLeitura++)
                        {
                            if (iLeitura > 0 & dados[iLeitura] != null && dados[iLeitura].Trim() != "")
                                drprod[iLeitura - 1] = dados[iLeitura].Trim();

                        }
                        drprod[19] = idprod.ToString(); //det_Id
                        dsNfe.Tables["prod"].Rows.Add(drprod);
                    }

                    if (dados[0] == "I18")
                    {
                        DataRow drDI = dsNfe.Tables["DI"].NewRow();
                        for (iLeitura = 0; iLeitura <= 6; iLeitura++)
                        {
                            if (iLeitura > 0 & dados[iLeitura] != null && dados[iLeitura].Trim() != "")
                                drDI[iLeitura - 1] = dados[iLeitura].Trim();

                        }
                        dsNfe.Tables["DI"].Rows.Add(drDI);
                    }

                    if (dados[0] == "I25")
                    {
                        DataRow dradi = dsNfe.Tables["adi"].NewRow();
                        for (iLeitura = 0; iLeitura <= 4; iLeitura++)
                        {
                            if (iLeitura > 0 & dados[iLeitura] != null && dados[iLeitura].Trim() != "")
                                dradi[iLeitura - 1] = dados[iLeitura].Trim();

                        }
                        dsNfe.Tables["adi"].Rows.Add(dradi);
                    }

                    if (dados[0] == "J")
                    {
                        DataRow drveicProd = dsNfe.Tables["veicProd"].NewRow();
                        for (iLeitura = 0; iLeitura <= 22; iLeitura++)
                        {
                            if (iLeitura > 0 & dados[iLeitura] != null && dados[iLeitura].Trim() != "")
                                drveicProd[iLeitura - 1] = dados[iLeitura].Trim();

                        }
                        dsNfe.Tables["veicProd"].Rows.Add(drveicProd);
                    }

                    if (dados[0] == "K")
                    {
                        DataRow drveicProd = dsNfe.Tables["veicProd"].NewRow();
                        for (iLeitura = 0; iLeitura <= 5; iLeitura++)
                        {
                            if (iLeitura > 0 & dados[iLeitura] != null && dados[iLeitura].Trim() != "")
                                drveicProd[iLeitura - 1] = dados[iLeitura].Trim();
                        }
                        dsNfe.Tables["veicProd"].Rows.Add(drveicProd);
                    }

                    if (dados[0] == "L")
                    {
                        DataRow drarma = dsNfe.Tables["arma"].NewRow();
                        for (iLeitura = 0; iLeitura <= 4; iLeitura++)
                        {
                            if (iLeitura > 0 & dados[iLeitura] != null && dados[iLeitura].Trim() != "")
                                drarma[iLeitura - 1] = dados[iLeitura].Trim();
                        }
                        dsNfe.Tables["arma"].Rows.Add(drarma);
                    }

                    if (dados[0] == "L01")
                    {
                        DataRow drcomb = dsNfe.Tables["comb"].NewRow();
                        for (iLeitura = 0; iLeitura <= 3; iLeitura++)
                        {
                            if (iLeitura > 0 & dados[iLeitura] != null && dados[iLeitura].Trim() != "")
                                drcomb[iLeitura - 1] = dados[iLeitura].Trim();
                        }
                        dsNfe.Tables["comb"].Rows.Add(drcomb);
                    }

                    if (dados[0] == "L05")
                    {
                        DataRow drCIDE = dsNfe.Tables["CIDE"].NewRow();
                        for (iLeitura = 0; iLeitura <= 3; iLeitura++)
                        {
                            if (iLeitura > 0 & dados[iLeitura] != null && dados[iLeitura].Trim() != "")
                                drCIDE[iLeitura - 1] = dados[iLeitura].Trim();
                        }
                        dsNfe.Tables["CIDE"].Rows.Add(drCIDE);
                    }

                    if (dados[0] == "L09")
                    {
                        DataRow drICMSComb = dsNfe.Tables["ICMSComb"].NewRow();
                        for (iLeitura = 0; iLeitura <= 4; iLeitura++)
                        {
                            if (iLeitura > 0 & dados[iLeitura] != null && dados[iLeitura].Trim() != "")
                                drICMSComb[iLeitura - 1] = dados[iLeitura].Trim();
                        }
                        dsNfe.Tables["ICMSComb"].Rows.Add(drICMSComb);
                    }

                    if (dados[0] == "L114")
                    {
                        DataRow drICMSInter = dsNfe.Tables["ICMSInter"].NewRow();
                        for (iLeitura = 0; iLeitura <= 2; iLeitura++)
                        {
                            if (iLeitura > 0 & dados[iLeitura] != null && dados[iLeitura].Trim() != "")
                                drICMSInter[iLeitura - 1] = dados[iLeitura].Trim();
                        }
                        dsNfe.Tables["ICMSInter"].Rows.Add(drICMSInter);
                    }

                    if (dados[0] == "L117")
                    {
                        DataRow drICMSCons = dsNfe.Tables["ICMSCons"].NewRow();
                        for (iLeitura = 0; iLeitura <= 3; iLeitura++)
                        {
                            if (iLeitura > 0 & dados[iLeitura] != null && dados[iLeitura].Trim() != "")
                                drICMSCons[iLeitura - 1] = dados[iLeitura].Trim();
                        }
                        dsNfe.Tables["ICMSCons"].Rows.Add(drICMSCons);
                    }


                    if (dados[0] == "L117")
                    {
                        DataRow drICMSCons = dsNfe.Tables["ICMSCons"].NewRow();
                        for (iLeitura = 0; iLeitura <= 3; iLeitura++)
                        {
                            if (iLeitura > 0 & dados[iLeitura] != null && dados[iLeitura].Trim() != "")
                                drICMSCons[iLeitura - 1] = dados[iLeitura].Trim();
                        }
                        dsNfe.Tables["ICMSCons"].Rows.Add(drICMSCons);
                    }

                    if (dados[0] == "N")
                    {

                        DataRow dr = dsNfe.Tables["imposto"].NewRow();
                        dr["imposto_Id"] = idprod.ToString();
                        dr["det_Id"] = idprod.ToString();

                        dsNfe.Tables["imposto"].Rows.Add(dr);

                        dr = dsNfe.Tables["ICMS"].NewRow();
                        dr["ICMS_Id"] = idprod.ToString();
                        dr["imposto_Id"] = idprod.ToString();
                        dsNfe.Tables["ICMS"].Rows.Add(dr);

                    }

                    if (dados[0] == "N02")
                    {
                        DataRow dr = dsNfe.Tables["ICMS00"].NewRow();
                        for (iLeitura = 0; iLeitura <= 6; iLeitura++)
                        {
                            if (iLeitura > 0 & dados[iLeitura] != null && dados[iLeitura].Trim() != "")
                                dr[iLeitura - 1] = dados[iLeitura].Trim();
                        }
                        dr["ICMS_Id"] = idprod.ToString();
                        dsNfe.Tables["ICMS00"].Rows.Add(dr);
                    }

                    if (dados[0] == "N03")
                    {
                        DataRow dr = dsNfe.Tables["ICMS10"].NewRow();
                        for (iLeitura = 0; iLeitura <= 12; iLeitura++)
                        {
                            if (iLeitura > 0 & dados[iLeitura] != null && dados[iLeitura].Trim() != "")
                                dr[iLeitura - 1] = dados[iLeitura].Trim();
                        }
                        dr["ICMS_Id"] = idprod.ToString();
                        dsNfe.Tables["ICMS10"].Rows.Add(dr);
                    }

                    if (dados[0] == "N04")
                    {
                        DataRow dr = dsNfe.Tables["ICMS20"].NewRow();
                        for (iLeitura = 0; iLeitura <= 7; iLeitura++)
                        {
                            if (iLeitura > 0 & dados[iLeitura] != null && dados[iLeitura].Trim() != "")
                                dr[iLeitura - 1] = dados[iLeitura].Trim();
                        }
                        dr["ICMS_Id"] = idprod.ToString();
                        dsNfe.Tables["ICMS20"].Rows.Add(dr);
                    }


                    if (dados[0] == "N05")
                    {
                        DataRow dr = dsNfe.Tables["ICMS30"].NewRow();
                        for (iLeitura = 0; iLeitura <= 8; iLeitura++)
                        {
                            if (iLeitura > 0 & dados[iLeitura] != null && dados[iLeitura].Trim() != "")
                                dr[iLeitura - 1] = dados[iLeitura].Trim();
                        }
                        dr["ICMS_Id"] = idprod.ToString();
                        dsNfe.Tables["ICMS30"].Rows.Add(dr);
                    }


                    if (dados[0] == "N06")
                    {
                        DataRow dr = dsNfe.Tables["ICMS40"].NewRow();
                        for (iLeitura = 0; iLeitura <= 2; iLeitura++)
                        {
                            if (iLeitura > 0 & dados[iLeitura] != null && dados[iLeitura].Trim() != "")
                                dr[iLeitura - 1] = dados[iLeitura].Trim();
                        }
                        dr["ICMS_Id"] = idprod.ToString();
                        dsNfe.Tables["ICMS40"].Rows.Add(dr);
                    }


                    if (dados[0] == "N07")
                    {
                        DataRow dr = dsNfe.Tables["ICMS51"].NewRow();
                        for (iLeitura = 0; iLeitura <= 7; iLeitura++)
                        {
                            if (iLeitura > 0 & dados[iLeitura] != null && dados[iLeitura].Trim() != "")
                                dr[iLeitura - 1] = dados[iLeitura].Trim();
                        }
                        dr["ICMS_Id"] = idprod.ToString();
                        dsNfe.Tables["ICMS51"].Rows.Add(dr);
                    }

                    if (dados[0] == "N08")
                    {
                        DataRow dr = dsNfe.Tables["ICMS60"].NewRow();
                        for (iLeitura = 0; iLeitura <= 4; iLeitura++)
                        {
                            if (iLeitura > 0 & dados[iLeitura] != null && dados[iLeitura].Trim() != "")
                                dr[iLeitura - 1] = dados[iLeitura].Trim();
                        }
                        dr["ICMS_Id"] = idprod.ToString();
                        dsNfe.Tables["ICMS60"].Rows.Add(dr);
                    }


                    if (dados[0] == "N09")
                    {
                        DataRow dr = dsNfe.Tables["ICMS70"].NewRow();
                        for (iLeitura = 0; iLeitura <= 13; iLeitura++)
                        {
                            if (iLeitura > 0 & dados[iLeitura] != null && dados[iLeitura].Trim() != "")
                                dr[iLeitura - 1] = dados[iLeitura].Trim();
                        }
                        dr["ICMS_Id"] = idprod.ToString();
                        dsNfe.Tables["ICMS70"].Rows.Add(dr);
                    }

                    if (dados[0] == "N10")
                    {
                        DataRow dr = dsNfe.Tables["ICMS90"].NewRow();
                        for (iLeitura = 0; iLeitura <= 13; iLeitura++)
                        {
                            if (iLeitura > 0 & dados[iLeitura] != null && dados[iLeitura].Trim() != "")
                                dr[iLeitura - 1] = dados[iLeitura].Trim();
                        }
                        dr["ICMS_Id"] = idprod.ToString();
                        dsNfe.Tables["ICMS90"].Rows.Add(dr);
                    }

                    if (dados[0] == "O") //IPI
                    {
                        DataRow dr = dsNfe.Tables["IPI"].NewRow();
                        for (iLeitura = 0; iLeitura <= 5; iLeitura++)
                        {
                            if (iLeitura > 0 & dados[iLeitura] != null && dados[iLeitura].Trim() != "")
                                dr[iLeitura - 1] = dados[iLeitura].Trim();
                        }

                        dr["IPI_Id"] = idprod.ToString();
                        dr["imposto_Id"] = idprod.ToString();
                        dsNfe.Tables["IPI"].Rows.Add(dr);
                    }


                    if (dados[0] == "O07" || dados[0] == "O10" || dados[0] == "O11")  //IPITrib
                    {
                        if (dados[0] == "O07")
                        {
                            drIPITrib = dsNfe.Tables["IPITrib"].NewRow();
                            drIPITrib["CST"] = dados[1].Trim();
                            drIPITrib["VIPI"] = dados[2].Trim();
                        }

                        if (dados[0] == "O10")
                        {
                            drIPITrib["VBC"] = dados[1].Trim();
                            drIPITrib["PIPI"] = dados[2].Trim();
                        }

                        if (dados[0] == "O11")
                        {
                            drIPITrib["QUnid"] = dados[1].Trim();
                            drIPITrib["VUnid"] = dados[2].Trim();
                        }

                        drIPITrib["IPI_Id"] = idprod.ToString();
                        if (dados[0] != "O07")
                            dsNfe.Tables["IPITrib"].Rows.Add(drIPITrib);
                    }

                    if (dados[0] == "O08") //IPINT
                    {
                        DataRow dr = dsNfe.Tables["IPINT"].NewRow();
                        dr["CST"] = dados[1].Trim();
                        dr["IPI_Id"] = idprod.ToString();
                        dsNfe.Tables["IPINT"].Rows.Add(dr);
                    }

                    if (dados[0] == "P") //II
                    {
                        DataRow dr = dsNfe.Tables["II"].NewRow();
                        for (iLeitura = 0; iLeitura <= 4; iLeitura++)
                        {
                            if (iLeitura > 0 & dados[iLeitura] != null && dados[iLeitura].Trim() != "")
                                dr[iLeitura - 1] = dados[iLeitura].Trim();
                        }


                        dr["imposto_Id"] = idprod.ToString();
                        dsNfe.Tables["II"].Rows.Add(dr);
                    }

                    if (dados[0] == "Q") //II
                    {
                        DataRow dr = dsNfe.Tables["PIS"].NewRow();
                        dr["PIS_Id"] = idprod.ToString();
                        dr["imposto_Id"] = idprod.ToString();
                        dsNfe.Tables["PIS"].Rows.Add(dr);
                    }

                    if (dados[0] == "Q02") //
                    {
                        DataRow dr = dsNfe.Tables["PISAliq"].NewRow();
                        for (iLeitura = 0; iLeitura <= 4; iLeitura++)
                        {
                            if (iLeitura > 0 & dados[iLeitura] != null && dados[iLeitura].Trim() != "")
                                dr[iLeitura - 1] = dados[iLeitura].Trim();
                        }

                        dr["PIS_Id"] = idprod.ToString();
                        dsNfe.Tables["PISAliq"].Rows.Add(dr);
                    }

                    if (dados[0] == "Q03") //
                    {
                        DataRow dr = dsNfe.Tables["PISQtde"].NewRow();
                        for (iLeitura = 0; iLeitura <= 4; iLeitura++)
                        {
                            if (iLeitura > 0 & dados[iLeitura] != null && dados[iLeitura].Trim() != "")
                                dr[iLeitura - 1] = dados[iLeitura].Trim();
                        }

                        dr["PIS_Id"] = idprod.ToString();
                        dsNfe.Tables["PISQtde"].Rows.Add(dr);

                    }

                    if (dados[0] == "Q04") //
                    {
                        DataRow dr = dsNfe.Tables["PISNT"].NewRow();
                        dr["CST"] = dados[1].Trim();
                        dr["PIS_Id"] = idprod.ToString();
                        dsNfe.Tables["PISNT"].Rows.Add(dr);
                    }

                    if (dados[0] == "Q05") //
                    {
                        drPISOutr = dsNfe.Tables["PISOutr"].NewRow();
                        drPISOutr["CST"] = dados[1].Trim();
                        drPISOutr["VPIS"] = dados[2].Trim();

                        drPISOutr["PIS_Id"] = idprod.ToString();
                        dsNfe.Tables["PISOutr"].Rows.Add(drPISOutr);
                    }

                    if (dados[0] == "Q07")
                    {
                        drPISOutr["VBC"] = dados[1];
                        drPISOutr["PPIS"] = dados[2];
                    }

                    if (dados[0] == "Q10")
                    {
                        drPISOutr["QBCProd"] = dados[1];
                        drPISOutr["VAliqProd"] = dados[2];

                    }
                    if (dados[0] == "R")
                    {
                        drPISST = dsNfe.Tables["PISST"].NewRow();
                        drPISST[4] = dados[1]; //vPIS
                    }

                    if (dados[0] == "R02")
                    {
                        drPISST[0] = dados[1]; //VBC
                        drPISST[1] = dados[2]; //pPIS
                    }
                    if (dados[0] == "R04")
                    {
                        drPISST[2] = dados[1]; //qBCProd
                        drPISST[3] = dados[2]; //vAliqProd
                    }
                    if (dados[0] == "S") //cofins
                    {
                        DataRow drCOFINS = dsNfe.Tables["COFINS"].NewRow();
                        drCOFINS["COFINS_Id"] = idprod.ToString();
                        drCOFINS["imposto_Id"] = idprod.ToString();
                        dsNfe.Tables["COFINS"].Rows.Add(drCOFINS);
                    }

                    if (dados[0] == "S02") //COFINSAliq
                    {
                        DataRow dr = dsNfe.Tables["COFINSAliq"].NewRow();
                        for (iLeitura = 0; iLeitura <= 4; iLeitura++)
                        {
                            if (iLeitura > 0 & dados[iLeitura] != null && dados[iLeitura].Trim() != "")
                                dr[iLeitura - 1] = dados[iLeitura].Trim();
                        }
                        dr["COFINS_Id"] = idprod.ToString();
                        dsNfe.Tables["COFINSAliq"].Rows.Add(dr);
                    }

                    if (dados[0] == "S03") //COFINSQtde
                    {
                        DataRow dr = dsNfe.Tables["COFINSQtde"].NewRow();
                        for (iLeitura = 0; iLeitura <= 4; iLeitura++)
                        {
                            if (iLeitura > 0 & dados[iLeitura] != null && dados[iLeitura].Trim() != "")
                                dr[iLeitura - 1] = dados[iLeitura].Trim();
                        }
                        dr["COFINS_Id"] = idprod.ToString();
                        dsNfe.Tables["COFINSQtde"].Rows.Add(dr);
                    }
                    if (dados[0] == "S04") //COFINSNT
                    {
                        DataRow dr = dsNfe.Tables["COFINSNT"].NewRow();
                        dr["CST"] = dados[1];
                        dr["COFINS_Id"] = idprod.ToString();
                        dsNfe.Tables["COFINSNT"].Rows.Add(dr);
                    }

                    if (dados[0] == "S05") //COFINSOutr
                    {
                        drCOFINSOutr = dsNfe.Tables["COFINSOutr"].NewRow();
                        drCOFINSOutr["CST"] = dados[1];
                        drCOFINSOutr["VCOFINS"] = dados[2];
                        // drCOFINSOutr["qBCProd"] = null;
                        drCOFINSOutr["COFINS_Id"] = idprod.ToString();
                    }

                    if (dados[0] == "S07") //COFINSOutr
                    {
                        drCOFINSOutr["VBC"] = dados[1];
                        drCOFINSOutr["PCOFINS"] = dados[2];
                        dsNfe.Tables["COFINSOutr"].Rows.Add(drCOFINSOutr); //executa  o Add, porque sempre tera o S07 ou S09

                    }
                    if (dados[0] == "S09") //COFINSOutr
                    {
                        drCOFINSOutr["QBCProd"] = dados[1];
                        drCOFINSOutr["VAliqProd"] = dados[2];
                        dsNfe.Tables["COFINSOutr"].Rows.Add(drCOFINSOutr); //executa  o Add, porque sempre tera o S07 ou S09

                    }
                    if (dados[0] == "T") //COFINSST
                    {
                        drCOFINSST = dsNfe.Tables["COFINSST"].NewRow();
                        drCOFINSST["vCOFINS"] = dados[1];
                        drCOFINSST["imposto_Id"] = idprod.ToString();
                    }

                    if (dados[0] == "T02") //COFINSST
                    {
                        drCOFINSST["QBCProd"] = 0;
                        drCOFINSST["VAliqProd"] = 0;
                        drCOFINSST["VBC"] = dados[1];
                        drCOFINSST["PCOFINS"] = dados[2];
                        dsNfe.Tables["COFINSST"].Rows.Add(drCOFINSST);
                    }

                    if (dados[0] == "T04") //COFINSST
                    {
                        drCOFINSST["QBCProd"] = dados[1];
                        drCOFINSST["VAliqProd"] = dados[2];
                        drCOFINSST["VBC"] = 0;
                        drCOFINSST["PCOFINS"] = 0;
                        dsNfe.Tables["COFINSST"].Rows.Add(drCOFINSST);
                    }

                    if (dados[0] == "U") //ISSQN
                    {
                        DataRow dr = dsNfe.Tables["ISSQN"].NewRow();
                        for (iLeitura = 0; iLeitura <= 5; iLeitura++)
                        {
                            if (iLeitura > 0 & dados[iLeitura] != null && dados[iLeitura].Trim() != "")
                                dr[iLeitura - 1] = dados[iLeitura].Trim();
                        }
                        dr["imposto_Id"] = idprod.ToString();
                    }

                    if (dados[0] == "W") //total
                    {
                        DataRow dr = dsNfe.Tables["total"].NewRow();
                        dr["total_Id"] = idprod.ToString();
                        dr["infNFe_Id"] = 0;
                        dsNfe.Tables["total"].Rows.Add(dr);
                    }


                    if (dados[0] == "W02") //ICMSTot
                    {
                        DataRow dr = dsNfe.Tables["ICMSTot"].NewRow();
                        for (iLeitura = 0; iLeitura <= 14; iLeitura++)
                        {
                            if (iLeitura > 0 & dados[iLeitura] != null && dados[iLeitura].Trim() != "")
                                dr[iLeitura - 1] = dados[iLeitura].Trim();
                        }
                        dr["total_Id"] = idprod.ToString();
                        dsNfe.Tables["ICMSTot"].Rows.Add(dr);
                    }
                    if (dados[0] == "W17") //ISSQNtot
                    {
                        DataRow dr = dsNfe.Tables["ISSQNtot"].NewRow();
                        for (iLeitura = 0; iLeitura <= 5; iLeitura++)
                        {
                            if (iLeitura > 0 & dados[iLeitura] != null && dados[iLeitura].Trim() != "")
                                dr[iLeitura - 1] = dados[iLeitura].Trim();
                        }

                        dr["total_Id"] = idprod.ToString();
                        dsNfe.Tables["ISSQNtot"].Rows.Add(dr);

                    }

                    if (dados[0] == "W23") //retTrib
                    {
                        DataRow dr = dsNfe.Tables["retTrib"].NewRow();
                        bool lEntrou = false;
                        for (iLeitura = 0; iLeitura <= 7; iLeitura++)
                        {
                            if (iLeitura > 0 & dados[iLeitura] != null && dados[iLeitura].Trim() != "")
                            {
                                dr[iLeitura - 1] = dados[iLeitura].Trim();
                                lEntrou = true;
                            }
                        }
                        if (lEntrou == true)
                        {
                            dr["total_Id"] = idprod.ToString();
                            dsNfe.Tables["retTrib"].Rows.Add(dr);
                        }
                    }
                    #region Informações relacionadas a transportadora / volumes
                    if (dados[0] == "X") //transp
                    {
                        DataRow dr = dsNfe.Tables["transp"].NewRow();
                        dr["ModFrete"] = dados[1];
                        dr["transp_Id"] = 0;
                        dr["infNFe_Id"] = 0;

                        dsNfe.Tables["transp"].Rows.Add(dr);

                    }

                    if (dados[0] == "X03") //transporta
                    {

                        drtransporta = dsNfe.Tables["transporta"].NewRow();
                        /*
                        for (iLeitura = 0; iLeitura <= 6; iLeitura++)
                        {
                            if (iLeitura > 0 & dados[iLeitura] != null && dados[iLeitura].Trim() != "")
                                drtransporta[iLeitura -1 ] = dados[iLeitura].Trim();
                        }
                         * */
                        //         1       3   3         4    5
                        //X03 | XNome | IE | XEnder | UF | XMun |
                        drtransporta["XNome"] = dados[1];
                        drtransporta["IE"] = dados[2];
                        drtransporta["XEnder"] = dados[3];
                        drtransporta["UF"] = dados[4];
                        drtransporta["XMun"] = dados[5];


                        drtransporta["transp_Id"] = 0;


                    }

                    if (dados[0] == "X04")  //CNPJ|
                    {
                        /* dsNfe.Tables["transporta"].Columns["CPF"].AllowDBNull = true; */
                        drtransporta["CNPJ"] = dados[1];

                        dsNfe.Tables["transporta"].Rows.Add(drtransporta);
                    }
                    if (dados[0] == "X05")  //CPF
                    {
                        /* dsNfe.Tables["transporta"].Columns["CNPJ"].AllowDBNull = true; */
                        drtransporta["CPF"] = dados[1];
                        dsNfe.Tables["transporta"].Rows.Add(drtransporta);

                    }

                    if (dados[0] == "X11") //retTransp
                    {
                        DataRow dr = dsNfe.Tables["retTransp"].NewRow();
                        for (iLeitura = 0; iLeitura <= 7; iLeitura++)
                        {
                            if (iLeitura > 0 & dados[iLeitura] != null && dados[iLeitura].Trim() != "")
                                dr[iLeitura - 1] = dados[iLeitura].Trim();
                        }

                        dr["transp_Id"] = 0;
                        dsNfe.Tables["retTransp"].Rows.Add(dr);

                    }


                    if (dados[0] == "X18") //veicTransp
                    {
                        DataRow dr = dsNfe.Tables["veicTransp"].NewRow();
                        dr["placa"] = dados[1];
                        dr["UF"] = dados[2];
                        dr["RNTC"] = dados[3];
                        dr["transp_Id"] = 0;
                        dsNfe.Tables["veicTransp"].Rows.Add(dr);

                    }

                    if (dados[0] == "X22") //reboque
                    {
                        DataRow dr = dsNfe.Tables["reboque"].NewRow();
                        dr["placa"] = dados[1];
                        dr["UF"] = dados[2];
                        dr["RNTC"] = dados[3];
                        dr["transp_Id"] = 0;
                        dsNfe.Tables["reboque"].Rows.Add(dr);
                    }

                    if (dados[0] == "X26") //vol
                    {
                        DataRow dr = dsNfe.Tables["vol"].NewRow();
                        for (iLeitura = 0; iLeitura <= 6; iLeitura++)
                        {
                            if (iLeitura > 0 && dados[iLeitura] != null && dados[iLeitura].Trim() != "")
                                dr[iLeitura - 1] = dados[iLeitura].Trim();
                        }
                        dr["vol_Id"] = 0;
                        dr["transp_Id"] = 0;
                        dsNfe.Tables["vol"].Rows.Add(dr);
                    }

                    if (dados[0] == "X33") //lacres
                    {
                        DataRow dr = dsNfe.Tables["lacres"].NewRow();
                        dr["nLacre"] = dados[1];
                        dr["vol_Id"] = 0;
                        dsNfe.Tables["lacres"].Rows.Add(dr);
                    }
                    #endregion

                    #region Informações relacionadas a cobrança
                    if (dados[0] == "Y") //cobr
                    {
                        DataRow dr = dsNfe.Tables["cobr"].NewRow();
                        dr["cobr_Id"] = 0;
                        dr["infNFe_Id"] = 0;
                        dsNfe.Tables["cobr"].Rows.Add(dr);

                    }

                    if (dados[0] == "Y02") //fat
                    {
                        DataRow dr = dsNfe.Tables["fat"].NewRow();
                        dr["nFat"] = dados[1];
                        dr["vOrig"] = dados[2];
                        dr["vDesc"] = dados[3];
                        dr["vLiq"] = dados[4];

                        dr["cobr_Id"] = 0;
                        dsNfe.Tables["fat"].Rows.Add(dr);

                    }

                    if (dados[0] == "Y07") //dup
                    {
                        DataRow dr = dsNfe.Tables["dup"].NewRow();
                        dr["nDup"] = dados[1];
                        dr["dVenc"] = dados[2];
                        dr["vDup"] = dados[3];
                        dr["cobr_Id"] = 0;
                        dsNfe.Tables["dup"].Rows.Add(dr);

                    }
                    #endregion
                    #region Observações da Nf
                    if (dados[0] == "Z") //infAdic
                    {
                        DataRow dr = dsNfe.Tables["infAdic"].NewRow();
                        if (dados[1] != "")
                            dr["infAdFisco"] = dados[1];
                        if (dados[2] != "")
                            dr["infCpl"] = dados[2];
                        dr["infNFe_Id"] = 0;
                        dsNfe.Tables["infAdic"].Rows.Add(dr);
                    }

                    if (dados[0] == "Z04") //obsCont
                    {
                        DataRow dr = dsNfe.Tables["obsCont"].NewRow();
                        dr["xCampo"] = dados[1];
                        dr["xTexto"] = dados[2];
                        //dr["infNFe_Id"] = 0;
                        dsNfe.Tables["obsCont"].Rows.Add(dr);
                    }
                    //dsNfe.Tables["obsFisco"]; não encontrei na documentação do TXT nada que fala sobre o conteudo dessa tebela

                    if (dados[0] == "Z10") //procRef
                    {
                        DataRow dr = dsNfe.Tables["procRef"].NewRow();
                        dr["nProc"] = dados[1];
                        dsNfe.Tables["procRef"].Rows.Add(dr);
                    }
                    #endregion
                    if (dados[0] == "ZA") //EXPORTA
                    {
                        DataRow dr = dsNfe.Tables["exporta"].NewRow();
                        dr["UFEmbarq"] = dados[1].Trim();
                        dr["xLocEmbarq"] = dados[2].Trim();
                        dr["infNFe_Id"] = 0;
                        dsNfe.Tables["exporta"].Rows.Add(dr);
                    }

                    if (dados[0] == "ZB") //compra
                    {
                        DataRow dr = dsNfe.Tables["compra"].NewRow();
                        dr["xNEmp"] = dados[1];
                        dr["xPed"] = dados[2];
                        dr["xCont"] = dados[3];
                        dr["infNFe_Id"] = 0;
                        dsNfe.Tables["compra"].Rows.Add(dr);
                    }

                }

                txt.Close();
                cChave += serie.ToString("000") + nNF.ToString("000000000") + cNF.ToString("000000000") + cDV.ToString("0");
                dsNfe.Tables["infNFe"].Rows[0]["Id"] = "NFe" + cChave;
                dsNfe.AcceptChanges();
                Retorno = cDestino + "\\" + cChave + "-nfe.xml";
                this.ChaveNfe = cChave;

                StringWriter TextoXml = new StringWriter();
                TextoXml.NewLine = "";

                dsNfe.WriteXml(TextoXml, XmlWriteMode.IgnoreSchema);
                #region Invertendo a TAG IE do emitente e destinatario
                string sAux;
                sAux = limpa_texto(TextoXml.ToString());

                //remove os espacos entre as tags
                TextoXml.GetStringBuilder().Remove(0, TextoXml.ToString().Length);
                TextoXml.GetStringBuilder().Append(sAux);




                //Ajustando a Tag de <NFref>
                if (TextoXml.ToString().IndexOf("<NFref>") > -1 && TextoXml.ToString().LastIndexOf("</NFref>") > -1)
                {
                    sAux = TextoXml.ToString().Substring(TextoXml.ToString().IndexOf("<NFref>"), TextoXml.ToString().LastIndexOf("</NFref>") - TextoXml.ToString().IndexOf("<NFref>") + 8);
                    //remove o texto que foi jogado para variavel
                    TextoXml.GetStringBuilder().Remove(TextoXml.ToString().IndexOf("<NFref>"), TextoXml.ToString().LastIndexOf("</NFref>") - TextoXml.ToString().IndexOf("<NFref>") + 8);
                    //insere o texto antes da tag tpImp
                    TextoXml.GetStringBuilder().Replace("<tpImp>", sAux + "<tpImp>");

                }

                //movendo os dados do </infAdProd> para apos a tag imposto
                sAux = "";
                //if (TextoXml.ToString().IndexOf("</infAdProd><prod>") > -1)
                while (TextoXml.ToString().IndexOf("</infAdProd><prod>") > -1)
                {
                    sAux = TextoXml.ToString().Substring(TextoXml.ToString().IndexOf("\"><infAdProd>") + 2, TextoXml.ToString().IndexOf("</infAdProd><prod>") - TextoXml.ToString().IndexOf("\"><infAdProd>") + 10);
                    //MessageBox.Show(sAux);

                    TextoXml.GetStringBuilder().Remove(TextoXml.ToString().IndexOf("\"><infAdProd>") + 2, sAux.Length);
                    TextoXml.GetStringBuilder().Insert(TextoXml.ToString().IndexOf("</imposto></det>") + 10, sAux);

                }



                //Ajustando a tag IE do emitente 
                sAux = TextoXml.ToString().Substring(TextoXml.ToString().IndexOf("<enderEmit>"), (TextoXml.ToString().IndexOf("</enderEmit>") - TextoXml.ToString().IndexOf("<enderEmit>")) + 12);
                TextoXml.GetStringBuilder().Replace(sAux, "").Replace("NewDataSet", "NFe"); //.Replace("\n","ç").Replace("\r","Í");
                TextoXml.GetStringBuilder().Replace("</xFant>", "</xFant>" + sAux);

                // Ajustando a tag IE do destinatario 

                sAux = TextoXml.ToString().Substring(TextoXml.ToString().IndexOf("<enderDest>"), (TextoXml.ToString().IndexOf("</enderDest>") - TextoXml.ToString().IndexOf("<enderDest>")) + 12);

                TextoXml.GetStringBuilder().Replace(sAux, "");
                //MessageBox.Show(TextoXml.ToString().IndexOf("</xNome></dest>").ToString());
                //achandoa posição para qual vai mover o <enderDest>
                iLeitura = -1;
                iLeitura = TextoXml.ToString().IndexOf("</xNome><IE/></dest>");
                if (iLeitura > -1)
                    iLeitura = iLeitura + 8; // posição entre </xNome><IE/>
                else
                {
                    iLeitura = TextoXml.ToString().Substring(0, TextoXml.ToString().IndexOf("</dest>")).LastIndexOf("<IE>");
                    if (iLeitura == -1)
                        throw new ArgumentException("Não foi possivel inverter as Tags EnderDest");

                }
                TextoXml.GetStringBuilder().Insert(iLeitura, sAux);
                TextoXml.GetStringBuilder().Replace("<IE/>", "").Replace("<IE />", "");

                //TextoXml.GetStringBuilder().Insert(TextoXml.ToString().IndexOf("<IE>", TextoXml.ToString().IndexOf("<dest>")), sAux);


                //TextoXml.GetStringBuilder().Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "");
                //TextWriter txtDestino = new StreamWriter(@Retorno);

                //MessageBox.Show(TextoXml.GetStringBuilder().GetHashCode().ToString());
                /*
                                txtDestino.Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + TextoXml.ToString());
                                txtDestino.Flush();
                                txtDestino.Close();
                  */
                txt.Close();

                XmlDocument xdoc = new XmlDocument();



                xdoc.LoadXml(TextoXml.ToString());

                XmlTextWriter xWriter = new XmlTextWriter(@Retorno, Encoding.UTF8);
                xWriter.Formatting = Formatting.None;
                xdoc.Save(xWriter);

                xWriter.Close();
                #endregion
            }
            catch (Exception Ex)
            {
                cMensagemErro = Ex.Message;
                txt.Close();
            }
        }

        private String gera_chave(String cChave)
        {
            /* Function Criada por Marcos Paulo Gomes
             * Funcção que retorna o DV da chave de acesso, para entender melhor a formula de cálculo do digito, veja a pagina 69 
             * do manual de integração, tópico "CÁLCULO DO DÍGITO VERIFICADOR DA CHAVE DE ACESSO DA NF-e"
             * 
             */
            int peso = 2;
            int icont;
            int total = 0;

            for (icont = cChave.Length - 1; icont >= 0; icont--)
            {
                if (peso > 9) { peso = 2; }

                total += Convert.ToInt16(cChave.Substring(icont, 1)) * peso;
                peso++;

            }
            return cChave + (11 - (total % 11)).ToString();
        }

 
        private string valida_elemento(string cElem, int iCampos)
        {
            /*
             * cElem - Recebe o tipo de elemento que será validado
             * iCampos - Recebe a quantidade de campos
             */
            string[,] aValidar = new string[88, 2];
            string cRetorno = "OK";
            int iPos = -1;
            /* Gera uma array com a identficicação da linha e quantos campos tem que ter na linha, por exemplo:
             * a linha com a identificação B deve ter 20 campos, caso a quantidade seja diferente disso é porqueo txt foi gerado
             * errado, sendo assim ele gera uma exceção e devolve erro para a rotina principal
             */
            #region Dfinição de array para validação dos seguimentos do arquivo txt
            iPos++; aValidar[iPos, 0] = "NOTA FISCAL"; aValidar[iPos, 1] = "3";
            iPos++; aValidar[iPos, 0] = "A"; aValidar[iPos, 1] = "4";
            iPos++; aValidar[iPos, 0] = "B"; aValidar[iPos, 1] = "20";
            iPos++; aValidar[iPos, 0] = "B13"; aValidar[iPos, 1] = "3";
            iPos++; aValidar[iPos, 0] = "B14"; aValidar[iPos, 1] = "8";
            iPos++; aValidar[iPos, 0] = "C"; aValidar[iPos, 1] = "8";
            iPos++; aValidar[iPos, 0] = "C02"; aValidar[iPos, 1] = "3";
            iPos++; aValidar[iPos, 0] = "C02a"; aValidar[iPos, 1] = "3";
            iPos++; aValidar[iPos, 0] = "C05"; aValidar[iPos, 1] = "13";
            iPos++; aValidar[iPos, 0] = "D"; aValidar[iPos, 1] = "13";
            iPos++; aValidar[iPos, 0] = "E"; aValidar[iPos, 1] = "5";
            iPos++; aValidar[iPos, 0] = "E02"; aValidar[iPos, 1] = "3";
            iPos++; aValidar[iPos, 0] = "E03"; aValidar[iPos, 1] = "3";
            iPos++; aValidar[iPos, 0] = "E05"; aValidar[iPos, 1] = "13";
            iPos++; aValidar[iPos, 0] = "F"; aValidar[iPos, 1] = "10";
            iPos++; aValidar[iPos, 0] = "G"; aValidar[iPos, 1] = "10";
            iPos++; aValidar[iPos, 0] = "H"; aValidar[iPos, 1] = "4";
            iPos++; aValidar[iPos, 0] = "I"; aValidar[iPos, 1] = "19";
            iPos++; aValidar[iPos, 0] = "I18"; aValidar[iPos, 1] = "8";
            iPos++; aValidar[iPos, 0] = "I25"; aValidar[iPos, 1] = "6";
            iPos++; aValidar[iPos, 0] = "J"; aValidar[iPos, 1] = "20";
            iPos++; aValidar[iPos, 0] = "c"; aValidar[iPos, 1] = "5";
            iPos++; aValidar[iPos, 0] = "K"; aValidar[iPos, 1] = "7";
            iPos++; aValidar[iPos, 0] = "L"; aValidar[iPos, 1] = "6";
            iPos++; aValidar[iPos, 0] = "L01"; aValidar[iPos, 1] = "5";
            iPos++; aValidar[iPos, 0] = "L105"; aValidar[iPos, 1] = "5";
            iPos++; aValidar[iPos, 0] = "L109"; aValidar[iPos, 1] = "6";
            iPos++; aValidar[iPos, 0] = "L114"; aValidar[iPos, 1] = "4";
            iPos++; aValidar[iPos, 0] = "L117"; aValidar[iPos, 1] = "5";
            iPos++; aValidar[iPos, 0] = "M"; aValidar[iPos, 1] = "2";
            iPos++; aValidar[iPos, 0] = "N"; aValidar[iPos, 1] = "2";
            iPos++; aValidar[iPos, 0] = "N02"; aValidar[iPos, 1] = "8";
            iPos++; aValidar[iPos, 0] = "N03"; aValidar[iPos, 1] = "14";
            iPos++; aValidar[iPos, 0] = "N04"; aValidar[iPos, 1] = "9";
            iPos++; aValidar[iPos, 0] = "N05"; aValidar[iPos, 1] = "10";
            iPos++; aValidar[iPos, 0] = "N06"; aValidar[iPos, 1] = "4";
            iPos++; aValidar[iPos, 0] = "N07"; aValidar[iPos, 1] = "9";
            iPos++; aValidar[iPos, 0] = "N08"; aValidar[iPos, 1] = "6";
            iPos++; aValidar[iPos, 0] = "N09"; aValidar[iPos, 1] = "15";
            iPos++; aValidar[iPos, 0] = "N10"; aValidar[iPos, 1] = "15";
            iPos++; aValidar[iPos, 0] = "O"; aValidar[iPos, 1] = "7";
            iPos++; aValidar[iPos, 0] = "O07"; aValidar[iPos, 1] = "4";
            iPos++; aValidar[iPos, 0] = "O10"; aValidar[iPos, 1] = "4";
            iPos++; aValidar[iPos, 0] = "O11"; aValidar[iPos, 1] = "4";
            iPos++; aValidar[iPos, 0] = "O08"; aValidar[iPos, 1] = "3";
            iPos++; aValidar[iPos, 0] = "P"; aValidar[iPos, 1] = "6";
            iPos++; aValidar[iPos, 0] = "Q"; aValidar[iPos, 1] = "2";
            iPos++; aValidar[iPos, 0] = "Q02"; aValidar[iPos, 1] = "6";
            iPos++; aValidar[iPos, 0] = "Q03"; aValidar[iPos, 1] = "6";
            iPos++; aValidar[iPos, 0] = "Q04"; aValidar[iPos, 1] = "3";
            iPos++; aValidar[iPos, 0] = "Q05"; aValidar[iPos, 1] = "4";
            iPos++; aValidar[iPos, 0] = "Q07"; aValidar[iPos, 1] = "4";
            iPos++; aValidar[iPos, 0] = "Q10"; aValidar[iPos, 1] = "4";
            iPos++; aValidar[iPos, 0] = "R"; aValidar[iPos, 1] = "3";
            iPos++; aValidar[iPos, 0] = "R02"; aValidar[iPos, 1] = "4";
            iPos++; aValidar[iPos, 0] = "R04"; aValidar[iPos, 1] = "4";
            iPos++; aValidar[iPos, 0] = "S"; aValidar[iPos, 1] = "2";
            iPos++; aValidar[iPos, 0] = "S02"; aValidar[iPos, 1] = "6";
            iPos++; aValidar[iPos, 0] = "S03"; aValidar[iPos, 1] = "6";
            iPos++; aValidar[iPos, 0] = "S04"; aValidar[iPos, 1] = "3";
            iPos++; aValidar[iPos, 0] = "S05"; aValidar[iPos, 1] = "4";
            iPos++; aValidar[iPos, 0] = "S07"; aValidar[iPos, 1] = "4";
            iPos++; aValidar[iPos, 0] = "S09"; aValidar[iPos, 1] = "4";
            iPos++; aValidar[iPos, 0] = "T"; aValidar[iPos, 1] = "3";
            iPos++; aValidar[iPos, 0] = "T02"; aValidar[iPos, 1] = "4";
            iPos++; aValidar[iPos, 0] = "T04"; aValidar[iPos, 1] = "4";
            iPos++; aValidar[iPos, 0] = "U"; aValidar[iPos, 1] = "7";
            iPos++; aValidar[iPos, 0] = "W"; aValidar[iPos, 1] = "2";
            iPos++; aValidar[iPos, 0] = "W02"; aValidar[iPos, 1] = "16";
            iPos++; aValidar[iPos, 0] = "W17"; aValidar[iPos, 1] = "7";
            iPos++; aValidar[iPos, 0] = "W23"; aValidar[iPos, 1] = "9";
            iPos++; aValidar[iPos, 0] = "X"; aValidar[iPos, 1] = "3";
            iPos++; aValidar[iPos, 0] = "X03"; aValidar[iPos, 1] = "7";
            iPos++; aValidar[iPos, 0] = "X04"; aValidar[iPos, 1] = "3";
            iPos++; aValidar[iPos, 0] = "X05"; aValidar[iPos, 1] = "3";
            iPos++; aValidar[iPos, 0] = "X11"; aValidar[iPos, 1] = "8";
            iPos++; aValidar[iPos, 0] = "X18"; aValidar[iPos, 1] = "5";
            iPos++; aValidar[iPos, 0] = "X22"; aValidar[iPos, 1] = "5";
            iPos++; aValidar[iPos, 0] = "X26"; aValidar[iPos, 1] = "8";
            iPos++; aValidar[iPos, 0] = "X33"; aValidar[iPos, 1] = "3";
            iPos++; aValidar[iPos, 0] = "Y"; aValidar[iPos, 1] = "2";
            iPos++; aValidar[iPos, 0] = "Y02"; aValidar[iPos, 1] = "6";
            iPos++; aValidar[iPos, 0] = "Y07"; aValidar[iPos, 1] = "5";
            iPos++; aValidar[iPos, 0] = "Z"; aValidar[iPos, 1] = "4";
            iPos++; aValidar[iPos, 0] = "Z04"; aValidar[iPos, 1] = "4";
            iPos++; aValidar[iPos, 0] = "Z10"; aValidar[iPos, 1] = "4";
            iPos++; aValidar[iPos, 0] = "ZA"; aValidar[iPos, 1] = "4";
            iPos++; aValidar[iPos, 0] = "ZB"; aValidar[iPos, 1] = "5";
            #endregion

            for (iPos = 0; iPos <= 87; iPos++)
            {
                if (cElem.ToUpper() == aValidar[iPos, 0])
                {
                    if (Convert.ToInt16(aValidar[iPos, 1])  != iCampos)
                    {
                        if ((aValidar[iPos, 0] == "I") && (iCampos == 19 || iCampos == 20))
                        {
                            cRetorno = "OK";
                        }
                        else
                        cRetorno = "A quantidade de campos no seguimento " + cElem + " é " + iCampos.ToString() + ", o correto é " +
                          Convert.ToInt16(aValidar[iPos, 1]) + " campos.";
                    }

                }




            }
            return cRetorno;


        }
        private string limpa_texto(string cTexto)
        {
            int iControle;
            string cRetorno;
            cRetorno = cTexto;
            while (cRetorno.IndexOf("> ") > -1)
            {
                cRetorno = cRetorno.Replace("> ",">");
                
                /*
                for (iControle = 0; iControle < cTexto.Length; iControle++)
                {
                    if (cRetorno.IndexOf("> ") > -1)
                    {
                        //cRetorno.Substring(iControle
                    }
                }
                 */
            }
            cRetorno = cRetorno.Replace(" />", "/>");
            return cRetorno;
        }




    }
}

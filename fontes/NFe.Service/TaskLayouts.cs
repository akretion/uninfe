using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using NFe.Components;
using NFe.Settings;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace NFe.Service
{
    public class TaskLayouts : TaskAbst
    {
        public override void Execute()
        {
            Exception exx = null;
            var uext = NFe.Components.Propriedade.Extensao(Components.Propriedade.TipoEnvio.pedLayouts);
            string ExtRetorno = (this.NomeArquivoXML.ToLower().EndsWith(".xml") ? uext.EnvioXML : uext.EnvioTXT);
            string finalArqErro = uext.EnvioXML.Replace(".xml", ".err");

            string pastaRetorno = Propriedade.PastaGeralRetorno;
            string arqRetorno = pastaRetorno + "\\" + Functions.ExtrairNomeArq(this.NomeArquivoXML, null) + Propriedade.Extensao(Propriedade.TipoEnvio.pedLayouts).RetornoXML;

            Functions.DeletarArquivo(arqRetorno);
            Functions.DeletarArquivo(Propriedade.PastaGeralRetorno + "\\" + Functions.ExtrairNomeArq(this.NomeArquivoXML, null) + finalArqErro);

            //Document document = new Document();
            try
            {
                CriaPDFLayout(arqRetorno);
            }
            catch (DocumentException de)
            {
                exx = de;
            }
            catch (IOException ioe)
            {
                exx = ioe;
            }
            catch(Exception ex)
            {
                exx = ex;
            }
            finally
            {
                Functions.DeletarArquivo(this.NomeArquivoXML);

                if (exx != null)
                {
                    Functions.DeletarArquivo(arqRetorno);

                    try
                    {
                        NFe.Service.TFunctions.GravarArqErroServico(this.NomeArquivoXML, ExtRetorno, finalArqErro, exx);
                    }
                    catch { }
                }
            }
        }

        public void CriaPDFLayout(string arqRetorno)
        {
                using (Document document = new Document(PageSize.A4, 20f, 20f, 80f, 10f))
                {
                    var pdfWriter = PdfWriter.GetInstance(document, new FileStream(arqRetorno, FileMode.Create));

                    pdfWriter.PageEvent = new ITextEvents();

                    document.AddTitle("Layout UniNFe");
                    document.AddSubject("Layout de arquivos texto");
                    document.AddCreator(Propriedade.DescricaoAplicacao);
                    document.AddAuthor(ConfiguracaoApp.NomeEmpresa);

                    //HeaderFooter header = new HeaderFooter(new Phrase("This is a header"), false);
                    //document.Header = header;

                    // landscape
                    document.SetPageSize(PageSize.A4.Rotate());

                    document.Open();

                    PdfPTable table = null;// new PdfPTable(3);

                    //document.Add(new Phrase("Extensões permitidas", new Font(Font.FontFamily.HELVETICA, 18)));
                    //Paragraph epara = new Paragraph("Extensões permitidas", new Font(Font.FontFamily.HELVETICA, 22));
                    //epara.Alignment = Element.ALIGN_CENTER;
                    //document.Add(epara);
                    //document.Add(new Phrase());

                    int n = 0;
                    foreach (Propriedade.TipoEnvio item in Enum.GetValues(typeof(Propriedade.TipoEnvio)))
                    {
                    if ((n % 16) == 0 || table == null)
                        {
                            if (table != null)
                            {
                                document.Add(table);
                                document.NewPage();
                            }

                            document.Add(new Phrase("Extensões permitidas" + (n > 0 ? "..." : ""), new Font(Font.FontFamily.HELVETICA, 18)));

                            table = new PdfPTable(3);
                            table.HorizontalAlignment = 0;
                            table.TotalWidth = document.PageSize.GetRight(41);
                            table.LockedWidth = true;
                            table.PaddingTop = 2;
                            table.SetWidths(new float[] { 40f, 40f, 100f });

                            table.AddCell(this.titulo("Envios"));
                            table.AddCell(this.titulo("Retornos"));
                            table.AddCell(this.titulo("Descrição"));
                        }
                        ++n;
                        var EXT = Propriedade.Extensao(item);
                        table.AddCell(this.texto(this.fmtname(EXT.EnvioXML) + this.fmtname(EXT.EnvioTXT," ou ")));
                        table.AddCell(this.texto(this.fmtname(EXT.RetornoXML) + this.fmtname(EXT.RetornoTXT," ou ")));
                        table.AddCell(this.texto(EXT.descricao));
                    }
                    if (table != null)
                    {
                        document.Add(table);
                        document.NewPage();
                    }
                    ///
                    /// Layout NFe/NFCe
                    /// 

                string old = "";

                    n = 0;
                    table = null;
                    //document.Add(new Phrase("Layout de notas NF-e/NFc-e", new Font(Font.FontFamily.HELVETICA, 18)));
                    foreach (KeyValuePair<string, string> key in new NFe.ConvertTxt.ConversaoTXT().LayoutTXT)
                    {
                    if (old == key.Value.ToString().Substring(1)) continue;

                    old = key.Value.ToString().Substring(1);

                        if (key.Key.Contains("_"))
                            ///
                            /// só considera o layout da versao >= 3.1
                            if (key.Key.Contains("_200")) continue;

                    if ((n % 20) == 0 || table == null)
                        {
                            if (table != null)
                            {
                                document.Add(table);
                                document.NewPage();
                            }
                            document.Add(new Phrase("Layout de notas NF-e/NFc-e" + (n>0 ? "...":""), new Font(Font.FontFamily.HELVETICA, 18)));
                            table = new PdfPTable(2);
                            table.AddCell(this.titulo("Segmento"));
                            table.AddCell(this.titulo("Formato"));
                            table.HorizontalAlignment = 0;
                            table.TotalWidth = document.PageSize.GetRight(41);
                            table.LockedWidth = true;
                            table.PaddingTop = 2;
                        table.SetWidths(new float[] { 15f, 100f });
                        }
                        ++n;
                    //if (key.Key.Contains("_"))
                    //    table.AddCell(this.texto(key.Key.Substring(0, key.Key.IndexOf("_"))));
                    //else
                            table.AddCell(this.texto(key.Key));
                        table.AddCell(this.texto(key.Value.ToString().Substring(1)));
                    }
                    if (table != null)
                    {
                        document.Add(table);
                        document.NewPage();
                    }

                    //for (int i = 0; i < 2; i++)
                    //{
                    //    Paragraph para = new Paragraph("Hello world. Checking Header Footer", new Font(Font.FontFamily.HELVETICA, 22));

                    //    para.Alignment = Element.ALIGN_CENTER;
                    //    document.Add(para);
                    //    document.NewPage();
                    //}

                    //document.NewPage();
                    //// step 4: we Add a paragraph to the document
                    //for (int i = 0; i < 2000; i++)
                    //{
                    //    if (( i % 36) == 0)
                    //        document.NewPage();

                    //    document.Add(new Paragraph("Hello World " + i.ToString(), new Font(Font.FontFamily.COURIER, 8)));
                    //}
                    //document.NewPage();
                    //document.Add(new Phrase("Phrase: Hello World, Hello Sun, Hello Moon, Hello Stars, Hello Sea, Hello Land, Hello People. "));

                    //Paragraph paragraph = new Paragraph();
                    //paragraph.Alignment = Element.ALIGN_JUSTIFIED;
                    //for (int i = 0; i < 2000; i++)
                    //{
                    //    //paragraph.Add("Hello World, Hello Sun, Hello Moon, Hello Stars, Hello Sea, Hello Land, Hello People. ");
                    //}
                    //document.Add(paragraph);

                    document.Close();
                }
            }

        string fmtname(string value, string prefix = "")
        {
            if (string.IsNullOrEmpty(value)) return "";
            if (value.StartsWith("-")) return prefix + "???" + value;
            return prefix + value;
        }
        
        PdfPCell titulo(string value)
        {
            PdfPCell cell = new PdfPCell(new Phrase(value, FontFactory.GetFont(FontFactory.COURIER, 10, Font.BOLD)));
            cell.BackgroundColor = new BaseColor(0xC0, 0xC0, 0xC0);
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_CENTER;
            return cell;
        }
        
        PdfPCell texto(string value)
        {
            PdfPCell cell = new PdfPCell(new Phrase(value, FontFactory.GetFont(FontFactory.COURIER, 10, Font.NORMAL)));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            return cell;
        }
    }
    public class ITextEvents : PdfPageEventHelper
    {
        // This is the contentbyte object of the writer
        PdfContentByte cb;

        // we will put the final number of pages in a template
        PdfTemplate headerTemplate, footerTemplate;

        // this is the BaseFont we are going to use for the header / footer
        BaseFont bf = null;

        // This keeps track of the creation time
        DateTime PrintTime = DateTime.Now;

        #region Fields
        private string _header;
        #endregion

        #region Properties
        public string Header
        {
            get { return _header; }
            set { _header = value; }
        }
        #endregion


        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            try
            {
                PrintTime = DateTime.Now;
                bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb = writer.DirectContent;
                headerTemplate = cb.CreateTemplate(100, 50);
                footerTemplate = cb.CreateTemplate(50, 50);
            }
            catch (DocumentException)
            {
            }
            catch (System.IO.IOException)
            {
            }
        }

        public override void OnEndPage(iTextSharp.text.pdf.PdfWriter writer, iTextSharp.text.Document document)
        {
            base.OnEndPage(writer, document);

            iTextSharp.text.Font baseFontNormal = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);

            //Create PdfTable object
            PdfPTable pdfTab = new PdfPTable(3);
            
            //Row 1
            Phrase p11 = null;
            try
            {
                Image img = Image.GetInstance(NFe.Components.Propriedade.PastaExecutavel + "\\logounimake.png");
                img.ScaleAbsolute(120, 25);
                //img.ScalePercent(10);//0,100);
                //img.ScaleAbsoluteHeight(20);
                //img.ScaleAbsoluteWidth(110);
                img.ScaleToFitHeight = false;
                //img.SetAbsolutePosition(100, 200);
                var ck = new Chunk(img, 0, 0);
                p11 = new Phrase(ck);
            }
            catch
            {
                p11 = new Phrase(ConfiguracaoApp.NomeEmpresa, baseFontNormal);
            }
            PdfPCell pdfCell1 = new PdfPCell(p11);
            pdfCell1.HorizontalAlignment = Element.ALIGN_LEFT;//.ALIGN_CENTER;
            pdfCell1.Border = 0;

            String text = "Página " + writer.PageNumber + " de ";


            //Add paging to header
            {
                //cb.BeginText();
                //cb.SetFontAndSize(bf, 10);
                //cb.SetTextMatrix(document.PageSize.GetRight(107), document.PageSize.GetTop(45));
                //cb.ShowText(text);
                //cb.EndText();
                //float len = bf.GetWidthPoint(text, 10);
                ////Adds "12" in Page 1 of 12
                //cb.AddTemplate(headerTemplate, document.PageSize.GetRight(107) + len, document.PageSize.GetTop(45));
            }
            //Add paging to footer
            {
                float p1 = 92;
                cb.BeginText();
                cb.SetFontAndSize(bf, 10);
                cb.SetTextMatrix(document.PageSize.GetRight(p1), document.PageSize.GetBottom(30));
                cb.ShowText(text);
                cb.EndText();

                cb.BeginText();
                cb.SetFontAndSize(bf, 10);
                cb.SetTextMatrix(20f, document.PageSize.GetBottom(30));
                cb.ShowText("Data da criação: " + PrintTime.ToLongDateString());
                cb.EndText();
                float len = bf.GetWidthPoint(text, 10);
                cb.AddTemplate(footerTemplate, document.PageSize.GetRight(p1) + len, document.PageSize.GetBottom(30));
            }
            //Row 2
            PdfPCell pdfCell4_SubTitle = new PdfPCell(new Phrase("Layouts arquivos texto", baseFontNormal));
            pdfCell4_SubTitle.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell4_SubTitle.VerticalAlignment = Element.ALIGN_TOP;
            pdfCell4_SubTitle.Border = 0;
            pdfCell4_SubTitle.Colspan = 3;
            //Row 3

            //iTextSharp.text.Font baseFontBig = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);

            //PdfPCell pdfCell5 = new PdfPCell(new Phrase("Data da criação:" + PrintTime.ToShortDateString(), baseFontBig));
            //pdfCell5.HorizontalAlignment = Element.ALIGN_LEFT;
            //pdfCell5.VerticalAlignment = Element.ALIGN_MIDDLE;
            //pdfCell5.Border = 0;

            //PdfPCell pdfCell6 = new PdfPCell();
            //pdfCell6.HorizontalAlignment = Element.ALIGN_CENTER;
            //pdfCell6.VerticalAlignment = Element.ALIGN_MIDDLE;
            //pdfCell6.Border = 0;

            //PdfPCell pdfCell7 = new PdfPCell(new Phrase("Hora:" + string.Format("{0:t}", PrintTime), baseFontBig));
            //pdfCell7.HorizontalAlignment = Element.ALIGN_RIGHT;
            //pdfCell7.VerticalAlignment = Element.ALIGN_MIDDLE;
            //pdfCell7.Border = 0;

            //set the alignment of all three cells and set border to 0
            PdfPCell pdfCell3 = new PdfPCell();
            pdfCell3.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell3.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell3.Border = 0;

            Phrase p1Header = new Phrase(ConfiguracaoApp.NomeEmpresa, baseFontNormal);
            PdfPCell pdfCell_p1Header = new PdfPCell(p1Header);
            pdfCell_p1Header.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell_p1Header.VerticalAlignment = Element.ALIGN_BOTTOM;
            pdfCell_p1Header.Border = 0;

            //add all three cells into PdfTable
            pdfTab.AddCell(pdfCell1);
            pdfTab.AddCell(pdfCell_p1Header);
            pdfTab.AddCell(pdfCell3);
            pdfTab.AddCell(pdfCell4_SubTitle);
            //pdfTab.AddCell(pdfCell5);
            //pdfTab.AddCell(pdfCell6);
            //pdfTab.AddCell(pdfCell7);

            pdfTab.TotalWidth = document.PageSize.Width - 80f;
            pdfTab.WidthPercentage = 70;
            //pdfTab.HorizontalAlignment = Element.ALIGN_CENTER;

            //call WriteSelectedRows of PdfTable. This writes rows from PdfWriter in PdfTable
            //first param is start row. -1 indicates there is no end row and all the rows to be included to write
            //Third and fourth param is x and y position to start writing
            pdfTab.WriteSelectedRows(0, -1, 40, document.PageSize.Height - 30, writer.DirectContent);
            //set pdfContent value

            cb.MoveTo(20, document.PageSize.Height - 80);
            cb.LineTo(document.PageSize.Width - 20, document.PageSize.Height - 80);
            cb.Stroke();


            //Move the pointer and draw line to separate header section from rest of page
            //cb.MoveTo(40, document.PageSize.Height - 100);
            //cb.LineTo(document.PageSize.Width - 40, document.PageSize.Height - 100);
            //cb.Stroke();

            //Move the pointer and draw line to separate footer section from rest of page
            cb.MoveTo(20, document.PageSize.GetBottom(50));
            cb.LineTo(document.PageSize.Width - 20, document.PageSize.GetBottom(50));
            cb.Stroke();
        }

        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);

            //headerTemplate.BeginText();
            //headerTemplate.SetFontAndSize(bf, 10);
            //headerTemplate.SetTextMatrix(0, 0);
            //headerTemplate.ShowText((writer.PageNumber - 1).ToString());
            //headerTemplate.EndText();

            footerTemplate.BeginText();
            footerTemplate.SetFontAndSize(bf, 10);
            footerTemplate.SetTextMatrix(0, 0);
            footerTemplate.ShowText((writer.PageNumber - 1).ToString());
            footerTemplate.EndText();
        }
    }
}

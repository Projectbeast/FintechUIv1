﻿using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.xml;
using iTextSharp.text.html;
using System.Data;
using System;
using System.Web;
using System.Web.UI;
using System.Text;
using System.IO;
using S3GBusEntity;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Collections.Generic;
/// <summary>
/// Created By          :   Shibu
/// Created Date        :   20-Jun-2013
/// Purpose             :   To set PDF Report Header And Footer

/// </summary>
public class PDFPageSetup : iTextSharp.text.pdf.PdfPageEventHelper
{
    private string _HeaderText;
    private string _PDFText;
    private bool _HeaderLogo;
    private string _FooterText;


    public string HeaderText    // the Header property
    {
        get
        {
            return _HeaderText;
        }
        set
        {
            _HeaderText = value;
        }
    }

    public string FooterText    // the Footer property
    {
        get
        {
            return _FooterText;
        }
        set
        {
            _FooterText = value;
        }
    }

    public bool HeaderLogo    // the Footer property
    {
        get
        {
            return _HeaderLogo;
        }
        set
        {
            _HeaderLogo = value;
        }
    }

    //public Pdfpagesetup()
    //{
    //    //
    //    // TODO: Add constructor logic here
    //    //
    //}
    //  public new Pdfpagesetup()
    // {
    // }
    // public new Pdfpagesetup(string sHeaderText)
    //{  
    //HeaderText = sHeaderText;
    //}
    public PDFPageSetup(string sHeaderText, bool sHeaderLog)
    {
        HeaderText = sHeaderText;
        HeaderLogo = sHeaderLog;
    }

    //override the OnStartPage event handler to add our header
    public override void OnStartPage(PdfWriter writer, Document doc)
    {
        PdfPTable headerTbl;
        if (_HeaderText != "")
        {
            iTextSharp.text.Font headerFont = FontFactory.GetFont("Segoe UI", 13, Font.BOLD, BaseColor.GRAY);
            if (_HeaderLogo == true)
            {
                Single[] sWidths = { Convert.ToSingle((doc.PageSize.Width * 80) / 100), Convert.ToSingle((doc.PageSize.Width * 20) / 100) };
                headerTbl = new PdfPTable(2);
                headerTbl.SetTotalWidth(sWidths);
            }
            else
            {
                headerTbl = new PdfPTable(1);
            }
            headerTbl.TotalWidth = doc.PageSize.Width;
            PdfPCell cell = new PdfPCell(new Phrase(_HeaderText, headerFont));
            if (_HeaderLogo == true)
            {
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.PaddingLeft = 20;
                cell.PaddingTop = 16;
                cell.VerticalAlignment = Element.ALIGN_TOP;
            }
            else
            {
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
            }
            cell.Border = 0;
            headerTbl.AddCell(cell);
            if (_HeaderLogo == true)
            {
                // iTextSharp.text.Image objImages = new iTextSharp.text.Image.GetInstance(HttpContext.Current.Request.PhysicalApplicationPath + "Images\\login\\s3g_logo.png");
                // objImages.ScalePercent(70.0F);
                cell = new PdfPCell(iTextSharp.text.Image.GetInstance(HttpContext.Current.Request.PhysicalApplicationPath + "Images/SfLogo.png"));
                //cell = new PdfPCell(objImages);
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.VerticalAlignment = Element.ALIGN_TOP;
                cell.PaddingRight = 16;
                cell.Border = 0;
                headerTbl.AddCell(cell);
            }
            //write the rows out to the PDF output stream. I use the height of the document to position the table. Positioning seems quite strange in iTextSharp and caused me the biggest headache.. It almost seems like it starts from the bottom of the page and works up to the top, so you may ned to play around with this.
            headerTbl.WriteSelectedRows(0, -1, 0, (doc.PageSize.Height - 10), writer.DirectContent);
        }

    }

    public override void OnEndPage(PdfWriter writer, Document doc)
    {
        iTextSharp.text.Font footerFont = FontFactory.GetFont("Segoe UI", 9, Font.NORMAL, BaseColor.DARK_GRAY);

        PdfPTable footerTbl = new PdfPTable(2);
        //'Dim sWidths As Single() = {CType((doc.PageSize.Width * 80) / 100, Single), CType((doc.PageSize.Width * 20) / 100, Single)}
        Single[] sWidths = { Convert.ToSingle((doc.PageSize.Width * 80) / 100), Convert.ToSingle((doc.PageSize.Width * 20) / 100) };

        footerTbl.SetTotalWidth(sWidths);
        //'set the width of the table to be the same as the document
        //'footerTbl.TotalWidth = doc.PageSize.Width

        footerTbl.HorizontalAlignment = Element.ALIGN_CENTER;
        Phrase sPhrase = new Phrase("This is an automated report from ", footerFont);
        iTextSharp.text.Font footerFontIbharname = FontFactory.GetFont("Segoe UI", 10, Font.ITALIC, BaseColor.DARK_GRAY);
        Chunk sChunk = new Chunk(" Sundaram Infotech. ", footerFontIbharname);
        Chunk sChunk1 = new Chunk(" Printed on  : " + System.DateTime.Now.ToString("dd-MMM-yyyy HH:mm"), footerFont);
        sPhrase.Add(sChunk);
        sPhrase.Add(sChunk1);
        Paragraph para = new Paragraph(sPhrase);
        PdfPCell cell = new PdfPCell(para);
        cell.Border = 0;
        cell.PaddingLeft = 10;
        footerTbl.AddCell(cell);


        para = new Paragraph("Page : " + doc.PageNumber, footerFont);
        cell = new PdfPCell(para);
        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        cell.Border = 0;
        cell.PaddingRight = 40;
        footerTbl.AddCell(cell);

        //write the rows out to the PDF output stream.
        footerTbl.WriteSelectedRows(0, -1, 20, (doc.BottomMargin) - 1, writer.DirectContent);
    }

    public static string FormatHTML(string strHTML)
    {
        try
        {

            //strHTML = strHTML.Replace("&nbsp;", "");
            strHTML = strHTML.Replace("MARGIN: 0in 0in 0pt", "");
            strHTML = strHTML.Replace("<o:p></o:p>", "");
            strHTML = strHTML.Replace("class=MsoNormal", "");

            MatchCollection matchesspan = System.Text.RegularExpressions.Regex.Matches(strHTML, "<span(.+?)>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            int spanprvlen = 0;
            foreach (Match match in matchesspan)
            {
                foreach (Capture capture in match.Captures)
                {
                    string align = "";
                    MatchCollection matchesstyle = System.Text.RegularExpressions.Regex.Matches(capture.Value, "<span.+?font-family:(.+?)>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                    foreach (Match match1 in matchesstyle)
                    {
                        align = match1.Groups[1].Value;
                    }

                    string rs = string.Empty;
                    if (!capture.Value.Contains("font-family:"))
                        rs = "<span>";
                    else
                        rs = "<span style='font-family:" + align + ">";

                    int capind = capture.Index - spanprvlen;
                    spanprvlen += capture.Length - rs.Length;
                    strHTML = strHTML.Remove(capind, capture.Length).Insert(capind, rs);
                }
            }

            MatchCollection matchestable = System.Text.RegularExpressions.Regex.Matches(strHTML, "<table(.+?)>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            int tablecount = 1;
            int tabprvlen = 0;
            foreach (Match match in matchestable)
            {
                foreach (Capture capture in match.Captures)
                {
                    string align = "";
                    MatchCollection matchesstyle = System.Text.RegularExpressions.Regex.Matches(capture.Value, "<table.+?border=(.+?)>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                    foreach (Match match1 in matchesstyle)
                    {
                        align = match1.Groups[1].Value;
                    }
                    string rs = string.Empty;
                    if (!capture.Value.Contains("border="))
                        rs = "<br /><table id='table" + tablecount + "' style='BORDER-COLLAPSE: collapse; COLOR: #000000' cellSpacing=0 cellPadding=1 border=0 width='100%'>";
                    else
                    {
                        if (align == "0")
                            rs = "<br /><table id='table" + tablecount + "' style='BORDER-COLLAPSE: collapse; COLOR: #000000' cellSpacing=0 cellPadding=1 border=0 width='100%'>";
                        else
                            rs = "<br /><table id='table" + tablecount + "' style='BORDER-COLLAPSE: collapse; COLOR: #000000' cellSpacing=0 cellPadding=1 rules=all border=" + align + " width='100%'>";

                    }
                    int capind = capture.Index - tabprvlen;

                    tabprvlen += capture.Length - rs.Length;
                    strHTML = strHTML.Remove(capind, capture.Length).Insert(capind, rs);
                    tablecount++;
                }
            }

            MatchCollection matchestr = System.Text.RegularExpressions.Regex.Matches(strHTML, "<tr(.+?)>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            int rowcount = 1;
            int rowprvlen = 0;
            foreach (Match match in matchestr)
            {
                foreach (Capture capture in match.Captures)
                {
                    int capind = capture.Index - rowprvlen;
                    string rs = "<tr id='row" + rowcount + "'>";
                    rowprvlen += capture.Length - rs.Length;
                    strHTML = strHTML.Remove(capind, capture.Length).Insert(capind, rs);
                    rowcount++;
                }
            }

            MatchCollection matchestd = System.Text.RegularExpressions.Regex.Matches(strHTML, "<td(.+?)>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            int tdprvlen = 0;
            foreach (Match match in matchestd)
            {
                foreach (Capture capture in match.Captures)
                {
                    string colspan = "";
                    MatchCollection matchesstyle = System.Text.RegularExpressions.Regex.Matches(capture.Value, "<td.+?colSpan=(.+?)>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                    foreach (Match match1 in matchesstyle)
                    {
                        colspan = match1.Groups[1].Value;
                    }

                    string vAlign = "";
                    MatchCollection matchesstylevAlign = System.Text.RegularExpressions.Regex.Matches(capture.Value, "<td.+?vAlign=(.+?)>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    foreach (Match match1 in matchesstylevAlign)
                    {
                        vAlign = match1.Groups[1].Value;
                        //vAlign = match1.Groups[1].Value.Split(' ')[0];
                    }

                    string rs = string.Empty;

                    if (!capture.Value.Contains("colSpan"))
                        rs = "<td";
                    else
                        rs = "<td colSpan=" + colspan;

                    if (!capture.Value.Contains("vAlign"))
                        rs = rs + ">";
                    else
                        rs = rs + " vAlign=" + vAlign + ">";

                    int capind = capture.Index - tdprvlen;
                    tdprvlen += capture.Length - rs.Length;
                    strHTML = strHTML.Remove(capind, capture.Length).Insert(capind, rs);

                }
            }

            MatchCollection matchesspantag = System.Text.RegularExpressions.Regex.Matches(strHTML, "<span></SPAN>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            int empspanprvlen = 0;
            foreach (Match match in matchesspantag)
            {
                foreach (Capture capture in match.Captures)
                {
                    string rs = "";
                    int capind = capture.Index - empspanprvlen;
                    empspanprvlen += capture.Length - rs.Length;
                    strHTML = strHTML.Remove(capind, capture.Length).Insert(capind, rs);
                }
            }

            MatchCollection matchesdiv = System.Text.RegularExpressions.Regex.Matches(strHTML, "<div(.+?)>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            int divprvlen = 0;
            foreach (Match match in matchesdiv)
            {
                foreach (Capture capture in match.Captures)
                {
                    string align = "";
                    MatchCollection matchesstyle = System.Text.RegularExpressions.Regex.Matches(capture.Value, "<div.+?align=(.+?)>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                    foreach (Match match1 in matchesstyle)
                    {
                        align = match1.Groups[1].Value;
                    }

                    string rs = string.Empty;
                    if (!capture.Value.Contains("align"))
                        rs = "<div>";
                    else
                        rs = "<div style='text-align:" + align + "'>";

                    int capind = capture.Index - divprvlen;
                    divprvlen += capture.Length - rs.Length;
                    strHTML = strHTML.Remove(capind, capture.Length).Insert(capind, rs);
                }
            }

            MatchCollection matchesp = System.Text.RegularExpressions.Regex.Matches(strHTML, "<p(.+?)>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            int pprvlen = 0;
            foreach (Match match in matchesp)
            {
                foreach (Capture capture in match.Captures)
                {
                    string align = "";
                    MatchCollection matchesstyle = System.Text.RegularExpressions.Regex.Matches(capture.Value, "<p.+?align=(.+?)>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                    foreach (Match match1 in matchesstyle)
                    {
                        align = match1.Groups[1].Value;
                    }

                    string rs = string.Empty;
                    if (!capture.Value.Contains("align"))
                        rs = "<div>";
                    else
                        rs = "<div style='text-align:" + align + "'>";

                    int capind = capture.Index - pprvlen;
                    pprvlen += capture.Length - rs.Length;
                    strHTML = strHTML.Remove(capind, capture.Length).Insert(capind, rs);
                }
            }

            MatchCollection matchesli = System.Text.RegularExpressions.Regex.Matches(strHTML, "<li.+?style=(.+?)>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            int liprvlen = 0;
            foreach (Match match in matchesli)
            {
                foreach (Capture capture in match.Captures)
                {
                    string rs = "<li style='MARGIN: 0in 0in 10pt;'>";
                    int capind = capture.Index - liprvlen;
                    liprvlen += capture.Length - rs.Length;
                    strHTML = strHTML.Remove(capind, capture.Length).Insert(capind, rs);
                }
            }

            MatchCollection matchesblockquote = System.Text.RegularExpressions.Regex.Matches(strHTML, "<blockquote.+?style=(.+?)>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            int bcprvlen = 0;
            foreach (Match match in matchesblockquote)
            {
                foreach (Capture capture in match.Captures)
                {
                    string rs = "";
                    int capind = capture.Index - bcprvlen;
                    bcprvlen += capture.Length - rs.Length;
                    strHTML = strHTML.Remove(capind, capture.Length).Insert(capind, rs);
                }
            }

            MatchCollection matchesxml = System.Text.RegularExpressions.Regex.Matches(strHTML, "<?xml(.+?)>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            int xmlprvlen = 0;
            foreach (Match match in matchesxml)
            {
                foreach (Capture capture in match.Captures)
                {
                    string rs = "";
                    int capind = capture.Index - xmlprvlen;
                    xmlprvlen += capture.Length - rs.Length;
                    strHTML = strHTML.Remove(capind, capture.Length).Insert(capind, rs);
                }
            }

            MatchCollection matchesimg = System.Text.RegularExpressions.Regex.Matches(strHTML, "<img(.+?)>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            int imgcount = 1;
            int imgprvlen = 0;
            foreach (Match match in matchesimg)
            {
                foreach (Capture capture in match.Captures)
                {
                    string rs = "<img id='img" + imgcount + "' />";
                    int capind = capture.Index - imgprvlen;
                    imgprvlen += capture.Length - rs.Length;
                    strHTML = strHTML.Remove(capind, capture.Length).Insert(capind, rs);
                    imgcount++;
                }
            }

            strHTML = strHTML.Replace("</P>", "</div>");
            strHTML = strHTML.Replace("</BLOCKQUOTE>", "");
            strHTML = strHTML.Replace("<FONT size=2> </FONT>", "<br />");
            strHTML = strHTML.Replace("<span><span><o:p>", "");
            strHTML = strHTML.Replace("</o:p></SPAN></SPAN>", "");
            strHTML = strHTML.Replace("<?", "");
            strHTML = strHTML.Replace("<>", "");
            strHTML = strHTML.Replace("</P>", "</p>");
            strHTML = strHTML.Replace("<BR>", "<br />");

            strHTML = strHTML.Replace("</P>", "</p>");
            strHTML = strHTML.Replace("</SPAN>", "</span>");
            strHTML = strHTML.Replace("<STRONG>", "<strong>");
            strHTML = strHTML.Replace("</STRONG>", "</strong>");
            strHTML = strHTML.Replace("</TABLE>", "</table>");
            strHTML = strHTML.Replace("<TBODY>", "<tbody>");
            strHTML = strHTML.Replace("</TBODY>", "</tbody>");
            strHTML = strHTML.Replace("</TD>", "</td>");
            strHTML = strHTML.Replace("<FONT", "<font");
            strHTML = strHTML.Replace("</FONT>", "</font>");
            strHTML = strHTML.Replace("</TR>", "</tr>");
            strHTML = strHTML.Replace("<DIV", "<div");
            strHTML = strHTML.Replace("</DIV>", "</div>");
            strHTML = strHTML.Replace("<LI", "<li");
            strHTML = strHTML.Replace("<UL", "<ul");
            strHTML = strHTML.Replace("<B", "<b");
            strHTML = strHTML.Replace("</B>", "</b>");
            strHTML = strHTML.Replace("</table>", "</table><br />");
            strHTML = strHTML.Replace("</table>", "</table><br />");
            strHTML = strHTML.Replace("<b></b>", "");
            strHTML = strHTML.Replace("<div style='text-align:justify'></div>", "<br />");
            strHTML = strHTML.Replace("<div style='text-align:left'></div>", "<br />");
            strHTML = strHTML.Replace("<div style='text-align:right'></div>", "<br />");
            strHTML = strHTML.Replace("<div style='text-align:center'></div>", "<br />");
            strHTML = "<table rules=all width='100%'><tr><td>" + strHTML + "</td></tr></table>";
            return strHTML;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    public static byte[] AddPageNumberForPDF(byte[] pdf)
    {
        try
        {
            MemoryStream ms = new MemoryStream();
            PdfReader reader = new PdfReader(pdf);
            int n = reader.NumberOfPages;
            Rectangle psize = reader.GetPageSize(1);
            Document document = new Document(psize, 25f, 25f, 25f, 25f);
            PdfWriter writer = PdfWriter.GetInstance(document, ms);
            document.Open();
            PdfContentByte cb = writer.DirectContent;

            int p = 0;
            for (int page = 1; page <= reader.NumberOfPages; page++)
            {
                document.NewPage();
                p++;

                PdfImportedPage importedPage = writer.GetImportedPage(reader, page);
                cb.AddTemplate(importedPage, 0, 0);

                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_BOLDITALIC, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb.BeginText();
                cb.SetFontAndSize(bf, 12);
                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, +p + " / " + n, (((iTextSharp.text.Rectangle)(psize)).Width) / 2, 10, 0);
                cb.EndText();
            }
            document.Close();
            return ms.ToArray();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    public static void MergePDFs(string[] fileNames, string outFile)
    {
        try
        {

            Document document = new Document();
            PdfCopy writer = new PdfCopy(document, new FileStream(outFile, FileMode.Create));
            if (writer == null)
            {
                return;
            }
            document.Open();

            foreach (string fileName in fileNames)
            {
                PdfReader reader = new PdfReader(fileName);
                reader.ConsolidateNamedDestinations();
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    PdfImportedPage page = writer.GetImportedPage(reader, i);
                    writer.AddPage(page);
                }

                PRAcroForm form = reader.AcroForm;
                if (form != null)
                {
                    writer.CopyAcroForm(reader);
                }

                reader.Close();
            }
            writer.Close();
            document.Close();
            foreach (string filePath in fileNames)
                File.Delete(filePath);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    public static void CopyPageSetupForWord(Microsoft.Office.Interop.Word.PageSetup source, Microsoft.Office.Interop.Word.PageSetup target)
    {
        target.PaperSize = source.PaperSize;

        if (!source.Orientation.Equals(target.Orientation))
            target.TogglePortrait();

        target.TopMargin = source.TopMargin;
        target.BottomMargin = source.BottomMargin;
        target.RightMargin = source.RightMargin;
        target.LeftMargin = source.LeftMargin;
        target.FooterDistance = source.FooterDistance;
        target.HeaderDistance = source.HeaderDistance;
        target.LayoutMode = source.LayoutMode;
    }

    public static Microsoft.Office.Interop.Word._Document SetWordProperties(Microsoft.Office.Interop.Word._Document doc)
    {
        try
        {
            doc.PageSetup.LeftMargin = 5f;

            doc.PageSetup.RightMargin = doc.PageSetup.TopMargin = doc.PageSetup.BottomMargin = 0f;
            doc.PageSetup.HeaderDistance = 0f;
            doc.PageSetup.FooterDistance = 0f;
            return doc;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    public static Microsoft.Office.Interop.Word._Document SetWordProperties(Microsoft.Office.Interop.Word._Document doc, List<string> ObjPageSetup)
    {
        try
        {
            doc.PageSetup.LeftMargin = float.Parse(ObjPageSetup[0].ToString(), CultureInfo.InvariantCulture.NumberFormat);
            doc.PageSetup.RightMargin = float.Parse(ObjPageSetup[1].ToString(), CultureInfo.InvariantCulture.NumberFormat);
            doc.PageSetup.TopMargin = float.Parse(ObjPageSetup[2].ToString(), CultureInfo.InvariantCulture.NumberFormat);
            doc.PageSetup.BottomMargin = float.Parse(ObjPageSetup[3].ToString(), CultureInfo.InvariantCulture.NumberFormat);
            doc.PageSetup.HeaderDistance = float.Parse(ObjPageSetup[4].ToString(), CultureInfo.InvariantCulture.NumberFormat);
            doc.PageSetup.FooterDistance = float.Parse(ObjPageSetup[5].ToString(), CultureInfo.InvariantCulture.NumberFormat);
            return doc;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    //public static string FunPubGetTemplateContent(int CompanyID, int LobID, int LocationID, int TemplateTypeCode, string Reference_ID)
    //{
    //    try
    //    {
    //        DataTable dt = new DataTable();
    //        Dictionary<string, string> Procparam = new Dictionary<string, string>();
    //        Procparam.Add("@Company_Id", CompanyID.ToString());
    //        Procparam.Add("@Lob_Id", LobID.ToString());
    //        Procparam.Add("@Location_ID", LocationID.ToString());
    //        Procparam.Add("@Template_Type_Code", TemplateTypeCode.ToString());
    //        Procparam.Add("@Reference_ID", Reference_ID);
    //        dt = Utility.GetDefaultData("S3G_Get_TemplateCont", Procparam);

    //        if (dt.Rows.Count == 0)
    //        {
    //            return "";
    //        }
    //        else
    //            return "<META content='text/html; charset=utf-8' http-equiv='Content-Type'><TABLE><TR><TD> " + dt.Rows[0]["Template_Content"].ToString() + "</TD></TR></TABLE>";
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
    //        throw ex;
    //    }
    //}

    public static string FunPubGetTemplateContent(int CompanyID, int Program_ID, int language, int TemplateTypeCode, string Reference_ID)
    {
        try
        {
            DataTable dt = new DataTable();
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_Id", CompanyID.ToString());
            Procparam.Add("@Program_ID", Program_ID.ToString());
            Procparam.Add("@language", language.ToString());
            Procparam.Add("@Template_Type_Code", TemplateTypeCode.ToString());
            Procparam.Add("@Tran_ID", Reference_ID);
            dt = Utility.GetDefaultData("S3G_GET_TEMPLATECONTENT", Procparam);

            if (dt.Rows.Count == 0)
            {
                return "";
            }
            else
                return "<META content='text/html; charset=utf-8' http-equiv='Content-Type'><TABLE><TR><TD> " + dt.Rows[0]["Template_Content"].ToString() + "</TD></TR></TABLE>";
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }


    public static string FunPubGetTemplateContent_FA(int CompanyID, int LobID, int LocationID, int TemplateTypeCode, string Reference_ID)
    {
        try
        {
            DataTable dt = new DataTable();
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_Id", CompanyID.ToString());
            Procparam.Add("@Lob_Id", LobID.ToString());
            Procparam.Add("@Location_ID", LocationID.ToString());
            Procparam.Add("@Template_Type_Code", TemplateTypeCode.ToString());
            Procparam.Add("@Reference_ID", Reference_ID);
            dt = Utility_FA.GetDefaultData("FA_Get_TemplateCont", Procparam);

            if (dt.Rows.Count == 0)
            {
                return "";
            }
            else
                return "<META content='text/html; charset=utf-8' http-equiv='Content-Type'><TABLE><TR><TD> " + dt.Rows[0]["Template_Content"].ToString() + "</TD></TR></TABLE>";
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    public static string FunPubGetTemplateFromMaster(int CompanyID, int LobID, string LocationCode, int TemplateTypeCode, string Reference_ID, int LANGUAGE)
    {
        try
        {
            DataTable dt = new DataTable();
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_Id", CompanyID.ToString());
            Procparam.Add("@Lob_Id", LobID.ToString());
            Procparam.Add("@Location_ID", LocationCode);
            Procparam.Add("@Template_Type_Code", TemplateTypeCode.ToString());
            Procparam.Add("@Reference_ID", Reference_ID);
            Procparam.Add("@LANGUAGE", LANGUAGE.ToString());
            dt = Utility.GetDefaultData("S3G_Get_TemplateCont", Procparam);

            if (dt.Rows.Count == 0)
            {
                return "";
            }
            else
                return "<META content='text/html; charset=utf-8' http-equiv='Content-Type'><TABLE><TR><TD> " + dt.Rows[0]["Template_Content"].ToString() + "</TD></TR></TABLE>";
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    public static string FunPubGetDocumentPath(int CompanyID, int ProgramID)
    {
        try
        {
            DataTable dt = new DataTable();
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", CompanyID.ToString());
            Procparam.Add("@Program_ID", ProgramID.ToString());
            dt = Utility.GetDefaultData("S3G_Get_DocumentationPath", Procparam);

            if (dt.Rows.Count == 0)
            {
                return "";
            }
            else
                return dt.Rows[0]["Document_Path"].ToString();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    public static string FunPubGetFileName(string FileName)
    {
        try
        {
            return FileName.Replace(' ', '_').Replace('/', '_').Replace('-', '_').Replace(@"\", "/");
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    public static string FunPubBindTable(string strtblName, string strHTML, DataTable dtHeader)
    {
        try
        {
            string row = "";
            string newtr = String.Empty;
            var startTag = "";
            var endTag = "";
            int startIndex = 0;
            int endIndex = 0;
            string strrow = "";
            string strTable;

            startTag = strtblName;
            endTag = "</table>";
            startIndex = strHTML.LastIndexOf("<table", strHTML.IndexOf(startTag) + startTag.Length);
            endIndex = strHTML.IndexOf(endTag, startIndex) + endTag.Length;
            strTable = strHTML.Substring(startIndex, endIndex - startIndex);
            string strtempTable = strTable;


            startTag = "<tr";
            endTag = "</tr>";
            startIndex = strtempTable.IndexOf(startTag);
            endIndex = strtempTable.IndexOf(endTag, startIndex) + endTag.Length;
            strrow = strtempTable.Substring(startIndex, endIndex - startIndex);
            strtempTable = strtempTable.Replace(strrow, "");
            strHTML = strHTML.Replace(strTable, strtempTable);

            startTag = "<tr";
            endTag = "</tr>";
            startIndex = strtempTable.IndexOf(startTag);
            endIndex = strtempTable.IndexOf(endTag, startIndex) + endTag.Length;
            strrow = strtempTable.Substring(startIndex, endIndex - startIndex);
            strtempTable = strtempTable.Replace(strrow, "");

            startTag = "<tr";
            endTag = "</tr>";
            startIndex = strtempTable.IndexOf(startTag);
            endIndex = strtempTable.IndexOf(endTag, startIndex) + endTag.Length;
            row = strtempTable.Substring(startIndex, endIndex - startIndex);

            for (int i = 0; i < dtHeader.Rows.Count; i++)
            {

                string tr = row;
                DataRow dr = dtHeader.NewRow();
                foreach (DataColumn dcol in dtHeader.Columns)
                {
                    dr = dtHeader.Rows[i];
                    string ColName1 = string.Empty;
                    ColName1 = "~" + dcol.ColumnName + "~";
                    if (tr.Contains(ColName1))
                        tr = tr.Replace(ColName1, dr[dcol].ToString());
                }
                newtr = newtr + " " + tr;


            }
            strHTML = strHTML.Replace(row, newtr);
            return strHTML;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    public static string FunPubBindTableUpper(string strtblName, string strHTML, DataTable dtHeader)
    {
        try
        {
            string row = "";
            string newtr = String.Empty;
            var startTag = "";
            var endTag = "";
            int startIndex = 0;
            int endIndex = 0;
            string strrow = "";
            string strTable;

            startTag = strtblName;
            endTag = "</TABLE>";
            startIndex = strHTML.LastIndexOf("<TABLE", strHTML.IndexOf(startTag) + startTag.Length);
            endIndex = strHTML.IndexOf(endTag, startIndex) + endTag.Length;
            strTable = strHTML.Substring(startIndex, endIndex - startIndex);
            string strtempTable = strTable;


            startTag = "<TR";
            endTag = "</TR>";
            startIndex = strtempTable.IndexOf(startTag);
            endIndex = strtempTable.IndexOf(endTag, startIndex) + endTag.Length;
            strrow = strtempTable.Substring(startIndex, endIndex - startIndex);
            strtempTable = strtempTable.Replace(strrow, "");
            strHTML = strHTML.Replace(strTable, strtempTable);

            startTag = "<TR";
            endTag = "</TR>";
            startIndex = strtempTable.IndexOf(startTag);
            endIndex = strtempTable.IndexOf(endTag, startIndex) + endTag.Length;
            strrow = strtempTable.Substring(startIndex, endIndex - startIndex);
            strtempTable = strtempTable.Replace(strrow, "");

            startTag = "<TR";
            endTag = "</TR>";
            startIndex = strtempTable.IndexOf(startTag);
            endIndex = strtempTable.IndexOf(endTag, startIndex) + endTag.Length;
            row = strtempTable.Substring(startIndex, endIndex - startIndex);

            for (int i = 0; i < dtHeader.Rows.Count; i++)
            {

                string tr = row;
                DataRow dr = dtHeader.NewRow();
                foreach (DataColumn dcol in dtHeader.Columns)
                {
                    dr = dtHeader.Rows[i];
                    string ColName1 = string.Empty;
                    ColName1 = "~" + dcol.ColumnName + "~";
                    if (tr.Contains(ColName1))
                        tr = tr.Replace(ColName1, dr[dcol].ToString());
                }
                newtr = newtr + " " + tr;


            }
            strHTML = strHTML.Replace(row, newtr);
            return strHTML;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    public static string FunPubBindTableWithMerge(string strtblName, string strHTML, DataTable dtHeader)
    {
        try
        {
            string row = "";
            string newtr = String.Empty;
            var startTag = "";
            var endTag = "";
            int startIndex = 0;
            int endIndex = 0;
            string strrow = "";
            string strTable;

            startTag = strtblName;
            endTag = "</TABLE>";
            startIndex = strHTML.LastIndexOf("<TABLE", strHTML.IndexOf(startTag) + startTag.Length);
            endIndex = strHTML.IndexOf(endTag, startIndex) + endTag.Length;
            strTable = strHTML.Substring(startIndex, endIndex - startIndex);
            string strtempTable = strTable;

            if (strtempTable.Contains("~DYRS~"))
            {
                startTag = "<TR";
                endTag = "</TR>";
                startIndex = strtempTable.IndexOf(startTag);
                endIndex = strtempTable.IndexOf(endTag, startIndex) + endTag.Length;
                strrow = strtempTable.Substring(startIndex, endIndex - startIndex);
                strtempTable = strtempTable.Replace(strrow, "");

                startTag = "~DYRS~";
                endTag = "</TR>";
                startIndex = strtempTable.LastIndexOf("<TR", strtempTable.IndexOf(startTag) + startTag.Length);
                endIndex = strtempTable.IndexOf(endTag, startIndex) + endTag.Length;
                strrow = strtempTable.Substring(startIndex, endIndex - startIndex);
                row = strrow.Replace("~DYRS~", "");
                strtempTable = strtempTable.Replace("~DYRS~", "");

                strHTML = strHTML.Replace(strTable, strtempTable);
            }
            else
            {
                startTag = "<TR";
                endTag = "</TR>";
                startIndex = strtempTable.IndexOf(startTag);
                endIndex = strtempTable.IndexOf(endTag, startIndex) + endTag.Length;
                strrow = strtempTable.Substring(startIndex, endIndex - startIndex);
                strtempTable = strtempTable.Replace(strrow, "");
                strHTML = strHTML.Replace(strTable, strtempTable);

                startTag = "<TR";
                endTag = "</TR>";
                startIndex = strtempTable.IndexOf(startTag);
                endIndex = strtempTable.IndexOf(endTag, startIndex) + endTag.Length;
                strrow = strtempTable.Substring(startIndex, endIndex - startIndex);
                strtempTable = strtempTable.Replace(strrow, "");

                startTag = "<TR";
                endTag = "</TR>";
                startIndex = strtempTable.IndexOf(startTag);
                endIndex = strtempTable.IndexOf(endTag, startIndex) + endTag.Length;
                row = strtempTable.Substring(startIndex, endIndex - startIndex);
            }
            string strtempTD = string.Empty;
            string strTD = string.Empty;

            for (int i = 0; i < dtHeader.Rows.Count; i++)
            {

                string tr = row;
                if (strtempTable.Contains("~RSPN~"))
                {
                    if (i == 0)
                    {
                        startTag = "~RSPN~";
                        endTag = "</TD>";
                        startIndex = tr.LastIndexOf("<TD", tr.IndexOf(startTag) + startTag.Length);
                        endIndex = tr.IndexOf(endTag, startIndex) + endTag.Length;
                        strTD = tr.Substring(startIndex, endIndex - startIndex);
                        strtempTD = strTD.Replace("<TD", "<TD rowspan = " + dtHeader.Rows.Count + " "); ;
                        tr = tr.Replace(strTD, strtempTD);
                        tr = tr.Replace("~RSPN~", "");
                    }
                    else
                    {
                        startTag = "~RSPN~";
                        endTag = "</TD>";
                        startIndex = tr.LastIndexOf("<TD", tr.IndexOf(startTag) + startTag.Length);
                        endIndex = tr.IndexOf(endTag, startIndex) + endTag.Length;
                        strTD = tr.Substring(startIndex, endIndex - startIndex);
                        tr = tr.Replace(strTD, "");
                    }
                }

                DataRow dr = dtHeader.NewRow();
                foreach (DataColumn dcol in dtHeader.Columns)
                {
                    dr = dtHeader.Rows[i];
                    string ColName1 = string.Empty;
                    ColName1 = "~" + dcol.ColumnName + "~";
                    if (tr.Contains(ColName1))
                        tr = tr.Replace(ColName1, dr[dcol].ToString());
                }
                newtr = newtr + " " + tr;
            }
            strHTML = strHTML.Replace(row, newtr);
            return strHTML;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    public static string FunPubBindCommonVariables(string str, DataTable dtCommonHeader)
    {
        try
        {
            DataRow dr = dtCommonHeader.NewRow();
            foreach (DataColumn dcol in dtCommonHeader.Columns)
            {
                dr = dtCommonHeader.Rows[0];
                string ColName1 = string.Empty;
                ColName1 = "~" + dcol.ColumnName + "~";
                if (str.Contains(ColName1))
                    str = str.Replace(ColName1, dr[dcol].ToString());
            }
            return str;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    public static string FunPubBindImages(List<string> listImageName, List<string> listImagePath, string strHTML)
    {
        try
        {
            for (int i = 0; i < listImageName.Count; i++)
            {
                string ImageTag = "<img src='" + listImagePath[i].ToString() + "' alt='Image'>";
                if (strHTML.Contains(listImageName[i].ToString()))
                    strHTML = strHTML.Replace(listImageName[i].ToString(), ImageTag);
            }
            return strHTML;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    public static string FunPubDeleteTable(string strtblName, string strHTML)
    {
        try
        {
            string newtr = String.Empty;
            var startTag = "";
            var endTag = "";
            int startIndex = 0;
            int endIndex = 0;
            string strTable;

            startTag = strtblName;
            endTag = "</TABLE>";
            startIndex = strHTML.LastIndexOf("<TABLE", strHTML.IndexOf(startTag) + startTag.Length);
            endIndex = strHTML.IndexOf(endTag, startIndex) + endTag.Length;
            strTable = strHTML.Substring(startIndex, endIndex - startIndex);

            strHTML = strHTML.Replace(strTable, "");
            return strHTML;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    public static void FunPubSaveDocument(string strHTML, string FilePath, string FileName, string DocumentType, int IDummy, string DateFormat)
    {
        try
        {
            string strhtmlFile = FilePath + "\\" + FileName + ".html";
            string strwordFile = FilePath + "\\" + FileName + ".doc";
            string strpdfFile = FilePath + "\\" + FileName + ".pdf";
            object file = strhtmlFile;
            object oMissing = System.Reflection.Missing.Value;
            object readOnly = false;
            object oFalse = false;
            object fileFormat = null;
            Microsoft.Office.Interop.Word._Application oWord = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word._Document oDoc = new Microsoft.Office.Interop.Word.Document();

            if (!Directory.Exists(FilePath))
            {
                Directory.CreateDirectory(FilePath);
            }

            try
            {
                if (File.Exists(strhtmlFile) == true)
                {
                    File.Delete(strhtmlFile);
                }
                if (File.Exists(strwordFile) == true)
                {
                    File.Delete(strwordFile);
                }
                File.WriteAllText(strhtmlFile, strHTML);

                oDoc = oWord.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                oDoc = oWord.Documents.Open(ref file, ref oMissing, ref readOnly, ref oMissing, ref oMissing, ref oMissing, ref oMissing
                    , ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                oDoc = PDFPageSetup.SetWordProperties(oDoc);

                //oDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
                //oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;
                //oDoc.ActiveWindow.Selection.Fields.Add(oDoc.ActiveWindow.Selection.Range, Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage, ref oMissing, ref oMissing);
                //oDoc.ActiveWindow.Selection.TypeText("\t");
                //oDoc.ActiveWindow.Selection.TypeText("           ");
                //string footerimagepath = HttpContext.Current.Server.MapPath("../Images/TemplateImages/1/OPCFooter.png");
                //oDoc.ActiveWindow.Selection.InlineShapes.AddPicture(footerimagepath, oMissing, true, oMissing);

                //Added on 05-Jul-2018 Starts here
                //To Add Page numbers to the document



                oDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
                oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;
                oDoc.ActiveWindow.Selection.Font.Name = "Arial";
                oDoc.ActiveWindow.Selection.Font.Size = 8;
                oDoc.ActiveWindow.Selection.TypeText(DateTime.Now.ToString(DateFormat + "   HH:mm:ss") + "                                                                                                                                                                                             ");
                oWord.ActiveWindow.Selection.Fields.Add(oWord.Selection.Range, ref oMissing, ref oMissing, ref oMissing);

                oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                oDoc.ActiveWindow.Selection.Font.Name = "Arial";
                oDoc.ActiveWindow.Selection.Font.Size = 8;
                oDoc.ActiveWindow.Selection.TypeText("Page ");
                Object CurrentPage = Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage;
                oDoc.ActiveWindow.Selection.Fields.Add(oWord.Selection.Range, ref CurrentPage, ref oMissing, ref oMissing);
                oDoc.ActiveWindow.Selection.TypeText(" of ");
                Object TotalPages = Microsoft.Office.Interop.Word.WdFieldType.wdFieldNumPages;
                oWord.ActiveWindow.Selection.Fields.Add(oWord.Selection.Range, ref TotalPages, ref oMissing, ref oMissing);


                //Add Date Time




                //Added on 05-Jul-2018 Ends here

                if (DocumentType == "P")
                {
                    fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF;
                    file = strpdfFile;
                }
                else if (DocumentType == "W")
                {
                    fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDocument;
                    file = strwordFile;
                }

                oDoc.SaveAs(ref file, ref fileFormat, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing
                    , ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);

                //RemoveBlankPage(strwordFile);
            }
            catch (Exception objException)
            {
                ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
                throw objException;
            }
            finally
            {
                oDoc.Close(ref oFalse, ref oMissing, ref oMissing);
                oWord.Quit(ref oMissing, ref oMissing, ref oMissing);
                File.Delete(strhtmlFile);
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    public static void FunPubSaveDocument(string strHTML, string FilePath, string FileName, string DocumentType, int IDummy, string DateFormat, string IDummyFile, string FooterNote)
    {
        try
        {
            string strhtmlFile = FilePath + "\\" + FileName + ".html";
            string strwordFile = FilePath + "\\" + FileName + ".doc";
            string strpdfFile = FilePath + "\\" + FileName + ".pdf";
            object file = strhtmlFile;
            object oMissing = System.Reflection.Missing.Value;
            object readOnly = false;
            object oFalse = false;
            object fileFormat = null;
            Microsoft.Office.Interop.Word._Application oWord = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word._Document oDoc = new Microsoft.Office.Interop.Word.Document();

            if (!Directory.Exists(FilePath))
            {
                Directory.CreateDirectory(FilePath);
            }

            try
            {
                if (File.Exists(strhtmlFile) == true)
                {
                    File.Delete(strhtmlFile);
                }
                if (File.Exists(strwordFile) == true)
                {
                    File.Delete(strwordFile);
                }
                File.WriteAllText(strhtmlFile, strHTML);

                oDoc = oWord.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                oDoc = oWord.Documents.Open(ref file, ref oMissing, ref readOnly, ref oMissing, ref oMissing, ref oMissing, ref oMissing
                    , ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                oDoc = PDFPageSetup.SetWordProperties(oDoc);

                //oDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
                //oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;
                //oDoc.ActiveWindow.Selection.Fields.Add(oDoc.ActiveWindow.Selection.Range, Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage, ref oMissing, ref oMissing);
                //oDoc.ActiveWindow.Selection.TypeText("\t");
                //oDoc.ActiveWindow.Selection.TypeText("           ");
                //string footerimagepath = HttpContext.Current.Server.MapPath("../Images/TemplateImages/1/OPCFooter.png");
                //oDoc.ActiveWindow.Selection.InlineShapes.AddPicture(footerimagepath, oMissing, true, oMissing);

                //Added on 05-Jul-2018 Starts here
                //To Add Page numbers to the document



                oDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
                oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;
                oDoc.ActiveWindow.Selection.Font.Name = "Arial";
                oDoc.ActiveWindow.Selection.Font.Size = 8;
                oDoc.ActiveWindow.Selection.TypeText(DateTime.Now.ToString(DateFormat + "   HH:mm:ss") + "                                                                                                                                                                                             ");
                oWord.ActiveWindow.Selection.Fields.Add(oWord.Selection.Range, ref oMissing, ref oMissing, ref oMissing);

                oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                oDoc.ActiveWindow.Selection.Font.Name = "Arial";
                oDoc.ActiveWindow.Selection.Font.Size = 8;

                //oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;
                oDoc.ActiveWindow.Selection.TypeText("Page ");
                Object CurrentPage = Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage;
                oDoc.ActiveWindow.Selection.Fields.Add(oWord.Selection.Range, ref CurrentPage, ref oMissing, ref oMissing);
                oDoc.ActiveWindow.Selection.TypeText(" of ");
                Object TotalPages = Microsoft.Office.Interop.Word.WdFieldType.wdFieldNumPages;
                oWord.ActiveWindow.Selection.Fields.Add(oWord.Selection.Range, ref TotalPages, ref oMissing, ref oMissing);


                if (FooterNote != "")
                {
                    oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    oDoc.ActiveWindow.Selection.Font.Size = 9;
                    oDoc.ActiveWindow.Selection.Font.Name = "Arial";
                    oDoc.ActiveWindow.Selection.TypeText("\n");
                    oDoc.ActiveWindow.Selection.TypeText(FooterNote);
                    oWord.ActiveWindow.Selection.Fields.Add(oWord.Selection.Range, ref oMissing, ref oMissing, ref oMissing);
                }
                else
                {
                    oDoc.ActiveWindow.Selection.TypeText("\t");
                    oDoc.ActiveWindow.Selection.TypeText("           ");

                }

                //Add Date Time




                //Added on 05-Jul-2018 Ends here

                if (DocumentType == "P")
                {
                    fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF;
                    file = strpdfFile;
                }
                else if (DocumentType == "W")
                {
                    fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDocument;
                    file = strwordFile;
                }

                oDoc.SaveAs(ref file, ref fileFormat, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing
                    , ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);

                //RemoveBlankPage(strwordFile);
            }
            catch (Exception objException)
            {
                ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
                throw objException;
            }
            finally
            {
                oDoc.Close(ref oFalse, ref oMissing, ref oMissing);
                oWord.Quit(ref oMissing, ref oMissing, ref oMissing);
                File.Delete(strhtmlFile);
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    public static void FunPubSaveDocument(string strHTML, string FilePath, string FileName, string DocumentType, string FooterNote)
    {
        try
        {
            string strhtmlFile = FilePath + "\\" + FileName + ".html";
            string strwordFile = FilePath + "\\" + FileName + ".doc";
            string strpdfFile = FilePath + "\\" + FileName + ".pdf";
            object file = strhtmlFile;
            object oMissing = System.Reflection.Missing.Value;
            object readOnly = false;
            object oFalse = false;
            object fileFormat = null;



            Microsoft.Office.Interop.Word._Application oWord = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word._Document oDoc = new Microsoft.Office.Interop.Word.Document();





            if (!Directory.Exists(FilePath))
            {
                Directory.CreateDirectory(FilePath);
            }

            try
            {
                if (File.Exists(strhtmlFile) == true)
                {
                    File.Delete(strhtmlFile);
                }
                if (File.Exists(strwordFile) == true)
                {
                    File.Delete(strwordFile);
                }
                File.WriteAllText(strhtmlFile, strHTML);

                oDoc = oWord.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                oDoc = oWord.Documents.Open(ref file, ref oMissing, ref readOnly, ref oMissing, ref oMissing, ref oMissing, ref oMissing
                    , ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                oDoc = PDFPageSetup.SetWordProperties(oDoc);

                oDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
                oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;
                oDoc.ActiveWindow.Selection.Fields.Add(oDoc.ActiveWindow.Selection.Range, Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage, ref oMissing, ref oMissing);

                if (FooterNote != "")
                {
                    oDoc.ActiveWindow.Selection.Font.Size = 9;
                    oDoc.ActiveWindow.Selection.Font.Name = "Arial";
                    oDoc.ActiveWindow.Selection.TypeText(FooterNote);
                }
                else
                {
                    oDoc.ActiveWindow.Selection.TypeText("\t");
                    oDoc.ActiveWindow.Selection.TypeText("           ");

                }


                string footerimagepath = HttpContext.Current.Server.MapPath("../Images/TemplateImages/1/OPCFooter.png");
                if (File.Exists(footerimagepath))
                {
                    oDoc.ActiveWindow.Selection.InlineShapes.AddPicture(footerimagepath, oMissing, true, oMissing);
                }
                if (DocumentType == "P")
                {
                    fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF;
                    file = strpdfFile;
                }
                else if (DocumentType == "W")
                {
                    fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDocument;
                    file = strwordFile;
                }

                oDoc.SaveAs(ref file, ref fileFormat, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing
                    , ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            }
            catch (Exception objException)
            {
                ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
                throw objException;
            }
            finally
            {
                oDoc.Close(ref oFalse, ref oMissing, ref oMissing);
                oWord.Quit(ref oMissing, ref oMissing, ref oMissing);
                File.Delete(strhtmlFile);
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }


    public static string FunPubBindImages(string strImageName, string strImagePath, string strHTML)
    {
        try
        {
            string ImageTag = "<img src='" + strImagePath + "' alt='Image'>";
            if (strHTML.Contains(strImageName))
                strHTML = strHTML.Replace(strImageName, ImageTag);
            return strHTML;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static string FunPubDeleteTableHeader(string strtblName, string strHTML)
    {
        try
        {
            string row = "";
            string newtr = String.Empty;
            var startTag = "";
            var endTag = "";
            int startIndex = 0;
            int endIndex = 0;
            string strrow = "";
            string strTable;

            startTag = strtblName;
            endTag = "</TABLE>";
            startIndex = strHTML.LastIndexOf("<TABLE", strHTML.IndexOf(startTag) + startTag.Length);
            endIndex = strHTML.IndexOf(endTag, startIndex) + endTag.Length;
            strTable = strHTML.Substring(startIndex, endIndex - startIndex);
            string strtempTable = strTable;

            startTag = "<TR";
            endTag = "</TR>";
            startIndex = strtempTable.IndexOf(startTag);
            endIndex = strtempTable.IndexOf(endTag, startIndex) + endTag.Length;
            strrow = strtempTable.Substring(startIndex, endIndex - startIndex);
            strtempTable = strtempTable.Replace(strrow, "");
            strHTML = strHTML.Replace(strTable, strtempTable);

            return strHTML;
        }
        catch (Exception ex)
        {
            // ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    public static void FunPrintTemplate(string strHTML, string strPrintType, string strFileDescription, string strServerMapPath, string intUserID)
    {
        string strhtmlFile = (Convert.ToString(strServerMapPath) + "\\PDF Files\\" + Convert.ToString(strFileDescription) + "_" + intUserID + "_" + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".html");
        string strwordFile = string.Empty;
        string strpdfFile = string.Empty;

        if (Convert.ToString(strPrintType) == "P")
        {
            strpdfFile = (Convert.ToString(strServerMapPath) + "\\PDF Files\\" + Convert.ToString(strFileDescription) + "_" + intUserID + "_" + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".pdf");
        }
        else if (Convert.ToString(strPrintType) == "W")
        {
            strwordFile = (Convert.ToString(strServerMapPath) + "\\PDF Files\\" + Convert.ToString(strFileDescription) + "_" + intUserID + "_" + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".doc");
        }

        try
        {
            if (File.Exists(strhtmlFile) == true)
            {
                File.Delete(strhtmlFile);
            }
            File.WriteAllText(strhtmlFile, strHTML);
            object file = strhtmlFile;
            object oMissing = System.Reflection.Missing.Value;
            object readOnly = false;
            object oFalse = false;

            Microsoft.Office.Interop.Word._Application oWord = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word._Document oDoc = new Microsoft.Office.Interop.Word.Document();
            oDoc = oWord.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            oDoc = oWord.Documents.Open(ref file, ref oMissing, ref readOnly, ref oMissing, ref oMissing, ref oMissing, ref oMissing
                , ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            oDoc = PDFPageSetup.SetWordProperties(oDoc);

            object fileFormat = null;

            if (Convert.ToString(strPrintType) == "P")
            {
                fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF;
                file = strpdfFile;
            }

            else if (Convert.ToString(strPrintType) == "W")
            {
                fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDocument;
                file = strwordFile;
            }

            //oDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
            ////oDoc.ActiveWindow.Selection.TypeText(" \t ");
            //oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
            //Object TotalPages = Microsoft.Office.Interop.Word.WdFieldType.wdFieldNumPages;
            //Object CurrentPage = Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage;
            //oDoc.ActiveWindow.Selection.Fields.Add(oDoc.ActiveWindow.Selection.Range, ref CurrentPage, ref oMissing, ref oMissing);
            //oDoc.ActiveWindow.Selection.TypeText(" / ");
            //oDoc.ActiveWindow.Selection.Fields.Add(oDoc.ActiveWindow.Selection.Range, ref TotalPages, ref oMissing, ref oMissing);

            Microsoft.Office.Interop.Word.Range rng = null;
            string img = string.Empty;

            //if (oDoc.InlineShapes.Count >= 1)
            //{
            //    img = Server.MapPath("../Images/login/s3g_logo.png");
            //    rng = oDoc.InlineShapes[1].Range;
            //    rng.Delete();
            //    rng.InlineShapes.AddPicture(img, false, true, Type.Missing);
            //}

            //if (oDoc.InlineShapes.Count >= 2)
            //{
            //    img = Server.MapPath("../Images/login/posign.png");
            //    rng = oDoc.InlineShapes[2].Range;
            //    rng.Delete();
            //    rng.InlineShapes.AddPicture(img, false, true, Type.Missing);
            //}

            oDoc.SaveAs(ref file, ref fileFormat, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing
                , ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            oDoc.Close(ref oFalse, ref oMissing, ref oMissing);
            oWord.Quit(ref oMissing, ref oMissing, ref oMissing);
            File.Delete(strhtmlFile);

            if (Convert.ToString(strPrintType) == "P")
            {
                System.Web.HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=" + strpdfFile);
                System.Web.HttpContext.Current.Response.ContentType = "application/pdf";
                System.Web.HttpContext.Current.Response.WriteFile(strpdfFile);

            }
            else if (Convert.ToString(strPrintType) == "W")
            {
                System.Web.HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=" + strwordFile);
                System.Web.HttpContext.Current.Response.ContentType = "application/vnd.ms-word";
                System.Web.HttpContext.Current.Response.WriteFile(strwordFile);
            }
        }
        catch (Exception objException)
        {
            //FunPriErrorLog(objException);
        }
    }


    public static void FunPrintTemplate(string strHTML, string strPrintType, string strFileDescription, string strServerMapPath, string intUserID, string strHeaderImagePath, string strFooterImagePath, bool Immediate_Display, out string StrFilePath)
    {
        StrFilePath = "";

        if (!Directory.Exists(strServerMapPath + "\\PDF Files"))
        {
            Directory.CreateDirectory(strServerMapPath + "\\PDF Files");
        }

        string strhtmlFile = (Convert.ToString(strServerMapPath) + "\\PDF Files\\" + Convert.ToString(strFileDescription) + "_" + intUserID + "_" + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".html");
        string strwordFile = string.Empty;
        string strpdfFile = string.Empty;

        if (Convert.ToString(strPrintType) == "P")
        {
            strpdfFile = (Convert.ToString(strServerMapPath) + "\\PDF Files\\" + Convert.ToString(strFileDescription) + "_" + intUserID + "_" + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".pdf");
        }
        else if (Convert.ToString(strPrintType) == "W")
        {
            strwordFile = (Convert.ToString(strServerMapPath) + "\\PDF Files\\" + Convert.ToString(strFileDescription) + "_" + intUserID + "_" + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".doc");
        }

        try
        {
            if (File.Exists(strhtmlFile) == true)
            {
                File.Delete(strhtmlFile);
            }
            File.WriteAllText(strhtmlFile, strHTML);
            object file = strhtmlFile;
            object oMissing = System.Reflection.Missing.Value;
            object readOnly = false;
            object oFalse = false;

            Microsoft.Office.Interop.Word._Application oWord = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word._Document oDoc = new Microsoft.Office.Interop.Word.Document();
            oDoc = oWord.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            oDoc = oWord.Documents.Open(ref file, ref oMissing, ref readOnly, ref oMissing, ref oMissing, ref oMissing, ref oMissing
                , ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            oDoc = PDFPageSetup.SetWordProperties(oDoc);

            if (Convert.ToString(strHeaderImagePath) != "")
            {
                oDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageHeader;
                oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;
                //oDoc.ActiveWindow.Selection.Fields.Add(oDoc.ActiveWindow.Selection.Range, Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage, ref oMissing, ref oMissing);
                oDoc.ActiveWindow.Selection.TypeText("\t");
                oDoc.ActiveWindow.Selection.TypeText("           ");
                string HeaderImagePath = HttpContext.Current.Server.MapPath(strHeaderImagePath);
                oDoc.ActiveWindow.Selection.InlineShapes.AddPicture(HeaderImagePath, oMissing, true, oMissing);
            }

            if (Convert.ToString(strFooterImagePath) != "")
            {
                oDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
                oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;
                //oDoc.ActiveWindow.Selection.Fields.Add(oDoc.ActiveWindow.Selection.Range, Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage, ref oMissing, ref oMissing);
                oDoc.ActiveWindow.Selection.TypeText("\t");
                oDoc.ActiveWindow.Selection.TypeText("           ");
                string footerimagepath = HttpContext.Current.Server.MapPath(strFooterImagePath);
                oDoc.ActiveWindow.Selection.InlineShapes.AddPicture(footerimagepath, oMissing, true, oMissing);
            }

            //Added on 05-Jul-2018 Starts here
            //To Add Page numbers to the document

            oDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
            oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
            oDoc.ActiveWindow.Selection.Font.Name = "Arial";
            oDoc.ActiveWindow.Selection.Font.Size = 8;
            oDoc.ActiveWindow.Selection.TypeText("Page ");
            Object CurrentPage = Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage;
            oDoc.ActiveWindow.Selection.Fields.Add(oWord.Selection.Range, ref CurrentPage, ref oMissing, ref oMissing);
            oDoc.ActiveWindow.Selection.TypeText(" of ");
            Object TotalPages = Microsoft.Office.Interop.Word.WdFieldType.wdFieldNumPages;
            oWord.ActiveWindow.Selection.Fields.Add(oWord.Selection.Range, ref TotalPages, ref oMissing, ref oMissing);

            //Added on 05-Jul-2018 Ends here


            object fileFormat = null;

            if (Convert.ToString(strPrintType) == "P")
            {
                fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF;
                file = strpdfFile;
                StrFilePath = strpdfFile;
            }

            else if (Convert.ToString(strPrintType) == "W")
            {
                fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDocument;
                file = strwordFile;
                StrFilePath = strwordFile;
            }

            Microsoft.Office.Interop.Word.Range rng = null;
            string img = string.Empty;

            oDoc.SaveAs(ref file, ref fileFormat, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing
                , ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            oDoc.Close(ref oFalse, ref oMissing, ref oMissing);
            oWord.Quit(ref oMissing, ref oMissing, ref oMissing);
            File.Delete(strhtmlFile);

            if (Immediate_Display == true)
            {
                if (Convert.ToString(strPrintType) == "P")
                {
                    System.Web.HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=" + strpdfFile);
                    System.Web.HttpContext.Current.Response.ContentType = "application/pdf";
                    System.Web.HttpContext.Current.Response.WriteFile(strpdfFile);

                }
                else if (Convert.ToString(strPrintType) == "W")
                {
                    System.Web.HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=" + strwordFile);
                    System.Web.HttpContext.Current.Response.ContentType = "application/vnd.ms-word";
                    System.Web.HttpContext.Current.Response.WriteFile(strwordFile);
                }
            }
        }
        catch (Exception objException)
        {
            //FunPriErrorLog(objException);
        }
    }

    public static void FunPrintTemplate(string strHTML, string strPrintType, string strFileDescription,
        string strServerMapPath, string intUserID, string strHeaderImagePath, string strFooterImagePath,
        bool Immediate_Display, out string StrFilePath, string PageOrientationType)
    {
        StrFilePath = "";

        if (!Directory.Exists(strServerMapPath + "\\PDF Files"))
        {
            Directory.CreateDirectory(strServerMapPath + "\\PDF Files");
        }

        string strhtmlFile = (Convert.ToString(strServerMapPath) + "\\PDF Files\\" + Convert.ToString(strFileDescription) + "_" + intUserID + "_" + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".html");
        string strwordFile = string.Empty;
        string strpdfFile = string.Empty;

        if (Convert.ToString(strPrintType) == "P")
        {
            strpdfFile = (Convert.ToString(strServerMapPath) + "\\PDF Files\\" + Convert.ToString(strFileDescription) + "_" + intUserID + "_" + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".pdf");
        }
        else if (Convert.ToString(strPrintType) == "W")
        {
            strwordFile = (Convert.ToString(strServerMapPath) + "\\PDF Files\\" + Convert.ToString(strFileDescription) + "_" + intUserID + "_" + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".doc");
        }

        try
        {
            if (File.Exists(strhtmlFile) == true)
            {
                File.Delete(strhtmlFile);
            }
            File.WriteAllText(strhtmlFile, strHTML);
            object file = strhtmlFile;
            object oMissing = System.Reflection.Missing.Value;
            object readOnly = false;
            object oFalse = false;

            Microsoft.Office.Interop.Word._Application oWord = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word._Document oDoc = new Microsoft.Office.Interop.Word.Document();
            oDoc = oWord.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            oDoc = oWord.Documents.Open(ref file, ref oMissing, ref readOnly, ref oMissing, ref oMissing, ref oMissing, ref oMissing
                , ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            oDoc = PDFPageSetup.SetWordProperties(oDoc);

            if (Convert.ToString(strHeaderImagePath) != "")
            {
                oDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageHeader;
                oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;
                //oDoc.ActiveWindow.Selection.Fields.Add(oDoc.ActiveWindow.Selection.Range, Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage, ref oMissing, ref oMissing);
                oDoc.ActiveWindow.Selection.TypeText("\t");
                oDoc.ActiveWindow.Selection.TypeText("           ");
                string HeaderImagePath = HttpContext.Current.Server.MapPath(strHeaderImagePath);
                oDoc.ActiveWindow.Selection.InlineShapes.AddPicture(HeaderImagePath, oMissing, true, oMissing);
            }

            if (Convert.ToString(strFooterImagePath) != "")
            {
                oDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
                oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;
                //oDoc.ActiveWindow.Selection.Fields.Add(oDoc.ActiveWindow.Selection.Range, Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage, ref oMissing, ref oMissing);
                oDoc.ActiveWindow.Selection.TypeText("\t");
                oDoc.ActiveWindow.Selection.TypeText("           ");
                string footerimagepath = HttpContext.Current.Server.MapPath(strFooterImagePath);
                oDoc.ActiveWindow.Selection.InlineShapes.AddPicture(footerimagepath, oMissing, true, oMissing);
            }

            //Added on 05-Jul-2018 Starts here
            //To Add Page numbers to the document

            oDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
            oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
            oDoc.ActiveWindow.Selection.Font.Name = "Arial";
            oDoc.ActiveWindow.Selection.Font.Size = 8;
            oDoc.ActiveWindow.Selection.TypeText("Page ");
            Object CurrentPage = Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage;
            oDoc.ActiveWindow.Selection.Fields.Add(oWord.Selection.Range, ref CurrentPage, ref oMissing, ref oMissing);
            oDoc.ActiveWindow.Selection.TypeText(" of ");
            Object TotalPages = Microsoft.Office.Interop.Word.WdFieldType.wdFieldNumPages;
            oWord.ActiveWindow.Selection.Fields.Add(oWord.Selection.Range, ref TotalPages, ref oMissing, ref oMissing);

            //Added on 05-Jul-2018 Ends here

            if (PageOrientationType == "L")
            {
                oDoc.PageSetup.Orientation = Microsoft.Office.Interop.Word.WdOrientation.wdOrientLandscape;
            }
            else
            {
                oDoc.PageSetup.Orientation = Microsoft.Office.Interop.Word.WdOrientation.wdOrientPortrait;
            }


            object fileFormat = null;

            if (Convert.ToString(strPrintType) == "P")
            {
                fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF;
                file = strpdfFile;
                StrFilePath = strpdfFile;
            }

            else if (Convert.ToString(strPrintType) == "W")
            {
                fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDocument;
                file = strwordFile;
                StrFilePath = strwordFile;
            }

            Microsoft.Office.Interop.Word.Range rng = null;
            string img = string.Empty;

            oDoc.SaveAs(ref file, ref fileFormat, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing
                , ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            oDoc.Close(ref oFalse, ref oMissing, ref oMissing);
            oWord.Quit(ref oMissing, ref oMissing, ref oMissing);
            File.Delete(strhtmlFile);

            if (Immediate_Display == true)
            {
                if (Convert.ToString(strPrintType) == "P")
                {
                    System.Web.HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=" + strpdfFile);
                    System.Web.HttpContext.Current.Response.ContentType = "application/pdf";
                    System.Web.HttpContext.Current.Response.WriteFile(strpdfFile);

                }
                else if (Convert.ToString(strPrintType) == "W")
                {
                    System.Web.HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=" + strwordFile);
                    System.Web.HttpContext.Current.Response.ContentType = "application/vnd.ms-word";
                    System.Web.HttpContext.Current.Response.WriteFile(strwordFile);
                }
            }
        }
        catch (Exception objException)
        {
            //FunPriErrorLog(objException);
        }
    }


    public static void FunPrintTemplate(string strHTML, string strPrintType, string strFileDescription,
    string strServerMapPath, string intUserID, string strHeaderImagePath, string strFooterImagePath,
    bool Immediate_Display, out string StrFilePath, string PageOrientationType, List<string> ObjPageSetup)
    {
        StrFilePath = "";

        if (!Directory.Exists(strServerMapPath + "\\PDF Files"))
        {
            Directory.CreateDirectory(strServerMapPath + "\\PDF Files");
        }

        string strhtmlFile = (Convert.ToString(strServerMapPath) + "\\PDF Files\\" + Convert.ToString(strFileDescription) + "_" + intUserID + "_" + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".html");
        string strwordFile = string.Empty;
        string strpdfFile = string.Empty;

        if (Convert.ToString(strPrintType) == "P")
        {
            strpdfFile = (Convert.ToString(strServerMapPath) + "\\PDF Files\\" + Convert.ToString(strFileDescription) + "_" + intUserID + "_" + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".pdf");
        }
        else if (Convert.ToString(strPrintType) == "W")
        {
            strwordFile = (Convert.ToString(strServerMapPath) + "\\PDF Files\\" + Convert.ToString(strFileDescription) + "_" + intUserID + "_" + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".doc");
        }

        try
        {
            if (File.Exists(strhtmlFile) == true)
            {
                File.Delete(strhtmlFile);
            }
            File.WriteAllText(strhtmlFile, strHTML);
            object file = strhtmlFile;
            object oMissing = System.Reflection.Missing.Value;
            object readOnly = false;
            object oFalse = false;

            Microsoft.Office.Interop.Word._Application oWord = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word._Document oDoc = new Microsoft.Office.Interop.Word.Document();
            oDoc = oWord.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            oDoc = oWord.Documents.Open(ref file, ref oMissing, ref readOnly, ref oMissing, ref oMissing, ref oMissing, ref oMissing
                , ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            oDoc = PDFPageSetup.SetWordProperties(oDoc, ObjPageSetup);

            if (Convert.ToString(strHeaderImagePath) != "")
            {
                oDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageHeader;
                oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;
                //oDoc.ActiveWindow.Selection.Fields.Add(oDoc.ActiveWindow.Selection.Range, Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage, ref oMissing, ref oMissing);
                oDoc.ActiveWindow.Selection.TypeText("\t");
                oDoc.ActiveWindow.Selection.TypeText("           ");
                string HeaderImagePath = HttpContext.Current.Server.MapPath(strHeaderImagePath);
                oDoc.ActiveWindow.Selection.InlineShapes.AddPicture(HeaderImagePath, oMissing, true, oMissing);
            }

            if (Convert.ToString(strFooterImagePath) != "")
            {
                oDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
                oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;
                //oDoc.ActiveWindow.Selection.Fields.Add(oDoc.ActiveWindow.Selection.Range, Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage, ref oMissing, ref oMissing);
                oDoc.ActiveWindow.Selection.TypeText("\t");
                oDoc.ActiveWindow.Selection.TypeText("           ");
                string footerimagepath = HttpContext.Current.Server.MapPath(strFooterImagePath);
                oDoc.ActiveWindow.Selection.InlineShapes.AddPicture(footerimagepath, oMissing, true, oMissing);
            }

            //Added on 05-Jul-2018 Starts here
            //To Add Page numbers to the document

            oDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
            oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
            oDoc.ActiveWindow.Selection.Font.Name = "Arial";
            oDoc.ActiveWindow.Selection.Font.Size = 8;
            oDoc.ActiveWindow.Selection.TypeText("Page ");
            Object CurrentPage = Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage;
            oDoc.ActiveWindow.Selection.Fields.Add(oWord.Selection.Range, ref CurrentPage, ref oMissing, ref oMissing);
            oDoc.ActiveWindow.Selection.TypeText(" of ");
            Object TotalPages = Microsoft.Office.Interop.Word.WdFieldType.wdFieldNumPages;
            oWord.ActiveWindow.Selection.Fields.Add(oWord.Selection.Range, ref TotalPages, ref oMissing, ref oMissing);

            //Added on 05-Jul-2018 Ends here

            if (PageOrientationType == "L")
            {
                oDoc.PageSetup.Orientation = Microsoft.Office.Interop.Word.WdOrientation.wdOrientLandscape;
            }
            else
            {
                oDoc.PageSetup.Orientation = Microsoft.Office.Interop.Word.WdOrientation.wdOrientPortrait;
            }


            object fileFormat = null;

            if (Convert.ToString(strPrintType) == "P")
            {
                fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF;
                file = strpdfFile;
                StrFilePath = strpdfFile;
            }

            else if (Convert.ToString(strPrintType) == "W")
            {
                fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDocument;
                file = strwordFile;
                StrFilePath = strwordFile;
            }

            Microsoft.Office.Interop.Word.Range rng = null;
            string img = string.Empty;

            oDoc.SaveAs(ref file, ref fileFormat, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing
                , ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            oDoc.Close(ref oFalse, ref oMissing, ref oMissing);
            oWord.Quit(ref oMissing, ref oMissing, ref oMissing);
            File.Delete(strhtmlFile);

            if (Immediate_Display == true)
            {
                if (Convert.ToString(strPrintType) == "P")
                {
                    System.Web.HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=" + strpdfFile);
                    System.Web.HttpContext.Current.Response.ContentType = "application/pdf";
                    System.Web.HttpContext.Current.Response.WriteFile(strpdfFile);

                }
                else if (Convert.ToString(strPrintType) == "W")
                {
                    System.Web.HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=" + strwordFile);
                    System.Web.HttpContext.Current.Response.ContentType = "application/vnd.ms-word";
                    System.Web.HttpContext.Current.Response.WriteFile(strwordFile);
                }
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }


    public static void RemoveBlankPage(string strWordPath)
    {


        object file = strWordPath;
        object oMissing = System.Reflection.Missing.Value;
        object readOnly = false;
        object oFalse = false;
        object fileFormat = null;



        Microsoft.Office.Interop.Word.Application wordapp = null;
        Microsoft.Office.Interop.Word.Document doc = null;
        Microsoft.Office.Interop.Word.Paragraphs paragraphs = null;
        try
        {
            // Start Word APllication and set it be invisible 
            wordapp = new Microsoft.Office.Interop.Word.Application();
            wordapp.Visible = false;
            doc = wordapp.Documents.Open(strWordPath);
            paragraphs = doc.Paragraphs;
            foreach (Microsoft.Office.Interop.Word.Paragraph paragraph in paragraphs)
            {
                if (paragraph.Range.Text.Trim() == string.Empty)
                {
                    paragraph.Range.Select();
                    wordapp.Selection.Delete();
                }
            }


            // Save the document and close document 
            //doc.SaveAs();
            doc.SaveAs(ref file, ref fileFormat, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing
            , ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            ((Microsoft.Office.Interop.Word._Document)doc).Close();


            // Quit the word application 
            ((Microsoft.Office.Interop.Word._Application)wordapp).Quit();


        }
        catch (Exception ex)
        {
            //MessageBox.Show("Exception Occur, error message is: " + ex.Message);
            //return false;
        }
        finally
        {
            // Clean up the unmanaged Word COM resources by explicitly 
            // call Marshal.FinalReleaseComObject on all accessor objects 
            if (paragraphs != null)
            {
                //Marshal.FinalReleaseComObject(paragraphs);
                //paragraphs = null;
            }
            if (doc != null)
            {
                //Marshal.FinalReleaseComObject(doc);
                //doc = null;
            }
            if (wordapp != null)
            {
                //Marshal.FinalReleaseComObject(wordapp);
                //wordapp = null;
            }
        }



    }
    //Code added to Merge Files
    public static void FunPriGenerateFiles(List<string> filepaths, string OutputFile, string DocumentType)
    {
        try
        {
            object fileFormat = null;
            object file = null;
            object oMissing = System.Reflection.Missing.Value;
            object readOnly = false;
            object oFalse = false;
            string[] filesToMerge = filepaths.ToArray();

            if (DocumentType == "P")
            {
                PDFPageSetup.MergePDFs(filesToMerge, OutputFile);

                for (int i = 0; i < filesToMerge.Length; i++)
                {
                    if (File.Exists(filesToMerge[i]) == true)
                    {
                        File.Delete(filesToMerge[i]);
                    }
                }
            }
            else if (DocumentType == "W")
            {
                fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDocument;
                file = OutputFile;
                Microsoft.Office.Interop.Word._Application wordApplication = new Microsoft.Office.Interop.Word.Application();
                Microsoft.Office.Interop.Word._Document wordDocument = wordApplication.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                Microsoft.Office.Interop.Word.Selection selection = wordApplication.Selection;
                int temp = 0;
                foreach (string file1 in filesToMerge)
                {
                    temp++;
                    Microsoft.Office.Interop.Word._Document CurrentDocument = wordApplication.Documents.Add(file1);
                    PDFPageSetup.CopyPageSetupForWord(CurrentDocument.PageSetup, wordDocument.Sections.Last.PageSetup);
                    CurrentDocument.Range().Copy();
                    selection.PasteAndFormat(Microsoft.Office.Interop.Word.WdRecoveryType.wdFormatOriginalFormatting);
                    if (temp != filesToMerge.Length)
                        selection.InsertBreak(Microsoft.Office.Interop.Word.WdBreakType.wdSectionBreakNextPage);
                }
                wordDocument.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
                wordDocument.SaveAs(ref file, ref fileFormat, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing
                    , ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                wordDocument.Close(ref oFalse, ref oMissing, ref oMissing);
                wordApplication.Quit(ref oMissing, ref oMissing, ref oMissing);

                for (int i = 0; i < filesToMerge.Length; i++)
                {
                    if (File.Exists(filesToMerge[i]) == true)
                    {
                        File.Delete(filesToMerge[i]);
                    }
                }
            }

        }
        catch (System.IO.IOException ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex; // Utility.FunShowAlertMsg(this.Page, "Error: Unable to Print");
        }
    }

    public static void FunPubSaveDocument(string strHTML, string FilePath, string FileName, string DocumentType, int IDummy, string DateFormat, string PageOrientationType)
    {
        try
        {
            string strhtmlFile = FilePath + "\\" + FileName + ".html";
            string strwordFile = FilePath + "\\" + FileName + ".doc";
            string strpdfFile = FilePath + "\\" + FileName + ".pdf";
            object file = strhtmlFile;
            object oMissing = System.Reflection.Missing.Value;
            object readOnly = false;
            object oFalse = false;
            object fileFormat = null;
            Microsoft.Office.Interop.Word._Application oWord = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word._Document oDoc = new Microsoft.Office.Interop.Word.Document();

            if (!Directory.Exists(FilePath))
            {
                Directory.CreateDirectory(FilePath);
            }

            try
            {
                if (File.Exists(strhtmlFile) == true)
                {
                    File.Delete(strhtmlFile);
                }
                if (File.Exists(strwordFile) == true)
                {
                    File.Delete(strwordFile);
                }
                File.WriteAllText(strhtmlFile, strHTML);

                oDoc = oWord.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                oDoc = oWord.Documents.Open(ref file, ref oMissing, ref readOnly, ref oMissing, ref oMissing, ref oMissing, ref oMissing
                    , ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                oDoc = PDFPageSetup.SetWordProperties(oDoc);

                //oDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
                //oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;
                //oDoc.ActiveWindow.Selection.Fields.Add(oDoc.ActiveWindow.Selection.Range, Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage, ref oMissing, ref oMissing);
                //oDoc.ActiveWindow.Selection.TypeText("\t");
                //oDoc.ActiveWindow.Selection.TypeText("           ");
                //string footerimagepath = HttpContext.Current.Server.MapPath("../Images/TemplateImages/1/OPCFooter.png");
                //oDoc.ActiveWindow.Selection.InlineShapes.AddPicture(footerimagepath, oMissing, true, oMissing);

                //Added on 05-Jul-2018 Starts here
                //To Add Page numbers to the document



                oDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
                oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;
                oDoc.ActiveWindow.Selection.Font.Name = "Arial";
                oDoc.ActiveWindow.Selection.Font.Size = 8;
                oDoc.ActiveWindow.Selection.TypeText(DateTime.Now.ToString(DateFormat + "   HH:mm:ss") + "                                                                                                                                                                                             ");
                oWord.ActiveWindow.Selection.Fields.Add(oWord.Selection.Range, ref oMissing, ref oMissing, ref oMissing);

                oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                oDoc.ActiveWindow.Selection.Font.Name = "Arial";
                oDoc.ActiveWindow.Selection.Font.Size = 8;
                oDoc.ActiveWindow.Selection.TypeText("Page ");
                Object CurrentPage = Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage;
                oDoc.ActiveWindow.Selection.Fields.Add(oWord.Selection.Range, ref CurrentPage, ref oMissing, ref oMissing);
                oDoc.ActiveWindow.Selection.TypeText(" of ");
                Object TotalPages = Microsoft.Office.Interop.Word.WdFieldType.wdFieldNumPages;
                oWord.ActiveWindow.Selection.Fields.Add(oWord.Selection.Range, ref TotalPages, ref oMissing, ref oMissing);


                //Add Date Time

                if (PageOrientationType == "L")
                {
                    oDoc.PageSetup.Orientation = Microsoft.Office.Interop.Word.WdOrientation.wdOrientLandscape;
                }
                else
                {
                    oDoc.PageSetup.Orientation = Microsoft.Office.Interop.Word.WdOrientation.wdOrientPortrait;
                }


                //Added on 05-Jul-2018 Ends here

                if (DocumentType == "P")
                {
                    fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF;
                    file = strpdfFile;
                }
                else if (DocumentType == "W")
                {
                    fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDocument;
                    file = strwordFile;
                }

                oDoc.SaveAs(ref file, ref fileFormat, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing
                    , ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);

                //RemoveBlankPage(strwordFile);
            }
            catch (Exception objException)
            {
                ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
                throw objException;
            }
            finally
            {
                oDoc.Close(ref oFalse, ref oMissing, ref oMissing);
                oWord.Quit(ref oMissing, ref oMissing, ref oMissing);
                File.Delete(strhtmlFile);
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
       
    }

    public static void FunPubSaveDocumentWithoutFooterDetails(string strHTML, string FilePath, string FileName, string DocumentType, string FooterNote, string PageOrientationType)
    {
        try
        {
            string strhtmlFile = FilePath + "\\" + FileName + ".html";
            string strwordFile = FilePath + "\\" + FileName + ".doc";
            string strpdfFile = FilePath + "\\" + FileName + ".pdf";
            object file = strhtmlFile;
            object oMissing = System.Reflection.Missing.Value;
            object readOnly = false;
            object oFalse = false;
            object fileFormat = null;



            Microsoft.Office.Interop.Word._Application oWord = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word._Document oDoc = new Microsoft.Office.Interop.Word.Document();





            if (!Directory.Exists(FilePath))
            {
                Directory.CreateDirectory(FilePath);
            }

            try
            {
                if (File.Exists(strhtmlFile) == true)
                {
                    File.Delete(strhtmlFile);
                }
                if (File.Exists(strwordFile) == true)
                {
                    File.Delete(strwordFile);
                }
                File.WriteAllText(strhtmlFile, strHTML);

                oDoc = oWord.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                oDoc = oWord.Documents.Open(ref file, ref oMissing, ref readOnly, ref oMissing, ref oMissing, ref oMissing, ref oMissing
                    , ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                oDoc = PDFPageSetup.SetWordProperties(oDoc);

                oDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
                oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;
                oDoc.ActiveWindow.Selection.Fields.Add(oDoc.ActiveWindow.Selection.Range, Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage, ref oMissing, ref oMissing);


                if (PageOrientationType == "L")
                {
                    oDoc.PageSetup.Orientation = Microsoft.Office.Interop.Word.WdOrientation.wdOrientLandscape;
                }
                else
                {
                    oDoc.PageSetup.Orientation = Microsoft.Office.Interop.Word.WdOrientation.wdOrientPortrait;
                }

                if (DocumentType == "P")
                {
                    fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF;
                    file = strpdfFile;
                }
                else if (DocumentType == "W")
                {
                    fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDocument;
                    file = strwordFile;
                }

                oDoc.SaveAs(ref file, ref fileFormat, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing
                    , ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            }
            catch (Exception objException)
            {
                ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
                throw objException;
            }
            finally
            {
                oDoc.Close(ref oFalse, ref oMissing, ref oMissing);
                oWord.Quit(ref oMissing, ref oMissing, ref oMissing);
                File.Delete(strhtmlFile);
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
}

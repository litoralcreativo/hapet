using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using iTextSharp.text;
using iTextSharp.text.pdf;

public class PDFFooter : PdfPageEventHelper
{
    // This is the contentbyte object of the writer  
    PdfContentByte cb;

    // we will put the final number of pages in a template  
    PdfTemplate footerTemplate;

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
            footerTemplate = cb.CreateTemplate(50, 50);
        }
        catch (DocumentException de)
        {
        }
        catch (System.IO.IOException ioe)
        {
        }
    }

    // write on end of each page
    public override void OnEndPage(PdfWriter writer, Document document)
    {
        base.OnEndPage(writer, document);

        /*PdfPTable tabFot = new PdfPTable(3);
        tabFot.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
        tabFot.DefaultCell.Border = 0;

        tabFot.AddCell("");
        
        PdfPCell cell = new PdfPCell(new Phrase("Pagina número" + " - " + writer.PageNumber));
        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        cell.Border = 0;
        tabFot.AddCell(cell);
        tabFot.AddCell("");
        tabFot.WriteSelectedRows(0, -1, document.LeftMargin, writer.PageSize.GetBottom(document.Bottom), cb);
        */

        string footerStr = "Resultados exportados del Software HAPET";
        string footerStr2 = "Desarrollador del Software: Gastón Chatelet";
        string footerStr3 = "Propiedad Intelectual: LIMH";

        {
            cb.BeginText();
            cb.SetFontAndSize(bf, 10);
            cb.SetTextMatrix(document.LeftMargin, document.PageSize.GetBottom(40));
            cb.ShowText(footerStr);
            cb.SetTextMatrix(document.LeftMargin, document.PageSize.GetBottom(30));
            cb.ShowText(footerStr2);
            cb.SetTextMatrix(document.LeftMargin, document.PageSize.GetBottom(20));
            cb.ShowText(footerStr3);
            cb.EndText();
            float len = bf.GetWidthPoint(footerStr, 10);
            cb.AddTemplate(footerTemplate, document.LeftMargin + len, document.PageSize.GetBottom(40));
        }
    }

}

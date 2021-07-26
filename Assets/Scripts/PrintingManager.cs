using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;



public class PrintingManager : MonoBehaviour
{
    [ShowOnly] public string path = null;
    [ShowOnly] public string videoPath = null;
    [ShowOnly] public string extension;

    public FileManager fileManager;
    public GridInfoPanelScript eventsCIPI;
    public GridInfoPanelScript eventsDPPI;
    public GridInfoPanelScript eventsCIPD;
    public GridInfoPanelScript eventsDPPD;

    public GridInfoPanelScript StepL;
    public GridInfoPanelScript FlyL;
    public GridInfoPanelScript PassL;
    public GridInfoPanelScript CycleL;

    public GridInfoPanelScript StepR;
    public GridInfoPanelScript FlyR;
    public GridInfoPanelScript PassR;
    public GridInfoPanelScript CycleR;

    public GridInfoPanelScript SymmetryStep;
    public GridInfoPanelScript SymmetryFly;
    public GridInfoPanelScript SymmetryPass;
    public GridInfoPanelScript SymmetryCycle;

    public UnityEngine.UI.Toggle openFileBeforeExport;

    public void GenerateFile() 
    {

        // Get Path
        path = fileManager.saveUrl;
        // Get Extension
        extension = fileManager.saveExten;

        // VideoPath
        videoPath = Path.GetFileName(fileManager.path);

        if (extension == "pdf")
        {

            if (!File.Exists(path))
            {
                FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);

                Document document = new Document(PageSize.A4, 20f, 10f, 10f, 100f);

                PdfWriter writer = PdfWriter.GetInstance(document, fileStream);
                writer.PageEvent = new PDFFooter();

                document.Open();


                document.NewPage();

                 

                /*
                string footerStr = "Resultados exportados del Software HAPET \n Desarrollador del Software: Gastón Chatelet \n Propiedad Intelectual: LIMH \n ";
                Phrase p2 = new Phrase(footerStr);
                HeaderFooter footer2 = new HeaderFooter(p2, false);
                footer2.Alignment = 1;
                //document.Footer = new HeaderFooter(new Phrase(""), new Phrase(footerStr));
                document.Footer = footer2;
                */

                // Header


                Image header = Image.GetInstance(Application.streamingAssetsPath + "/PrintingImgs/Header.png");
                header.Alignment = Element.ALIGN_CENTER;
                header.ScaleToFit(document.PageSize.Width - 20 - 10, document.PageSize.Height);
                document.Add(header);


                // break
                document.Add(new Paragraph("\n"));

                // Title
                Paragraph p = new Paragraph(string.Format("Paciente : {0}", fileManager.patientName));
                p.Alignment = Element.ALIGN_LEFT;
                p.Font.Size = 14f;
                document.Add(p);

                p = new Paragraph(string.Format("Fecha : {0}", DateTime.Now));
                p.Alignment = Element.ALIGN_LEFT;
                p.Font.Size = 8f;
                document.Add(p);

                if (videoPath != null && videoPath != "")
                {
                    p = new Paragraph(string.Format("Archivo de video : {0}", videoPath));
                    p.Alignment = Element.ALIGN_LEFT;
                    p.Font.Size = 8f;
                    document.Add(p);

                }

                // break
                document.Add(new Paragraph("\n"));

                #region EVENTS TABLE

                // Table
                PdfPTable table = new PdfPTable(4);
                PdfPCell defaultCell = new PdfPCell();
                //table.DefaultCell.Border = Rectangle.NO_BORDER;
                table.DefaultCell.UseVariableBorders = true;
                table.DefaultCell.DisableBorderSide(Rectangle.TOP_BORDER);
                table.DefaultCell.DisableBorderSide(Rectangle.BOTTOM_BORDER);
                table.DefaultCell.BorderColorLeft = iTextSharp.text.Color.WHITE;
                table.DefaultCell.BorderColorRight = iTextSharp.text.Color.WHITE;
                table.DefaultCell.BorderWidth = 2f;
                table.DefaultCell.BackgroundColor = new iTextSharp.text.Color(200, 200, 220, 255);
                table.DefaultCell.HorizontalAlignment = 1;
                table.TotalWidth = document.PageSize.Width - 20 - 10;

                Phrase headerPhrase = new Phrase("Eventos");
                headerPhrase.Font.Size = 12f;
                headerPhrase.Font.Color = new iTextSharp.text.Color(33, 33, 53, 255);
                PdfPCell cell = new PdfPCell(headerPhrase);
                cell.Colspan = 4;
                cell.Border = Rectangle.NO_BORDER;
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell.BackgroundColor = new iTextSharp.text.Color(150, 150, 175, 255);

                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(" "));
                cell.Colspan = 4;
                cell.FixedHeight = 5f;
                cell.Border = Rectangle.NO_BORDER;
                table.AddCell(cell);

                Image eventHeader1 = Image.GetInstance(Application.streamingAssetsPath + "/PrintingImgs/ContactLeft.png");
                Image eventHeader2 = Image.GetInstance(Application.streamingAssetsPath + "/PrintingImgs/TakeOffLeft.png");
                Image eventHeader3 = Image.GetInstance(Application.streamingAssetsPath + "/PrintingImgs/ContactRight.png");
                Image eventHeader4 = Image.GetInstance(Application.streamingAssetsPath + "/PrintingImgs/TakeOffRight.png");

                // headers (text)
                cell = new PdfPCell(new Phrase("Contacto Inicial Izquierdo"));
                cell.Phrase.Font.Size = 9f;
                cell.Border = Rectangle.NO_BORDER;
                cell.Phrase.Font.Color = new iTextSharp.text.Color(33, 33, 53, 255);
                cell.HorizontalAlignment = 1;
                cell.VerticalAlignment = 1;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Despegue Izquierdo"));
                cell.Phrase.Font.Size = 9f;
                cell.Border = Rectangle.NO_BORDER;
                cell.Phrase.Font.Color = new iTextSharp.text.Color(33, 33, 53, 255);
                cell.VerticalAlignment = 1;
                cell.HorizontalAlignment = 1;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Contacto Inicial Derecho"));
                cell.Phrase.Font.Size = 9f;
                cell.Border = Rectangle.NO_BORDER;
                cell.Phrase.Font.Color = new iTextSharp.text.Color(33, 33, 53, 255);
                cell.VerticalAlignment = 1;
                cell.HorizontalAlignment = 1;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Despegue Derecho"));
                cell.Phrase.Font.Size = 9f;
                cell.Border = Rectangle.NO_BORDER;
                cell.Phrase.Font.Color = new iTextSharp.text.Color(33, 33, 53, 255);
                cell.VerticalAlignment = 1;
                cell.HorizontalAlignment = 1;
                table.AddCell(cell);

                // headers
                table.AddCell(eventHeader1);
                table.AddCell(eventHeader2);
                table.AddCell(eventHeader3);
                table.AddCell(eventHeader4);

                for (int r = 0; r < 6; r++)
                {
                    if (eventsCIPI.panelListFloats.Count > r)
                    {
                        string value = eventsCIPI.panelListFloats[r];
                        Phrase ph = new Phrase(value);
                        ph.Font.Size = 9f;
                        table.AddCell(ph);
                    }
                    else
                    {
                        table.AddCell(" ");
                    }

                    if (eventsDPPI.panelListFloats.Count > r)
                    {
                        string value = eventsDPPI.panelListFloats[r];
                        Phrase ph = new Phrase(value);
                        ph.Font.Size = 9f;
                        table.AddCell(ph);
                    }
                    else
                    {
                        table.AddCell(" ");
                    }

                    if (eventsCIPD.panelListFloats.Count > r)
                    {
                        string value = eventsCIPD.panelListFloats[r];
                        Phrase ph = new Phrase(value);
                        ph.Font.Size = 9f;
                        table.AddCell(ph);
                    }
                    else
                    {
                        table.AddCell(" ");
                    }

                    if (eventsDPPD.panelListFloats.Count > r)
                    {
                        string value = eventsDPPD.panelListFloats[r];
                        Phrase ph = new Phrase(value);
                        ph.Font.Size = 9f;
                        table.AddCell(ph);
                    }
                    else
                    {
                        table.AddCell(" ");
                    }

                    /*for (int c = 0; c < 4; c++)
                    {
                        string value = "Col " + c + " Row " + r;
                        table.AddCell(value);

                    }
                    */
                }

                document.Add(table);
                // break
                document.Add(new Paragraph("\n"));
                #endregion

                #region PARAM L
                // Table
                PdfPTable table2 = new PdfPTable(4);
                //PdfPCell defaultCell2 = new PdfPCell();
                //table.DefaultCell.Border = Rectangle.NO_BORDER;
                table2.DefaultCell.UseVariableBorders = true;
                table2.DefaultCell.DisableBorderSide(Rectangle.TOP_BORDER);
                table2.DefaultCell.DisableBorderSide(Rectangle.BOTTOM_BORDER);
                table2.DefaultCell.BorderColorLeft = iTextSharp.text.Color.WHITE;
                table2.DefaultCell.BorderColorRight = iTextSharp.text.Color.WHITE;
                table2.DefaultCell.BorderWidth = 2f;
                table2.DefaultCell.BackgroundColor = new iTextSharp.text.Color(200, 220, 200, 255);
                table2.DefaultCell.HorizontalAlignment = 1;
                table2.TotalWidth = document.PageSize.Width - 20 - 10;

                Phrase headerPhrase2 = new Phrase("Parametros Izquierdos");
                headerPhrase2.Font.Size = 12f;
                headerPhrase2.Font.Color = new iTextSharp.text.Color(33, 53, 33, 255);
                PdfPCell cell2 = new PdfPCell(headerPhrase2);
                cell2.Colspan = 4;
                cell2.Border = Rectangle.NO_BORDER;
                cell2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell2.BackgroundColor = new iTextSharp.text.Color(150, 175, 150, 255);

                table2.AddCell(cell2);

                cell2 = new PdfPCell(new Phrase(" "));
                cell2.Colspan = 4;
                cell2.FixedHeight = 5f;
                cell2.Border = Rectangle.NO_BORDER;
                table2.AddCell(cell2);

                Image CalcLHeader1 = Image.GetInstance(Application.streamingAssetsPath + "/PrintingImgs/StepTime.png");
                Image CalcLHeader2 = Image.GetInstance(Application.streamingAssetsPath + "/PrintingImgs/FlyTime.png");
                Image CalcLHeader3 = Image.GetInstance(Application.streamingAssetsPath + "/PrintingImgs/PassTimeLR.png");
                Image CalcLHeader4 = Image.GetInstance(Application.streamingAssetsPath + "/PrintingImgs/CycleTime.png");

                // headers (text)
                cell = new PdfPCell(new Phrase("Tiempo de Apoyo"));
                cell.Phrase.Font.Size = 9f;
                cell.Border = Rectangle.NO_BORDER;
                cell.Phrase.Font.Color = new iTextSharp.text.Color(33, 53, 33, 255);
                cell.HorizontalAlignment = 1;
                cell.VerticalAlignment = 1;
                table2.AddCell(cell);

                cell = new PdfPCell(new Phrase("Tiempo de Vuelo"));
                cell.Phrase.Font.Size = 9f;
                cell.Border = Rectangle.NO_BORDER;
                cell.Phrase.Font.Color = new iTextSharp.text.Color(33, 53, 33, 255);
                cell.VerticalAlignment = 1;
                cell.HorizontalAlignment = 1;
                table2.AddCell(cell);

                cell = new PdfPCell(new Phrase("Tiempo de Paso"));
                cell.Phrase.Font.Size = 9f;
                cell.Border = Rectangle.NO_BORDER;
                cell.Phrase.Font.Color = new iTextSharp.text.Color(33, 53, 33, 255);
                cell.VerticalAlignment = 1;
                cell.HorizontalAlignment = 1;
                table2.AddCell(cell);

                cell = new PdfPCell(new Phrase("Tiempo de Ciclo"));
                cell.Phrase.Font.Size = 9f;
                cell.Border = Rectangle.NO_BORDER;
                cell.Phrase.Font.Color = new iTextSharp.text.Color(33, 53, 33, 255);
                cell.VerticalAlignment = 1;
                cell.HorizontalAlignment = 1;
                table2.AddCell(cell);

                // headers
                table2.AddCell(CalcLHeader1);
                table2.AddCell(CalcLHeader2);
                table2.AddCell(CalcLHeader3);
                table2.AddCell(CalcLHeader4);

                for (int r = 0; r < 6; r++)
                {
                    if (StepL.calculationPanelListString.Count > r)
                    {
                        string value = StepL.calculationPanelListString[r];
                        Phrase phrase = new Phrase(value);
                        if (r == StepL.calculationPanelListString.Count - 1)
                        {
                            phrase.Font.SetStyle(iTextSharp.text.Font.BOLD);
                        }
                        else
                        {
                            phrase.Font.SetStyle(iTextSharp.text.Font.NORMAL);
                        }
                        phrase.Font.Size = 9f;
                        table2.AddCell(phrase);
                    }
                    else
                    {
                        table2.AddCell(" ");
                    }

                    if (FlyL.calculationPanelListString.Count > r)
                    {
                        string value = FlyL.calculationPanelListString[r];
                        Phrase phrase = new Phrase(value);
                        if (r == FlyL.calculationPanelListString.Count - 1)
                        {
                            phrase.Font.SetStyle(iTextSharp.text.Font.BOLD);
                        }
                        else
                        {
                            phrase.Font.SetStyle(iTextSharp.text.Font.NORMAL);
                        }
                        phrase.Font.Size = 9f;
                        table2.AddCell(phrase);
                    }
                    else
                    {
                        table2.AddCell(" ");
                    }

                    if (PassL.calculationPanelListString.Count > r)
                    {
                        string value = PassL.calculationPanelListString[r];
                        Phrase phrase = new Phrase(value);
                        if (r == PassL.calculationPanelListString.Count - 1)
                        {
                            phrase.Font.SetStyle(iTextSharp.text.Font.BOLD);
                        }
                        else
                        {
                            phrase.Font.SetStyle(iTextSharp.text.Font.NORMAL);
                        }
                        phrase.Font.Size = 9f;
                        table2.AddCell(phrase);
                    }
                    else
                    {
                        table2.AddCell(" ");
                    }

                    if (CycleL.calculationPanelListString.Count > r)
                    {
                        string value = CycleL.calculationPanelListString[r];
                        Phrase phrase = new Phrase(value);
                        if (r == CycleL.calculationPanelListString.Count - 1)
                        {
                            phrase.Font.SetStyle(iTextSharp.text.Font.BOLD);
                        }
                        else
                        {
                            phrase.Font.SetStyle(iTextSharp.text.Font.NORMAL);
                        }
                        phrase.Font.Size = 9f;
                        table2.AddCell(phrase);
                    }
                    else
                    {
                        table2.AddCell(" ");
                    }

                    /*for (int c = 0; c < 4; c++)
                    {
                        string value = "Col " + c + " Row " + r;
                        table.AddCell(value);

                    }
                    */
                }

                document.Add(table2);
                // break
                document.Add(new Paragraph("\n"));
                #endregion

                #region PARAM R

                // Table
                PdfPTable table3 = new PdfPTable(4);
                //PdfPCell defaultCell3 = new PdfPCell();
                //table.DefaultCell.Border = Rectangle.NO_BORDER;
                table3.DefaultCell.UseVariableBorders = true;
                table3.DefaultCell.DisableBorderSide(Rectangle.TOP_BORDER);
                table3.DefaultCell.DisableBorderSide(Rectangle.BOTTOM_BORDER);
                table3.DefaultCell.BorderColorLeft = iTextSharp.text.Color.WHITE;
                table3.DefaultCell.BorderColorRight = iTextSharp.text.Color.WHITE;
                table3.DefaultCell.BorderWidth = 2f;
                table3.DefaultCell.BackgroundColor = new iTextSharp.text.Color(200, 220, 200, 255);
                table3.DefaultCell.HorizontalAlignment = 1;
                table3.TotalWidth = document.PageSize.Width - 20 - 10;

                Phrase headerPhrase3 = new Phrase("Parametros Derechos");
                headerPhrase3.Font.Size = 12f;
                headerPhrase3.Font.Color = new iTextSharp.text.Color(33, 53, 33, 255);
                PdfPCell cell3 = new PdfPCell(headerPhrase3);
                cell3.Colspan = 4;
                cell3.Border = Rectangle.NO_BORDER;
                cell3.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell3.BackgroundColor = new iTextSharp.text.Color(150, 175, 150, 255);

                table3.AddCell(cell3);

                cell3 = new PdfPCell(new Phrase(" "));
                cell3.Colspan = 4;
                cell3.FixedHeight = 5f;
                cell3.Border = Rectangle.NO_BORDER;
                table3.AddCell(cell3);

                Image CalcRHeader1 = Image.GetInstance(Application.streamingAssetsPath + "/PrintingImgs/StepTime.png");
                Image CalcRHeader2 = Image.GetInstance(Application.streamingAssetsPath + "/PrintingImgs/FlyTime.png");
                Image CalcRHeader3 = Image.GetInstance(Application.streamingAssetsPath + "/PrintingImgs/PassTimeRL.png");
                Image CalcRHeader4 = Image.GetInstance(Application.streamingAssetsPath + "/PrintingImgs/CycleTime.png");

                // headers (text)
                cell = new PdfPCell(new Phrase("Tiempo de Apoyo"));
                cell.Phrase.Font.Size = 9f;
                cell.Border = Rectangle.NO_BORDER;
                cell.Phrase.Font.Color = new iTextSharp.text.Color(33, 53, 33, 255);
                cell.HorizontalAlignment = 1;
                cell.VerticalAlignment = 1;
                table3.AddCell(cell);

                cell = new PdfPCell(new Phrase("Tiempo de Vuelo"));
                cell.Phrase.Font.Size = 9f;
                cell.Border = Rectangle.NO_BORDER;
                cell.Phrase.Font.Color = new iTextSharp.text.Color(33, 53, 33, 255);
                cell.VerticalAlignment = 1;
                cell.HorizontalAlignment = 1;
                table3.AddCell(cell);

                cell = new PdfPCell(new Phrase("Tiempo de Paso"));
                cell.Phrase.Font.Size = 9f;
                cell.Border = Rectangle.NO_BORDER;
                cell.Phrase.Font.Color = new iTextSharp.text.Color(33, 53, 33, 255);
                cell.VerticalAlignment = 1;
                cell.HorizontalAlignment = 1;
                table3.AddCell(cell);

                cell = new PdfPCell(new Phrase("Tiempo de Ciclo"));
                cell.Phrase.Font.Size = 9f;
                cell.Border = Rectangle.NO_BORDER;
                cell.Phrase.Font.Color = new iTextSharp.text.Color(33, 53, 33, 255);
                cell.VerticalAlignment = 1;
                cell.HorizontalAlignment = 1;
                table3.AddCell(cell);

                // headers
                table3.AddCell(CalcRHeader1);
                table3.AddCell(CalcRHeader2);
                table3.AddCell(CalcRHeader3);
                table3.AddCell(CalcRHeader4);

                for (int r = 0; r < 6; r++)
                {
                    if (StepR.calculationPanelListString.Count > r)
                    {
                        string value = StepR.calculationPanelListString[r];
                        Phrase phrase = new Phrase(value);
                        if (r == StepR.calculationPanelListString.Count - 1)
                        {
                            phrase.Font.SetStyle(iTextSharp.text.Font.BOLD);
                        }
                        else
                        {
                            phrase.Font.SetStyle(iTextSharp.text.Font.NORMAL);
                        }
                        phrase.Font.Size = 9f;
                        table3.AddCell(phrase);
                    }
                    else
                    {
                        table3.AddCell(" ");
                    }

                    if (FlyR.calculationPanelListString.Count > r)
                    {
                        string value = FlyR.calculationPanelListString[r];
                        Phrase phrase = new Phrase(value);
                        if (r == FlyR.calculationPanelListString.Count - 1)
                        {
                            phrase.Font.SetStyle(iTextSharp.text.Font.BOLD);
                        }
                        else
                        {
                            phrase.Font.SetStyle(iTextSharp.text.Font.NORMAL);
                        }
                        phrase.Font.Size = 9f;
                        table3.AddCell(phrase);
                    }
                    else
                    {
                        table3.AddCell(" ");
                    }

                    if (PassR.calculationPanelListString.Count > r)
                    {
                        string value = PassR.calculationPanelListString[r];
                        Phrase phrase = new Phrase(value);
                        if (r == PassR.calculationPanelListString.Count - 1)
                        {
                            phrase.Font.SetStyle(iTextSharp.text.Font.BOLD);
                        }
                        else
                        {
                            phrase.Font.SetStyle(iTextSharp.text.Font.NORMAL);
                        }
                        phrase.Font.Size = 9f;
                        table3.AddCell(phrase);
                    }
                    else
                    {
                        table3.AddCell(" ");
                    }

                    if (CycleR.calculationPanelListString.Count > r)
                    {
                        string value = CycleR.calculationPanelListString[r];
                        Phrase phrase = new Phrase(value);
                        if (r == CycleR.calculationPanelListString.Count - 1)
                        {
                            phrase.Font.SetStyle(iTextSharp.text.Font.BOLD);
                        }
                        else
                        {
                            phrase.Font.SetStyle(iTextSharp.text.Font.NORMAL);
                        }
                        phrase.Font.Size = 9f;
                        table3.AddCell(phrase);
                    }
                    else
                    {
                        table3.AddCell(" ");
                    }
                }

                document.Add(table3);
                // break
                document.Add(new Paragraph("\n"));

                #endregion

                #region SIMMETRY

                // Table
                PdfPTable table4 = new PdfPTable(4);
                //PdfPCell defaultCell3 = new PdfPCell();
                //table.DefaultCell.Border = Rectangle.NO_BORDER;
                table4.DefaultCell.UseVariableBorders = true;
                table4.DefaultCell.DisableBorderSide(Rectangle.TOP_BORDER);
                table4.DefaultCell.DisableBorderSide(Rectangle.BOTTOM_BORDER);
                table4.DefaultCell.BorderColorLeft = iTextSharp.text.Color.WHITE;
                table4.DefaultCell.BorderColorRight = iTextSharp.text.Color.WHITE;
                table4.DefaultCell.BorderWidth = 2f;
                table4.DefaultCell.BackgroundColor = new iTextSharp.text.Color(210, 200, 170, 255);
                table4.DefaultCell.HorizontalAlignment = 1;
                table4.TotalWidth = document.PageSize.Width - 20 - 10;

                Phrase headerPhrase4 = new Phrase("Simetría");
                headerPhrase4.Font.Size = 12f;
                headerPhrase4.Font.Color = new iTextSharp.text.Color(60, 45, 30, 255);
                PdfPCell cell4 = new PdfPCell(headerPhrase4);
                cell4.Colspan = 4;
                cell4.Border = Rectangle.NO_BORDER;
                cell4.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell4.BackgroundColor = new iTextSharp.text.Color(150, 100, 50, 150);

                table4.AddCell(cell4);

                cell4 = new PdfPCell(new Phrase(" "));
                cell4.Colspan = 4;
                cell4.FixedHeight = 5f;
                cell4.Border = Rectangle.NO_BORDER;
                table4.AddCell(cell4);

                Image Symmetry1 = Image.GetInstance(Application.streamingAssetsPath + "/PrintingImgs/SymStep.png");
                Image Symmetry2 = Image.GetInstance(Application.streamingAssetsPath + "/PrintingImgs/SymFly.png");
                Image Symmetry3 = Image.GetInstance(Application.streamingAssetsPath + "/PrintingImgs/SymPass.png");
                Image Symmetry4 = Image.GetInstance(Application.streamingAssetsPath + "/PrintingImgs/SymCyc.png");

                // headers (text)
                cell = new PdfPCell(new Phrase("Apoyo"));
                cell.Phrase.Font.Size = 9f;
                cell.Border = Rectangle.NO_BORDER;
                cell.Phrase.Font.Color = new iTextSharp.text.Color(60, 45, 30, 255);
                cell.HorizontalAlignment = 1;
                cell.VerticalAlignment = 1;
                table4.AddCell(cell);

                cell = new PdfPCell(new Phrase("Vuelo"));
                cell.Phrase.Font.Size = 9f;
                cell.Border = Rectangle.NO_BORDER;
                cell.Phrase.Font.Color = new iTextSharp.text.Color(60, 45, 30, 255);
                cell.VerticalAlignment = 1;
                cell.HorizontalAlignment = 1;
                table4.AddCell(cell);

                cell = new PdfPCell(new Phrase("Paso"));
                cell.Phrase.Font.Size = 9f;
                cell.Border = Rectangle.NO_BORDER;
                cell.Phrase.Font.Color = new iTextSharp.text.Color(60, 45, 30, 255);
                cell.VerticalAlignment = 1;
                cell.HorizontalAlignment = 1;
                table4.AddCell(cell);

                cell = new PdfPCell(new Phrase("Ciclo"));
                cell.Phrase.Font.Size = 9f;
                cell.Border = Rectangle.NO_BORDER;
                cell.Phrase.Font.Color = new iTextSharp.text.Color(60, 45, 30, 255);
                cell.VerticalAlignment = 1;
                cell.HorizontalAlignment = 1;
                table4.AddCell(cell);

                // headers
                table4.AddCell(Symmetry1);
                table4.AddCell(Symmetry2);
                table4.AddCell(Symmetry3);
                table4.AddCell(Symmetry4);

                string val = SymmetryStep.calculationPanelListString[0];
                Phrase phr = new Phrase(val);
                phr.Font.Size = 9f;
                table4.AddCell(phr);

                val = SymmetryFly.calculationPanelListString[0];
                phr = new Phrase(val);

                phr.Font.Size = 9f;
                table4.AddCell(phr);

                val = SymmetryPass.calculationPanelListString[0];
                phr = new Phrase(val);

                phr.Font.Size = 9f;
                table4.AddCell(phr);

                val = SymmetryCycle.calculationPanelListString[0];
                phr = new Phrase(val);

                phr.Font.Size = 9f;
                table4.AddCell(phr);



                document.Add(table4);
                // break
                document.Add(new Paragraph("\n"));

                #endregion                


                document.Close();
                writer.Close();

                // opens the file if the user.config allows it
                if (openFileBeforeExport.isOn)
                {
                    PrintFiles();
                }
            }
        }
    }

    void PrintFiles()
    {
        if (path == null)
            return;

        if (File.Exists(path))
        {
            Debug.Log("file found");
            //var startInfo = new System.Diagnostics.ProcessStartInfo(path);
            //int i = 0;
            //foreach (string verb in startInfo.Verbs)
            //{
            //    // Display the possible verbs.
            //    Debug.Log(string.Format("  {0}. {1}", i.ToString(), verb));
            //    i++;
            //}
        }
        else
        {
            return;
        }
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
        process.StartInfo.UseShellExecute = true;
        process.StartInfo.FileName = path;
        //process.StartInfo.Verb = "print";

        process.Start();
        //process.WaitForExit();

    }
}

﻿//////////////////////////////////////////////////////////////////////////////////////////
// ANNdotNET - Deep Learning Tool                                                  //
// Copyright 2017-2018 Bahrudin Hrnjica                                                 //
//                                                                                      //
// This code is free software under the MIT License                                     //
// See license section of  https://github.com/bhrnjica/anndotnet/blob/master/LICENSE.md  //
//                                                                                      //
// Bahrudin Hrnjica                                                                     //
// bhrnjica@hotmail.com                                                                 //
// Bihac, Bosnia and Herzegovina                                                         //
// http://bhrnjica.net                                                       //
//////////////////////////////////////////////////////////////////////////////////////////
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANNdotNET.Lib.Export
{
    public static class ExportToExcel
    {
        public static void Export(List<List<string>> dataTrain, List<List<string>> dataTest, string strFilePath, string excelFormula, bool autoheader)
        {
            try
            {
                //
                var ace = new AlphaCharEnum();
                var wb = new XLWorkbook();
                var ws1 = wb.Worksheets.Add("Training");
                IXLWorksheet ws2 = null;
                ws1.Cell(1, 1).Value = "Training Data";
                if (dataTest != null)
                {
                    ws2 = wb.Worksheets.Add("Validation");
                    ws2.Cell(1, 1).Value = "Validation Data";
                }


                writeDataToExcel(dataTrain,ws1, autoheader);

                if (dataTest != null)
                    writeDataToExcel(dataTest, ws2, autoheader);

                var ind = ace.AlphabetFromIndex(dataTrain[0].Count-1)+"3";

                //column name for model output 
                var colCount = dataTrain[0].Count;
                var ouputColumn = dataTrain[0][colCount - 1].Replace("_actual", "_predicted");
                ws1.Cell(2, dataTrain[0].Count + 1).Value = ouputColumn;
                ws1.Cell(3, dataTrain[0].Count + 1).Value = string.Format(excelFormula, "A3", ind);
                if (dataTest != null)
                {
                    ws1.Cell(2, dataTrain[0].Count + 1).Value = ouputColumn;
                    ws2.Cell(3, dataTrain[0].Count + 1).Value = string.Format(excelFormula, "A3", ind);
                }
                    
                //
                wb.SaveAs(strFilePath, false);
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Write data set to excel worksheet
        /// </summary>
        /// <param name="experiment"></param>
        /// <param name="ws"></param>
        /// <param name="isTest"></param>
        private static void writeDataToExcel(List<List<string>> data, IXLWorksheet ws, bool autoheader)
        {
            //TITLE
            ws.Range("A1", "D1").Style.Font.Bold = true;
            ws.Range("A1", "D1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            if (data == null || data.Count == 0)
                return;
            //get input parameter column
            var colCount = data[0].Count;
            int cellIndex = 1;//starting with offset of 2 cells  
            int rowIndex = 2;
            //Input variable names
            if(autoheader)
            {
                for (int j = 0; j < data[0].Count; j++)
                {
                    if (j + 1 == data[0].Count)
                        ws.Cell(rowIndex, cellIndex).Value = $"Y{j + 1}";
                    else
                        ws.Cell(rowIndex, cellIndex).Value = $"X{j + 1}";

                    cellIndex++;
                }
            }
            
            if(autoheader)
                rowIndex++;

            //data
            for (int i = 0; i < data.Count; i++)
            {
                cellIndex = 1;
                for (int j = 0; j < data[i].Count; j++)
                {
                    ws.Cell(rowIndex + i, cellIndex).Value = data[i][j];
                    cellIndex++;
                }
            }

        }


    }
}

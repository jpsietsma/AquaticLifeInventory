using System;
using System.Collections.Generic;
using System.Text;
using BarcodeLib;
using System.Drawing;
using System.Drawing.Imaging;

using Color = System.Drawing.Color;
using static AQLI.DataServices.BarcodeConfig;

namespace AQLI.DataServices
{
    public class BarcodeFactory
    {
        public BarcodeFactory()
        {

        }

        public Image GenerateBarcode(int data, BarcodeTypes barcodeType)
        {
            // Create an instance of the API
            Barcode barcodeAPI = new Barcode();

            switch (barcodeType)
            {
                case BarcodeTypes.Invoice:
                    {
                        var newData = string.Concat("INV-", data);

                        // Define basic settings of the image
                        int imageWidth = 200;
                        int imageHeight = 45;
                        Color foreColor = Color.Gray;
                        Color backColor = Color.Transparent;
                        //string data = "038000356216";

                        // Generate the barcode with your settings
                        Image barcodeImage = barcodeAPI.Encode(TYPE.CODE128, newData, foreColor, backColor, imageWidth, imageHeight);

                        return barcodeImage;

                        // Store image in some path with the desired format
                        //barcodeImage.Save(@"C:\Users\sdkca\Desktop\upca_example.png", ImageFormat.Png);
                    }                    
                case BarcodeTypes.Supply:
                    {
                        return null;
                    }
                case BarcodeTypes.Live_Creature:
                    {
                        return null;
                    }
                case BarcodeTypes.Equipment:
                    {
                        return null;
                    }
                case BarcodeTypes.Decoration:
                    {
                        return null;
                    }
                default:
                    {
                        return null;
                    }
            }
                        
        }        
    }

    public static class BarcodeConfig
    {
        public enum BarcodeTypes
        {
            Invoice,
            Supply,
            Live_Creature,
            Equipment,
            Decoration
        }
    }

}

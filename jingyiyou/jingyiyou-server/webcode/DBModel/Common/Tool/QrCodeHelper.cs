using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ncc2019.Common.Tool
{
    public class QrCodeHelper
    {
        public ErrorCorrectionLevel Ecl;
        public string Content;
        public QuietZoneModules QuietZones;
        public int ModuleSize;
        public BitMatrix Matrix;
        public string ContentType;


        public QrCodeHelper()
        {
            Ecl = ErrorCorrectionLevel.M;
            QuietZones = QuietZoneModules.Two;
            ModuleSize = 12;
        }

        /// <summary>
        /// Encode the content with desired parameters and save the generated Matrix
        /// </summary>
        /// <returns>True if the encoding succeeded, false if the content is empty or too large to fit in a QR code</returns>
        private bool TryEncode()
        {
            var encoder = new QrEncoder(Ecl);
            QrCode qr;
            if (!encoder.TryEncode(Content, out qr))
                return false;

            Matrix = qr.Matrix;
            return true;
        }

        /// <summary>
        /// Render the Matrix as a PNG image
        /// </summary>
        /// <param name="ms">MemoryStream to store the image bytes into</param>
        public byte[] Render()
        {
            byte[] buffer;
            TryEncode();
            using (var ms = new MemoryStream())
            {
                var render = new GraphicsRenderer(new FixedModuleSize(ModuleSize, QuietZones));
                render.WriteToStream(Matrix, ImageFormat.Png, ms);               
                buffer = ms.GetBuffer();
            }


            return buffer;
        }
    }
}

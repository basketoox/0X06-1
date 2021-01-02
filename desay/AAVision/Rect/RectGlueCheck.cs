using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Text;
using NationalInstruments.Vision.WindowsForms;
using NationalInstruments.Vision;
using NationalInstruments.Vision.Analysis;
using Vision_Assistant.Utilities;
using desay.ProductData;

namespace Vision_Assistant
{
    static class RectGlueCheck
    {
        public static ParticleMeasurementsReport vaParticleReport;
		public static ParticleMeasurementsReport vaParticleReportCalibrated;
        public static double TotalAreas;
        public static int MaxMassIndex;
		
		

        
		private static void IVA_Mask_From_ROI(VisionImage image, Roi roi, bool invertMask, bool extractRegion)
		{

			using (VisionImage imageMask = new VisionImage(ImageType.U8, 7))
			{
				PixelValue fillValue = new PixelValue(255);
				
				// Transforms the region of interest into a mask image.
				Algorithms.RoiToMask(imageMask, roi, fillValue, image);
				
				if (invertMask)
				{
					// Inverts the mask image.
					Algorithms.Xor(imageMask, fillValue, imageMask);
				}
				
				// Masks the input image using the mask image we just created.
				Algorithms.Mask(image, image, imageMask);
				
				if (extractRegion)
				{
					// Extracts the bounding box.
					Algorithms.Extract(image, image, roi.GetBoundingRectangle());
				}
			}
		}
		
		private static void IVA_Particle(VisionImage image,
										Connectivity connectivity,
										Collection<MeasurementType> pPixelMeasurements,
										Collection<MeasurementType> pCalibratedMeasurements,
										IVA_Data ivaData,
										int stepIndex,
										out ParticleMeasurementsReport partReport,
										out ParticleMeasurementsReport partReportCal)
		{

			// Computes the requested pixel measurements.
			if (pPixelMeasurements.Count != 0)
			{
				partReport = Algorithms.ParticleMeasurements(image, pPixelMeasurements, connectivity, ParticleMeasurementsCalibrationMode.Pixel);
			}
			else
			{
				partReport = new ParticleMeasurementsReport();
			}
			
			// Computes the requested calibrated measurements.
			if (pCalibratedMeasurements.Count != 0)
			{
				partReportCal = Algorithms.ParticleMeasurements(image, pCalibratedMeasurements, connectivity, ParticleMeasurementsCalibrationMode.Calibrated);
			}
			else
			{
				partReportCal = new ParticleMeasurementsReport();
			}
			
			// Computes the center of mass of each particle to log as results.
			ParticleMeasurementsReport centerOfMass;
			Collection<MeasurementType> centerOfMassMeasurements = new Collection<MeasurementType>();
			centerOfMassMeasurements.Add(MeasurementType.CenterOfMassX);
			centerOfMassMeasurements.Add(MeasurementType.CenterOfMassY);
			
			if ((image.InfoTypes & InfoTypes.Calibration) != 0)
			{
				centerOfMass = Algorithms.ParticleMeasurements(image, centerOfMassMeasurements, connectivity, ParticleMeasurementsCalibrationMode.Both);
			}
			else
			{
				centerOfMass = Algorithms.ParticleMeasurements(image, centerOfMassMeasurements, connectivity, ParticleMeasurementsCalibrationMode.Pixel);
			}
			
			// Delete all the results of this step (from a previous iteration)
			Functions.IVA_DisposeStepResults(ivaData, stepIndex);
			
			ivaData.stepResults[stepIndex].results.Add(new IVA_Result("Object #", centerOfMass.PixelMeasurements.GetLength(0)));
			
			if (centerOfMass.PixelMeasurements.GetLength(0) > 0)
			{
				for(int i = 0; i < centerOfMass.PixelMeasurements.GetLength(0); ++i)
				{
					ivaData.stepResults[stepIndex].results.Add(new IVA_Result(String.Format("Particle {0}.X Position (Pix.)", i+1), centerOfMass.PixelMeasurements[i, 0]));
					ivaData.stepResults[stepIndex].results.Add(new IVA_Result(String.Format("Particle {0}.Y Position (Pix.)", i+1), centerOfMass.PixelMeasurements[i, 1]));
					
					// If the image is calibrated, also store the real world coordinates.
					if ((image.InfoTypes & InfoTypes.Calibration) != 0)
					{
						ivaData.stepResults[stepIndex].results.Add(new IVA_Result(String.Format("Particle {0}.X Position (Calibrated)", i+1), centerOfMass.CalibratedMeasurements[i, 0]));
						ivaData.stepResults[stepIndex].results.Add(new IVA_Result(String.Format("Particle {0}.Y Position (Calibrated)", i+1), centerOfMass.CalibratedMeasurements[i, 1]));
					}
				}
			}
		}

        public static void IVA_OverlayClosedContour(VisionImage image, int[] xCoordinates, int[] yCoordinates, byte[] colors, DrawingMode drawMode)
        {

            Rgb32Value color = new Rgb32Value(colors[0], colors[1], colors[2]);
            Collection<PointContour> points = new Collection<PointContour>();

            for (int i = 0; i < xCoordinates.Length; ++i)
            {
                points.Add(new PointContour(xCoordinates[i], yCoordinates[i]));
            }

            image.Overlays.Default.AddPolygon(new PolygonContour(points), color, drawMode);
        }

        private static void IVA_OverlayText(VisionImage image,
                                    int originX,
                                    int originY,
                                    string text,
                                    byte[] colors,
                                    string fontName,
                                    int[] options,
                                    HorizontalTextAlignment alignmentH,
                                    VerticalTextAlignment alignmentV,
                                    double angle)
        {

            Rgb32Value color = new Rgb32Value(colors[0], colors[1], colors[2]);
            Rgb32Value bgColor = new Rgb32Value(colors[3], colors[4], colors[5], colors[6]);
            OverlayTextOptions textOptions = new OverlayTextOptions(fontName, options[0]);

            textOptions.TextDecoration.Bold = Convert.ToBoolean(options[1]);
            textOptions.TextDecoration.Italic = Convert.ToBoolean(options[2]);
            textOptions.TextDecoration.Underline = Convert.ToBoolean(options[3]);
            textOptions.TextDecoration.Strikeout = Convert.ToBoolean(options[4]);
            textOptions.HorizontalAlignment = alignmentH;
            textOptions.VerticalAlignment = alignmentV;
            textOptions.BackgroundColor = bgColor;
            textOptions.Angle = angle;

            image.Overlays.Default.AddText(text, new PointContour(originX, originY), color, textOptions);
        }

        public static void IVA_OverlayOval(VisionImage image, int[] coordinates, byte[] colors, DrawingMode drawMode)
        {

            Rgb32Value color = new Rgb32Value(colors[0], colors[1], colors[2]);
            OvalContour oval = new OvalContour();

            oval.Top = coordinates[0];
            oval.Left = coordinates[1];
            oval.Height = coordinates[2];
            oval.Width = coordinates[3];

            image.Overlays.Default.AddOval(oval, color, drawMode);
        }

        public static PaletteType ProcessImage(VisionImage image, double[] offset)
        {
            // Initialize the IVA_Data structure to pass results and coordinate systems.
			IVA_Data ivaData = new IVA_Data(13, 0);
			
			// Extract Color Plane
			using (VisionImage plane = new VisionImage(ImageType.U8, 7))
			{
				// Extract the green color plane and copy it to the main image.
				Algorithms.ExtractColorPlanes(image, ColorMode.Rgb, null, plane, null);
				Algorithms.Copy(plane, image);
			}
			
			// Creates a new, empty region of interest.
			Roi roi = new Roi();
            // Creates a new RotatedRectangleContour using the given values.
            PointContour vaCenter = new PointContour(1295 + offset[0], 994 + offset[1]);
            RotatedRectangleContour vaRotatedRect = new RotatedRectangleContour(vaCenter, 1478, 1488, 0);
            roi.Add(vaRotatedRect);
            // Mask from ROI
			IVA_Mask_From_ROI(image, roi, false, false);
			roi.Dispose();
            
            // Creates a new, empty region of interest.
			Roi roi2 = new Roi();
			// Creates a new RectangleContour using the given values.
            RectangleContour vaRect = new RectangleContour(720 + offset[0], 420 + offset[1], 1148, 1156);
            roi2.Add(vaRect);
            // Mask from ROI
			IVA_Mask_From_ROI(image, roi2, true, false);
			roi2.Dispose();
            
            // Manual Threshold
			Algorithms.Threshold(image, image, new Range(160, 255), true, 1);
			
			// Advanced Morphology: Remove Objects
			int[] vaCoefficients = {1, 1, 1, 1, 1, 1, 1, 1, 1};
			StructuringElement vaStructElem = new StructuringElement(3, 3, vaCoefficients);
			vaStructElem.Shape = StructuringElementShape.Square;
			// Filters particles based on their size.
			Algorithms.RemoveParticle(image, image, 5, SizeToKeep.KeepLarge, Connectivity.Connectivity8, vaStructElem);

            // Basic Morphology - Applies morphological transformations to binary images.
            int[] vaCoefficients2 = { 0, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0 };
            StructuringElement vaStructElem2 = new StructuringElement(7, 7, vaCoefficients2);
            vaStructElem.Shape = StructuringElementShape.Square;
            // Applies morphological transformations
            for (int i = 0; i < 4; ++i)
            {
                Algorithms.Morphology(image, image, MorphologyMethod.Dilate, vaStructElem2);
            }

            // Basic Morphology - Applies morphological transformations to binary images.
            int[] vaCoefficients3 = { 0, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0 };
            StructuringElement vaStructElem3 = new StructuringElement(7, 7, vaCoefficients3);
            vaStructElem.Shape = StructuringElementShape.Square;
            // Applies morphological transformations
            for (int i = 0; i < 4; ++i)
            {
                Algorithms.Morphology(image, image, MorphologyMethod.Erode, vaStructElem3);
            }

            // Particle Analysis - Computes the number of particles detected in a binary image and
            // returns the requested measurements about the particles.
            Collection<MeasurementType> vaPixelMeasurements = new Collection<MeasurementType>(new MeasurementType[] { MeasurementType.CenterOfMassX, MeasurementType.CenterOfMassY, MeasurementType.BoundingRectWidth, MeasurementType.BoundingRectHeight, MeasurementType.Area });
            Collection<MeasurementType> vaCalibratedMeasurements = new Collection<MeasurementType>(new MeasurementType[]{});
			IVA_Particle(image, Connectivity.Connectivity8, vaPixelMeasurements, vaCalibratedMeasurements, ivaData, 8, out vaParticleReport, out vaParticleReportCalibrated);

            // Overlays a closed contour onto the image.
            int w = (int)Position.Instance.CenterOffset_X;
            int h = (int)Position.Instance.CenterOffset_Y;
            int[] xCoords = { 1296 - w, 1296 + w, 1296 + w, 1296 - w, 1296 - w };
            int[] yCoords = { 972 - h, 972 - h, 972 + h, 972 + h, 972 - h };
            byte[] vaColor = { 255, 255, 255 };
            IVA_OverlayClosedContour(image, xCoords, yCoords, vaColor, DrawingMode.DrawValue);

            GetTotalAreas();
            int Masscenter_x = (int)vaParticleReport.PixelMeasurements[MaxMassIndex, 0];
            int Masscenter_y = (int)vaParticleReport.PixelMeasurements[MaxMassIndex, 1];


            // Overlays a closed contour onto the image.
            int[] vaCoords = { Masscenter_y - 25, Masscenter_x - 25, 25, 25 };
            byte[] vaColor1 = { 0, 0, 255 };
            IVA_OverlayOval(image, vaCoords, vaColor1, DrawingMode.PaintValue);

            // Overlays the string of text onto the image.
            string vaOverlayText = "胶路面积："+TotalAreas.ToString("");
            byte[] vaColor2 = { 255, 255, 255, 0, 0, 0, 1 };
            string vaFontName = "Sys Font";
            int[] vaOptions = { 100, 0, 0, 0, 0 };
            IVA_OverlayText(image, 10, 100, vaOverlayText, vaColor2, vaFontName, vaOptions, HorizontalTextAlignment.Left, VerticalTextAlignment.Bottom, 0);

            // Overlays the string of text onto the image.
            string vaOverlayText2 = "等效矩形：H:" + vaParticleReport.PixelMeasurements[MaxMassIndex, 2].ToString("") + ",W:" +
                           vaParticleReport.PixelMeasurements[MaxMassIndex, 3].ToString("");
            IVA_OverlayText(image, 10, 200, vaOverlayText2, vaColor2, vaFontName, vaOptions, HorizontalTextAlignment.Left, VerticalTextAlignment.Bottom, 0);

            // Dispose the IVA_Data structure.
            ivaData.Dispose();
			
			// Return the palette type of the final image.
			return PaletteType.Binary;

        }

        public static void GetTotalAreas()
        {
            TotalAreas = 0;
            double MaxArea = 0;
            for (int i = 0; i < RectGlueCheck.vaParticleReport.PixelMeasurements.GetLength(0); i++)
            {
                if(RectGlueCheck.vaParticleReport.PixelMeasurements[i, 4] > MaxArea)
                {
                    MaxArea = RectGlueCheck.vaParticleReport.PixelMeasurements[i, 4];
                    MaxMassIndex = i;
                }
                TotalAreas += RectGlueCheck.vaParticleReport.PixelMeasurements[i, 4];
            }
        }
    }
}


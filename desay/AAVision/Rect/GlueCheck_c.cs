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
    static class GlueCheck_c
    {
        public static FindCircularEdgeReport vaCircularEdgeReport;
		public static ParticleMeasurementsReport vaParticleReport;
		public static ParticleMeasurementsReport vaParticleReportCalibrated;
        public static double MaxDistance;
        public static int MaxDistanceIndex;
        public static double MaxDistance1;
        public static int MaxDistanceIndex1;




        private static FindCircularEdgeReport IVA_FindCircularEdge(VisionImage image,
															Roi roi,
															SpokeDirection direction,
															EdgeOptions options,
                                                            CircularEdgeFitOptions fitOptions,
															IVA_Data ivaData, 
															int stepIndex)
		{

            // First, delete all the results of this step (from a previous iteration)
			Functions.IVA_DisposeStepResults(ivaData, stepIndex);
            FindCircularEdgeOptions circleOptions = new FindCircularEdgeOptions(direction);
            circleOptions.EdgeOptions = options;
            FindCircularEdgeReport circleReport = new FindCircularEdgeReport();		

			// Calculate the edge locations
			circleReport = Algorithms.FindCircularEdge(image, roi, circleOptions, fitOptions);
			
			// If a circle was found, add results
			if (circleReport.CircleFound)
			{
			    ivaData.stepResults[stepIndex].results.Add(new IVA_Result("Center.X Position (Pix.)", circleReport.Center.X));
			    ivaData.stepResults[stepIndex].results.Add(new IVA_Result("Center.Y Position (Pix.)", circleReport.Center.Y));
			    if ((image.InfoTypes & InfoTypes.Calibration) != 0)
			    {
				    ivaData.stepResults[stepIndex].results.Add(new IVA_Result("Center.X Position (Calibrated)", circleReport.CenterCalibrated.X));
				    ivaData.stepResults[stepIndex].results.Add(new IVA_Result("Center.Y Position (Calibrated)", circleReport.CenterCalibrated.Y));
			    }
			    ivaData.stepResults[stepIndex].results.Add(new IVA_Result("Radius (Pix.)", circleReport.Radius));
			    if ((image.InfoTypes & InfoTypes.Calibration) != 0)
			    {
				    ivaData.stepResults[stepIndex].results.Add(new IVA_Result("Radius (Calibrated)", circleReport.RadiusCalibrated));
			    }
			    ivaData.stepResults[stepIndex].results.Add(new IVA_Result("Deviation", circleReport.Roundness));
    		}
			return circleReport;
		}
		
		private static void IVA_CoordSys(int coordSysIndex,
										 int originStepIndex,
										 int originResultIndex,
										 int angleStepIndex,
										 int angleResultIndex,
										 double baseOriginX,
										 double baseOriginY,
										 double baseAngle,
										 AxisOrientation baseAxisOrientation,
										 int mode,
										 IVA_Data ivaData)
		{

			ivaData.baseCoordinateSystems[coordSysIndex].Origin.X = baseOriginX;
			ivaData.baseCoordinateSystems[coordSysIndex].Origin.Y = baseOriginY;
			ivaData.baseCoordinateSystems[coordSysIndex].Angle = baseAngle;
			ivaData.baseCoordinateSystems[coordSysIndex].AxisOrientation = baseAxisOrientation;
			
			ivaData.MeasurementSystems[coordSysIndex].Origin.X = baseOriginX;
			ivaData.MeasurementSystems[coordSysIndex].Origin.Y = baseOriginY;
			ivaData.MeasurementSystems[coordSysIndex].Angle = baseAngle;
			ivaData.MeasurementSystems[coordSysIndex].AxisOrientation = baseAxisOrientation;
			
			switch (mode)
			{
				// Horizontal motion
				case 0:
					ivaData.MeasurementSystems[coordSysIndex].Origin.X = Functions.IVA_GetNumericResult(ivaData, originStepIndex, originResultIndex);
					break;
				// Vertical motion
				case 1:
					ivaData.MeasurementSystems[coordSysIndex].Origin.Y = Functions.IVA_GetNumericResult(ivaData, originStepIndex, originResultIndex + 1);
					break;		
				// Horizontal and vertical motion
				case 2:
					ivaData.MeasurementSystems[coordSysIndex].Origin = Functions.IVA_GetPoint(ivaData, originStepIndex, originResultIndex);
					break;				
				// Horizontal, vertical and angular motion
				case 3:
					ivaData.MeasurementSystems[coordSysIndex].Origin = Functions.IVA_GetPoint(ivaData, originStepIndex, originResultIndex);
					ivaData.MeasurementSystems[coordSysIndex].Angle = Functions.IVA_GetNumericResult(ivaData, angleStepIndex, angleResultIndex);
					break;
			}
		}
		
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
		
		private static void IVA_FFT_Truncate(VisionImage image, TruncateMode truncateMode, double ratio)
		{

			// Creates the complex image for the FFT.
			using (VisionImage fftImage = new VisionImage(ImageType.Complex, 7))
			{
				// Computes the Fourier transform of the image.
				Algorithms.Fft(image, fftImage);
				
				// Truncates the frequencies of the complex image.
				Algorithms.ComplexTruncate(fftImage, fftImage, truncateMode, ratio);
				
				// Takes the inverse Fourier transform of the complex image.
				Algorithms.InverseFft(fftImage, image);
			}
		}
		
		
		private static void IVA_BinaryInverse(VisionImage image)
		{

			Collection<short> vaLookupTable = new Collection<short>();
			
			vaLookupTable.Add(1);
			for(int i = 1; i < 256; ++i)
			{
				vaLookupTable.Add(0);
			}
			
			// Inverses the binary image.
			Algorithms.UserLookup(image, image, vaLookupTable);
		}
		
		private static ExtractContourReport IVA_ExtractContour(VisionImage image,
                                                               Roi roi, 
											                   ExtractContourDirection direction,
                                                               CurveParameters curveSettings,
                                                               ConnectionConstraintType[] constraintTypeArray,
                                                               double[] constraintMinArray,
                                                               double[] constraintMaxArray,
                                                               ExtractContourSelection selection)
		{

			// Build the ConnectionConstraint Collection
            Collection<ConnectionConstraint> constraints = new Collection<ConnectionConstraint>();
			for(int i = 0; i < constraintTypeArray.Length; ++i)
			{
                constraints.Add(new ConnectionConstraint(constraintTypeArray[i], new Range(constraintMinArray[i], constraintMaxArray[i])));
			}
            // Extract contours from image
            return Algorithms.ExtractContour(image, roi, direction, curveSettings, constraints, selection);
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
		

        public static PaletteType ProcessImage(VisionImage image,string path,out double[] distance)
        {
            // Initialize the IVA_Data structure to pass results and coordinate systems.
			IVA_Data ivaData = new IVA_Data(17, 1);
            distance = new double[4] { 0, 0 ,0,0};

			// Creates a new, empty region of interest.
			Roi roi = new Roi();
			// Creates a new AnnulusContour using the given values.
			PointContour vaCenter = new PointContour(1283, 965);
            AnnulusContour vaOval = new AnnulusContour(vaCenter, 418, 702, 0, 0);
            roi.Add(vaOval);
            // Find Circular Edge
			EdgeOptions vaOptions = new EdgeOptions();
			vaOptions.ColumnProcessingMode = ColumnProcessingMode.Average;
			vaOptions.InterpolationType = InterpolationMethod.Bilinear;
			vaOptions.KernelSize = 3;
			vaOptions.MinimumThreshold = 18;
			vaOptions.Polarity = EdgePolaritySearchMode.Rising;
			vaOptions.Width = 3;
            CircularEdgeFitOptions vaFitOptions = new CircularEdgeFitOptions();
            vaFitOptions.ProcessType = RakeProcessType.GetFirstEdges;
            vaFitOptions.StepSize = 7;
            vaFitOptions.MaxPixelRadius = 3;

			vaCircularEdgeReport = IVA_FindCircularEdge(image, roi, SpokeDirection.InsideToOutside, vaOptions, vaFitOptions, ivaData, 1);
			
			roi.Dispose();
            
            // Set Coordinate System
 			int vaCoordSystemIndex = 0;
			int stepIndexOrigin = 1;
			int resultIndexOrigin = 0;
			int stepIndexAngle = -1;
			int resultIndexAngle = 0;
 			double refSysOriginX = vaCircularEdgeReport.Center.X;
			double refSysOriginY = vaCircularEdgeReport.Center.Y;
			double refSysAngle = 0;
			AxisOrientation refSysAxisOrientation = AxisOrientation.Direct;
			int vaCoordSystemType = 0;
			IVA_CoordSys(vaCoordSystemIndex, stepIndexOrigin, resultIndexOrigin, stepIndexAngle, resultIndexAngle, refSysOriginX, refSysOriginY, refSysAngle, refSysAxisOrientation, vaCoordSystemType, ivaData);
			
			// Image Buffer: Push
			Functions.IVA_PushBuffer(ivaData, image, 0);
			
			// Get Image
 			string vaFilePath = path;
			FileInformation vaFileInfo = Algorithms.GetFileInformation(vaFilePath);
			// Set the image size to 0 to speed up the cast.
			//image.SetSize(0, 0);
			//image.Type = vaFileInfo.ImageType;
			//image.BitDepth = 0;
			image.ReadFile(vaFilePath);
			
	        switch (image.Type)
            {
                case ImageType.I16:case ImageType.U16:
				if (image.BitDepth == 0 & false)
					{
                        image.BitDepth = 10;
					}
                    break;
				default:
                    break;
            }
            // Operators: Absolute Difference Image
			Algorithms.AbsoluteDifference(image, Functions.IVA_GetBuffer(ivaData, 0), image);
			
			// Creates a new, empty region of interest.
			Roi roi2 = new Roi();
			// Creates a new AnnulusContour using the given values.
			PointContour vaCenter2 = new PointContour(vaCircularEdgeReport.Center.X, vaCircularEdgeReport.Center.Y);
            AnnulusContour vaOval2 = new AnnulusContour(vaCenter2, 527, 846, 0, 0);
            roi2.Add(vaOval2);
            // Reposition the region of interest based on the coordinate system.
			int coordSystemIndex = 0;
			Algorithms.TransformRoi(roi2, new CoordinateTransform(ivaData.baseCoordinateSystems[coordSystemIndex], ivaData.MeasurementSystems[coordSystemIndex]));
			// Mask from ROI
			IVA_Mask_From_ROI(image, roi2, false, false);
			roi2.Dispose();
            
            // Color Threshold
			Range plane1Range = new Range(0, Position.Instance.RedMax_Threshold);
			Range plane2Range = new Range(0, Position.Instance.GreenMax_Threshold);
			Range plane3Range = new Range(0, Position.Instance.BlueMax_Threshold);
			using (VisionImage thresholdImage = new VisionImage(ImageType.U8, 7))
			{
				Algorithms.ColorThreshold(image, thresholdImage, ColorMode.Rgb, 1, plane1Range, plane2Range, plane3Range);
				Algorithms.Copy(thresholdImage, image);
			}
			
			// Truncates the frequencies of an image.
			IVA_FFT_Truncate(image, TruncateMode.High, Position.Instance.FFT_Frequency);
			
			// Advanced Morphology: Remove Objects
			int[] vaCoefficients = {1, 1, 1, 1, 1, 1, 1, 1, 1};
			StructuringElement vaStructElem = new StructuringElement(3, 3, vaCoefficients);
			vaStructElem.Shape = StructuringElementShape.Square;
			// Filters particles based on their size.
			Algorithms.RemoveParticle(image, image, 30, SizeToKeep.KeepLarge, Connectivity.Connectivity8, vaStructElem);
			
			// Invert Binary Image.
			IVA_BinaryInverse(image);
			
			// Advanced Morphology: Remove Objects
			int[] vaCoefficients2 = {1, 1, 1, 1, 1, 1, 1, 1, 1};
			StructuringElement vaStructElem2 = new StructuringElement(3, 3, vaCoefficients2);
			vaStructElem.Shape = StructuringElementShape.Square;
			// Filters particles based on their size.
			Algorithms.RemoveParticle(image, image, 5, SizeToKeep.KeepLarge, Connectivity.Connectivity8, vaStructElem2);
			
			// Basic Morphology - Applies morphological transformations to binary images.
			int[] vaCoefficients3 = {0, 1, 0, 1, 1, 1, 0, 1, 0};
			StructuringElement vaStructElem3 = new StructuringElement(3, 3, vaCoefficients3);
			vaStructElem.Shape = StructuringElementShape.Square;
			// Applies morphological transformations
			for(int i = 0; i < 3; ++i)
			{
				Algorithms.Morphology(image, image, MorphologyMethod.Erode, vaStructElem3);
			}

            // Advanced Morphology: Fill Holes
            VisionImage image1 = new VisionImage();
            Algorithms.FillHoles(image, image1, Connectivity.Connectivity8);

            // Particle Analysis - Computes the number of particles detected in a binary image and
            // returns the requested measurements about the particles.
            Collection<MeasurementType> vaPixelMeasurements = new Collection<MeasurementType>(new MeasurementType[] { MeasurementType.Area });
            Collection<MeasurementType> vaCalibratedMeasurements = new Collection<MeasurementType>(new MeasurementType[] { });
            IVA_Particle(image1, Connectivity.Connectivity8, vaPixelMeasurements, vaCalibratedMeasurements, ivaData, 16, out vaParticleReport, out vaParticleReportCalibrated);
            double[,] area = vaParticleReport.PixelMeasurements;
            double Maxarea = 0;
            for (int i = 0; i < area.GetLength(0); i++)
            {
                for (int j = 0; j < area.GetLength(1); j++)
                {
                    if (area[i,j] > Maxarea)
                    {
                        Maxarea = area[i,j];
                    }
                }
            }
            image1.Dispose();

            if (Maxarea>1000000)
            {
                // Creates a new, empty region of interest.
                Roi roi3 = new Roi();
                // Creates a new AnnulusContour using the given values.
                PointContour vaCenter3 = new PointContour(1295, 963);
                AnnulusContour vaOval3 = new AnnulusContour(vaCenter3, 496, 892, 0, 0);
                roi3.Add(vaOval3);
                // Reposition the region of interest based on the coordinate system.
                int coordSystemIndex2 = 0;
                Algorithms.TransformRoi(roi3, new CoordinateTransform(ivaData.baseCoordinateSystems[coordSystemIndex2], ivaData.MeasurementSystems[coordSystemIndex2]));
                // Extract the contour edges from the image
                CurveParameters vaCurveParams = new CurveParameters(ExtractionMode.NormalImage, 1, EdgeFilterSize.ContourTracing, 30, 20, 10, true);
                double[] vaConstraintMinArray = { };
                double[] vaConstraintMaxArray = { };
                ConnectionConstraintType[] vaConstraintTypeArray = { };
                ExtractContourReport vaExtractReport = IVA_ExtractContour(image, roi3, ExtractContourDirection.AnnulusOuterInner, vaCurveParams, vaConstraintTypeArray, vaConstraintMinArray, vaConstraintMaxArray, ExtractContourSelection.Closest);
                // Fit a circle to the contour
                ContourOverlaySettings vaEquationOverlay = new ContourOverlaySettings(true, Rgb32Value.GreenColor, 1, true);
                ContourOverlaySettings vaPointsOverlay = new ContourOverlaySettings(true, Rgb32Value.RedColor, 1, true);
                PartialCircle vaCircleReport = Algorithms.ContourFitCircle(image, 100, true);
                Algorithms.ContourOverlay(image, image, vaPointsOverlay, vaEquationOverlay);
                ComputeDistanceReport vaDistanceReport = Algorithms.ContourComputeDistances(image, image, 0);

                MaxDistance=0;
                MaxDistanceIndex=0;
                for (int i = 0; i < vaDistanceReport.Distances.Count; i++)
                {
                    if(vaDistanceReport.Distances[i].Distance>MaxDistance)
                    {
                        MaxDistance = vaDistanceReport.Distances[i].Distance;
                        MaxDistanceIndex = i;
                    }
                }
                var pos = vaDistanceReport.Distances[MaxDistanceIndex];
                distance[0] = MaxDistance;

                roi3.Dispose();

                // Creates a new, empty region of interest.
                Roi roi4 = new Roi();
                // Creates a new AnnulusContour using the given values.
                PointContour vaCenter4 = new PointContour(1294, 962);
                AnnulusContour vaOval4 = new AnnulusContour(vaCenter4, 499, 885, 0, 0);
                roi4.Add(vaOval4);
                // Reposition the region of interest based on the coordinate system.
                int coordSystemIndex3 = 0;
                Algorithms.TransformRoi(roi4, new CoordinateTransform(ivaData.baseCoordinateSystems[coordSystemIndex3], ivaData.MeasurementSystems[coordSystemIndex3]));
                // Extract the contour edges from the image
                CurveParameters vaCurveParams2 = new CurveParameters(ExtractionMode.NormalImage, 1, EdgeFilterSize.ContourTracing, 30, 25, 10, true);
                double[] vaConstraintMinArray2 = { };
                double[] vaConstraintMaxArray2 = { };
                ConnectionConstraintType[] vaConstraintTypeArray2 = { };
                ExtractContourReport vaExtractReport2 = IVA_ExtractContour(image, roi4, ExtractContourDirection.AnnulusInnerOuter, vaCurveParams2, vaConstraintTypeArray2, vaConstraintMinArray2, vaConstraintMaxArray2, ExtractContourSelection.Closest);
                // Fit a circle to the contour
                ContourOverlaySettings vaEquationOverlay2 = new ContourOverlaySettings(true, Rgb32Value.GreenColor, 1, true);
                ContourOverlaySettings vaPointsOverlay2 = new ContourOverlaySettings(true, Rgb32Value.RedColor, 1, true);
                PartialCircle vaCircleReport2 = Algorithms.ContourFitCircle(image, 100, true);
                Algorithms.ContourOverlay(image, image, vaPointsOverlay2, vaEquationOverlay2);
                ComputeDistanceReport vaDistanceReport2 = Algorithms.ContourComputeDistances(image, image, 0);

                MaxDistance1 = 0;
                MaxDistanceIndex1 = 0;
                for (int i = 0; i < vaDistanceReport2.Distances.Count; i++)
                {
                    if (vaDistanceReport2.Distances[i].Distance > MaxDistance1)
                    {
                        MaxDistance1 = vaDistanceReport2.Distances[i].Distance;
                        MaxDistanceIndex1 = i;
                    }
                }
                var pos1 = vaDistanceReport2.Distances[MaxDistanceIndex1];
                distance[1] = MaxDistance1;
                distance[2] = (vaCircleReport2.Center.X - vaCircularEdgeReport.Center.X) / 96;
                distance[3] = (vaCircleReport2.Center.Y - vaCircularEdgeReport.Center.Y) / 96;
                roi4.Dispose();
            }
            else
            {
                distance[0] = 9999;
                distance[1] = 9999;
                distance[2] = 9999;
                distance[3] = 9999;
            }

            // Dispose the IVA_Data structure.
			ivaData.Dispose();
            if(path== $"{ @"./ImageTemp/temp.jpg"}")
            {
                image.Dispose();
            }
			
			// Return the palette type of the final image.
			return PaletteType.Binary;


        }
    }
}


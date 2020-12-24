using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Text;
using NationalInstruments.Vision.WindowsForms;
using NationalInstruments.Vision;
using NationalInstruments.Vision.Analysis;
using Vision_Assistant.Utilities;

namespace Vision_Assistant
{
    static class FindCircularCenter
    {
        public static FindCircularEdgeReport vaCircularEdgeReport;
		

        
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
		

        public static PaletteType ProcessImage(VisionImage image)
        {
            // Initialize the IVA_Data structure to pass results and coordinate systems.
			IVA_Data ivaData = new IVA_Data(2, 0);
			
			// Creates a new, empty region of interest.
			Roi roi = new Roi();
			// Creates a new AnnulusContour using the given values.
			PointContour vaCenter = new PointContour(1298, 969);
            AnnulusContour vaOval = new AnnulusContour(vaCenter, 289, 858, 0, 0);
            roi.Add(vaOval);
            // Find Circular Edge
			EdgeOptions vaOptions = new EdgeOptions();
			vaOptions.ColumnProcessingMode = ColumnProcessingMode.Average;
			vaOptions.InterpolationType = InterpolationMethod.Bilinear;
			vaOptions.KernelSize = 3;
			vaOptions.MinimumThreshold = 50;
			vaOptions.Polarity = EdgePolaritySearchMode.Rising;
			vaOptions.Width = 3;
            CircularEdgeFitOptions vaFitOptions = new CircularEdgeFitOptions();
            vaFitOptions.ProcessType = RakeProcessType.GetBestEdges;
            vaFitOptions.StepSize = 10;
            vaFitOptions.MaxPixelRadius = 3;

			vaCircularEdgeReport = IVA_FindCircularEdge(image, roi, SpokeDirection.InsideToOutside, vaOptions, vaFitOptions, ivaData, 1);
			
			roi.Dispose();
            
            // Dispose the IVA_Data structure.
			ivaData.Dispose();
			
			// Return the palette type of the final image.
			return PaletteType.Gray;

        }
    }
}


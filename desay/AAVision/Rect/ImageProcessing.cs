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
    static class Image_Processing
    {
        public static Collection<GeometricEdgeBasedPatternMatch> gpm2Results;



        private static Collection<GeometricEdgeBasedPatternMatch> IVA_MatchGeometricPattern2(VisionImage image,
                                                                                                    string templatePath,
                                                                                                    CurveOptions curveOptions,
                                                                                                    MatchGeometricPatternEdgeBasedOptions matchOptions,
                                                                                                    IVA_Data ivaData,
                                                                                                    int stepIndex,
                                                                                                    Roi roi)
        {

            // Geometric Matching (Edge Based)

            // Creates the image template.
            using (VisionImage imageTemplate = new VisionImage(ImageType.U8, 7))
            {
                // Read the image template.
                imageTemplate.ReadVisionFile(templatePath);

                Collection<GeometricEdgeBasedPatternMatch> gpmResults = Algorithms.MatchGeometricPatternEdgeBased(image, imageTemplate, curveOptions, matchOptions, roi);

                // Store the results in the data structure.

                // First, delete all the results of this step (from a previous iteration)
                Functions.IVA_DisposeStepResults(ivaData, stepIndex);

                ivaData.stepResults[stepIndex].results.Add(new IVA_Result("# Matches", gpmResults.Count));

                for (int i = 0; i < gpmResults.Count; ++i)
                {
                    ivaData.stepResults[stepIndex].results.Add(new IVA_Result(String.Format("Match {0}.X Position (Pix.)", i + 1), gpmResults[i].Position.X));
                    ivaData.stepResults[stepIndex].results.Add(new IVA_Result(String.Format("Match {0}.Y Position (Pix.)", i + 1), gpmResults[i].Position.Y));

                    // If the image is calibrated, log the calibrated results.
                    if ((image.InfoTypes & InfoTypes.Calibration) != 0)
                    {
                        ivaData.stepResults[stepIndex].results.Add(new IVA_Result(String.Format("Match {0}.X Position (World)", i + 1), gpmResults[i].CalibratedPosition.X));
                        ivaData.stepResults[stepIndex].results.Add(new IVA_Result(String.Format("Match {0}.Y Position (World)", i + 1), gpmResults[i].CalibratedPosition.Y));
                    }

                    ivaData.stepResults[stepIndex].results.Add(new IVA_Result(String.Format("Match {0}.Angle (degrees)", i + 1), gpmResults[i].Rotation));
                    ivaData.stepResults[stepIndex].results.Add(new IVA_Result(String.Format("Match {0}.Scale", i + 1), gpmResults[i].Scale));
                    ivaData.stepResults[stepIndex].results.Add(new IVA_Result(String.Format("Match {0}.Score", i + 1), gpmResults[i].Score));
                    ivaData.stepResults[stepIndex].results.Add(new IVA_Result(String.Format("Match {0}.Occlusion", i + 1), gpmResults[i].Occlusion));
                    ivaData.stepResults[stepIndex].results.Add(new IVA_Result(String.Format("Match {0}.Template Target Curve Score", i + 1), gpmResults[i].TemplateMatchCurveScore));
                    ivaData.stepResults[stepIndex].results.Add(new IVA_Result(String.Format("Match {0}.Correlation Score", i + 1), gpmResults[i].CorrelationScore));
                }

                return gpmResults;
            }
        }


        public static PaletteType ProcessImage(VisionImage image)
        {
            // Initialize the IVA_Data structure to pass results and coordinate systems.
			IVA_Data ivaData = new IVA_Data(3, 0);
			
			// Extract Color Plane
			using (VisionImage plane = new VisionImage(ImageType.U8, 7))
			{
				// Extract the green color plane and copy it to the main image.
				Algorithms.ExtractColorPlanes(image, ColorMode.Rgb, null, plane, null);
				Algorithms.Copy(plane, image);
			}
			
			// Creates a new, empty region of interest.
			Roi roi = new Roi();
            // Creates a new RectangleContour using the given values.
            RectangleContour vaRect = new RectangleContour(1000, 800, 2250, 1850);
            roi.Add(vaRect);
            // Geometric Matching
 			string vaTemplateFile = $"{ @"./VisionModel/Polygon_20.5M/Mode.png"}";
            CurveOptions vaCurveOptions = new CurveOptions();
			vaCurveOptions.ColumnStepSize = 15;
			vaCurveOptions.ExtractionMode = ExtractionMode.NormalImage;
			vaCurveOptions.FilterSize = EdgeFilterSize.Normal;
			vaCurveOptions.MaximumEndPointGap = 10;
			vaCurveOptions.MinimumLength = 25;
			vaCurveOptions.RowStepSize = 15;
            vaCurveOptions.Threshold = Position.Instance.RectPosThreshold;
			
			MatchGeometricPatternEdgeBasedOptions matchGPMOptions = new MatchGeometricPatternEdgeBasedOptions();
			matchGPMOptions.Advanced.ContrastMode = ContrastMode.Original;
			matchGPMOptions.Advanced.MatchStrategy = GeometricMatchingSearchStrategy.Conservative;
			matchGPMOptions.MinimumMatchScore = 800;
			matchGPMOptions.Mode = GeometricMatchModes.RotationInvariant | GeometricMatchModes.ScaleInvariant | GeometricMatchModes.OcclusionInvariant;
            matchGPMOptions.NumberOfMatchesRequested = 2;
			double[] vaRangesMin = {-20, 0, 85, 0};
			double[] vaRangesMax = {20, 0, 105, 10};
			matchGPMOptions.OcclusionRange = new Range(vaRangesMin[3], vaRangesMax[3]);
			matchGPMOptions.RotationAngleRanges.Add(new Range(vaRangesMin[0], vaRangesMax[0]));
			matchGPMOptions.RotationAngleRanges.Add(new Range(vaRangesMin[1], vaRangesMax[1])); 
            matchGPMOptions.ScaleRange = new Range(vaRangesMin[2], vaRangesMax[2]);
			matchGPMOptions.SubpixelAccuracy = true;
			
			gpm2Results = IVA_MatchGeometricPattern2(image, vaTemplateFile, vaCurveOptions, matchGPMOptions, ivaData, 2, roi);
         
            roi.Dispose();
            
            // Dispose the IVA_Data structure.
			ivaData.Dispose();
			
			// Return the palette type of the final image.
			return PaletteType.Gray;

        }
    }
}


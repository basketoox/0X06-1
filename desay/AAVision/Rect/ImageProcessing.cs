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
        public static Collection<PatternMatchReport> pmResults;



        private static Collection<PatternMatchReport> IVA_MatchPattern(VisionImage image,
                                                         IVA_Data ivaData,
                                                         string templatePath,
                                                         MatchingAlgorithm algorithm,
                                                         float[] angleRangeMin,
                                                         float[] angleRangeMax,
                                                         int[] advOptionsItems,
                                                         double[] advOptionsValues,
                                                         int numAdvancedOptions,
                                                         int matchesRequested,
                                                         float score,
                                                         Roi roi,
                                                         int stepIndex)
        {

            FileInformation fileInfo;
            fileInfo = Algorithms.GetFileInformation(templatePath);
            using (VisionImage imageTemplate = new VisionImage(fileInfo.ImageType, 7))
            {
                int numObjectResults = 4;
                Collection<PatternMatchReport> patternMatchingResults = new Collection<PatternMatchReport>();

                // Read the image template.
                imageTemplate.ReadVisionFile(templatePath);

                // If the image is calibrated, we also need to log the calibrated position (x and y) and angle -> 7 results instead of 4
                if ((image.InfoTypes & InfoTypes.Calibration) != 0)
                {
                    numObjectResults = 7;
                }

                // Set the angle range.
                Collection<RotationAngleRange> angleRange = new Collection<RotationAngleRange>();
                for (int i = 0; i < 2; ++i)
                {
                    angleRange.Add(new RotationAngleRange(angleRangeMin[i], angleRangeMax[i]));
                }

                // Set the advanced options.
                Collection<PMMatchAdvancedSetupDataOption> advancedMatchOptions = new Collection<PMMatchAdvancedSetupDataOption>();
                for (int i = 0; i < numAdvancedOptions; ++i)
                {
                    advancedMatchOptions.Add(new PMMatchAdvancedSetupDataOption((MatchSetupOption)advOptionsItems[i], advOptionsValues[i]));
                }

                // Searches for areas in the image that match a given pattern.
                patternMatchingResults = Algorithms.MatchPattern3(image, imageTemplate, algorithm, matchesRequested, score, angleRange, roi, advancedMatchOptions);

                // ////////////////////////////////////////
                // Store the results in the data structure.
                // ////////////////////////////////////////

                // First, delete all the results of this step (from a previous iteration)
                Functions.IVA_DisposeStepResults(ivaData, stepIndex);

                if (patternMatchingResults.Count > 0)
                {
                    ivaData.stepResults[stepIndex].results.Add(new IVA_Result("# of objects", patternMatchingResults.Count));

                    for (int i = 0; i < patternMatchingResults.Count; ++i)
                    {
                        ivaData.stepResults[stepIndex].results.Add(new IVA_Result(String.Format("Match {0}.X Position (Pix.)", i + 1), patternMatchingResults[i].Position.X));
                        ivaData.stepResults[stepIndex].results.Add(new IVA_Result(String.Format("Match {0}.Y Position (Pix.)", i + 1), patternMatchingResults[i].Position.Y));

                        // If the image is calibrated, add the calibrated positions.
                        if ((image.InfoTypes & InfoTypes.Calibration) != 0)
                        {
                            ivaData.stepResults[stepIndex].results.Add(new IVA_Result(String.Format("Match {0}.X Position (World)", i + 1), patternMatchingResults[i].CalibratedPosition.X));
                            ivaData.stepResults[stepIndex].results.Add(new IVA_Result(String.Format("Match {0}.Y Position (World)", i + 1), patternMatchingResults[i].CalibratedPosition.Y));
                        }

                        ivaData.stepResults[stepIndex].results.Add(new IVA_Result(String.Format("Match {0}.Angle (degrees)", i + 1), patternMatchingResults[i].Rotation));
                        if ((image.InfoTypes & InfoTypes.Calibration) != 0)
                        {
                            ivaData.stepResults[stepIndex].results.Add(new IVA_Result(String.Format("Match {0}.Calibrated Angle (degrees)", i + 1), patternMatchingResults[i].CalibratedRotation));
                        }

                        ivaData.stepResults[stepIndex].results.Add(new IVA_Result(String.Format("Match {0}.Score", i + 1), patternMatchingResults[i].Score));
                    }
                }

                return patternMatchingResults;

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
            RectangleContour vaRect = new RectangleContour(1280, 900, 950, 880);
            roi.Add(vaRect);
            // MatchPattern Grayscale
            string vaTemplateFile = AppConfig.PosMode_Rect;
            MatchingAlgorithm matchAlgorithm = MatchingAlgorithm.MatchGrayValuePyramid;
            float[] minAngleVals = { -20, 0 };
            float[] maxAngleVals = { 20, 0 };
            int[] advancedOptionsItems = { 100, 102, 106, 107, 108, 109, 114, 116, 117, 118, 111, 112, 113, 103, 104, 105 };
            double[] advancedOptionsValues = { 5, 10, 300, 0, 6, 1, 25, 0, 0, 0, 30, 10, 30, 1, 20, 0 };
            int numberAdvOptions = 16;
            int vaNumMatchesRequested = 1;
            float vaMinMatchScore = Position.Instance.RectPosScore;
            pmResults = IVA_MatchPattern(image, ivaData, vaTemplateFile, matchAlgorithm, minAngleVals, maxAngleVals, advancedOptionsItems, advancedOptionsValues, numberAdvOptions, vaNumMatchesRequested, vaMinMatchScore, roi, 2);

            roi.Dispose();
            
            // Dispose the IVA_Data structure.
			ivaData.Dispose();
			
			// Return the palette type of the final image.
			return PaletteType.Gray;

        }
    }
}


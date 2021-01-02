#define IVA_STORE_RESULT_NAMES

using System;
using System.Collections.ObjectModel;
using NationalInstruments.Vision;
using NationalInstruments.Vision.Analysis;

namespace Vision_Assistant.Utilities
{
    public enum IVA_ResultType { Numeric, Boolean, String }

    public struct IVA_ResultValue  /// A result in Vision Assistant can be of type double, bool, or string.
    {
        public double numVal;
        public bool boolVal;
        public string strVal;

        /// Constructor for numeric result value
        public IVA_ResultValue(double num)
        {
            numVal = num;
            boolVal = false;
            strVal = "";
        }

        /// Constructor for boolean result value
        public IVA_ResultValue(bool b)
        {
            numVal = 0;
            boolVal = b;
            strVal = "";
        }

        /// Constructor for string result value
        public IVA_ResultValue(string str)
        {
            numVal = 0;
            boolVal = false;
            strVal = str;
        }
    }

    public class IVA_Result
    {
        #if IVA_STORE_RESULT_NAMES
        public string resultName;       /// Result name
        #endif
        public IVA_ResultType type;         /// Result type
        public IVA_ResultValue resultVal;   /// Result value

        /// Constructor
        private IVA_Result(string name)
        {
            #if IVA_STORE_RESULT_NAMES
            resultName = name;
            #endif
        }

        /// Constructor for numeric results
        public IVA_Result(string name, double value)
            : this(name)
        {
            type = IVA_ResultType.Numeric;
            resultVal = new IVA_ResultValue(value);
        }

        /// Constructor for boolean results
        public IVA_Result(string name, bool value)
            : this(name)
        {
            type = IVA_ResultType.Boolean;
            resultVal = new IVA_ResultValue(value);
        }

        /// Constructor for string results
        public IVA_Result(string name, string value)
            : this(name)
        {
            type = IVA_ResultType.String;
            resultVal = new IVA_ResultValue(value);
        }
    }

    public class IVA_StepResults
    {
        #if IVA_STORE_RESULT_NAMES
        public string stepName;                 /// Result name
        #endif
        public Collection<IVA_Result> results;      /// Result value

        /// Constructor
        public IVA_StepResults()
        {
            #if IVA_STORE_RESULT_NAMES
            stepName = "";
            #endif
            results = new Collection<IVA_Result>();
        }
    }

    public class IVA_Data : IDisposable
    {
        public Collection<VisionImage> buffers;                     /// Vision Assistant Image Buffers
        public Collection<IVA_StepResults> stepResults;             /// Array of step results
        public Collection<CoordinateSystem> baseCoordinateSystems;  /// Base Coordinate Systems
        public Collection<CoordinateSystem> MeasurementSystems;     /// Measurement Coordinate Systems

        /// Constructor
        public IVA_Data(int numSteps, int numCoordSys)
        {
            buffers = new Collection<VisionImage>();
            stepResults = new Collection<IVA_StepResults>();
            baseCoordinateSystems = new Collection<CoordinateSystem>();
            MeasurementSystems = new Collection<CoordinateSystem>();

            for (int i = 0; i < Functions.IVA_MAX_BUFFERS; i++)
            {
                buffers.Add(new VisionImage());
            }

            for (int i = 0; i < numSteps; i++)
            {
                stepResults.Add(new IVA_StepResults());
            }

            for (int i = 0; i < numCoordSys; i++)
            {
                baseCoordinateSystems.Add(new CoordinateSystem());
                MeasurementSystems.Add(new CoordinateSystem());
            }
        }

        /// IDisposable pattern
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                /// Releases the memory allocated for the image buffers.
                foreach (VisionImage image in buffers)
                {
                    image.Dispose();
                }
            }
        }

        ~IVA_Data()
        {
            Dispose(false);
        }
    }

    public static class Functions
    {
        public const int IVA_MAX_BUFFERS = 20;

        /////////////////////////////////////////////////////////////////////////////////
        ///
        /// Function Name: IVA_DisposeStepResults
        ///
        /// Description  : Dispose of the results of a specific step.
        ///
        /// Parameters   : ivaData    -  Internal data structure
        ///                stepIndex  -  step index
        ///
        /// Return Value : success
        ///
        /////////////////////////////////////////////////////////////////////////////////
        public static void IVA_DisposeStepResults(IVA_Data ivaData, int stepIndex)
        {
            ivaData.stepResults[stepIndex].results.Clear();
        }

        /////////////////////////////////////////////////////////////////////////////////
        ///
        /// Function Name: IVA_GetNumericResult
        ///
        /// Description  : Retrieves a numeric result value from the data manager.
        ///
        /// Parameters   : ivaData      -  Internal data structure
        ///                stepIndex    -  Index of the step that produced the result
        ///                resultIndex  -  result index.
        ///
        /// Return Value : numeric result value
        ///
        /////////////////////////////////////////////////////////////////////////////////
        public static double IVA_GetNumericResult(IVA_Data ivaData, int stepIndex, int resultIndex)
        {
            double value = 0;

            if (resultIndex < ivaData.stepResults[stepIndex].results.Count)
            {
                value = ivaData.stepResults[stepIndex].results[resultIndex].resultVal.numVal;
            }

            return value;
        }

        /////////////////////////////////////////////////////////////////////////////////
        ///
        /// Function Name: IVA_PushBuffer
        ///
        /// Description  : Stores an image in a buffer
        ///
        /// Parameters   : ivaData       -  Internal data structure
        ///                image         -  image
        ///                bufferNumber  -  Buffer index
        ///
        /// Return Value : success
        ///
        /////////////////////////////////////////////////////////////////////////////////
        public static void IVA_PushBuffer(IVA_Data ivaData, VisionImage image, int bufferNumber)
        {
            /// Release the previous image that was contained in the buffer
            ivaData.buffers[bufferNumber].Dispose();

            /// Creates an image buffer of the same type of the source image.
            ivaData.buffers[bufferNumber] = new VisionImage(image.Type, 7);

            /// Copies the image in the buffer.
            Algorithms.Copy(image, ivaData.buffers[bufferNumber]);
        }


        /////////////////////////////////////////////////////////////////////////////////
        ///
        /// Function Name: IVA_PopBuffer
        ///
        /// Description  : Retrieves an image from a buffer
        ///
        /// Parameters   : ivaData       -  Internal data structure
        ///                bufferNumber  -  Buffer index
        ///
        /// Return Value : success
        ///
        /////////////////////////////////////////////////////////////////////////////////
        public static VisionImage IVA_GetBuffer(IVA_Data ivaData, int bufferNumber)
        {
            return ivaData.buffers[bufferNumber];
        }

        /////////////////////////////////////////////////////////////////////////////////
        ///
        /// Function Name: IVA_GetPoint
        ///
        /// Description  : Retrieves a point from the PointsResults array
        ///
        /// Parameters   : ivaData     -  Internal data structure
        ///                stepIndex   -  Index of the step that produced the point
        ///                pointIndex  -  Index of the x value.
        ///
        /// Return Value : point
        ///
        /////////////////////////////////////////////////////////////////////////////////
        public static PointContour IVA_GetPoint(IVA_Data ivaData, int stepIndex, int pointIndex)
        {
            PointContour point = new PointContour();

            if ((pointIndex + 1) < ivaData.stepResults[stepIndex].results.Count)
            {
                point.X = ivaData.stepResults[stepIndex].results[pointIndex].resultVal.numVal;
                point.Y = ivaData.stepResults[stepIndex].results[pointIndex + 1].resultVal.numVal;
            }
            return point;
        }

        /////////////////////////////////////////////////////////////////////////////////
        ///
        /// Function Name: IVA_SetPixelValue
        ///
        /// Description  : Initialize a PixelValue structure
        ///
        /// Parameters   : imageType          -  image type
        ///                grayscaleConstant
        ///                redConstant
        ///                greenConstant
        ///                blueConstant
        ///
        /// Return Value : a PixelValue with the specified parameters
        ///
        /////////////////////////////////////////////////////////////////////////////////
        public static PixelValue IVA_SetPixelValue(ImageType imageType,
                                                   float grayscaleConstant,
                                                   byte redConstant,
                                                   byte greenConstant,
                                                   byte blueConstant)
        {
            PixelValue pVal;

            /// Sets the pixel value.
            switch (imageType)
            {
                case ImageType.U8:
                case ImageType.I16:
                case ImageType.U16:
                case ImageType.Single:
                    pVal = new PixelValue(grayscaleConstant);
                    break;
                case ImageType.Complex:
                    pVal = new PixelValue(new Complex());
                    break;
                case ImageType.Rgb32:
                    pVal = new PixelValue(new Rgb32Value(redConstant, greenConstant, blueConstant));
                    break;
                case ImageType.RgbU64:
                    pVal = new PixelValue(new RgbU64Value(redConstant, greenConstant, blueConstant));
                    break;
                case ImageType.Hsl32:
                    pVal = new PixelValue(new Hsl32Value());
                    break;
                default:
                    pVal = new PixelValue();
                    break;
            }
            return pVal;
        }

        ////////////////////////////////////////////////////////////////////////////////
        //
        // Function Name: IVA_Classification_Segmentation
        //
        // Description  : Segments the classification image
        //
        // Parameters   : image                 - Input Image
        //                imageMask             - Segmented image
        //                roi                   - Region of Interest
        //                preprocessingOptions  - Preprocessing options
        //
        // Return Value : None
        //
        ////////////////////////////////////////////////////////////////////////////////
        public static void IVA_Classification_Segmentation(VisionImage image, VisionImage imageMask, Roi roi, ParticleClassifierPreprocessingOptions preprocessingOptions)
        {
           int useExpandedROI = 0;
           RectangleContour boundingBox = roi.GetBoundingRectangle(), expandedBox = roi.GetBoundingRectangle(), reducedBox = roi.GetBoundingRectangle(); 
           
           // Local Threshold method uses a kernel size, and the ROI must be larger than the kernel size for the function to work
           // Take care of making the ROI to extract larger if needed for the local threshold case 
           if (preprocessingOptions.ThresholdType == ThresholdType.Local)
           {
               // Get the image size
               int xRes, yRes;
               xRes = image.Width;
               yRes = image.Height;
               boundingBox = roi.GetBoundingRectangle();
       
               // Take into account clipping of ROI. Just get ROI that's within bounds of image.
               if (Math.Min(xRes, (boundingBox.Left + boundingBox.Width)) - Math.Max(0, boundingBox.Left) < preprocessingOptions.LocalThresholdOptions.WindowWidth || Math.Min(yRes, (boundingBox.Top + boundingBox.Height)) - Math.Max(0, boundingBox.Top) < preprocessingOptions.LocalThresholdOptions.WindowHeight)
               {
                  // The ROI is smaller than the kernel. Try to expand the kernel in the directions needed to meet the minimum size required by the kernel.
                  
            
                  int expandX = (int)(preprocessingOptions.LocalThresholdOptions.WindowWidth / 2);
                  int expandY = (int)(preprocessingOptions.LocalThresholdOptions.WindowHeight / 2); 
                  int leftExpand = expandX;
                  int rightExpand = expandX;
                  int leftSlack = (int)boundingBox.Left;
                  int rightSlack = (int)(xRes - (boundingBox.Left + boundingBox.Width));

                  if (leftExpand > leftSlack) 
                  {
                     rightExpand += (leftExpand - leftSlack);
                  }

                  if (rightExpand > rightSlack) 
                  {
                     leftExpand += (rightExpand - rightSlack);
                  }

                  int leftOut = (int)boundingBox.Left - leftExpand;
                  if (leftOut < 0) 
                  {
                     leftOut = 0;
                  }
                  int rightOut = (int)(boundingBox.Left + boundingBox.Width + rightExpand);
                  if (rightOut > xRes) 
                  {
                     rightOut = xRes;
                  }

                  int topExpand = expandY;
                  int bottomExpand = expandY;
                  int topSlack = (int)boundingBox.Top;
                  int bottomSlack = (int)(yRes - (boundingBox.Top + boundingBox.Height));
                  if (topExpand > topSlack) 
                  {
                     bottomExpand += (topExpand - topSlack);
                  }
                  if (bottomExpand > bottomSlack) 
                  {
                     topExpand += (bottomExpand - bottomSlack);
                  }
                  int topOut = (int)(boundingBox.Top - topExpand);
                  if (topOut < 0) 
                  {
                     topOut = 0;
                  }
                  int bottomOut = (int)(boundingBox.Top + boundingBox.Height + bottomExpand);
                  if (bottomOut > yRes) {
                     bottomOut = yRes;
                  }
                  expandedBox.Initialize(leftOut, topOut, rightOut - leftOut, bottomOut - topOut);

                  // Create the reduced Rect so after performing the local threshold, we can reduce the size back to the original ROI dimensions.
                  reducedBox.Initialize(Math.Max(boundingBox.Left - leftOut, 0), Math.Max(boundingBox.Top - topOut, 0), boundingBox.Width + Math.Min(boundingBox.Left, 0), boundingBox.Height + Math.Min(boundingBox.Top, 0));

                  // Set this flag so the image can be reduced after performing the local threshold.
                  useExpandedROI = 1;
               }

            }

            // if Expanded Box hasn't been updated, use the boundingBox passed in to extract. 
            if (useExpandedROI == 0)
            {
               expandedBox = boundingBox;
            }

           
            // Extract the region of interest into the mask image.
            Algorithms.Extract(image, imageMask, expandedBox, 1, 1);

            // Create a temporary ROI that will be used to mask the extracted image, to get rid of
            // the pixels outside of the rotated rectangle.
            Roi tmpROI = new Roi(roi);

            // If the ROI is a rotated rectangle, then compute the new location of the search ROI.
            if ((roi[0].Type == ContourType.RotatedRectangle) && (((RotatedRectangleContour)roi[0].Shape).Angle > 0.01))
            {
                CoordinateSystem baseSystem = new CoordinateSystem();
                baseSystem.Origin.X = (roi.GetBoundingRectangle().Left < 0 ? 0 : roi.GetBoundingRectangle().Left);
                baseSystem.Origin.Y = (roi.GetBoundingRectangle().Top < 0 ? 0 : roi.GetBoundingRectangle().Top);
                baseSystem.Angle = 0;
                baseSystem.AxisOrientation = AxisOrientation.Direct;

                CoordinateSystem newSystem = new CoordinateSystem(new PointContour(0,0), 0, AxisOrientation.Direct);

                CoordinateTransform transform = new CoordinateTransform(baseSystem, newSystem);

                Algorithms.TransformRoi(tmpROI, transform);
            }

            // Create a temporary image.
            using (VisionImage tmpImageMask = new VisionImage(ImageType.U8, 7))
            {
                double thresholdMin;
                double thresholdMax;

               switch (preprocessingOptions.ThresholdType)
               {
                   case ThresholdType.Manual:
                     thresholdMin = preprocessingOptions.ManualThresholdRange.Minimum;
                     thresholdMax = preprocessingOptions.ManualThresholdRange.Maximum;
                     Algorithms.Threshold(imageMask, imageMask, new Range(thresholdMin, thresholdMax), true, 1);
                     break;
                   case ThresholdType.Auto:
                     Collection<ThresholdData> thresholdData;
                     thresholdData = Algorithms.AutoThreshold(image, tmpImageMask, 2, preprocessingOptions.AutoThresholdOptions.Method);

                     if (preprocessingOptions.AutoThresholdOptions.ParticleType == ParticleType.Bright)
                     {
                        thresholdMin = (thresholdData[0].Range.Maximum > preprocessingOptions.AutoThresholdOptions.Limits.Minimum ?
                                        thresholdData[0].Range.Maximum : preprocessingOptions.AutoThresholdOptions.Limits.Minimum);
                        thresholdMax = 255;
                     }
                     else
                     {
                        thresholdMin = 0;
                        thresholdMax = (thresholdData[0].Range.Maximum < preprocessingOptions.AutoThresholdOptions.Limits.Maximum ?
                                        thresholdData[0].Range.Maximum : preprocessingOptions.AutoThresholdOptions.Limits.Maximum);
                     }
                     Algorithms.Threshold(imageMask, imageMask, new Range(thresholdMin, thresholdMax), true, 1);
                     break;
                   case ThresholdType.Local:
                     LocalThresholdOptions Options = new LocalThresholdOptions(preprocessingOptions.LocalThresholdOptions.ParticleType, preprocessingOptions.LocalThresholdOptions.Method, 1.0, preprocessingOptions.LocalThresholdOptions.WindowWidth, preprocessingOptions.LocalThresholdOptions.WindowHeight);
                     Options.DeviationWeight = preprocessingOptions.LocalThresholdOptions.DeviationWeight;
                     Algorithms.LocalThreshold(imageMask, imageMask, Options);
                     break;
                  default:
                     break;
               }

               /// If the expanded ROI was used, reduce it so no particles are found outside requested ROI.
               if (useExpandedROI == 1)
               {
                  Algorithms.Extract(imageMask, imageMask, reducedBox, 1, 1);
               }

                // Cast the image to 8 bit.
                imageMask.Type = ImageType.U8;

                // Eliminates particles that touch the border of the image.
                if (preprocessingOptions.RejectBorder)
                {
                    if ((roi[0].Type == ContourType.RotatedRectangle) && (((RotatedRectangleContour)roi[0].Shape).Angle > 0.01))
                    {
                        // Special case for the rotated rectangle.
                        Algorithms.Label(imageMask, imageMask, Connectivity.Connectivity8);
            
                        Collection<short> lookupTable = new Collection<short>();
                        lookupTable.Add(0);
                        for (int i=1 ; i < 256 ; i++)
                            lookupTable.Add(1);

                        RoiProfileReport roiProfile = Algorithms.RoiProfile(imageMask, tmpROI);

                        for (int i=0 ; i < roiProfile.Report.ProfileData.Count ; i++)
                            lookupTable[0] = (short)roiProfile.Report.ProfileData[i];

                        Algorithms.UserLookup(imageMask, imageMask, lookupTable);
                    }
                    else
                        Algorithms.RejectBorder(imageMask, imageMask, Connectivity.Connectivity8);
                }

                // Remove small particles.
                if (preprocessingOptions.NumberOfErosions > 0)
                    Algorithms.RemoveParticle(imageMask, imageMask, preprocessingOptions.NumberOfErosions, SizeToKeep.KeepLarge);

                // If the rectangle is rotated, mask out the areas of the image that are not in the ROI.
                if ((roi[0].Type == ContourType.RotatedRectangle) && (((RotatedRectangleContour)roi[0].Shape).Angle > 0.01))
                {
                    // Perform the mask
                    Algorithms.RoiToMask(tmpImageMask, tmpROI, new PixelValue(255));
                    Algorithms.And(imageMask, tmpImageMask, imageMask);
                }

                // Sets the mask offset.
                imageMask.MaskOffset.X = Math.Max (0, roi.GetBoundingRectangle().Left);
                imageMask.MaskOffset.Y = Math.Max (0, roi.GetBoundingRectangle().Top);
            }
            tmpROI.Dispose();
        }

        ////////////////////////////////////////////////////////////////////////////////
        //
        // Function Name: IVA_Classification_Extract_Particles
        //
        // Description  : Extracts the region of interests of the bounding rectangles of
        //                all particles.
        //
        // Parameters   : image         - Input image
        //                imageMask     - Image mask
        //                rois          - Array of ROIs
        //
        // Return Value : success
        //
        ////////////////////////////////////////////////////////////////////////////////
        public static Collection<Roi> IVA_Classification_Extract_Particles(VisionImage image, VisionImage imageMask)
        {
            Collection<MeasurementType> measurements = new Collection<MeasurementType>();
            measurements.Add(MeasurementType.BoundingRectLeft);
            measurements.Add(MeasurementType.BoundingRectTop);
            measurements.Add(MeasurementType.BoundingRectRight);
            measurements.Add(MeasurementType.BoundingRectBottom);

            // Compute the region of interests around each particle.
            ParticleMeasurementsReport particleReport = Algorithms.ParticleMeasurements(imageMask, measurements, Connectivity.Connectivity8, ParticleMeasurementsCalibrationMode.Pixel);

            Collection<Roi> rois = new Collection<Roi>();

            for (int i = 0; i < particleReport.PixelMeasurements.GetLength(0); i++)
            {
                double top = particleReport.PixelMeasurements[i,1] + imageMask.MaskOffset.Y - 5;
                top = (top < 0 ? 0 : top);

                double left = particleReport.PixelMeasurements[i, 0] + imageMask.MaskOffset.X - 5;
                left = (left < 0 ? 0 : left);

                double bottom = particleReport.PixelMeasurements[i, 3] + imageMask.MaskOffset.Y + 5;
                bottom = (bottom > (image.Height - 1) ? (image.Height - 1) : bottom);

                double right = particleReport.PixelMeasurements[i, 2] + imageMask.MaskOffset.X + 5;
                right = (right > (image.Width - 1) ? (image.Width - 1) : right);

                Roi particleROI = new Roi();
                particleROI.Add(new RectangleContour(left, top, right - left + 1, bottom - top + 1));
                rois.Add(particleROI);
            }
            return rois;
        }

        //////////////////////////////////////////////////////////////////////////////
        //
        //  IVA_SplitRotatedRectangle
        //
        //  Description:
        //      Splits a rotated rectangle into two rotated rectangles
        //
        //  Parameters:
        //      roiToSplit  -  The rectangle roi to split.
        //      direction   -  The direction of the search; the split occurs in the
        //                     perpendicular direction
        //      firstRoi    -  The resulting first rectangle roi.
        //      secondRoi   -  The resulting second rectangle roi.
        //
        //  Return Value:
        //      None
        //
        //////////////////////////////////////////////////////////////////////////////
        public static void IVA_SplitRotatedRectangle(Roi roiToSplit, RakeDirection direction, out Roi firstRoi, out Roi secondRoi)
        {
            RotatedRectangleContour rectToSplit = (RotatedRectangleContour)roiToSplit[0].Shape;
            RotatedRectangleContour firstRect = new RotatedRectangleContour();
            RotatedRectangleContour secondRect = new RotatedRectangleContour();
            double xAdjustment;
            double yAdjustment;
            double shiftFactor;

            // Transfer the angle to each rectangle (the direction does not effect the angle).
            firstRect.Angle = rectToSplit.Angle;
            secondRect.Angle = rectToSplit.Angle;

            // Determine which direction to split the rectangle
            switch (direction)
            {
                case RakeDirection.LeftToRight:
                case RakeDirection.RightToLeft:
                    // Split the rectangle down the verticle axis
                    firstRect.Width = rectToSplit.Width / 2;
                    firstRect.Height = rectToSplit.Height;
                    secondRect.Width = rectToSplit.Width / 2;
                    secondRect.Height = rectToSplit.Height;

                    // Calculate the shift for the left coordinate of the two rects
                    shiftFactor = (double)(rectToSplit.Width / 4.0);

                    // Set the size of the adjustment
                    xAdjustment = (double)(Math.Cos(rectToSplit.Angle * Math.PI / 180) * shiftFactor);
                    yAdjustment = (double)(Math.Sin(rectToSplit.Angle * Math.PI / 180) * shiftFactor);

                    // Fix the directions for the adjustments
                    if (direction == RakeDirection.LeftToRight)
                    {
                        firstRect.Center.X = (int)(rectToSplit.Center.X - xAdjustment  + (shiftFactor));
                        firstRect.Center.Y = (int)(rectToSplit.Center.Y - xAdjustment  + (shiftFactor));
                        secondRect.Center.X = (int)(rectToSplit.Center.X + xAdjustment  + (shiftFactor));
                        secondRect.Center.Y = (int)(rectToSplit.Center.Y - yAdjustment);
                    }
                    else
                    {
                        firstRect.Center.X = (int)(rectToSplit.Center.X + xAdjustment + (shiftFactor));
                        firstRect.Center.Y = (int)(rectToSplit.Center.Y - yAdjustment);
                        secondRect.Center.X = (int)(rectToSplit.Center.X - xAdjustment + (shiftFactor));
                        secondRect.Center.Y = (int)(rectToSplit.Center.Y + yAdjustment);
                    }
                    break;
                case RakeDirection.TopToBottom :
                case RakeDirection.BottomToTop :
                    // Split the rectangle down the horizontal axis
                    firstRect.Width  = rectToSplit.Width;
                    firstRect.Height = rectToSplit.Height / 2;
                    secondRect.Width = rectToSplit.Width;
                    secondRect.Height = rectToSplit.Height / 2;

                    // Calculate the shift for the top coordinate of the two rects
                    shiftFactor = (double)(rectToSplit.Height / 4.0);

                    // Set the size of the adjustment
                    xAdjustment = (double)(Math.Sin(rectToSplit.Angle * Math.PI / 180) * shiftFactor);
                    yAdjustment = (double)(Math.Cos(rectToSplit.Angle * Math.PI / 180) * shiftFactor);

                    // Fix the directions for the adjustments
                    if (direction == RakeDirection.TopToBottom)
                    {
                        firstRect.Center.X = (int)(rectToSplit.Center.X - xAdjustment);
                        firstRect.Center.Y = (int)(rectToSplit.Center.Y - yAdjustment + (shiftFactor));
                        secondRect.Center.X = (int)(rectToSplit.Center.X + xAdjustment);
                        secondRect.Center.Y = (int)(rectToSplit.Center.Y  + yAdjustment  + (shiftFactor));
                    }
                    else
                    {
                        firstRect.Center.X = (int)(rectToSplit.Center.X + xAdjustment);
                        firstRect.Center.Y = (int)(rectToSplit.Center.Y  + yAdjustment + (shiftFactor));
                        secondRect.Center.X = (int)(rectToSplit.Center.X - xAdjustment);
                        secondRect.Center.Y = (int)(rectToSplit.Center.Y  - yAdjustment + (shiftFactor));
                    }
                    break;
                }
            firstRoi = new Roi(firstRect);
            secondRoi = new Roi(secondRect);
        }

        //////////////////////////////////////////////////////////////////////////////
        //
        //  IVA_FindExtremeEdge
        //
        //  Description:
        //      Finds the edge that is either closest or farthest from the start
        //      of a set of rake lines.
        //
        //  Parameters:
        //      report          -   The rake report to process.
        //      findClosestEdge -   The direction of the main axis.
        //                          If false, report must contain both first and last
        //                          edge information
        //
        //  Return Value:
        //      The extreme edge
        //
        //////////////////////////////////////////////////////////////////////////////
        public static PointContour IVA_FindExtremeEdge(RakeReport report, bool findClosestEdge)
        {
            Double extremeDistance;
            Double tempDistance;
            PointContour edge = new PointContour(0,0);

            // Determine if the function should process the first edges or the last edges array
            if (findClosestEdge)
            {
                if (report.FirstEdges.Count > 0 )
                {
                    extremeDistance = report.FirstEdges[0].Distance;
                    edge = report.FirstEdges[0].Position;

                    // Then find the edge that is closest to the boundary of the search area
                    for (int i = 1 ; i < report.FirstEdges.Count ; i++)
                    {
                        tempDistance = report.FirstEdges[i].Distance;
                        if (tempDistance < extremeDistance)
                        {
                            extremeDistance = tempDistance;
                            edge = report.FirstEdges[i].Position;
                        }
                    }
                }
            }
            else
            {
                if( report.LastEdges.Count > 0 )
                {
                    // First, intialize the value with the first edge in the array
                    extremeDistance = report.LastEdges[0].Distance;
                    edge = report.LastEdges[0].Position;

                    // Then find the edge that are closest to the boundary of the search area
                    for (int i = 1 ; i < report.LastEdges.Count ; i++ )
                    {
                        tempDistance = report.LastEdges[i].Distance;
                        if (tempDistance < extremeDistance)
                        {
                            extremeDistance = tempDistance;
                            edge = report.LastEdges[i].Position;
                        }
                    }
                }
            }
            
            return edge;
        }

        //////////////////////////////////////////////////////////////////////////////
        //
        //  IVA_MeasureMinDistance
        //
        //  Description:
        //      Measures the minimum distance between edges of two rake reports
        //
        //  Parameters:
        //      firstReport             -   The first rake report.
        //      secondReport            -   The second rake report.
        //      firstPerpendicularLine  -   Upon return, a line perpendicular to the 
        //                                  first edge point. 
        //      lastPerpendicularLine   -   Upon return, a line perpendicular to the 
        //                                  last edge point.
        //      distance                -   Upon return, the distance between the two
        //                                  perpendicular lines.
        //      firstEdge               -   Upon return, the location of the first edge.
        //      lastEdge                -   Upon return, the location of the last edge.
        //
        //  Return Value:
        //      The distance
        //
        //////////////////////////////////////////////////////////////////////////////
        public static double IVA_MeasureMinDistance(RakeReport firstReport, RakeReport secondReport, out LineContour firstPerpendicularLine, out LineContour lastPerpendicularLine, out PointContour firstEdge, out PointContour lastEdge)
        {
            // Intialize all returned data to zero
            firstPerpendicularLine = new LineContour(new PointContour(0,0), new PointContour(0,0));
            lastPerpendicularLine = new LineContour(new PointContour(0,0), new PointContour(0,0));
            firstEdge = new PointContour(0,0);
            lastEdge =  new PointContour(0,0);
            double distance = 0;

            // Only proceed if there was at least one rake line in each rake report
            if (firstReport.SearchLines.Count > 0 && secondReport.SearchLines.Count > 0)
            {
                // Find the edges that are closest to the boundaries of each search area
                firstEdge = IVA_FindExtremeEdge(firstReport, true);
                lastEdge =  IVA_FindExtremeEdge(secondReport, true);

                // Find the edge lines by find perpendicular lines from the edges to the rake lines.
                LineContour tempPerpendicularLine1 = Algorithms.FindPerpendicularLine(firstReport.SearchLines[0].Line, firstEdge);
                LineContour tempPerpendicularLine2 = Algorithms.FindPerpendicularLine(firstReport.SearchLines[firstReport.SearchLines.Count - 1].Line, firstEdge);
                firstPerpendicularLine.Start = tempPerpendicularLine1.End;
                firstPerpendicularLine.End = tempPerpendicularLine2.End;
                tempPerpendicularLine1 = Algorithms.FindPerpendicularLine(firstReport.SearchLines[0].Line, lastEdge);
                tempPerpendicularLine2 = Algorithms.FindPerpendicularLine(firstReport.SearchLines[firstReport.SearchLines.Count - 1].Line, lastEdge);
                lastPerpendicularLine.Start = tempPerpendicularLine1.End;
                lastPerpendicularLine.End = tempPerpendicularLine2.End;

                // Compute the distance
                distance = Algorithms.FindPointDistances(new Collection<PointContour>(new PointContour[]{firstPerpendicularLine.Start, lastPerpendicularLine.Start}))[0];
            }
            return distance;
        }

        //////////////////////////////////////////////////////////////////////////////
        //
        //  IVA_ClampMin_Algorithm
        //
        //  Description:
        //      Measures a distance from the center of the search area towards the 
        //      sides of the search area.
        //
        //  Parameters:
        //      image      -  The image that the function uses for distance 
        //                    measurement.
        //      roi        -  The coordinate location of the rectangular search area 
        //                    of the distance measurement.
        //      direction  -  The direction the function search for edges along the 
        //                    search lines.
        //      stepSize   -  step size
        //      options    -  Describes how you want the function to detect edges
        //      firstEdge  -  Upon return, the coordinate location of the first edge 
        //                    used to measure the distance.
        //      lastEdge   -  Upon return, the coordinate location of the last edge 
        //                    used to measure the distance.
        //
        //  Return Value:
        //      The distance measured between the two parallel hit-lines
        //
        //////////////////////////////////////////////////////////////////////////////
        public static double IVA_ClampMin_Algorithm(VisionImage image, Roi roi, RakeDirection direction, int stepSize, EdgeOptions options, out PointContour firstEdge, out PointContour lastEdge)
        {
            Roi firstSearchRectROI;
            Roi secondSearchRectROI;

            // Separate the rect into two rectangles
            IVA_SplitRotatedRectangle(roi, direction, out firstSearchRectROI, out secondSearchRectROI);

            // Calculate the edge locations for the second half rect 
            RakeReport secondReport = Algorithms.Rake(image, secondSearchRectROI, direction, EdgeProcess.First, stepSize, options);

            // Find the opposite direction, then calculate the edge locations for the second half rect 
            RakeDirection oppositeDirection = RakeDirection.BottomToTop;
            switch (direction)
            {
                case RakeDirection.LeftToRight:
                    oppositeDirection = RakeDirection.RightToLeft;
                    break;
                case RakeDirection.RightToLeft:
                    oppositeDirection = RakeDirection.LeftToRight;
                    break;
                case RakeDirection.TopToBottom:
                    oppositeDirection = RakeDirection.BottomToTop;
                    break;
                case RakeDirection.BottomToTop:
                    oppositeDirection = RakeDirection.TopToBottom;
                    break;
            }
            
            RakeReport firstReport = Algorithms.Rake(image, firstSearchRectROI, oppositeDirection, EdgeProcess.First, stepSize, options);

            // Use the rake report to calculate the minimum distance
            LineContour firstPerpendicularLine;
            LineContour lastPerpendicularLine;
            double distance = IVA_MeasureMinDistance(firstReport, secondReport, out firstPerpendicularLine, out lastPerpendicularLine, out firstEdge, out lastEdge);

            // Dispose of the ROIs
            secondSearchRectROI.Dispose();
            firstSearchRectROI.Dispose();

            return distance;
        }

        //////////////////////////////////////////////////////////////////////////////
        //
        //  IVA_MeasureMaxDistance
        //
        //  Description:
        //      Measures the maximum distance between edges of a rake report
        //
        //  Parameters:
        //      reportToProcess         -   The rake report. Must be generated with 
        //                                  process equal to IMAQ_FIRST_AND_LAST or 
        //                                  IMAQ_ALL. 
        //      firstPerpendicularLine  -   Upon return, a line perpendicular to the 
        //                                  first edge point. 
        //                                  This is an optional parameter.
        //      lastPerpendicularLine   -   Upon return, a line perpendicular to the 
        //                                  last edge point.
        //      distance                -   Upon return, the distance between the two
        //                                  perpendicular lines.
        //      firstEdge               -   Upon return, the location of the first edge.
        //      lastEdge                -   Upon return, the location of the last edge.
        //
        //  Return Value:
        //      TRUE if successful, FALSE if there was an error
        //
        //////////////////////////////////////////////////////////////////////////////
        public static double IVA_MeasureMaxDistance(RakeReport reportToProcess,
                                                    out LineContour firstPerpendicularLine,
                                                    out LineContour lastPerpendicularLine,
                                                    out PointContour firstEdge,
                                                    out PointContour lastEdge)
        {
            // Intialize all returned data to zero
            firstPerpendicularLine = new LineContour(new PointContour(0, 0), new PointContour(0, 0));
            lastPerpendicularLine = new LineContour(new PointContour(0, 0), new PointContour(0, 0));
            double distance = 0;
            firstEdge = new PointContour(0, 0);
            lastEdge = new PointContour(0, 0);

            // Only proceed if there was at least one rake line
            if (reportToProcess.SearchLines.Count > 0)
            {
                // Locate the two extreme edges
                firstEdge = IVA_FindExtremeEdge(reportToProcess, true);
                lastEdge = IVA_FindExtremeEdge(reportToProcess, false);
    
                // Find the edge lines by find perpendicular lines from the edges to the rake lines.
                LineContour tempPerpendicularLine1 = Algorithms.FindPerpendicularLine(reportToProcess.SearchLines[0].Line, firstEdge);
                LineContour tempPerpendicularLine2 = Algorithms.FindPerpendicularLine(reportToProcess.SearchLines[reportToProcess.SearchLines.Count - 1].Line, firstEdge);
                firstPerpendicularLine.Start = tempPerpendicularLine1.End;
                firstPerpendicularLine.End = tempPerpendicularLine2.End;
                tempPerpendicularLine1 = Algorithms.FindPerpendicularLine(reportToProcess.SearchLines[0].Line, lastEdge);
                tempPerpendicularLine2 = Algorithms.FindPerpendicularLine(reportToProcess.SearchLines[reportToProcess.SearchLines.Count - 1].Line, lastEdge);
                lastPerpendicularLine.Start = tempPerpendicularLine1.End;
                lastPerpendicularLine.End = tempPerpendicularLine2.End;

                // Compute the distance
                distance = Algorithms.FindPointDistances(new Collection<PointContour>(new PointContour[]{firstPerpendicularLine.Start, lastPerpendicularLine.Start}))[0];
            }
            return distance;
        }

        //////////////////////////////////////////////////////////////////////////////
        //
        //  IVA_ComputePMOffset
        //
        //  Description:
        //      Computes the offset based on the new angle
        //
        //  Parameters:
        //      matchOffsetX  -  match offset X
        //      matchOffsetY  -  match offset Y
        //      angle         -  match angle
        //
        //  Return Value:
        //      offset as PointContour
        //
        //////////////////////////////////////////////////////////////////////////////
        public static PointContour IVA_ComputePMOffset(double matchOffsetX, double matchOffsetY, double angle)
        {
            Collection<PointContour> points = new Collection<PointContour>();
            points.Add(new PointContour(0, 0));
            points.Add(new PointContour(matchOffsetX, matchOffsetY));

            Collection<double> distances = Algorithms.FindPointDistances(points);
            double r = distances[0];
            double teta = Math.Atan2(matchOffsetY, matchOffsetX);
            double newTeta = teta - (angle * 2 * Math.PI / 360);

            PointContour matchOffset = new PointContour(r * Math.Cos(newTeta), r * Math.Sin(newTeta));

            return matchOffset;
        }
    }
}

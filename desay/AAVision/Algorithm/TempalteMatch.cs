using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace desay.AAVision.Algorithm
{
    public static class TempalteMatch
    {
        public static double[] action(HWindow window, HObject image, string ROIFileName, string ModelFileName)
        {
            double[] data = new double[] { -100, -100 };
            #region
            // Local iconic variables 
             
            HObject ho_RegionROI, ho_ImageReduced;

            // Local control variables 

            HTuple hv_ModelID = null;
            HTuple hv_Row = null, hv_Column = null;
            HTuple hv_Angle = null, hv_Score = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_RegionROI);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            #endregion
            try
            {
                ho_RegionROI.Dispose();
                HOperatorSet.ReadRegion(out ho_RegionROI, ROIFileName);
                HOperatorSet.ReadShapeModel(ModelFileName, out hv_ModelID);
            }
            catch (Exception)
            {
                MessageBox.Show("在读取ROI和模板时出现错误，请检查模板和ROI文件是否存在于路径！", "错误信息",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return data;
            }
            try
            {
                ho_ImageReduced.Dispose();
            HOperatorSet.ReduceDomain(image, ho_RegionROI, out ho_ImageReduced);
            HOperatorSet.FindShapeModel(ho_ImageReduced, hv_ModelID, -0.39, 0.79, 0.4, 1,
                0.5, "least_squares_high", 4, 0.9, out hv_Row, out hv_Column, out hv_Angle, out hv_Score);
            if ((int)(new HTuple((new HTuple(hv_Score.TupleLength())).TupleGreater(0.7))) != 0)
            {
                HOperatorSet.ClearShapeModel(hv_ModelID);
                ho_RegionROI.Dispose();
                ho_ImageReduced.Dispose();
                
                data[0] = hv_Column;
                data[1] = hv_Row;
                return data;
            }
            else
            {
                HOperatorSet.ClearShapeModel(hv_ModelID);
                ho_RegionROI.Dispose();
                ho_ImageReduced.Dispose();
                
                return data;
            }
            }
            catch (Exception)
            {
                MessageBox.Show("找模板失败！", "错误信息",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return data;
            }

        }
    }
}

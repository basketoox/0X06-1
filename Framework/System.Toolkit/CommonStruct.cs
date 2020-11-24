using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Toolkit
{
    /// <summary>
    /// 用户等级
    /// </summary>
    public enum UserLevel
    {
        None,
        操作员,
        工程师,
        管理员
    };
    public struct ArcParam<T>
    {
        public T X;
        public T Y;
        public T R;
        public int DIR;
    }
    public struct Point3D<T>
    {
        public T X;
        public T Y;
        public T Z;
        public override string ToString()
        {
            return X.ToString() + "," + Y.ToString() + "," + Z.ToString();
        }
        public static Point3D<T> Parse(string str)
        {
            string[] strValue = str.Split(',');
            var point3D = new Point3D<T>();
            point3D.X = (T)Convert.ChangeType(strValue[0], typeof(T));
            point3D.Y = (T)Convert.ChangeType(strValue[1], typeof(T));
            point3D.Z = (T)Convert.ChangeType(strValue[2], typeof(T));
            return point3D;
        }
    }
    public struct Point3Dxzc<T>
    {
        public T X;
        public T Z;
        public T C;
        public override string ToString()
        {
            return X.ToString() + "," + Z.ToString() + "," + C.ToString();
        }
        public static Point3Dxzc<T> Parse(string str)
        {
            string[] strValue = str.Split(',');
            var point3D = new Point3Dxzc<T>();
            point3D.X = (T)Convert.ChangeType(strValue[0], typeof(T));
            point3D.Z = (T)Convert.ChangeType(strValue[1], typeof(T));
            point3D.C = (T)Convert.ChangeType(strValue[2], typeof(T));
            return point3D;
        }
    }
    public struct Point<T>
    {
        public T X;
        public T Y;
        public override string ToString() => X.ToString() + "," + Y.ToString();
        public static Point<T> Parse(string str)
        {
            var pos = new Point<T>();
            string[] strValue = str.Split(',');
            pos.X = (T)Convert.ChangeType(strValue[0], typeof(T));
            pos.Y = (T)Convert.ChangeType(strValue[1], typeof(T));
            return pos;
        }
    }
    public struct Pointxz<T>
    {
        public T X;
        public T Z;
        public override string ToString() => X.ToString() + "," + Z.ToString();
        public static Pointxz<T> Parse(string str)
        {
            var pos = new Pointxz<T>();
            string[] strValue = str.Split(',');
            pos.X = (T)Convert.ChangeType(strValue[0], typeof(T));
            pos.Z = (T)Convert.ChangeType(strValue[1], typeof(T));
            return pos;
        }
    }

}
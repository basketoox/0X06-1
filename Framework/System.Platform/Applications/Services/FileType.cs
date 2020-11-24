using System.Collections.Generic;

namespace System.Platform.Applications.Services
{
    /// <summary>
    /// 表示文件类型.
    /// </summary>
    public class FileType
    {
        private readonly string description;
        private readonly string fileExtension;


        /// <summary>
        /// 初始化<see cref="FileType"/>类的新实例.
        /// </summary>
        /// <param name="description">文件类型的描述.</param>
        /// <param name="fileExtension">文件扩展名。这个字符串必须以 '.'开头的点。使用字符串".*" 允许所有文件扩展名.</param>
        /// <exception cref="ArgumentException">描述为空字符串或空字符串.</exception>
        /// <exception cref="ArgumentException">fileExtension是null，一个空字符串或不以'.'开始点字符.</exception>
        public FileType(string description, string fileExtension)
        {
            if (string.IsNullOrEmpty(description)) { throw new ArgumentException("The argument description must not be null or empty."); }
            if (string.IsNullOrEmpty(fileExtension)) { throw new ArgumentException("The argument fileExtension must not be null or empty."); }
            if (fileExtension[0] != '.') { throw new ArgumentException("The argument fileExtension must start with the '.' character."); }

            this.description = description;
            this.fileExtension = fileExtension;
        }

        /// <summary>
        /// 初始化<see cref="FileType"/>类的新实例.
        /// </summary>
        /// <param name="description">文件类型的描述.</param>
        /// <param name="fileExtensions">文件扩展名列表。每根弦都必须以'.'开头点.</param>
        /// <exception cref="ArgumentException">描述为空字符串或空字符串.</exception>
        /// <exception cref="ArgumentException">一个或多个文件扩展字符串不以'.'开头点字符.</exception>
        /// <exception cref="ArgumentNullException">fileExtensions为空.</exception>
        public FileType(string description, IEnumerable<string> fileExtensions)
            : this(description, string.Join(";*", CheckFileExtensions(fileExtensions)))
        {
        }


        /// <summary>
        /// 获取文件类型的描述.
        /// </summary>
        public string Description { get { return description; } }

        /// <summary>
        /// 获取文件扩展名。这个字符串以 '.'开头的点。多个文件扩展名与字符串“;*”连接为分隔符
        /// </summary>
        public string FileExtension { get { return fileExtension; } }


        private static IEnumerable<string> CheckFileExtensions(IEnumerable<string> fileExtensions)
        {
            if (fileExtensions == null) { throw new ArgumentNullException("fileExtensions"); }
            foreach (var fileExtension in fileExtensions)
            {
                if (string.IsNullOrEmpty(fileExtension) || fileExtension[0] != '.')
                {
                    throw new ArgumentException("The argument fileExtension must start with the '.' character.");
                }
            }
            return fileExtensions;
        }
    }
}

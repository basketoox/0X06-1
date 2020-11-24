namespace System.Platform.Applications.Services
{
    /// <summary>
    /// 包含文件对话框中工作的结果信息.
    /// </summary>
    public class FileDialogResult
    {
        private readonly string _fileName;
        private readonly FileType _selectedFileType;


        /// <summary>
        /// 使用空值初始化<see cref="FileDialogResult"/>类的新实例
        /// 当用户取消文件对话框时，使用此构造函数.
        /// </summary>
        public FileDialogResult() : this(null, null)
        {
        }

        /// <summary>
        /// 使用空值初始化<see cref="FileDialogResult"/>类的新实例.
        /// </summary>
        /// <param name="fileName">用户输入的文件名.</param>
        /// <param name="selectedFileType">用户选择的文件类型.</param>
        public FileDialogResult(string fileName, FileType selectedFileType)
        {
            _fileName = fileName;
            _selectedFileType = selectedFileType;
        }


        /// <summary>
        /// 获取一个值，该值指示此实例是否包含有效数据。这个属性返回 <c>false</c>
        /// 当用户取消文件对话框时.
        /// </summary>
        public bool IsValid { get { return FileName != null && SelectedFileType != null; } }

        /// <summary>
        /// 获取用户输入的文件名或当用户取消对话框时<c>null</c>.
        /// </summary>
        public string FileName { get { return _fileName; } }

        /// <summary>
        /// 获取用户选择的文件类型或当用户取消对话框时<c>null</c>.
        /// </summary>
        public FileType SelectedFileType { get { return _selectedFileType; } }
    }
}

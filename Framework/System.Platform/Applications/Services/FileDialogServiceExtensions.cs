using System.Collections.Generic;

namespace System.Platform.Applications.Services
{
    /// <summary>
    /// 为<see cref="IFileDialogService"/>提供方法重载，以简化其使用.
    /// </summary>
    public static class FileDialogServiceExtensions
    {
        /// <summary>
        /// 显示“打开文件”对话框，该对话框允许用户指定应该打开的文件.
        /// </summary>
        /// <param name="service">文件对话服务.</param>
        /// <param name="fileType">支持的文件类型.</param>
        /// <returns>文件对话框对象，包含用户选择的文件名.</returns>
        /// <exception cref="ArgumentNullException">服务不能为空.</exception>
        /// <exception cref="ArgumentNullException">文件类型不能为空.</exception>
        public static FileDialogResult ShowOpenFileDialog(this IFileDialogService service, FileType fileType)
        {
            if (service == null) { throw new ArgumentNullException("service"); }
            if (fileType == null) { throw new ArgumentNullException("fileType"); }
            return service.ShowOpenFileDialog(null, new[] { fileType }, fileType, null);
        }

        /// <summary>
        /// 显示“打开文件”对话框，该对话框允许用户指定应该打开的文件.
        /// </summary>
        /// <param name="service">文件对话服务.</param>
        /// <param name="owner">拥有这个OpenFileDialog的窗口.</param>
        /// <param name="fileType">支持的文件类型.</param>
        /// <returns>文件对话框对象，包含用户选择的文件名.</returns>
        /// <exception cref="ArgumentNullException">服务不能为空.</exception>
        /// <exception cref="ArgumentNullException">文件类型不能为空.</exception>
        public static FileDialogResult ShowOpenFileDialog(this IFileDialogService service, object owner, FileType fileType)
        {
            if (service == null) { throw new ArgumentNullException("service"); }
            if (fileType == null) { throw new ArgumentNullException("fileType"); }
            return service.ShowOpenFileDialog(owner, new[] { fileType }, fileType, null);
        }

        /// <summary>
        /// 显示“打开文件”对话框，该对话框允许用户指定应该打开的文件.
        /// </summary>
        /// <param name="service">文件对话服务.</param>
        /// <param name="fileType">支持的文件类型.</param>
        /// <param name="defaultFileName">默认的文件名.指定目录名时，将其用作初始目录.</param>
        /// <returns>文件对话框对象，包含用户选择的文件名.</returns>
        /// <exception cref="ArgumentNullException">服务不能为空.</exception>
        /// <exception cref="ArgumentNullException">文件类型不能为空.</exception>
        public static FileDialogResult ShowOpenFileDialog(this IFileDialogService service, FileType fileType, string defaultFileName)
        {
            if (service == null) { throw new ArgumentNullException("service"); }
            if (fileType == null) { throw new ArgumentNullException("fileType"); }
            return service.ShowOpenFileDialog(null, new[] { fileType }, fileType, defaultFileName);
        }

        /// <summary>
        /// 显示“打开文件”对话框，该对话框允许用户指定应该打开的文件.
        /// </summary>
        /// <param name="service">文件对话服务.</param>
        /// <param name="owner">拥有这个OpenFileDialog的窗口.</param>
        /// <param name="fileType">支持的文件类型.</param>
        /// <param name="defaultFileName">默认的文件名。指定目录名时，将其用作初始目录.</param>
        /// <returns>文件对话框对象，包含用户选择的文件名.</returns>
        /// <exception cref="ArgumentNullException">服务不能为空.</exception>
        /// <exception cref="ArgumentNullException">文件类型不能为空.</exception>
        public static FileDialogResult ShowOpenFileDialog(this IFileDialogService service, object owner, FileType fileType, string defaultFileName)
        {
            if (service == null) { throw new ArgumentNullException("service"); }
            if (fileType == null) { throw new ArgumentNullException("fileType"); }
            return service.ShowOpenFileDialog(owner, new[] { fileType }, fileType, defaultFileName);
        }

        /// <summary>
        /// 显示“打开文件”对话框，该对话框允许用户指定应该打开的文件.
        /// </summary>
        /// <param name="service">文件对话服务.</param>
        /// <param name="fileTypes">支持的文件类型.</param>
        /// <returns>文件对话框对象，包含用户选择的文件名.</returns>
        /// <exception cref="ArgumentNullException">服务不能为空.</exception>
        /// <exception cref="ArgumentNullException">文件类型不能为空.</exception>
        /// <exception cref="ArgumentException">文件类型必须至少包含一个项.</exception>
        public static FileDialogResult ShowOpenFileDialog(this IFileDialogService service, IEnumerable<FileType> fileTypes)
        {
            if (service == null) { throw new ArgumentNullException("service"); }
            return service.ShowOpenFileDialog(null, fileTypes, null, null);
        }

        /// <summary>
        /// 显示“打开文件”对话框，该对话框允许用户指定应该打开的文件.
        /// </summary>
        /// <param name="service">文件对话服务.</param>
        /// <param name="owner">拥有这个OpenFileDialog的窗口.</param>
        /// <param name="fileTypes">支持的文件类型.</param>
        /// <returns>文件对话框对象，包含用户选择的文件名.</returns>
        /// <exception cref="ArgumentNullException">服务不能为空.</exception>
        /// <exception cref="ArgumentNullException">文件类型不能为空.</exception>
        /// <exception cref="ArgumentException">文件类型必须至少包含一个项.</exception>
        public static FileDialogResult ShowOpenFileDialog(this IFileDialogService service, object owner, IEnumerable<FileType> fileTypes)
        {
            if (service == null) { throw new ArgumentNullException("service"); }
            return service.ShowOpenFileDialog(owner, fileTypes, null, null);
        }

        /// <summary>
        /// 显示“打开文件”对话框，该对话框允许用户指定应该打开的文件.
        /// </summary>
        /// <param name="service">文件对话服务.</param>
        /// <param name="fileTypes">支持的文件类型.</param>
        /// <param name="defaultFileType">默认文件类型.</param>
        /// <param name="defaultFileName">默认的文件名.指定目录名时，将其用作初始目录.</param>
        /// <returns>文件对话框对象，包含用户选择的文件名.</returns>
        /// <exception cref="ArgumentNullException">服务不能为空.</exception>
        /// <exception cref="ArgumentNullException">文件类型不能为空.</exception>
        /// <exception cref="ArgumentException">文件类型必须至少包含一个项.</exception>
        public static FileDialogResult ShowOpenFileDialog(this IFileDialogService service, IEnumerable<FileType> fileTypes, 
            FileType defaultFileType, string defaultFileName)
        {
            if (service == null) { throw new ArgumentNullException("service"); }
            return service.ShowOpenFileDialog(null, fileTypes, defaultFileType, defaultFileName);
        }

        /// <summary>
        /// 显示“保存文件”对话框，该对话框允许用户指定文件名以将文件保存为.
        /// </summary>
        /// <param name="service">文件对话服务.</param>
        /// <param name="fileType">支持的文件类型.</param>
        /// <returns>文件对话框对象，包含用户输入的文件名.</returns>
        /// <exception cref="ArgumentNullException">服务不能为空.</exception>
        /// <exception cref="ArgumentNullException">文件类型不能为空.</exception>
        public static FileDialogResult ShowSaveFileDialog(this IFileDialogService service, FileType fileType)
        {
            if (service == null) { throw new ArgumentNullException("service"); }
            if (fileType == null) { throw new ArgumentNullException("fileType"); }
            return service.ShowSaveFileDialog(null, new[] { fileType }, fileType, null);
        }

        /// <summary>
        /// 显示“保存文件”对话框，该对话框允许用户指定文件名以将文件保存为.
        /// </summary>
        /// <param name="service">文件对话服务.</param>
        /// <param name="owner">拥有这个SaveFileDialog的窗口.</param>
        /// <param name="fileType">支持的文件类型.</param>
        /// <returns>文件对话框对象，包含用户输入的文件名.</returns>
        /// <exception cref="ArgumentNullException">服务不能为空.</exception>
        /// <exception cref="ArgumentNullException">文件类型不能为空.</exception>
        public static FileDialogResult ShowSaveFileDialog(this IFileDialogService service, object owner, FileType fileType)
        {
            if (service == null) { throw new ArgumentNullException("service"); }
            if (fileType == null) { throw new ArgumentNullException("fileType"); }
            return service.ShowSaveFileDialog(owner, new[] { fileType }, fileType, null);
        }

        /// <summary>
        /// 显示“保存文件”对话框，该对话框允许用户指定文件名以将文件保存为.
        /// </summary>
        /// <param name="service">文件对话服务.</param>
        /// <param name="fileType">支持的文件类型.</param>
        /// <param name="defaultFileName">默认的文件名。指定目录名时，将其用作初始目录.</param>
        /// <returns>文件对话框对象，包含用户输入的文件名.</returns>
        /// <exception cref="ArgumentNullException">服务不能为空.</exception>
        /// <exception cref="ArgumentNullException">文件类型不能为空.</exception>
        public static FileDialogResult ShowSaveFileDialog(this IFileDialogService service, FileType fileType, string defaultFileName)
        {
            if (service == null) { throw new ArgumentNullException("service"); }
            if (fileType == null) { throw new ArgumentNullException("fileType"); }
            return service.ShowSaveFileDialog(null, new[] { fileType }, fileType, defaultFileName);
        }

        /// <summary>
        /// 显示“保存文件”对话框，该对话框允许用户指定文件名以将文件保存为.
        /// </summary>
        /// <param name="service">文件对话服务.</param>
        /// <param name="owner">拥有这个SaveFileDialog的窗口.</param>
        /// <param name="fileType">支持的文件类型.</param>
        /// <param name="defaultFileName">默认的文件名。指定目录名时，将其用作初始目录.</param>
        /// <returns>文件对话框对象，包含用户输入的文件名.</returns>
        /// <exception cref="ArgumentNullException">服务不能为空.</exception>
        /// <exception cref="ArgumentNullException">文件类型不能为空.</exception>
        public static FileDialogResult ShowSaveFileDialog(this IFileDialogService service, object owner, FileType fileType, string defaultFileName)
        {
            if (service == null) { throw new ArgumentNullException("service"); }
            if (fileType == null) { throw new ArgumentNullException("fileType"); }
            return service.ShowSaveFileDialog(owner, new[] { fileType }, fileType, defaultFileName);
        }

        /// <summary>
        /// 显示“保存文件”对话框，该对话框允许用户指定文件名以将文件保存为.
        /// </summary>
        /// <param name="service">文件对话服务.</param>
        /// <param name="fileTypes">支持的文件类型.</param>
        /// <returns>文件对话框对象，包含用户输入的文件名.</returns>
        /// <exception cref="ArgumentNullException">服务不能为空.</exception>
        /// <exception cref="ArgumentNullException">文件类型不能为空.</exception>
        /// <exception cref="ArgumentException">文件类型必须至少包含一个项.</exception>
        public static FileDialogResult ShowSaveFileDialog(this IFileDialogService service, IEnumerable<FileType> fileTypes)
        {
            if (service == null) { throw new ArgumentNullException("service"); }
            return service.ShowSaveFileDialog(null, fileTypes, null, null);
        }

        /// <summary>
        /// 显示“保存文件”对话框，该对话框允许用户指定文件名以将文件保存为.
        /// </summary>
        /// <param name="service">文件对话服务.</param>
        /// <param name="owner">拥有这个SaveFileDialog的窗口.</param>
        /// <param name="fileTypes">支持的文件类型.</param>
        /// <returns>文件对话框对象，包含用户输入的文件名.</returns>
        /// <exception cref="ArgumentNullException">服务不能为空.</exception>
        /// <exception cref="ArgumentNullException">文件类型不能为空.</exception>
        /// <exception cref="ArgumentException">文件类型必须至少包含一个项.</exception>
        public static FileDialogResult ShowSaveFileDialog(this IFileDialogService service, object owner, IEnumerable<FileType> fileTypes)
        {
            if (service == null) { throw new ArgumentNullException("service"); }
            return service.ShowSaveFileDialog(owner, fileTypes, null, null);
        }

        /// <summary>
        /// 如何保存文件对话框，该对话框允许用户指定文件名以将文件保存为.
        /// </summary>
        /// <param name="service">文件对话服务.</param>
        /// <param name="fileTypes">支持的文件类型.</param>
        /// <param name="defaultFileType">默认文件类型.</param>
        /// <param name="defaultFileName">默认的文件名.指定目录名时，将其用作初始目录.</param>
        /// <returns>文件对话框对象，包含用户输入的文件名.</returns>
        /// <exception cref="ArgumentNullException">服务不能为空.</exception>
        /// <exception cref="ArgumentNullException">文件类型不能为空.</exception>
        /// <exception cref="ArgumentException">文件类型必须至少包含一个项.</exception>
        public static FileDialogResult ShowSaveFileDialog(this IFileDialogService service, IEnumerable<FileType> fileTypes, 
            FileType defaultFileType, string defaultFileName)
        {
            if (service == null) { throw new ArgumentNullException("service"); }
            return service.ShowSaveFileDialog(null, fileTypes, defaultFileType, defaultFileName);
        }
    }
}

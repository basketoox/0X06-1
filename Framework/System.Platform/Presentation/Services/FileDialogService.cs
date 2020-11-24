using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Platform.Applications.Services;
using System.Windows;
using Microsoft.Win32;

namespace System.Platform.Presentation.Services
{
    /// <summary>
    /// 这是<see cref="IFileDialogService"/>接口的默认实现。它显示一个打开或保存文件对话框.
    /// </summary>
    /// <remarks>
    /// 如果这个服务的默认实现不能满足您的需求，那么您可以提供自己的实现.
    /// </remarks>
    [Export(typeof(IFileDialogService))]
    public class FileDialogService : IFileDialogService
    {
        /// <summary>
        /// 显示“打开文件”对话框，该对话框允许用户指定应该打开的文件.
        /// </summary>
        /// <param name="owner">拥有这个OpenFileDialog的窗口.</param>
        /// <param name="fileTypes">支持的文件类型.</param>
        /// <param name="defaultFileType">默认文件类型.</param>
        /// <param name="defaultFileName">默认的文件名.指定目录名时，将其用作初始目录.</param>
        /// <returns>文件对话框对象，包含用户选择的文件名.</returns>
        /// <exception cref="ArgumentNullException">文件类型不能为空.</exception>
        /// <exception cref="ArgumentException">文件类型必须至少包含一个项.</exception>
        public FileDialogResult ShowOpenFileDialog(object owner, IEnumerable<FileType> fileTypes, FileType defaultFileType, string defaultFileName)
        {
            if (fileTypes == null) { throw new ArgumentNullException("fileTypes"); }
            if (!fileTypes.Any()) { throw new ArgumentException("The fileTypes collection must contain at least one item."); }

            var dialog = new OpenFileDialog();

            return ShowFileDialog(owner, dialog, fileTypes, defaultFileType, defaultFileName);
        }

        /// <summary>
        /// 显示“保存文件”对话框，该对话框允许用户指定文件名以将文件保存为.
        /// </summary>
        /// <param name="owner">拥有这个SaveFileDialog的窗口.</param>
        /// <param name="fileTypes">支持的文件类型.</param>
        /// <param name="defaultFileType">默认文件类型.</param>
        /// <param name="defaultFileName">默认的文件名.指定目录名时，将其用作初始目录.</param>
        /// <returns>FileDialogResult对象，包含用户输入的文件名.</returns>
        /// <exception cref="ArgumentNullException">文件类型不能为空.</exception>
        /// <exception cref="ArgumentException">文件类型必须至少包含一个项.</exception>
        public FileDialogResult ShowSaveFileDialog(object owner, IEnumerable<FileType> fileTypes, FileType defaultFileType, string defaultFileName)
        {
            if (fileTypes == null) { throw new ArgumentNullException("fileTypes"); }
            if (!fileTypes.Any()) { throw new ArgumentException("The fileTypes collection must contain at least one item."); }

            var dialog = new SaveFileDialog();

            return ShowFileDialog(owner, dialog, fileTypes, defaultFileType, defaultFileName);
        }

        private static FileDialogResult ShowFileDialog(object owner, FileDialog dialog, IEnumerable<FileType> fileTypes,
            FileType defaultFileType, string defaultFileName)
        {
            var filterIndex = fileTypes.ToList().IndexOf(defaultFileType);
            if (filterIndex >= 0) { dialog.FilterIndex = filterIndex + 1; }
            if (!string.IsNullOrEmpty(defaultFileName))
            {
                dialog.FileName = Path.GetFileName(defaultFileName);
                var directory = Path.GetDirectoryName(defaultFileName);
                if (!string.IsNullOrEmpty(directory))
                {
                    dialog.InitialDirectory = directory;
                }
            }

            dialog.Filter = CreateFilter(fileTypes);
            if (dialog.ShowDialog(owner as Window) == true)
            {
                filterIndex = dialog.FilterIndex - 1;
                if (filterIndex >= 0 && filterIndex < fileTypes.Count())
                {
                    defaultFileType = fileTypes.ElementAt(filterIndex);
                }
                else
                {
                    defaultFileType = null;
                }
                return new FileDialogResult(dialog.FileName, defaultFileType);
            }
            return new FileDialogResult();
        }

        private static string CreateFilter(IEnumerable<FileType> fileTypes)
        {
            var builder = new Text.StringBuilder();

            foreach (var fileType in fileTypes)
            {
                if (builder.Length > 0) { builder.Append("|"); }
                builder.Append(fileType.Description + "|*" + fileType.FileExtension);
            }
            return builder.ToString();
        }
    }
}

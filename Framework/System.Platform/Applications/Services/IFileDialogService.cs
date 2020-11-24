using System.Collections.Generic;

namespace System.Platform.Applications.Services
{
    /// <summary>
    /// 此服务允许用户指定要打开或保存文件的文件名.
    /// </summary>
    /// <remarks>
    /// 这个界面是为了简单而设计的。如果您必须完成更高级的场景，
    /// 那么我们建议实现您自己的特定文件对话服务.
    /// </remarks>
    public interface IFileDialogService
    {
        /// <summary>
        /// 显示“打开文件”对话框，该对话框允许用户指定应该打开的文件.
        /// </summary>
        /// <param name="owner">拥有这个OpenFileDialog的窗口.</param>
        /// <param name="fileTypes">支持的文件类型.</param>
        /// <param name="defaultFileType">默认文件类型.</param>
        /// <param name="defaultFileName">默认的文件名。指定目录名时，将其用作初始目录.</param>
        /// <returns>文件对话框对象，包含用户选择的文件名.</returns>
        /// <exception cref="ArgumentNullException">文件类型不能为空.</exception>
        /// <exception cref="ArgumentException">文件类型必须至少包含一个项.</exception>
        FileDialogResult ShowOpenFileDialog(object owner, IEnumerable<FileType> fileTypes, FileType defaultFileType, string defaultFileName);

        /// <summary>
        /// 显示“保存文件”对话框，该对话框允许用户指定文件名以将文件保存为.
        /// </summary>
        /// <param name="owner">拥有这个SaveFileDialog的窗口.</param>
        /// <param name="fileTypes">支持的文件类型.</param>
        /// <param name="defaultFileType">默认文件类型.</param>
        /// <param name="defaultFileName">默认的文件名。指定目录名时，将其用作初始目录.</param>
        /// <returns>文件对话框对象，包含用户输入的文件名.</returns>
        /// <exception cref="ArgumentNullException">文件类型不能为空.</exception>
        /// <exception cref="ArgumentException">文件类型必须至少包含一个项.</exception>
        FileDialogResult ShowSaveFileDialog(object owner, IEnumerable<FileType> fileTypes, FileType defaultFileType, string defaultFileName);
    }
}

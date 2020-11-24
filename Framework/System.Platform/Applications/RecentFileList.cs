using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Platform.Applications
{
    /// <summary>
    /// 该类封装了最近文件列表的逻辑.
    /// </summary>
    /// <remarks>
    ///该类可用于设置文件中，以存储和加载作为用户设置的最新文件列表。在Visual Studio中，您可能需要
    ///输入完整的类名“System.Waf.Applications”。“选择一个类型”对话框中的RecentFileList.
    /// </remarks>
    public sealed class RecentFileList : IXmlSerializable
    {
        private readonly ObservableCollection<RecentFile> _recentFiles;
        private readonly ReadOnlyObservableCollection<RecentFile> _readOnlyRecentFiles;
        private int _maxFilesNumber = 8;


        /// <summary>
        /// 初始化<see cref="RecentFileList"/>类的新实例.
        /// </summary>
        public RecentFileList()
        {
            _recentFiles = new ObservableCollection<RecentFile>();
            _readOnlyRecentFiles = new ReadOnlyObservableCollection<RecentFile>(_recentFiles);
        }


        /// <summary>
        /// 获取最近文件的列表.
        /// </summary>
        public ReadOnlyObservableCollection<RecentFile> RecentFiles { get { return _readOnlyRecentFiles; } }

        /// <summary>
        /// 获取或设置列表中最近文件的最大数量.
        /// </summary>
        /// <remarks>
        /// 如果集合编号低于列表计数，则将最近的文件列表截断为该编号.
        /// </remarks>
        /// <exception cref="ArgumentException">该值必须等于或大于1.</exception>
        public int MaxFilesNumber
        {
            get { return _maxFilesNumber; }
            set
            {
                if (_maxFilesNumber != value)
                {
                    if (value <= 0) { throw new ArgumentException("The value must be equal or larger than 1."); }

                    _maxFilesNumber = value;

                    if (_recentFiles.Count - _maxFilesNumber >= 1)
                    {
                        RemoveRange(_maxFilesNumber, _recentFiles.Count - _maxFilesNumber);
                    }
                }
            }
        }

        private int PinCount { get { return _recentFiles.Count(r => r.IsPinned); } }


        /// <summary>
        /// 将指定的集合加载到最近的文件列表中。当需要手动初始化RecentFileList时，使用此方法。
        /// 当您使用自己的持久性实现时，这可能很有用.
        /// </summary>
        /// <remarks>删除在调用Load方法之前存在的最近的文件项.</remarks>
        /// <param name="recentFiles">最近的文件.</param>
        /// <exception cref="ArgumentNullException">recentFiles参数不能为空.</exception>
        public void Load(IEnumerable<RecentFile> recentFiles)
        {
            if (recentFiles == null) { throw new ArgumentNullException("recentFiles"); }

            Clear();
            AddRange(recentFiles.Take(_maxFilesNumber));
        }

        /// <summary>
        /// 将文件添加到最近的文件列表中。如果文件已经存在于列表中，那么它只会改变列表中的位置.
        /// </summary>
        /// <param name="fileName">最近文件的路径.</param>
        /// <exception cref="ArgumentException">参数文件名不能为空或空.</exception>
        public void AddFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) { throw new ArgumentException("The argument fileName must not be null or empty."); }

            var recentFile = _recentFiles.FirstOrDefault(r => r.Path == fileName);

            if (recentFile != null)
            {
                var oldIndex = _recentFiles.IndexOf(recentFile);
                var newIndex = recentFile.IsPinned ? 0 : PinCount;
                if (oldIndex != newIndex)
                {
                    _recentFiles.Move(oldIndex, newIndex);
                }
            }
            else
            {
                if (PinCount < _maxFilesNumber)
                {
                    if (_recentFiles.Count >= _maxFilesNumber)
                    {
                        RemoveAt(_recentFiles.Count - 1);
                    }
                    Insert(PinCount, new RecentFile(fileName));
                }
            }
        }

        /// <summary>
        /// 删除指定的最近文件.
        /// </summary>
        /// <param name="recentFile">最近要删除的文件.</param>
        /// <exception cref="ArgumentNullException">最近文件参数不能为空.</exception>
        /// <exception cref="ArgumentException">参数recentFile在最近文件列表中没有找到.</exception>
        public void Remove(RecentFile recentFile)
        {
            if (recentFile == null) { throw new ArgumentNullException("recentFile"); }
            if (_recentFiles.Remove(recentFile))
            {
                recentFile.PropertyChanged -= RecentFilePropertyChanged;
            }
            else
            {
                throw new ArgumentException("The passed recentFile was not found in the recent files list.");
            }
        }

        XmlSchema IXmlSerializable.GetSchema() { return null; }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            if (reader == null) { throw new ArgumentNullException("reader"); }

            reader.ReadToDescendant("RecentFile");
            while (reader.MoveToContent() == XmlNodeType.Element && reader.LocalName == "RecentFile")
            {
                var recentFile = new RecentFile();
                ((IXmlSerializable)recentFile).ReadXml(reader);
                Add(recentFile);
            }
            if (!reader.IsEmptyElement) { reader.ReadEndElement(); }
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            if (writer == null) { throw new ArgumentNullException("writer"); }

            foreach (var recentFile in _recentFiles)
            {
                writer.WriteStartElement("RecentFile");
                ((IXmlSerializable)recentFile).WriteXml(writer);
                writer.WriteEndElement();
            }
        }

        private void Insert(int index, RecentFile recentFile)
        {
            recentFile.PropertyChanged += RecentFilePropertyChanged;
            _recentFiles.Insert(index, recentFile);
        }

        private void Add(RecentFile recentFile)
        {
            recentFile.PropertyChanged += RecentFilePropertyChanged;
            _recentFiles.Add(recentFile);
        }

        private void AddRange(IEnumerable<RecentFile> recentFilesToAdd)
        {
            foreach (var recentFile in recentFilesToAdd)
            {
                Add(recentFile);
            }
        }

        private void RemoveAt(int index)
        {
            _recentFiles[index].PropertyChanged -= RecentFilePropertyChanged;
            _recentFiles.RemoveAt(index);
        }

        private void RemoveRange(int index, int count)
        {
            for (var i = 0; i < count; i++)
            {
                RemoveAt(index);
            }
        }

        private void Clear()
        {
            foreach (var recentFile in _recentFiles)
            {
                recentFile.PropertyChanged -= RecentFilePropertyChanged;
            }
            _recentFiles.Clear();
        }

        private void RecentFilePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsPinned")
            {
                var recentFile = (RecentFile)sender;
                var oldIndex = _recentFiles.IndexOf(recentFile);
                if (recentFile.IsPinned)
                {
                    _recentFiles.Move(oldIndex, 0);
                }
                else
                {
                    var newIndex = PinCount;
                    if (oldIndex != newIndex)
                    {
                        _recentFiles.Move(oldIndex, newIndex);
                    }
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPKViewerProject
{
    /// <summary>
    /// Handles information about a successfully loaded WPK.
    /// </summary>
    public class AssetInfo
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public Type Type { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="AssetInfo" /> is compressed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if compressed; otherwise, <c>false</c>.
        /// </value>
        public bool Compressed { get; set; }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName { get; set; }
    }
}

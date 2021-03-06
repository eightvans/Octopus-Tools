﻿using System;

namespace OctopusTools.Client
{
    /// <summary>
    /// In the same way web browsers can follow links like "/api/foo" by knowing the URI of the current page, this class allows application 
    /// links to be resolved into fully-qualified URI's. This implementation also supports virtual directories. It assumes that the 
    /// API endpoint starts with <c>/api</c>.
    /// </summary>
    public class DefaultLinkResolver : ILinkResolver
    {
        readonly string root;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultLinkResolver"/> class.
        /// </summary>
        /// <param name="root">The root URI of the server.</param>
        /// <param name="allUrisStartWith">A segment that users might or might not include when entering the root URI. If the segment exists, it will be ignored.</param>
        public DefaultLinkResolver(Uri root, string allUrisStartWith = "/api")
        {
            allUrisStartWith = (allUrisStartWith.EndsWith("/") ? allUrisStartWith : allUrisStartWith + "/");
            var rootUri = root.ToString();
            rootUri = (rootUri.EndsWith("/") ? rootUri : rootUri + "/");

            var indexOfMandatorySegment = rootUri.LastIndexOf(allUrisStartWith, StringComparison.OrdinalIgnoreCase);
            if (indexOfMandatorySegment >= 1)
            {
                rootUri = rootUri.Substring(0, indexOfMandatorySegment);
            }

            this.root = rootUri.TrimEnd('/');
        }

        /// <summary>
        /// Resolves the specified link into a fully qualified URI.
        /// </summary>
        /// <param name="link">The application relative link (should begin with a <c>/</c>).</param>
        /// <returns>
        /// The fully resolved URI.
        /// </returns>
        public Uri Resolve(string link)
        {
            if (!link.StartsWith("/")) link = '/' + link;
            return new Uri(root + link);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents the root URI that URI's are resolved from.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return root;
        }
    }
}
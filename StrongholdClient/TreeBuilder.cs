/// TreeBuilder.cs
/// Thomas Kempton 2012
///

namespace StrongholdClient
{
    using System.IO;
    using System.Text;
    using System.Windows.Forms;
    using System.Xml;
    using StrongholdClient.FileStronghold;
    using System.Collections.Generic;
    using System.Drawing;

    /// <summary>
    /// Helper class for building a tree view.
    /// </summary>
    public static class TreeBuilder
    {
        /// <summary>
        /// Builds the tree view.
        /// </summary>
        /// <param name="soap">The SOAP response.</param>
        public static TreeNode BuildTreeView(DirectoryListing directory)
        {
            var tree = new TreeNode("root");

            AddTreeNode(tree, directory);

            return tree.Nodes[0];
        }

        /// <summary>
        /// Adds the tree node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="dir">The dir.</param>
        private static void AddTreeNode(TreeNode node, DirectoryListing dir)
        {
            if (dir.IsDirectory)
            {
                var n = node.Nodes.Add(dir.Name);
                n.ForeColor = Color.Blue;
                foreach (var child in dir.SubDirectories)
                {
                    // recursion!
                    AddTreeNode(n, child);
                }
            }
            else
            {
                node.Nodes.Add(dir.Name);
            }
        }

        /// <summary>
        /// Gets the child nodes.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>THe child nodes.</returns>
        public static IEnumerable<TreeNode> GetNodes(this TreeNode node)
        {
            foreach (TreeNode n in node.Nodes)
            {
                yield return n;
            }

            yield break;
        }
    }
}

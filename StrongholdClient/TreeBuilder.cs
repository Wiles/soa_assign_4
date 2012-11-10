namespace StrongholdClient
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;
    using StrongholdClient.FileStronghold;

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
                foreach (var child in dir.SubDirectories.OrderBy(d => d.Name)
                                    .OrderByDescending(d => d.IsDirectory))
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

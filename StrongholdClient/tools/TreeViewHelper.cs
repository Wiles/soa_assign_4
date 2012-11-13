using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace StrongholdClient
{
    class TreeViewHelper
    {
        private readonly string username;
        private readonly TreeView treeDirectory;
        public TreeViewHelper(string username, TreeView treeDirectory)
        {
            this.username = username;
            this.treeDirectory = treeDirectory;
        }

        /// <summary>
        /// Returns whether the treenode is a directory or not
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool IsTreeNodeDirectory(TreeNode node)
        {
            // why, you guys hack like dis...?
            return node.ForeColor == ClientForm.DirectoryForeColor;
        }

        /// <summary>
        /// Is the currently selected path a directory
        /// </summary>
        /// <returns>Is the selected path a directory</returns>
        public bool IsSelectedPathDirectory()
        {
            return IsTreeNodeDirectory(treeDirectory.SelectedNode);
        }


        /// <summary>
        /// Recursively get all the child nodes for a tree node
        /// </summary>
        /// <param name="node">Node to get children of</param>
        /// <returns>All the child nodes for a tree node</returns>
        public List<TreeNode> ListChildNodes(TreeNode node)
        {
            return ListChildNodes(node, new List<TreeNode>());
        }

        /// <summary>
        /// Recursively get all the child nodes for a tree node
        /// </summary>
        /// <param name="node">Node to get children of</param>
        /// <param name="nodes">Mutable list of nodes (this list will be overwritten)</param>
        /// <returns>All the child nodes for a tree node</returns>
        public List<TreeNode> ListChildNodes(TreeNode node, List<TreeNode> nodes)
        {
            foreach (var obj in node.Nodes)
            {
                var innerNode = (TreeNode)obj;
                if (IsTreeNodeDirectory(innerNode))
                {
                    ListChildNodes(innerNode, nodes);
                }
                else
                {
                    nodes.Add(innerNode);
                }
            }

            return nodes;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        public List<string> GetUniqueDirectories(List<string> paths)
        {
            var directories = from f in paths
                              orderby Path.GetDirectoryName(f)
                              select Path.GetDirectoryName(f);

            return directories.Distinct().ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<string> RecursiveListFiles()
        {
            var files = new List<String>();
            var treeNodes = new List<TreeNode>();

            var paths = new List<string>();
            foreach (var node in ListChildNodes(treeDirectory.SelectedNode, treeNodes))
            {
                paths.Add(GetPathForNode(node));
            }

            return paths;
        }

        /// <summary>
        /// Gets the currently selected path.
        /// </summary>
        /// <returns>The path.</returns>
        public string GetPath()
        {
            TreeNode node = treeDirectory.SelectedNode;
            return GetPathForNode(node);
        }

        /// <summary>
        /// Get the relative file path for a node on the file tree
        /// </summary>
        /// <param name="node">Node to get file path for</param>
        /// <returns>File path for node</returns>
        public string GetPathForNode(TreeNode node)
        {
            List<String> directories = new List<String>();
            while (node != null)
            {
                directories.Add(node.Text);
                node = node.Parent;
            }

            if (directories.Count == 0)
            {
                directories.Add(username);
            }

            directories.Reverse();
            StringBuilder baseDir = new StringBuilder();
            foreach (var dir in directories)
            {
                baseDir.Append(dir).Append("\\");
            }

            baseDir.Length -= 1;
            return baseDir.ToString();
        }
    }
}

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
            // Facepalm...
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
                    nodes.Add(innerNode);
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
        /// List the files of the node and all its sub-nodes files
        /// 
        /// Path is relative to passed in path!
        /// </summary>
        /// <param name="root">Node to begin at</param>
        /// <returns>Files of the node and all its sub-nodes files</returns>
        public Dictionary<string, string> RecursiveListFiles(TreeNode root)
        {
            return RecursiveList(root, (node) => !IsTreeNodeDirectory(node), root);
        }

        /// <summary>
        /// List the folders of the node and all its sub-nodes folders.
        /// 
        /// Path is relative to passed in path!
        /// </summary>
        /// <param name="root">Node to begin at</param>
        /// <returns>Folders of the node and all its sub-nodes folders</returns>
        public Dictionary<string, string> RecursiveListFolders(TreeNode root)
        {
            return RecursiveList(root, (node) => IsTreeNodeDirectory(node), root);
        }

        /// <summary>
        /// List the node and all its sub-nodes based on the predicate
        /// </summary>
        /// <param name="search">Node to begin at</param>
        /// <param name="root">Node to begin at</param>
        /// <returns>The node and all its sub-nodes based on the predicate</returns>
        private Dictionary<string, string> RecursiveList(TreeNode search, Func<TreeNode, bool> predicate, TreeNode relative = null)
        {
            var files = new List<String>();
            var treeNodes = new List<TreeNode>();

            var paths = new Dictionary<string, string>();
            foreach (var node in ListChildNodes(search, treeNodes))
            {
                if (predicate(node))
                {
                    var key = GetPathForNode(node);
                    var value = GetRelativePathForNode(relative, node);
                    paths.Add(key, value);
                }
            }

            return paths;
        }

        /// <summary>
        /// Gets the currently selected path.
        /// </summary>
        /// <returns>The path.</returns>
        public string GetSelectedPath()
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

        /// <summary>
        /// Get the relative file path for a node based on the relative node
        /// </summary>
        /// <param name="relative">Node to base path off of</param>
        /// <param name="node">Node to get file path for</param>
        /// <returns>File path for node</returns>
        public string GetRelativePathForNode(TreeNode relative, TreeNode node)
        {
            List<String> directories = new List<String>();
            while (node != relative)
            {
                directories.Add(node.Text);
                node = node.Parent;
            }

            directories.Add(relative.Text);

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

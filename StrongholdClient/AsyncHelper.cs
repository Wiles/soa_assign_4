namespace StrongholdClient
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    /// <summary>
    /// Extension methods to ease asynchronous forms.
    /// </summary>
    public static class AsyncHelper
    {
        /// <summary>
        /// Invokes an action on the UI thread.
        /// </summary>
        /// <typeparam name="TControl">The type of the control.</typeparam>
        /// <param name="control">The control.</param>
        /// <param name="action">The action.</param>
        public static void InvokeOnUI<TControl>(
                this TControl control,
                Action action) where TControl : Control
        {
            control.Invoke(new MethodInvoker(delegate { action.Invoke(); }));
        }
        
        /// <summary>
        /// Invokes an action asynchronously.
        /// </summary>
        /// <typeparam name="TControl">The type of the control.</typeparam>
        /// <param name="control">The control.</param>
        /// <param name="action">The action.</param>
        public static void InvokeAsync<TControl>(
                this TControl control,
                Action action) where TControl : Control
        {
            // BackgroundWorker doesn't actually need to
            // be disposed, because of its implementation
            var bg = new BackgroundWorker();
            bg.DoWork += new DoWorkEventHandler((source, evt) => action());
            bg.RunWorkerAsync();
        }

        /// <summary>
        /// Sets the in middle.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="child">The child.</param>
        public static void SetInMiddle(this Form parent, Form child)
        {
            var coff = child.Width / 2;
        }
    }
}

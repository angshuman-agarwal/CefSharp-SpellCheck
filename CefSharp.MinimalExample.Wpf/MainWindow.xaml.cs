using CefSharp.Wpf;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;

namespace CefSharp.MinimalExample.Wpf
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Browser.IsBrowserInitializedChanged += Browser_IsBrowserInitializedChanged;
        }

        private void Browser_IsBrowserInitializedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Cef.UIThreadTaskFactory.StartNew(delegate
            {
                var cwb = (sender as ChromiumWebBrowser);
                var rc = cwb.GetBrowserHost().RequestContext;

                string error;
                var dicts = new List<string> { "en-GB", "en-US" };

                // returns false always with error = Trying to set a preference of type LIST to value of type NULL
                // if you hrdcode the second parameter as true, then error message will be:
                // Trying to set a preference of type LIST to value of type BOOLEAN

                // So, issue is that the Cef SetPreference here expects a LIST but C# LIST
                // is not getting translated to CEF compatible datatype.


                // It is an array, even doing ToArray() does not make any difference
                // https://cs.chromium.org/chromium/src/chrome/browser/resources/settings/languages_page/languages.js?type=cs&q=spellcheck.dictionaries&g=0&l=342
                var success = rc.SetPreference("spellcheck.dictionaries", dicts, out error);
                if (!success)
                {
                    Debug.WriteLine($"spellcheck.dictionaries : {error}");
                }
            }
            );
        }
    }
}

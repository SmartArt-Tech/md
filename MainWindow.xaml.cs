using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using Microsoft.Win32;
using System.Text.RegularExpressions;

namespace MarkdownViewer
{
    public partial class MainWindow : Window
    {
        private string currentFilePath = "";
        
        public MainWindow()
        {
            InitializeComponent();
            InitializeAsync();
            
            // Add keyboard shortcuts
            this.KeyDown += MainWindow_KeyDown;
        }

        private void InitializeAsync()
        {
            // Check if a file was passed as command line argument
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1 && File.Exists(args[1]) && args[1].EndsWith(".md", StringComparison.OrdinalIgnoreCase))
            {
                OpenMarkdownFile(args[1]);
            }
            else
            {
                ShowWelcomeMessage();
            }
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.O && Keyboard.Modifiers == ModifierKeys.Control)
            {
                OpenFile_Click(sender, null);
            }
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Markdown files (*.md)|*.md|All files (*.*)|*.*",
                FilterIndex = 1
            };

            if (openFileDialog.ShowDialog() == true)
            {
                OpenMarkdownFile(openFileDialog.FileName);
            }
        }

        private void OpenMarkdownFile(string filePath)
        {
            try
            {
                string markdownContent = File.ReadAllText(filePath);
                string htmlContent = ConvertMarkdownToHtml(markdownContent);
                
                webBrowser.NavigateToString(htmlContent);
                
                currentFilePath = filePath;
                filePathText.Text = filePath;
                statusText.Text = $"Loaded: {Path.GetFileName(filePath)}";
                Title = $"Markdown Viewer - {Path.GetFileName(filePath)}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening file: {ex.Message}", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
                statusText.Text = "Error loading file";
            }
        }

        private string ConvertMarkdownToHtml(string markdown)
        {
            string html = markdown;
            
            // Convert headers
            html = Regex.Replace(html, @"^# (.+)$", "<h1>$1</h1>", RegexOptions.Multiline);
            html = Regex.Replace(html, @"^## (.+)$", "<h2>$1</h2>", RegexOptions.Multiline);
            html = Regex.Replace(html, @"^### (.+)$", "<h3>$1</h3>", RegexOptions.Multiline);
            html = Regex.Replace(html, @"^#### (.+)$", "<h4>$1</h4>", RegexOptions.Multiline);
            html = Regex.Replace(html, @"^##### (.+)$", "<h5>$1</h5>", RegexOptions.Multiline);
            html = Regex.Replace(html, @"^###### (.+)$", "<h6>$1</h6>", RegexOptions.Multiline);
            
            // Convert bold and italic
            html = Regex.Replace(html, @"\*\*(.+?)\*\*", "<strong>$1</strong>");
            html = Regex.Replace(html, @"\*(.+?)\*", "<em>$1</em>");
            html = Regex.Replace(html, @"__(.+?)__", "<strong>$1</strong>");
            html = Regex.Replace(html, @"_(.+?)_", "<em>$1</em>");
            
            // Convert links
            html = Regex.Replace(html, @"\[(.+?)\]\((.+?)\)", "<a href=\"$2\">$1</a>");
            
            // Convert code blocks
            html = Regex.Replace(html, @"```(.+?)```", "<pre><code>$1</code></pre>", RegexOptions.Singleline);
            html = Regex.Replace(html, @"`(.+?)`", "<code>$1</code>");
            
            // Convert lists
            html = Regex.Replace(html, @"^- (.+)$", "<li>$1</li>", RegexOptions.Multiline);
            html = Regex.Replace(html, @"^(\d+)\. (.+)$", "<li>$2</li>", RegexOptions.Multiline);
            
            // Convert blockquotes
            html = Regex.Replace(html, @"^> (.+)$", "<blockquote>$1</blockquote>", RegexOptions.Multiline);
            
            // Convert line breaks
            html = html.Replace("\n\n", "</p><p>").Replace("\n", "<br>");
            
            // Wrap in paragraphs if not already wrapped
            if (!html.StartsWith("<") && !string.IsNullOrWhiteSpace(html))
            {
                html = "<p>" + html + "</p>";
            }
            
            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <title>Markdown Preview</title>
    <style>
        body {{
            font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Helvetica, Arial, sans-serif;
            line-height: 1.6;
            max-width: 800px;
            margin: 0 auto;
            padding: 20px;
            color: #24292e;
        }}
        h1, h2, h3, h4, h5, h6 {{
            margin-top: 24px;
            margin-bottom: 16px;
            font-weight: 600;
            line-height: 1.25;
        }}
        h1 {{ border-bottom: 1px solid #eaecef; padding-bottom: 10px; }}
        h2 {{ border-bottom: 1px solid #eaecef; padding-bottom: 8px; }}
        code {{
            background-color: rgba(27,31,35,.05);
            border-radius: 3px;
            padding: 2px 4px;
            font-family: 'SFMono-Regular', Consolas, 'Liberation Mono', Menlo, monospace;
        }}
        pre {{
            background-color: #f6f8fa;
            border-radius: 6px;
            padding: 16px;
            overflow: auto;
        }}
        blockquote {{
            border-left: 4px solid #dfe2e5;
            padding-left: 16px;
            color: #6a737d;
        }}
        table {{
            border-collapse: collapse;
            width: 100%;
        }}
        table th, table td {{
            border: 1px solid #dfe2e5;
            padding: 6px 13px;
        }}
        table th {{
            background-color: #f6f8fa;
            font-weight: 600;
        }}
        img {{
            max-width: 100%;
            height: auto;
        }}
        a {{
            color: #0366d6;
            text-decoration: none;
        }}
        a:hover {{
            text-decoration: underline;
        }}
    </style>
</head>
<body>
{html}
</body>
</html>";
        }

        private void ShowWelcomeMessage()
        {
            string welcomeHtml = @"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <title>Markdown Viewer</title>
    <style>
        body {
            font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Helvetica, Arial, sans-serif;
            line-height: 1.6;
            max-width: 600px;
            margin: 50px auto;
            padding: 20px;
            color: #24292e;
            text-align: center;
        }
        .welcome {
            background-color: #f6f8fa;
            border-radius: 6px;
            padding: 30px;
            border: 1px solid #e1e4e8;
        }
        .shortcut {
            background-color: #fff;
            border: 1px solid #d1d5da;
            border-radius: 3px;
            padding: 20px;
            margin-top: 20px;
            text-align: left;
        }
        .key {
            background-color: #fafbfc;
            border: 1px solid #d1d5da;
            border-radius: 3px;
            padding: 2px 6px;
            font-family: monospace;
        }
    </style>
</head>
<body>
    <div class='welcome'>
        <h1>📝 Markdown Viewer</h1>
        <p>Welcome to the simple Markdown file viewer!</p>
        <p>Open a Markdown (.md) file to get started.</p>
        
        <div class='shortcut'>
            <h3>How to use:</h3>
            <ul>
                <li>Click <strong>File → Open</strong> or press <span class='key'>Ctrl+O</span></li>
                <li>Drag and drop .md files (when set as default handler)</li>
                <li>Use <strong>Settings → Set as Default MD Viewer</strong> to handle .md files</li>
            </ul>
        </div>
    </div>
</body>
</html>";
            webBrowser.NavigateToString(welcomeHtml);
            statusText.Text = "Ready - Open a Markdown file to begin";
        }

        private void WebBrowser_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            // Allow navigation to external links
            if (!string.IsNullOrEmpty(e.Uri.ToString()) && e.Uri.ToString().StartsWith("http"))
            {
                e.Cancel = true;
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = e.Uri.ToString(),
                    UseShellExecute = true
                });
            }
        }

        private void SetAsDefault_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RegisterAsDefaultHandler();
                MessageBox.Show("Markdown Viewer has been set as the default handler for .md files.", 
                    "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                statusText.Text = "Set as default .md file handler";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error setting as default handler: {ex.Message}\n\nPlease run as administrator.", 
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                statusText.Text = "Failed to set as default handler";
            }
        }

        private void RegisterAsDefaultHandler()
        {
            string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            if (exePath.EndsWith(".dll"))
            {
                exePath = exePath.Replace(".dll", ".exe");
            }

            // Register file association for .md files
            using (RegistryKey key = Registry.ClassesRoot.CreateSubKey(".md"))
            {
                key.SetValue("", "MarkdownViewer.Document");
            }

            using (RegistryKey key = Registry.ClassesRoot.CreateSubKey("MarkdownViewer.Document"))
            {
                key.SetValue("", "Markdown Document");
                
                using (RegistryKey iconKey = key.CreateSubKey("DefaultIcon"))
                {
                    iconKey.SetValue("", $"{exePath},0");
                }
                
                using (RegistryKey commandKey = key.CreateSubKey("shell\\open\\command"))
                {
                    commandKey.SetValue("", $"\"{exePath}\" \"%1\"");
                }
            }
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Markdown Viewer v1.0\n\nA simple Windows application for viewing Markdown files.\n\nBuilt with WPF and custom markdown parser.\n\nDeveloped by SmartArt Tech\n© 2024 SmartArt Tech. All rights reserved.", 
                "About Markdown Viewer", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
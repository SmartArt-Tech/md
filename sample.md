# Markdown Viewer Test

This is a **sample markdown file** to test the Markdown Viewer application.

## Features

- ✅ **File Opening**: Open `.md` files through File menu or command line
- ✅ **HTML Rendering**: Convert markdown to styled HTML
- ✅ **Default Handler**: Set as default application for `.md` files
- ✅ **WebView2**: Modern web rendering engine
- ✅ **Keyboard Shortcuts**: Ctrl+O to open files

## Code Example

```csharp
public void OpenMarkdownFile(string filePath)
{
    string content = File.ReadAllText(filePath);
    string html = ConvertMarkdownToHtml(content);
    webView.NavigateToString(html);
}
```

## Lists

### Ordered List
1. First item
2. Second item
3. Third item

### Unordered List
- Bullet point
- Another point
  - Nested point
  - Another nested point

## Links and Images

Visit [GitHub](https://github.com) for more information.

## Tables

| Feature | Status | Notes |
|---------|--------|-------|
| File Opening | ✅ Complete | Works with drag & drop |
| Markdown Rendering | ✅ Complete | Uses Markdig library |
| Default Handler | ✅ Complete | Registry integration |

## Blockquote

> This is a blockquote example.
> It can span multiple lines and provides
> a nice way to highlight important information.

---

**End of test document**
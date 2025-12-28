# GitHub Auto-Release Setup Instructions

This guide will help you set up automatic releases for the Markdown Viewer application on GitHub.

## Prerequisites

- GitHub account
- Git installed on your machine
- The Markdown Viewer project files

## Step 1: Create GitHub Repository

1. **Create a new repository** on GitHub:
   - Go to [github.com](https://github.com)
   - Click the "+" icon → "New repository"
   - Repository name: `markdown-viewer` (or your preferred name)
   - Description: `Simple Windows Markdown file viewer by SmartArt Tech`
   - Make it **Public** or **Private** (your choice)
   - ✅ Check "Add a README file"
   - Choose license: MIT or your preferred license
   - Click "Create repository"

2. **Clone the repository** to your local machine:
   ```bash
   git clone https://github.com/YOUR_USERNAME/markdown-viewer.git
   cd markdown-viewer
   ```

## Step 2: Add Project Files

1. **Copy all project files** to the cloned repository folder:
   ```
   markdown-viewer/
   ├── .github/
   │   └── workflows/
   │       └── release.yml
   ├── App.xaml
   ├── App.xaml.cs
   ├── MainWindow.xaml
   ├── MainWindow.xaml.cs
   ├── MarkdownViewer.csproj
   ├── sample.md
   ├── app.ico (if you have the icon)
   ├── README.md
   └── GITHUB_SETUP.md
   ```

2. **Commit and push** the files:
   ```bash
   git add .
   git commit -m "Initial commit: Markdown Viewer by SmartArt Tech"
   git push origin main
   ```

## Step 3: Configure Repository Settings

1. **Go to repository Settings**:
   - Navigate to your repository on GitHub
   - Click the "Settings" tab

2. **Configure Actions permissions**:
   - Go to "Actions" → "General" in the left sidebar
   - Under "Actions permissions", select:
     - ✅ "Allow all actions and reusable workflows"
   - Under "Workflow permissions", select:
     - ✅ "Read and write permissions"
     - ✅ "Allow GitHub Actions to create and approve pull requests"
   - Click "Save"

## Step 4: Create Your First Release

### Method 1: Using Git Command Line

1. **Create and push a version tag**:
   ```bash
   git tag v1.0.0
   git push origin v1.0.0
   ```

### Method 2: Using GitHub Web Interface

1. **Go to Releases**:
   - In your repository, click "Releases" (on the right side)
   - Click "Create a new release"

2. **Configure the release**:
   - Tag version: `v1.0.0`
   - Release title: `Markdown Viewer 1.0.0`
   - Description: Add release notes
   - Click "Publish release"

## Step 5: Verify Auto-Release

After creating the tag/release, the GitHub Actions workflow will automatically:

1. ✅ **Build** the application
2. ✅ **Create executables** for Windows x64 and x86
3. ✅ **Package** them into zip files
4. ✅ **Upload** to the GitHub release
5. ✅ **Add release notes** with SmartArt Tech branding

### Monitor the Build Process

1. **Check Actions tab**:
   - Go to the "Actions" tab in your repository
   - You should see a "Release" workflow running
   - Click on it to view progress and logs

2. **Verify release artifacts**:
   - Once complete, go to "Releases"
   - Your release should contain:
     - `MarkdownViewer-v1.0.0-win-x64.zip`
     - `MarkdownViewer-v1.0.0-win-x86.zip`

## Step 6: Future Releases

For future releases, simply create new version tags:

```bash
# For version 1.1.0
git tag v1.1.0
git push origin v1.1.0

# For version 2.0.0
git tag v2.0.0
git push origin v2.0.0
```

The workflow will automatically:
- Build and package the latest code
- Create a new release with proper versioning
- Upload the executables

## Customizing the Release Process

### Update Release Notes

Edit `.github/workflows/release.yml` and modify the `body:` section under "Create Release" to customize:
- Release description
- What's new section
- Installation instructions

### Add More Build Targets

To build for additional platforms, add more publish steps:

```yaml
- name: Publish Windows ARM64
  run: dotnet publish MarkdownViewer.csproj -c Release -r win-arm64 --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=true --output ./publish/win-arm64
```

### Versioning Strategy

Use semantic versioning:
- `v1.0.0` - Major release
- `v1.1.0` - Minor release (new features)
- `v1.0.1` - Patch release (bug fixes)

## Troubleshooting

### Build Fails
1. Check the Actions tab for error logs
2. Ensure .NET 8 compatibility
3. Verify project file syntax

### No Release Created
1. Verify repository permissions (Step 3)
2. Check that tag follows `v*` pattern
3. Ensure workflow file is in `.github/workflows/`

### Missing Files in Release
1. Check file paths in the workflow
2. Verify build output directory
3. Review compress archive commands

## Security Considerations

- The workflow uses minimal permissions
- No secrets are required for basic releases
- All builds run in GitHub's secure environment

---

**Support**: For issues with the release process, check the Actions logs or create an issue in the repository.

**SmartArt Tech** - © 2024 All rights reserved.
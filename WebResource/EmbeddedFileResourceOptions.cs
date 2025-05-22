// This file is part of the WinFormedge project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reflection;


namespace WinFormedge.WebResource;

public sealed class EmbeddedFileResourceOptions : WebResourceOptions
{
    public string? DefaultFolderName { get; init; }
    public string? DefaultNamespace { get; init; }

    public CoreWebView2WebResourceContext WebResourceContext { get; init; } = CoreWebView2WebResourceContext.All;
    public required Assembly ResourceAssembly { get; init; }

}

// This file is part of the WinFormedge project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the MIT License.
// See the LICENSE file in the project root for more information.

namespace WinFormedge.WebResource;

public delegate string WebResourceFallbackDelegate(string requestUrl);
public abstract class WebResourceOptions
{
    public required string Scheme { get; init; } = "http";
    public required string HostName { get; init; }
    public WebResourceFallbackDelegate? OnFallback { get; set; }
}
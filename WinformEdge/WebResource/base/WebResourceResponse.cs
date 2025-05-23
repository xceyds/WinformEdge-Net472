// This file is part of the WinFormedge project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Specialized;

namespace WinFormedge.WebResource;

public sealed class WebResourceResponse : IDisposable
{
    const string DEFAULT_CONTENT_TYPE = "text/plain";
    public int HttpStatus { get; set; } = StatusCodes.Status200OK;

    public Stream? ContentBody { get; set; }

    public string ContentType { 
        get {
            return Headers["Content-Type"]?.ToString() ?? DEFAULT_CONTENT_TYPE;
        }
        set {
            Headers["Content-Type"] = value;
        } 
    }
    public NameValueCollection Headers { get; } = new NameValueCollection();


    public WebResourceResponse(string? contentType = null, byte[]? buff = null)
    {
        if (!string.IsNullOrEmpty(contentType))
        {
            ContentType = contentType;
        }

        if (buff != null)
        {
            ContentBody = new MemoryStream(buff);
        }
    }

    public void Dispose()
    {
        ContentBody?.Close();
        ContentBody?.Dispose();
        ContentBody = null;
        GC.SuppressFinalize(this);
    }

    internal void Content(byte[] buff, string? contentType = null)
    {
        if (!string.IsNullOrEmpty(contentType))
        {
            ContentType = contentType;
        }

        Headers.Set("Content-Type", ContentType);

        if (ContentBody != null)
        {
            ContentBody.Dispose();
            ContentBody = null;
        }

        ContentBody = new MemoryStream(buff);

        HttpStatus = StatusCodes.Status200OK;
    }

    internal void JsonContent(object data, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data, jsonSerializerOptions));

        Content(bytes, "application/json");
    }


    internal void JsonContent<T>(T data, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data, jsonSerializerOptions));

        Content(bytes, "application/json");
    }


    internal void TextContent(string text)
    {
        TextContent(text, Encoding.UTF8);
    }


    internal void TextContent(string text, Encoding encoding)
    {
        Content(text, "text/plain", encoding);
    }


    internal void Content(string content)
    {
        Content(Encoding.UTF8.GetBytes(content), "text/plain");
    }

    internal void Content(string content, string contentType)
    {
        Content(Encoding.UTF8.GetBytes(content), contentType);
    }

    internal void Content(string content, string contentType, Encoding encoding)
    {
        Content(encoding.GetBytes(content), contentType);
    }
}

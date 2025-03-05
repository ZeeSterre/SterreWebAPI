
using NUnit.Framework;
using System;

[Serializable]
public class APIResponse
{
    public bool Success { get; set; } 
    public string Message { get; set; }
    public string Data { get; set; }
    public int StatusCode { get; set; }

    public APIResponse(bool success, string message, string data = null, int statusCode = 0)
    {
        Success = success;
        Message = message;
        Data = data;
        StatusCode = statusCode;
    }
}

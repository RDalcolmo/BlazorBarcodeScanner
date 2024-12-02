using System;
using System.Threading.Tasks;

namespace BlazorZXingJSApp.Shared;

public static class BlazorZXingExceptions
{
    public static Task<Exception> ReaderNotInitializedException =>
        Task.FromResult(new Exception("Reader is not initialized"));
}
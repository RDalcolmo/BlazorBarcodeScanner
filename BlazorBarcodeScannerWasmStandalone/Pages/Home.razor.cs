using BlazorBarcodeScanner.ZXing.Cpp;
using BlazorZXingJSApp.Shared;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorBarcodeScannerWasmStandalone.Pages;

public partial class Home
{
    private BarcodeReader? _reader;
    private int _streamWidth = 720;
    private int _streamHeight = 540;

    private string _localBarcodeText = string.Empty;
    private int _currentVideoSourceIdx;

    private string _imgSrc = string.Empty;
    private string? _lastError = string.Empty;

    protected override void OnAfterRender(bool firstRender)
    {
        base.OnAfterRender(firstRender);

        if (firstRender && !string.IsNullOrWhiteSpace(_reader?.SelectedVideoInputId))
        {
            _currentVideoSourceIdx = SourceIndexFromId();
        }
    }

    private int SourceIndexFromId()
    {
        var inputs = _reader?.VideoInputDevices.ToList() ?? [];
        int result;
        for (result = 0; result < inputs.Count; result++)
        {
            var deviceId = inputs[result].DeviceId;
            if (deviceId != null && deviceId.Equals(_reader?.SelectedVideoInputId))
            {
                break;
            }
        }
        return result;
    }

    private async Task LocalReceivedBarcodeText(BarcodeReceivedEventArgs args)
    {
        _localBarcodeText = args.BarcodeText;
            
        if (_reader != null) 
            await _reader.StopDecoding();
    }

    private void LocalReceivedError(ErrorReceivedEventArgs args)
    {
        _lastError = args.Message;
    }

    private async Task CapturePicture()
    {
        if (_reader != null)
        {
            _imgSrc = await _reader.Capture();
            StateHasChanged();
        }
    }

    private async Task OnVideoSourceNext(MouseEventArgs args)
    {
        var inputs = _reader?.VideoInputDevices.ToList() ?? [];

        if (inputs.Count == 0)
            return;

        _currentVideoSourceIdx++;
        if (_currentVideoSourceIdx >= inputs.Count)
        {
            _currentVideoSourceIdx = 0;
        }

        if (_reader != null) 
            await _reader.SelectVideoInput(inputs[_currentVideoSourceIdx]);
    }

    private Task UpdateResolution() => 
        _reader != null
            ? _reader.UpdateResolution() 
            : BlazorZXingExceptions.ReaderNotInitializedException;

    private Task StartDecoding() => 
        _reader != null
            ? _reader.StartDecoding() 
            : BlazorZXingExceptions.ReaderNotInitializedException;

    private Task StopDecoding()=> 
        _reader != null
            ? _reader.StopDecoding() 
            : BlazorZXingExceptions.ReaderNotInitializedException;

    private Task ToggleTorch() => 
        _reader != null
            ? _reader.ToggleTorch() 
            : BlazorZXingExceptions.ReaderNotInitializedException;

    private Task TorchOn() => 
        _reader != null
            ? _reader.TorchOn() 
            : BlazorZXingExceptions.ReaderNotInitializedException;

    private Task TorchOff() => 
        _reader != null
            ? _reader.TorchOff() 
            : BlazorZXingExceptions.ReaderNotInitializedException;
}
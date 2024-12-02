using BlazorBarcodeScanner.ZXing.JS;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorZXingJSApp.Client.Pages;

public partial class FullWidthVideoExample
{
    private BarcodeReader _reader;
    private const int StreamWidth = 720;
    private const int StreamHeight = 540;

    private string _localBarcodeText;

    protected override void OnAfterRender(bool firstRender)
    {
        base.OnAfterRender(firstRender);

        if (firstRender)
        {
            if (!string.IsNullOrWhiteSpace(_reader.SelectedVideoInputId))
            {
                SourceIndexFromId();
            }
        }
    }

    private void SourceIndexFromId()
    {
        var inputs = _reader.VideoInputDevices.ToList();
        int result;
        for (result = 0; result < inputs.Count; result++)
        {
            if (inputs[result].DeviceId.Equals(_reader.SelectedVideoInputId))
            {
                break;
            }
        }
    }

    private async Task LocalReceivedBarcodeText(BarcodeReceivedEventArgs args)
    {
        await InvokeAsync(async () => {
            _localBarcodeText = args.BarcodeText;
                
            StateHasChanged();
            await _reader.StopDecoding();
        });
    }
}
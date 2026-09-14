using System.Windows.Input;

namespace MauiRenderDemo;

public partial class ButtonPage : ContentPage
{
    public ICommand IconTileCommand { private set; get; }
    public ButtonPage()
	{
        // vor InitializeComponent - dann klappt die Bindung.
        IconTileCommand = new Command(() =>
        {
            System.Diagnostics.Debug.WriteLine("Y0!");
        });


        InitializeComponent();

	}
}
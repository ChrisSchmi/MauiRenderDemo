using System.Windows.Input;

namespace MauiRenderDemo;

public partial class BottomSheetPage : ContentPage
{

	public ICommand ShowBottomSheetCommand { get; private set; }
    public ICommand HideBottomSheetCommand { get; private set; }

    public BottomSheetPage()
	{
		ShowBottomSheetCommand = new Command(() => {
            BottomSheet.Show();
        });

        HideBottomSheetCommand = new Command(() => {
            BottomSheet.Close();
        });

        InitializeComponent();
	}
}
using System.Windows.Input;

namespace MauiRenderDemo;

public partial class FadingBackgroundPage : ContentPage
{
    public ICommand SetBackgroundCommand { private set; get; }

    public FadingBackgroundPage()
	{
        SetBackgroundCommand = new Command<string>(OnSetBackground);

		InitializeComponent();
	}

    private void OnSetBackground(string parameter)
    {
        if(string.IsNullOrEmpty(parameter))
        {
            return;
        }

        var selected = parameter.ToLower();

        switch(selected)
        {
            case "top":
                FadedBackgroundDrawable.VisibleImagePosition = Renderers.VisibleImagePosition.Top;
                break;
            case "bottom":
                FadedBackgroundDrawable.VisibleImagePosition = Renderers.VisibleImagePosition.Bottom;
                break;
            case "full":
                FadedBackgroundDrawable.VisibleImagePosition = Renderers.VisibleImagePosition.Full;
                break;
        }

        FadeView.Invalidate();
    }

}
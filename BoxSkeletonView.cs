namespace TeaStoreApp;

public class BoxSkiletonView : BoxView
{
    public BoxSkiletonView()
    {
        Device.StartTimer(TimeSpan.FromSeconds(1.5), () =>
        {
            this.FadeTo(0.5, 750, Easing.CubicInOut).ContinueWith((x) =>
            {
                this.FadeTo(1, 750, Easing.CubicInOut);
            });

            return true; //Fade In Fade out Animation everytime
        });
    }

}

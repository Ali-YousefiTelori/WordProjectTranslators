namespace Translators.UI.Effects
{
    public class CollectionViewAutoScroll
    {
        public static readonly BindableProperty BindToProperty = BindableProperty.CreateAttached("BindTo", typeof(int), typeof(CollectionViewAutoScroll), 0, propertyChanged: BindToValue);
        static void BindToValue(BindableObject bindable, object oldValue, object newValue)
        {
            bindable.CoerceValue(BindToProperty);
            if (bindable is CollectionView collectionView && newValue is int value && value >= 0)
                collectionView.ScrollTo(value, position: ScrollToPosition.Center);
        }

        public static int GetBindTo(BindableObject target)
        {
            return (int)target.GetValue(BindToProperty);
        }

        public static void SetBindTo(BindableObject target, int value)
        {
            target.SetValue(BindToProperty, value);
        }
    }
}

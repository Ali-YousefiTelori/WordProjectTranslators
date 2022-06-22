using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;

namespace Translators.UI.Droid
{
    [Activity(Label = "Translators", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
            StartForegroundServiceCompat<TranslatorsService>(this);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        protected override void OnDestroy()
        {
            if (TranslatorsService.This != null)
            {
                try
                {
                    TranslatorsService.This.StopForeground(true);
                }
                catch (System.Exception ex)
                {
                }
            }
            base.OnDestroy();
        }

        public static void StartForegroundServiceCompat<T>(Context context, Bundle args = null) where T : Service
        {
            var intent = new Intent(context, typeof(T));
            if (args != null)
            {
                intent.PutExtras(args);
            }

            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                context.StartForegroundService(intent);
            }
            else
            {
                context.StartService(intent);
            }
        }
    }

    [Service(Exported = true)]
    [IntentFilter(new string[] { "noorpod.ir.translators.translatorservice" })]
    public class TranslatorsService : Service
    {
        public static TranslatorsService This { get; set; }
        Notification GetNotification()
        {
            Notification notification = null;
            if ((int)Build.VERSION.SdkInt >= 26)
            {
                var channelId = CreateNotificationChannel("TranslatorsService", "کتب مقدس");
                notification = new Notification.Builder(this, channelId).Build();
            }
            else
            {
                var builder = new Notification.Builder(this).SetContentTitle("کتب مقدس").SetContentText("Translators").SetSmallIcon(Resource.Drawable.icon_add);
                notification = builder.Build();
            }
            return notification;
        }
        private string CreateNotificationChannel(string channelId, string channelName)
        {
            NotificationChannel chan = new NotificationChannel(channelId, channelName, NotificationImportance.High);
            chan.LockscreenVisibility = NotificationVisibility.Private;
            NotificationManager service = GetSystemService(Context.NotificationService) as NotificationManager;
            service.CreateNotificationChannel(chan);
            return channelId;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            This = this;
            try
            {
                Notification notif = GetNotification();
                StartForeground(9000, notif);
            }
            catch (System.Exception ex)
            {

            }
            return StartCommandResult.Sticky;
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }
    }
}
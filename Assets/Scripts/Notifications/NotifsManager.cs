using System.Collections;
using UnityEngine;

#if UNITY_ANDROID
using Unity.Notifications.Android;

public class NotifsManager : MonoBehaviour
{
    private const string GROUP_ID = "Main";
    private const string CHANNEL_ID = "Notification";

    private static NotifsManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (!PlayerPrefs.HasKey("NotificationsInitialized"))
        {
            var group = new AndroidNotificationChannelGroup()
            {
                Id = GROUP_ID,
                Name = "Main notifications"
            };

            AndroidNotificationCenter.RegisterNotificationChannelGroup(group);

            var channel = new AndroidNotificationChannel()
            {
                Id = CHANNEL_ID,
                Name = "All notifications",
                Importance = Importance.Default,
                Description = "all of the game's notifications",
                Group = GROUP_ID
            };

            AndroidNotificationCenter.RegisterNotificationChannel(channel);

            StartCoroutine(RequestPermission());

            PlayerPrefs.SetString("NotificationsInitialized", "si");
            PlayerPrefs.Save();
        }

        else ScheduleNotifications();
    }

    private IEnumerator RequestPermission()
    {
        var request = new PermissionRequest();

        while (request.Status == PermissionStatus.RequestPending)
            yield return new WaitForEndOfFrame();

        ScheduleNotifications();
    }

    private void ScheduleNotifications()
    {
        AndroidNotificationCenter.CancelAllScheduledNotifications();

        var notification01 = new AndroidNotification();

        notification01.Title = "Catalina Orgniani :)";
        notification01.Text = "Hey! You haven't played in 10 minutes! Come back!";
        notification01.FireTime = System.DateTime.Now.AddMinutes(10);

        AndroidNotificationCenter.SendNotification(notification01, CHANNEL_ID);
    }
}
#endif
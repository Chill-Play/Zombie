using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.Android;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    const string CHANNEL_ID = "retention_notifications";
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        var channel = new AndroidNotificationChannel()
        {
            Id = "retention_notifications",
            Name = "Survivors Channel",
            Importance = Importance.Default,
            Description = "Survivors notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);


        var notification = new AndroidNotification();
        notification.Title = "Your country is under attack!";
        notification.Text = "Grab survivors! Rebuild the base during the zombie apocalypse!";
        notification.FireTime = System.DateTime.Now.AddHours(23);

        AndroidNotificationCenter.SendNotification(notification, CHANNEL_ID);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

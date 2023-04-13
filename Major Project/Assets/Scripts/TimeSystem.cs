using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;
using UnityEngine;
using NaughtyAttributes;

public class TimeSystem : MonoBehaviour
{
    public static TimeSystem Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public TimeData timeData = new TimeData();

    private int counter = 0;

    private const string FMT = "O";

    public Dictionary<DateTime, HoursArray> GameTimeDict = new Dictionary<DateTime, HoursArray>();

    public int SecondsPlayedToday
    {
        get
        {
            DateTime today = DateTime.Today;
            int seconds = 0;
            if (GameTimeDict.ContainsKey(today))
            {
                for (int i = 0; i < GameTimeDict[today].hours.Length; i++)
                {
                    seconds += GameTimeDict[today].hours[i];
                }
            }
            return seconds;
        }
    }

    public int HoursPlayedToday
    {
        get
        {
            return SecondsPlayedToday / 3600;
        }
    }

    public int MinutesPlayedToday
    {
        get
        {
            return (SecondsPlayedToday % 3600) / 60;
        }
    }

    public int SecondsPlayedThisWeek
    {
        get
        {
            int seconds = 0;
            for (int i = 0; i < 7; i++)
            {
                DateTime day = DateTime.Today.AddDays(-i);
                if (GameTimeDict.ContainsKey(day))
                {
                    for (int j = 0; j < GameTimeDict[day].hours.Length; j++)
                    {
                        seconds += GameTimeDict[day].hours[j];
                    }
                }
            }
            return seconds;
        }
    }

    public int HoursPlayedThisWeek
    {
        get
        {
            return SecondsPlayedThisWeek / 3600;
        }
    }

    public int MinutesPlayedThisWeek
    {
        get
        {
            return (SecondsPlayedThisWeek % 3600) / 60;
        }
    }

    public int SecondsPlayedThisMonth
    {
        get
        {
            int seconds = 0;
            for (int i = 0; i < DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month); i++)
            {
                DateTime day = DateTime.Today.AddDays(-i);
                if (GameTimeDict.ContainsKey(day))
                {
                    for (int j = 0; j < GameTimeDict[day].hours.Length; j++)
                    {
                        seconds += GameTimeDict[day].hours[j];
                    }
                }
            }
            return seconds;
        }
    }

    public int HoursPlayedThisMonth
    {
        get
        {
            return SecondsPlayedThisMonth / 3600;
        }
    }

    public int MinutesPlayedThisMonth
    {
        get
        {
            return (SecondsPlayedThisMonth % 3600) / 60;
        }
    }

    public int SecondsPlayedAllTime
    {
        get
        {
            int seconds = 0;
            foreach (var day in GameTimeDict)
            {
                for (int i = 0; i < day.Value.hours.Length; i++)
                {
                    seconds += day.Value.hours[i];
                }
            }
            return seconds;
        }
    }

    public int HoursPlayedAllTime
    {
        get
        {
            return SecondsPlayedAllTime / 3600;
        }
    }

    public int MinutesPlayedAllTime
    {
        get
        {
            return (SecondsPlayedAllTime % 3600) / 60;
        }
    }

    private void Start()
    {
        Load();
        StartCoroutine(TimeLoop());
    }

    IEnumerator TimeLoop()
    {
        yield return new WaitForSecondsRealtime(1f);

        counter++;

        // Calculate the current day
        DateTime today = DateTime.Today;

        // If the day is not in the dictionary, add it
        if (!GameTimeDict.ContainsKey(today))
        {
            GameTimeDict.Add(today, new HoursArray(new int[24]));
            print("Added " + today + " to the dictionary");
            print(GameTimeDict[today]);
        }

        print(GameTimeDict[today].hours);
        print(DateTime.Now.Hour);

        // Increment the current hour by one second
        GameTimeDict[today].hours[DateTime.Now.Hour]++;

        // Update the time data
        timeData.Days = new List<string>(GameTimeDict.Keys.Select(x => x.ToString(FMT)));
        timeData.Hours = new List<HoursArray>(GameTimeDict.Values);

        // Save the time data
        if (counter % 5 == 0)
        {
            counter = 0;
            Save();
            SaveSystem.Instance.Save();
        }

        // Start the loop again
        StartCoroutine(TimeLoop());
    }

    private void Save()
    {
        string json = JsonUtility.ToJson(timeData);
        File.WriteAllText(Application.persistentDataPath + "/time.json", json);
    }

    private void Load()
    {
        if(!File.Exists(Application.persistentDataPath + "/time.json"))
        {
            ClearAll();
            return;
        }

        string json = File.ReadAllText(Application.persistentDataPath + "/time.json");
        timeData = JsonUtility.FromJson<TimeData>(json);

        // Convert days from the data to DateTime objects, then to a list
        List<DateTime> days = timeData.Days.Select(x => DateTime.ParseExact(x, FMT, CultureInfo.InvariantCulture)).ToList();

        // Zip the list of days with the list of hours, then convert the result to a dictionary
        GameTimeDict = days.Zip(timeData.Hours, (k, v) => new { k, v }).ToDictionary(x => x.k, x => x.v);
    }

    [Button]
    public void PrintGameTimeDict()
    {
        foreach (var day in GameTimeDict)
        {
            Debug.Log("Day: " + day.Key);
            for (int i = 0; i < day.Value.hours.Length; i++)
            {
                Debug.Log("Hour: " + i + " - " + day.Value.hours[i]);
            }
        }
    }

    [Button]
    public void PrintToday()
    {
        DateTime today = DateTime.Today;
        Debug.Log("Today: " + today);
    }

    [Button]
    public void PrintCurrentHour()
    {
        Debug.Log("Current Hour: " + DateTime.Now.Hour);
    }

    [Button]
    public void DeleteTimeData()
    {
        var file = Application.persistentDataPath + "/time.json";

        if (File.Exists(file))
        {
            File.Delete(file);
            Load();
        }
    }

    private void ClearAll()
    {
        GameTimeDict.Clear();
        timeData.Days.Clear();
        timeData.Hours.Clear();
    }

    [Button]
    public void AddFakeDataPastWeek()
    {
        DeleteTimeData();

        for (int i = 0; i < 7; i++)
        {
            DateTime day = DateTime.Today.AddDays(-i);
            AddFakeData(day);
        }
    }

    [Button]
    public void AddFakeDataPastMonth()
    {
        DeleteTimeData();

        for (int i = 0; i < DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month); i++)
        {
            DateTime day = DateTime.Today.AddDays(-i);
            AddFakeData(day);
        }
    }

    public void AddFakeData(DateTime day)
    {
        if (GameTimeDict.ContainsKey(day)) return;

        GameTimeDict.Add(day, new HoursArray(new int[24]));
        for (int i = 0; i < 24; i++)
        {
            GameTimeDict[day].hours[i] = UnityEngine.Random.Range(0, 3600);
        }
    }
}

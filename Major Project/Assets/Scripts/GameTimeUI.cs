using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class GameTimeUI : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerControls playerControls;

    private InputAction gameTimeEnableAction;
    private InputAction gameTimeDisableAction;
    private InputAction moveGameTimeSelectionAction;

    private bool menuOpen = false;

    private float gameTimeAnimationTime = 0.5f;

    [SerializeField] private GameObject gameTimeObject;
    [SerializeField] private TextMeshProUGUI todayTimeText;
    [SerializeField] private TextMeshProUGUI thisWeekTimeText;
    [SerializeField] private TextMeshProUGUI thisMonthTimeText;
    [SerializeField] private TextMeshProUGUI allTimeText;

    [SerializeField] private TextMeshProUGUI weekChartYAxisText;
    [SerializeField] private TextMeshProUGUI[] weekChartXAxisTexts;
    [SerializeField] private GameObject[] weekChartXAxisTextsBackgrounds;
    [SerializeField] private RectTransform[] weekChartBars;

    [SerializeField] private TextMeshProUGUI dayChartYAxisText;
    [SerializeField] private RectTransform[] dayChartBars;

    private List<DateTime> currentlyViewedDates = new List<DateTime>();
    private int dayIndex = 6;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GRefs.Instance.PlayerInput;
        playerControls = GRefs.Instance.PlayerControls;

        gameTimeEnableAction = playerInput.actions[playerControls.Player.OpenTimeMenu.name];
        gameTimeEnableAction.performed += GameTimeToggle;

        gameTimeDisableAction = playerInput.actions[playerControls.TimeUI.CloseTimeMenu.name];
        gameTimeDisableAction.performed += GameTimeToggle;

        moveGameTimeSelectionAction = playerInput.actions[playerControls.TimeUI.MoveTimeSelection.name];
        moveGameTimeSelectionAction.performed += MoveTimeSelection;
    }

    private void OnDisable()
    {
        gameTimeEnableAction.performed -= GameTimeToggle;
        gameTimeDisableAction.performed -= GameTimeToggle;
        moveGameTimeSelectionAction.performed -= MoveTimeSelection;
    }

    private void GameTimeToggle(InputAction.CallbackContext context)
    {
        if (menuOpen)
        {
            menuOpen = false;
            AnimateGameTimeMenuOut();
        }
        else
        {
            menuOpen = true;
            playerInput.SwitchCurrentActionMap(GRefs.Instance.TimeUIActionMap);
            // Update UI
            UpdateTimeTexts();
            DateTime start = DateTime.Today - TimeSpan.FromDays(6);
            currentlyViewedDates = Enumerable.Range(0, 7).Select(x => start + TimeSpan.FromDays(x)).ToList();
            UpdateBothCharts(currentlyViewedDates);
            AnimateGameTimeMenuIn();
        }
    }

    private void AnimateGameTimeMenuIn()
    {
        LeanTween.moveLocalY(gameTimeObject, 0, gameTimeAnimationTime).setEaseOutCirc();
        LeanTween.scale(gameTimeObject, Vector3.one, gameTimeAnimationTime).setEaseOutBack();
    }

    private void AnimateGameTimeMenuOut()
    {
        LeanTween.moveLocalY(gameTimeObject, 1000, gameTimeAnimationTime).setEaseInCirc();
        LeanTween.scale(gameTimeObject, Vector3.zero, gameTimeAnimationTime).setEaseInBack().setOnComplete(() => playerInput.SwitchCurrentActionMap(GRefs.Instance.PlayerActionMap));
    }

    private void MoveTimeSelection(InputAction.CallbackContext context)
    {
        float input = context.ReadValue<float>();

        // If the input is negative and the index is not at the start of the list
        if (input < 0 && dayIndex > 0)
        {
            // Move the index back
            dayIndex--;
        }
        else if (input > 0 && dayIndex < currentlyViewedDates.Count - 1)
        {
            // Move the index forward
            dayIndex++;
        }
        else if (input != 0 && (dayIndex == 0 || dayIndex == currentlyViewedDates.Count - 1))
        {
            // Prevent the index from going into the future
            if (input > 0 && currentlyViewedDates[currentlyViewedDates.Count - 1] == DateTime.Today)
            {
                UpdateBothCharts(currentlyViewedDates);
                return;
            }

            // Remove the first or last date from the list
            currentlyViewedDates.RemoveAt(input < 0 ? currentlyViewedDates.Count - 1 : 0);

            // Add a new date to the start or end of the list
            currentlyViewedDates.Insert(input < 0 ? 0 : currentlyViewedDates.Count, currentlyViewedDates[input < 0 ? 0 : currentlyViewedDates.Count - 1] + TimeSpan.FromDays(input < 0 ? -1 : 1));
        }

        // Update the charts
        UpdateBothCharts(currentlyViewedDates);

        // Update the time texts
        UpdateTimeTexts();
    }

    private void UpdateTimeTexts()
    {
        todayTimeText.text = $"Today: <color=#9B9FFE>{TimeSystem.Instance.HoursPlayedToday}h {TimeSystem.Instance.MinutesPlayedToday}m</color>";
        thisWeekTimeText.text = $"Week: <color=#9B9FFE>{TimeSystem.Instance.HoursPlayedThisWeek}h {TimeSystem.Instance.MinutesPlayedThisWeek}m</color>";
        thisMonthTimeText.text = $"Month: <color=#9B9FFE>{TimeSystem.Instance.HoursPlayedThisMonth}h {TimeSystem.Instance.MinutesPlayedThisMonth}m</color>";
        allTimeText.text = $"All Time: <color=#9B9FFE>{TimeSystem.Instance.HoursPlayedAllTime}h {TimeSystem.Instance.MinutesPlayedAllTime}m</color>";
    }

    private void UpdateBothCharts(List<DateTime> dates)
    {
        UpdateWeekChart(dates);
        UpdateDayChart(dates[dayIndex]);
    }

    private void UpdateWeekChart(List<DateTime> dates)
    {
        // Find all the dates that are in the dictionary
        List<DateTime> datesInDict = dates.Where(x => TimeSystem.Instance.GameTimeDict.ContainsKey(x)).ToList();

        // Find the greatest value from the dates in the dictionary and round it up to the nearest hour
        int max = 0;
        if (datesInDict.Count > 0)
            max = Mathf.CeilToInt(datesInDict.Select(x => TimeSystem.Instance.GameTimeDict[x].hours.Sum()).Max() / 3600f) * 3600;

        // Set the y axis text to the max value
        weekChartYAxisText.text = $"{max / 3600}h";

        // For each day, find the percentage of the max value
        for (int i = 0; i < dates.Count; i++)
        {
            float percentage = 0;
            if (TimeSystem.Instance.GameTimeDict.ContainsKey(dates[i]))
                percentage = (float)TimeSystem.Instance.GameTimeDict[dates[i]].hours.Sum() / (float)max;

            // Set the height of the bar to the percentage
            weekChartBars[i].sizeDelta = new Vector2(weekChartBars[i].sizeDelta.x, percentage * 262);

            // Update the date text beneath the bar
            weekChartXAxisTexts[i].text = dates[i].ToString("dd/MM");

            // If the day index is the same as i, enable the text highlight, otherwise disable it
            weekChartXAxisTextsBackgrounds[i].SetActive(dayIndex == i);
        }
    }

    private void UpdateDayChart(DateTime date)
    {
        // Find the greatest value in the day rounded to the nearest 10 minutes
        int max = 0;
        if (TimeSystem.Instance.GameTimeDict.ContainsKey(date))
            max = Mathf.CeilToInt(TimeSystem.Instance.GameTimeDict[date].hours.Max() / 600f) * 600;

        // Set the y axis text to the max value
        dayChartYAxisText.text = $"{max/60}m";

        // If the date doesn't exist in the dictionary, set the day chart bars to 0
        if (!TimeSystem.Instance.GameTimeDict.ContainsKey(date))
        {
            for (int i = 0; i < dayChartBars.Length; i++)
            {
                dayChartBars[i].sizeDelta = new Vector2(dayChartBars[i].sizeDelta.x, 0);
            }
            return;
        }

        // For each hour, find the percentage of the max value
        for (int i = 0; i < TimeSystem.Instance.GameTimeDict[date].hours.Length; i++)
        {
            float percentage = 0;
            if (TimeSystem.Instance.GameTimeDict.ContainsKey(date))
                percentage = (float)TimeSystem.Instance.GameTimeDict[date].hours[i] / (float)max;

            // Set the height of the bar to the percentage
            dayChartBars[i].sizeDelta = new Vector2(dayChartBars[i].sizeDelta.x, percentage * 112);
        }
    }
}

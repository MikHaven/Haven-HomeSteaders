using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HomeStead
{
    /// <summary>
    /// Class to keep track of the Weeks
    /// Every week something can happen.
    /// Meant as a Time (Singleton) tracker
    /// Akin to TickTimer and GameTime in Haven Engines.
    /// </summary>
    public class Week : MonoBehaviour
    {
        public static Week instance = null;
        void Awake()
        {
            instance = this;
            UpdateUI();
        }

        public int CurrentWeek = 0;

        public static System.Action OnWeekTicked = null;

        public TMPro.TextMeshProUGUI weekDisplay;

        int weekChange = 0;
        int weekModifier = 1;

        public static bool isUpdating = false;

        /// <summary>
        /// Given test UI we can move the week as we wish
        /// BUT server, timed events will occur in the future
        /// </summary>
        void Update()
        {
            weekChange = 0;
            if (Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                weekChange++;
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                weekModifier = 10;
            }
            else
            {
                weekModifier = 1;
            }

            if (weekChange != 0)
            {
                StartCoroutine(WeekChanges(weekChange * weekModifier));
            }
        }

        /// <summary>
        /// We need to run a Tick loop to notifiy all our data
        /// We have NO idea how long this will take.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        IEnumerator WeekChanges(int value)
        {
            isUpdating = true;
            for (int i = 0; i < value; i++)
            {
                CurrentWeek += 1;
                UpdateUI();
                OnWeekTicked?.Invoke();
                yield return new WaitForSeconds(0.15f);
            }
            isUpdating = false;
        }

        /// <summary>
        /// Once we have made changes to the Data.
        /// We report it to the UI so the User/Player can see it.
        /// </summary>
        void UpdateUI()
        {
            weekDisplay.text = "Week: " + CurrentWeek;
        }
    }
}
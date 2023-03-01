using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UIScripts
{
    public class IndividualBadge : MonoBehaviour
    {
        public TextMeshProUGUI headerText;
        public TextMeshProUGUI bodyText;
        public TextMeshProUGUI footerText;

        public void LoadPreviousScene()
        {
            SceneManager.LoadScene("BadgeGallery");
        }

        private void Start()
        {
            var badgeName = BadgeInfo.CrossSceneInformation;
            if (badgeName == null) return;
            
            var badgeNumber = badgeName.Substring(badgeName.Length - 2);
            var badgeNumberInt = Convert.ToInt32(badgeNumber);
            var lastChar = (badgeNumberInt % 10) + 1;
            var firstChar = badgeNumberInt / 10;
            
            if (firstChar == 0) {
                headerText.text = "Badge " + lastChar;
            }
            else {
                headerText.text = "Badge " + firstChar + lastChar;
            }
        }
    }
}

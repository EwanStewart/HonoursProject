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
            var badgeName = BadgeInfo.CrossSceneInformation; // Get the badge name from the cross scene information
            if (badgeName == null) return;
            
            var badgeNumber = badgeName.Substring(badgeName.Length - 2); // Get the badge number from the badge name
            var badgeNumberInt = Convert.ToInt32(badgeNumber);
            var lastChar = (badgeNumberInt % 10) + 1; // Get the last character of the badge number
            var firstChar = badgeNumberInt / 10;      // Get the first character of the badge number
            
            if (firstChar == 0) { // If the badge number is less than 10, don't display the first character
                headerText.text = "Badge " + lastChar;
            } else { // If the badge number is greater than 10, display the first character
                headerText.text = "Badge " + firstChar + lastChar;
            }
            
            var list = BadgeInfo.LoadBadgeData();
            foreach (var badge in list) 
            {
                badge.Name = badge.Name.Replace("\"", "");              // Remove the quotation marks from the name
                badge.Description = badge.Description.Replace("\"", ""); // Remove the quotation marks from the description
                badge.Name = badge.Name.ToLower();                                      // Make the name lowercase
                if (badge.Name != badgeName) continue;                                // If the badge name doesn't match the badge name from the cross scene information, continue
                bodyText.text = badge.Description;                                   // Set the body text to the description of the badge
            }
        }
    }
}

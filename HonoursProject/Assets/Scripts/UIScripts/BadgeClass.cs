using System.Collections.Generic;
using Defective.JSON;
using UnityEngine;

namespace UIScripts
{
    public abstract class BadgeInfo
    {
        public class Badge
        {
            public string Name;
            public string Description;

            public Badge(string name, string description)
            {
                this.Name = name;
                this.Description = description;
            }
        }
        public static string CrossSceneInformation { get; set; }
        public static List<Badge> LoadBadgeData()
        {
            List<Badge> badges = new List<Badge>();
            TextAsset jsonFile = Resources.Load<TextAsset>("badgeInfo");
            string jsonString = jsonFile.text;
            JSONObject json = new JSONObject(jsonString);
            foreach(JSONObject badgeJson in json.list)
            {
                var badgeName = badgeJson.GetField("name").ToString();
                var badgeDescription = badgeJson.GetField("description").ToString();
                var badge = new Badge(badgeName, badgeDescription);
                badges.Add(badge);
            }
            return badges;
        }
    }
}


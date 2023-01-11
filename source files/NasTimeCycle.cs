using MCGalaxy;
using MCGalaxy.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MCGalaxy.Events;
using MCGalaxy.Events.ServerEvents;
using MCGalaxy.Network;
namespace NotAwesomeSurvival
{
    public partial class NasTimeCycle {

        // Vars
        public static float globalCurrentTime;
        public static DayCycles globalCurrentDayCycle;
        public static float staticMaxTime;
        public static int gameday = 0; // just defining it here, starting at 0. then adding on to it :)
        public static string TimeFilePath = Nas.CoreSavePath + "time.json";
        static JsonSerializer serializer = new JsonSerializer();
		public static Scheduler weatherScheduler;
		public static SchedulerTask task;
        public static string globalSkyColor; // self explanatory
        public static string globalCloudColor;
        public static string globalSunColor;
        public static string globalShadowColor;

        // Cycle Settings
        public static DayCycles dayCycle = DayCycles.Sunrise; // default cycle
        public static int cycleCurrentTime = 0; // current cycle time (must be zero to start)
        public static int cycleMaxTime = 14400; // duration a whole day
        public static int hourMinutes = 600; //seconds in an hour

        public enum DayCycles // Enum with day and night cycles
        {
            Sunrise, Day, Sunset, Night, Midnight
        }



        public static void Setup()
        {
        	if (weatherScheduler == null) weatherScheduler = new Scheduler("WeatherScheduler");
        	task = weatherScheduler.QueueRepeat(Update, null, new TimeSpan(0, 0, 7));
            dayCycle = DayCycles.Sunrise; // start with sunrise state
            // Static variables to keep time after switching scenes

            if (!File.Exists(TimeFilePath))
            {
                File.Create(TimeFilePath).Dispose();
                Logger.Log(LogType.Debug, "Created new json time file " + TimeFilePath + " !");
                using (StreamWriter sw = new StreamWriter(TimeFilePath)) // To help you better understand, this is the stream writer
                using (JsonWriter writer = new JsonTextWriter(sw)) // this is the json writer that will help me to serialize and deserialize items in the file
                {
                    serializer.Serialize(writer, cyc);
                }
            }

            // cycleCurrentTime = serializer.Deserialize(reader)
            string jsonString = File.ReadAllText(TimeFilePath);
            NasTimeCycle ntc = JsonConvert.DeserializeObject<NasTimeCycle>(jsonString);
            dayCycle = ntc.cycle;
            gameday = ntc.day;
            cycleCurrentTime = ntc.minutes;

            gameday = cyc.day;
            cycleCurrentTime = cyc.minutes;
            dayCycle = cyc.cycle;
            
            staticMaxTime = cycleMaxTime;
        }

        public static void TakeDown()
        {
            Server.MainScheduler.Cancel(task);
        }

        public static void Update(SchedulerTask task) // this gets executed each time a second has passed.
        {
            // Update cycle time
            cycleCurrentTime += 6 * 7;

            // Static variables to keep time after switching scenes
            globalCurrentTime = cycleCurrentTime;
            globalCurrentDayCycle = dayCycle;

            // Check if cycle time reach cycle duration time
            if (cycleCurrentTime >= cycleMaxTime)
            {
                cycleCurrentTime = 0; // back to 0 (restarting cycle time)
                gameday += 1; // one more in-game day just passed :p
                dayCycle++; // change cycle state
            }

            //when to change cycles
            if (cycleCurrentTime >= 7 * hourMinutes & cycleCurrentTime < 8 *hourMinutes) {dayCycle = DayCycles.Sunrise;} // 7am
            if (cycleCurrentTime >= 8 * hourMinutes & cycleCurrentTime < 19*hourMinutes) {dayCycle = DayCycles.Day;} // 8am
            if (cycleCurrentTime >= 19 * hourMinutes & cycleCurrentTime < 20*hourMinutes) {dayCycle = DayCycles.Sunset;} // 6pm
            if (cycleCurrentTime >= 20 * hourMinutes & cycleCurrentTime < 24*hourMinutes) {dayCycle = DayCycles.Night;} // 8pm
            if (cycleCurrentTime == 24 * hourMinutes | cycleCurrentTime == 0 | cycleCurrentTime < 7*hourMinutes) {dayCycle = DayCycles.Midnight;} // 0 am
            // Sunrise state (you can do a lot of stuff based on every cycle state, like enable monster spawning only when dark)
            if (dayCycle == DayCycles.Sunrise)
            {
                globalCloudColor = "#ff8c00"; // Dark Orange
                globalSkyColor = "#FFA500"; // Orange
                globalSunColor = "#a9a9a9"; // Dark Gray
                globalShadowColor = "#828282";
            }

            // Mid Day state
            if (dayCycle == DayCycles.Day)
            {
                globalCloudColor = "#ffffff"; // white
                globalSkyColor = "#ADD8E6"; // light blue
                globalSunColor = "#ffffff"; // white
                globalShadowColor = "#9B9B9B";
            }

            // Sunset state
            if (dayCycle == DayCycles.Sunset)
            {
                globalCloudColor = "#cf5c00"; // Dark Orange
                globalSkyColor = "#FFB500"; // Orange
                globalSunColor = "#a9a9a9"; // Dark Gray
                globalShadowColor = "#828282";
            }

            // Night state
            if (dayCycle == DayCycles.Night)
            {
                globalCloudColor = "#808080"; // grey
                globalSkyColor = "#404040"; // darko grey
                globalSunColor = "#808080"; // grey
                globalShadowColor = "#595959";
            }

            // Midnight state
            if (dayCycle == DayCycles.Midnight)
            {
                globalCloudColor = "#404040"; // darko grey
                globalSkyColor = "#000000"; // black
                globalSunColor = "#404040"; // darko grey
                globalShadowColor = "#494949";
            }
			//globalCloudColor = "#ffffff"; // white
            UpdateEnvSettings(globalCloudColor, globalSkyColor, globalSunColor, globalShadowColor);
            StoreTimeData(gameday, cycleCurrentTime, dayCycle);
        }

        static void UpdateEnvSettings(string cloud, string sky, string sun, string shadow)
        {
             
           
             foreach (Level lvl in LevelInfo.Loaded.Items)
            {
             	if (NasLevel.Get(lvl.name).biome < 0) {continue;}
             	//Logger.Log(LogType.Debug, "updating " + lvl.name);
                lvl.Config.LightColor = sun; // Sun Colour
                lvl.Config.CloudColor = cloud; // Cloud Colour
                lvl.Config.SkyColor = sky; // Sky
                lvl.Config.ShadowColor = shadow; // Shadow
                lvl.SaveSettings(); // We save these settings after
             	}
             foreach (Player p in PlayerInfo.Online.Items) {
             	if (NasLevel.Get(p.level.name).biome < 0) {continue;}
             	p.SendCurrentEnv();
             }
             

        }

    }
}
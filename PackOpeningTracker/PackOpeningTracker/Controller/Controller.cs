using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace PackOpeningTracker
{
    public class Controller
    {
        private StatisticsData statisticsData;
        private Model.SettingsData settingsData;

        public Controller(UI mainUI)
        {
            statisticsData = new StatisticsData(this);
            settingsData = new Model.SettingsData();
        }

        public void collectData(bool Realtime, List<string> files)
        {
            if (Realtime)
                statisticsData.cardDigger();
            else
                statisticsData.loadFromFile(files);

            statisticsData.collectData();
        }

        public void writeToFile(string fileName)
        {
            statisticsData.statsToFile(fileName);
        }

        public void saveToFile(string fileName)
        {
            statisticsData.saveToFile(fileName);
        }

        public List<Card> getCardsFromPacks()
        {
            return statisticsData.cfp;
        }

        public List<int> getStats()
        {
            return statisticsData.Stats;
        }

        public float getAvgDustPerPack()
        {
            return statisticsData.AvgDustPerPack;
        }

        public List<Card>getBestPack()
        {
            return statisticsData.BestPack;
        }

        public string getHSPath()
        {
            return settingsData.HSPath;
        }

        public void setHSPath(string path)
        {
            settingsData.HSPath = path;
        }

        public bool getPatchState()
        {
            return settingsData.checkIfPatched();
        }

        public List<Card> getCardsFromPacksInOrder()
        {
            return statisticsData.cfpInOrder;
        }

        public List<int> getPacksPosition(string type)
        {
            switch (type)
            {
                case "Classic": return statisticsData.ClassicPositions;
                case "GVG": return statisticsData.GVGPositions;
                case "TGT": return statisticsData.TGTPositions;
            }

            return null;
        }
    }
}
